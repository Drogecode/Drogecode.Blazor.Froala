using Drogecode.Blazor.Froala.Exceptions;
using Drogecode.Blazor.Froala.Helpers;
using Drogecode.Blazor.Froala.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Drogecode.Blazor.Froala;

public sealed partial class FroalaEditor : IAsyncDisposable
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    [Parameter, EditorRequired] public FroalaEditorConfig Config { get; set; } = default!;
    [Parameter, EditorRequired] public FroalaEditorDetail Detail { get; set; } = default!;
    [Parameter] public string Id { get; set; } = string.Empty;
    [Parameter] public string Class { get; set; } = string.Empty;

    /// <summary>
    /// Enable when debugging FroalaEditor, should never be enabled in production
    /// </summary>
    [Parameter]
    public bool LogProgressToConsole { get; set; }

    private DotNetObjectReference<FroalaEditor>? _dotNetHelper;
    private CancellationTokenSource _cts = new();
    private ulong _forced = 0;
    private string _froalaId = string.Empty;
    private bool _dragging;
    private bool _dotNetHelperSet;
    private bool _isSaving;

    protected override void OnInitialized()
    {
        NullCheck();
        _froalaId = string.IsNullOrEmpty(Detail.FroalaId) ? $"froala-editor-{RandomHelper.RandomName(5)}" : Detail.FroalaId;
        Detail.IsDisposed = false;
        Detail.FroalaId = _froalaId;
        Detail.RefreshRequested += RefreshMe;
        Detail.DeInitialize += DeInitialize;
        Detail.Initialize += Initialize;
        Detail.Save += Save;
    }

    private void NullCheck()
    {
        if (Config == null)
            throw new DrogecodeBlazorFroalaException("FroalaEditorConfig is required!");
        if (Detail == null)
            throw new DrogecodeBlazorFroalaException("FroalaEditorDetail is required!");
        if (string.IsNullOrEmpty(Config?.Key))
            Console.WriteLine("FroalaApiKey in Config.Key is absent this will give an error in production!");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            LogToConsole($"Rendering: {_froalaId} {(Detail.HtmlContent.Length > 30 ? Detail.HtmlContent[..30] : Detail.HtmlContent)}");
            _dotNetHelper = DotNetObjectReference.Create(this);
            await JsRuntime.InvokeVoidAsync("frSetDotNetHelper", _froalaId, _dotNetHelper);
            _dotNetHelperSet = true;
            if (Detail.InitializeFroalaOnFirstRender && !Detail.IsDeleted)
                await CreateEditor();
            else
                LogToConsole($"Not initializing: {_froalaId} {(Detail.HtmlContent.Length > 30 ? Detail.HtmlContent[..30] : Detail.HtmlContent)}");
            Detail.IsRendered = true;
        }
    }

    private async Task CreateEditor()
    {
        try
        {
            if (!Detail.IsDeleted)
            {
                await JsRuntime.InvokeVoidAsync("frCreateEditor", _cts.Token, $"#{_froalaId}", _froalaId, Detail.Id, Config);
                Detail.IsInitialized = true;
                Detail.CallRefreshParent();
                LogToConsole($"Editor {_froalaId} (re)created");
            }
            else
                LogToConsole($"Editor {_froalaId} marked as deleted and not (re)created");
        }
        catch (TaskCanceledException)
        {
            LogToConsole($"CreateEditor {_froalaId} canceled");
        }
    }

    [JSInvokable]
    public void ContentChanged()
    {
        Detail.CallOnContentChanged();
    }

    [JSInvokable]
    public void Click()
    {
        Detail.CallOnClick();
    }

    [JSInvokable]
    public void Blur()
    {
        Detail.CallOnBlur();
    }

    [JSInvokable]
    public void SaveBefore()
    {
        _isSaving = true;
        Detail.CallOnSaveBefore();
    }

    [JSInvokable]
    public void SaveAfter()
    {
        _isSaving = false;
        Detail.CallOnSaveAfter();
    }

    [JSInvokable]
    public void SaveError()
    {
        _isSaving = false;
        Detail.CallOnSaveError();
    }

    private async void DeInitialize()
    {
        await DisposeFroalaJs();
        _forced++;
        _dragging = true;
        StateHasChanged();
    }

    private async void Initialize()
    {
        await WaitForSaveDone();
        StateHasChanged();
        await CreateEditor();
        _dragging = false;
    }

    private async void Save()
    {
        if (!Detail.IsInitialized) return;
        _isSaving = true;
        await JsRuntime.InvokeVoidAsync("frSave", _froalaId);
    }

    private async void RefreshMe()
    {
        if (Detail.IsDeleted) return;
        await WaitForSaveDone();
        LogToConsole($"Waiting {_froalaId} done");
        _forced++;
        StateHasChanged();
    }

    private async Task DisposeFroalaJs()
    {
        if (Detail.IsInitialized)
        {
            await WaitForSaveDone();
            await JsRuntime.InvokeVoidAsync("frDisposeEditor", _froalaId);
            Detail.IsInitialized = false;
            Detail.CallRefreshParent();
            LogToConsole($"Editor {_froalaId} disposed");
        }

        Detail.InitializeFroalaOnFirstRender = true;
    }

    private async Task WaitForSaveDone()
    {
        var i = 0;
        while (_isSaving)
        {
            i++;
            await Task.Delay(10);
            if (i <= 100) continue;
            LogToConsole($"Still saving {_froalaId} after waiting 1 second, letting progress continue");
            break;
        }
    }

    private void LogToConsole(string message)
    {
        if (LogProgressToConsole)
            Console.WriteLine(message);
    }

    public async ValueTask DisposeAsync()
    {
        LogToConsole($"Dispose {_froalaId}");
        _cts.Cancel();
        _dotNetHelper?.Dispose();
        await DisposeFroalaJs();
        if (_dotNetHelperSet)
            await JsRuntime.InvokeVoidAsync("frDisposeDotNetHelper", _froalaId);
        _dotNetHelperSet = false;
        Detail.IsDisposed = true;
        Detail.IsRendered = false;
        Detail.RefreshRequested -= RefreshMe;
        Detail.DeInitialize -= DeInitialize;
        Detail.Initialize -= Initialize;
        Detail.Save -= Save;
        _forced++;
    }
}