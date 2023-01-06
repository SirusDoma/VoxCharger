using System.Windows.Forms;

namespace VoxCharger
{
    partial class ConverterForm
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
            this.LevelGroupBox = new System.Windows.Forms.GroupBox();
            this.InfEditButton = new System.Windows.Forms.Button();
            this.ExhEditButton = new System.Windows.Forms.Button();
            this.AdvEditButton = new System.Windows.Forms.Button();
            this.NovEditButton = new System.Windows.Forms.Button();
            this.JacketInfPictureBox = new System.Windows.Forms.PictureBox();
            this.JacketExhPictureBox = new System.Windows.Forms.PictureBox();
            this.JacketAdvPictureBox = new System.Windows.Forms.PictureBox();
            this.JacketNovPictureBox = new System.Windows.Forms.PictureBox();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.SoundEffectsCheckBox = new System.Windows.Forms.CheckBox();
            this.CameraEffectsCheckBox = new System.Windows.Forms.CheckBox();
            this.CameraEffectsGroupBox = new System.Windows.Forms.GroupBox();
            this.CameraMappingButton = new System.Windows.Forms.Button();
            this.CameraEffectsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.TracksGroupBox = new System.Windows.Forms.GroupBox();
            this.TrackFXRCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackFXLCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackVolRCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackButtonDCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackButtonCCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackButtonBCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackButtonACheckBox = new System.Windows.Forms.CheckBox();
            this.TrackVolLCheckBox = new System.Windows.Forms.CheckBox();
            this.SoundEffectGroupBox = new System.Windows.Forms.GroupBox();
            this.SoundMappingButton = new System.Windows.Forms.Button();
            this.SoundEffectsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.VersionDropDown = new System.Windows.Forms.ComboBox();
            this.InfVerDropDown = new System.Windows.Forms.ComboBox();
            this.BackgroundDropDown = new System.Windows.Forms.ComboBox();
            this.BackgroundLabel = new System.Windows.Forms.Label();
            this.RealignOffsetCheckBox = new System.Windows.Forms.CheckBox();
            this.AsciiAutoCheckBox = new System.Windows.Forms.CheckBox();
            this.InfVerLabel = new System.Windows.Forms.Label();
            this.MeasureLabel = new System.Windows.Forms.Label();
            this.AsciiTextBox = new System.Windows.Forms.TextBox();
            this.MusicCodeLabel = new System.Windows.Forms.Label();
            this.CancelConvertButton = new System.Windows.Forms.Button();
            this.ProcessConvertButton = new System.Windows.Forms.Button();
            this.TargetLabel = new System.Windows.Forms.Label();
            this.MusicGroupBox = new System.Windows.Forms.GroupBox();
            this.FormatLabel = new System.Windows.Forms.Label();
            this.PreviewTimePicker = new System.Windows.Forms.DateTimePicker();
            this.MusicFormatDropDown = new System.Windows.Forms.ComboBox();
            this.PreviewOffsetLabel = new System.Windows.Forms.Label();
            this.LevelGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JacketInfPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketExhPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketAdvPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketNovPictureBox)).BeginInit();
            this.OptionsGroupBox.SuspendLayout();
            this.CameraEffectsGroupBox.SuspendLayout();
            this.TracksGroupBox.SuspendLayout();
            this.SoundEffectGroupBox.SuspendLayout();
            this.MusicGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LevelGroupBox
            // 
            this.LevelGroupBox.Controls.Add(this.InfEditButton);
            this.LevelGroupBox.Controls.Add(this.ExhEditButton);
            this.LevelGroupBox.Controls.Add(this.AdvEditButton);
            this.LevelGroupBox.Controls.Add(this.NovEditButton);
            this.LevelGroupBox.Controls.Add(this.JacketInfPictureBox);
            this.LevelGroupBox.Controls.Add(this.JacketExhPictureBox);
            this.LevelGroupBox.Controls.Add(this.JacketAdvPictureBox);
            this.LevelGroupBox.Controls.Add(this.JacketNovPictureBox);
            this.LevelGroupBox.Location = new System.Drawing.Point(12, 44);
            this.LevelGroupBox.Name = "LevelGroupBox";
            this.LevelGroupBox.Size = new System.Drawing.Size(447, 165);
            this.LevelGroupBox.TabIndex = 1;
            this.LevelGroupBox.TabStop = false;
            this.LevelGroupBox.Text = "Levels";
            // 
            // InfEditButton
            // 
            this.InfEditButton.Location = new System.Drawing.Point(331, 133);
            this.InfEditButton.Name = "InfEditButton";
            this.InfEditButton.Size = new System.Drawing.Size(108, 26);
            this.InfEditButton.TabIndex = 3;
            this.InfEditButton.Tag = "4";
            this.InfEditButton.Text = "--";
            this.InfEditButton.UseVisualStyleBackColor = true;
            this.InfEditButton.Click += new System.EventHandler(this.OnLevelEditButtonClick);
            // 
            // ExhEditButton
            // 
            this.ExhEditButton.Location = new System.Drawing.Point(223, 133);
            this.ExhEditButton.Name = "ExhEditButton";
            this.ExhEditButton.Size = new System.Drawing.Size(108, 26);
            this.ExhEditButton.TabIndex = 2;
            this.ExhEditButton.Tag = "3";
            this.ExhEditButton.Text = "EXH";
            this.ExhEditButton.UseVisualStyleBackColor = true;
            this.ExhEditButton.Click += new System.EventHandler(this.OnLevelEditButtonClick);
            // 
            // AdvEditButton
            // 
            this.AdvEditButton.Location = new System.Drawing.Point(115, 133);
            this.AdvEditButton.Name = "AdvEditButton";
            this.AdvEditButton.Size = new System.Drawing.Size(108, 26);
            this.AdvEditButton.TabIndex = 1;
            this.AdvEditButton.Tag = "2";
            this.AdvEditButton.Text = "ADV";
            this.AdvEditButton.UseVisualStyleBackColor = true;
            this.AdvEditButton.Click += new System.EventHandler(this.OnLevelEditButtonClick);
            // 
            // NovEditButton
            // 
            this.NovEditButton.Location = new System.Drawing.Point(7, 133);
            this.NovEditButton.Name = "NovEditButton";
            this.NovEditButton.Size = new System.Drawing.Size(108, 26);
            this.NovEditButton.TabIndex = 0;
            this.NovEditButton.Tag = "1";
            this.NovEditButton.Text = "NOV";
            this.NovEditButton.UseVisualStyleBackColor = true;
            this.NovEditButton.Click += new System.EventHandler(this.OnLevelEditButtonClick);
            // 
            // JacketInfPictureBox
            // 
            this.JacketInfPictureBox.Image = global::VoxCharger.Properties.Resources.jk_dummy_s;
            this.JacketInfPictureBox.Location = new System.Drawing.Point(331, 19);
            this.JacketInfPictureBox.Name = "JacketInfPictureBox";
            this.JacketInfPictureBox.Size = new System.Drawing.Size(108, 108);
            this.JacketInfPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.JacketInfPictureBox.TabIndex = 6;
            this.JacketInfPictureBox.TabStop = false;
            this.JacketInfPictureBox.Tag = "4";
            // 
            // JacketExhPictureBox
            // 
            this.JacketExhPictureBox.Image = global::VoxCharger.Properties.Resources.jk_dummy_s;
            this.JacketExhPictureBox.Location = new System.Drawing.Point(223, 19);
            this.JacketExhPictureBox.Name = "JacketExhPictureBox";
            this.JacketExhPictureBox.Size = new System.Drawing.Size(108, 108);
            this.JacketExhPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.JacketExhPictureBox.TabIndex = 29;
            this.JacketExhPictureBox.TabStop = false;
            this.JacketExhPictureBox.Tag = "3";
            // 
            // JacketAdvPictureBox
            // 
            this.JacketAdvPictureBox.Image = global::VoxCharger.Properties.Resources.jk_dummy_s;
            this.JacketAdvPictureBox.Location = new System.Drawing.Point(115, 19);
            this.JacketAdvPictureBox.Name = "JacketAdvPictureBox";
            this.JacketAdvPictureBox.Size = new System.Drawing.Size(108, 108);
            this.JacketAdvPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.JacketAdvPictureBox.TabIndex = 1;
            this.JacketAdvPictureBox.TabStop = false;
            this.JacketAdvPictureBox.Tag = "2";
            // 
            // JacketNovPictureBox
            // 
            this.JacketNovPictureBox.Image = global::VoxCharger.Properties.Resources.jk_dummy_s;
            this.JacketNovPictureBox.Location = new System.Drawing.Point(7, 19);
            this.JacketNovPictureBox.Name = "JacketNovPictureBox";
            this.JacketNovPictureBox.Size = new System.Drawing.Size(108, 108);
            this.JacketNovPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.JacketNovPictureBox.TabIndex = 0;
            this.JacketNovPictureBox.TabStop = false;
            this.JacketNovPictureBox.Tag = "1";
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(50, 17);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.ReadOnly = true;
            this.PathTextBox.Size = new System.Drawing.Size(409, 20);
            this.PathTextBox.TabIndex = 0;
            // 
            // OptionsGroupBox
            // 
            this.OptionsGroupBox.Controls.Add(this.SoundEffectsCheckBox);
            this.OptionsGroupBox.Controls.Add(this.CameraEffectsCheckBox);
            this.OptionsGroupBox.Controls.Add(this.CameraEffectsGroupBox);
            this.OptionsGroupBox.Controls.Add(this.TracksGroupBox);
            this.OptionsGroupBox.Controls.Add(this.SoundEffectGroupBox);
            this.OptionsGroupBox.Controls.Add(this.VersionDropDown);
            this.OptionsGroupBox.Controls.Add(this.InfVerDropDown);
            this.OptionsGroupBox.Controls.Add(this.BackgroundDropDown);
            this.OptionsGroupBox.Controls.Add(this.BackgroundLabel);
            this.OptionsGroupBox.Controls.Add(this.RealignOffsetCheckBox);
            this.OptionsGroupBox.Controls.Add(this.AsciiAutoCheckBox);
            this.OptionsGroupBox.Controls.Add(this.InfVerLabel);
            this.OptionsGroupBox.Controls.Add(this.MeasureLabel);
            this.OptionsGroupBox.Controls.Add(this.AsciiTextBox);
            this.OptionsGroupBox.Controls.Add(this.MusicCodeLabel);
            this.OptionsGroupBox.Location = new System.Drawing.Point(12, 215);
            this.OptionsGroupBox.Name = "OptionsGroupBox";
            this.OptionsGroupBox.Size = new System.Drawing.Size(447, 372);
            this.OptionsGroupBox.TabIndex = 2;
            this.OptionsGroupBox.TabStop = false;
            this.OptionsGroupBox.Text = "Metadata && Charts";
            // 
            // SoundEffectsCheckBox
            // 
            this.SoundEffectsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SoundEffectsCheckBox.AutoSize = true;
            this.SoundEffectsCheckBox.Checked = true;
            this.SoundEffectsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SoundEffectsCheckBox.Location = new System.Drawing.Point(17, 119);
            this.SoundEffectsCheckBox.Name = "SoundEffectsCheckBox";
            this.SoundEffectsCheckBox.Size = new System.Drawing.Size(93, 17);
            this.SoundEffectsCheckBox.TabIndex = 6;
            this.SoundEffectsCheckBox.Text = "Sound Effects";
            this.SoundEffectsCheckBox.UseVisualStyleBackColor = true;
            this.SoundEffectsCheckBox.CheckedChanged += new System.EventHandler(this.OnSoundEffectsCheckBoxCheckedChanged);
            // 
            // CameraEffectsCheckBox
            // 
            this.CameraEffectsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CameraEffectsCheckBox.AutoSize = true;
            this.CameraEffectsCheckBox.Checked = true;
            this.CameraEffectsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CameraEffectsCheckBox.Location = new System.Drawing.Point(235, 119);
            this.CameraEffectsCheckBox.Name = "CameraEffectsCheckBox";
            this.CameraEffectsCheckBox.Size = new System.Drawing.Size(98, 17);
            this.CameraEffectsCheckBox.TabIndex = 7;
            this.CameraEffectsCheckBox.Text = "Camera Effects";
            this.CameraEffectsCheckBox.UseVisualStyleBackColor = true;
            this.CameraEffectsCheckBox.CheckedChanged += new System.EventHandler(this.OnCameraEffectsCheckBoxCheckedChanged);
            // 
            // CameraEffectsGroupBox
            // 
            this.CameraEffectsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CameraEffectsGroupBox.Controls.Add(this.CameraMappingButton);
            this.CameraEffectsGroupBox.Controls.Add(this.CameraEffectsCheckedListBox);
            this.CameraEffectsGroupBox.Location = new System.Drawing.Point(228, 121);
            this.CameraEffectsGroupBox.Name = "CameraEffectsGroupBox";
            this.CameraEffectsGroupBox.Size = new System.Drawing.Size(211, 168);
            this.CameraEffectsGroupBox.TabIndex = 9;
            this.CameraEffectsGroupBox.TabStop = false;
            // 
            // CameraMappingButton
            // 
            this.CameraMappingButton.Location = new System.Drawing.Point(6, 134);
            this.CameraMappingButton.Name = "CameraMappingButton";
            this.CameraMappingButton.Size = new System.Drawing.Size(199, 28);
            this.CameraMappingButton.TabIndex = 1;
            this.CameraMappingButton.Text = "Configure Mappings..";
            this.CameraMappingButton.UseVisualStyleBackColor = true;
            this.CameraMappingButton.Click += new System.EventHandler(this.OnMappingButtonClick);
            // 
            // CameraEffectsCheckedListBox
            // 
            this.CameraEffectsCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            this.CameraEffectsCheckedListBox.CheckOnClick = true;
            this.CameraEffectsCheckedListBox.FormattingEnabled = true;
            this.CameraEffectsCheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.CameraEffectsCheckedListBox.Name = "CameraEffectsCheckedListBox";
            this.CameraEffectsCheckedListBox.ScrollAlwaysVisible = true;
            this.CameraEffectsCheckedListBox.Size = new System.Drawing.Size(199, 109);
            this.CameraEffectsCheckedListBox.TabIndex = 0;
            this.CameraEffectsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnCameraEffectsCheckedListBoxItemCheck);
            // 
            // TracksGroupBox
            // 
            this.TracksGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TracksGroupBox.Controls.Add(this.TrackFXRCheckBox);
            this.TracksGroupBox.Controls.Add(this.TrackFXLCheckBox);
            this.TracksGroupBox.Controls.Add(this.TrackVolRCheckBox);
            this.TracksGroupBox.Controls.Add(this.TrackButtonDCheckBox);
            this.TracksGroupBox.Controls.Add(this.TrackButtonCCheckBox);
            this.TracksGroupBox.Controls.Add(this.TrackButtonBCheckBox);
            this.TracksGroupBox.Controls.Add(this.TrackButtonACheckBox);
            this.TracksGroupBox.Controls.Add(this.TrackVolLCheckBox);
            this.TracksGroupBox.Location = new System.Drawing.Point(10, 295);
            this.TracksGroupBox.Name = "TracksGroupBox";
            this.TracksGroupBox.Size = new System.Drawing.Size(429, 68);
            this.TracksGroupBox.TabIndex = 10;
            this.TracksGroupBox.TabStop = false;
            this.TracksGroupBox.Text = "Tracks";
            // 
            // TrackFXRCheckBox
            // 
            this.TrackFXRCheckBox.AutoSize = true;
            this.TrackFXRCheckBox.Checked = true;
            this.TrackFXRCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackFXRCheckBox.Location = new System.Drawing.Point(236, 42);
            this.TrackFXRCheckBox.Name = "TrackFXRCheckBox";
            this.TrackFXRCheckBox.Size = new System.Drawing.Size(50, 17);
            this.TrackFXRCheckBox.TabIndex = 7;
            this.TrackFXRCheckBox.Text = "FX-R";
            this.TrackFXRCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackFXLCheckBox
            // 
            this.TrackFXLCheckBox.AutoSize = true;
            this.TrackFXLCheckBox.Checked = true;
            this.TrackFXLCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackFXLCheckBox.Location = new System.Drawing.Point(124, 42);
            this.TrackFXLCheckBox.Name = "TrackFXLCheckBox";
            this.TrackFXLCheckBox.Size = new System.Drawing.Size(48, 17);
            this.TrackFXLCheckBox.TabIndex = 6;
            this.TrackFXLCheckBox.Text = "FX-L";
            this.TrackFXLCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackVolRCheckBox
            // 
            this.TrackVolRCheckBox.AutoSize = true;
            this.TrackVolRCheckBox.Checked = true;
            this.TrackVolRCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackVolRCheckBox.Location = new System.Drawing.Point(331, 19);
            this.TrackVolRCheckBox.Name = "TrackVolRCheckBox";
            this.TrackVolRCheckBox.Size = new System.Drawing.Size(52, 17);
            this.TrackVolRCheckBox.TabIndex = 5;
            this.TrackVolRCheckBox.Text = "Vol-R";
            this.TrackVolRCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackButtonDCheckBox
            // 
            this.TrackButtonDCheckBox.AutoSize = true;
            this.TrackButtonDCheckBox.Checked = true;
            this.TrackButtonDCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackButtonDCheckBox.Location = new System.Drawing.Point(274, 19);
            this.TrackButtonDCheckBox.Name = "TrackButtonDCheckBox";
            this.TrackButtonDCheckBox.Size = new System.Drawing.Size(51, 17);
            this.TrackButtonDCheckBox.TabIndex = 4;
            this.TrackButtonDCheckBox.Text = "BT-D";
            this.TrackButtonDCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackButtonCCheckBox
            // 
            this.TrackButtonCCheckBox.AutoSize = true;
            this.TrackButtonCCheckBox.Checked = true;
            this.TrackButtonCCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackButtonCCheckBox.Location = new System.Drawing.Point(218, 19);
            this.TrackButtonCCheckBox.Name = "TrackButtonCCheckBox";
            this.TrackButtonCCheckBox.Size = new System.Drawing.Size(50, 17);
            this.TrackButtonCCheckBox.TabIndex = 3;
            this.TrackButtonCCheckBox.Text = "BT-C";
            this.TrackButtonCCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackButtonBCheckBox
            // 
            this.TrackButtonBCheckBox.AutoSize = true;
            this.TrackButtonBCheckBox.Checked = true;
            this.TrackButtonBCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackButtonBCheckBox.Location = new System.Drawing.Point(162, 19);
            this.TrackButtonBCheckBox.Name = "TrackButtonBCheckBox";
            this.TrackButtonBCheckBox.Size = new System.Drawing.Size(50, 17);
            this.TrackButtonBCheckBox.TabIndex = 2;
            this.TrackButtonBCheckBox.Text = "BT-B";
            this.TrackButtonBCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackButtonACheckBox
            // 
            this.TrackButtonACheckBox.AutoSize = true;
            this.TrackButtonACheckBox.Checked = true;
            this.TrackButtonACheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackButtonACheckBox.Location = new System.Drawing.Point(106, 19);
            this.TrackButtonACheckBox.Name = "TrackButtonACheckBox";
            this.TrackButtonACheckBox.Size = new System.Drawing.Size(50, 17);
            this.TrackButtonACheckBox.TabIndex = 1;
            this.TrackButtonACheckBox.Text = "BT-A";
            this.TrackButtonACheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackVolLCheckBox
            // 
            this.TrackVolLCheckBox.AutoSize = true;
            this.TrackVolLCheckBox.Checked = true;
            this.TrackVolLCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackVolLCheckBox.Location = new System.Drawing.Point(50, 19);
            this.TrackVolLCheckBox.Name = "TrackVolLCheckBox";
            this.TrackVolLCheckBox.Size = new System.Drawing.Size(50, 17);
            this.TrackVolLCheckBox.TabIndex = 0;
            this.TrackVolLCheckBox.Text = "Vol-L";
            this.TrackVolLCheckBox.UseVisualStyleBackColor = true;
            // 
            // SoundEffectGroupBox
            // 
            this.SoundEffectGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SoundEffectGroupBox.Controls.Add(this.SoundMappingButton);
            this.SoundEffectGroupBox.Controls.Add(this.SoundEffectsCheckedListBox);
            this.SoundEffectGroupBox.Location = new System.Drawing.Point(10, 121);
            this.SoundEffectGroupBox.Name = "SoundEffectGroupBox";
            this.SoundEffectGroupBox.Size = new System.Drawing.Size(212, 168);
            this.SoundEffectGroupBox.TabIndex = 8;
            this.SoundEffectGroupBox.TabStop = false;
            // 
            // SoundMappingButton
            // 
            this.SoundMappingButton.Location = new System.Drawing.Point(6, 134);
            this.SoundMappingButton.Name = "SoundMappingButton";
            this.SoundMappingButton.Size = new System.Drawing.Size(200, 28);
            this.SoundMappingButton.TabIndex = 1;
            this.SoundMappingButton.Text = "Configure Mappings..";
            this.SoundMappingButton.UseVisualStyleBackColor = true;
            this.SoundMappingButton.Click += new System.EventHandler(this.OnMappingButtonClick);
            // 
            // SoundEffectsCheckedListBox
            // 
            this.SoundEffectsCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            this.SoundEffectsCheckedListBox.CheckOnClick = true;
            this.SoundEffectsCheckedListBox.FormattingEnabled = true;
            this.SoundEffectsCheckedListBox.Location = new System.Drawing.Point(6, 19);
            this.SoundEffectsCheckedListBox.Name = "SoundEffectsCheckedListBox";
            this.SoundEffectsCheckedListBox.ScrollAlwaysVisible = true;
            this.SoundEffectsCheckedListBox.Size = new System.Drawing.Size(200, 109);
            this.SoundEffectsCheckedListBox.TabIndex = 0;
            this.SoundEffectsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.OnSoundEffectsCheckedListBoxItemCheck);
            // 
            // VersionDropDown
            // 
            this.VersionDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.VersionDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VersionDropDown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionDropDown.FormattingEnabled = true;
            this.VersionDropDown.Items.AddRange(new object[] { "Sound Voltex: Booth", "Sound Voltex II: Infinite Infection", "Sound Voltex III: Gravity Wars", "Sound Voltex IV: Heavenly Haven", "Sound Voltex V: Vivid Wave", "Sound Voltex VI: Exceed Gear" });
            this.VersionDropDown.Location = new System.Drawing.Point(90, 42);
            this.VersionDropDown.Name = "VersionDropDown";
            this.VersionDropDown.Size = new System.Drawing.Size(247, 21);
            this.VersionDropDown.TabIndex = 2;
            // 
            // InfVerDropDown
            // 
            this.InfVerDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfVerDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InfVerDropDown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfVerDropDown.FormattingEnabled = true;
            this.InfVerDropDown.Items.AddRange(new object[] { "MXM", "INF", "GRV", "HVN", "VVD", "XCD" });
            this.InfVerDropDown.Location = new System.Drawing.Point(343, 42);
            this.InfVerDropDown.Name = "InfVerDropDown";
            this.InfVerDropDown.Size = new System.Drawing.Size(96, 21);
            this.InfVerDropDown.TabIndex = 3;
            this.InfVerDropDown.SelectedIndexChanged += new System.EventHandler(this.OnInfVerDropDownSelectedIndexChanged);
            // 
            // BackgroundDropDown
            // 
            this.BackgroundDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BackgroundDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BackgroundDropDown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackgroundDropDown.FormattingEnabled = true;
            this.BackgroundDropDown.Items.AddRange(new object[] { "00", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "18", "19", "27", "29", "30", "31", "34", "36", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "53", "54", "57", "58", "59", "60", "61", "63", "65", "66", "67", "68", "69", "70", "71", "72", "73", "74", "75", "76", "77", "78", "79", "80", "81", "82", "83", "84", "86", "87", "88", "89", "90", "91", "92", "93", "92", "95", "96", "97", "98" });
            this.BackgroundDropDown.Location = new System.Drawing.Point(90, 65);
            this.BackgroundDropDown.Name = "BackgroundDropDown";
            this.BackgroundDropDown.Size = new System.Drawing.Size(349, 21);
            this.BackgroundDropDown.TabIndex = 4;
            this.BackgroundDropDown.SelectedIndexChanged += new System.EventHandler(this.OnBackgroundDropDownSelectedIndexChanged);
            // 
            // BackgroundLabel
            // 
            this.BackgroundLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BackgroundLabel.Location = new System.Drawing.Point(10, 65);
            this.BackgroundLabel.Name = "BackgroundLabel";
            this.BackgroundLabel.Size = new System.Drawing.Size(74, 20);
            this.BackgroundLabel.TabIndex = 0;
            this.BackgroundLabel.Text = "Background";
            this.BackgroundLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RealignOffsetCheckBox
            // 
            this.RealignOffsetCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RealignOffsetCheckBox.AutoSize = true;
            this.RealignOffsetCheckBox.Location = new System.Drawing.Point(90, 91);
            this.RealignOffsetCheckBox.Name = "RealignOffsetCheckBox";
            this.RealignOffsetCheckBox.Size = new System.Drawing.Size(195, 17);
            this.RealignOffsetCheckBox.TabIndex = 5;
            this.RealignOffsetCheckBox.Text = "Adapt Start Music Offset (Unstable!)";
            this.RealignOffsetCheckBox.UseVisualStyleBackColor = true;
            // 
            // AsciiAutoCheckBox
            // 
            this.AsciiAutoCheckBox.AutoSize = true;
            this.AsciiAutoCheckBox.Checked = true;
            this.AsciiAutoCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AsciiAutoCheckBox.Location = new System.Drawing.Point(391, 21);
            this.AsciiAutoCheckBox.Name = "AsciiAutoCheckBox";
            this.AsciiAutoCheckBox.Size = new System.Drawing.Size(48, 17);
            this.AsciiAutoCheckBox.TabIndex = 1;
            this.AsciiAutoCheckBox.Text = "Auto";
            this.AsciiAutoCheckBox.UseVisualStyleBackColor = true;
            this.AsciiAutoCheckBox.CheckedChanged += new System.EventHandler(this.OnAsciiAutoCheckBoxCheckedChanged);
            // 
            // InfVerLabel
            // 
            this.InfVerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.InfVerLabel.Location = new System.Drawing.Point(10, 42);
            this.InfVerLabel.Name = "InfVerLabel";
            this.InfVerLabel.Size = new System.Drawing.Size(74, 20);
            this.InfVerLabel.TabIndex = 0;
            this.InfVerLabel.Text = "Version";
            this.InfVerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MeasureLabel
            // 
            this.MeasureLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MeasureLabel.Location = new System.Drawing.Point(10, 88);
            this.MeasureLabel.Name = "MeasureLabel";
            this.MeasureLabel.Size = new System.Drawing.Size(74, 20);
            this.MeasureLabel.TabIndex = 0;
            this.MeasureLabel.Text = "Offset";
            this.MeasureLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AsciiTextBox
            // 
            this.AsciiTextBox.Location = new System.Drawing.Point(90, 20);
            this.AsciiTextBox.Name = "AsciiTextBox";
            this.AsciiTextBox.ReadOnly = true;
            this.AsciiTextBox.Size = new System.Drawing.Size(295, 20);
            this.AsciiTextBox.TabIndex = 0;
            // 
            // MusicCodeLabel
            // 
            this.MusicCodeLabel.Location = new System.Drawing.Point(10, 19);
            this.MusicCodeLabel.Name = "MusicCodeLabel";
            this.MusicCodeLabel.Size = new System.Drawing.Size(74, 20);
            this.MusicCodeLabel.TabIndex = 0;
            this.MusicCodeLabel.Text = "Music Code";
            this.MusicCodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CancelConvertButton
            // 
            this.CancelConvertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelConvertButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelConvertButton.Location = new System.Drawing.Point(343, 687);
            this.CancelConvertButton.Name = "CancelConvertButton";
            this.CancelConvertButton.Size = new System.Drawing.Size(116, 28);
            this.CancelConvertButton.TabIndex = 4;
            this.CancelConvertButton.Text = "Cancel";
            this.CancelConvertButton.UseVisualStyleBackColor = true;
            this.CancelConvertButton.Click += new System.EventHandler(this.OnCancelConvertButtonClick);
            // 
            // ProcessConvertButton
            // 
            this.ProcessConvertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessConvertButton.Location = new System.Drawing.Point(221, 687);
            this.ProcessConvertButton.Name = "ProcessConvertButton";
            this.ProcessConvertButton.Size = new System.Drawing.Size(116, 28);
            this.ProcessConvertButton.TabIndex = 3;
            this.ProcessConvertButton.Text = "Continue";
            this.ProcessConvertButton.UseVisualStyleBackColor = true;
            this.ProcessConvertButton.Click += new System.EventHandler(this.OnProcessConvertButtonClick);
            // 
            // TargetLabel
            // 
            this.TargetLabel.AutoSize = true;
            this.TargetLabel.Location = new System.Drawing.Point(9, 20);
            this.TargetLabel.Name = "TargetLabel";
            this.TargetLabel.Size = new System.Drawing.Size(38, 13);
            this.TargetLabel.TabIndex = 21;
            this.TargetLabel.Text = "Target";
            this.TargetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MusicGroupBox
            // 
            this.MusicGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.MusicGroupBox.Controls.Add(this.FormatLabel);
            this.MusicGroupBox.Controls.Add(this.PreviewTimePicker);
            this.MusicGroupBox.Controls.Add(this.MusicFormatDropDown);
            this.MusicGroupBox.Controls.Add(this.PreviewOffsetLabel);
            this.MusicGroupBox.Location = new System.Drawing.Point(12, 593);
            this.MusicGroupBox.Name = "MusicGroupBox";
            this.MusicGroupBox.Size = new System.Drawing.Size(447, 76);
            this.MusicGroupBox.TabIndex = 22;
            this.MusicGroupBox.TabStop = false;
            this.MusicGroupBox.Text = "Music Format && Converter";
            // 
            // FormatLabel
            // 
            this.FormatLabel.Location = new System.Drawing.Point(7, 20);
            this.FormatLabel.Name = "FormatLabel";
            this.FormatLabel.Size = new System.Drawing.Size(110, 20);
            this.FormatLabel.TabIndex = 23;
            this.FormatLabel.Text = "Format";
            this.FormatLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PreviewTimePicker
            // 
            this.PreviewTimePicker.CustomFormat = "mm:ss";
            this.PreviewTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.PreviewTimePicker.Location = new System.Drawing.Point(122, 45);
            this.PreviewTimePicker.Name = "PreviewTimePicker";
            this.PreviewTimePicker.ShowUpDown = true;
            this.PreviewTimePicker.Size = new System.Drawing.Size(315, 20);
            this.PreviewTimePicker.TabIndex = 23;
            this.PreviewTimePicker.ValueChanged += new System.EventHandler(this.OnPreviewTimePickerValueChanged);
            // 
            // MusicFormatDropDown
            // 
            this.MusicFormatDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MusicFormatDropDown.FormattingEnabled = true;
            this.MusicFormatDropDown.Items.AddRange(new object[] { "Legacy Format (.2dx)" });
            this.MusicFormatDropDown.Location = new System.Drawing.Point(122, 19);
            this.MusicFormatDropDown.Name = "MusicFormatDropDown";
            this.MusicFormatDropDown.Size = new System.Drawing.Size(316, 21);
            this.MusicFormatDropDown.TabIndex = 0;
            this.MusicFormatDropDown.SelectedIndexChanged += new System.EventHandler(this.OnMusicFormatDropDownSelectedIndexChanged);
            // 
            // PreviewOffsetLabel
            // 
            this.PreviewOffsetLabel.Location = new System.Drawing.Point(7, 45);
            this.PreviewOffsetLabel.Name = "PreviewOffsetLabel";
            this.PreviewOffsetLabel.Size = new System.Drawing.Size(110, 20);
            this.PreviewOffsetLabel.TabIndex = 8;
            this.PreviewOffsetLabel.Text = "Preview TimeStamp";
            this.PreviewOffsetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConverterForm
            // 
            this.AcceptButton = this.ProcessConvertButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelConvertButton;
            this.ClientSize = new System.Drawing.Size(471, 724);
            this.Controls.Add(this.MusicGroupBox);
            this.Controls.Add(this.TargetLabel);
            this.Controls.Add(this.CancelConvertButton);
            this.Controls.Add(this.ProcessConvertButton);
            this.Controls.Add(this.OptionsGroupBox);
            this.Controls.Add(this.PathTextBox);
            this.Controls.Add(this.LevelGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConverterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Music";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.OnHelpButtonClicked);
            this.Load += new System.EventHandler(this.OnConverterFormLoad);
            this.LevelGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.JacketInfPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketExhPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketAdvPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketNovPictureBox)).EndInit();
            this.OptionsGroupBox.ResumeLayout(false);
            this.OptionsGroupBox.PerformLayout();
            this.CameraEffectsGroupBox.ResumeLayout(false);
            this.TracksGroupBox.ResumeLayout(false);
            this.TracksGroupBox.PerformLayout();
            this.SoundEffectGroupBox.ResumeLayout(false);
            this.MusicGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label FormatLabel;

        private System.Windows.Forms.CheckBox TrackFXLCheckBox;
        private System.Windows.Forms.CheckBox TrackFXRCheckBox;

        private System.Windows.Forms.DateTimePicker PreviewTimePicker;

        private System.Windows.Forms.Label PreviewOffsetLabel;

        private System.Windows.Forms.CheckBox SoundEffectsCheckBox;
        private System.Windows.Forms.CheckedListBox SoundEffectsCheckedListBox;
        private System.Windows.Forms.Button SoundMappingButton;
        private System.Windows.Forms.GroupBox MusicGroupBox;
        private System.Windows.Forms.ComboBox MusicFormatDropDown;

        private System.Windows.Forms.CheckBox CameraEffectsCheckBox;

        private System.Windows.Forms.Button CameraMappingButton;

        private System.Windows.Forms.CheckedListBox CameraEffectsCheckedListBox;

        private System.Windows.Forms.CheckBox TrackButtonBCheckBox;
        private System.Windows.Forms.CheckBox TrackButtonCCheckBox;
        private System.Windows.Forms.CheckBox TrackButtonDCheckBox;
        private System.Windows.Forms.CheckBox TrackVolRCheckBox;

        private System.Windows.Forms.GroupBox CameraEffectsGroupBox;

        private System.Windows.Forms.GroupBox SoundEffectGroupBox;
        private System.Windows.Forms.GroupBox TracksGroupBox;

        #endregion

        private System.Windows.Forms.GroupBox LevelGroupBox;
        private System.Windows.Forms.Button InfEditButton;
        private System.Windows.Forms.Button ExhEditButton;
        private System.Windows.Forms.Button AdvEditButton;
        private System.Windows.Forms.Button NovEditButton;
        private System.Windows.Forms.PictureBox JacketInfPictureBox;
        private System.Windows.Forms.PictureBox JacketExhPictureBox;
        private System.Windows.Forms.PictureBox JacketAdvPictureBox;
        private System.Windows.Forms.PictureBox JacketNovPictureBox;
        private System.Windows.Forms.TextBox PathTextBox;
        private System.Windows.Forms.GroupBox OptionsGroupBox;
        private System.Windows.Forms.Label MusicCodeLabel;
        private System.Windows.Forms.Label MeasureLabel;
        private System.Windows.Forms.TextBox AsciiTextBox;
        private System.Windows.Forms.CheckBox TrackButtonACheckBox;
        private System.Windows.Forms.CheckBox TrackVolLCheckBox;
        private System.Windows.Forms.Label InfVerLabel;
        private System.Windows.Forms.CheckBox AsciiAutoCheckBox;
        private System.Windows.Forms.Button CancelConvertButton;
        private System.Windows.Forms.Button ProcessConvertButton;
        private System.Windows.Forms.CheckBox RealignOffsetCheckBox;
        private System.Windows.Forms.Label BackgroundLabel;
        private System.Windows.Forms.ComboBox BackgroundDropDown;
        private System.Windows.Forms.ComboBox InfVerDropDown;
        private System.Windows.Forms.ComboBox VersionDropDown;
        private System.Windows.Forms.Label TargetLabel;
    }
}