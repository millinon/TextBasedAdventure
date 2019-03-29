using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Areas;

namespace TextBasedAdventure.Base.Actors
{
    public abstract class Player : Actor
    {
        public abstract bool WonGame
        {
            get;
        }

        public abstract bool LostGame
        {
            get;
        }

        public Player(Engine Engine) : base(Engine)
        {

        }

        public override Move Choose()
        {
            return Engine.GetCommand(this);
        }

        public override void Listen(GameObject Sender, string Message)
        {
            Engine.Logger.Log(this, this, $"{Sender.Name} says \"{Message}\"");
        }

        public override void Spawn(Area Area)
        {
            base.Spawn(Area);

            Engine.Logger.Log(this, this, Area.Intro);
        }

    }
}
