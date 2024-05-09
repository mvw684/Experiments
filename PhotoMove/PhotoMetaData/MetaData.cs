using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;

using MetadataExtractor;
using MetadataExtractor.Formats.Adobe;
using MetadataExtractor.Formats.Avi;
using MetadataExtractor.Formats.Bmp;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.Exif.Makernotes;
using MetadataExtractor.Formats.FileSystem;
using MetadataExtractor.Formats.FileType;
using MetadataExtractor.Formats.Gif;
using MetadataExtractor.Formats.Icc;
using MetadataExtractor.Formats.Iptc;
using MetadataExtractor.Formats.Jfif;
using MetadataExtractor.Formats.Jpeg;
using MetadataExtractor.Formats.Photoshop;
using MetadataExtractor.Formats.Png;
using MetadataExtractor.Formats.QuickTime;
using MetadataExtractor.Formats.Xmp;

namespace PhotoMetaData {
    public class MetaData {
        private DateTime? creationDate;
        private FileInfo file;
        private int indentLevel;
        private string indentString = string.Empty;
        Action<EventLevel, string> logger;
        public DateTime? CreationDate {
            get {
                return creationDate;
            }

        }
        private void SetCreationDate(DateTime value) {
            if (!value.IsValid()) {
                return;
            }
            var localValue = value.ToLocalTime();
            if (!creationDate.HasValue) {
                creationDate = localValue;
            } else {
                if (creationDate > localValue) {
                    creationDate = localValue;
                }
            }
        }

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

        public bool Parse() {
            try {
                if (file.Name.Equals("Desktop.ini", StringComparison.OrdinalIgnoreCase)) {
                    return false;
                }
                var exifData = ImageMetadataReader.ReadMetadata(file.FullName);
                logger(EventLevel.Informational, file.FullName);
                Indent++;
                foreach (var exifDirectory in exifData) {
                    ReadGenericDirectory(exifDirectory);
                }
                Indent--;
                return true;
            } catch (Exception e) {
                logger(EventLevel.Warning, e.GetType().Name + " " + e.Message + " " + file.FullName);
                return false;
            }
        }

        private void ReadGenericDirectory(MetadataExtractor.Directory exifDirectory) {
            switch(exifDirectory) {
                case AdobeJpegDirectory adobeJpegDirectory:
                    ReadDirectoryDetails(adobeJpegDirectory);
                    break;
                case JpegDirectory jpegDirectory:
                    ReadDirectoryDetails(jpegDirectory);
                    break;
                case GifHeaderDirectory gifHeaderDirectory:
                    ReadDirectoryDetails(gifHeaderDirectory);
                    break;
                case GifControlDirectory gifControlDirectory:
                    ReadDirectoryDetails(gifControlDirectory);
                    break;
                case GifImageDirectory gifImageDirectory:
                    ReadDirectoryDetails(gifImageDirectory);
                    break;
                case JfifDirectory jfifDirectory:
                    ReadDirectoryDetails(jfifDirectory);
                    break;
                case AviDirectory aviDirectory:
                    ReadDirectoryDetails(aviDirectory); 
                    break;
                case HuffmanTablesDirectory huffmanTablesDirectory:
                    ReadDirectoryDetails(huffmanTablesDirectory);
                    break;
                case FileTypeDirectory fileTypeDirectory:
                    ReadDirectoryDetails(fileTypeDirectory);
                    break;
                case ExifIfd0Directory exifIfd0Directory:
                    ReadDirectoryDetails(exifIfd0Directory);
                    break;
                case ExifInteropDirectory exifInteropDirectory:
                    ReadDirectoryDetails(exifInteropDirectory);
                    break;
                case ExifSubIfdDirectory exifSubIfdDirectory:
                    ReadDirectoryDetails(exifSubIfdDirectory);
                    break;
                case ExifThumbnailDirectory exifThumbnailDirectory:
                    ReadDirectoryDetails(exifThumbnailDirectory);
                    break;
                case GpsDirectory gpsDirectory:
                    ReadDirectoryDetails(gpsDirectory);
                    break;
                case FileMetadataDirectory fileMetadataDirectory:
                    ReadDirectoryDetails(fileMetadataDirectory);
                    break;
                case CanonMakernoteDirectory canonMakernoteDirectory:
                    ReadDirectoryDetails(canonMakernoteDirectory);
                    break;
                case PngDirectory pngDirectory:
                    ReadDirectoryDetails(pngDirectory);
                    break;
                case PhotoshopDirectory photoshopDirectory:
                    ReadDirectoryDetails(photoshopDirectory);
                    break;
                case IptcDirectory iptcDirectory:
                    ReadDirectoryDetails(iptcDirectory);
                    break;
                case QuickTimeFileTypeDirectory quickTimeFileTypeDirectory:
                    ReadDirectoryDetails(quickTimeFileTypeDirectory);
                    break;
                case QuickTimeMetadataHeaderDirectory quickTimeMetadataHeaderDirectory:
                    ReadDirectoryDetails(quickTimeMetadataHeaderDirectory);
                    break;
                case QuickTimeMovieHeaderDirectory quickTimeMovieHeaderDirectory:
                    ReadDirectoryDetails(quickTimeMovieHeaderDirectory);
                    break;
                case QuickTimeTrackHeaderDirectory quickTimeTrackHeaderDirectory:
                    ReadDirectoryDetails(quickTimeTrackHeaderDirectory);
                    break;
                case XmpDirectory xmpDirectory:
                    ReadDirectoryDetails(xmpDirectory);
                    break;
                case JpegCommentDirectory jpegCommentDirectory:
                    ReadDirectoryDetails(jpegCommentDirectory);
                    break;
                case IccDirectory iccDirectory:
                    ReadDirectoryDetails(iccDirectory);
                    break;
                case SamsungType2MakernoteDirectory samsungType2MakernoteDirectory:
                    ReadDirectoryDetails(samsungType2MakernoteDirectory);
                    break;
                case KodakMakernoteDirectory kodakMakernoteDirectory:
                    ReadDirectoryDetails(kodakMakernoteDirectory);
                    break;
                case PanasonicMakernoteDirectory panasonicMakernoteDirectory:
                    ReadDirectoryDetails(panasonicMakernoteDirectory);
                    break;
                case NikonType2MakernoteDirectory nikonType2MakernoteDirectory:
                    ReadDirectoryDetails(nikonType2MakernoteDirectory);
                    break;
                case CasioType2MakernoteDirectory casioType2MakernoteDirectory:
                    ReadDirectoryDetails(casioType2MakernoteDirectory);
                    break;
                case SonyType1MakernoteDirectory sonyType1MakernoteDirectory:
                    ReadDirectoryDetails(sonyType1MakernoteDirectory);
                    break;
                case PrintIMDirectory printIMDirectory:
                    ReadDirectoryDetails(printIMDirectory);
                    break;

                default:
                    if (Debugger.IsAttached) {
                        Debugger.Break();
                    } else {
                        Debugger.Launch();
                    }
                    logger(EventLevel.Warning, "UNKNOWN DIR " + exifDirectory.GetType().FullName);
                    //Indent++;
                    //DumpDirectory(exifDirectory);
                    //Indent--;
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
            if (directory.TryGetDateTime(ExifIfd0Directory.TagDateTime, out var dateTime)) {
                SetCreationDate(dateTime);
            }
            if (directory.TryGetDateTime(ExifIfd0Directory.TagDateTimeDigitized, out dateTime)) {
                SetCreationDate(dateTime);
            }
            var camera = directory.GetString(ExifIfd0Directory.TagMake);
            if (!string.IsNullOrEmpty(camera)) {
                logger(EventLevel.LogAlways, "camera: " + camera);
            }
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

        private void ReadDirectoryDetails(JpegDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(GifHeaderDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(GifControlDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(GifImageDirectory directory) {
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

        private void ReadDirectoryDetails(ExifInteropDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(GpsDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(FileMetadataDirectory directory) {
            if (directory.TryGetDateTime(FileMetadataDirectory.TagFileModifiedDate, out var dateTime)) {
                SetCreationDate(dateTime);
            }
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(CanonMakernoteDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(PngDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(PhotoshopDirectory directory) {
            DumpDirectory(directory, EventLevel.Informational);
        }

        private void ReadDirectoryDetails(IptcDirectory directory) {
            DumpDirectory(directory, EventLevel.Informational);
        }

        private void ReadDirectoryDetails(QuickTimeFileTypeDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(QuickTimeMetadataHeaderDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(QuickTimeMovieHeaderDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(QuickTimeTrackHeaderDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(PanasonicMakernoteDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(SamsungType2MakernoteDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(KodakMakernoteDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(NikonType2MakernoteDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(CasioType2MakernoteDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(SonyType1MakernoteDirectory directory) {
            DumpDirectory(directory);
        }
                
        private void ReadDirectoryDetails(PrintIMDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(XmpDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(JpegCommentDirectory directory) {
            DumpDirectory(directory);
        }

        private void ReadDirectoryDetails(IccDirectory directory) {
            DumpDirectory(directory);
        }


        private void DumpDirectory<T>(
            T directory, 
            EventLevel eventLevel = EventLevel.Verbose
        ) where T: MetadataExtractor.Directory {
            logger(EventLevel.Verbose, directory.Name + " - " + typeof(T).FullName);
            Indent++;
            foreach (var tag in directory.Tags) {
                logger(eventLevel, directory.Name + "." + tag.Name + "=" + tag.Description);
            }
            Indent--;
        }
    }
}
