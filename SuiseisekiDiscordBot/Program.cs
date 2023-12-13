using Azure.AI.OpenAI;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddJsonFile("appsettings.json");

var openAIClient = new OpenAIClient(builder.Configuration.GetSection("OPENAI_KEY").Value!);

DiscordSocketConfig discordSocketConfig = new();

builder.Configuration.GetSection(nameof(DiscordSocketConfig)).Bind(discordSocketConfig);
DiscordSocketClient discordSocketClient = new(discordSocketConfig);

builder.Services
    .AddSingleton(openAIClient)
    .AddSingleton<AIService>()
    .AddSingleton(discordSocketClient)
    .AddSingleton<CommandService>()
    .AddHostedService<DiscordCommandService>()
    .AddHostedService<DiscordStartupService>()
    .AddHostedService<HttpListenerService>();

using IHost host = builder.Build();

await host.RunAsync();
