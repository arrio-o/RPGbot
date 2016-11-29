using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RPGbot.Modules
{
    internal partial class RPGModule : IModule
    {
        private class RoleDefinition
        {
            public string Id;
            public string Name;
            public Color Color;
            public RoleDefinition(string name, Color color)
            {
                Name = name;
                Id = name.ToLowerInvariant();
                Color = color;
            }
        }

        private readonly List<RoleDefinition> _roles;
        private List<Character> _allCharacters = new List<Character>();
        private readonly Dictionary<string, RoleDefinition> _rolesMap;
        private ModuleManager _manager;
        private DiscordClient _client;
        private Party _party = Party.Instance;

        public RPGModule()
        {
            _roles = new List<RoleDefinition>()
            {
                new RoleDefinition("GM", Color.Blue),
                new RoleDefinition("PartyLeader", Color.Gold),
                new RoleDefinition("PartyMember", Color.Default),                
            };
            _rolesMap = _roles.ToDictionary(x => x.Id);
        }

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;
            //TODO: заменить лямбды на методы
            manager.CreateCommands("party", group =>
            {
                group.CreateCommand("list")
                    .Alias(new string[] { "список", "игроки" })
                    .Description("Gives a list of all party members.")
                    .Do(async e =>
                    {
                        string text = "NotImplemented";
                        await _client.Reply(e, text);
                    });
                group.CreateCommand("invite")
                    .Alias(new string[] { "пригласить" })
                    .Parameter("user", ParameterType.Required)
                    //.MinPermissions((int)PermissionLevel.BotOwner)
                    .Description("Invites player to party./ Пригласить игрока в группу.")
                    .Do(async e =>
                   {
                       try
                       {
                           User user = e.Server.FindUsers(e.Args[0]).FirstOrDefault();
                           if (user == null)
                               await _client.ReplyError(e, "Unknown user");
                           else
                           {

                               Character userCharacter = _allCharacters.Where(x => x?.OwnerId == e.User.Id).FirstOrDefault();
                               if (userCharacter != null)
                               {
                                   _party.AddMember(userCharacter);
                                   await _client.Reply(e, $"Персонаж '{userCharacter?.Name}' игрока {user} добавлен в группу");
                               }
                               await AddRole(e, user, "PartyMember");
                           }
                       }
                       catch (System.Exception exc)
                       {
                           await _client.Reply(e, exc.Message);
                       }
                   });
                group.CreateCommand("banish")
                    .Alias(new string[] { "изгнать", "выгнать", "shoo", "out" })
                    .Parameter("user", ParameterType.Required)
                    .Description("Removes player from party./ Выгнать игрока из группы.")
                    .Do(async e =>
                    {
                        if (!e.Server.CurrentUser.ServerPermissions.ManageRoles)
                        {
                            await _client.ReplyError(e, "This command requires the bot have Manage Roles permission.");
                            return;
                        }
                        User user = e.Server.FindUsers(e.Args[0]).FirstOrDefault();
                        //var otherRoles = GetOtherRoles(e.User);
                        Role role = e.Server.Roles.Where(x => x.Name == "PartyMember").FirstOrDefault();
                        if (role != null)
                        {
                            await user.RemoveRoles(role);
                            await _client.Reply(e, $"Banishing player from party.");
                        }

                        Character userCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
                        if (userCharacter != null)
                        {
                            _party.RemoveMember(userCharacter);
                            await _client.Reply(e, $"Персонаж '{userCharacter?.Name}' игрока {user} выгнан из группы");
                        }
                    });
            });
            manager.CreateCommands("character", command => {
                command.CreateCommand("create")
                    .Alias(new string[] { "создать" })
                    .Parameter("Name", ParameterType.Required)
                    .Parameter("Race", ParameterType.Required)
                    .Description("Создание персонажа: Имя Раса")
                    .Do(async e =>
                    {
                        await CreateCharacter(e);
                    });
                command.CreateCommand("unload")
                    .Alias(new string[] { "выгрузить" })                    
                    .Description("")
                    .Do(async e =>
                    {
                        await UnloadCharacter(e);
                    });
                command.CreateCommand("check")
                    .Alias(new string[] { "my", "me", "я", "мой" })
                    .Description("")
                    .Do(async e =>
                    {
                        await CheckCharacter(e);
                    });

                command.CreateCommand("readraw")
                    .Alias(new string[] { "raw", "файл"})
                    .Description("Чтение сырых данных из файла персонажа")
                    .Do(async e =>
                    {
                        await ReadRawCharacter(e);
                    });
                command.CreateCommand("load")
                    .Alias(new string[] { "загрузить", "открыть" })
                    .Parameter("Name")
                    .Description("Загрузить персонажа")
                    .Do(async e =>
                    {
                        await LoadCharacter(e);
                    });
            });
        }

        private IEnumerable<Role> GetOtherRoles(User user)
            => user.Roles.Where(x => !_rolesMap.ContainsKey(x.Name.ToLowerInvariant()));

        private async Task AddRole(CommandEventArgs e, User user, string roleName)
        {
            RoleDefinition partyRole;
            if (!_rolesMap.TryGetValue(roleName.ToLowerInvariant(), out partyRole))
            {
                await _client.ReplyError(e, "Unknown role");
                return;
            }
            if (!e.Server.CurrentUser.ServerPermissions.ManageRoles)
            {
                await _client.ReplyError(e, "This command requires the bot have Manage Roles permission.");
                return;
            }
            Role role = e.Server.Roles.Where(x => x.Name == partyRole.Name).FirstOrDefault();
            if (role == null)
            {
                role = await e.Server.CreateRole(partyRole.Name);
                await role.Edit(permissions: ServerPermissions.None, color: partyRole.Color);
            }
            var otherRoles = GetOtherRoles(user);
            await user.Edit(roles: otherRoles.Concat(new Role[] { role }));
            await _client.Reply(e, $"Adding {partyRole.Name} role to {user.Name}");
        }       
    }
}