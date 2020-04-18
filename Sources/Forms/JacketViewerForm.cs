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
    public partial class JacketViewerForm : Form
    {
        public JacketViewerForm(Image image)
        {
            InitializeComponent();

            JacketPictureBox.Size  = Size = image.Size;
            JacketPictureBox.Image = image;
        }

        private void OnJacketFormLoad(object sender, EventArgs e)
        {
        }

        private void JacketPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                using (var browser = new SaveFileDialog())
                {
                    browser.Filter = "Image Files|*.png;*.jpg;*.bmp";
                    if (browser.ShowDialog() == DialogResult.OK)
                        JacketPictureBox.Image.Save(browser.FileName);
                    
                }
            }
            else
                Close();
        }
    }
}
