namespace Fitness.Config
{
    public class AppSettings
    {
        public ConnectionStrings? ConnectionStrings { get; set; }
        public Version? Version { get; set; }
    }

    public class ConnectionStrings
    {
        public string? Default { get; set; }
    }

    public class Version
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
    }
}