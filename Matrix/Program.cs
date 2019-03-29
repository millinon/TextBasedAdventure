using System.Collections.Generic;
using System.Linq;
using TextBasedAdventure;
using TextBasedAdventure.Base;
using TextBasedAdventure.Base.Actors;
using TextBasedAdventure.Base.Areas;
using TextBasedAdventure.Base.Interfaces;
using TextBasedAdventure.Drivers;

namespace Matrix
{
    class Program
    {
        public class MatrixGame : Game
        {
            public class Say : Action
            {
                public Say() : base("Say")
                {

                }

                public override bool CanTarget(GameObject Object)
                {
                    return true;
                }

                public override void Run(TextBasedAdventure.Base.Move Move)
                {
                    var SayMove = Move as Move;

                    if (SayMove.Target != null)
                    {
                        (SayMove.Target as Actor).Listen(SayMove.Actor, SayMove.Message);
                    }
                    else
                    {
                        foreach (var actor in Move.Actor.CurrentArea.Actors.Where(a => a != Move.Actor))
                        {
                            actor.Listen(SayMove.Actor, SayMove.Message);
                        }
                    }
                }

                public override bool TryParse(IEnumerable<string> Arguments, Actor Actor, out TextBasedAdventure.Base.Move Move)
                {
                    Move = null;

                    if (!Arguments.Any())
                    {
                        return false;
                    }

                    if (Arguments.Where(arg => arg.ToLower() == "to").Any())
                    {
                        var target_tokens = Arguments.SkipWhile(arg => arg.ToLower() != "to").Skip(1);

                        if (!target_tokens.Any()) return false;

                        var targets = Actor.CurrentArea.FindObject(string.Join(" ", target_tokens));

                        if (!targets.Any()) return false;

                        var message = Arguments.Take(Arguments.Count() - (target_tokens.Count() + 1));

                        if (!message.Any()) return false;

                        Move = new Move(this, Actor, string.Join(" ", message), targets.First());

                        return true;
                    }
                    else
                    {
                        Move = new Move(this, Actor, string.Join(" ", Arguments), null);

                        return true;
                    }
                }

                public class Move : TextBasedAdventure.Base.Move
                {
                    public readonly string Message;

                    public Move(Action Action, Actor Actor, string Message, GameObject Target = null) : base(Action, Actor, Target)
                    {
                        this.Message = Message;
                    }
                }
            }

            public class Eat : Action
            {
                public Eat() : base("Eat")
                {

                }

                public override bool CanTarget(GameObject Object)
                {
                    return Object is IEdible;
                }

                public override void Run(Move Move)
                {
                    var EatMove = Move as Move;

                    (EatMove.Target as IEdible).Eat(Move.Actor);
                }

                public override bool TryParse(IEnumerable<string> Arguments, Actor Actor, out Move Move)
                {
                    Move = null;

                    if (!Arguments.Any()) return false;

                    var target_objs = Actor.CurrentArea.FindObject(string.Join(" ", Arguments));

                    if (!target_objs.Any()) return false;

                    var target = target_objs.First();

                    if (!(target is IEdible)) return false;

                    Move = new Move(this, Actor, target);

                    return true;
                }
            }

            public class Look : Action
            {
                public Look() : base("Look")
                {

                }

                public override bool CanTarget(GameObject Object)
                {
                    return true;
                }

                public override void Run(Move Move)
                {
                    if (Move.Actor is Neo p)
                    {
                        if (Move.Target != null)
                        {
                            if (Move.Target is SpookyMirror m && p.took_red_pill)
                            {
                                p.Engine.Logger.Log(this, p, $"You look at the mirror. It looks different - like a moving fluid.");
                            }
                            else
                            {
                                p.Engine.Logger.Log(this, p, Move.Target.Description, TextBasedAdventure.Interfaces.EventType.INFO);
                            }
                        }
                        else
                        {
                            p.Engine.Logger.Log(this, p, p.CurrentArea.Description, TextBasedAdventure.Interfaces.EventType.INFO);
                        }
                    }
                }

                public override bool TryParse(IEnumerable<string> Arguments, Actor Actor, out Move Move)
                {
                    Move = null;

                    if (!Arguments.Any())
                    {
                        Move = new Move(this, Actor, null);

                        return true;
                    }
                    else
                    {
                        var target_objs = Actor.CurrentArea.FindObject(string.Join(" ", Arguments));

                        if (!target_objs.Any()) return false;

                        Move = new Move(this, Actor, target_objs.First());

                        return true;
                    }
                }
            }

            public class Touch : Action
            {
                public Touch() : base("Touch")
                {

                }

                public override bool CanTarget(GameObject Object)
                {
                    return true;
                }

                public override void Run(Move Move)
                {
                    if (Move.Actor is Neo n)
                    {
                        if (Move.Target is Actor a)
                        {
                            n.Engine.Logger.Log(this, n, $"{Move.Target.Name} looks at you funny");
                        }
                        else
                        {
                            if (Move.Target is SpookyMirror m && n.took_red_pill)
                            {
                                n.Engine.Logger.Log(this, n, $"You touch the mirror. It sticks to you, and begins to crawl along your arm. It advances along your skin, covering you completely.");

                                n.Go(m);
                            }
                            else
                            {
                                n.Engine.Logger.Log(this, n, $"You touch {Move.Target.Name}. Nothing happens.");
                            }
                        }
                    }
                }

                public override bool TryParse(IEnumerable<string> Arguments, Actor Actor, out Move Move)
                {
                    Move = null;

                    if (!Arguments.Any()) return false;

                    var target_objs = Actor.CurrentArea.FindObject(string.Join(" ", Arguments));

                    if (!target_objs.Any()) return false;

                    Move = new Move(this, Actor, target_objs.First());

                    return true;
                }
            }

            public class Morpheus : NPC
            {
                private readonly Say say_action;

                public Morpheus(Engine Engine) : base(Engine)
                {
                    say_action = new Say();

                    RegisterAction(say_action);
                }

                public override Attributes BaseStats => new Attributes();

                public override string Name => "Morpheus";

                public override string Description => "A striking resemblence of Laurence Fishburne. He is holding two pills; one red and one blue.";

                public override uint Size => 25;

                private bool gave_pill_speech = false;

                private readonly List<string> quotes = new List<string>()
                {
                    "The Matrix is everywhere. It is all around us. Even now, in this very room. You can see it when you look out your window or when you turn on your television. You can feel it when you go to work... when you go to church... when you pay your taxes. It is the world that has been pulled over your eyes to blind you from the truth.",
                    "What is real? How do you define 'real'? If you're talking about what you can feel, what you can smell, what you can taste and see, then 'real' is simply electrical signals interpreted by your brain.",
                    "There's a difference between knowing the path and walking the path.",
                    "The body cannot live without the mind.",
                    "You think that's air you're breathing?",
                };

                public override Move Choose()
                {
                    string quote;

                    if (!gave_pill_speech)
                    {
                        quote = "This is your last chance. After this, there is no turning back. You take the blue pill—the story ends, you wake up in your bed and believe whatever you want to believe. You take the red pill—you stay in Wonderland, and I show you how deep the rabbit hole goes. Remember: all I'm offering is the truth. Nothing more.";

                        gave_pill_speech = true;
                    }
                    else
                    {
                        quote = quotes[Engine.D(quotes.Count()) - 1];
                    }

                    return new Say.Move(say_action, this, quote, CurrentArea.Actors.Where(actor => actor.Name == "Neo").First());
                }

                public override void Listen(GameObject Sender, string Message)
                {

                }
            }

            public class Neo : Player
            {
                private readonly Say say_action;
                private readonly Look look_action;
                private readonly Eat eat_action;
                private readonly Touch touch_action;

                public override string Name => "Neo";

                public override string Description => "A striking resemblance to Keanu Reeves";

                public override uint Size => 20;

                public bool took_red_pill
                {
                    get; set;
                } = false;

                public bool took_blue_pill
                {
                    get; set;
                } = false;

                public override bool LostGame => took_blue_pill;

                public override bool WonGame => CurrentArea == Game.realworld;

                public override Attributes BaseStats => new Attributes();

                private readonly MatrixGame Game;

                public Neo(MatrixGame Game) : base(Game.Engine)
                {
                    this.Game = Game;

                    look_action = new Look();
                    say_action = new Say();
                    eat_action = new Eat();
                    touch_action = new Touch();

                    RegisterAction(look_action);
                    RegisterAction(say_action);
                    RegisterAction(eat_action);
                    RegisterAction(touch_action);
                }
            }

            public class SpookyMirror : Portal
            {
                public SpookyMirror(Engine Engine, Area Destination) : base(Engine, Destination)
                {

                }

                public override string Name => "Mirror";

                public override string Description => "An antique mirror.";

                public override uint Size => 20;

                public override bool CanGo(Actor Actor)
                {
                    if (!(Actor is Neo n))
                    {
                        return false;
                    }
                    else
                    {
                        return n.took_red_pill;
                    }
                }
            }

            public class PillRoom : Area
            {
                public PillRoom(MatrixGame Game) : base(Game.Engine)
                {
                    AddActor(new Morpheus(Engine));

                    DropObject(new Pill(false, Game.Engine));
                    DropObject(new Pill(true, Engine));

                    DropObject(new SpookyMirror(Engine, Game.realworld));
                }

                public override string Intro => "You are sitting in a large room. Seated opposite of you is Morpheus. He holds two pills - one red and one blue. There is a mirror next to you.";

                public override string Description => "It's just a room.";
            }

            public class Pill : GameObject, IEdible
            {
                public readonly bool IsRed;

                public Pill(bool IsRed, Engine Engine) : base(Engine)
                {
                    this.IsRed = IsRed;
                }

                public override string Name => $"{(IsRed ? "Red" : "Blue")} Pill";

                public override string Description => $"A large {(IsRed ? "Red" : "Blue")} pill.";

                public override uint Size => 2;

                public void Eat(Actor Actor)
                {
                    if (Actor is Neo n)
                    {
                        n.Engine.Logger.Log(this, n, "You eat the pill.", TextBasedAdventure.Interfaces.EventType.INFO);

                        if (IsRed)
                        {
                            n.took_red_pill = true;
                        }
                        else
                        {
                            n.took_blue_pill = true;
                        }
                    }
                }
            }

            public class RealWorld : Area
            {
                public RealWorld(Engine Engine) : base(Engine)
                {

                }

                public override string Intro => "You are now in the real world.";

                public override string Description => "The real world.";
            }

            private readonly PillRoom pillroom;
            private readonly RealWorld realworld;

            public MatrixGame(Engine Engine) : base(Engine)
            {
                realworld = new RealWorld(Engine);
                pillroom = new PillRoom(this);
            }

            public override IReadOnlyCollection<Area> Areas => new List<Area> {
                pillroom, realworld
                };

            public override Area StartingArea => pillroom;

            public override int MaxPlayers => 1;
        }

        static void Main(string[] args)
        {
            var driver = new ConsoleDriver();
            var engine = new Engine(driver, driver);
            var game = new MatrixGame(engine);

            engine.Load(game);

            var player = new MatrixGame.Neo(game);

            game.AddPlayer(player);

            game.Start();

            while (!game.GameOver)
            {
                engine.Step(false);
            }

            if (player.WonGame)
            {
                System.Console.WriteLine("You win!");
            }
            else
            {
                System.Console.WriteLine("You lose.");
            }

            System.Console.WriteLine("Press Enter to continue...");
            System.Console.ReadLine();
        }
    }
}
