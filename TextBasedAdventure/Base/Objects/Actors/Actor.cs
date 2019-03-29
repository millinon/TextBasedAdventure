using System;
using System.Collections.Generic;
using System.Linq;
using TextBasedAdventure.Base.Areas;

namespace TextBasedAdventure.Base.Actors
{
    public abstract class Actor : GameObject
    {
        public int Faction
        {
            get;
            protected set;
        } = 0;


        public Attributes Stats;
        public abstract Attributes BaseStats
        {
            get;
        }


        private readonly Dictionary<string, Action> AvailableActions;
        private readonly Dictionary<string, string> ActionAliases;


        public IReadOnlyDictionary<string, Action> Actions
        {
            get
            {
                return AvailableActions;
            }
        }
       
        protected void RegisterAction(Action Action)
        {
            var lowername = Action.Name.ToLower();
            
            if (AvailableActions.ContainsKey(Action.Name.ToLower()))
            {
                throw new InvalidOperationException();
            }
            
            foreach(var alias in Action.Aliases.Select(a => a.ToLower()))
            {
                if (ActionAliases.ContainsKey(alias))
                {
                    throw new InvalidOperationException();
                }
            }
            
            AvailableActions[lowername] = Action;

            foreach(var alias in Action.Aliases.Select(a => a.ToLower()))
            {
                ActionAliases[alias] = Action.Name;
            }
        }

        public Action FindAction(string ActionName)
        {
            var actionname = ActionName.ToLower();

            if (AvailableActions.ContainsKey(actionname))
            {
                return AvailableActions[actionname];
            }
            else if (ActionAliases.ContainsKey(actionname))
            {
                return AvailableActions[ActionAliases[actionname]];
            }

            return null;
        }

        public readonly List<ActiveEffect> Effects = new List<ActiveEffect>();

        public ReadOnlyAttributes EffectiveAttributes
        {
            get
            {
                var stats = Stats.Clone() as Attributes;

                foreach (var effect in Effects)
                {
                    foreach (var key in effect.Effect.Stats.Skills)
                    {
                        stats[key] += effect.Effect.Stats[key];
                    }
                }

                return stats.AsReadOnly();
            }
        }

        public Actor(Engine Engine) : base(Engine)
        {
            AvailableActions = new Dictionary<string, Action>();
            ActionAliases = new Dictionary<string, string>();
        }

        public override void Spawn(Area Area)
        {
            base.Spawn(Area);

            Stats = BaseStats.Clone() as Attributes;
        }

        public override void Destroy(bool Silent)
        {
            if(!Silent)
            {
                Engine.Logger.Log(this, $"{Name} was killed.");
            }
        }

        public abstract Move Choose();

        public void Do(Move Move)
        {
            bool interrupted = false;

            foreach (var ae in Effects)
            {
                if (ae.Effect.ModifyMove(ref Move))
                {
                    interrupted = true;

                    break;
                }
            }

            if(!interrupted)
            {
                Move.Run();
            }
        }

        public virtual bool Go(Portal Portal)
        {
            if(!Portal.CanGo(this))
            {
                return false;
            } else
            {
                MoveTo(Portal.DestinationArea);

                if(this is Player p)
                {
                    Engine.Logger.Log(this, p, Portal.DestinationArea.Intro, TextBasedAdventure.Interfaces.EventType.INFO);
                }

                return true;
            }
        }

        public abstract void Listen(GameObject Sender, string Message);

        protected virtual void Say(string Message)
        {
            foreach(var recipient in CurrentArea.Actors)
            {
                recipient.Listen(this, Message);
            }
        }
    }
}
