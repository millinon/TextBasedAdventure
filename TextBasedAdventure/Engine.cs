using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base;
using TextBasedAdventure.Base.Actors;
using TextBasedAdventure.Base.Areas;
using TextBasedAdventure.Interfaces;

namespace TextBasedAdventure
{
    public class Engine
    {
        private readonly Random Random;

        public int D2
        {
            get
            {
                return D(2);
            }
        }

        public int D4
        {
            get
            {
                return D(4);
            }
        }

        public int D6
        {
            get
            {
                return D(6);
            }
        }

        public int D10
        {
            get
            {
                return D(10);
            }
        }

        public int D20
        {
            get
            {
                return D(20);
            }
        }

        public int D100
        {
            get
            {
                return D(100);
            }
        }

        public int D(int Sides, int Modifier = 0)
        {
            return (Random.Next(Sides) + 1) + Modifier;
        }

        public readonly IEventLogger Logger;
        public readonly ICommandProvider CommandProvider;
        
        public Engine(IEventLogger Logger, ICommandProvider CommandProvider)
        {
            Random = new Random();

            this.Logger = Logger;
            this.CommandProvider = CommandProvider;
        }

        private Game Game;

        public bool GameLoaded
        {
            get
            {
                return Game != null;
            }
        }

        public void Load(Game Game)
        {
            this.Game = Game;
        }

        public void Step(bool RunIdleAreas)
        {
            if(!GameLoaded)
            {
                throw new InvalidOperationException();
            }

            Game.Step(RunIdleAreas);
        }

        public Move GetCommand(Player Player)
        {
            var input = CommandProvider.GetCommand(Player);

            var toks = input.Split(null).Select(s => s.Trim()).Where(s => s.Length > 0);

            if (!toks.Any()) return null;

            var verb = toks.First();
            
            var action = Player.FindAction(verb);

            if (action == null) return null;

            if(! action.TryParse(toks.Skip(1), Player, out Move choice))
            {
                return null;
            } else
            {
                return choice;
            }
        }
    }
}
