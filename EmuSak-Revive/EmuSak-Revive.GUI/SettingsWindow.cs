﻿using Bunifu.UI.WinForms;
using EmuSak_Revive.GUI.Generics;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmuSak_Revive.GUI
{
    public partial class SettingsWindow : Form
    {
        public string PortableYuzuPath { get => Properties.Settings.Default.PortableYuzuPath; }
        public string PortableRyujinxPath { get => Properties.Settings.Default.PortableRyujinxpath; }
        public string ShaderUrl { get => Properties.Settings.Default.ShaderLinks; }
        public bool PortableYuzu { get => Properties.Settings.Default.PortableYuzu; }
        public bool PortableRyujinx { get => Properties.Settings.Default.PortableRyujinx; }

        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void LoadSettings()
        {
            YuzuPath_TextBox.Text = Properties.Settings.Default.PortableYuzuPath;
            RyuPath_TextBox.Text = Properties.Settings.Default.PortableRyujinxpath;
            PasteBinUrl_TextBox.Text = Properties.Settings.Default.ShaderLinks;
            PlaySounds_CheckBox.Checked = Properties.Settings.Default.PlaySounds;
            //To ensure that the images aren't rounded.
            Ryujinx_Image.BorderRadius = 0;
            Yuzu_Image.BorderRadius = 0;
            MainWindow_AudioSlider.Value = Properties.Settings.Default.MainWindowVolume;
        }

        private void SaveSettings()
        {
            if (!string.IsNullOrWhiteSpace(YuzuPath_TextBox.Text))
            {
                Properties.Settings.Default.PortableYuzuPath = YuzuPath_TextBox.Text;
                Properties.Settings.Default.PortableYuzu = true;
            }

            if (!string.IsNullOrWhiteSpace(RyuPath_TextBox.Text))
            {
                Properties.Settings.Default.PortableRyujinxpath = RyuPath_TextBox.Text;
                Properties.Settings.Default.PortableRyujinx = true;
            }

            if (string.IsNullOrWhiteSpace(YuzuPath_TextBox.Text))
            {
                Properties.Settings.Default.PortableYuzuPath = string.Empty;
                Properties.Settings.Default.PortableYuzu = false;
            }

            if (string.IsNullOrWhiteSpace(RyuPath_TextBox.Text))
            {
                Properties.Settings.Default.PortableRyujinxpath = string.Empty;
                Properties.Settings.Default.PortableRyujinx = false;
            }
            Properties.Settings.Default.ShaderLinks = PasteBinUrl_TextBox.Text;
            Network.Networking.ShaderUrl = ShaderUrl;
            Properties.Settings.Default.PlaySounds = PlaySounds_CheckBox.Checked;
            Properties.Settings.Default.MainWindowVolume = MainWindow_AudioSlider.Value;
            Properties.Settings.Default.Save();
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            UI.ChangeToDarkMode(this);
            LoadSettings();
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            Hide();
        }

        private void OpenFolder(BunifuTextBox pathBox)
        {
            var opener = new CommonOpenFileDialog { IsFolderPicker = true };

            if (opener.ShowDialog() == CommonFileDialogResult.Ok)
            {
                pathBox.Text = opener.FileName;
            }
        }

        private void SelectYuzuPath_Button_Click(object sender, EventArgs e)
        {
            OpenFolder(YuzuPath_TextBox);
        }

        private void SelectRyuPath_Button_Click(object sender, EventArgs e)
        {
            OpenFolder(RyuPath_TextBox);
        }

        private void SaveAndClose_Button_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
        }

        private void Help_Button_Click(object sender, EventArgs e)
        {
            MessageBox.Show("If you want to select a portable install make sure to select the 'user' (yuzu)/'portable'(ryujinx)" +
                " folder of your portable emu install.\n\n" +
                "If you want to set the shaders paste make sure to paste a valid paste in! " +
                "It can be any raw file on the web that contains the links and game names to the shaders.",
                "Info",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void MainWindow_AudioSlider_Scroll(object sender, Utilities.BunifuSlider.BunifuHScrollBar.ScrollEventArgs e)
        {
            MainWindow.mainWindowPlayer.Volume = MainWindow_AudioSlider.Value / 100f;
        }
    }
}