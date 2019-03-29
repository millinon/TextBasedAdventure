using System;
using System.Collections.Generic;
using TextBasedAdventure.Base;
using TextBasedAdventure.Base.Actors;
using TextBasedAdventure.Base.Areas;

namespace TextBasedAdventure
{
    public abstract class GameObject
    {
        public readonly string ID;
        public readonly Engine Engine;

        public abstract string Name
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        private int _hp;
        public int HP
        {
            get
            {
                return _hp;
            }
            set
            {
                if(value > MaxHP)
                {
                    throw new ArgumentException($"{value} > MaxHP ({MaxHP})");
                }

                _hp = value;

                if (CurrentState == State.DESTROYED)
                {
                    Destroy(false);
                }
            }
        }

        public int MaxHP
        {
            get;
            protected set;
        }

        public abstract uint Size
        {
            get;
        }

        public class DamageEventArgs
        {
            public GameObject Source;
            public GameObject Target;
            public int Points;
            public bool IsDestroyed;
        }

        public event EventHandler<DamageEventArgs> OnDamage;

        public virtual void Damage(GameObject DamageSource, int Points)
        {
            HP -= Points;

            OnDamage?.Invoke(this, new DamageEventArgs
            {
                Source = DamageSource,
                Target = this,
                Points = Points,
                IsDestroyed = CurrentState == State.DESTROYED,
            });
        }
        
        public virtual void Destroy(bool Silent)
        {

        }

        public Area CurrentArea
        {
            get; set;
        }

        public class AreaChangedEventArgs
        {
            public GameObject Object;
            public Area LastArea;
            public Area NewArea;
        }

        public event EventHandler<AreaChangedEventArgs> OnAreaChanged;

        public void MoveTo(Area NewArea)
        {
            var last = CurrentArea;

            CurrentArea = NewArea;

            OnAreaChanged?.Invoke(this, new AreaChangedEventArgs
            {
                Object = this,
                LastArea = last,
                NewArea = NewArea,
            });
        }

        public virtual void Spawn(Area Area)
        {
            HP = MaxHP;
            this.CurrentArea = Area;
        }

        public GameObject(Engine Engine)
        {
            ID = Guid.NewGuid().ToString();

            this.Engine = Engine;
        }

        public enum State
        {
            NORMAL, DAMAGED, DESTROYED,
        }

        public State CurrentState
        {
            get
            {
                if (HP == MaxHP) return State.NORMAL;
                else if (HP > 0) return State.DAMAGED;
                else return State.DESTROYED;
            }
        }
    }
}
