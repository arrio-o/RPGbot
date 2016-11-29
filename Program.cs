using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using RPGbot.Modules;

namespace RPGbot
{
    class RPGbot
    {
        public static DiscordClient Client { get; private set; }

        static void Main(string[] args)
        {
            GlobalConfig.Load();

            Client = new DiscordClient(new DiscordConfigBuilder()
            {
                MessageCacheSize = 10,
                ConnectionTimeout = 60000,
                LogLevel = LogSeverity.Debug,
                LogHandler = (s, e) =>
                    Console.WriteLine($"Severity: {e.Severity}" +
                                      $"Message: {e.Message}" +
                                      $"ExceptionMessage: {e.Exception?.Message ?? "-"}"),
            })
            .UsingCommands(x =>
            {
                x.AllowMentionPrefix = true;
                x.PrefixChar = '!';
                x.HelpMode = HelpMode.Public;
                x.ExecuteHandler = (s, e) =>
                {
                    Client.Log.Info("Command", $"{e.Command.Text} ({e.User.Name})");
                };
                x.ErrorHandler = async (s, e) =>
                {
                    if (e.ErrorType != CommandErrorType.BadPermissions)
                        return;
                    if (string.IsNullOrWhiteSpace(e.Exception?.Message))
                        return;
                    try
                    {
                        Client.Log.Error("Error", $"{e.Exception.Message}");
                        await e.Channel.SendMessage(e.Exception.Message).ConfigureAwait(false);
                    }
                    catch { }
                };
            })
            .UsingModules()
            .UsingPermissionLevels(PermissionResolver); ;
     
            //TODO: удалить - только для отладки
            Client.MessageReceived += async (s, e) =>
            {
                if (!e.Message.IsAuthor)
                    Console.WriteLine(e.User.Name + " said " + e.Message.Text);
                if (e.Message.Text.StartsWith("--test"))
                {                    
                    Role role = e.Server.Roles.Where(x => x.Name == "PartyMember").FirstOrDefault();
                    try
                    {
                        //User user = e.Server.FindUsers("RPGbot").FirstOrDefault();
                        await e.User.AddRoles(new Role[] { role });

                        //await e.User.Edit(roles: (new Role[] { role }));
                    }
                    catch (Exception exc)
                    {
                        await e.Channel.SendMessage(exc.Message);
                    }
                }
            };

            //Client.AddModule<ColorsModule>("Colors", ModuleFilter.None);//.ServerWhitelist);
            Client.AddModule<RPGModule>("RPG", ModuleFilter.None);//.ServerWhitelist);

            Client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await Client.Connect(GlobalConfig.Discord.Token, TokenType.Bot);
                        Client.SetGame("test");
                        //await _client.ClientAPI.Send(new Discord.API.Client.Rest.HealthRequest());
                        break;
                    }
                    catch (Exception ex)
                    {
                        Client.Log.Error($"Login Failed", ex);
                        await Task.Delay(Client.Config.FailedReconnectDelay);
                    }
                }
            });
        }

        public static int PermissionResolver(User user, Channel channel)
        {
            if (user.Id == GlobalConfig.Users.DevId)
                return (int)PermissionLevel.BotOwner;
            if (user.Server != null)
            {
                if (user == channel.Server.Owner)
                    return (int)PermissionLevel.ServerOwner;

                var serverPerms = user.ServerPermissions;
                if (serverPerms.ManageRoles)
                    return (int)PermissionLevel.ServerAdmin;
                if (serverPerms.ManageMessages && serverPerms.KickMembers && serverPerms.BanMembers)
                    return (int)PermissionLevel.ServerModerator;

                var channelPerms = user.GetPermissions(channel);
                if (channelPerms.ManagePermissions)
                    return (int)PermissionLevel.ChannelAdmin;
                if (channelPerms.ManageMessages)
                    return (int)PermissionLevel.ChannelModerator;
            }
            return (int)PermissionLevel.User;
        }
    }
}
