using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;


namespace supremum {
    /// <summary>
    /// Generic HashSet{int} implementation 
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    [Serializable()]
    public sealed class HashSetInt : ICollection<int> {
        private const int StackAllocThreshold = 100;
        private const int ShrinkThreshold = 3;
        private int[] m_buckets;
        private Slot[] m_slots;
        private int m_count;
        private int m_lastIndex;
        private int m_freeList;
        
        /// <summary>Gets the number of elements that are contained in a set.</summary>
        /// <returns>The number of elements that are contained in the set.</returns>
        public int Count {
            get {
                return m_count;
            }
        }

        bool ICollection<int>.IsReadOnly {
            get {
                return false;
            }
        }
        
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.HashSet`1" /> class that is empty and uses the specified equality comparer for the set type.</summary>
        public HashSetInt(int capacity) {
            m_lastIndex = 0;
            m_count = 0;
            m_freeList = -1;
            Initialize(capacity);
        }

        void ICollection<int>.Add(int item) {
            AddIfNotPresent(item);
        }

        /// <summary>Removes all elements from a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
        public void Clear() {
            if (m_lastIndex > 0) {
                Array.Clear(m_slots, 0, m_lastIndex);
                Array.Clear(m_buckets, 0, m_buckets.Length);
                m_lastIndex = 0;
                m_count = 0;
                m_freeList = -1;
            }
        }

        /// <summary>Determines whether a <see cref="T:System.Collections.Generic.HashSet`1" /> object contains the specified element.</summary>
        /// <param name="item">The element to locate in the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
        /// <returns>true if the <see cref="T:System.Collections.Generic.HashSet`1" /> object contains the specified element; otherwise, false.</returns>
        public bool Contains(int item) {
            if (m_buckets != null) {
                for (int index = m_buckets[item % m_buckets.Length] - 1; index >= 0; index = m_slots[index].next) {
                    if (m_slots[index].value == item)
                        return true;
                }
            }
            return false;
        }

        /// <summary>Copies the elements of a <see cref="T:System.Collections.Generic.HashSet`1" /> object to an array, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.HashSet`1" /> object. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex" /> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="arrayIndex" /> is greater than the length of the destination <paramref name="array" />.</exception>
        public void CopyTo(int[] array, int arrayIndex) {
            CopyTo(array, arrayIndex, m_count);
        }

        /// <summary>Removes the specified element from a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>true if the element is successfully found and removed; otherwise, false.  method returns false if <paramref name="item" /> is not found in the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</returns>
        public bool Remove(int item) {
            if (m_buckets != null) {
                int index1 = item % m_buckets.Length;
                int index2 = -1;
                for (int index3 = m_buckets[index1] - 1; index3 >= 0; index3 = m_slots[index3].next) {
                    if (m_slots[index3].value == item) {
                        if (index2 < 0)
                            m_buckets[index1] = m_slots[index3].next + 1;
                        else
                            m_slots[index2].next = m_slots[index3].next;
                        m_slots[index3].value = -1;
                        m_slots[index3].next = m_freeList;
                        m_count = m_count - 1;
                        if (m_count == 0) {
                            m_lastIndex = 0;
                            m_freeList = -1;
                        } else
                            m_freeList = index3;
                        return true;
                    }
                    index2 = index3;
                }
            }
            return false;
        }

        /// <summary>Returns an enumerator that iterates through a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
        /// <returns>A <see cref="T:System.Collections.Generic.HashSet`1.Enumerator" /> object for the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</returns>
        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator() {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return new Enumerator(this);
        }

        /// <summary>Adds the specified element to a set.</summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns>true if the element is added to the <see cref="T:System.Collections.Generic.HashSet`1" /> object; false if the element is already present.</returns>
        public bool Add(int item) {
            return AddIfNotPresent(item);
        }

        /// <summary>Modifies the current <see cref="T:System.Collections.Generic.HashSet`1" /> object to contain all elements that are present in itself, the specified collection, or both.</summary>
        /// <param name="other">The collection to compare to the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is null.</exception>
        public void UnionWith(IEnumerable<int> other) {
            if (other == null)
                throw new ArgumentNullException("other");
            foreach (int obj in other)
                AddIfNotPresent(obj);
        }

        /// <summary>Removes all elements in the specified collection from the current <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
        /// <param name="other">The collection of items to remove from the <see cref="T:System.Collections.Generic.HashSet`1" /> object.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="other" /> is null.</exception>
        public void ExceptWith(IEnumerable<int> other) {
            if (other == null)
                throw new ArgumentNullException("other");
            if (m_count == 0)
                return;
            if (other == this) {
                Clear();
            } else {
                foreach (int obj in other)
                    Remove(obj);
            }
        }

        /// <summary>Copies the elements of a <see cref="T:System.Collections.Generic.HashSet`1" /> object to an array.</summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.HashSet`1" /> object. The array must have zero-based indexing.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array" /> is null.</exception>
        public void CopyTo(int[] array) {
            CopyTo(array, 0, m_count);
        }

        /// <summary>Copies the specified number of elements of a <see cref="T:System.Collections.Generic.HashSet`1" /> object to an array, starting at the specified array index.</summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the <see cref="T:System.Collections.Generic.HashSet`1" /> object. The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <param name="count">The number of elements to copy to <paramref name="array" />.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="arrayIndex" /> is less than 0.-or-<paramref name="count" /> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">
        /// <paramref name="arrayIndex" /> is greater than the length of the destination <paramref name="array" />.-or-<paramref name="count" /> is greater than the available space from the <paramref name="index" /> to the end of the destination <paramref name="array" />.</exception>
        public void CopyTo(int[] array, int arrayIndex, int count) {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count");
            if (arrayIndex > array.Length || count > array.Length - arrayIndex)
                throw new ArgumentException("array too small");
            int num = 0;
            for (int index = 0; index < m_lastIndex && num < count; ++index) {
                if (m_slots[index].value >= 0) {
                    array[arrayIndex + num] = m_slots[index].value;
                    ++num;
                }
            }
        }

        /// <summary>Removes all elements that match the conditions defined by the specified predicate from a <see cref="T:System.Collections.Generic.HashSet`1" /> collection.</summary>
        /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the elements to remove.</param>
        /// <returns>The number of elements that were removed from the <see cref="T:System.Collections.Generic.HashSet`1" /> collection.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="match" /> is null.</exception>
        public int RemoveWhere(Predicate<int> match) {
            if (match == null)
                throw new ArgumentNullException("match");
            int num = 0;
            for (int index = 0; index < m_lastIndex; ++index) {
                if (m_slots[index].value >= 0) {
                    int obj = m_slots[index].value;
                    if (match(obj) && Remove(obj))
                        ++num;
                }
            }
            return num;
        }

        /// <summary>Sets the capacity of a <see cref="T:System.Collections.Generic.HashSet`1" /> object to the actual number of elements it contains, rounded up to a nearby, implementation-specific value.</summary>
        public void TrimExcess() {
            if (m_count == 0) {
                m_buckets = null;
                m_slots = null;
            } else {
                int prime = Primes.GetNextBucketsSize(m_count);
                Slot[] slotArray = new Slot[prime];
                int[] numArray = new int[prime];
                int index1 = 0;
                for (int index2 = 0; index2 < m_lastIndex; ++index2) {
                    if (m_slots[index2].value >= 0) {
                        slotArray[index1] = m_slots[index2];
                        int index3 = slotArray[index1].value % prime;
                        slotArray[index1].next = numArray[index3] - 1;
                        numArray[index3] = index1 + 1;
                        ++index1;
                    }
                }
                m_lastIndex = index1;
                m_slots = slotArray;
                m_buckets = numArray;
                m_freeList = -1;
            }
        }

        internal void Initialize(int capacity) {
            int prime = Primes.GetNextBucketsSize(capacity);
            m_buckets = new int[prime];
            m_slots = new Slot[prime];
        }

        private void IncreaseCapacity() {
            int newSize = Primes.GetNextBucketsSize(2 * m_count);
            if (newSize <= m_count)
                throw new ArgumentException("Next prime to small");
            SetCapacity(newSize);
        }

        private void SetCapacity(int newSize) {
            Slot[] slotArray = new Slot[newSize];
            if (m_slots != null)
                Array.Copy(m_slots, 0, slotArray, 0, m_lastIndex);
            int[] numArray = new int[newSize];
            for (int index1 = 0; index1 < m_lastIndex; ++index1) {
                int index2 = slotArray[index1].value % newSize;
                slotArray[index1].next = numArray[index2] - 1;
                numArray[index2] = index1 + 1;
            }
            m_slots = slotArray;
            m_buckets = numArray;
        }

        private bool AddIfNotPresent(int value) {
            if (value < 0) {
                throw new ArgumentException("Negative values not allowed");
            }
            if (m_buckets == null)
                Initialize(0);
            int index1 = value % m_buckets.Length;
            int num = 0;
            for (int index2 = m_buckets[value % m_buckets.Length] - 1; index2 >= 0; index2 = m_slots[index2].next) {
                if (m_slots[index2].value == value)
                    return false;
                ++num;
            }
            int index3;
            if (m_freeList >= 0) {
                index3 = m_freeList;
                m_freeList = m_slots[index3].next;
            } else {
                if (m_lastIndex == m_slots.Length) {
                    IncreaseCapacity();
                    index1 = value % m_buckets.Length;
                }
                index3 = m_lastIndex;
                m_lastIndex = m_lastIndex + 1;
            }
            m_slots[index3].value = value;
            m_slots[index3].next = m_buckets[index1] - 1;
            m_buckets[index1] = index3 + 1;
            m_count = m_count + 1;
            return true;
        }

        
        
        internal int[] ToArray() {
            int[] array = new int[Count];
            CopyTo(array);
            return array;
        }

        internal static bool HashSetEquals(HashSetInt set1, HashSetInt set2) {
            if (set1 == null)
                return set2 == null;
            if (set2 == null)
                return false;
            if (set1.Count != set2.Count)
                return false;
            foreach (int obj in set2) {
                if (!set1.Contains(obj))
                    return false;
            }
            return true;
        }
        
        internal struct Slot {
            internal int value;
            internal int next;
        }

        /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.HashSet`1" /> object.</summary>
        [Serializable]
        [HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
        public struct Enumerator : IEnumerator<int>, IDisposable, IEnumerator {
            private HashSetInt set;
            private int index;
            private int current;

            /// <summary>Gets the element at the current position of the enumerator.</summary>
            /// <returns>The element in the <see cref="T:System.Collections.Generic.HashSet`1" /> collection at the current position of the enumerator.</returns>
            public int Current {
                get {
                    return current;
                }
            }

            object IEnumerator.Current {
                get {
                    if (index == 0 || index == set.m_lastIndex + 1)
                        throw new InvalidOperationException("cannot happen");
                    return (object)Current;
                }
            }

            internal Enumerator(HashSetInt set) {
                this.set = set;
                index = 0;
                current = default(int);
            }

            /// <summary>Releases all resources used by a <see cref="T:System.Collections.Generic.HashSet`1.Enumerator" /> object.</summary>
            public void Dispose() {
            }

            /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.HashSet`1" /> collection.</summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public bool MoveNext() {
                for (; index < set.m_lastIndex; index = index + 1) {
                    if (set.m_slots[index].value >= 0) {
                        current = set.m_slots[index].value;
                        index = index + 1;
                        return true;
                    }
                }
                index = set.m_lastIndex + 1;
                current = default(int);
                return false;
            }

            void IEnumerator.Reset() {
                index = 0;
                current = default(int);
            }
        }
    }
}
