using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Actors;

namespace TextBasedAdventure.Base
{
    public abstract class Action
    {
        public readonly string Name;
        public virtual IEnumerable<string> Aliases
        {
            get { return new List<string>(); }
        }

        public virtual Attributes RequiredSkills
        {
            get
            {
                return new Attributes();
            }
        }

        public Action(string Name)
        {
            this.Name = Name;
        }

        public abstract bool CanTarget(GameObject Object);

        public abstract void Run(Move Move);

        public virtual bool Check()
        {
            return true;
        }

        public abstract bool TryParse(IEnumerable<string> Arguments, Actor Actor, out Move Move);
    }

    
}
