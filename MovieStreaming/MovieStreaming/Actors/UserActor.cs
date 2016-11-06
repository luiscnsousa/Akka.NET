namespace MovieStreaming.Actors
{
    using System;
    using Akka.Actor;
    using Messages;

    public class UserActor : ReceiveActor
    {
        private string currentlyWatching;
        private int userId;

        //public UserActor()
        //{
        //    Console.WriteLine("Creating a UserActor");

        //    //this.Receive<PlayMovieMessage>(message => HandlePlayMovieMessage(message));
        //    //this.Receive<StopMovieMessage>(message => HandleStopMovieMessage());

        //    ColorConsole.WriteLineCyan("Setting initial behaviour to stopped");
        //    this.Stopped();
        //}

        public UserActor(int userId)
        {
            this.userId = userId;
            this.Stopped();
        }

        private void Playing()
        {
            ColorConsole.WriteLineYellow(string.Format("UserActor {0} has now become Playing", this.userId));

            this.Receive<StopMovieMessage>(message => StopPlayingCurrentMovie());
            this.Receive<PlayMovieMessage>(
                message => ColorConsole.WriteLineRed(
                    string.Format("UserActor {0} Error: cannot start playing another movie before stopping existing one", this.userId)));
        }

        private void Stopped()
        {
            ColorConsole.WriteLineYellow(string.Format("UserActor {0} has now become Stopped", this.userId));

            this.Receive<PlayMovieMessage>(message => StartPlayingMovie(message.MovieTitle));
            this.Receive<StopMovieMessage>(
                message => ColorConsole.WriteLineRed(
                    string.Format("UserActor {0} Error: cannot stop if nothing is playing", this.userId)));
        }

        //private void HandlePlayMovieMessage(PlayMovieMessage message)
        //{
        //    if (string.IsNullOrEmpty(currentlyWatching))
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
            this.currentlyWatching = title;

            ColorConsole.WriteLineYellow(string.Format("UserActor {0} is currently watching '{1}'", this.userId, this.currentlyWatching));

            this.Become(this.Playing);

            Context.ActorSelection("/user/Playback/PlaybackStatistics/MoviePlayCounter")
                .Tell(new IncrementPlayCountMessage(title));
        }

        //private void HandleStopMovieMessage()
        //{
        //    if (string.IsNullOrEmpty(currentlyWatching))
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
            ColorConsole.WriteLineYellow(string.Format("UserActor {0} has stopped watching '{1}'", this.userId, this.currentlyWatching));

            this.currentlyWatching = null;
            
            this.Become(this.Stopped);
        }

        protected override void PreStart()
        {
            ColorConsole.WriteLineYellow(string.Format("UserActor {0} PreStart", this.userId));
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineYellow(string.Format("UserActor {0} PostStop", this.userId));
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineYellow(string.Format("UserActor {0} PreRestart because: {1}", this.userId, reason));

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineYellow(string.Format("UserActor {0} PostRestart because: {1}", this.userId, reason));

            base.PostRestart(reason);
        }
    }
}
