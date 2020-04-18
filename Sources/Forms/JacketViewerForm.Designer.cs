namespace VoxCharger
{
    partial class JacketViewerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.JacketPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.JacketPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // JacketPictureBox
            // 
            this.JacketPictureBox.Location = new System.Drawing.Point(0, 0);
            this.JacketPictureBox.Name = "JacketPictureBox";
            this.JacketPictureBox.Size = new System.Drawing.Size(676, 676);
            this.JacketPictureBox.TabIndex = 0;
            this.JacketPictureBox.TabStop = false;
            this.JacketPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.JacketPictureBox_MouseClick);
            // 
            // JacketViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 676);
            this.Controls.Add(this.JacketPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "JacketViewerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jacket Preview";
            this.Load += new System.EventHandler(this.OnJacketFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.JacketPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox JacketPictureBox;
    }
}