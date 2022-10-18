namespace WalletService.WebApi.Domain.Exceptions;

public class LimitExceededException : Exception
{
    internal protected LimitExceededException(string message = "The limit already was exceeded!")
        :base(message){}
}