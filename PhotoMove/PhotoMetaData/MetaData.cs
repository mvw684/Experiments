using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetadataExtractor;
using MetadataExtractor.Formats.Adobe;
using MetadataExtractor.Formats.Avi;
using MetadataExtractor.Formats.Bmp;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileType;
using MetadataExtractor.Formats.Jfif;
using MetadataExtractor.Formats.Jpeg;

namespace PhotoMetaData {
    public class MetaData {
        private FileInfo file;
        private int indentLevel;
        private string indentString = string.Empty;
        Action<EventLevel, string> logger;
        public DateTime? CreationDate { get; private set; }

        public MetaData(FileInfo file, Action<EventLevel, string> logger) {
            this.file = file;
            this.logger = logger;
        }

        private int Indent {
            get {
                return indentLevel;
            }

            set {
                indentLevel = value;
                indentString = new string(' ', indentLevel);
            }
        }

        public void Parse() { 
            var exifData = ImageMetadataReader.ReadMetadata(file.FullName);
            LogInfo(file.FullName);
            Indent++;
            foreach(var exifDirectory in exifData) {
                ReadDirectory(exifDirectory);
            }
            Indent--;
        }

        private void ReadDirectory(MetadataExtractor.Directory exifDirectory) {
            switch(exifDirectory) {
                case AdobeJpegDirectory adobeJpegDirectory:
                    ReadDirectoryDetails(adobeJpegDirectory);
                    break;
                case JpegDirectory jpegDirectory:
                    ReadDirectoryDetails(jpegDirectory);
                    break;
                case JfifDirectory jfifDirectory:
                    ReadDirectoryDetails(jfifDirectory);
                    break;
                case HuffmanTablesDirectory huffmanTablesDirectory:
                    ReadDirectoryDetails(huffmanTablesDirectory);
                    break;
                case FileTypeDirectory fileTypeDirectory:
                    ReadDirectoryDetails(fileTypeDirectory);
                    break;
                default:
                    if (Debugger.IsAttached) {
                        Debugger.Break();
                    } else {
                        Debugger.Launch();
                    }
                    LogInfo("UNKNOWN DIR " + exifDirectory.GetType().FullName);
                    Indent++;
                    DumpDirectory(exifDirectory);
                    Indent--;
                    break;
            }
        }

        private void ReadDirectoryDetails(AdobeJpegDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(AviDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(BmpHeaderDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(ExifIfd0Directory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(ExifImageDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(ExifSubIfdDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(ExifThumbnailDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(GpsDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(JpegDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(JfifDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(HuffmanTablesDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(FileTypeDirectory directory) {
            DumpDirectory(directory);
        }

        private void DumpDirectory<T>(T directory) where T: MetadataExtractor.Directory {
            LogInfo(directory.Name);
            Indent++;
            foreach (var tag in directory.Tags) {
                LogInfo(tag.Name + "=" + tag.Description);
            }
            Indent--;
        }

        private void LogInfo(string message) {
            logger(EventLevel.Informational, indentString + message);
        }
    }
}
