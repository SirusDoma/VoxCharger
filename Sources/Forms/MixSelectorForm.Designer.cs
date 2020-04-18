namespace VoxCharger
{
    partial class MixSelectorForm
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
            this.ModSelectorLabel = new System.Windows.Forms.Label();
            this.MixSelectorDropDown = new System.Windows.Forms.ComboBox();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ModSelectorLabel
            // 
            this.ModSelectorLabel.AutoSize = true;
            this.ModSelectorLabel.Location = new System.Drawing.Point(10, 14);
            this.ModSelectorLabel.Name = "ModSelectorLabel";
            this.ModSelectorLabel.Size = new System.Drawing.Size(59, 13);
            this.ModSelectorLabel.TabIndex = 0;
            this.ModSelectorLabel.Text = "Select Mix:";
            // 
            // MixSelectorDropDown
            // 
            this.MixSelectorDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MixSelectorDropDown.FormattingEnabled = true;
            this.MixSelectorDropDown.Items.AddRange(new object[] {
            "-- Create New Mix --",
            "original"});
            this.MixSelectorDropDown.Location = new System.Drawing.Point(12, 30);
            this.MixSelectorDropDown.Name = "MixSelectorDropDown";
            this.MixSelectorDropDown.Size = new System.Drawing.Size(274, 21);
            this.MixSelectorDropDown.TabIndex = 1;
            this.MixSelectorDropDown.SelectedIndexChanged += new System.EventHandler(this.OnModSelectorDropDownSelectedIndexChanged);
            // 
            // ContinueButton
            // 
            this.ContinueButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ContinueButton.Enabled = false;
            this.ContinueButton.Location = new System.Drawing.Point(12, 62);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(274, 27);
            this.ContinueButton.TabIndex = 2;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = true;
            this.ContinueButton.Click += new System.EventHandler(this.OnContinueButtonClick);
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(12, 57);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(274, 20);
            this.NameTextBox.TabIndex = 3;
            this.NameTextBox.Text = "custom";
            this.NameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NameTextBox.Visible = false;
            // 
            // MixSelectorForm
            // 
            this.AcceptButton = this.ContinueButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 101);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.MixSelectorDropDown);
            this.Controls.Add(this.ModSelectorLabel);
            this.Controls.Add(this.NameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MixSelectorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mix Selection";
            this.Load += new System.EventHandler(this.OnModSelectorFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ModSelectorLabel;
        private System.Windows.Forms.ComboBox MixSelectorDropDown;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.TextBox NameTextBox;
    }
}