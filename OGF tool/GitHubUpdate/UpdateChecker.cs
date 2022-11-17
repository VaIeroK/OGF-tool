using System;
using System.Linq;
using System.Windows.Forms;

using Octokit;
using Semver;
using System.Threading.Tasks;

namespace GitHubUpdate
{
    public enum UpdateType
    {
        None,
        Major,
        Minor,
        Patch
    }

    public class UpdateChecker
    {
        IReleasesClient releaseClient;
        internal GitHubClient github;

        internal SemVersion currentVersion;
        internal string repositoryOwner;
        internal string repostoryName;
        internal Release latestRelease;

        void init(string owner, string name, SemVersion version)
        {
            github = new GitHubClient(new ProductHeaderValue(name + "-UpdateCheck"));
            releaseClient = github.Release;

            repositoryOwner = owner;
            repostoryName = name;
            currentVersion = version;
        }

        public UpdateChecker(string owner, string name)
        {
            Helper.ArgumentNotNullOrEmptyString(owner, "owner");
            Helper.ArgumentNotNullOrEmptyString(name, "name");

            string version = System.Windows.Forms.Application.ProductVersion;

            version = version.Substring(0, version.LastIndexOf('.'));

            init(owner, name, version);
        }

        public UpdateChecker(string owner, string name, string version)
        {
            Helper.ArgumentNotNullOrEmptyString(owner, "owner");
            Helper.ArgumentNotNullOrEmptyString(name, "name");
            Helper.ArgumentNotNullOrEmptyString(version, "version");

            init(owner, name, Helper.StripInitialV(version));
        }

        public async Task<UpdateType> CheckUpdate()
        {
            var releases = await releaseClient.GetAll(repositoryOwner, repostoryName);
            latestRelease = releases.FirstOrDefault(release => !release.Prerelease && Helper.StripInitialV(release.TagName) > currentVersion);

            UpdateType type = UpdateType.None;
            string tagName = null, body = null;
            if (latestRelease != null)
            {
                tagName = latestRelease.TagName;
                body = latestRelease.Body;

                var latestVersion = Helper.StripInitialV(tagName);
                if (latestVersion.Major != currentVersion.Major)
                    type = UpdateType.Major;
                else if (latestVersion.Minor != currentVersion.Minor)
                    type = UpdateType.Minor;
                else if (latestVersion.Patch != currentVersion.Patch)
                    type = UpdateType.Patch;
            }

            return type;
        }

        public async Task<string> RenderReleaseNotes()
        {
            if (latestRelease == null)
                throw new InvalidOperationException();

            return await github.Miscellaneous.RenderRawMarkdown(latestRelease.Body);
        }

        public async void DownloadAsset(string assetname)
        {
            assetname += latestRelease.TagName + ".rar";
            string url = $"https://github.com/{repositoryOwner}/{repostoryName}/releases/download/{latestRelease.TagName}/{assetname}";
            System.Diagnostics.Process.Start(url);
        }
    }
}
