using System;
using System.Collections.Generic;
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
                var sourceFiles = source.GetFiles();
                logger(EventLevel.Informational, "Found " + sourceFiles.Length + " files");
                for(int i = 0; !Aborted && i < sourceFiles.Length && i <= 5; i++) {
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
            metaData.Parse();
            if (metaData.CreationDate.HasValue) {
                string folder = metaData.CreationDate.Value.ToString("yyyy\\yyyy_MM\\yyyy_MM_dd");
                string targetName = Path.Combine(target.FullName, folder, sourceFile.Name);
                try {
                    if (doExecute && !Directory.Exists(folder)) {
                        Directory.CreateDirectory(folder);
                        logger(EventLevel.Informational, "Created:  " + folder);
                    }
                    if (!File.Exists(targetName)) {
                        if (doExecute) {
                            //File.Move(sourceFile.FullName, targetName);
                        }
                        logger(EventLevel.Informational, (doExecute ? "moving: " : "Fake: ") + sourceFile.Name + " -> " + targetName);
                    } else {
                        logger(EventLevel.Warning, "Already exists: " + sourceFile.Name + " -> " + targetName);
                    }
                } catch(Exception e) {
                    logger(EventLevel.Error, "Exception: " + sourceFile.Name + " -> " + targetName);
                    logger(EventLevel.Error, "Exception: " + e.Message);
                }
            }
        }
    }
}
