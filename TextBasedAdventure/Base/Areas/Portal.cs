using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Actors;

namespace TextBasedAdventure.Base.Areas
{
    public abstract class Portal : GameObject
    {
        public readonly Area DestinationArea;

        public Portal(Engine Engine, Area DestinationArea) : base(Engine)
        {
            this.DestinationArea = DestinationArea;
        }

        public virtual bool CanGo(Actor Actor)
        {
            return true;
        }
    }
}
