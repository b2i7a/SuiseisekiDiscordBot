using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Discord;

namespace SuiseisekiDiscordBot.Modules
{
    public class ChatModule : ModuleBase<SocketCommandContext>
    {
        private readonly AIService _aiService;

        public ChatModule(AIService aiService)
        {
            _aiService = aiService;
        }

        [Command("sui", RunMode = RunMode.Async)]
        public async Task SayAsync([Remainder] string text)
        {
            var answer = _aiService.GetAnswer(Context.User.GlobalName, text);

            await Context.Message.ReplyAsync(answer);
        }
    }
}
