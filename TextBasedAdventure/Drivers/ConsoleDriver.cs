using System;
using System.Collections.Generic;
using TextBasedAdventure.Base.Actors;
using TextBasedAdventure.Interfaces;

namespace TextBasedAdventure.Drivers
{
    public class ConsoleDriver : ICommandProvider, IEventLogger
    {
        private static readonly Dictionary<EventType, ConsoleColor> ColorMapping = new Dictionary<EventType, ConsoleColor>()
        {
            [EventType.BAD] = ConsoleColor.Red,
            [EventType.GOOD] = ConsoleColor.Green,
            [EventType.DEBUG] = ConsoleColor.Yellow
        };

        public bool DebugEnabled;
        public string CommandPrompt;
        public ConsoleColor PromptColor;

        public ConsoleDriver(string CommandPrompt = "> ", ConsoleColor PromptColor = ConsoleColor.Cyan, bool DebugEnabled = false)
        {
            this.CommandPrompt = CommandPrompt;
            this.PromptColor = PromptColor;
            this.DebugEnabled = DebugEnabled;
        }

        public string GetCommand(Player Player)
        {
            Console.ForegroundColor = PromptColor;
            Console.Write(CommandPrompt);
            Console.ResetColor();

            return Console.ReadLine();
        }

        public void Log(object Source, string Message, EventType Type = EventType.INFO)
        {
            if (ColorMapping.ContainsKey(Type))
            {
                Console.ForegroundColor = ColorMapping[Type];
            }
            else
            {
                Console.ResetColor();
            }

            Console.WriteLine(Message);

            Console.ResetColor();
        }

        public void Log(object Source, Player Player, string Message, EventType Type = EventType.INFO)
        {
            Log(Source, Message, Type);
        }
    }
}
