namespace MovieStreaming.Actors
{
    using System;
    using System.Collections.Generic;
    using Akka.Actor;
    using Messages;

    public class UserCoordinatorActor : ReceiveActor
    {
        private readonly Dictionary<int, IActorRef> users;

        public UserCoordinatorActor()
        {
            this.users = new Dictionary<int, IActorRef>();

            this.Receive<PlayMovieMessage>(
                message =>
                {
                    CreateChildUserIfNotExists(message.UserId);

                    var childActorRef = this.users[message.UserId];

                    childActorRef.Tell(message);  
                });

            this.Receive<StopMovieMessage>(
                message =>
                {
                    CreateChildUserIfNotExists(message.UserId);

                    var childActorRef = this.users[message.UserId];

                    childActorRef.Tell(message);
                });
        }

        private void CreateChildUserIfNotExists(int userId)
        {
            if (!this.users.ContainsKey(userId))
            {
                var newChildActorRef =
                    Context.ActorOf(Props.Create(() => new UserActor(userId)), string.Concat("User", userId));

                this.users.Add(userId, newChildActorRef);

                ColorConsole.WriteLineCyan(string.Format("UserCoordinatorActor created new child UserActor for {0} (Total Users: {1})", userId, this.users.Count));
            }
        }
        
        #region Lifecycle hooks
        protected override void PreStart()
        {
            ColorConsole.WriteLineCyan("UserCoordinatorActor PreStart");
        }

        protected override void PostStop()
        {
            ColorConsole.WriteLineCyan("UserCoordinatorActor PostStop");
        }

        protected override void PreRestart(Exception reason, object message)
        {
            ColorConsole.WriteLineCyan(string.Concat("UserCoordinatorActor PreRestart because: ", reason));

            base.PreRestart(reason, message);
        }

        protected override void PostRestart(Exception reason)
        {
            ColorConsole.WriteLineCyan(string.Concat("UserCoordinatorActor PostRestart because: ", reason));

            base.PostRestart(reason);
        }
        #endregion
    }
}
