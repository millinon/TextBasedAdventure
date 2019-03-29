using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Actors;
using TextBasedAdventure.Base.Areas;

namespace TextBasedAdventure.Base
{
    public abstract class Game
    {
        public readonly Engine Engine;

        public abstract IReadOnlyCollection<Area> Areas
        {
            get;
        }

        public abstract Area StartingArea
        {
            get;
        }

        public abstract int MaxPlayers
        {
            get;
        }

        private readonly HashSet<Player> players = new HashSet<Player>();

        public IEnumerable<Player> Players
        {
            get
            {
                return players;
            }
        }
    
        public void AddPlayer(Player Player)
        {
            if(Players.Count() > MaxPlayers)
            {
                throw new InvalidOperationException();
            }

            players.Add(Player);

            StartingArea.AddActor(Player);
        }

        public void RemovePlayer(Player Player)
        {
            if (players.Contains(Player))
            {
                players.Remove(Player);
            }
        }
        
        public virtual bool GameOver
        {
            get
            {
                if (Players.Where(p => p.WonGame).Any()) return true;
                else if (!Players.Where(p => ! p.LostGame).Any()) return true;

                return false;
            }
        }

        public Game(Engine Engine)
        {
            this.Engine = Engine;
        }

        public virtual void Start()
        {
            foreach(var area in Areas)
            {
                area.Spawn();
            }
        }

        public virtual void Step(bool RunIdleAreas)
        {
            foreach (var area in Areas)
            {
                if (RunIdleAreas || area.Actors.Where(a => a is Player).Any())
                {
                    area.Step();
                }
            }
        }
    }
}