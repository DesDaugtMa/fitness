namespace Fitness.Config
{
    public class AppSettings
    {
        public ConnectionStrings? ConnectionStrings { get; set; }
        public GoogleAuthSettings? GoogleAuthSettings { get; set; }
        public Version? Version { get; set; }
    }

    public class ConnectionStrings
    {
        public string? Default { get; set; }
    }

    public class GoogleAuthSettings
    {
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
    }

    public class Version
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
    }
}