namespace BLL.Options
{
	public class EmailOptions
	{
		public static string Name => "Email";

		public string Host { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }
	}
}
