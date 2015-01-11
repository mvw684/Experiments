using System;
using System.Windows.Forms;

namespace Fractals
{
    public partial class FractalControl : Form
    {
        public class Engine {
            public Engine(Type form) {
                this.Form = form;
                this.Name = form.Name;
            }
            public Type Form;
            public string Name;

            public override string ToString() {
                return Name;
            }
        }
        public FractalControl()
        {
            InitializeComponent();
            Type[] types = typeof(FractalControl).Assembly.GetExportedTypes();
            for (int i = 0; i < types.Length; i++) {
                var type = types[i];
                if (type.BaseType == typeof(Form) && type.Name.EndsWith("Display")) {
                    engines.Items.Add(new Engine(type));
                }
            }
            engines.SelectedIndex = 0;
        }

        private Engine selected;

        private void engines_SelectedIndexChanged(object sender, EventArgs e) {
            selected = (Engine)engines.SelectedItem;
            launch.Text = selected.Name;
        }

        private void launch_Click(object sender, EventArgs e) {
            if (selected != null) {
                Form f = (Form)Activator.CreateInstance(selected.Form);
                f.ShowDialog();
            }
        }

        private void engines_MouseDoubleClick(object sender, MouseEventArgs e) {
            if (selected != null) {
                Form f = (Form)Activator.CreateInstance(selected.Form);
                f.Show(this);
            }
        }
    }
}
