using TextBasedAdventure.Base.Actors;

namespace TextBasedAdventure.Base.Interfaces
{
    public interface ILockable
    {
       bool Unlock(Actor Actor, GameObject Key);
    }
}
