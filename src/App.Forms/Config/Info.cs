namespace App.Forms.Config
{
    public class Info
    {
        public static Version GetVersion()
        {
            return typeof(Info).Assembly.GetName().Version;
        }

        public static string GetVersionString()
        {
            Version version = Info.GetVersion();
            string versionstring = version.Major + "." + version.Minor +
                "." + version.Build + "." + version.Revision;
            return versionstring;
        }
    }
}
