using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPGbot.Modules
{
    internal class RPGModule : IModule
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
        private readonly Dictionary<string, RoleDefinition> _rolesMap;
        private ModuleManager _manager;
        private DiscordClient _client;

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
                    .Parameter("user")
                    //.MinPermissions((int)PermissionLevel.BotOwner)
                    .Description("Invites player to party./ Пригласить игрока в группу.")
                    .Do( e =>
                    {
                        User user = e.Server.FindUsers(e.Args[0]).FirstOrDefault();
                        if (user == null)
                            return _client.ReplyError(e, "Unknown user");
                        return AddRole(e, user, "PartyMember");
                    });
                group.CreateCommand("banish")
                    .Alias(new string[] { "изгнать", "выгнать", "shoo", "out" })
                    .Parameter("user")
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