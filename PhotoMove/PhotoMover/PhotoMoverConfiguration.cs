using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PhotoMover {
    public class PhotoMoverConfiguration : INotifyPropertyChanged {

        private string sourceLocation;
        private string targetLocation;

        public event PropertyChangedEventHandler PropertyChanged;

        public string SourceLocation {
            get {
                return sourceLocation;
            }
            set {
                sourceLocation = value;
                NotifyPropertyChanged();
            }
        }

        public string TargetLocation {
            get {
                return targetLocation;
            }
            set {
                targetLocation = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string property = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
