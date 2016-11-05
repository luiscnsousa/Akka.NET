namespace MovieStreaming.Actors
{
    using System;
    using System.Threading.Tasks;
    using Akka.Actor;
    using Messages;

    public class UserActor : ReceiveActor
    {
        private string _currentlyWatching;

        public UserActor()
        {
            Console.WriteLine("Creating a UserActor");

            //this.Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
            //this.Receive<StopMovieMessage>(message => HandleStopMovieMessage());

            ColorConsole.WriteLineCyan("Setting initial behaviour to stopped");
            this.Stopped();
        }

        private void Playing()
        {
            this.Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());
            this.Receive<PlayMovieMessage>(message => ColorConsole.WriteLineRed("Error: cannot start playing another movie before stopping existing one"));

            ColorConsole.WriteLineCyan("UserActor has now become Playing");
        }

        private void Stopped()
        {
            this.Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            this.Receive<StopMovieMessage>(message => ColorConsole.WriteLineRed("Error: cannot stop if nothing is playing"));

            ColorConsole.WriteLineCyan("UserActor has now become Stopped");
        }

        //private void HandlePlayMovieMessage(PlayMovieMessage message)
        //{
        //    if (string.IsNullOrEmpty(_currentlyWatching))
        //    {
        //        this.StartPlayingMovie(message.MovieTitle);
        //    }
        //    else
        //    {
        //        ColorConsole.WriteLineRed(
        //            "Error: cannot start playing another movie before stopping existing one");
        //    }
        //}

        private void StartPlayingMovie(string title)
        {
            _currentlyWatching = title;

            ColorConsole.WriteLineYellow(string.Format("User is currently watching '{0}'", _currentlyWatching));

            this.Become(this.Playing);
        }

        //private void HandleStopMovieMessage()
        //{
        //    if (string.IsNullOrEmpty(_currentlyWatching))
        //    {
        //        ColorConsole.WriteLineRed(
        //            "Error: cannot stop if nothing is playing");
        //    }
        //    else
        //    {
        //        this.StopPlayingCurrentMovie();
        //    }
        //}

        private void StopPlayingCurrentMovie()
        {
            ColorConsole.WriteLineYellow(string.Format("User has stopped watching '{0}'", _currentlyWatching));

            _currentlyWatching = null;
            
            this.Become(this.Stopped);
        }

        protected override void PreStart()
        {
            ColorConsole.WriteLineGreen("UserActor PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineGreen("UserActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineGreen(string.Concat("UserActor PreRestart because: ", reason));

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineGreen(string.Concat("UserActor PostRestart because: ", reason));

            base.PostRestart(reason);
        }
    }
}
