using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cardinal.Plugins
{
    public partial class AgilWindow : Form
    {
        private Dictionary<string, Image> images = new Dictionary<string, Image>();
        public AgilPlugin Plugin { get; set; }

        public AgilWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            foreach (string path in Directory.EnumerateFiles(Directories.AGIL_IMAGES, "*.png"))
            {
                string name = Path.GetFileNameWithoutExtension(path);
                if (name == null) { continue; }
                images[name] = Image.FromFile(path);
            }

            BackgroundImage = images["Agil Background"];
            SettingsButton.Image = images["Settings Normal"];
            WeightsButton.Image = images["Weights Normal"];
            ReloadButton.Image = images["Reload Normal"];
        }

        #region Settings Button
        private void SettingsButonHoverEnter(object sender, EventArgs e)
        {
            SettingsButton.Image = images["Settings Highlight"];
        }

        private void SettingsButonHoverExit(object sender, EventArgs e)
        {
            SettingsButton.Image = images["Settings Normal"];
        }

        private void SettingsButtonReleased(object sender, MouseEventArgs e)
        {
            SettingsButton.Image = images["Settings Highlight"];
        }

        private void SettingsButtonPressed(object sender, MouseEventArgs e)
        {
            SettingsButton.Image = images["Settings Pressed"];
        }

        private void SettingsButtonClick(object sender, EventArgs e)
        {
            if (!File.Exists(Directories.AGIL_SETTINGS))
            {
                var settings = new AgilSettings();
                DataLoader.YamlSave(Directories.AGIL_SETTINGS, settings);
            }

            Process.Start("notepad.exe", Directories.AGIL_SETTINGS);
        }
        #endregion

        #region Weights Button
        private void WeightsButonHoverEnter(object sender, EventArgs e)
        {
            WeightsButton.Image = images["Weights Highlight"];
        }

        private void WeightsButonHoverExit(object sender, EventArgs e)
        {
            WeightsButton.Image = images["Weights Normal"];
        }

        private void WeightsButtonReleased(object sender, MouseEventArgs e)
        {
            WeightsButton.Image = images["Weights Highlight"];
        }

        private void WeightsButtonPressed(object sender, MouseEventArgs e)
        {
            WeightsButton.Image = images["Weights Pressed"];
        }

        private void WeightsButtonClick(object sender, EventArgs e)
        {
            if (!File.Exists(Directories.STAT_WEIGHTS_YAML))
            {
                var weights = new List<StatWeights>();
                DataLoader.YamlSave(Directories.STAT_WEIGHTS_YAML, weights);
            }

            Process.Start("notepad.exe", Directories.STAT_WEIGHTS_YAML);
        }
        #endregion

        #region Reload Button
        private void ReloadButonHoverEnter(object sender, EventArgs e)
        {
            ReloadButton.Image = images["Reload Highlight"];
        }

        private void ReloadButonHoverExit(object sender, EventArgs e)
        {
            ReloadButton.Image = images["Reload Normal"];
        }

        private void ReloadButtonReleased(object sender, MouseEventArgs e)
        {
            ReloadButton.Image = images["Reload Highlight"];
        }

        private void ReloadButtonPressed(object sender, MouseEventArgs e)
        {
            ReloadButton.Image = images["Reload Pressed"];
        }

        private void ReloadButtonClick(object sender, EventArgs e)
        {
            Plugin.Reload();
        }
        #endregion
    }
}
