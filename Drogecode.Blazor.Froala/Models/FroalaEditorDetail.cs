namespace Drogecode.Blazor.Froala.Models;

public sealed class FroalaEditorDetail
{
    // Drogecode.Blazor.Froala actions
    public event Action? RefreshRequested;
    public event Action? DeInitialize;
    public event Action? Initialize;
    public event Action? Save;
    public event Action? RefreshParent;

    // Froala actions
    public event Action? OnContentChanged;
    public event Action? OnClick;
    public event Action? OnBlur;
    public event Action? OnSaveBefore;
    public event Action? OnSaveAfter;
    public event Action? OnSaveError;

    // Drogecode.Blazor.Froala actions
    public void CallRequestRefresh()
    {
        RefreshRequested?.Invoke();
    }
    public void CallDeInitialize()
    {
        DeInitialize?.Invoke();
    }
    public void CallInitialize()
    {
        Initialize?.Invoke();
    }
    public void CallSave()
    {
        Save?.Invoke();
    }
    public void CallRefreshParent()
    {
        RefreshParent?.Invoke();
    }

    // Froala actions
    public void CallOnContentChanged()
    {
        OnContentChanged?.Invoke();
    }
    public void CallOnClick()
    {
        OnClick?.Invoke();
    }
    public void CallOnBlur()
    {
        OnBlur?.Invoke();
    }
    public void CallOnSaveBefore()
    {
        OnSaveBefore?.Invoke();
    }
    public void CallOnSaveAfter()
    {
        OnSaveAfter?.Invoke();
    }
    public void CallOnSaveError()
    {
        OnSaveError?.Invoke();
    }

    public Guid Id { get; set; }
    /// <summary>
    /// If null a random id will be generated.
    /// </summary>
    public string? FroalaId { get; set; }
    public string HtmlContent { get; set; } = string.Empty;
    public bool InitializeFroalaOnFirstRender { get; set; } = true;
    public bool IsInitialized { get; internal set; }
    public bool IsRendered { get; internal set; }
    public bool IsDeleted { get; internal set; }
    public bool IsDisposed { get; internal set; }
}