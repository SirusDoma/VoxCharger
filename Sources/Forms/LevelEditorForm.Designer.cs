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
            this.MetadataGroupBox = new System.Windows.Forms.GroupBox();
            this.Preview2DX = new System.Windows.Forms.Button();
            this.MainDxButton = new System.Windows.Forms.Button();
            this.VoxButton = new System.Windows.Forms.Button();
            this.JacketPictureBox = new System.Windows.Forms.PictureBox();
            this.RadarGroupBox = new System.Windows.Forms.GroupBox();
            this.RadarHandTripLabel = new System.Windows.Forms.Label();
            this.HandTripNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RadarTsumamiLabel = new System.Windows.Forms.Label();
            this.TsumamiNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RadarTrickyLabel = new System.Windows.Forms.Label();
            this.TrickyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RadarOneHandLabel = new System.Windows.Forms.Label();
            this.OneHandNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RadarPeakLabel = new System.Windows.Forms.Label();
            this.PeakNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.RadarNotesLabel = new System.Windows.Forms.Label();
            this.NotesNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.AddRadarDataButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LevelNumericBox)).BeginInit();
            this.MetadataGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JacketPictureBox)).BeginInit();
            this.RadarGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HandTripNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TsumamiNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrickyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OneHandNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeakNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesNumericUpDown)).BeginInit();
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
            this.LevelNumericBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.LevelNumericBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LevelNumericBox.Location = new System.Drawing.Point(72, 24);
            this.LevelNumericBox.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            this.LevelNumericBox.Name = "LevelNumericBox";
            this.LevelNumericBox.Size = new System.Drawing.Size(214, 21);
            this.LevelNumericBox.TabIndex = 0;
            this.LevelNumericBox.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // EffectorTextBox
            // 
            this.EffectorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.EffectorTextBox.Location = new System.Drawing.Point(72, 51);
            this.EffectorTextBox.Name = "EffectorTextBox";
            this.EffectorTextBox.Size = new System.Drawing.Size(214, 20);
            this.EffectorTextBox.TabIndex = 1;
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
            this.IllustratorTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.IllustratorTextBox.Location = new System.Drawing.Point(72, 77);
            this.IllustratorTextBox.Name = "IllustratorTextBox";
            this.IllustratorTextBox.Size = new System.Drawing.Size(214, 20);
            this.IllustratorTextBox.TabIndex = 2;
            // 
            // CancelEditButton
            // 
            this.CancelEditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelEditButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelEditButton.Location = new System.Drawing.Point(343, 411);
            this.CancelEditButton.Name = "CancelEditButton";
            this.CancelEditButton.Size = new System.Drawing.Size(80, 28);
            this.CancelEditButton.TabIndex = 4;
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
            this.JacketButton.Text = "Import Jacket";
            this.JacketButton.UseVisualStyleBackColor = true;
            this.JacketButton.Click += new System.EventHandler(this.OnJacketButtonClick);
            // 
            // SaveEditButton
            // 
            this.SaveEditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveEditButton.Location = new System.Drawing.Point(257, 411);
            this.SaveEditButton.Name = "SaveEditButton";
            this.SaveEditButton.Size = new System.Drawing.Size(80, 28);
            this.SaveEditButton.TabIndex = 3;
            this.SaveEditButton.Text = "Save";
            this.SaveEditButton.UseVisualStyleBackColor = true;
            this.SaveEditButton.Click += new System.EventHandler(this.OnSaveEditButtonClick);
            // 
            // MetadataGroupBox
            // 
            this.MetadataGroupBox.Controls.Add(this.Preview2DX);
            this.MetadataGroupBox.Controls.Add(this.MainDxButton);
            this.MetadataGroupBox.Controls.Add(this.VoxButton);
            this.MetadataGroupBox.Controls.Add(this.IllustratorTextBox);
            this.MetadataGroupBox.Controls.Add(this.LevelNumericBox);
            this.MetadataGroupBox.Controls.Add(this.LevelLabel);
            this.MetadataGroupBox.Controls.Add(this.IllustratorLabel);
            this.MetadataGroupBox.Controls.Add(this.EffectorTextBox);
            this.MetadataGroupBox.Controls.Add(this.EffectorLabel);
            this.MetadataGroupBox.Location = new System.Drawing.Point(126, 12);
            this.MetadataGroupBox.Name = "MetadataGroupBox";
            this.MetadataGroupBox.Size = new System.Drawing.Size(297, 162);
            this.MetadataGroupBox.TabIndex = 0;
            this.MetadataGroupBox.TabStop = false;
            this.MetadataGroupBox.Text = "Level Data";
            // 
            // Preview2DX
            // 
            this.Preview2DX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.Preview2DX.Location = new System.Drawing.Point(181, 129);
            this.Preview2DX.Name = "Preview2DX";
            this.Preview2DX.Size = new System.Drawing.Size(105, 22);
            this.Preview2DX.TabIndex = 5;
            this.Preview2DX.Text = "Import Preview";
            this.Preview2DX.UseVisualStyleBackColor = true;
            this.Preview2DX.Click += new System.EventHandler(this.OnPreview2DXClick);
            // 
            // MainDxButton
            // 
            this.MainDxButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.MainDxButton.Location = new System.Drawing.Point(72, 129);
            this.MainDxButton.Name = "MainDxButton";
            this.MainDxButton.Size = new System.Drawing.Size(105, 22);
            this.MainDxButton.TabIndex = 4;
            this.MainDxButton.Text = "Import Music";
            this.MainDxButton.UseVisualStyleBackColor = true;
            this.MainDxButton.Click += new System.EventHandler(this.OnMainDxButtonClick);
            // 
            // VoxButton
            // 
            this.VoxButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.VoxButton.Location = new System.Drawing.Point(72, 101);
            this.VoxButton.Name = "VoxButton";
            this.VoxButton.Size = new System.Drawing.Size(214, 22);
            this.VoxButton.TabIndex = 3;
            this.VoxButton.Text = "Import Chart Data";
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
            // RadarGroupBox
            // 
            this.RadarGroupBox.Controls.Add(this.RadarHandTripLabel);
            this.RadarGroupBox.Controls.Add(this.HandTripNumericUpDown);
            this.RadarGroupBox.Controls.Add(this.RadarTsumamiLabel);
            this.RadarGroupBox.Controls.Add(this.TsumamiNumericUpDown);
            this.RadarGroupBox.Controls.Add(this.RadarTrickyLabel);
            this.RadarGroupBox.Controls.Add(this.TrickyNumericUpDown);
            this.RadarGroupBox.Controls.Add(this.RadarOneHandLabel);
            this.RadarGroupBox.Controls.Add(this.OneHandNumericUpDown);
            this.RadarGroupBox.Controls.Add(this.RadarPeakLabel);
            this.RadarGroupBox.Controls.Add(this.PeakNumericUpDown);
            this.RadarGroupBox.Controls.Add(this.RadarNotesLabel);
            this.RadarGroupBox.Controls.Add(this.NotesNumericUpDown);
            this.RadarGroupBox.Location = new System.Drawing.Point(12, 180);
            this.RadarGroupBox.Name = "RadarGroupBox";
            this.RadarGroupBox.Size = new System.Drawing.Size(411, 225);
            this.RadarGroupBox.TabIndex = 2;
            this.RadarGroupBox.TabStop = false;
            this.RadarGroupBox.Text = "Radar";
            // 
            // RadarHandTripLabel
            // 
            this.RadarHandTripLabel.AutoSize = true;
            this.RadarHandTripLabel.Location = new System.Drawing.Point(101, 111);
            this.RadarHandTripLabel.Name = "RadarHandTripLabel";
            this.RadarHandTripLabel.Size = new System.Drawing.Size(33, 26);
            this.RadarHandTripLabel.TabIndex = 35;
            this.RadarHandTripLabel.Text = "Hand\r\nTrip";
            this.RadarHandTripLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // HandTripNumericUpDown
            // 
            this.HandTripNumericUpDown.Location = new System.Drawing.Point(85, 141);
            this.HandTripNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.HandTripNumericUpDown.Name = "HandTripNumericUpDown";
            this.HandTripNumericUpDown.Size = new System.Drawing.Size(70, 20);
            this.HandTripNumericUpDown.TabIndex = 29;
            // 
            // RadarTsumamiLabel
            // 
            this.RadarTsumamiLabel.AutoSize = true;
            this.RadarTsumamiLabel.Location = new System.Drawing.Point(264, 111);
            this.RadarTsumamiLabel.Name = "RadarTsumamiLabel";
            this.RadarTsumamiLabel.Size = new System.Drawing.Size(49, 26);
            this.RadarTsumamiLabel.TabIndex = 34;
            this.RadarTsumamiLabel.Text = "Tsumami\r\n(Laser)";
            this.RadarTsumamiLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TsumamiNumericUpDown
            // 
            this.TsumamiNumericUpDown.Location = new System.Drawing.Point(256, 141);
            this.TsumamiNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.TsumamiNumericUpDown.Name = "TsumamiNumericUpDown";
            this.TsumamiNumericUpDown.Size = new System.Drawing.Size(70, 20);
            this.TsumamiNumericUpDown.TabIndex = 30;
            // 
            // RadarTrickyLabel
            // 
            this.RadarTrickyLabel.AutoSize = true;
            this.RadarTrickyLabel.Location = new System.Drawing.Point(185, 171);
            this.RadarTrickyLabel.Name = "RadarTrickyLabel";
            this.RadarTrickyLabel.Size = new System.Drawing.Size(36, 13);
            this.RadarTrickyLabel.TabIndex = 33;
            this.RadarTrickyLabel.Text = "Tricky";
            this.RadarTrickyLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TrickyNumericUpDown
            // 
            this.TrickyNumericUpDown.Location = new System.Drawing.Point(170, 187);
            this.TrickyNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.TrickyNumericUpDown.Name = "TrickyNumericUpDown";
            this.TrickyNumericUpDown.Size = new System.Drawing.Size(70, 20);
            this.TrickyNumericUpDown.TabIndex = 31;
            // 
            // RadarOneHandLabel
            // 
            this.RadarOneHandLabel.AutoSize = true;
            this.RadarOneHandLabel.Location = new System.Drawing.Point(101, 43);
            this.RadarOneHandLabel.Name = "RadarOneHandLabel";
            this.RadarOneHandLabel.Size = new System.Drawing.Size(33, 26);
            this.RadarOneHandLabel.TabIndex = 32;
            this.RadarOneHandLabel.Text = "One\r\nHand";
            this.RadarOneHandLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // OneHandNumericUpDown
            // 
            this.OneHandNumericUpDown.Location = new System.Drawing.Point(85, 73);
            this.OneHandNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.OneHandNumericUpDown.Name = "OneHandNumericUpDown";
            this.OneHandNumericUpDown.Size = new System.Drawing.Size(70, 20);
            this.OneHandNumericUpDown.TabIndex = 25;
            // 
            // RadarPeakLabel
            // 
            this.RadarPeakLabel.AutoSize = true;
            this.RadarPeakLabel.Location = new System.Drawing.Point(272, 57);
            this.RadarPeakLabel.Name = "RadarPeakLabel";
            this.RadarPeakLabel.Size = new System.Drawing.Size(32, 13);
            this.RadarPeakLabel.TabIndex = 28;
            this.RadarPeakLabel.Text = "Peak";
            this.RadarPeakLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // PeakNumericUpDown
            // 
            this.PeakNumericUpDown.Location = new System.Drawing.Point(256, 73);
            this.PeakNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.PeakNumericUpDown.Name = "PeakNumericUpDown";
            this.PeakNumericUpDown.Size = new System.Drawing.Size(70, 20);
            this.PeakNumericUpDown.TabIndex = 27;
            // 
            // RadarNotesLabel
            // 
            this.RadarNotesLabel.AutoSize = true;
            this.RadarNotesLabel.Location = new System.Drawing.Point(185, 14);
            this.RadarNotesLabel.Name = "RadarNotesLabel";
            this.RadarNotesLabel.Size = new System.Drawing.Size(35, 13);
            this.RadarNotesLabel.TabIndex = 26;
            this.RadarNotesLabel.Text = "Notes";
            this.RadarNotesLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // NotesNumericUpDown
            // 
            this.NotesNumericUpDown.Location = new System.Drawing.Point(170, 30);
            this.NotesNumericUpDown.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            this.NotesNumericUpDown.Name = "NotesNumericUpDown";
            this.NotesNumericUpDown.Size = new System.Drawing.Size(70, 20);
            this.NotesNumericUpDown.TabIndex = 24;
            // 
            // AddRadarDataButton
            // 
            this.AddRadarDataButton.Location = new System.Drawing.Point(12, 180);
            this.AddRadarDataButton.Name = "AddRadarDataButton";
            this.AddRadarDataButton.Size = new System.Drawing.Size(411, 25);
            this.AddRadarDataButton.TabIndex = 1;
            this.AddRadarDataButton.Text = "Add Radar Data";
            this.AddRadarDataButton.UseVisualStyleBackColor = true;
            this.AddRadarDataButton.Click += new System.EventHandler(this.OnAddRadarDataButtonClick);
            // 
            // LevelEditorForm
            // 
            this.AcceptButton = this.SaveEditButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelEditButton;
            this.ClientSize = new System.Drawing.Size(435, 451);
            this.Controls.Add(this.SaveEditButton);
            this.Controls.Add(this.CancelEditButton);
            this.Controls.Add(this.RadarGroupBox);
            this.Controls.Add(this.AddRadarDataButton);
            this.Controls.Add(this.JacketPictureBox);
            this.Controls.Add(this.JacketButton);
            this.Controls.Add(this.MetadataGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LevelEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Level Editor - MXM";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnLevelEditorFormFormClosing);
            this.Load += new System.EventHandler(this.OnLevelEditorFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.LevelNumericBox)).EndInit();
            this.MetadataGroupBox.ResumeLayout(false);
            this.MetadataGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JacketPictureBox)).EndInit();
            this.RadarGroupBox.ResumeLayout(false);
            this.RadarGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HandTripNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TsumamiNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrickyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OneHandNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeakNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesNumericUpDown)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button AddRadarDataButton;

        private System.Windows.Forms.Label RadarHandTripLabel;
        private System.Windows.Forms.NumericUpDown HandTripNumericUpDown;
        private System.Windows.Forms.Label RadarTsumamiLabel;
        private System.Windows.Forms.NumericUpDown TsumamiNumericUpDown;
        private System.Windows.Forms.Label RadarTrickyLabel;
        private System.Windows.Forms.NumericUpDown TrickyNumericUpDown;
        private System.Windows.Forms.Label RadarOneHandLabel;
        private System.Windows.Forms.NumericUpDown OneHandNumericUpDown;
        private System.Windows.Forms.Label RadarPeakLabel;
        private System.Windows.Forms.NumericUpDown PeakNumericUpDown;
        private System.Windows.Forms.Label RadarNotesLabel;
        private System.Windows.Forms.NumericUpDown NotesNumericUpDown;

        private System.Windows.Forms.GroupBox RadarGroupBox;

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
        private System.Windows.Forms.GroupBox MetadataGroupBox;
        private System.Windows.Forms.Button VoxButton;
        private System.Windows.Forms.Button MainDxButton;
        private System.Windows.Forms.Button Preview2DX;
    }
}