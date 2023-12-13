using System.ComponentModel.Design;

namespace SuiseisekiDiscordBot.Services
{
    public class DiscordCommandService : IHostedService
    {
        private readonly DiscordSocketClient _discord;
        private readonly AIService _aiService;
        private readonly CommandService _commands;
        private readonly IServiceProvider _serviceProvider;

        public DiscordCommandService(
            DiscordSocketClient discord,
            AIService aiService,
            CommandService commands)
        {
            _discord = discord;
            _aiService = aiService;
            _commands = commands;

            _serviceProvider = new ServiceCollection()
                .AddSingleton(_aiService)
                .BuildServiceProvider();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _discord.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: _serviceProvider);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null)
            {
                return;
            }

            var argPos = 0;

            if (!(message.HasCharPrefix('!', ref argPos) ||
                  message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
            {
                return;
            }

            var context = new SocketCommandContext(_discord, message);

            await _commands.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _serviceProvider);
        }
    }
}
