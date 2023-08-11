namespace LoginMicroservice.Infra.Exceptions;

public class LoginContextException : Exception
{
    public LoginContextException(string message) : base(message)
    {
        
    }
}
