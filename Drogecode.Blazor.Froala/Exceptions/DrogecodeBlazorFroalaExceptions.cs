namespace Drogecode.Blazor.Froala.Exceptions;
public class DrogecodeBlazorFroalaExceptions : Exception
{
    public DrogecodeBlazorFroalaExceptions()
    {
    }

    public DrogecodeBlazorFroalaExceptions(string message)
        : base(message)
    {
    }

    public DrogecodeBlazorFroalaExceptions(string message, Exception inner)
        : base(message, inner)
    {
    }
}