using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Modules;

namespace RPGbot.Modules.RPG
{
    internal partial class RPGModule : IModule
    {
        private async Task UseSkillAttack(CommandEventArgs e, Skill skill)
        {
            Character userCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
            if (userCharacter == null)
                await _client.Reply(e, $"У вас нет персонажа.");
            else
            {
                Character targetCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
                if (userCharacter == null)
                    targetCharacter = userCharacter;
                string flavorText = "";
                if (skill.FlavorText.Count > 0)
                    flavorText = skill.FlavorText[(new Random()).Next(0, skill.FlavorText.Count)];                
                await _client.Reply(e, string.Format(flavorText, userCharacter.Name, targetCharacter.Name, e.Args?[1]));
                //int damage;
                //TODO: apply damage
                targetCharacter.Status.Hitpoints -= (new Dice(skill.BaseDamage).Roll());
            }
        }
    }
}
