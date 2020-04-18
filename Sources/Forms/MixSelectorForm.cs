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
    public partial class MixSelectorForm : Form
    {
        public int EXPANDED_HEIGHT  = 165;
        public int COLLAPSED_HEIGHT = 140;

        public MixSelectorForm(bool createMode = false)
        {
            InitializeComponent();

            if (createMode)
            {
                Text                              = "Create Mix";
                MixSelectorDropDown.SelectedIndex = 0;
                NameTextBox.Location              = MixSelectorDropDown.Location;
                MixSelectorDropDown.Visible       = MixSelectorDropDown.Enabled = false;

                ModSelectorLabel.Text = "Enter mix name:";
                Size = new Size(Size.Width, COLLAPSED_HEIGHT);
            }
        }

        private void OnModSelectorFormLoad(object sender, EventArgs e)
        {
            foreach (string mix in AssetManager.GetMixes())
                MixSelectorDropDown.Items.Add(mix);
        }

        private void OnModSelectorDropDownSelectedIndexChanged(object sender, EventArgs e)
        {
            if (MixSelectorDropDown.SelectedIndex == 0)
                Size = new Size(Size.Width, EXPANDED_HEIGHT);
            else
                Size = new Size(Size.Width, COLLAPSED_HEIGHT);

            NameTextBox.Visible    = MixSelectorDropDown.SelectedIndex == 0;
            ContinueButton.Enabled = true;
        }

        private void OnContinueButtonClick(object sender, EventArgs e)
        {
            if (MixSelectorDropDown.SelectedIndex == 0)
            {
                if (string.IsNullOrEmpty(NameTextBox.Text))
                {
                    MessageBox.Show(
                        "Mix name cannot be empty",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }
                else if (AssetManager.GetMixes().Contains(NameTextBox.Text) || NameTextBox.Text.ToLower() == "original")
                {
                    MessageBox.Show(
                        "Mix name is already exists",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    return;
                }

                AssetManager.CreateMix(NameTextBox.Text);
            }
            else if (MixSelectorDropDown.SelectedIndex == 1)
                AssetManager.LoadMix(string.Empty);
            else
                AssetManager.LoadMix(MixSelectorDropDown.SelectedItem.ToString());

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
