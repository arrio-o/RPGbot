using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPGbot.Modules
{
    /// <summary> Creates a role for each built-in color and allows users to freely select them. </summary>
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
                new RoleDefinition("GroupLeader", Color.Gold),
                new RoleDefinition("GroupMember", Color.Default),                
            };
            _rolesMap = _roles.ToDictionary(x => x.Id);
        }

        void IModule.Install(ModuleManager manager)
        {
            _manager = manager;
            _client = _manager.Client;

            manager.CreateCommands("group", group =>
            {
                group.CreateCommand("list")
                    .Alias(new string[] { "список", "игроки" })
                    .Description("Gives a list of all group members.")
                    .Do(async e =>
                    {
                        string text = "NotImplemented";
                        await _client.Reply(e, text);
                    });
                group.CreateCommand("invite")
                    .Alias(new string[] { "пригласить" })
                    .Parameter("user")
                    //.MinPermissions((int)PermissionLevel.BotOwner)
                    .Description("Invites player to group.")
                    .Do( e =>
                    {
                        User user = e.Server.FindUsers(e.Args[0]).FirstOrDefault();
                        if (user == null)
                            return _client.ReplyError(e, "Unknown user");
                        //string text = "NotImplemented";
                        //await _client.Reply(e, text);
                        return AddRole(e, user, "GroupMember");
                    });
                group.CreateCommand("banish")
                    .Alias(new string[] { "изгнать", "выгнать", "shoo", "out" })
                    .Parameter("user")
                    .Description("Removes player from group.")
                    .Do(async e =>
                    {
                        if (!e.Server.CurrentUser.ServerPermissions.ManageRoles)
                        {
                            await _client.ReplyError(e, "This command requires the bot have Manage Roles permission.");
                            return;
                        }
                        User user = e.Server.FindUsers(e.Args[0]).FirstOrDefault();
                        //var otherRoles = GetOtherRoles(e.User);
                        Role role = e.Server.Roles.Where(x => x.Name == "GroupMember").FirstOrDefault();
                        if (role != null)
                        {
                            await user.RemoveRoles(role);
                            await _client.Reply(e, $"Banishing player from group.");
                        }
                    });
            });
        }

        private IEnumerable<Role> GetOtherRoles(User user)
            => user.Roles.Where(x => !_rolesMap.ContainsKey(x.Name.ToLowerInvariant()));

        private async Task AddRole(CommandEventArgs e, User user, string roleName)
        {
            RoleDefinition groupRole;
            if (!_rolesMap.TryGetValue(roleName.ToLowerInvariant(), out groupRole))
            {
                await _client.ReplyError(e, "Unknown role");
                return;
            }
            if (!e.Server.CurrentUser.ServerPermissions.ManageRoles)
            {
                await _client.ReplyError(e, "This command requires the bot have Manage Roles permission.");
                return;
            }
            Role role = e.Server.Roles.Where(x => x.Name == groupRole.Name).FirstOrDefault();
            if (role == null)
            {
                role = await e.Server.CreateRole(groupRole.Name);
                await role.Edit(permissions: ServerPermissions.None, color: groupRole.Color);
            }
            var otherRoles = GetOtherRoles(user);
            await user.Edit(roles: otherRoles.Concat(new Role[] { role }));
            await _client.Reply(e, $"Adding {groupRole.Name} role to {user.Name}");
        }       
    }
}