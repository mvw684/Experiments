using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PhotoMetaData {
    public static class Extensions {

        private static DateTime zeroFileTime = DateTime.FromFileTime(0);
        private static DateTime zeroPosix = new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc);
        private static DateTime zeroPosixLocal = zeroPosix.ToLocalTime();

        public static bool IsValid(this DateTime dateTime) {
            return
                dateTime != DateTime.MinValue &&
                dateTime.Ticks != 0 &&
                dateTime != zeroFileTime &&
                dateTime != zeroPosix &&
                dateTime != zeroPosixLocal;
        }

        public static DateTime OldestDate(this FileInfo file) {
            var dates = new DateTime[3];
            int index = 0;
            var create = file.CreationTimeUtc;
            var modify = file.LastWriteTimeUtc;
            var lastAccess = file.LastAccessTimeUtc;

            if (create.IsValid()) {
                dates[index++] = create;
            }

            if (modify.IsValid()) {
                dates[index++] = modify;
            }
            if (lastAccess.IsValid()) {
                dates[index++] = lastAccess;
            }
            Array.Sort(dates, 0, index);
            return dates[0];
        }
    }
}
