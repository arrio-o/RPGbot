using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGbot.Modules.RPG
{
    public class Dice
    {
        public byte Count { get; set; } = 0;
        public byte Sides { get; set; } = 0;
        public int Modifier { get; set; } = 0;
        public long LastRoll { get; private set; } = 0;

        public Dice()
        {

        }

        public Dice(byte count, byte sides, int modifier)
        {
            Count = count;
            Sides = sides;
            Modifier = modifier;
        }

        public Dice(string diceString)
        {
            diceString = diceString.ToLowerInvariant();
            if (diceString.Contains('d'))
            {
                Modifier = (diceString.Contains('+') ? 1 : diceString.Contains('-') ? -1 : 0);
                var str = diceString.Split('d');
                Count = byte.Parse(str[0]);
                str = str[1].Split('+', '-');
                Sides = byte.Parse(str[0]);
                if (Modifier != 0)
                    Modifier *= (int.Parse(str[1]));
            }
            else
            {
                Modifier = int.Parse(diceString);
            }
        }

        public long Roll()
        {
            var Rand = System.Security.Cryptography.RandomNumberGenerator.Create();
            byte[] four_bytes = new byte[4];
            long result = 0;

            for (int i = 0; i < Count; ++i)
            {
                Rand.GetBytes(four_bytes);
                result += BitConverter.ToUInt32(four_bytes, 0) % (Sides+1);
                //result += (new Random()).Next(1, (int)Sides);
            }
            LastRoll = result + Modifier;
            return LastRoll;
        }

        //public static long GetCryptoRandom(byte maxValue)
        //{
        //    var Rand = System.Security.Cryptography.RandomNumberGenerator.Create();
        //    byte[] four_bytes = new byte[4];
        //    Rand.GetBytes(four_bytes);
        //    return BitConverter.ToUInt32(four_bytes, 0) % (maxValue+1);
        //}

        public override string ToString()
        {
            //return String.Format("{0}d{1}{2:+;-}{3}", Count, Sides, Modifier, Modifier);
            return String.Format($"{Count}d{Sides}{Modifier.ToString("+#;-#;+0")}");
        }
    }
}
