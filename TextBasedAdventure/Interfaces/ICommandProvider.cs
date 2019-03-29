using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextBasedAdventure.Base.Actors;

namespace TextBasedAdventure.Interfaces
{
    public interface ICommandProvider
    {
        string GetCommand(Player Player);
    }
}
