namespace App.Forms.Config
{
    public class InfoHeader
    {
        public bool IsProductionEnvironment { get; set; } = false;
        public string? Url { get; set; }
        public string? Version { get; set; }
        public DateTime LastUpdate { get; set; }
        public string? Environment { get; set; }
    }
}