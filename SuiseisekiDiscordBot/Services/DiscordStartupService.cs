namespace SuiseisekiDiscordBot.Services
{
    public class DiscordStartupService : IHostedService
    {
        private readonly DiscordSocketClient _discord;
        private readonly IConfiguration _config;

        public DiscordStartupService(
            DiscordSocketClient discord,
            IConfiguration config,
            ILogger<DiscordSocketClient> logger)
        {
            _discord = discord;
            _config = config;

            _discord.Log += msg => LogHelper.OnLogAsync(logger, msg);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _discord.LoginAsync(TokenType.Bot, _config["DISCORD_TOKEN"]);
            await _discord.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discord.LogoutAsync();
            await _discord.StopAsync();
        }
    }
}
