using System;

namespace WorldRank.Console.Exceptions;

public class InvalidPlayerScoreException : PlayerException
{
    public InvalidPlayerScoreException(string message) : base(message)
    {
    }

    public InvalidPlayerScoreException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

