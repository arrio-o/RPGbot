using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGbot.Modules//.RPGModule
{
    public sealed class Party
    {
        // ----- Реализация синглтона
        private static readonly Lazy<Party> lazy = new Lazy<Party>(() => new Party());

        public static Party Instance { get { return lazy.Value; } }        

        private Party()
        {

        }
        // =====/реализация синглтона.

        public string PartyName { get; private set; } = "";
        public List<Character> Members { get; private set; } = new List<Character>();

        public class Position
        {
            public int x { get; set; }
            public int y { get; set; }
        };
        public Position currentPosition { get; private set; } = new Position();

        public class Map
        {
            public bool IsVisible { get; set; }
            //TODO: terrain types
            public int[][] map { get; set; }
        };
        public Map map { get; private set; } = new Map();

        public void AddMember(Character chara)
        {
            Members.Add(chara);
        }
        public void RemoveMember(Character chara)
        {
            Members.Remove(chara);
        }
        public void MoveNorth()
        {
            //TODO: check for terrain type
            currentPosition.y -= 1;
        }
        public void MoveSouth()
        {
            //TODO: check for terrain type
            currentPosition.y += 1;
        }
        public void MoveWest()
        {
            //TODO: check for terrain type
            currentPosition.x -= 1;
        }
        public void MoveEast()
        {
            //TODO: check for terrain type
            currentPosition.x += 1;
        }
    }
}
