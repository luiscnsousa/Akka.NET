namespace MovieStreaming.Common.Messages
{
    using System;

    public class StopMovieMessage
    {
        public int UserId { get; private set; }

        public StopMovieMessage(int userId)
        {
            this.UserId = userId;
        }

        public StopMovieMessage()
        {
        }
    }
}
