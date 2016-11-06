namespace MovieStreaming.Remote
{
    using System.Threading.Tasks;
    using Akka.Actor;
    using Common;

    class Program
    {
        private static ActorSystem MovieStreamingActorSystem;

        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            ColorConsole.WriteLineGray("Creating MovieStreamingActorSystem in remote process");

            MovieStreamingActorSystem = ActorSystem.Create("MovieStreamingActorSystem");

            await MovieStreamingActorSystem.WhenTerminated;
        }
    }
}
