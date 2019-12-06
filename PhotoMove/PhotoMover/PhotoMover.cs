using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Windows.Forms;
using Configuration;

namespace PhotoMover {
    public partial class PhotoMover : Form {

        private static string[] logLevelPrefix = GetLogLevelPrefixes();

        private ConfigurationManager configurationManager;
        private PhotoMoverConfiguration configuration;
        private Action<DateTime, EventLevel, string> logDelegate;
        private EventHandler<EventArgs> completed;
        private BackgroundMover mover;
        
        private static string[] GetLogLevelPrefixes() {
            var result = new string[6];
            result[(int)EventLevel.LogAlways] = "Always   : ";
            result[(int)EventLevel.Critical] = "Critical : ";
            result[(int)EventLevel.Error] = "Error    : ";
            result[(int)EventLevel.Warning] = "Warning  : ";
            result[(int)EventLevel.Informational] = "Info     : ";
            result[(int)EventLevel.Verbose] = "Verbose  : ";
            return result;
    }

        public PhotoMover() {
            configurationManager = new ConfigurationManager(nameof(PhotoMover));
            configuration = configurationManager.Read<PhotoMoverConfiguration>();
            configuration.PropertyChanged += ConfigurationChanged;
            InitializeComponent();
            source.DataBindings.Add(new Binding(nameof(ComboBox.Text), configuration, nameof(PhotoMoverConfiguration.SourceLocation)));
            target.DataBindings.Add(new Binding(nameof(ComboBox.Text), configuration, nameof(PhotoMoverConfiguration.TargetLocation)));
            verbose.DataBindings.Add(new Binding(nameof(CheckBox.Checked), configuration, nameof(PhotoMoverConfiguration.Verbose)));
            logDelegate = LogInternal;
            mover = new BackgroundMover(Log);
            abort.Enabled = false;
            mover.Completed += OnCompleted;
            completed = new EventHandler<EventArgs>(OnCompleted);
        }

        private void ConfigurationChanged(object sender, PropertyChangedEventArgs e) {
            configurationManager.Save(configuration);
        }

        private void Execute(object sender, EventArgs e) {
            if (
                !Debugger.IsAttached || 
                MessageBox.Show("Are you sure?", "PhotoMover", MessageBoxButtons.YesNo) == DialogResult.Yes
            ) {
                Execute(doExecute: true);
            }
        }

        private void Fake(object sender, EventArgs e) {
            Execute(doExecute: false);
        }

        private void Execute(bool doExecute) {
            mover.SourceLocation = source.Text;
            mover.TargetLocation = target.Text;
            mover.Start(doExecute);
            execute.Enabled = false;
            abort.Enabled = true;
        }

        private void OnCompleted(object sender, EventArgs e) {
            if (InvokeRequired) {
                Invoke(completed, sender, e);
            } else {
                abort.Enabled = false;
                execute.Enabled = true;
            }
        }

        private void Abort(object sender, EventArgs e) {
            Log(EventLevel.Warning, "Aborting...");
            mover.Aborted = true;
        }

        internal void Log(EventLevel level, string message) {
            if (InvokeRequired) {
                Invoke(logDelegate, DateTime.Now, level, message);
            } else {
                logDelegate(DateTime.Now, level, message);
            }
        }

        private void LogInternal(DateTime timestamp, EventLevel level, string message) {
            if (level == EventLevel.Verbose && !configuration.Verbose) {
                return;
            }
            log.AppendText(timestamp.ToString("HH:mm:ss.ffffff") + " " + logLevelPrefix[(int)level] + message + Environment.NewLine);
        }
    }
}
