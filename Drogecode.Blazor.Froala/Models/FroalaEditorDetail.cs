namespace Drogecode.Blazor.Froala.Models;

public sealed class FroalaEditorDetail
{
    public event Action? RefreshRequested;
    public event Action? StartDrag;
    public event Action? StopDrag;
    public event Action? Save;
    public void CallRequestRefresh()
    {
        RefreshRequested?.Invoke();
    }
    public void CallStartDrag()
    {
        StartDrag?.Invoke();
    }
    public void CallStopDrag()
    {
        StopDrag?.Invoke();
    }
    public void CallSave()
    {
        Save?.Invoke();
    }

    public Guid Id { get; set; }
    /// <summary>
    /// If null a random id will be generated.
    /// </summary>
    public string? FroalaId { get; set; }
    public string HtmlContent { get; set; } = string.Empty;
    public string StylingId { get; set; } = string.Empty;
    public bool InitializeFroalaOnFirstRender { get; set; } = true;
    public bool IsInitialized { get; internal set; }
    public bool IsRendered { get; internal set; }
    public bool IsDeleted { get; internal set; }
    public bool IsDisposed { get; internal set; }
}