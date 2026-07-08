using System;

namespace WorldRank.Console.Exceptions;

public class NullPlayerException : PlayerException
{
    public NullPlayerException(string message) : base(message)
    {
    }

    public NullPlayerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

