namespace Drogecode.Blazor.Froala.Helpers;

public static class RandomHelper
{
    private static readonly Random _random = new Random();

    public static string RandomName(int characters)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, characters).Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}