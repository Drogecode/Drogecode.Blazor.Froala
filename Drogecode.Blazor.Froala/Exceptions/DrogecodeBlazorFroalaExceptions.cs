using System.Runtime.Serialization;

namespace Drogecode.Blazor.Froala.Exceptions;

[Serializable]
public class DrogecodeBlazorFroalaExceptions : Exception, ISerializable
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