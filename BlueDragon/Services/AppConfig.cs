namespace BlueDragon.Services
{
    public class AppConfig
    {
        public string CurrentPageTitle { get; set; } = "";
        public string FullPageTitle => $"Blue Dragon HCCP ({CurrentPageTitle}) | Ox Studios";
    }
}
