using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Modules;

namespace RPGbot.Modules.RPG
{
    internal partial class RPGModule : IModule
    {
        private async Task LoadCharacter(CommandEventArgs e)
        {
            Character loadingCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
            if (loadingCharacter != null)
                await _client.Reply(e, $"У вас уже есть персонаж: {e.Args[0]}");
            else
            {
                //newCharacter = new Character(e.User, e.Args[0]);
                try
                {
                    loadingCharacter = UtilityClass.Deserialize<Character>(System.IO.Path.Combine(Environment.CurrentDirectory, "Data\\Characters\\" + e.User.Name + "\\" + e.Args[0] + ".json"));
                }
                catch (Exception ex)
                {
                    await _client.ReplyError(e, ex.Message);
                    return;
                }

                _allCharacters.Add(loadingCharacter);

                Role role = e.Server.Roles.Where(x => x.Name == "PartyMember").FirstOrDefault();
                if (e.User.HasRole(role))
                    _party.AddMember(loadingCharacter);
                await _client.Reply(e, $"Персонаж загружен.");

            }
        }

        private async Task CheckCharacter(CommandEventArgs e)
        {
            Character userCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
            if (userCharacter == null)
                await _client.Reply(e, $"У вас нет персонажа.");
            else
            {                
                await _client.Reply(e, $"Ваш персонаж: {userCharacter?.Name} {userCharacter?.RaceName}");
            }
        }

        private async Task ReadRawCharacter(CommandEventArgs e)
        {
            Character userCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
            if (userCharacter == null)
                await _client.Reply(e, $"У вас нет персонажа.");
            else
            {
                string res = Newtonsoft.Json.JsonConvert.SerializeObject(userCharacter, Newtonsoft.Json.Formatting.Indented);
                await _client.Reply(e, res.Substring(0, 1024));
                await _client.Reply(e, res.Substring(1024));
            }
        }

        private async Task UnloadCharacter(CommandEventArgs e)
        {
            Character removeCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
            if (removeCharacter == null)
                await _client.Reply(e, $"У вас нет персонажа.");
            else
            {
                _allCharacters.Remove(removeCharacter);
                _party.RemoveMember(removeCharacter);
                await _client.Reply(e, $"Персонаж выгружен.");
            }
        }

        private async Task CreateCharacter(CommandEventArgs e)
        {
            Character newCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
            if (newCharacter != null)
                await _client.Reply(e, $"У вас уже есть персонаж: {e.Args[0]}");
            else
            {
                //Race race = new Race();
                try
                {
                    var race = UtilityClass.Deserialize<Race>(System.IO.Path.Combine(Environment.CurrentDirectory, "Data\\Races\\" + e.Args[1] + ".json"));
                    newCharacter = new Character(e.User, e.Args[0], race);
                }
                catch (Exception ex)
                {
                    await _client.ReplyError(e, ex.Message);
                    return;
                }                
                
                _allCharacters.Add(newCharacter);

                Role role = e.Server.Roles.Where(x => x.Name == "PartyMember").FirstOrDefault();
                if (e.User.HasRole(role))
                    _party.AddMember(newCharacter);
                await _client.Reply(e, $"Персонаж создан.");
                try
                {
                    UtilityClass.Serialize<Character>(newCharacter, System.IO.Path.Combine(System.Environment.CurrentDirectory, "Data\\Characters\\" + e.User.Name + "\\" + newCharacter.Name + ".json"));
                }
                catch (Exception ex)
                {
                    await _client.ReplyError(e, ex.Message);
                }
            }
        }
    }
}
