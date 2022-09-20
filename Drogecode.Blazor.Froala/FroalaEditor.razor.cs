﻿using Drogecode.Blazor.Froala.Helpers;
using Drogecode.Blazor.Froala.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Drogecode.Blazor.Froala;

public sealed partial class FroalaEditor : IAsyncDisposable
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    [Parameter, EditorRequired] public FroalaEditorConfig Config { get; set; } = default!;
    [Parameter, EditorRequired] public FroalaEditorDetail Detail { get; set; } = default!;
    [Parameter] public string FroalaApiKey { get; set; } = string.Empty;
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public EventCallback OnContentChanged { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public EventCallback OnBlur { get; set; }
    [Parameter] public EventCallback OnSaveBefore { get; set; }
    [Parameter] public EventCallback OnSaveAfter { get; set; }
    [Parameter] public EventCallback OnSaveError { get; set; }
    /// <summary>
    /// Enable when debugging FroalaEditor, should never be enabled in production
    /// </summary>
    [Parameter] public bool LogProgressToConsole { get; set; }
    private DotNetObjectReference<FroalaEditor>? _dotNetHelper;
    private CancellationTokenSource _cts = new();
    private ulong _forced = 0;
    private string _froalaId = string.Empty;
    private bool _dragging;
    private bool _dotNetHelperSet;

    protected override void OnInitialized()
    {
        NullCheck();
        _froalaId = string.IsNullOrEmpty(Detail.FroalaId) ? $"froala-editor-{RandomHelper.RandomName(5)}" : Detail.FroalaId;
        Detail.RefreshRequested += RefreshMe;
        Detail.StartDrag += StartDrag;
        Detail.StopDrag += StopDrag;
        Detail.Save += Save;
    }

    private void NullCheck()
    {
        if (Config == null)
            throw new NullReferenceException("FroalaEditorConfig is required!");
        if (Detail == null)
            throw new NullReferenceException("FroalaEditorDetail is required!");
        if (string.IsNullOrEmpty(FroalaApiKey))
            Console.WriteLine("FroalaApiKey is absent this will give an error in production!");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            LogToConsole($"Rendering: {_froalaId} {(Detail.HtmlContent.Length > 30 ? Detail.HtmlContent[..30] : string.Empty)}");
            _dotNetHelper = DotNetObjectReference.Create(this);
            await JsRuntime.InvokeVoidAsync("frSetDotNetHelper", _froalaId, _dotNetHelper);
            _dotNetHelperSet = true;
            if (Detail.InitializeFroalaOnFirstRender)
                await CreateEditor();
            else
                LogToConsole($"Not initializing: {_froalaId} {(Detail.HtmlContent.Length > 30 ? Detail.HtmlContent[..30] : string.Empty)}");
            Detail.Rendered = true;
        }
    }

    private async Task CreateEditor()
    {
        try
        {
            await JsRuntime.InvokeVoidAsync("frCreateEditor", _cts.Token, $"#{_froalaId}", _froalaId, Detail.Id, Config);
            Detail.Initialized = true;
            LogToConsole($"Editor {_froalaId} created");
        }
        catch (TaskCanceledException)
        {
        }
    }

    [JSInvokable]
    public async Task ContentChanged()
    {
        await OnContentChanged.InvokeAsync();
    }

    [JSInvokable]
    public async Task Click()
    {
        await OnClick.InvokeAsync();
    }

    [JSInvokable]
    public async Task Blur()
    {
        await OnBlur.InvokeAsync();
    }

    [JSInvokable]
    public async Task SaveBefore()
    {
        await OnSaveBefore.InvokeAsync();
    }

    [JSInvokable]
    public async Task SaveAfter()
    {
        await OnSaveAfter.InvokeAsync();
    }

    [JSInvokable]
    public async Task SaveError()
    {
        await OnSaveError.InvokeAsync();
    }

    private async void StartDrag()
    {
        if (_dragging) return;
        await DisposeFroalaJs();
        _forced++;
        _dragging = true;
        StateHasChanged();
    }

    private async void StopDrag()
    {
        if (!_dragging) return;
        StateHasChanged();
        await CreateEditor();
        _dragging = false;
    }

    private async void Save()
    {
        if (Detail.Initialized)
            await JsRuntime.InvokeVoidAsync("frSave", _froalaId);
    }

    private void RefreshMe()
    {
        _forced++;
        StateHasChanged();
    }

    private async Task DisposeFroalaJs()
    {
        if (Detail.Initialized)
        {
            await JsRuntime.InvokeVoidAsync("frDisposeEditor", _froalaId);
            Detail.Initialized = false;
            LogToConsole($"Editor {_froalaId} disposed");
        }

        Detail.InitializeFroalaOnFirstRender = true;
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
        Detail.Rendered = false;
        Detail.RefreshRequested -= RefreshMe;
        Detail.StartDrag -= StartDrag;
        Detail.StopDrag -= StopDrag;
        Detail.Save -= Save;
        _forced++;
    }
}