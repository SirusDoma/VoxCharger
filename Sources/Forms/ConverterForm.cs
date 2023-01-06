using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace VoxCharger
{
    public enum ConvertMode
    {
        Converter = 0,
        Importer  = 1,
        BulkConverter = 2,
        BulkImporter  = 3
    }

    public partial class ConverterForm : Form
    {
        private enum SoundFxType
        {
            Chip,
            Long,
            Laser
        }

        private class CameraEffectOption
        {
            public Camera.WorkType Work { get; set; }
            public bool SlamImpact { get; set; } = false;
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private class SoundFxOption
        {
            public SoundFxType Type { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        private static readonly Image DummyJacket = VoxCharger.Properties.Resources.jk_dummy_s;
        public static string LastBackground { get; private set; } = "88";

        private readonly CameraEffectOption[] _cameraOptions =
        {
            new CameraEffectOption
            {
                Work = Camera.WorkType.Realize,
                Name = "Realize"
            },
            new CameraEffectOption
            {
                Work = Camera.WorkType.Rotation,
                Name = "CAM_RotX"
            },
            new CameraEffectOption
            {
                Work = Camera.WorkType.Radian,
                Name = "CAM_Radian"
            },
            new CameraEffectOption
            {
                Work = Camera.WorkType.Tilt,
                Name = "Tilt / Tilt Mode"
            },
            new CameraEffectOption
            {
                Work = Camera.WorkType.LaneClear,
                Name = "LaneY"
            },
            new CameraEffectOption
            {
                SlamImpact = true,
                Name = "Slam Impact"
            },
        };

        private readonly SoundFxOption[] _sfxOptions =
        {
            new SoundFxOption
            {
                Type = SoundFxType.Chip,
                Name = "Chip"
            },
            new SoundFxOption
            {
                Type = SoundFxType.Long,
                Name = "Long"
            },
            new SoundFxOption
            {
                Type = SoundFxType.Laser,
                Name = "Laser"
            }
        };

        private string _target;
        private string _defaultAscii;
        private ConvertMode _mode;
        private Dictionary<Difficulty, ChartInfo> _charts = new Dictionary<Difficulty, ChartInfo>();
        private Ksh.Exporter _exporter;
        private bool _updatingAllEffects = false;

        public VoxHeader Result        { get; private set; } = null;
        public VoxHeader[] ResultSet   { get; private set; } = new VoxHeader[0];
        public Ksh.ParseOption Options { get; private set; } = new Ksh.ParseOption();
        public Action Action           { get; private set; } = null;
        public Dictionary<string, Action> ActionSet { get; private set; } = new Dictionary<string, Action>();

        public ConverterForm(string path, ConvertMode convert)
        {
            InitializeComponent();

            _target = path;
            _mode   = convert;

            foreach (var camOpt in _cameraOptions)
                CameraEffectsCheckedListBox.Items.Add(camOpt, camOpt.Work == Camera.WorkType.Realize ? CheckState.Indeterminate : CheckState.Checked);

            foreach (var sfxOpt in _sfxOptions)
                SoundEffectsCheckedListBox.Items.Add(sfxOpt, true);

            // Cancerous code to adjust layout depending what this form going to be
            if (_mode == ConvertMode.Converter)
            {
                MusicCodeLabel.Visible     = false;
                InfVerLabel.Visible        = false;
                BackgroundLabel.Visible    = false;
                BackgroundDropDown.Enabled = BackgroundDropDown.Visible = false;
                MusicGroupBox.Enabled      = MusicGroupBox.Visible      = false;
                LevelGroupBox.Enabled      = LevelGroupBox.Visible      = false;
                AsciiTextBox.Enabled       = AsciiTextBox.Visible       = false;
                AsciiAutoCheckBox.Enabled  = AsciiAutoCheckBox.Visible  = false;
                VersionDropDown.Enabled    = VersionDropDown.Visible    = false;
                InfVerDropDown.Enabled     = InfVerDropDown.Visible     = false;

                int componentHeight      = AsciiTextBox.Height + VersionDropDown.Height + BackgroundDropDown.Height;
                OptionsGroupBox.Location = LevelGroupBox.Location;
                OptionsGroupBox.Height  -= componentHeight;
                Height                  -= LevelGroupBox.Height + componentHeight + MusicGroupBox.Height;

                Text = "Convert Music";
                ProcessConvertButton.Text = "Convert";
                PathTextBox.Text = _target;

                return;
            }
            else if (_mode == ConvertMode.BulkImporter)
            {
                MusicCodeLabel.Visible     = false;
                LevelGroupBox.Enabled      = LevelGroupBox.Visible      = false;
                AsciiTextBox.Enabled       = AsciiTextBox.Visible       = false;
                AsciiAutoCheckBox.Enabled  = AsciiAutoCheckBox.Visible  = false;
                MusicGroupBox.Enabled      = MusicGroupBox.Visible      = false;

                int componentHeight      = AsciiTextBox.Height;
                OptionsGroupBox.Location = LevelGroupBox.Location;
                OptionsGroupBox.Height  -= componentHeight;
                Height                  -= LevelGroupBox.Height + MusicGroupBox.Height + componentHeight;
            }

            MusicGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            PreviewTimePicker.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            MusicFormatDropDown.SelectedIndex = 0;
            MusicFormatDropDown.Enabled = false;

            PathTextBox.Text                 = _target;
            BackgroundDropDown.SelectedItem  = LastBackground;
            VersionDropDown.SelectedIndex    = 5;
            InfVerDropDown.SelectedIndex     = 0;
            ProcessConvertButton.Text        = "Add";
        }

        private void OnConverterFormLoad(object sender, EventArgs e)
        {
            if (_mode != ConvertMode.Importer)
                return;

            try
            {
                var main = new Ksh();
                main.Parse(_target);
 
                Result       = main.ToHeader();
                Result.Id    = AssetManager.GetNextMusicId();
                Result.Ascii = _defaultAscii = AsciiTextBox.Text = Path.GetFileName(Path.GetDirectoryName(_target));
                _exporter     = new Ksh.Exporter(main);

                for (int i = 1; Directory.Exists(AssetManager.GetMusicPath(Result)); i++)
                {
                    if (i >= 100)
                        break; // seriously? stupid input get stupid output

                    Result.Ascii = $"{_defaultAscii}{i:D2}";
                }

                _defaultAscii = AsciiTextBox.Text = Result.Ascii;
                _charts[main.Difficulty] = new ChartInfo(main, main.ToLevelHeader(), _target);
                LoadJacket(_charts[main.Difficulty]);

                // Try to locate another difficulty
                foreach (var lv in Ksh.Exporter.GetCharts(Path.GetDirectoryName(_target), main.Title))
                {
                    // Don't replace main file, there might 2 files with similar meta or another stupid cases
                    if (lv.Key != main.Difficulty)
                        _charts[lv.Key] = lv.Value;
                }

                UpdateLevels();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load ksh chart.\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void OnAsciiAutoCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            AsciiTextBox.ReadOnly = AsciiAutoCheckBox.Checked;
            if (AsciiTextBox.ReadOnly)
                AsciiTextBox.Text = _defaultAscii;
        }

        private void OnBackgroundDropDownSelectedIndexChanged(object sender, EventArgs e)
        {
            LastBackground = BackgroundDropDown.SelectedItem.ToString();
        }

        private void OnInfVerDropDownSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_charts.ContainsKey(Difficulty.Infinite))
                InfEditButton.Text = "--";
            else
                InfEditButton.Text = InfVerDropDown.SelectedItem.ToString();
        }

        private void OnLevelEditButtonClick(object sender, EventArgs e)
        {
            using (var browser = new OpenFileDialog())
            {
                browser.Filter = "KShoot Mania Chart|*.ksh";
                browser.CheckFileExists = true;

                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                var control = (Button)sender;
                if (!Enum.TryParse(control.Tag.ToString(), out Difficulty diff))
                    return;

                try
                {
                    var chart = new Ksh();
                    chart.Parse(browser.FileName);
                    chart.Difficulty = diff; // make sure to replace diff

                    _charts[diff] = new ChartInfo(chart, chart.ToLevelHeader(), browser.FileName);
                    UpdateLevels();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                       $"Failed to load ksh chart.\n{ex.Message}",
                       "Error",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Error
                   );
                }
            }
        }

        private void OnSoundEffectsCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (SoundEffectsCheckBox.CheckState == CheckState.Indeterminate)
                return;

            _updatingAllEffects = true;

            for (int i = 0; i < SoundEffectsCheckedListBox.Items.Count; i++)
            {
                if (SoundEffectsCheckedListBox.GetItemCheckState(i) == CheckState.Indeterminate)
                    continue;

                SoundEffectsCheckedListBox.SetItemChecked(i, SoundEffectsCheckBox.Checked);
            }

            _updatingAllEffects = false;
        }

        private void OnCameraEffectsCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            if (CameraEffectsCheckBox.CheckState == CheckState.Indeterminate)
                return;

            _updatingAllEffects = true;

            for (int i = 0; i < CameraEffectsCheckedListBox.Items.Count; i++)
            {
                if (CameraEffectsCheckedListBox.GetItemCheckState(i) == CheckState.Indeterminate)
                    continue;

                CameraEffectsCheckedListBox.SetItemChecked(i, CameraEffectsCheckBox.Checked);
            }

            _updatingAllEffects = false;
        }

        private void OnSoundEffectsCheckedListBoxItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updatingAllEffects || e.CurrentValue == CheckState.Indeterminate)
                return;

            bool value = e.NewValue == CheckState.Checked || e.NewValue == CheckState.Indeterminate;
            for (int i = 0; i < SoundEffectsCheckedListBox.Items.Count; i++)
            {
                if (i != e.Index && SoundEffectsCheckedListBox.GetItemChecked(i) != value)
                {
                    SoundEffectsCheckBox.CheckState = CheckState.Indeterminate;
                    return;
                }
            }

            SoundEffectsCheckBox.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
        }

        private void OnCameraEffectsCheckedListBoxItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_updatingAllEffects || e.CurrentValue == CheckState.Indeterminate)
            {
                if (e.CurrentValue == CheckState.Indeterminate)
                    e.NewValue = CheckState.Indeterminate;

                return;
            }

            bool value = e.NewValue == CheckState.Checked || e.NewValue == CheckState.Indeterminate;
            for (int i = 0; i < CameraEffectsCheckedListBox.Items.Count; i++)
            {
                if (i != e.Index && CameraEffectsCheckedListBox.GetItemChecked(i) != value)
                {
                    CameraEffectsCheckBox.CheckState = CheckState.Indeterminate;
                    return;
                }
            }

            CameraEffectsCheckBox.CheckState = value ? CheckState.Checked : CheckState.Unchecked;
        }

        private void OnMappingButtonClick(object sender, EventArgs e)
        {
            MessageBox.Show("Custom mapping for Sound & Camera Effects is not supported (yet).",
                "Coming Soon™", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void OnMusicFormatDropDownSelectedIndexChanged(object sender, EventArgs e)
        {
            // Not Implemented
        }

        private void OnBrowseMainToolButtonClick(object sender, EventArgs e)
        {
        }

        private void OnBrowseSecondaryToolButtonClick(object sender, EventArgs e)
        {
            using (var browser = new OpenFileDialog())
            {
                browser.Filter          = $"{PreviewOffsetLabel.Text.Replace(".exe", string.Empty)} | {PreviewOffsetLabel.Text}";
                browser.CheckFileExists = true;
                browser.Title           = "Browse Converter";
            }
        }

        private void OnHelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            using (var help = new HelpForm())
                help.ShowDialog();
        }

        private void OnCancelConvertButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnProcessConvertButtonClick(object sender, EventArgs e)
        {
            bool slamImpact = true;
            var cameraOpts = new Dictionary<Camera.WorkType, bool>();
            for (int i = 0; i < CameraEffectsCheckedListBox.Items.Count; i++)
            {
                if (!(CameraEffectsCheckedListBox.Items[i] is CameraEffectOption opt))
                    continue;

                if (opt.SlamImpact)
                    slamImpact = CameraEffectsCheckedListBox.GetItemChecked(i);
                else
                    cameraOpts[opt.Work] = CameraEffectsCheckedListBox.GetItemChecked(i);
            }

            Options = new Ksh.ParseOption
            {
                RealignOffset = RealignOffsetCheckBox.Checked,
                Camera = new Ksh.ParseOption.CameraOptions
                {
                    SlamImpact   = slamImpact,
                    EnabledWorks = cameraOpts
                },
                SoundFx = new Ksh.ParseOption.SoundFxOptions
                {
                    Chip  = SoundEffectsCheckedListBox.GetItemChecked(0),
                    Long  = SoundEffectsCheckedListBox.GetItemChecked(1),
                    Laser = SoundEffectsCheckedListBox.GetItemChecked(2),
                },
                Track = new Ksh.ParseOption.TrackOptions
                {
                    EnabledLaserTracks = new Dictionary<Event.LaserTrack, bool>
                    {
                        { Event.LaserTrack.Left, TrackVolLCheckBox.Checked },
                        { Event.LaserTrack.Right, TrackVolRCheckBox.Checked }
                    },
                    EnabledButtonTracks = new Dictionary<Event.ButtonTrack, bool>
                    {
                        { Event.ButtonTrack.A, TrackButtonACheckBox.Checked },
                        { Event.ButtonTrack.B, TrackButtonBCheckBox.Checked },
                        { Event.ButtonTrack.C, TrackButtonCCheckBox.Checked },
                        { Event.ButtonTrack.D, TrackButtonDCheckBox.Checked },
                        { Event.ButtonTrack.FxL, TrackFXLCheckBox.Checked   },
                        { Event.ButtonTrack.FxR, TrackFXRCheckBox.Checked   }
                    }
                }
            };

            // Act as converter
            switch (_mode)
            {
                case ConvertMode.Converter:     SingleConvert(); break;
                case ConvertMode.BulkConverter: BulkConvert();   break;
                case ConvertMode.Importer:      SingleImport();  break;
                case ConvertMode.BulkImporter:  BulkImport();    break;
            }
        }

        private void SingleImport()
        {

            try 
            { 
                // Assign metadata
                Result.Ascii        = AsciiTextBox.Text;
                Result.BackgroundId = short.Parse((BackgroundDropDown.SelectedItem ?? "0").ToString().Split(' ')[0]);
                Result.Version      = (GameVersion)(VersionDropDown.SelectedIndex + 1);
                Result.InfVersion   = InfVerDropDown.SelectedIndex == 0 ? InfiniteVersion.Mxm : (InfiniteVersion)(InfVerDropDown.SelectedIndex + 1);
                Result.GenreId      = 16;
                Result.Levels       = new Dictionary<Difficulty, VoxLevelHeader>();

                if (Result.BpmMin != Result.BpmMax && _exporter.Source.MusicOffset % 48 != 0 && Options.RealignOffset)
                {
                    // You've been warned!
                    var prompt = MessageBox.Show(
                       "Adapting music offset could break this chart.\n" +
                       "Do you want to continue?",
                       "Warning",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Warning
                    );

                    if (prompt == DialogResult.No)
                        return;
                }

                if (Directory.Exists(AssetManager.GetMusicPath(Result)) || AssetManager.Headers.Any(h => h.Ascii == Result.Ascii))
                {
                    MessageBox.Show(
                        $"Music Code {Result.CodeName} is already taken.\nTry configure \"Music Code\" manually.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }

                var importOptions = new AudioImportOptions
                {
                    Format        = AudioFormat.Iidx,
                    PreviewOffset = PreviewTimePicker.Value.Minute * 60 + PreviewTimePicker.Value.Second
                };

                _exporter.Export(Result, _charts, Options, importOptions);
                Action = _exporter.Action;

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to import ksh chart.\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                // Eliminate non-existent files
                foreach (var chart in _charts.Values.ToArray())
                {
                    if (!File.Exists(chart.FileName))
                        _charts.Remove(chart.Header.Difficulty);
                }

                // Reload jacket
                UpdateLevels();
            }
        }

        private void BulkImport()
        {
            var output  = new List<VoxHeader>();
            var actions = new Dictionary<string, Action>();
            var errors  = new List<string>();
            using (var loader = new LoadingForm())
            {
                loader.SetAction(dialog =>
                {
                    var directories = Directory.GetDirectories(_target);
                    int current     = 0;
                    foreach (string dir in directories)
                    {
                        dialog.SetStatus($"Processing {Path.GetFileName(dir)}..");
                        dialog.SetProgress((current + 1 / (float)directories.Length) * 100f);

                        var files = Directory.GetFiles(dir, "*.ksh");
                        if (files.Length == 0)
                            continue;

                        string fn = files[0];

                        try
                        {
                            var ksh = new Ksh();
                            ksh.Parse(fn, Options);

                            var header          = ksh.ToHeader();
                            header.Id           = AssetManager.GetNextMusicId() + current++;
                            header.BackgroundId = short.Parse((BackgroundDropDown.SelectedItem ?? "0").ToString().Split(' ')[0]);
                            header.Version      = (GameVersion)(VersionDropDown.SelectedIndex + 1);
                            header.InfVersion   = InfVerDropDown.SelectedIndex == 0 ? InfiniteVersion.Mxm : (InfiniteVersion)(InfVerDropDown.SelectedIndex + 1);
                            header.GenreId      = 16;
                            header.Levels       = new Dictionary<Difficulty, VoxLevelHeader>();

                            string ascii = Path.GetFileName(Path.GetDirectoryName(fn));
                            if (AssetManager.Headers.Any(v => v.Ascii == ascii)     || // Duplicate header with same ascii
                                Directory.Exists(AssetManager.GetMusicPath(header)) || // Asset that use the ascii is already exists
                                output.Any(h => h.Ascii == ascii))                     // Output with same ascii is already exists
                            {
                                continue;
                            }

                            var charts   = Ksh.Exporter.GetCharts(Path.GetDirectoryName(fn), header.Title);
                            var exporter = new Ksh.Exporter(ksh);

                            header.Ascii = ascii;
                            exporter.Export(header, charts, Options);

                            output.Add(header);
                            actions.Add(ascii, exporter.Action);
                        }
                        catch (Exception ex)
                        {
                            string err = $"Failed attempt to convert ksh file: {Path.GetFileName(fn)} ({ex.Message})";
                            errors.Add(err);
                            Debug.WriteLine(err);
                        }
                    }

                    dialog.Complete();
                });

                loader.ShowDialog();
            }

            if (errors.Count != 0)
            {
                string message = "Failed to import one or more charts:";
                foreach (string err in errors)
                    message += $"\n{err}";

                MessageBox.Show(
                   message,
                   "Warning",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning
               );
            }

            ResultSet = output.ToArray();
            ActionSet = actions;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void SingleConvert()
        {
            try 
            {
                // Single convert
                using (var browser = new SaveFileDialog())
                {
                    browser.Filter = "Sound Voltex Chart File|*.vox|All Files|*.*";
                    if (browser.ShowDialog() != DialogResult.OK)
                        return;

                    var ksh = new Ksh();
                    ksh.Parse(_target, Options);

                    var vox = new VoxChart();
                    vox.Import(ksh);
                    vox.Serialize(browser.FileName);

                    MessageBox.Show(
                        "Chart has been converted successfully",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                   $"Failed to convert ksh chart.\n{ex.Message}",
                   "Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Error
                );
            }
        }

        private void BulkConvert()
        {
            var errors = new List<string>();
            using (var browser = new CommonOpenFileDialog())
            {
                browser.IsFolderPicker = true;
                browser.Multiselect    = false;

                if (browser.ShowDialog() != CommonFileDialogResult.Ok)
                    return;

                string outputDir = browser.FileName;
                using (var loader = new LoadingForm())
                {
                    loader.SetAction(dialog =>
                    {
                        var directories = Directory.GetDirectories(_target);
                        int progress = 0;
                        foreach (string dir in directories)
                        {
                            dialog.SetStatus($"Processing {Path.GetFileName(dir)}..");
                            dialog.SetProgress((progress++ / (float)directories.Length) * 100f);
                            foreach (var fn in Directory.GetFiles(dir, "*.ksh"))
                            {
                                try
                                {
                                    // Determine output path
                                    string path = Path.Combine(
                                        $"{outputDir}",
                                        $"{Path.GetFileName(dir)}\\"
                                    );
                                    
                                    // Create output folder if it's not exists
                                    Directory.CreateDirectory(path);
                                    string output = Path.Combine(path, Path.GetFileName(fn.Replace(".ksh", ".vox")));

                                    // If you happen to read the source, you're probably looking for these boring lines
                                    var ksh = new Ksh();
                                    ksh.Parse(fn, Options);

                                    var vox = new VoxChart();
                                    vox.Import(ksh);
                                    vox.Serialize(output);
                                }
                                catch (Exception ex)
                                {
                                    string err = $"Failed attempt to convert ksh file: {Path.GetFileName(fn)} ({ex.Message})";
                                    errors.Add(err);
                                    Debug.WriteLine(err);
                                }
                            }
                        }

                        dialog.Complete();
                    });

                    loader.ShowDialog();
                }
            }

            if (errors.Count == 0)
            {
                MessageBox.Show(
                    "Chart has been converted successfully",
                    "Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            else
            {
                string message = "Failed to convert one or more charts:";
                foreach (string err in errors)
                    message += $"\n{err}";

                MessageBox.Show(
                   message,
                   "Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning
               );
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void LoadJacket(ChartInfo info)
        {
            var chart = info.Source;
            if (chart == null)
                return;

            PictureBox pictureBox = null;
            string tag = ((int)chart.Difficulty).ToString();
            foreach (var control in LevelGroupBox.Controls)
            {
                if (control is PictureBox p && p.Tag.ToString() == tag)
                {
                    pictureBox = p;
                    break;
                }
            }

            if (pictureBox == null)
                return;

            string filename = Path.Combine(Path.GetDirectoryName(info.FileName) ?? "", chart.JacketFileName);
            if (!File.Exists(filename))
            {
                pictureBox.Image = DummyJacket;
                return;
            }

            try
            {
                using (var image = Image.FromFile(filename))
                    pictureBox.Image = new Bitmap(image);
            }
            catch (Exception ex)
            {
                pictureBox.Image = DummyJacket;
                Debug.WriteLine("Failed load ksh jacket: {0} ({1})", filename, ex.Message);
            }
        }

        private void UpdateLevels()
        {
            var buttons = new List<Button>();
            foreach (var control in LevelGroupBox.Controls)
            {
                if (control is Button button)
                    buttons.Add(button);
            }

            foreach (Difficulty diff in Enum.GetValues(typeof(Difficulty)))
            {
                Button button = null;
                string tag = ((int)diff).ToString();
                foreach (var bt in buttons)
                {
                    if (bt.Tag.ToString() == tag)
                    {
                        button = bt;
                        break;
                    }
                }

                if (!_charts.ContainsKey(diff) || _charts[diff] == null)
                {
                    _charts.Remove(diff);
                    if (button != null)
                        button.Text = "--";

                    foreach (var control in LevelGroupBox.Controls)
                    {
                        if (control is PictureBox picture && picture.Tag.ToString() == tag)
                        {
                            picture.Image = DummyJacket;
                            break;
                        }
                    }
                }
                else
                {
                    if (button != null)
                    {
                        switch (diff)
                        {
                            case Difficulty.Novice: button.Text = "NOV"; break;
                            case Difficulty.Advanced: button.Text = "ADV"; break;
                            case Difficulty.Exhaust: button.Text = "EXH"; break;
                            default:
                                button.Text = InfVerDropDown.SelectedItem.ToString();
                                break;
                        }
                    }

                    LoadJacket(_charts[diff]);
                }
            }
        }

        private void OnPreviewTimePickerValueChanged(object sender, EventArgs e)
        {
            if (PreviewTimePicker.Value.Minute == 59)
                PreviewTimePicker.Value = PreviewTimePicker.Value.AddMinutes(1);
            else if (PreviewTimePicker.Value.Minute >= 11)
                PreviewTimePicker.Value = PreviewTimePicker.Value.AddMinutes(-1);

            if (PreviewTimePicker.Value.Minute == 10)
                PreviewTimePicker.Value = PreviewTimePicker.Value.AddSeconds(-PreviewTimePicker.Value.Second);
        }
    }
}
