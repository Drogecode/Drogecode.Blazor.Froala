namespace Drogecode.Blazor.Froala.Models;

public sealed class FroalaEditorDetail : RefreshModel
{
    public event Action? StartDrag;
    public event Action? StopDrag;
    public event Action? Save;
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
    public bool Initialized { get; set; }
    public bool Rendered { get; set; }
    public bool IsDeleted { get; set; }
}