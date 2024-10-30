namespace MovieWave.Domain.Settings
{
	public class GoogleAuthSettings
	{
		public const string DefaultSection = "Authentication:Google";

		public string ClientId { get; set; }
		public string ClientSecret { get; set; }
		public string CallbackPath { get; set; } = "/signin-google";
	}
}