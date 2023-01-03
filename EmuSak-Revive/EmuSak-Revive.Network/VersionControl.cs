﻿using EmuSak_Revive.ConfigIni.Core;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmuSak_Revive.Network
{
    public static class VersionControl
    {
        public static Version InstalledVersion { get; private set; }
        public static Version NewVersion { get; private set; }

        private static IniParser iniParser = new IniParser("./updaterConfig.ini");

        public static async Task<bool> CheckGitHubNewerVersion(Assembly refAsm)
        {
            string workspaceName = iniParser.GetSetting("config", "author"); //"Glumboi";
            string repositoryName = iniParser.GetSetting("config", "repo");//"GlumSak";

            //Get all releases from GitHub
            //Source: https://octokitnet.readthedocs.io/en/latest/getting-started/
            var client = new GitHubClient(new ProductHeaderValue(repositoryName));
            var releases = await client.Repository.Release.GetAll(workspaceName, repositoryName);

            //Setup the versions
            var latestGitHubVersion = new Version(releases[0].TagName);
            var Reference = refAsm.GetName();
            var Version = Reference.Version;
            var localVersion = new Version(Version.ToString()); //Replace this with your local version.
            InstalledVersion = localVersion;
            NewVersion = latestGitHubVersion;
            //Only tested with numeric values.

            //Compare the Versions
            //Source: https://stackoverflow.com/questions/7568147/compare-version-numbers-without-using-split-function
            var versionComparison = localVersion.CompareTo(latestGitHubVersion);
            if (versionComparison < 0)
            {
                //The version on GitHub is more up to date than this local release.
                return true;
            }
            else if (versionComparison > 0)
            {
                //This local version is greater than the release version on GitHub.
                return false;
            }
            else
            {
                //This local Version and the Version on GitHub are equal.
                return false;
            }
        }
    }
}