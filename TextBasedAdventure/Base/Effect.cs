using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Actors;
using static TextBasedAdventure.Base.Actors.Actor;

namespace TextBasedAdventure.Base
{
    public class Effect
    {
        public virtual ReadOnlyAttributes Stats
        {
            get; protected set;
        }

        public virtual bool IsPermanent
        {
            get; protected set;
        } = false;

        public virtual uint DurationTicks
        {
            get; protected set;
        } = 1;

        public virtual bool DoesStack
        {
            get; protected set;
        } = false;

        public virtual bool ModifyMove(ref Move Move)
        {
            return false; // by default, don't interrupt the move
        }

        public void Apply(GameObject Source, Actor Target)
        {
            if (!DoesStack)
            {
                var aes = Target.Effects.Where(ae => ae.Effect == this);

                if (aes.Count() == 0)
                {
                    Target.Effects.Add(new ActiveEffect(this, Source, Target));
                }
                else if (aes.Count() == 1)
                {
                    aes.First().RemainingDuration = DurationTicks; // reset the timer for the active effect
                }
                else
                {
                    throw new InvalidOperationException("Multiple active non-stackable effects");
                }
            }
            else
            {
                Target.Effects.Add(new ActiveEffect(this, Source, Target));
            }
        }
    }

    public class ActiveEffect
    {
        public readonly Effect Effect;

        public uint RemainingDuration;

        public readonly GameObject Source;
        public readonly Actor Target;

        public ActiveEffect(Effect Effect, GameObject Source, Actor Target)
        {
            this.Effect = Effect;
            this.Source = Source;
            this.Target = Target;

            RemainingDuration = Effect.DurationTicks;
        }
    }
}
