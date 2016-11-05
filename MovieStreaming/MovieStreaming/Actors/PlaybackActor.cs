namespace MovieStreaming.Actors
{
    using System;
    using System.Threading.Tasks;
    using Akka.Actor;
    using Messages;

    public class PlaybackActor : ReceiveActor
    {
        public PlaybackActor()
        {
            Console.WriteLine("Creating a PlaybackActor");

            this.Receive<PlayMovieMessage>(
                message => HandlePlayMovieMessage(message), 
                message => message.UserId == 42);
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            Console.WriteLine("Received movie title " + message.MovieTitle);
            Console.WriteLine("Received user Id " + message.UserId);
        }

        //protected override void OnReceive(object message)
        //{
        //    if (message is PlayMovieMessage)
        //    {
        //        var m = message as PlayMovieMessage;
        //    }
        //    else
        //    {
        //        Unhandled(message);
        //    }
        //}
    }
}
