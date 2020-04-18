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
            this.SlamImpactCheckBox = new System.Windows.Forms.CheckBox();
            this.VersionDropDown = new System.Windows.Forms.ComboBox();
            this.InfVerDropDown = new System.Windows.Forms.ComboBox();
            this.BackgroundDropDown = new System.Windows.Forms.ComboBox();
            this.BackgroundLabel = new System.Windows.Forms.Label();
            this.RealignOffsetCheckBox = new System.Windows.Forms.CheckBox();
            this.AsciiAutoCheckBox = new System.Windows.Forms.CheckBox();
            this.InfVerLabel = new System.Windows.Forms.Label();
            this.CameraCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackButtonCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackLaserCheckBox = new System.Windows.Forms.CheckBox();
            this.TrackLabel = new System.Windows.Forms.Label();
            this.EffectsLabel = new System.Windows.Forms.Label();
            this.MeasureLabel = new System.Windows.Forms.Label();
            this.AsciiTextBox = new System.Windows.Forms.TextBox();
            this.LongFxCheckBox = new System.Windows.Forms.CheckBox();
            this.ChipFxCheckBox = new System.Windows.Forms.CheckBox();
            this.MusicCodeLabel = new System.Windows.Forms.Label();
            this.CancelConvertButton = new System.Windows.Forms.Button();
            this.ProcessConvertButton = new System.Windows.Forms.Button();
            this.TargetLabel = new System.Windows.Forms.Label();
            this.LevelGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JacketInfPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketExhPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketAdvPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.JacketNovPictureBox)).BeginInit();
            this.OptionsGroupBox.SuspendLayout();
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
            this.LevelGroupBox.TabIndex = 15;
            this.LevelGroupBox.TabStop = false;
            this.LevelGroupBox.Text = "Levels";
            // 
            // InfEditButton
            // 
            this.InfEditButton.Location = new System.Drawing.Point(331, 133);
            this.InfEditButton.Name = "InfEditButton";
            this.InfEditButton.Size = new System.Drawing.Size(108, 26);
            this.InfEditButton.TabIndex = 18;
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
            this.ExhEditButton.TabIndex = 17;
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
            this.AdvEditButton.TabIndex = 16;
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
            this.NovEditButton.TabIndex = 15;
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
            this.PathTextBox.TabIndex = 16;
            // 
            // OptionsGroupBox
            // 
            this.OptionsGroupBox.Controls.Add(this.SlamImpactCheckBox);
            this.OptionsGroupBox.Controls.Add(this.VersionDropDown);
            this.OptionsGroupBox.Controls.Add(this.InfVerDropDown);
            this.OptionsGroupBox.Controls.Add(this.BackgroundDropDown);
            this.OptionsGroupBox.Controls.Add(this.BackgroundLabel);
            this.OptionsGroupBox.Controls.Add(this.RealignOffsetCheckBox);
            this.OptionsGroupBox.Controls.Add(this.AsciiAutoCheckBox);
            this.OptionsGroupBox.Controls.Add(this.InfVerLabel);
            this.OptionsGroupBox.Controls.Add(this.CameraCheckBox);
            this.OptionsGroupBox.Controls.Add(this.TrackButtonCheckBox);
            this.OptionsGroupBox.Controls.Add(this.TrackLaserCheckBox);
            this.OptionsGroupBox.Controls.Add(this.TrackLabel);
            this.OptionsGroupBox.Controls.Add(this.EffectsLabel);
            this.OptionsGroupBox.Controls.Add(this.MeasureLabel);
            this.OptionsGroupBox.Controls.Add(this.AsciiTextBox);
            this.OptionsGroupBox.Controls.Add(this.LongFxCheckBox);
            this.OptionsGroupBox.Controls.Add(this.ChipFxCheckBox);
            this.OptionsGroupBox.Controls.Add(this.MusicCodeLabel);
            this.OptionsGroupBox.Location = new System.Drawing.Point(12, 215);
            this.OptionsGroupBox.Name = "OptionsGroupBox";
            this.OptionsGroupBox.Size = new System.Drawing.Size(447, 170);
            this.OptionsGroupBox.TabIndex = 1;
            this.OptionsGroupBox.TabStop = false;
            this.OptionsGroupBox.Text = "Options";
            // 
            // SlamImpactCheckBox
            // 
            this.SlamImpactCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SlamImpactCheckBox.AutoSize = true;
            this.SlamImpactCheckBox.Checked = true;
            this.SlamImpactCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SlamImpactCheckBox.Location = new System.Drawing.Point(293, 114);
            this.SlamImpactCheckBox.Name = "SlamImpactCheckBox";
            this.SlamImpactCheckBox.Size = new System.Drawing.Size(84, 17);
            this.SlamImpactCheckBox.TabIndex = 20;
            this.SlamImpactCheckBox.Text = "Slam Impact";
            this.SlamImpactCheckBox.UseVisualStyleBackColor = true;
            // 
            // VersionDropDown
            // 
            this.VersionDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VersionDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VersionDropDown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionDropDown.FormattingEnabled = true;
            this.VersionDropDown.Items.AddRange(new object[] {
            "Sound Voltex: Booth",
            "Sound Voltex II: Infinite Infection",
            "Sound Voltex III: Gravity Wars",
            "Sound Voltex IV: Heavenly Haven",
            "Sound Voltex V: VividWave"});
            this.VersionDropDown.Location = new System.Drawing.Point(90, 42);
            this.VersionDropDown.Name = "VersionDropDown";
            this.VersionDropDown.Size = new System.Drawing.Size(247, 21);
            this.VersionDropDown.TabIndex = 19;
            // 
            // InfVerDropDown
            // 
            this.InfVerDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.InfVerDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.InfVerDropDown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfVerDropDown.FormattingEnabled = true;
            this.InfVerDropDown.Items.AddRange(new object[] {
            "MXM",
            "INF",
            "GRV",
            "HVN",
            "VVD"});
            this.InfVerDropDown.Location = new System.Drawing.Point(343, 42);
            this.InfVerDropDown.Name = "InfVerDropDown";
            this.InfVerDropDown.Size = new System.Drawing.Size(96, 21);
            this.InfVerDropDown.TabIndex = 18;
            this.InfVerDropDown.SelectedIndexChanged += new System.EventHandler(this.OnInfVerDropDownSelectedIndexChanged);
            // 
            // BackgroundDropDown
            // 
            this.BackgroundDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BackgroundDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BackgroundDropDown.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackgroundDropDown.FormattingEnabled = true;
            this.BackgroundDropDown.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "18",
            "19",
            "27",
            "29",
            "30",
            "31",
            "34",
            "36",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "53",
            "54",
            "57",
            "58",
            "59",
            "60",
            "61",
            "63",
            "65",
            "66",
            "67",
            "68",
            "69",
            "70",
            "71",
            "72",
            "73",
            "74",
            "75",
            "76",
            "77",
            "78",
            "79",
            "80",
            "81"});
            this.BackgroundDropDown.Location = new System.Drawing.Point(90, 65);
            this.BackgroundDropDown.Name = "BackgroundDropDown";
            this.BackgroundDropDown.Size = new System.Drawing.Size(349, 21);
            this.BackgroundDropDown.TabIndex = 17;
            this.BackgroundDropDown.SelectedIndexChanged += new System.EventHandler(this.OnBackgroundDropDownSelectedIndexChanged);
            // 
            // BackgroundLabel
            // 
            this.BackgroundLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BackgroundLabel.Location = new System.Drawing.Point(10, 65);
            this.BackgroundLabel.Name = "BackgroundLabel";
            this.BackgroundLabel.Size = new System.Drawing.Size(74, 20);
            this.BackgroundLabel.TabIndex = 16;
            this.BackgroundLabel.Text = "Background";
            this.BackgroundLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // RealignOffsetCheckBox
            // 
            this.RealignOffsetCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RealignOffsetCheckBox.AutoSize = true;
            this.RealignOffsetCheckBox.Location = new System.Drawing.Point(90, 91);
            this.RealignOffsetCheckBox.Name = "RealignOffsetCheckBox";
            this.RealignOffsetCheckBox.Size = new System.Drawing.Size(141, 17);
            this.RealignOffsetCheckBox.TabIndex = 15;
            this.RealignOffsetCheckBox.Text = "Adapt Start Music Offset";
            this.RealignOffsetCheckBox.UseVisualStyleBackColor = true;
            // 
            // AsciiAutoCheckBox
            // 
            this.AsciiAutoCheckBox.AutoSize = true;
            this.AsciiAutoCheckBox.Checked = true;
            this.AsciiAutoCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AsciiAutoCheckBox.Location = new System.Drawing.Point(391, 23);
            this.AsciiAutoCheckBox.Name = "AsciiAutoCheckBox";
            this.AsciiAutoCheckBox.Size = new System.Drawing.Size(48, 17);
            this.AsciiAutoCheckBox.TabIndex = 14;
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
            this.InfVerLabel.TabIndex = 12;
            this.InfVerLabel.Text = "Version";
            this.InfVerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CameraCheckBox
            // 
            this.CameraCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CameraCheckBox.AutoSize = true;
            this.CameraCheckBox.Checked = true;
            this.CameraCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CameraCheckBox.Location = new System.Drawing.Point(225, 114);
            this.CameraCheckBox.Name = "CameraCheckBox";
            this.CameraCheckBox.Size = new System.Drawing.Size(62, 17);
            this.CameraCheckBox.TabIndex = 5;
            this.CameraCheckBox.Text = "Camera";
            this.CameraCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackButtonCheckBox
            // 
            this.TrackButtonCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TrackButtonCheckBox.AutoSize = true;
            this.TrackButtonCheckBox.Checked = true;
            this.TrackButtonCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackButtonCheckBox.Location = new System.Drawing.Point(157, 137);
            this.TrackButtonCheckBox.Name = "TrackButtonCheckBox";
            this.TrackButtonCheckBox.Size = new System.Drawing.Size(62, 17);
            this.TrackButtonCheckBox.TabIndex = 7;
            this.TrackButtonCheckBox.Text = "Buttons";
            this.TrackButtonCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackLaserCheckBox
            // 
            this.TrackLaserCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TrackLaserCheckBox.AutoSize = true;
            this.TrackLaserCheckBox.Checked = true;
            this.TrackLaserCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TrackLaserCheckBox.Location = new System.Drawing.Point(90, 137);
            this.TrackLaserCheckBox.Name = "TrackLaserCheckBox";
            this.TrackLaserCheckBox.Size = new System.Drawing.Size(57, 17);
            this.TrackLaserCheckBox.TabIndex = 6;
            this.TrackLaserCheckBox.Text = "Lasers";
            this.TrackLaserCheckBox.UseVisualStyleBackColor = true;
            // 
            // TrackLabel
            // 
            this.TrackLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TrackLabel.Location = new System.Drawing.Point(10, 134);
            this.TrackLabel.Name = "TrackLabel";
            this.TrackLabel.Size = new System.Drawing.Size(74, 20);
            this.TrackLabel.TabIndex = 11;
            this.TrackLabel.Text = "Tracks";
            this.TrackLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EffectsLabel
            // 
            this.EffectsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EffectsLabel.Location = new System.Drawing.Point(10, 111);
            this.EffectsLabel.Name = "EffectsLabel";
            this.EffectsLabel.Size = new System.Drawing.Size(74, 20);
            this.EffectsLabel.TabIndex = 10;
            this.EffectsLabel.Text = "Effects";
            this.EffectsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MeasureLabel
            // 
            this.MeasureLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.MeasureLabel.Location = new System.Drawing.Point(10, 88);
            this.MeasureLabel.Name = "MeasureLabel";
            this.MeasureLabel.Size = new System.Drawing.Size(74, 20);
            this.MeasureLabel.TabIndex = 6;
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
            // LongFxCheckBox
            // 
            this.LongFxCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LongFxCheckBox.AutoSize = true;
            this.LongFxCheckBox.Checked = true;
            this.LongFxCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LongFxCheckBox.Location = new System.Drawing.Point(157, 114);
            this.LongFxCheckBox.Name = "LongFxCheckBox";
            this.LongFxCheckBox.Size = new System.Drawing.Size(64, 17);
            this.LongFxCheckBox.TabIndex = 4;
            this.LongFxCheckBox.Text = "Long Fx";
            this.LongFxCheckBox.UseVisualStyleBackColor = true;
            // 
            // ChipFxCheckBox
            // 
            this.ChipFxCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ChipFxCheckBox.AutoSize = true;
            this.ChipFxCheckBox.Checked = true;
            this.ChipFxCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChipFxCheckBox.Location = new System.Drawing.Point(90, 114);
            this.ChipFxCheckBox.Name = "ChipFxCheckBox";
            this.ChipFxCheckBox.Size = new System.Drawing.Size(61, 17);
            this.ChipFxCheckBox.TabIndex = 3;
            this.ChipFxCheckBox.Text = "Chip Fx";
            this.ChipFxCheckBox.UseVisualStyleBackColor = true;
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
            this.CancelConvertButton.Location = new System.Drawing.Point(343, 401);
            this.CancelConvertButton.Name = "CancelConvertButton";
            this.CancelConvertButton.Size = new System.Drawing.Size(116, 28);
            this.CancelConvertButton.TabIndex = 20;
            this.CancelConvertButton.Text = "Cancel";
            this.CancelConvertButton.UseVisualStyleBackColor = true;
            this.CancelConvertButton.Click += new System.EventHandler(this.OnCancelConvertButtonClick);
            // 
            // ProcessConvertButton
            // 
            this.ProcessConvertButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ProcessConvertButton.Location = new System.Drawing.Point(221, 401);
            this.ProcessConvertButton.Name = "ProcessConvertButton";
            this.ProcessConvertButton.Size = new System.Drawing.Size(116, 28);
            this.ProcessConvertButton.TabIndex = 19;
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
            // ConverterForm
            // 
            this.AcceptButton = this.ProcessConvertButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelConvertButton;
            this.ClientSize = new System.Drawing.Size(471, 441);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

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
        private System.Windows.Forms.CheckBox LongFxCheckBox;
        private System.Windows.Forms.CheckBox ChipFxCheckBox;
        private System.Windows.Forms.Label MusicCodeLabel;
        private System.Windows.Forms.Label MeasureLabel;
        private System.Windows.Forms.TextBox AsciiTextBox;
        private System.Windows.Forms.CheckBox CameraCheckBox;
        private System.Windows.Forms.CheckBox TrackButtonCheckBox;
        private System.Windows.Forms.CheckBox TrackLaserCheckBox;
        private System.Windows.Forms.Label TrackLabel;
        private System.Windows.Forms.Label EffectsLabel;
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
        private System.Windows.Forms.CheckBox SlamImpactCheckBox;
    }
}