namespace VoxCharger
{
    partial class LevelEditorForm
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
            this.LevelLabel = new System.Windows.Forms.Label();
            this.LevelNumericBox = new System.Windows.Forms.NumericUpDown();
            this.EffectorTextBox = new System.Windows.Forms.TextBox();
            this.EffectorLabel = new System.Windows.Forms.Label();
            this.IllustratorLabel = new System.Windows.Forms.Label();
            this.IllustratorTextBox = new System.Windows.Forms.TextBox();
            this.CancelEditButton = new System.Windows.Forms.Button();
            this.JacketButton = new System.Windows.Forms.Button();
            this.SaveEditButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Preview2DX = new System.Windows.Forms.Button();
            this.MainDxButton = new System.Windows.Forms.Button();
            this.VoxButton = new System.Windows.Forms.Button();
            this.JacketPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LevelNumericBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JacketPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LevelLabel
            // 
            this.LevelLabel.AutoSize = true;
            this.LevelLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LevelLabel.Location = new System.Drawing.Point(11, 27);
            this.LevelLabel.Name = "LevelLabel";
            this.LevelLabel.Size = new System.Drawing.Size(32, 13);
            this.LevelLabel.TabIndex = 13;
            this.LevelLabel.Text = "Level";
            this.LevelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LevelNumericBox
            // 
            this.LevelNumericBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LevelNumericBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LevelNumericBox.Location = new System.Drawing.Point(72, 24);
            this.LevelNumericBox.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.LevelNumericBox.Name = "LevelNumericBox";
            this.LevelNumericBox.Size = new System.Drawing.Size(214, 21);
            this.LevelNumericBox.TabIndex = 14;
            this.LevelNumericBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // EffectorTextBox
            // 
            this.EffectorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EffectorTextBox.Location = new System.Drawing.Point(72, 51);
            this.EffectorTextBox.Name = "EffectorTextBox";
            this.EffectorTextBox.Size = new System.Drawing.Size(214, 20);
            this.EffectorTextBox.TabIndex = 15;
            // 
            // EffectorLabel
            // 
            this.EffectorLabel.AutoSize = true;
            this.EffectorLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EffectorLabel.Location = new System.Drawing.Point(12, 54);
            this.EffectorLabel.Name = "EffectorLabel";
            this.EffectorLabel.Size = new System.Drawing.Size(46, 13);
            this.EffectorLabel.TabIndex = 16;
            this.EffectorLabel.Text = "Effector";
            this.EffectorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IllustratorLabel
            // 
            this.IllustratorLabel.AutoSize = true;
            this.IllustratorLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IllustratorLabel.Location = new System.Drawing.Point(12, 80);
            this.IllustratorLabel.Name = "IllustratorLabel";
            this.IllustratorLabel.Size = new System.Drawing.Size(54, 13);
            this.IllustratorLabel.TabIndex = 18;
            this.IllustratorLabel.Text = "Illustrator";
            this.IllustratorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IllustratorTextBox
            // 
            this.IllustratorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IllustratorTextBox.Location = new System.Drawing.Point(72, 77);
            this.IllustratorTextBox.Name = "IllustratorTextBox";
            this.IllustratorTextBox.Size = new System.Drawing.Size(214, 20);
            this.IllustratorTextBox.TabIndex = 17;
            // 
            // CancelEditButton
            // 
            this.CancelEditButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelEditButton.Location = new System.Drawing.Point(343, 180);
            this.CancelEditButton.Name = "CancelEditButton";
            this.CancelEditButton.Size = new System.Drawing.Size(80, 28);
            this.CancelEditButton.TabIndex = 19;
            this.CancelEditButton.Text = "Cancel";
            this.CancelEditButton.UseVisualStyleBackColor = true;
            this.CancelEditButton.Click += new System.EventHandler(this.OnCancelEditButtonClick);
            // 
            // JacketButton
            // 
            this.JacketButton.Location = new System.Drawing.Point(12, 126);
            this.JacketButton.Name = "JacketButton";
            this.JacketButton.Size = new System.Drawing.Size(108, 22);
            this.JacketButton.TabIndex = 3;
            this.JacketButton.Text = "Jacket File";
            this.JacketButton.UseVisualStyleBackColor = true;
            this.JacketButton.Click += new System.EventHandler(this.OnJacketButtonClick);
            // 
            // SaveEditButton
            // 
            this.SaveEditButton.Location = new System.Drawing.Point(257, 180);
            this.SaveEditButton.Name = "SaveEditButton";
            this.SaveEditButton.Size = new System.Drawing.Size(80, 28);
            this.SaveEditButton.TabIndex = 23;
            this.SaveEditButton.Text = "Save";
            this.SaveEditButton.UseVisualStyleBackColor = true;
            this.SaveEditButton.Click += new System.EventHandler(this.OnSaveEditButtonClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Preview2DX);
            this.groupBox1.Controls.Add(this.MainDxButton);
            this.groupBox1.Controls.Add(this.VoxButton);
            this.groupBox1.Controls.Add(this.IllustratorTextBox);
            this.groupBox1.Controls.Add(this.LevelNumericBox);
            this.groupBox1.Controls.Add(this.LevelLabel);
            this.groupBox1.Controls.Add(this.IllustratorLabel);
            this.groupBox1.Controls.Add(this.EffectorTextBox);
            this.groupBox1.Controls.Add(this.EffectorLabel);
            this.groupBox1.Location = new System.Drawing.Point(126, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 162);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Level Data";
            // 
            // Preview2DX
            // 
            this.Preview2DX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Preview2DX.Location = new System.Drawing.Point(181, 129);
            this.Preview2DX.Name = "Preview2DX";
            this.Preview2DX.Size = new System.Drawing.Size(105, 22);
            this.Preview2DX.TabIndex = 23;
            this.Preview2DX.Text = "2DX Preview";
            this.Preview2DX.UseVisualStyleBackColor = true;
            this.Preview2DX.Click += new System.EventHandler(this.OnPreview2DXClick);
            // 
            // MainDxButton
            // 
            this.MainDxButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainDxButton.Location = new System.Drawing.Point(72, 129);
            this.MainDxButton.Name = "MainDxButton";
            this.MainDxButton.Size = new System.Drawing.Size(105, 22);
            this.MainDxButton.TabIndex = 22;
            this.MainDxButton.Text = "2DX Music";
            this.MainDxButton.UseVisualStyleBackColor = true;
            this.MainDxButton.Click += new System.EventHandler(this.OnMainDxButtonClick);
            // 
            // VoxButton
            // 
            this.VoxButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VoxButton.Location = new System.Drawing.Point(72, 101);
            this.VoxButton.Name = "VoxButton";
            this.VoxButton.Size = new System.Drawing.Size(214, 22);
            this.VoxButton.TabIndex = 21;
            this.VoxButton.Text = "Vox / Ksh File";
            this.VoxButton.UseVisualStyleBackColor = true;
            this.VoxButton.Click += new System.EventHandler(this.OnVoxButtonClick);
            // 
            // JacketPictureBox
            // 
            this.JacketPictureBox.Image = global::VoxCharger.Properties.Resources.jk_dummy;
            this.JacketPictureBox.Location = new System.Drawing.Point(12, 12);
            this.JacketPictureBox.Name = "JacketPictureBox";
            this.JacketPictureBox.Size = new System.Drawing.Size(108, 108);
            this.JacketPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.JacketPictureBox.TabIndex = 1;
            this.JacketPictureBox.TabStop = false;
            this.JacketPictureBox.Click += new System.EventHandler(this.OnJacketPictureBoxClick);
            // 
            // LevelEditorForm
            // 
            this.AcceptButton = this.SaveEditButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelEditButton;
            this.ClientSize = new System.Drawing.Size(435, 220);
            this.Controls.Add(this.SaveEditButton);
            this.Controls.Add(this.JacketPictureBox);
            this.Controls.Add(this.JacketButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CancelEditButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LevelEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Level Editor - MXM";
            this.Load += new System.EventHandler(this.OnLevelEditorFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.LevelNumericBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JacketPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox JacketPictureBox;
        private System.Windows.Forms.Label LevelLabel;
        private System.Windows.Forms.NumericUpDown LevelNumericBox;
        private System.Windows.Forms.TextBox EffectorTextBox;
        private System.Windows.Forms.Label EffectorLabel;
        private System.Windows.Forms.Label IllustratorLabel;
        private System.Windows.Forms.TextBox IllustratorTextBox;
        private System.Windows.Forms.Button CancelEditButton;
        private System.Windows.Forms.Button JacketButton;
        private System.Windows.Forms.Button SaveEditButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button VoxButton;
        private System.Windows.Forms.Button MainDxButton;
        private System.Windows.Forms.Button Preview2DX;
    }
}