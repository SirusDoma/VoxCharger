using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoxCharger
{
    public partial class LoadingForm : Form
    {
        private Action action;
        private bool completed = false;

        public LoadingForm()
        {
            InitializeComponent();
        }

        private void OnLoadingForm(object sender, EventArgs e)
        {
        }

        private void OnLoadingFormShown(object sender, EventArgs e)
        {
            this.action();
        }

        public void SetAction(Action action)
        {
            this.action = action;
        }

        public void SetStatus(string text)
        {
            if (StatusLabel.InvokeRequired)
            {
                StatusLabel.Invoke(new Action(() => SetStatus(text)));
                return;
            }

            StatusLabel.Text = text;
            StatusLabel.Update();
        }

        public void SetProgress(float percentage)
        {
            if (ProgressBar.InvokeRequired)
            {
                ProgressBar.Invoke(new Action<float>(SetProgress), percentage);
                return;
            }

            percentage = percentage > 100f ? 100f : percentage < 0f ? 0f : percentage;
            ProgressBar.Value = (int)percentage;
            ProgressBar.Update();
        }

        public void Complete()
        {
            completed = true;
            if (InvokeRequired)
                Invoke(new Action(Close));
            else
                Close();
        }

        private void OnLoadingFormFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !completed;
        }
    }
}
