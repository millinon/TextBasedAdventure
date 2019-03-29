using System;
using System.Collections.Generic;
using System.Linq;

namespace TextBasedAdventure
{
    public abstract class Inventory : GameObject
    {
        public int Count
        {
            get
            {
                return objects.Count();
            }
        }

        public uint Capacity
        {
            get; private set;
        }

        private readonly List<GameObject> objects = new List<GameObject>();

        public IEnumerable<GameObject> Contents
        {
            get
            {
                if (CurrentState == State.DESTROYED)
                {
                    throw new ArgumentException($"{Name} is destroyed");
                }
                else return objects;
            }
        }

        public Inventory(Engine Engine, uint Capacity) : base(Engine)
        {
            this.Capacity = Capacity;
        }

        public void Add(GameObject Object)
        {
            if(CurrentState == State.DESTROYED)
            {
                throw new ArgumentException($"{Name} is destroyed");
            }
            else if(Object.Size + Count > Capacity)
            {
                throw new ArgumentException($"Not enough space to store {Object.Name} in {Name}");
            }

            objects.Add(Object);
        }

        public void Remove(GameObject Object)
        {
            if (CurrentState == State.DESTROYED)
            {
                throw new ArgumentException($"{Name} is destroyed");
            }
            else if (!objects.Contains(Object))
            {
                throw new ArgumentException($"{Name} does not contain {Object.Name}");
            }

            objects.Remove(Object);
        }

        public override void Destroy(bool Silent)
        {
            if(!Silent)
            {
                Engine.Logger.Log(this, $"{Name} and its contents was destroyed");
            }

            foreach (var obj in objects) {
                obj.Destroy(true);
            };
        }
    }
}
