using System;
using System.Windows.Forms;

namespace VoxCharger
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void OnEmailLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto://o2jam@cxo2.me");
        }

        private void OnCloseButtonClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
