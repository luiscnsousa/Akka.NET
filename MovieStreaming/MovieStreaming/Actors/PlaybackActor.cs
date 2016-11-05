namespace MovieStreaming.Actors
{
    using System;
    using System.Threading.Tasks;
    using Akka.Actor;
    using Messages;

    public class PlaybackActor 
        //: UntypedActor
        : ReceiveActor

    {
        public PlaybackActor()
        {
            Console.WriteLine("Creating a PlaybackActor");
            
            // with this second parameter, the actor will only handle a message with the specified UserId   <--
            //this.Receive<PlayMovieMessage>(
            //    message => HandlePlayMovieMessage(message), 
            //    message => message.UserId == 42);

            this.Receive<PlayMovieMessage>(
                message => HandlePlayMovieMessage(message));
        }

        private void HandlePlayMovieMessage(PlayMovieMessage message)
        {
            ColorConsole.WriteLineYellow(
                string.Format("PlayMovieMessage '{0}' for user {1}", 
                message.MovieTitle, 
                message.UserId));
        }

        // it will Only be possible to override OnReceived method if PlaybackActor inherits from UntypedActor   <--
        //protected override void OnReceive(object message)
        //{
        //    if (message is PlayMovieMessage)
        //    {
        //        var m = message as PlayMovieMessage;
        //        Console.WriteLine("Received movie title " + m.MovieTitle);
        //        Console.WriteLine("Received user Id " + m.UserId);
        //    }
        //    else
        //    {
        //        Unhandled(message);
        //    }
        //}

        protected override void PreStart()
        {
            ColorConsole.WriteLineGreen("PlaybackActor PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineGreen("PlaybackActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineGreen(string.Concat("PlaybackActor PreRestart because: ", reason));

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineGreen(string.Concat("PlaybackActor PostRestart because: ", reason));

            base.PostRestart(reason);
        }
    }
}
