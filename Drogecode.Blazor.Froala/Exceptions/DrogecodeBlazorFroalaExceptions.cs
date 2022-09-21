using System.Runtime.Serialization;

namespace Drogecode.Blazor.Froala.Exceptions;

[Serializable]
public class DrogecodeBlazorFroalaException : Exception
{
    public DrogecodeBlazorFroalaException()
    {
    }

    public DrogecodeBlazorFroalaException(string message)
        : base(message)
    {
    }

    public DrogecodeBlazorFroalaException(string message, Exception inner)
        : base(message, inner)
    {
    }
}