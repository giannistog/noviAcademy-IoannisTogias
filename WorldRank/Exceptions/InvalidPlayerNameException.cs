using System;

namespace WorldRank.Console.Exceptions;

public class InvalidPlayerNameException : PlayerException
{
    public InvalidPlayerNameException(string message) : base(message)
    {
    }

    public InvalidPlayerNameException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

