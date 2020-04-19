using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace VoxCharger
{
    public partial class ConverterForm : Form
    {
        private class ChartInfo
        {
            public Ksh Source            { get; set; }
            public VoxLevelHeader Header { get; set; }
            public string FileName       { get; set; }
            public string MusicFileName  { get; set; }
            public string JacketFileName { get; set; }

            public ChartInfo(Ksh source, VoxLevelHeader header, string fileName)
            {
                Source = source;
                Header = header;
                FileName = fileName;
            }
        }

        private readonly Image DummyJacket = VoxCharger.Properties.Resources.jk_dummy_s;
        public static string LastBackground { get; private set; } = "63";

        private string target;
        private string defaultAscii;
        private bool converter;
        private Dictionary<Difficulty, ChartInfo> charts = new Dictionary<Difficulty, ChartInfo>();

        public VoxHeader Header        { get; private set; } = null;
        public Action Action           { get; private set; } = null;
        public Ksh.ParseOption Options { get; private set; } = new Ksh.ParseOption();

        public ConverterForm(string path, bool asConverter = false)
        {
            InitializeComponent();

            target = path;
            converter = asConverter;
            if (asConverter)
            {
                MusicCodeLabel.Visible     = false;
                InfVerLabel.Visible        = false;
                BackgroundLabel.Visible    = false;
                LevelGroupBox.Enabled      = LevelGroupBox.Visible      = false;
                AsciiTextBox.Enabled       = AsciiTextBox.Visible       = false;
                AsciiAutoCheckBox.Enabled  = AsciiAutoCheckBox.Visible  = false;
                VersionDropDown.Enabled    = VersionDropDown.Visible    = false;
                InfVerDropDown.Enabled     = InfVerDropDown.Visible     = false;
                BackgroundDropDown.Enabled = BackgroundDropDown.Visible = false;

                int componentHeight      = AsciiTextBox.Height + VersionDropDown.Height + BackgroundDropDown.Height;
                OptionsGroupBox.Location = LevelGroupBox.Location;
                OptionsGroupBox.Height  -= componentHeight;
                Height                  -= LevelGroupBox.Height + componentHeight;

                ProcessConvertButton.Text = "Convert";
            }
            else
                ProcessConvertButton.Text = "Add";

            PathTextBox.Text = target;
        }

        private void OnConverterFormLoad(object sender, EventArgs e)
        {
            var main = new Ksh();
            if (converter)
                return;

            try
            {
                main.Parse(target);

                Header = ToHeader(main);
                Header.Ascii = new DirectoryInfo(Path.GetDirectoryName(target)).Name;
                defaultAscii = AsciiTextBox.Text = Header.Ascii;

                for (int i = 1; Directory.Exists(AssetManager.GetMusicPath(Header)); i++)
                {
                    if (i >= 100)
                        break; // seriously? stupid input get stupid output

                    Header.Ascii = $"{defaultAscii}{i:D2}";
                }

                defaultAscii = AsciiTextBox.Text = Header.Ascii;
                BackgroundDropDown.SelectedItem = LastBackground;
                VersionDropDown.SelectedIndex = 4;
                InfVerDropDown.SelectedIndex = 0;

                charts[main.Difficulty] = new ChartInfo(main, ToLevelHeader(main), target);
                LoadJacket(charts[main.Difficulty]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Failed to load ksh chart.\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                Close();
                return;
            }

            // Try to locate another difficulty
            charts = GetCharts(target, main);
            UpdateUI();
        }

        private void OnAsciiAutoCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            AsciiTextBox.ReadOnly = AsciiAutoCheckBox.Checked;
            if (AsciiTextBox.ReadOnly)
                AsciiTextBox.Text = defaultAscii;
        }

        private void OnBackgroundDropDownSelectedIndexChanged(object sender, EventArgs e)
        {
            LastBackground = BackgroundDropDown.SelectedItem.ToString();
        }

        private void OnInfVerDropDownSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!charts.ContainsKey(Difficulty.Infinite))
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

                    charts[diff] = new ChartInfo(chart, ToLevelHeader(chart), browser.FileName);
                    UpdateUI();
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
            Options = new Ksh.ParseOption()
            {
                RealignOffset = RealignOffsetCheckBox.Checked,
                EnableChipFx  = ChipFxCheckBox.Checked,
                EnableLongFx  = LongFxCheckBox.Checked,
                EnableCamera  = CameraCheckBox.Checked,
                EnableSlamImpact  = SlamImpactCheckBox.Checked,
                EnableLaserTrack  = TrackLaserCheckBox.Checked,
                EnableButtonTrack = TrackButtonCheckBox.Checked
            };

            // Act as converter
            if (converter)
                Convert();
            else
                Process();
        }

        private VoxHeader ToHeader(Ksh chart)
        {
            return new VoxHeader()
            {
                ID               = AssetManager.GetNextMusicID(),
                Title            = chart.Title,
                Artist           = chart.Artist,
                BpmMin           = chart.BpmMin,
                BpmMax           = chart.BpmMax,
                Volume           = chart.Volume > 0 ? (short)chart.Volume : (short)91,
                DistributionDate = DateTime.Now,
                BackgroundId     = 63,
                GenreId          = 16,
            };
        }

        private VoxLevelHeader ToLevelHeader(Ksh chart)
        {
            return new VoxLevelHeader
            {
                Difficulty  = chart.Difficulty,
                Illustrator = chart.Illustrator,
                Effector    = chart.Effector,
                Level       = chart.Level
            };
        }

        private void Process()
        {
            try
            {
                bool warned = false;
                foreach (var header in AssetManager.Headers)
                {
                    if (Header.Ascii == header.Ascii)
                    {
                        MessageBox.Show(
                            $"Music Code is already exists.\n{AssetManager.GetMusicPath(header).Replace(AssetManager.GamePath, "")}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        return;
                    }
                }

                // Assign metadata
                Header.Ascii = AsciiTextBox.Text;
                Header.BackgroundId = short.Parse((BackgroundDropDown.SelectedItem ?? "0").ToString().Split(' ')[0]);
                Header.Version = (GameVersion)(VersionDropDown.SelectedIndex + 1);
                Header.InfVersion = InfVerDropDown.SelectedIndex == 0 ? InfiniteVersion.MXM : (InfiniteVersion)(InfVerDropDown.SelectedIndex + 1);
                Header.GenreId = 16;
                Header.Levels = new Dictionary<Difficulty, VoxLevelHeader>();

                if (Directory.Exists(AssetManager.GetMusicPath(Header)))
                {
                    MessageBox.Show(
                        $"Music asset for {Header.CodeName} is already exists.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }

                // Uhh, remove empty level?
                var entries = charts.Where(x => x.Value != null);
                charts = new Dictionary<Difficulty, ChartInfo>();
                foreach (var entry in entries)
                    charts[entry.Key] = entry.Value;

                // Assign level info and charts
                foreach (var chart in charts)
                {
                    var info = chart.Value;
                    if (!File.Exists(info.FileName))
                    {
                        MessageBox.Show(
                            $"Chart file was moved or deleted\n{info.FileName}",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );

                        charts[chart.Key] = null;
                        UpdateUI();

                        return;
                    }

                    // If you happen to read the source, this is probably what you're looking for
                    var ksh = new Ksh();
                    ksh.Parse(info.FileName, Options);

                    var bpmCount = ksh.Events.Count(ev => ev is Event.BPM);
                    if (!warned && bpmCount > 1 && ksh.MusicOffset % 48 != 0 && Options.RealignOffset)
                    {
                        // You've been warned!
                        var prompt = MessageBox.Show(
                           "Chart contains multiple bpm with music offset that non multiple of 48.\n" +
                           "Adapting music offset could break the chart.\n\n" +
                           "Do you want to continue?",
                           "Warning",
                           MessageBoxButtons.YesNo,
                           MessageBoxIcon.Warning
                        );

                        warned = true;
                        if (prompt == DialogResult.No)
                            return;
                    }

                    // Conversion is actually boring because its already "pre-converted"
                    var vox = new VoxChart();
                    vox.Import(ksh);

                    var level = ToLevelHeader(ksh);
                    level.Chart = vox;

                    info.Source = ksh;
                    Header.Levels[chart.Key] = charts[chart.Key].Header = level;
                }

                // Prevent resource files being moved or deleted, copy them into temporary storage
                string musicFile = string.Empty;
                string tmpFile = string.Empty;
                foreach (var chart in charts)
                {
                    var info = chart.Value;

                    // Make sure to reuse 2dx file for music that share same file
                    if (string.IsNullOrEmpty(musicFile) || chart.Value.MusicFileName != musicFile)
                    {
                        string music = Path.Combine(Path.GetDirectoryName(info.FileName), info.Source.MusicFileName);
                        if (File.Exists(music))
                        {
                            string tmp = Path.Combine(
                                Path.GetTempPath(),
                                $"{Path.GetRandomFileName()}{new FileInfo(info.Source.MusicFileName).Extension}"
                            );

                            musicFile = music;
                            info.MusicFileName = tmpFile = tmp;

                            File.Copy(music, tmp);
                        }
                        else
                            info.MusicFileName = string.Empty;
                    }
                    else
                        info.MusicFileName = tmpFile;

                    string jacket = Path.Combine(Path.GetDirectoryName(info.FileName), info.Source.JacketFileName);
                    if (File.Exists(jacket))
                    {
                        string tmp = Path.Combine(
                            Path.GetTempPath(),
                            $"{Path.GetRandomFileName()}{new FileInfo(info.Source.JacketFileName).Extension}"
                        );

                        try
                        {
                            using (var image = Image.FromFile(jacket))
                                info.Header.Jacket = new Bitmap(image);
                        }
                        catch (Exception)
                        {
                            info.Header.Jacket = null;
                        }

                        info.JacketFileName = tmp;
                        File.Copy(jacket, tmp);
                    }
                }

                Action = new Action(() =>
                {
                    bool unique = false;
                    musicFile = charts.Values.First().MusicFileName;
                    foreach (var chart in charts)
                    {
                        if (chart.Value.MusicFileName != musicFile)
                        {
                            unique = true;
                            break;
                        }
                    }

                // Import all music assets
                AssetManager.ImportVox(Header);

                // Make sure to use single asset for music for shared music file
                if (!unique)
                    {
                        AssetManager.Import2DX(musicFile, Header);
                        AssetManager.Import2DX(musicFile, Header, true);
                    }

                    foreach (var chart in charts.Values)
                    {
                        if (unique && File.Exists(chart.MusicFileName))
                        {
                            AssetManager.Import2DX(chart.MusicFileName, Header, chart.Header.Difficulty);
                            AssetManager.Import2DX(chart.MusicFileName, Header, chart.Header.Difficulty, true);
                        }

                        if (File.Exists(chart.JacketFileName))
                        {
                            using (var image = Image.FromFile(chart.JacketFileName))
                                AssetManager.ImportJacket(Header, chart.Header.Difficulty, new Bitmap(image));
                        }
                    }
                });


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
            }
        }

        private void Convert()
        {
            // Only serve as options dialog
            if (string.IsNullOrEmpty(target))
            {
                DialogResult = DialogResult.OK;
                Close();
            }

            try
            {
                if (File.Exists(target) || Directory.Exists(target))
                {
                    if (File.Exists(target))
                        SingleConvert(Options);
                    else if (Directory.Exists(target))
                        BulkConvert(Options);
                }
                else
                {
                    MessageBox.Show(
                        "Target path not found",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    DialogResult = DialogResult.Cancel;
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

        private Dictionary<Difficulty, ChartInfo> GetCharts(string target, Ksh main)
        {
            // Try to locate another difficulty
            string dir = Path.GetDirectoryName(target);
            var charts = new Dictionary<Difficulty, ChartInfo>();
            foreach (string fn in Directory.GetFiles(dir, "*.ksh"))
            {
                try
                {
                    var chart = new Ksh();
                    chart.Parse(fn);

                    // Different chart
                    if (chart.Title != main.Title)
                        continue;

                    charts[chart.Difficulty] = new ChartInfo(chart, ToLevelHeader(chart), fn);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed attempt to parse ksh file: {0} ({1})", fn, ex.Message);
                }
            }

            return charts;
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

            string filename = Path.Combine(Path.GetDirectoryName(info.FileName), chart.JacketFileName);
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

        private void UpdateUI()
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

                if (!charts.ContainsKey(diff) || charts[diff] == null)
                {
                    charts.Remove(diff); // should be UpdateUiButAlterMapCharts, ya whatever
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
                            case Difficulty.Novice: button.Text   = "NOV"; break;
                            case Difficulty.Advanced: button.Text = "ADV"; break;
                            case Difficulty.Exhaust: button.Text  = "EXH"; break;
                            default:
                                button.Text = InfVerDropDown.SelectedItem.ToString();
                                break;
                        }
                    }

                    LoadJacket(charts[diff]);
                }
            }
        }

        private void SingleConvert(Ksh.ParseOption options)
        {
            // Single convert
            using (var browser = new SaveFileDialog())
            {
                browser.Filter = "Sound Voltex Chart File|*.vox|All Files|*.*";
                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                var ksh = new Ksh();
                ksh.Parse(target, options);

                var bpmCount = ksh.Events.Count(ev => ev is Event.BPM);
                if (bpmCount > 1 && ksh.MusicOffset % 48 != 0 && options.RealignOffset)
                {
                    // You've been warned!
                    var prompt = MessageBox.Show(
                       "Chart contains multiple bpm with music offset that non multiple of 48.\n" +
                       "Adapting music offset could break the chart.\n\n" +
                       "Do you want to continue?",
                       "Warning",
                       MessageBoxButtons.YesNo,
                       MessageBoxIcon.Warning
                    );

                    if (prompt == DialogResult.No)
                        return;
                }

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

        private void BulkConvert(Ksh.ParseOption options)
        {
            bool warned = false;
            using (var browser = new FolderBrowserDialog())
            {
                browser.Description = "Select output directory";
                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                using (var loader = new LoadingForm())
                {
                    var action = new Action(() =>
                    {
                        var directories = Directory.GetDirectories(target);
                        int progress = 0;
                        foreach (string dir in directories)
                        {
                            loader.SetStatus($"Processing {Path.GetFileName(dir)}..");
                            loader.SetProgress((progress++ / (float)directories.Length) * 100f);
                            foreach (var fn in Directory.GetFiles(dir, "*.ksh"))
                            {
                                try
                                {
                                    var ksh = new Ksh();
                                    ksh.Parse(fn, options);

                                    var bpmCount = ksh.Events.Count(ev => ev is Event.BPM);
                                    if (!warned && bpmCount > 1 && ksh.MusicOffset % 48 != 0 && options.RealignOffset)
                                    {
                                        // You've been warned!
                                        var prompt = MessageBox.Show(
                                               "Chart contains multiple bpm with music offset that non multiple of 48.\n" +
                                               "Adapting music offset could break the chart.\n\n" +
                                               "Do you want to continue?",
                                               "Warning",
                                               MessageBoxButtons.YesNo,
                                               MessageBoxIcon.Warning
                                            );

                                        warned = true;
                                        if (prompt == DialogResult.No)
                                            return;
                                    }

                                    var vox = new VoxChart();
                                    vox.Import(ksh);

                                    string path = Path.Combine(
                                        $"{browser.SelectedPath}",
                                        $"{Path.GetFileName(dir)}\\"
                                    );

                                    if (!Directory.Exists(path))
                                        Directory.CreateDirectory(path);

                                    vox.Serialize($"{Path.Combine(path, Path.GetFileName(fn.Replace(".ksh", ".vox")))}");
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Failed attempt to convert ksh file: {0} ({1})", fn, ex.Message);
                                    continue;
                                }
                            }
                        }

                        loader.Complete();
                    });

                    loader.SetAction(action);
                    loader.ShowDialog();
                }
            }

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
}
