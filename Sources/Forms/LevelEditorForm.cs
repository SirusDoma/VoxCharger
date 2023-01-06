using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VoxCharger
{
    public partial class LevelEditorForm : Form
    {
        private static readonly Image DummyJacket = VoxCharger.Properties.Resources.jk_dummy_s;

        private VoxHeader _header;
        private Ksh _kshUpdate              = null;
        private string _voxUpdatePath       = string.Empty;
        private string _audioMainUpdatePath    = string.Empty;
        private string _audioPreviewUpdatePath = string.Empty;

        public VoxLevelHeader Result { get; private set; }

        public Action Action { get; private set; } = null;

        public LevelEditorForm(VoxHeader header, Difficulty difficulty)
        {
            this._header = header;
            VoxLevelHeader level;
            if (!header.Levels.TryGetValue(difficulty, out level))
                Result = new VoxLevelHeader();
            else
                Result = level;

            InitializeComponent();
        }

        private void OnLevelEditorFormLoad(object sender, EventArgs e)
        {
            switch (Result.Difficulty)
            {
                case Difficulty.Novice:   Text = "Level Editor - NOV"; break;
                case Difficulty.Advanced: Text = "Level Editor - ADV"; break;
                case Difficulty.Exhaust:  Text = "Level Editor - EXH"; break;
                default:
                    switch (_header.InfVersion)
                    {
                        case InfiniteVersion.Inf: Text = "Level Editor - INF"; break;
                        case InfiniteVersion.Grv: Text = "Level Editor - GRV"; break;
                        case InfiniteVersion.Hvn: Text = "Level Editor - HVN"; break;
                        case InfiniteVersion.Vvd: Text = "Level Editor - VVD"; break;
                        case InfiniteVersion.Xcd: Text = "Level Editor - XCD"; break;
                        default:                  Text = "Level Editor - MXM"; break;
                    }
                    break;
            }

            LevelNumericBox.Value   = Result.Level;
            EffectorTextBox.Text    = Result.Effector;
            IllustratorTextBox.Text = Result.Illustrator;
            
            AddRadarDataButton.Visible = Result.Radar == null;
            RadarGroupBox.Visible = Result.Radar != null;

            if (Result.Radar != null)
            {
                NotesNumericUpDown.Value    = Result.Radar.Notes;
                PeakNumericUpDown.Value     = Result.Radar.Peak;
                TsumamiNumericUpDown.Value  = Result.Radar.Lasers;
                TrickyNumericUpDown.Value   = Result.Radar.Tricky;
                HandTripNumericUpDown.Value = Result.Radar.HandTrip;
                OneHandNumericUpDown.Value  = Result.Radar.OneHand;
            }
            else
                Height -= (RadarGroupBox.Height - AddRadarDataButton.Height);
            
            LoadJacket();
        }

        private void OnJacketButtonClick(object sender, EventArgs e)
        {
            using (var browser = new OpenFileDialog())
            {
                browser.Filter = "Portable Network Graphic|*.png";
                browser.CheckFileExists = true;

                if (browser.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var image = Image.FromFile(browser.FileName))
                            JacketPictureBox.Image = Result.Jacket = new Bitmap(image);
                    }
                    catch (Exception)
                    {
                        Result.Jacket = null;
                    }
                }
            }
        }

        private void OnVoxButtonClick(object sender, EventArgs e)
        {
            using (var browser = new OpenFileDialog())
            {
                browser.Filter = "All supported types|*.vox;*.ksh|Sound Voltex Chart File|*.vox|Kshoot Chart File|*.ksh";
                browser.CheckFileExists = true;

                if (browser.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Shoving vox file while it's not actually vox? well.. stupid input get stupid output
                        string filename = browser.FileName;
                        if (filename.EndsWith(".vox"))
                        {
                            var vox = new VoxChart();
                            vox.Parse(filename);

                            _voxUpdatePath = filename;
                            _kshUpdate = null;
                        }
                        else if (filename.EndsWith(".ksh"))
                        {
                            var ksh = new Ksh();
                            ksh.Parse(filename);

                            _kshUpdate = ksh;
                            _voxUpdatePath = string.Empty;
                        }
                        else
                            MessageBox.Show("Warning! Stupid input, get stupid output :)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    catch (Exception ex)
                    {
                        _voxUpdatePath = null;
                        MessageBox.Show(
                            $"Failed to load chart.\n{ex.Message}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }

                }
            }
        }

        private void OnMainDxButtonClick(object sender, EventArgs e)
        {
            LoadAudio();
        }

        private void OnPreview2DXClick(object sender, EventArgs e)
        {
            LoadAudio(true);
        }
        
        private void OnAddRadarDataButtonClick(object sender, EventArgs e)
        {
            RadarGroupBox.Visible = true;
            Height += RadarGroupBox.Height - AddRadarDataButton.Height;
        }

        private void OnJacketPictureBoxClick(object sender, EventArgs e)
        {
            if (Result.Jacket == null)
            {
                string jacket = $"{AssetManager.GetJacketPath(_header, Result.Difficulty)}_b.png";
                if (!File.Exists(jacket))
                    return;

                using (var image = Image.FromFile(jacket))
                using (var viewer = new JacketViewerForm(image))
                    viewer.ShowDialog();
            }
            else
            {
                using (var viewer = new JacketViewerForm(Result.Jacket))
                    viewer.ShowDialog();
            }
        }

        private void OnCancelEditButtonClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnLevelEditorFormFormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void OnSaveEditButtonClick(object sender, EventArgs e)
        {
            try
            {
                string voxData = string.Empty;
                if (!string.IsNullOrEmpty(_voxUpdatePath))
                {
                    if (File.Exists(_voxUpdatePath))
                        voxData = File.ReadAllText(_voxUpdatePath, Encoding.GetEncoding("Shift_JIS"));
                    else
                        throw new FileNotFoundException("Vox file not found", _voxUpdatePath);
                }

                VoxChart voxChart = null;
                if (_kshUpdate != null)
                {
                    voxChart = new VoxChart();
                    voxChart.Import(_kshUpdate);
                }

                if (!string.IsNullOrEmpty(_audioMainUpdatePath))
                {
                    if (File.Exists(_audioMainUpdatePath))
                    {
                        string tmp = Path.Combine(
                            Path.GetTempPath(), 
                            $"{Path.GetRandomFileName()}{new FileInfo(_audioMainUpdatePath).Extension}"
                        );

                        File.Copy(_audioMainUpdatePath, tmp);
                        _audioMainUpdatePath = tmp;
                    }
                    else
                        throw new FileNotFoundException("Music file not found", _audioMainUpdatePath);
                }

                if (!string.IsNullOrEmpty(_audioPreviewUpdatePath))
                {
                    if (File.Exists(_audioPreviewUpdatePath))
                    {
                        string tmp = Path.Combine(
                            Path.GetTempPath(),
                            $"{Path.GetRandomFileName()}{new FileInfo(_audioPreviewUpdatePath).Extension}"
                        );

                        File.Copy(_audioPreviewUpdatePath, tmp);
                        _audioPreviewUpdatePath = tmp;
                    }
                    else
                        throw new FileNotFoundException("Preview file not found", _audioPreviewUpdatePath);
                }

                if (Result.Jacket != null || voxChart != null || !string.IsNullOrEmpty(voxData) || !string.IsNullOrEmpty(_audioMainUpdatePath) || !string.IsNullOrEmpty(_audioPreviewUpdatePath))
                {
                    Action = () =>
                    {
                        if (Result.Jacket != null)
                            AssetManager.ImportJacket(_header, Result.Difficulty, Result.Jacket);

                        if (!string.IsNullOrEmpty(voxData))
                            AssetManager.ImportVox(_header, Result.Difficulty, _voxUpdatePath);
                        else if (voxChart != null)
                            AssetManager.ImportVox(_header, Result.Difficulty, voxChart);

                        if (File.Exists(_audioMainUpdatePath))
                        {
                            var audioFormat = _audioMainUpdatePath.ToLower().EndsWith(".s3v") ? AudioFormat.S3V : AudioFormat.Iidx;
                            AssetManager.ImportAudio(_audioMainUpdatePath, _header, Result.Difficulty, AudioImportOptions.WithFormat(audioFormat));
                        }

                        if (File.Exists(_audioPreviewUpdatePath))
                        {
                            var audioFormat = _audioPreviewUpdatePath.ToLower().EndsWith(".s3v") ? AudioFormat.S3V : AudioFormat.Iidx;
                            AssetManager.ImportAudio(_audioPreviewUpdatePath, _header, Result.Difficulty, AudioImportOptions.WithFormat(audioFormat).AsPreview());
                        }
                    };
                }

                Result.Level       = (int)LevelNumericBox.Value;
                Result.Effector    = EffectorTextBox.Text;
                Result.Illustrator = IllustratorTextBox.Text;
                if (RadarGroupBox.Visible)
                {
                    Result.Radar = new VoxLevelRadar
                    {
                        Notes    = (byte)NotesNumericUpDown.Value,
                        Peak     = (byte)PeakNumericUpDown.Value,
                        Lasers   = (byte)TsumamiNumericUpDown.Value,
                        Tricky   = (byte)TrickyNumericUpDown.Value,
                        HandTrip = (byte)HandTripNumericUpDown.Value,
                        OneHand  = (byte)OneHandNumericUpDown.Value,
                    };
                }
                

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void LoadAudio(bool preview = false)
        {
            using (var browser = new OpenFileDialog())
            {
                browser.Filter = "All supported formats|*.2dx;*.s3v;*.asf;*.wav;*.ogg;*.mp3;*.flac|BEMANI Music Files|*.2dx;*.s3v|Music Files|*.wav;*.ogg;*.mp3;*.flac;*.asf";
                browser.CheckFileExists = true;

                if (browser.ShowDialog() == DialogResult.OK)
                {
                    if (!preview)
                        _audioMainUpdatePath = browser.FileName;
                    else
                        _audioPreviewUpdatePath = browser.FileName;
                }  
            }
        }

        private void LoadJacket()
        {
            if (Result.Jacket != null)
            {
                JacketPictureBox.Image = Result.Jacket;
                return;
            }

            try
            {
                string currentJacket = $"{AssetManager.GetJacketPath(_header, Result.Difficulty)}_s.png";
                string defaultJacket = $"{AssetManager.GetDefaultJacketPath(_header)}_s.png";
                if (File.Exists(currentJacket))
                {
                    using (var image = Image.FromFile(currentJacket))
                        JacketPictureBox.Image = new Bitmap(image);
                }
                else
                {
                    if (File.Exists(defaultJacket))
                    {
                        using (var image = Image.FromFile(defaultJacket))
                            JacketPictureBox.Image = new Bitmap(image);
                    }
                    else
                        JacketPictureBox.Image = DummyJacket;
                }
            }
            catch (Exception)
            {
                JacketPictureBox.Image = DummyJacket;
            }
        }
    }
}
