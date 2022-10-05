using Drogecode.Blazor.Froala.Models;

namespace Drogecode.Blazor.FroalaDemo.Pages;
public sealed partial class Index : IDisposable
{
    private readonly FroalaEditorConfig _config = new();
    private readonly FroalaEditorDetail _detail = new();

    protected override void OnInitialized()
    {
        _config.Key = ""; //Set your api key. When empty it shows an error in production.
        _detail.RefreshParent += RefreshMe; //To know if _detail.IsInitialized is updated.
        _detail.OnClick += OnClick;
        _detail.OnBlur += OnBlur;
        _detail.HtmlContent = @"<H1>Demo</H1>
This will show in the editor when you load the component!";
    }

    private void OnClick()
    {
        Console.WriteLine($"Onclick called for {_detail?.FroalaId}");
    }

    private void OnBlur()
    {
        Console.WriteLine($"OnBlur called for {_detail?.FroalaId}");
    }

    private void Delete()
    {
        _detail.IsDeleted = true;
        _detail.CallDeInitialize();
        RefreshMe();
    }

    private void RefreshMe()
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        _detail.RefreshParent -= RefreshMe;
        _detail.OnClick -= OnClick;
        _detail.OnBlur -= OnBlur;
    }
}
