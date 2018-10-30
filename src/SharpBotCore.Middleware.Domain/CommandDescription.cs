namespace SharpBotCore.Middleware.Domain
{
	public class CommandDescription
	{
		public string Command { get; set; }

		public string Description { get; set; }

		/// <summary>
		/// Example of how to use the command. Use "@{bot}" without the quotes to have the bots name automatically filled in.
		/// </summary>
		public string Example { get; set; }
	}
}
