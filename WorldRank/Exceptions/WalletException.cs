using System;
namespace WorldRank.Console.Exceptions;

public class WalletException : Exception
{
	public WalletException()
	{
	}
	public WalletException(string message):base(message) { 
	
	}

	public WalletException(string message, Exception InnerException):base(message, InnerException)
	{
		
	}
}
