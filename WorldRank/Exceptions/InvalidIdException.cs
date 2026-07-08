using System;

namespace WorldRank.Console.Exceptions;

public class InvalidIdException : PlayerException
{
    public InvalidIdException(string message) : base(message)
    {
    }

    public InvalidIdException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

