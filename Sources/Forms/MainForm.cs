using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VoxCharger
{
    public partial class MainForm : Form
    {
        #region -- Variables --
        private const int DefaultVolume = 91;
        private readonly Image DummyJacket = VoxCharger.Properties.Resources.jk_dummy_s;
        private readonly Dictionary<string, Queue<Action>> actions = new Dictionary<string, Queue<Action>>();

        private bool Pristine = true;
        private bool Autosave = true;
        #endregion

        #region --- Form ---
        public MainForm()
        {
            InitializeComponent();
        }

        private void OnMainFormLoad(object sender, EventArgs e)
        {
        }

        private void OnMainFormFormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Pristine && SaveFileMenu.Enabled)
            {
                var response = MessageBox.Show(
                    "Save file before exit the program?",
                    "Quit",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (response == DialogResult.Yes)
                    SaveFileMenu.PerformClick();
                else if (response == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }
        #endregion

        #region --- Menu ---
        private void OnNewFileMenuClick(object sender, EventArgs e)
        {
            if (!Pristine && SaveFileMenu.Enabled)
            {
                var response = MessageBox.Show(
                    "Save file before open another mix?",
                    "Create Mix",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (response == DialogResult.Yes)
                    SaveFileMenu.PerformClick();
                else if (response == DialogResult.Cancel)
                    return;

                Pristine = true;
                actions.Clear();
            }

            string gamePath = AssetManager.GamePath;
            if (string.IsNullOrEmpty(AssetManager.MixPath) || !Directory.Exists(AssetManager.MixPath))
            {
                using (var browser = new FolderBrowserDialog())
                {
                    browser.ShowNewFolderButton = true;
                    browser.Description = "Select KFC Content Root";

                    if (browser.ShowDialog() == DialogResult.OK)
                    {
                        gamePath = browser.SelectedPath;

                        PathTextBox.Text = string.Empty;
                        MusicListBox.Items.Clear();

                        ResetEditor();
                    }
                }
            }

            using (var mixSelector = new MixSelectorForm(createMode: true))
            {
                AssetManager.Initialize(gamePath);
                if (mixSelector.ShowDialog() == DialogResult.OK)
                    Reload();
            }
        }

        private void OnSaveFileMenuClick(object sender, EventArgs e)
        {
            try
            {
                if (Save(AssetManager.MdbFilename))
                {
                    MessageBox.Show(
                       "Mix has been saved successfully",
                       "Information",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information
                   );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message, 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error
                );
            }
        }


        private void OnSaveAsFileMenuClick(object sender, EventArgs e)
        {
            try
            {
                using (var exporter = new SaveFileDialog())
                {
                    exporter.Filter   = "Music DB|*.xml|All Files|*.*";
                    exporter.FileName = new FileInfo(AssetManager.MdbFilename).Name; 

                    if (exporter.ShowDialog() != DialogResult.OK)
                        return;

                    if (Save(exporter.FileName))
                    {
                        MessageBox.Show(
                           "Mix has been saved successfully",
                           "Information",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void OnChangeMixFileMenuClick(object sender, EventArgs e)
        {
            if (!Pristine && SaveFileMenu.Enabled)
            {
                var response = MessageBox.Show(
                    "Save file before open another mix?",
                    "Change Mix",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question
                );

                if (response == DialogResult.Yes)
                    SaveFileMenu.PerformClick();
                else if (response == DialogResult.Cancel)
                    return;

                Pristine = true;
                actions.Clear();
            }
           
            using (var mixSelector = new MixSelectorForm())
            {
                AssetManager.Initialize(AssetManager.GamePath);
                if (mixSelector.ShowDialog() == DialogResult.OK)
                    Reload();
            }
        }

        private void OnDeleteMixFileMenuClick(object sender, EventArgs e)
        {
            var prompt = MessageBox.Show(
                $"Are you sure want to delete \"{AssetManager.MixName}\"?",
                "Delete Mix",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (prompt == DialogResult.Yes)
            {
                try
                {
                    FileMenu.Enabled = OpenButton.Enabled = false;
                    Directory.Delete(AssetManager.MixPath, true);

                    PathTextBox.Text = "";
                    MetadataGroupBox.Enabled = false;
                    MusicListBox.Items.Clear();

                    DisableUI();
                    ResetEditor();

                    AssetManager.Initialize(AssetManager.GamePath);
                    OnChangeMixFileMenuClick(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Delete Mix",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                finally
                {
                    FileMenu.Enabled = OpenButton.Enabled = true;
                }
            }
        }

        private void OnAutosaveEditMenuClick(object sender, EventArgs e)
        {
            Autosave = !Autosave;
            AutosaveEditMenu.Checked = Autosave;
        }

        private void OnExplorerEditMenuClick(object sender, EventArgs e)
        {
            var header = MusicListBox.SelectedItem as VoxHeader;
            if (header == null)
                return;

            string path = AssetManager.GetMusicPath(header);
            Process.Start("explorer.exe", path);
        }

        private void OnSingleConvertToolsMenuClick(object sender, EventArgs e)
        {
            using (var browser = new OpenFileDialog())
            {
                browser.Filter = "Kshoot Chart File|*.ksh";
                browser.CheckFileExists = true;

                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                using (var converter = new ConverterForm(browser.FileName, ConvertMode.Converter))
                    converter.ShowDialog();
            }
        }

        private void OnBulkConvertToolsMenuClick(object sender, EventArgs e)
        {
            using (var browser = new FolderBrowserDialog())
            {
                browser.Description = "Select Kshoot chart repository";
                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                using (var converter = new ConverterForm(browser.SelectedPath, ConvertMode.Converter))
                    converter.ShowDialog();
            }
        }

        private void OnMusicFileBuilderClick(object sender, EventArgs e)
        {
            using (var browser  = new OpenFileDialog())
            using (var exporter = new SaveFileDialog())
            {
                browser.Filter = "Audio Files|*.wav;*.ogg;*.mp3;*.flac|2DX Music File|*.2dx;*.s3v|Music Files|*.wav;*.ogg;*.mp3;*.flac";
                browser.CheckFileExists = true;

                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                exporter.Filter = "2DX File|*.2dx";
                if (exporter.ShowDialog() != DialogResult.OK)
                    return;

                string error = string.Empty;
                using (var loader = new LoadingForm())
                {
                    loader.SetAction(() =>
                    {
                        try
                        {
                            loader.SetStatus("Processing assets..");

                            string source = browser.FileName;
                            string tmp = DxTool.ConvertToWave(source, false);
                            DxTool.Build(tmp, exporter.FileName);

                            Directory.Delete(tmp, true);

                            loader.SetProgress(100);
                            loader.DialogResult = DialogResult.OK;
                            loader.Complete();
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;

                            loader.DialogResult = DialogResult.Abort;
                            loader.Complete();
                        }
                    });

                    if (loader.ShowDialog() == DialogResult.OK)
                    {
                        MessageBox.Show(
                            "Audio file has been converted successfully",
                            "2DX Builder",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Failed to convert audio file.\n{error}",
                            "2DX Builder",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                    }
                }
            }
        }

        private void OnHelpHelpMenu_Click(object sender, EventArgs e)
        {
            using (var about = new HelpForm())
                about.ShowDialog();
        }

        private void OnAboutHelpMenuClick(object sender, EventArgs e)
        {
            using (var about = new AboutForm())
                about.ShowDialog();
        }

        private void OnExitFileMenuClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region -- Editor ---
        private void OnOpenButtonClick(object sender, EventArgs e)
        {
            try
            {
                if (!Pristine && SaveFileMenu.Enabled)
                {
                    var response = MessageBox.Show(
                        "Save file before open another mix?",
                        "Open Mix",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );

                    if (response == DialogResult.Yes)
                        SaveFileMenu.PerformClick();
                    else if (response == DialogResult.Cancel)
                        return;

                    Pristine = true;
                    actions.Clear();
                }

                using (var browser = new FolderBrowserDialog())
                using (var mixSelector = new MixSelectorForm())
                {
                    browser.ShowNewFolderButton = true;
                    browser.Description = "Select KFC Content Root";

                    if (browser.ShowDialog() == DialogResult.OK)
                    {
                        AssetManager.Initialize(browser.SelectedPath);

                        PathTextBox.Text = string.Empty;
                        MusicListBox.Items.Clear();

                        ResetEditor();

                        if (mixSelector.ShowDialog() == DialogResult.OK)
                            Reload();
                    }
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnAddNewMenuClick(object sender, EventArgs e)
        {
            string defaultAscii = AssetManager.MixName.ToLower().Replace(" ", string.Empty);
            var duplicates = new List<VoxHeader>();
            foreach (var h in AssetManager.Headers)
            {
                if (h.Ascii.StartsWith(defaultAscii))
                    duplicates.Add(h);
            }

            if (duplicates.Count > 0)
            {
                duplicates.Sort((a, b) => a.Ascii.CompareTo(b.Ascii));
                string ascii = $"{defaultAscii}_01";
                int counter = 1;
                foreach (var h in duplicates)
                {
                    if (h.Ascii == ascii)
                        ascii = $"{defaultAscii}_{++counter:D2}";
                    else
                        break;
                }

                defaultAscii = ascii;
            }

            var header = new VoxHeader()
            {
                Title            = "Untitled",
                Ascii            = defaultAscii,
                Artist           = "Unknown",
                Version          = GameVersion.ExceedGear,
                InfVersion       = InfiniteVersion.MXM,
                BackgroundId     = short.Parse(ConverterForm.LastBackground),
                GenreId          = 16,
                BpmMin           = 1,
                BpmMax           = 1,
                Volume           = 91,
                DistributionDate = DateTime.Now,
                Levels = new Dictionary<Difficulty, VoxLevelHeader>()
                {
                    { Difficulty.Infinite, new VoxLevelHeader() {} }
                }
            };

            Pristine = false;
            AssetManager.Headers.Add(header);
            MusicListBox.Items.Add(header);
        }

        private void OnSingleImportMenuClick(object sender, EventArgs e)
        {
            using (var browser = new OpenFileDialog())
            {
                browser.Filter = "KShoot Mania Chart|*.ksh";
                browser.CheckFileExists = true;

                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                using (var converter = new ConverterForm(browser.FileName, ConvertMode.Importer))
                {
                    if (converter.ShowDialog() != DialogResult.OK)
                        return;

                    var header = converter.Result;
                    if (!actions.ContainsKey(header.Ascii))
                        actions[header.Ascii] = new Queue<Action>();

                    Pristine = false;
                    AssetManager.Headers.Add(header);
                    MusicListBox.Items.Add(header);

                    actions[header.Ascii].Enqueue(converter.Action);
                    if (Autosave)
                        Save(AssetManager.MdbFilename);
                }
            }
        }

        private void OnBulkImportKshMenuClick(object sender, EventArgs e)
        {
            using (var browser = new FolderBrowserDialog())
            {
                browser.Description = "Select Kshoot chart repository";
                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                using (var converter = new ConverterForm(browser.SelectedPath, ConvertMode.BulkImporter))
                {
                    if (converter.ShowDialog() != DialogResult.OK)
                        return;

                    foreach (var header in converter.ResultSet)
                    {
                        if (!actions.ContainsKey(header.Ascii))
                            actions[header.Ascii] = new Queue<Action>();

                        AssetManager.Headers.Add(header);
                        MusicListBox.Items.Add(header);

                        actions[header.Ascii].Enqueue(converter.ActionSet[header.Ascii]);
                    }

                    Pristine = false;
                    if (Autosave)
                        Save(AssetManager.MdbFilename);
                }
            }
        }

        private void OnMetadataChanged(object sender, EventArgs e)
        {
            if (sender is TextBox textBox && !textBox.Modified)
                return;

            if (sender is Control control && !(control is ComboBox) && !control.ContainsFocus)
                return;

            if (!(MusicListBox.SelectedItem is VoxHeader header))
                return;

            if (int.TryParse(IdTextBox.Text, out int id))
            {
                // Validate ID
                if (!AssetManager.ValidateMusicID(id))
                {
                    IdTextBox.Text = header.ID.ToString();
                    MessageBox.Show("Music ID is already taken", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    header.ID = id;
            }
            else
                IdTextBox.Text = header.ID.ToString();

            double min = (double)Math.Min(BpmMinNumericBox.Value, BpmMaxNumericBox.Value);
            double max = (double)Math.Max(BpmMinNumericBox.Value, BpmMaxNumericBox.Value);
            BpmMinNumericBox.Value = (decimal)min;
            BpmMaxNumericBox.Value = (decimal)max;

            header.Title            = TitleTextBox.Text;
            header.Artist           = ArtistTextBox.Text;
            header.BpmMin           = min;
            header.BpmMax           = max;
            header.Version          = (GameVersion)(VersionDropDown.SelectedIndex + 1);
            header.InfVersion       = InfVerDropDown.SelectedIndex == 0 ? InfiniteVersion.MXM : (InfiniteVersion)(InfVerDropDown.SelectedIndex + 1);
            header.DistributionDate = DistributionPicker.Value;
            header.BackgroundId     = short.Parse((BackgroundDropDown.SelectedItem ?? "0").ToString().Split(' ')[0]);
            header.Volume           = (short)VolumeTrackBar.Value;

            VolumeIndicatorLabel.Text = $"{VolumeTrackBar.Value:#00}%";
            Pristine = false;
        }

        private void OnLevelEditButtonClick(object sender, EventArgs e)
        {
            if (MusicListBox.SelectedItem == null)
                return;

            var button = (Button)sender;
            var difficulty = (Difficulty)int.Parse(button.Tag.ToString());
            var header = MusicListBox.SelectedItem as VoxHeader;

            using (var editor = new LevelEditorForm(header, difficulty))
            {
                if (editor.ShowDialog() == DialogResult.OK)
                {
                    // Update the levels in case it's newly added
                    header.Levels[difficulty] = editor.Result;
                    if (editor.Action != null)
                    {
                        if (!actions.ContainsKey(header.Ascii))
                            actions[header.Ascii] = new Queue<Action>();

                        actions[header.Ascii].Enqueue(editor.Action);
                    }

                    if (header.Levels.ContainsKey(Difficulty.Infinite))
                        InfVerDropDown.Items[0] = "MXM";
                    else
                        InfVerDropDown.Items[0] = "--";

                    OnInfVerDropDownSelectedIndexChanged(sender, e);

                    Pristine = false;
                    LoadJacket(header);

                    if (Autosave)
                        Save(AssetManager.MdbFilename);
                }
            }
        }

        private void OnInfVerDropDownSelectedIndexChanged(object sender, EventArgs e)
        {
            var header = MusicListBox.SelectedItem as VoxHeader;
            if (header == null || InfVerDropDown.SelectedItem == null)
                InfEditButton.Text = "--";
            else if (header.Levels.ContainsKey(Difficulty.Infinite))
                InfEditButton.Text = InfVerDropDown.SelectedItem.ToString();
        }

        private void OnImport2DXMusicFileButtonClick(object sender, EventArgs e)
        {
            Import2DX();
            if (Autosave)
                Save(AssetManager.MdbFilename);
        }

        private void OnImport2DXPreviewFileButtonClick(object sender, EventArgs e)
        {
            Import2DX(true);
            if (Autosave)
                Save(AssetManager.MdbFilename);
        }

        private void OnJacketPictureBoxClick(object sender, EventArgs e)
        {
            var header = MusicListBox.SelectedItem as VoxHeader;
            if (header == null)
                return;

            var control    = (Control)sender;
            var difficulty = (Difficulty)int.Parse(control.Tag.ToString());
            if (!header.Levels.ContainsKey(difficulty))
                return;

            string jacket = $"{AssetManager.GetJacketPath(header, difficulty)}_b.png";
            if (!File.Exists(jacket))
            {
                jacket = $"{AssetManager.GetDefaultJacketPath(header)}_b.png";
                if (!File.Exists(jacket))
                    return;
            }

            using (var image  = Image.FromFile(jacket))
            using (var viewer = new JacketViewerForm(image))
                viewer.ShowDialog();
        }
        #endregion

        #region --- Mix List Management ---
        private void OnMusicListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            var header = MusicListBox.SelectedItem as VoxHeader;
            if (header == null)
            {
                MetadataGroupBox.Enabled = false;
                ResetEditor();

                return;
            }

            IdTextBox.Text                  = header.ID.ToString();
            TitleTextBox.Text               = VoxHeader.FixMappedChars(header.Title);
            ArtistTextBox.Text              = VoxHeader.FixMappedChars(header.Artist);
            BpmMinNumericBox.Value          = (decimal)header.BpmMin;
            BpmMaxNumericBox.Value          = (decimal)header.BpmMax;
            VersionDropDown.SelectedIndex   = (int)(header.Version) - 1;
            InfVerDropDown.SelectedIndex    = header.InfVersion == InfiniteVersion.MXM ? 0 : (int)(header.InfVersion) - 1;
            DistributionPicker.Value        = header.DistributionDate;
            VolumeTrackBar.Value            = header.Volume;
            BackgroundDropDown.SelectedItem = $"{header.BackgroundId:D2}";

            VolumeIndicatorLabel.Text = $"{VolumeTrackBar.Value:#00}%";
            if (header.Levels.ContainsKey(Difficulty.Infinite))
                InfVerDropDown.Items[0] = "MXM";
            else
                InfVerDropDown.Items[0] = "--";

            bool safe                        = !string.IsNullOrEmpty(AssetManager.MixName);
            AddButton.Enabled                = safe;
            AddEditMenu.Enabled              = safe;
            RemoveButton.Enabled             = safe;
            RemoveEditMenu.Enabled           = safe;
            Import2DXEditMenu.Enabled        = safe;
            Import2DXPreviewEditMenu.Enabled = safe;
            ExplorerEditMenu.Enabled         = true;
            EditMenu.Enabled                 = true;
            MetadataGroupBox.Enabled         = true;
            InfEditButton.Text               = InfVerDropDown.SelectedItem.ToString();
            
            LoadJacket(header);
        }

        private void OnRemoveButtonClick(object sender, EventArgs e)
        {
            var header = MusicListBox.SelectedItem as VoxHeader;
            if (header == null)
                return;

            var response = MessageBox.Show(
                $"Are you sure want to delete selected song?",
                "Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (response == DialogResult.No)
                return;

            AssetManager.Headers.Remove(header);
            MusicListBox.Items.Remove(header);
           
            // Clear pending modification, since this asset will be deleted anyway
            actions[header.Ascii] = new Queue<Action>();
            actions[header.Ascii].Enqueue(() => AssetManager.DeleteAssets(header));

            if (Autosave)
                Save(AssetManager.MdbFilename);
        }

        private void OnPathTextBoxTextChanged(object sender, EventArgs e)
        {
            MusicGroupBox.Enabled = !string.IsNullOrEmpty(PathTextBox.Text);
        }
        #endregion

        #region --- Functions ---

        private void LoadMusicDb()
        {
            using (var loader = new LoadingForm())
            {
                loader.SetAction(() =>
                {
                    MusicListBox.Items.Clear();
                    foreach (var header in AssetManager.Headers)
                    {
                        MusicListBox.Items.Add(header);

                        loader.SetStatus(header.Title);
                        loader.SetProgress(((float)MusicListBox.Items.Count / AssetManager.Headers.Count) * 100f);
                    }

                    loader.Complete();
                });


                loader.ShowDialog();
            }
        }

        private bool Save(string dbFilename)
        {
            var errors = new List<string>();
            using (var loader = new LoadingForm())
            {
                var proc = new Action(() =>
                {
                    float it = 1f;
                    foreach (var action in actions)
                    {
                        float progress = (it++ / actions.Count) * 100f;
                        loader.SetStatus($"[{progress:00}%] - Processing {action.Key} assets..");
                        loader.SetProgress(progress);

                        var queue = action.Value;
                        while (queue.Count > 0)
                        {
                            try
                            {
                                queue.Dequeue()?.Invoke();
                            }
                            catch (Exception ex)
                            {
                                errors.Add($"{action.Key}: {ex.Message}");
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }

                    loader.SetStatus("[100%] - Processing Music DB..");
                    loader.SetProgress(100f);

                    AssetManager.Headers.Save(dbFilename);
                    loader.Complete();
                });

                loader.SetAction(() => new Thread(() => proc()).Start());
                loader.ShowDialog();
            }

            if (errors.Count > 0)
            {
                string message = "Error occured when processing following assets:\n";
                foreach (var err in errors)
                    message += $"\n{err}";

                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            actions.Clear();
            Pristine = true;

            return errors.Count == 0;
        }

        private void Import2DX(bool preview = false)
        {
            var header = MusicListBox.SelectedItem as VoxHeader;
            if (header == null)
                return;

            using (var browser = new OpenFileDialog())
            {
                browser.Filter = "All supported format|*.2dx;*.s3v;*.wav;*.ogg;*.mp3;*.flac|2DX Music File|*.2dx;*.s3v|Music Files|*.wav;*.ogg;*.mp3;*.flac";
                browser.CheckFileExists = true;

                if (browser.ShowDialog() != DialogResult.OK)
                    return;

                string source = browser.FileName;
                string tmp = Path.Combine(
                    Path.GetTempPath(),
                    $"{Path.GetRandomFileName()}{new FileInfo(source).Extension}"
                );

                File.Copy(source, tmp);
                if (!actions.ContainsKey(header.Ascii))
                    actions[header.Ascii] = new Queue<Action>();

                actions[header.Ascii].Enqueue(() => AssetManager.Import2DX(tmp, header, preview));
            }
        }

        private void LoadJacket(VoxHeader header)
        {
            string defaultJacket = $"{AssetManager.GetDefaultJacketPath(header)}_s.png";
            foreach (Difficulty diff in Enum.GetValues(typeof(Difficulty)))
            {
                PictureBox picture;
                switch (diff)
                {
                    case Difficulty.Novice:   picture = JacketNovPictureBox; break;
                    case Difficulty.Advanced: picture = JacketAdvPictureBox; break;
                    case Difficulty.Exhaust:  picture = JacketExhPictureBox; break;
                    default:                  picture = JacketInfPictureBox; break;
                }

                if (!header.Levels.ContainsKey(diff))
                {
                    picture.Image = DummyJacket;
                    continue;
                }

                try
                {
                    string filename = $"{AssetManager.GetJacketPath(header, diff)}_s.png";
                    if (File.Exists(filename) && (Pristine || header.Levels[diff].Jacket == null))
                    {
                        using (var image = Image.FromFile(filename))
                            picture.Image = new Bitmap(image);

                        if (header.Levels[diff].Jacket != null) // clear cache
                        {
                            header.Levels[diff].Jacket.Dispose();
                            header.Levels[diff].Jacket = null; 
                        }
                    }
                    else if (header.Levels[diff].Jacket != null)
                    {
                        // use cache for new + unsaved header
                        picture.Image = header.Levels[diff].Jacket;
                    }
                    else if (File.Exists(defaultJacket))
                    {
                        using (var image = Image.FromFile(defaultJacket))
                            picture.Image = new Bitmap(image);                    
                    }
                    else
                        picture.Image = DummyJacket;
                }
                catch (Exception)
                {
                    picture.Image = DummyJacket;
                }
            }
        }

        private void Reload()
        {
            PathTextBox.Text = AssetManager.MixPath;
            MetadataGroupBox.Enabled = false;

            ResetEditor();
            DisableUI();

            LoadMusicDb();
            EnableUI();
        }

        private void EnableUI()
        {
            UpdateUI(true);
        }

        private void DisableUI()
        {
            UpdateUI(false);
            ResetEditor();
        }

        private void UpdateUI(bool state)
        {
            // Dont break your goddamn kfc
            bool safe = !string.IsNullOrEmpty(AssetManager.MixName);

            // Container
            MusicGroupBox.Enabled     = state;

            // Menu
            SaveFileMenu.Enabled      = state && safe; 
            SaveAsFileMenu.Enabled    = state;
            ChangeMixFileMenu.Enabled = state;
            DeleteMixFileMenu.Enabled = state && safe;
            AddButton.Enabled         = state && safe;
            AddEditMenu.Enabled       = state && safe;
            RemoveButton.Enabled      = state && safe;
            RemoveEditMenu.Enabled    = state && safe;
            EditMenu.Enabled          = state && safe;
            Import2DXEditMenu.Enabled = state && safe;
            Import2DXPreviewEditMenu.Enabled = state && safe;

            foreach (Control control in MetadataGroupBox.Controls)
            {
                if (control is TextBox textBox)
                    textBox.ReadOnly = !safe;
                else if (control is NumericUpDown numeric)
                    numeric.Enabled = safe;
                else if (!(control is PictureBox))
                    control.Enabled = safe;
            }

            // Modifying this could lead into disaster, must be left untouched
            IdTextBox.ReadOnly    = true;
            LevelGroupBox.Enabled = true;
            foreach (Control control in LevelGroupBox.Controls)
            {
                if (!(control is PictureBox))
                    control.Enabled = safe;
                else
                    control.Enabled = true;
            }
        }

        private void ResetEditor()
        {
            foreach (Control control in MetadataGroupBox.Controls)
            {
                if (control is TextBox)
                    control.Text                       = "";
                else if (control is NumericUpDown)
                    (control as NumericUpDown).Value   = 1;
                else if (control is ComboBox)
                    (control as ComboBox).SelectedItem = null;
            }

            foreach (Control control in LevelGroupBox.Controls)
            {
                if (control is PictureBox)
                {
                    var pictureBox = control as PictureBox;
                    pictureBox.Image = DummyJacket;
                }
                else if (control is Button && control.Tag.ToString() == "4")
                {
                    control.Text = "--";
                }
            }

            DistributionPicker.Value = DateTime.Now;
            VolumeTrackBar.Value     = DefaultVolume;

            EditMenu.Enabled                 = false;
            Import2DXEditMenu.Enabled        = false;
            Import2DXPreviewEditMenu.Enabled = false;
            ExplorerEditMenu.Enabled         = false;
        }
        #endregion

    }
}
