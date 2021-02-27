using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PhotoMetaData;

namespace PhotoMover {
    internal class BackgroundMover {

        private Action<EventLevel, string> logger;

            internal bool Executing { get; set; }
        internal bool Aborted { get; set; }
        internal string SourceLocation { get; set; }

        internal string TargetLocation { get; set; }
                
        internal EventHandler<EventArgs> Completed;
                
        internal BackgroundMover(Action<EventLevel, string> logger) {
            this.logger = logger;
        }

        internal void Start(bool doExecute) { 
            if (Executing) {
                throw new InvalidOperationException("Älready executing");
            }
            Thread thread = new Thread(Execute);
            thread.Name = "Moving ...";
            thread.IsBackground = true;
            Executing = true;
            thread.Start(doExecute);
        }

        internal void Execute(object doExecuteObject) {
            bool doExecute = (bool)doExecuteObject;
            try {
                DirectoryInfo source = new DirectoryInfo(SourceLocation);
                DirectoryInfo target = new DirectoryInfo(TargetLocation);

                if (!source.Exists) {
                    logger(EventLevel.Warning, source.FullName + " does not exist");
                    return;
                }
                if (!target.Exists) {
                    target.Create();
                }
                var sourceFiles = source.GetFiles("*", SearchOption.AllDirectories);
                logger(EventLevel.Informational, "Found " + sourceFiles.Length + " files");
                Array.Sort(sourceFiles, (a, b) => a.OldestDate().CompareTo(b.OldestDate()));
                int limit = Debugger.IsAttached ? 3 : int.MaxValue;
                for(int i = 0; !Aborted && i < sourceFiles.Length && i <= limit; i++) {
                    var sourceFile = sourceFiles[i];
                    MoveFile(sourceFile, target, doExecute);
                }
            } finally {
                if (Aborted) {
                    logger(EventLevel.LogAlways, "Aborted");
                } else {
                    logger(EventLevel.LogAlways, "Completed");
                }
                Executing = false;
                Completed?.Invoke(this, EventArgs.Empty);
            }
        }

        private void MoveFile(FileInfo sourceFile, DirectoryInfo target, bool doExecute) {
            var metaData = new MetaData(sourceFile, logger);
            if (metaData.Parse()) {
                if (metaData.CreationDate.HasValue) {
                    string folder = metaData.CreationDate.Value.ToString("yyyy\\\\yyyy_MM\\\\yyyy_MM_dd");
                    string targetFolder = Path.Combine(target.FullName, folder);
                    string targetName = Path.Combine(targetFolder, sourceFile.Name);
                    try {
                        if (doExecute && !Directory.Exists(targetFolder)) {
                            Directory.CreateDirectory(targetFolder);
                            logger(EventLevel.Informational, "Created:  " + targetFolder);
                        }
                        if (!File.Exists(targetName)) {
                            if (doExecute) {
                                File.Move(sourceFile.FullName, targetName);
                            }
                            logger(EventLevel.Informational, (doExecute ? "moving: " : "Fake: ") + sourceFile.Name + " -> " + targetName);
                        } else {
                            logger(EventLevel.Warning, "Already exists: " + sourceFile.Name + " -> " + targetName);
                            MoveFile(sourceFile, target.CreateSubdirectory("Duplicates"), doExecute);
                        }
                    } catch (Exception e) {
                        logger(EventLevel.Error, "Exception: " + sourceFile.Name + " -> " + targetName);
                        logger(EventLevel.Error, "Exception: " + e.Message);
                    }
                } else {
                    logger(EventLevel.Warning, "Could not move, no date information, " + sourceFile.FullName);
                }
            } else {
                logger(EventLevel.Warning, "Could not move, parse failed, " + sourceFile.FullName);
            }
        }
    }
}
