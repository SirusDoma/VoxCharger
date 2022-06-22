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
    public enum ConvertMode
    {
        Converter = 0,
        Importer  = 1,
        BulkConverter = 2,
        BulkImporter  = 3
    }

    public partial class ConverterForm : Form
    {
        private readonly Image DummyJacket = VoxCharger.Properties.Resources.jk_dummy_s;
        public static string LastBackground { get; private set; } = "63";

        private string target;
        private string defaultAscii;
        private ConvertMode mode;
        private Dictionary<Difficulty, ChartInfo> charts = new Dictionary<Difficulty, ChartInfo>();
        private Ksh.Exporter exporter;

        public VoxHeader Result        { get; private set; } = null;
        public VoxHeader[] ResultSet   { get; private set; } = new VoxHeader[0];
        public Ksh.ParseOption Options { get; private set; } = new Ksh.ParseOption();
        public Action Action           { get; private set; } = null;
        public Dictionary<string, Action> ActionSet { get; private set; } = new Dictionary<string, Action>();

        public ConverterForm(string path, ConvertMode convert)
        {
            InitializeComponent();

            target = path;
            mode   = convert;

            // Cancerous code to adjust layout depending what this form going to be
            if (mode == ConvertMode.Converter)
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
                PathTextBox.Text = target;

                return;
            }
            else if (mode == ConvertMode.BulkImporter)
            {
                MusicCodeLabel.Visible     = false;
                LevelGroupBox.Enabled      = LevelGroupBox.Visible      = false;
                AsciiTextBox.Enabled       = AsciiTextBox.Visible       = false;
                AsciiAutoCheckBox.Enabled  = AsciiAutoCheckBox.Visible  = false;

                int componentHeight      = AsciiTextBox.Height;
                OptionsGroupBox.Location = LevelGroupBox.Location;
                OptionsGroupBox.Height  -= componentHeight;
                Height                  -= LevelGroupBox.Height + componentHeight;
            }

            PathTextBox.Text                 = target;
            BackgroundDropDown.SelectedItem  = LastBackground;
            VersionDropDown.SelectedIndex    = 5;
            InfVerDropDown.SelectedIndex     = 0;
            ProcessConvertButton.Text        = "Add";
            
        }

        private void OnConverterFormLoad(object sender, EventArgs e)
        {
            if (mode != ConvertMode.Importer)
                return;

            try
            {
                var main = new Ksh();
                main.Parse(target);
 
                Result       = main.ToHeader();
                Result.ID    = AssetManager.GetNextMusicID();
                Result.Ascii = defaultAscii = AsciiTextBox.Text = Path.GetFileName(Path.GetDirectoryName(target));
                exporter     = new Ksh.Exporter(main);

                for (int i = 1; Directory.Exists(AssetManager.GetMusicPath(Result)); i++)
                {
                    if (i >= 100)
                        break; // seriously? stupid input get stupid output

                    Result.Ascii = $"{defaultAscii}{i:D2}";
                }

                defaultAscii = AsciiTextBox.Text = Result.Ascii;
                charts[main.Difficulty] = new ChartInfo(main, main.ToLevelHeader(), target);
                LoadJacket(charts[main.Difficulty]);

                // Try to locate another difficulty
                foreach (var lv in Ksh.Exporter.GetCharts(Path.GetDirectoryName(target), main.Title))
                {
                    // Don't replace main file, there might 2 files with similar meta or another stupid cases
                    if (lv.Key != main.Difficulty)
                        charts[lv.Key] = lv.Value;
                }

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

                DialogResult = DialogResult.Cancel;
                Close();
            }
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

                    charts[diff] = new ChartInfo(chart, chart.ToLevelHeader(), browser.FileName);
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
            switch (mode)
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
                Result.InfVersion   = InfVerDropDown.SelectedIndex == 0 ? InfiniteVersion.MXM : (InfiniteVersion)(InfVerDropDown.SelectedIndex + 1);
                Result.GenreId      = 16;
                Result.Levels       = new Dictionary<Difficulty, VoxLevelHeader>();

                if (Result.BpmMin != Result.BpmMax && exporter.Source.MusicOffset % 48 != 0 && Options.RealignOffset)
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
                        $"Music Code {Result.CodeName} is already taken.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }

                exporter.Export(Result, charts, Options);
                Action = exporter.Action;

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
                foreach (var chart in charts.Values.ToArray())
                {
                    if (!File.Exists(chart.FileName))
                        charts.Remove(chart.Header.Difficulty);
                }

                // Reload jacket
                UpdateUI();
            }
        }

        private void BulkImport()
        {
            var output  = new List<VoxHeader>();
            var actions = new Dictionary<string, Action>();
            var errors  = new List<string>();
            using (var loader = new LoadingForm())
            {
                var action = new Action(() =>
                {
                    var directories = Directory.GetDirectories(target);
                    int current     = 0;
                    foreach (string dir in directories)
                    {
                        loader.SetStatus($"Processing {Path.GetFileName(dir)}..");
                        loader.SetProgress((current + 1 / (float)directories.Length) * 100f);

                        var files = Directory.GetFiles(dir, "*.ksh");
                        if (files.Length == 0)
                            continue;

                        string fn = files[0];

                        try
                        {
                            var ksh = new Ksh();
                            ksh.Parse(fn, Options);

                            var header          = ksh.ToHeader();
                            header.ID           = AssetManager.GetNextMusicID() + current++;
                            header.BackgroundId = short.Parse((BackgroundDropDown.SelectedItem ?? "0").ToString().Split(' ')[0]);
                            header.Version      = (GameVersion)(VersionDropDown.SelectedIndex + 1);
                            header.InfVersion   = InfVerDropDown.SelectedIndex == 0 ? InfiniteVersion.MXM : (InfiniteVersion)(InfVerDropDown.SelectedIndex + 1);
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

                            continue;
                        }
                    }

                    loader.Complete();
                });

                loader.SetAction(action);
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
                    ksh.Parse(target, Options);

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
                                    // Determine output path
                                    string path = Path.Combine(
                                        $"{browser.SelectedPath}",
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
                            case Difficulty.Novice: button.Text = "NOV"; break;
                            case Difficulty.Advanced: button.Text = "ADV"; break;
                            case Difficulty.Exhaust: button.Text = "EXH"; break;
                            default:
                                button.Text = InfVerDropDown.SelectedItem.ToString();
                                break;
                        }
                    }

                    LoadJacket(charts[diff]);
                }
            }
        }
    }
}
