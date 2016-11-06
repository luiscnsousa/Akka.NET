namespace MovieStreaming.Actors
{
    using System;
    using Akka.Actor;
    using Exceptions;

    public class PlaybackStatisticsActor : ReceiveActor
    {
        public PlaybackStatisticsActor()
        {
            Context.ActorOf(Props.Create<MoviePlayCounterActor>(), "MoviePlayCounter");
        }

        // if a strategy if not defined, the default restart strategy will be used
        // which means that if the child MoviePlayCounterActor throws an exception, it will be restarted and reset its counter
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                exception =>
                {
                    if (exception is SimulatedCorruptStateException)
                    {
                        return Directive.Restart;
                    }

                    if (exception is SimulatedTerribleMovieException)
                    {
                        return Directive.Resume;
                    }

                    return Directive.Restart;
                });
        }

        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineWhite("PlaybackStatisticsActor PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineWhite("PlaybackStatisticsActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineWhite(string.Concat("PlaybackStatisticsActor PreRestart because: ", reason));

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineWhite(string.Concat("PlaybackStatisticsActor PostRestart because: ", reason));

            base.PostRestart(reason);
        }
        #endregion
    }
}
