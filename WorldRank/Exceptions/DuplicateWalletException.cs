using System;
namespace WorldRank.Console.Exceptions;


public class DuplicateWalletException : WalletException
{
    public DuplicateWalletException(string message) : base(message)
    {

    }
}
