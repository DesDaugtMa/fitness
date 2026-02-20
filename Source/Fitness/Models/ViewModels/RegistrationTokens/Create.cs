namespace Fitness.Models.ViewModels.RegistrationTokens
{
    public class Create
    {
        public required string Token { get; set; }

        public int AddDaysForExpiration { get; set; }

        public required string Description { get; set; }
    }
}
