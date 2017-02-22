using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontraktbaseret_udvikling___V2
{
    public class HasPlayedAgainstException : Exception
    {
        public HasPlayedAgainstException() { }
        public HasPlayedAgainstException(string message) : base(message) { }
    }

    public class PlayerNotFoundException : Exception
    {
        public PlayerNotFoundException() { }
        public PlayerNotFoundException(string message) : base(message) { }
    }

    public class InvalidPlayerAmountException : Exception
    {
        public InvalidPlayerAmountException() { }
        public InvalidPlayerAmountException(string message) : base(message) { }
    }

    public class PlayerListFullException : Exception
    {
        public PlayerListFullException() { }
        public PlayerListFullException(string message) : base(message) { }
    }

    public class PlayerListNotFullException : Exception
    {
        public PlayerListNotFullException() { }
        public PlayerListNotFullException(string message) : base(message) { }
    }

    public class GameAlreadyStartedException : Exception
    {
        public GameAlreadyStartedException() { }
        public GameAlreadyStartedException(string message) : base(message) { }
    }

    public class GameNotStartedException : Exception
    {
        public GameNotStartedException() { }
        public GameNotStartedException(string message) : base(message) { }
    }

    public class NoPlayersLeftInQueueException : Exception
    {
        public NoPlayersLeftInQueueException() { }
        public NoPlayersLeftInQueueException(string message) : base(message) { }
    }

    public class PlayersLeftInQueueException : Exception
    {
        public PlayersLeftInQueueException() { }
        public PlayersLeftInQueueException(string message) : base(message) { }
    }

    public class PlayerAlreadyPickedException : Exception
    {
        public PlayerAlreadyPickedException() { }
        public PlayerAlreadyPickedException(string message) : base(message) { }
    }

    public class InvalidPlayerPickException : Exception
    {
        public InvalidPlayerPickException() { }
        public InvalidPlayerPickException(string message) : base(message) { }
    }
}
