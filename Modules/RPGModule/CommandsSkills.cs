using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Modules;

namespace RPGbot.Modules
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
                //targetCharacter.Status.Hitpoints = 
            }
        }

        private void ParseDamageStr(string damageString)
        {
            //var str = damageString.Split('d');
            //var diceCount = int.Parse(str[0]);
            //str = str[1].Split('+', '-');
            //var diceSides = int.Parse(str[0]);
            //var diceModifier = int.Parse
            //foreach (Match m in Regex.Matches(damageString, pattern))
            //    Console.WriteLine("{0}: {1}", ++ctr, m.Groups[1].Value);
            //var diceCount = int.Parse(new string(damageString.TakeWhile(s => s == 'd').ToArray()));
            //var 
        }
    }
}
