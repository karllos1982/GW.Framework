using GW.Common;
using GW.ApplicationHelpers;

namespace GW.Core
{
    public interface ISettings
    {

        public SourceConfig[] Sources { get; set; }

        public string SiteURL { get; set; }

        public string ProfileImageDir { get; set; }

        public string ApplicationName { get; set; }

        public MailSettings MailSettings { get; set; }

    }
}
