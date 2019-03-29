using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextBasedAdventure.Base.Actors
{
    public abstract class NPC : Actor
    {
        public enum Hostility
        {
            HOSTILE, NEUTRAL, FRIENDLY,
        }

        public virtual Hostility Relationship(Actor Actor)
        {
            if (Faction == 0)
            {
                return Hostility.NEUTRAL;
            }
            else if (Faction == Actor.Faction)
            {
                return Hostility.FRIENDLY;
            }
            else
            {
                return Hostility.HOSTILE;
            }
        }

        public NPC(Engine Engine) : base(Engine)
        {

        }
    }
}
