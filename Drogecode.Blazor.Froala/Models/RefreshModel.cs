namespace Drogecode.Blazor.Froala.Models;
public class RefreshModel
{
    public event Action? RefreshRequested;
    public void CallRequestRefresh()
    {
        RefreshRequested?.Invoke();
    }
}
