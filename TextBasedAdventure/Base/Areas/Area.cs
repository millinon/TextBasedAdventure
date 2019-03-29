using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Actors;

namespace TextBasedAdventure.Base.Areas
{
    public abstract class Area
    {
        public readonly Engine Engine;

        public abstract string Intro
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public class PlacedObject
        {
            public readonly Area Area;
            public readonly GameObject Object;
            public GameObject Container;
            public bool Moved = false;
            public bool Taken = false;
            public virtual bool Static
            {
                get; protected set;
            } = false;

            public PlacedObject(Area Area, GameObject Object)
            {
                this.Area = Area;
                this.Object = Object;
            }
        }

        protected readonly List<PlacedObject> placed_objects = new List<PlacedObject>();
        public IEnumerable<GameObject> PlacedObjects
        {
            get
            {
                return placed_objects.Select(po => po.Object);
            }
        }
        
        public void DropObject(GameObject Object)
        {
            placed_objects.Add(new PlacedObject(this, Object));
        }

        public bool RemoveObject(GameObject Object)
        {
            if(!PlacedObjects.Contains(Object))
            {
                return false;
            }

            placed_objects.RemoveAll(po => po.Object == Object);

            return true;
        }

        protected readonly HashSet<Actor> localActors = new HashSet<Actor>();

        public void AddActor(Actor Actor)
        {
            localActors.Add(Actor);

            Actor.CurrentArea = this;
        }
        
        public void RemoveActor(Actor Actor)
        {
            if (localActors.Contains(Actor))
            {
                localActors.Remove(Actor);
            }
        }
        

        public virtual IEnumerable<Actor> Actors
        {
            get
            {
                return localActors;
            }
        }

        public Area(Engine Engine)
        {
            this.Engine = Engine;
        }
        
        public virtual void Spawn()
        {
            foreach(var obj in placed_objects)
            {
                obj.Object.Spawn(this);
            }

            foreach(var actor in Actors)
            {
                actor.Spawn(this);
            }
        }

        public IEnumerable<GameObject> FindObject(string Name)
        {
            var trimmed_name = Name.ToLower().Trim();

            var actors = Actors.Where(actor => actor.Name.ToLower().Contains(trimmed_name));

            if (actors.Any())
            {
                return actors;
            }

            return placed_objects.Where(obj => obj.Object.Name.ToLower().Contains(trimmed_name)).Select(placed_object => placed_object.Object);
        }

        public virtual void Step()
        {
            foreach(var actor in Actors)
            {
                var move = actor.Choose();

                if (move != null)
                {
                    actor.Do(move);
                }
            }
        }
    }
}