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
        private void UseSkillPreciseAttack(CommandEventArgs e, Character attackerCharacter, Skill skill, out string report)
        {
            report = "";

            Character targetCharacter = _allCharacters.Where(x => x.Name == e.Args?[0]).FirstOrDefault();
            if (attackerCharacter == null)
                targetCharacter = attackerCharacter;
            string flavorText = "";
            if (skill.FlavorText.Count > 0)
                flavorText = skill.FlavorText[(new Random()).Next(0, skill.FlavorText.Count)];

            var targetBodyPartType = BodyPartTemplate.GetSlotType(e.Args?[1]);
            bool isTargeted = targetBodyPartType != BodyPartTemplate.SlotTypes.undefined;
            var targetBodyPart = targetCharacter.ChooseTargetBodyPart(targetBodyPartType);

            report = string.Format(flavorText, attackerCharacter.Name, targetCharacter.Name);
            if (isTargeted)
                report += $" Цель: {(targetBodyPart.Description?.Count()>1? targetBodyPart.Description: targetBodyPart.Name)}";
            //await _client.Reply(e, report);
            report += Environment.NewLine;

            if (!targetCharacter.Dodge(attackerCharacter, skill, !string.IsNullOrWhiteSpace(e.Args?[1]), ref report))
                targetCharacter.ApplyDamage(attackerCharacter, skill, targetBodyPart, ref report);
            //await _client.Reply(e, report);            
        }
        //private void UseSkillAoE(CommandEventArgs e, Character attackerCharacter, Skill skill, out string report)
        //{
        //    report = "";

        //    Character targetCharacter = _allCharacters.Where(x => x.OwnerId == e.User.Id).FirstOrDefault();
        //    if (attackerCharacter == null)
        //        targetCharacter = attackerCharacter;
        //    string flavorText = "";
        //    if (skill.FlavorText.Count > 0)
        //        flavorText = skill.FlavorText[(new Random()).Next(0, skill.FlavorText.Count)];

        //    report = string.Format(flavorText, attackerCharacter.Name, targetCharacter.Name);
        //    //await _client.Reply(e, report);
        //    report += Environment.NewLine;
        //    report = "";

        //    if (!targetCharacter.Dodge(attackerCharacter, skill, !string.IsNullOrWhiteSpace(e.Args?[1]), ref report))
        //        targetCharacter.ApplyDamage(attackerCharacter, skill, targetBodyPart, ref report);
        //    //await _client.Reply(e, report);
        //}
    }
}
