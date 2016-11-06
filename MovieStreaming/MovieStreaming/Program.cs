namespace MovieStreaming
{
    using System;
    using System.Threading.Tasks;
    using Akka.Actor;
    using Akka.Configuration;
    using Common;
    using Common.Actors;
    using Common.Messages;

    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            //MainAsync().Wait();
            
            HierarchyMainAsync().Wait();
        }

        static async Task HierarchyMainAsync()
        {
            ColorConsole.WriteLineGray("Creating MovieStreamingActorSystem");
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");

            ColorConsole.WriteLineGray("Creating actor supervisory hierarchy");
            MovieStreamingActorSystem.ActorOf(Props.Create<PlaybackActor>(), "Playback");


            do
            {
                await ShortPause();

                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
                ColorConsole.WriteLineGray("enter a command and hit enter");

                var command = Console.ReadLine();
                object message = null;

                if (command.StartsWith("play"))
                {
                    var commandParts = command.Split(',');
                    var userId = int.Parse(commandParts[1]);
                    var movieTitle = commandParts[2];

                    message = new PlayMovieMessage(movieTitle, userId);
                }

                if (command.StartsWith("stop"))
                {
                    var userId = int.Parse(command.Split(',')[1]);

                    message = new StopMovieMessage(userId);
                }

                if (message != null)
                {
                    MovieStreamingActorSystem.ActorSelection("/user/Playback/UserCoordinator").Tell(message);
                }

                if (command == "exit")
                {
                    await MovieStreamingActorSystem.Terminate();
                    await MovieStreamingActorSystem.WhenTerminated;
                    ColorConsole.WriteLineGray("Actor system shutdown");
                    Console.ReadKey();
                    Environment.Exit(1);

                }
            }
            while (true);
        }

        private static async Task ShortPause()
        {
            await Task.Delay(1000);
        }

        static async Task MainAsync()
        {
            // [INITIALIZING]
            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");
            Console.WriteLine("Actor system created");


            Props userActorProps = Props.Create<UserActor>();
            IActorRef userActorRef = MovieStreamingActorSystem.ActorOf(userActorProps, "UserActor");

            //Props playbackActorProps = Props.Create<PlaybackActor>();
            //IActorRef playbackActorRef = MovieStreamingActorSystem.ActorOf(playbackActorProps, "PlaybackActor");
            
            //------------------------------

            // [SENDING]
            Console.ReadKey();
            var codenanDestroyer = new PlayMovieMessage("Codenan the Destroyer", 42);
            Console.WriteLine(string.Format("Sending a PlayMovieMessage ({0})", codenanDestroyer.MovieTitle));
            userActorRef.Tell(codenanDestroyer);

            Console.ReadKey();
            var booleanLies = new PlayMovieMessage("Boolean Lies", 42);
            Console.WriteLine(string.Format("Sending another PlayMovieMessage ({0})", booleanLies.MovieTitle));
            userActorRef.Tell(booleanLies);

            Console.ReadKey();
            Console.WriteLine("Sending a StopMovieMessage");
            userActorRef.Tell(new StopMovieMessage());

            Console.ReadKey();
            Console.WriteLine("Sending another StopMovieMessage");
            userActorRef.Tell(new StopMovieMessage());


            //playbackActorRef.Tell(new PlayMovieMessage("Akka.NET: The Movie", 42));
            //playbackActorRef.Tell(new PlayMovieMessage("Partial Recall", 99));
            //playbackActorRef.Tell(new PlayMovieMessage("Boolean Lies", 77));
            //playbackActorRef.Tell(new PlayMovieMessage("Codenan the Destroyer", 1));
            //playbackActorRef.Tell(PoisonPill.Instance); // the actor will process the previous messages and then take this poison pill

            //------------------------------

            // [TERMINATING]
            //press any key to start shutdown of system
            Console.ReadKey();
            
            // terminating the actor system will terminate all actors
            await MovieStreamingActorSystem.Terminate(); //Shutdown() is obsolete
            await MovieStreamingActorSystem.WhenTerminated; //AwaitTermination() is obsolete
            Console.WriteLine("Actor system shutdown");

            //press any key to stop console application
            Console.ReadKey();
        }
    }
}
