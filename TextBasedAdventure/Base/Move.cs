using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Actors;

namespace TextBasedAdventure.Base
{
    public class Move
    {
        public readonly Action Action;
        public readonly Actor Actor;
        public readonly GameObject Target;

        public bool HasTarget
        {
            get
            {
                return Target != null;
            }
        }

        public Move(Action Action, Actor Actor, GameObject Target = null)
        {
            this.Action = Action;
            this.Actor = Actor;
            this.Target = Target;

            if (HasTarget && !Action.CanTarget(Target))
            {
                throw new InvalidOperationException();
            }
        }

        public void Run()
        {
            Action.Run(this);
        }
    }
}
