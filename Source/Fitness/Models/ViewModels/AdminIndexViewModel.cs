namespace Fitness.Models.ViewModels
{
    public class AdminIndexViewModel
    {
        public List<AdminIndexViewModelRegistrationTokens> AdminIndexViewModelRegistrationTokens { get; set; } = new List<AdminIndexViewModelRegistrationTokens>();
    }

    public class AdminIndexViewModelRegistrationTokens
    {
        public string Token { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? UsedByUser { get; set; }
        public string? Description { get; set; }
    }
}
