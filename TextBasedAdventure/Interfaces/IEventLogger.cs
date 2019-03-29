using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Actors;

namespace TextBasedAdventure.Interfaces
{
    public enum EventType
    {
        INFO, BAD, GOOD, DEBUG
    };

    public interface IEventLogger
    {
        void Log(object Source, Player Player, string Message, EventType Type = EventType.INFO);
        void Log(object Source, string Message, EventType Type = EventType.INFO);
    }
}
