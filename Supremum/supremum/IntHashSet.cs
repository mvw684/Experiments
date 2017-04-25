using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace supremum {
    /// <summary>
    /// Generic Dictionary implementation were Key is an UInt32 
    /// (or at least castable to an UInt32 when creating specialized versions).
    /// This dictionary is NOT threadsafe.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    [Serializable()]
    public sealed class HashSetInt : IEnumerable, IEnumerable<int>, ICollection<int> {

        /// <summary>
        /// Nr of entries must be smaller then or equal too, <see cref="Entry" /> 
        /// </summary>
        private const int MaxEntries = Int32.MaxValue / 2;

        /// <summary>
        /// Dictionary Entries, 
        /// by using struct and an integer as pointer, being an index into an array
        /// of objects total performance will improve, data will be close 
        /// together in memory (one array) allocation will be relative cheap 
        /// since not each entry in/from the Buckets requires a seperately allocated object.
        /// </summary>
        /// <remarks>
        /// Usign next as indication for last and in use
        /// Assume 32 bit in an int.
        /// bits 29-0 indicate the index of next (as int)
        /// bit 31 indicates in use or on freelist (0 is on freelist)
        /// bit 30 indicates last (if last enry is obviously irrelevant)
        /// </remarks>
        private struct Entry {
            /// <summary>
            /// Bit indicating last (sign bit) so &lt; 0 is last
            /// </summary>
            internal const Int32 LastFlag = 1 << 31;

            /// <summary>
            /// Bit indicating Entry is in use, used for enumerations etc.
            /// </summary>
            internal const Int32 InUseFlag = 1 << 30;
            /// <summary>
            /// Retrieve index part of next
            /// </summary>
            internal const Int32 EntryMask = ~(LastFlag | InUseFlag);

            /// <summary>
            /// Index of next entry (bits 29-0), Last if &lt; 0,
            /// bit FreeFlag indicates whether the entry is on the free list,
            /// bit LastFlag indicates this is last in chain.
            /// </summary>
            internal int next;

            /// <summary>
            /// Key/value of entry
            /// </summary>
            internal int key;
        }

        /// <summary>
        /// Hashtable buckets, each bucket is a ptr (index) into the entries array
        /// </summary>
        private int[] buckets;

        /// <summary>
        /// The actual content of the hashtable.
        /// </summary>
        private Entry[] entries;

        /// <summary>
        /// All items in the entries array 0 to count are either in use,
        /// or on the freelist.
        /// Next the used bits indicated which entries are in use. 
        /// This allows for simple enumeration of the used entries from 0 to count.
        /// </summary>
        private int count;

        /// <summary>
        /// The first index of the freelist, (index into entries),
        /// Entry.Last if empty
        /// </summary>
        private int freeList;

        /// <summary>
        /// Count of items in the freelist.
        /// </summary>
        private int freeCount;

        /// <summary>
        /// Object to synchronize upon
        /// </summary>
        private Object syncRoot;

        /// <summary>
        /// Constructor with specified capacity, actual capacity will be a suitable higher prime
        /// </summary>
        public HashSetInt(int capacity) {
            Initialize(capacity);
        }

        
        #region IDictionary<UInt32,int> Members

        /// <summary>
        /// <see cref="IDictionary{UInt32,int}.Add"/>
        /// </summary>
        /// <remarks>
        /// This Dictionary implementation will not throw an exception when adding duplicate items.
        /// When adding duplicate items the old values will be overwritten.
        /// </remarks>
        public void Add(int key) {

            int bucket = ((int)(key & 0x7FFFFFFF)) % buckets.Length;

            // start with bucket with keys hash ( obviously keep it positive)
            int index = buckets[bucket];
            bool last = index < 0;
            while (!last) {
                if (entries[index].key == key) {
                    // already contained
                    return;
                }
                int next = entries[index].next;
                last = next < 0;
                index = next & Entry.EntryMask;
            }

            // not found, new entry to 'create'
            if (freeCount > 0) {
                // allocate from freelist
                // keeps data more local
                index = freeList;
                freeList = entries[index].next & Entry.EntryMask;
                freeCount--;
            } else {
                // maybe we have some to spare at the end
                if (count == entries.Length) {
                    // well lets create some spare entries.
                    Resize();
                    // new size recompute bucket
                    bucket = ((int)(key & 0x7FFFFFFF)) % buckets.Length;
                }
                // take the first free entrie at the end.
                index = count;
                count++;
            }

            // order below minimizes risk of errors on unsynchronized use,
            // it's a quite safe order, assuming 4 byte size assignments are atomic

            // chain curent in bucket to current and set in use flag
            entries[index].next = Entry.InUseFlag | buckets[bucket];
            // set current's values
            entries[index].key = key;
            // make current available in the Dictionary
            buckets[bucket] = index;
        }

        /// <summary>
        /// <see cref="IDictionary{UInt32,int}.ContainsKey"/>
        /// </summary>
        public bool Contains(Int32 key) {
            return FindEntry(key) >= 0;
        }

        /// <summary>
        /// <see cref="IDictionary{UInt32,int}.Remove"/>
        /// </summary>
        public bool Remove(int key) {
            int bucket = ((int)(key & 0x7FFFFFFF)) % buckets.Length;
            int previous = Entry.LastFlag;
            int index = buckets[bucket];
            bool last = index < 0;
            while (!last) {
                if (entries[index].key == key) {
                    // found !!
                    int next = entries[index].next;
                    if (next < 0) {
                        // last of chain
                        if (previous < 0) {
                            // no previous in chain
                            buckets[bucket] = Entry.LastFlag;
                        } else {
                            // previous became last
                            entries[previous].next = Entry.InUseFlag | Entry.LastFlag;
                        }
                    } else {
                        // not last of chain
                        if (previous < 0) {
                            buckets[bucket] = next & Entry.EntryMask;
                        } else {
                            entries[previous].next = (next & Entry.EntryMask) | Entry.InUseFlag;
                        }
                    }

                    // order below minimizes risk of errors on unsynchronized use,
                    // it's a quite safe order, assuming 4 byte size assignments are atomic
                    // see Add(UInt32 key) method
                    // 
                    // assure InUse bit is cleared.
                    entries[index].next = freeList;
                    entries[index].key = 0;
                    freeList = index;
                    freeCount++;
                    return true;
                }
                previous = index;
                index = entries[index].next;
                last = index < 0;
                index &= Entry.EntryMask;
            }

            return false;
        }

        /// <summary>
        /// <see cref="IDictionary{UInt32,int}.Values"/>
        /// </summary>
        public ICollection<int> Values {
            get {
                return new ValueCollection(this);
            }
        }

        #endregion

        #region ICollection<KeyValuePair<UInt32,int>> Members

        
        /// <summary>
        /// <see cref="ICollection{Object}.Clear"/>,
        /// <see cref="ICollection{Object}"/> containing <see cref="KeyValuePair{Uint32,int}"/>
        /// </summary>
        public void Clear() {
            if (count > 0) {
                for (int i = 0; i < buckets.Length; i++) {
                    // TODO: consider memset or ipp
                    buckets[i] = Entry.LastFlag;
                }
                Array.Clear(entries, 0, count);
                freeList = Entry.LastFlag;
                count = 0;
                freeCount = 0;
            }
        }

        
        /// <summary>
        /// Copies keys to provided list
        /// </summary>
        /// <returns>Number of actually copied items</returns>
        public int CopyTo(List<int> array, int index) {
            int count = this.count;
            Entry[] entries = this.entries;
            if (array.Capacity < index + Count) {
                array.Capacity = index + Count;
            }
            for (int i = 0; i < count; i++) {
                if ((entries[i].next & Entry.InUseFlag) != 0) {
                    array[index++] = entries[i].key;
                }
            }
            return Count;
        }

        /// <summary>
        /// Copies keys to provided array
        /// </summary>
        /// <returns>Number of actually copied items</returns>
        public void CopyTo(int[] array, int index) {
            if (Count + index > array.Length) {
                throw new ArgumentException("array to small");
            }
            int count = this.count;
            Entry[] entries = this.entries;
            for (int i = 0; i < count; i++) {
                if ((entries[i].next & Entry.InUseFlag) != 0) {
                    array[index++] = entries[i].key;
                }
            }
        }

        /// <summary>
        /// Copies keys to provided array
        /// </summary>
        /// <returns>actual number of copied keys</returns>
        public void CopyTo(object[] array, int index) {
            if (Count + index > array.Length) {
                throw new ArgumentException("array to small");
            }
            int count = this.count;
            Entry[] entries = this.entries;
            for (int i = 0; i < count; i++) {
                if ((entries[i].next & Entry.InUseFlag) != 0) {
                    array[index++] = entries[i].key;
                }
            }
        }

        /// <summary>
        /// <see cref="ICollection{Object}.Count"/>,
        /// <see cref="ICollection{Object}"/> containing <see cref="KeyValuePair{Uint32,int}"/>
        /// </summary>
        public int Count {
            get {
                return count - freeCount;
            }
        }

        /// <summary>
        /// <see cref="ICollection{Object}.IsReadOnly"/>,
        /// <see cref="ICollection{Object}"/> containing <see cref="KeyValuePair{Uint32,int}"/>
        /// </summary>
        public bool IsReadOnly {
            get {
                return false;
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<UInt32,int>> Members

        /// <summary>
        /// <see cref="IEnumerable{Object}.GetEnumerator"/>,
        /// <see cref="IEnumerable{Object}"/> enumerating over <see cref="KeyValuePair{Uint32,int}"/>
        /// </summary>
        public IEnumerator<int> GetEnumerator() {
            // Enumerated Generic Dictionary KeyValuePair
            return new Enumerator(this);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// <see cref="IEnumerable.GetEnumerator"/>,
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() {
            // Enumerated Generic Dictionary as Collection of KeyValuePair
            return new Enumerator(this);
        }

        #endregion

        /// <summary>
        /// <see cref="ICollection.CopyTo"/>
        /// array should be of type <see cref="KeyValuePair{UInt32, int}"/>,
        /// <see cref="DictionaryEntry"/> or  <see cref="Object"/>.
        /// </summary>
        public void CopyTo(Array array, int index) {
            if (Count + index > array.Length) {
                throw new ArgumentException("array to small");
            }

            int[] pairs = array as int[];
            if (pairs != null) {
                CopyTo(pairs, index);
            } else {
                object[] objects = array as object[];
                if (objects == null) {
                    throw new ArgumentException(
                        "Unexpected Array content type:" +
                        array.GetType().GetElementType().Name
                    );
                }

                try {
                    int count = this.count;
                    Entry[] entries = this.entries;
                    for (int i = 0; i < count; i++) {
                        if ((entries[i].next & Entry.InUseFlag) != 0) {
                            objects[index++] = entries[i].key;
                        }
                    }
                } catch (ArrayTypeMismatchException e) {
                    throw new ArgumentException("InvalidArrayType", e);
                }
            }
        }

        /// <summary>
        /// <see cref="ICollection.IsSynchronized"/>
        /// </summary>
        public bool IsSynchronized {
            get {
                return false;
            }
        }

        /// <summary>
        /// <see cref="ICollection.SyncRoot"/>
        /// </summary>
        public object SyncRoot {
            get {
                if (syncRoot == null) {
                    Interlocked.CompareExchange(ref syncRoot, new Object(), null);
                }
                return syncRoot;
            }
        }

        #region Helper methods

        private void Initialize(int capacity) {
            int size = Primes.GetNextBucketsSize(capacity);
            buckets = new int[size];
            for (int i = 0; i < buckets.Length; i++) {
                buckets[i] = Entry.LastFlag;
            }
            entries = new Entry[size];
            freeList = Entry.LastFlag;
            count = 0;
            freeCount = 0;
        }

        /// <summary>
        /// Resizes buckets, entries and used to a suitable larger size
        /// </summary>
        /// <remarks>
        /// When resizing freelist is empty so all items (0..count-1) are in use.
        /// </remarks>
        private void Resize() {
            int newSize = Primes.GetNextBucketsSize(2 * count);
            if ((newSize < 0) || (newSize > MaxEntries)) {
                throw new ArgumentOutOfRangeException(
                    "cannot increase size beyond " + MaxEntries
                );
            }
            int[] newBuckets = new int[newSize];
            for (int i = 0; i < newSize; i++) {
                newBuckets[i] = Entry.LastFlag;
            }
            Entry[] newEntries = new Entry[newSize];
            Array.Copy(entries, 0, newEntries, 0, count);

            // redistribute the entries over the new buckets.
            // note that the used bits do not need updating, 
            // all from 0 to count are in use, otherwise we would not be resizing.
            for (int index = 0; index < count; index++) {
                int bucket = ((int)(newEntries[index].key & 0x7FFFFFFF)) % newSize;
                newEntries[index].next = newBuckets[bucket] | Entry.InUseFlag;
                newBuckets[bucket] = index;
            }
            buckets = newBuckets;
            entries = newEntries;
        }

        /// <summary>
        /// Find index of entry with certain key.
        /// </summary>
        private int FindEntry(int key) {
            int index = buckets[(key & 0x7FFFFFFF) % buckets.Length];
            // bool last = index == Entry.LastFlag;

            // shadow instance variable to register
            Entry[] entries = this.entries;

            bool last = index < 0;
            while (!last) {
                if (entries[index].key == key) {
                    return index;
                }
                int next = entries[index].next;
                // last = (next & Entry.LastFlag) != 0;
                last = next < 0;
                index = next & Entry.EntryMask;
            }
            return Entry.LastFlag;
        }

        #endregion

        #region Helper classes

        private struct Enumerator : IEnumerator<int> {
            
            private HashSetInt dictionary;

            /// <summary>
            /// Initial value is 0, at end of enumerationset to -1
            /// </summary>
            private int index;
            private int current;
            
            internal Enumerator(HashSetInt dictionary) {
                this.dictionary = dictionary;
                index = 0;
                current = default(int);
            }

            #region IEnumerator<KeyValuePair<UInt32,int>> Members

            /// <summary>
            /// <see cref="IEnumerator{Object}.Current"/>
            /// </summary>
            public int Current {
                get {
                    return current;
                }
            }

            #endregion

            #region IDisposable Members

            /// <summary>
            /// <see cref="IDisposable.Dispose"/>
            /// </summary>
            public void Dispose() {
                dictionary = null;
                index = -1;
                current = default(int);
            }

            #endregion

            #region IEnumerator Members

            /// <summary>
            /// <see cref="IEnumerator.Current"/>
            /// </summary>
            object IEnumerator.Current {
                get {
                    return current;
                }
            }

            /// <summary>
            /// <see cref="IEnumerator.MoveNext"/>
            /// </summary>
            public bool MoveNext() {
                if (index >= 0) {
                    int count = dictionary.count;
                    Entry[] entries = dictionary.entries;
                    while (index < count) {
                        if ((entries[index].next & Entry.InUseFlag) != 0) {
                            current = dictionary.entries[index].key;
                            index++;
                            return true;
                        }
                        index++;
                    }
                    index = -1;
                    current = default(int);
                }
                return false;
            }

            /// <summary>
            /// <see cref="IEnumerator.Reset"/>
            /// </summary>
            public void Reset() {
                index = 0;
                current = default(int);
            }

            #endregion

        }
        
        [DebuggerDisplay("Count = {dictionary.Count}")]
        private struct ValueCollection : ICollection<int>, ICollection {
            private HashSetInt dictionary;

            internal ValueCollection(HashSetInt dictionary) {
                this.dictionary = dictionary;
            }

            #region ICollection<int> Members

            /// <summary>
            /// <see cref="ICollection{int}.Add"/>
            /// </summary>
            public void Add(int item) {
                throw new NotSupportedException("Add");
            }

            /// <summary>
            /// <see cref="ICollection{int}.Clear"/>
            /// </summary>
            public void Clear() {
                throw new NotSupportedException("Clear");
            }

            /// <summary>
            /// <see cref="ICollection{int}.Contains"/>
            /// </summary>
            public bool Contains(int item) {
                return dictionary.Contains(item);
            }

            /// <summary>
            /// <see cref="ICollection{int}.CopyTo"/>
            /// </summary>
            public void CopyTo(int[] array, int index) {
                dictionary.CopyTo(array, index);
            }

            /// <summary>
            /// <see cref="ICollection{int}.Count"/>
            /// </summary>
            public int Count {
                get {
                    return dictionary.Count;
                }
            }

            /// <summary>
            /// <see cref="ICollection{int}.IsReadOnly"/>
            /// </summary>
            public bool IsReadOnly {
                get {
                    return true;
                }
            }

            /// <summary>
            /// <see cref="ICollection{int}.Remove"/>
            /// </summary>
            public bool Remove(int item) {
                throw new NotSupportedException("Clear");
            }

            #endregion

            #region IEnumerable<int> Members

            /// <summary>
            /// <see cref="IEnumerable{int}.GetEnumerator"/>
            /// </summary>
            public IEnumerator<int> GetEnumerator() {
                return new Enumerator(dictionary);
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator() {
                return new Enumerator(dictionary);
            }

            #endregion

            #region ICollection Members

            void ICollection.CopyTo(Array array, int index) {

                int[] valueArray = array as int[];
                if (valueArray != null) {
                    dictionary.CopyTo(valueArray, index);
                } else {
                    object[] objectArray = array as object[];
                    if (objectArray != null) {
                        dictionary.CopyTo(objectArray, index);
                    } else {
                        throw new ArgumentException("Invalid Array Type " + array.GetType());
                    }
                }
            }

            int ICollection.Count {
                get {
                    return dictionary.Count;
                }
            }

            bool ICollection.IsSynchronized {
                get {
                    return false;
                }
            }

            object ICollection.SyncRoot {
                get {
                    return dictionary.SyncRoot;
                }
            }

            #endregion

            #region enumerator
            private struct Enumerator : IEnumerator<int>, IEnumerator {
                HashSetInt dictionary;
                private int index;
                private int current;


                internal Enumerator(HashSetInt dictionary) {
                    this.dictionary = dictionary;
                    index = 0;
                    current = default(int);
                }

                #region IEnumerator<int> Members

                /// <summary>
                /// <see cref="IEnumerator{int}.Current"/>
                /// </summary>
                public int Current {
                    get {
                        return current;
                    }
                }

                #endregion

                #region IDisposable Members

                /// <summary>
                /// <see cref="IDisposable.Dispose"/>
                /// </summary>
                public void Dispose() {
                    index = -1;
                    dictionary = null;
                    current = default(int);
                }

                #endregion

                #region IEnumerator Members

                /// <summary>
                /// <see cref="IEnumerator.Current"/>
                /// </summary>
                object IEnumerator.Current {
                    get {
                        if (index <= 0) {
                            throw new InvalidOperationException(
                                "Enumeration not started or at end"
                            );
                        }
                        return current;
                    }
                }

                /// <summary>
                /// <see cref="IEnumerator.MoveNext"/>
                /// </summary>
                public bool MoveNext() {
                    if (index >= 0) {
                        int count = dictionary.count;
                        Entry[] entries = dictionary.entries;
                        while (index < count) {

                            if ((entries[index].next & Entry.InUseFlag) != 0) {
                                current = entries[index].key;
                                index++;
                                return true;
                            }
                            index++;
                        }
                        index = -1;
                        current = default(int);
                    }
                    return false;
                }

                /// <summary>
                /// <see cref="IEnumerator.Reset"/>
                /// </summary>
                public void Reset() {
                    index = 0;
                    current = default(int);
                }

                #endregion
            }
            #endregion
        }
        #endregion
    }
}
