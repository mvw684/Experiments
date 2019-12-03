using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoMetaData {
    public enum ExifType {
        UnsignedByte = 1,
        Ascii = 2,
        UnsignedShort = 3,
        UnsignedLong = 4,
        UnsignedRational = 5,
        SignedByte = 6,
        UndefinedBytes = 7,
        SignedShort = 8,
        SignedLong = 9,
        SignedRational = 10,
        SingleFloat = 11,
        DoubleFloat = 12
    }
}
