using System;
namespace WorldRank.Console.Exceptions;


public class InsufficientFundsException : WalletException
{
	public InsufficientFundsException(string message): base (message)
	{

	}
}
