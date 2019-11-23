using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoMetaData {
    public class MetaData {
        private FileInfo file;
        public MetaData(FileInfo file) {
            this.file = file;
        }
    }
}
