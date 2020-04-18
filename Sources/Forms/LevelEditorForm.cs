using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VoxCharger
{
    public partial class LevelEditorForm : Form
    {
        private readonly Image DummyJacket = VoxCharger.Properties.Resources.jk_dummy_s;

        private VoxHeader header;
        private Ksh kshUpdate = null;
        private string voxUpdatePath       = string.Empty;
        private string dxMainUpdatePath    = string.Empty;
        private string dxPreviewUpdatePath = string.Empty;

        public VoxLevelHeader Result { get; private set; }

        public Action Action { get; private set; } = null;

        public LevelEditorForm(VoxHeader header, Difficulty difficulty)
        {
            this.header = header;
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
                    switch (header.InfVersion)
                    {
                        case InfiniteVersion.INF: Text = "Level Editor - INF"; break;
                        case InfiniteVersion.GRV: Text = "Level Editor - GRV"; break;
                        case InfiniteVersion.HVN: Text = "Level Editor - HVN"; break;
                        case InfiniteVersion.VVD: Text = "Level Editor - VVD"; break;
                        default:                  Text = "Level Editor - MXM"; break;
                    }
                    break;
            }

            LevelNumericBox.Value   = Result.Level;
            EffectorTextBox.Text    = Result.Effector;
            IllustratorTextBox.Text = Result.Illustrator;

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

                            voxUpdatePath = filename;
                            kshUpdate = null;
                        }
                        else if (filename.EndsWith(".ksh"))
                        {
                            var ksh = new Ksh();
                            ksh.Parse(filename);

                            kshUpdate = ksh;
                            voxUpdatePath = string.Empty;
                        }
                        else
                            MessageBox.Show("Warning! Stupid input, get stupid output :)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    catch (Exception ex)
                    {
                        voxUpdatePath = null;
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
            Load2DX();
        }

        private void OnPreview2DXClick(object sender, EventArgs e)
        {
            Load2DX(true);
        }

        private void Load2DX(bool preview = false)
        {
            using (var browser = new OpenFileDialog())
            {
                browser.Filter = "All supported formats|*.2dx;*.s3v;*.wav;*.ogg;*.mp3;*.flac|2DX Music File|*.2dx;*.s3v|Music Files|*.wav;*.ogg;*.mp3;*.flac"; ;
                browser.CheckFileExists = true;

                if (browser.ShowDialog() == DialogResult.OK)
                {
                    if (!preview)
                        dxMainUpdatePath = browser.FileName;
                    else
                        dxPreviewUpdatePath = browser.FileName;
                }  
            }
        }

        private void OnJacketPictureBoxClick(object sender, EventArgs e)
        {
            if (Result.Jacket == null)
            {
                string jacket = $"{AssetManager.GetJacketPath(header, Result.Difficulty)}_b.png";
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

        private void OnSaveEditButtonClick(object sender, EventArgs e)
        {
            try
            {
                string voxData = string.Empty;
                if (!string.IsNullOrEmpty(voxUpdatePath))
                {
                    if (File.Exists(voxUpdatePath))
                        voxData = File.ReadAllText(voxUpdatePath, Encoding.GetEncoding("Shift_JIS"));
                    else
                        throw new FileNotFoundException("Vox file not found", voxUpdatePath);
                }

                VoxChart voxChart = null;
                if (kshUpdate != null)
                {
                    voxChart = new VoxChart();
                    voxChart.Import(kshUpdate);
                }

                if (!string.IsNullOrEmpty(dxMainUpdatePath))
                {
                    if (File.Exists(dxMainUpdatePath))
                    {
                        string tmp = Path.Combine(
                            Path.GetTempPath(), 
                            $"{Path.GetRandomFileName()}{new FileInfo(dxMainUpdatePath).Extension}"
                        );

                        File.Copy(dxMainUpdatePath, tmp);
                        dxMainUpdatePath = tmp;
                    }
                    else
                        throw new FileNotFoundException("Music file not found", dxMainUpdatePath);
                }

                if (!string.IsNullOrEmpty(dxPreviewUpdatePath))
                {
                    if (File.Exists(dxPreviewUpdatePath))
                    {
                        string tmp = Path.Combine(
                            Path.GetTempPath(),
                            $"{Path.GetRandomFileName()}{new FileInfo(dxPreviewUpdatePath).Extension}"
                        );

                        File.Copy(dxPreviewUpdatePath, tmp);
                        dxPreviewUpdatePath = tmp;
                    }
                    else
                        throw new FileNotFoundException("Preview file not found", dxPreviewUpdatePath);
                }

                if (Result.Jacket != null || voxChart != null || !string.IsNullOrEmpty(voxData) || !string.IsNullOrEmpty(dxMainUpdatePath) || !string.IsNullOrEmpty(dxPreviewUpdatePath))
                {
                    Action = new Action(() =>
                    {
                        if (Result.Jacket != null)
                            AssetManager.ImportJacket(header, Result.Difficulty, Result.Jacket);

                        if (!string.IsNullOrEmpty(voxData))
                            AssetManager.ImportVox(header, Result.Difficulty, voxUpdatePath);
                        else if (voxChart != null)
                            AssetManager.ImportVox(header, Result.Difficulty, voxChart);

                        if (File.Exists(dxMainUpdatePath))
                            AssetManager.Import2DX(dxMainUpdatePath, header, Result.Difficulty);

                        if (File.Exists(dxPreviewUpdatePath))
                            AssetManager.Import2DX(dxPreviewUpdatePath, header, Result.Difficulty, true);
                    });
                }

                Result.Level       = (int)LevelNumericBox.Value;
                Result.Effector    = EffectorTextBox.Text;
                Result.Illustrator = IllustratorTextBox.Text;

                DialogResult       = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string currentJacket = $"{AssetManager.GetJacketPath(header, Result.Difficulty)}_s.png";
                string defaultJacket = $"{AssetManager.GetDefaultJacketPath(header)}_s.png";
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
