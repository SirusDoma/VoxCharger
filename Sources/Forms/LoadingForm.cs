using System;
using System.Windows.Forms;

namespace VoxCharger
{
    public partial class LoadingForm : Form
    {
        private Action<LoadingForm> _action;
        private bool _completed = false;

        public LoadingForm()
        {
            InitializeComponent();
        }

        private void OnLoadingForm(object sender, EventArgs e)
        {
        }

        private void OnLoadingFormShown(object sender, EventArgs e)
        {
            this._action(this);
        }

        public void SetAction(Action<LoadingForm> action)
        {
            this._action = action;
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
            _completed = true;
            if (InvokeRequired)
                Invoke(new Action(Close));
            else
                Close();
        }

        private void OnLoadingFormFormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !_completed;
        }
    }
}
