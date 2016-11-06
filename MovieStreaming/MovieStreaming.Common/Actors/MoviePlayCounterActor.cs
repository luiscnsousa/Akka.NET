namespace MovieStreaming.Common.Actors
{
    using System;
    using System.Collections.Generic;
    using Akka.Actor;
    using Exceptions;
    using Messages;

    public class MoviePlayCounterActor : ReceiveActor
    {
        private readonly Dictionary<string, int> moviePlayCounts;

        public MoviePlayCounterActor()
        {
            this.moviePlayCounts = new Dictionary<string, int>();

            this.Receive<IncrementPlayCountMessage>(message => HandleIncrementMessage(message));
        }

        private void HandleIncrementMessage(IncrementPlayCountMessage message)
        {
            var key = message.MovieTitle;
            if (this.moviePlayCounts.ContainsKey(key))
            {
                this.moviePlayCounts[key]++;
            }
            else
            {
                this.moviePlayCounts.Add(key, 1);
            }

            // SIMULATED BUGS
            if (moviePlayCounts[message.MovieTitle] > 3)
            {
                throw new SimulatedCorruptStateException(); // strategy is to RESTART this child
            }

            if (message.MovieTitle == "Partial Recoil")
            {
                throw new SimulatedTerribleMovieException(); // strategy is to RESUME this child, although...
                // it will resume only on the next message, which means that the following Console.Write will not be executed
                // but its counter will remain the same because this actor is not restarted
            }

            ColorConsole.WriteLineMagenta(
                string.Format("MoviePlayCounterActor '{0}' has been watched {1} times", key, moviePlayCounts[key]));
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineMagenta("MoviePlayCounterActor PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineMagenta("MoviePlayCounterActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineMagenta(string.Concat("MoviePlayCounterActor PreRestart because: ", reason));

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineMagenta(string.Concat("MoviePlayCounterActor PostRestart because: ", reason));

            base.PostRestart(reason);
        }
        #endregion
    }
}