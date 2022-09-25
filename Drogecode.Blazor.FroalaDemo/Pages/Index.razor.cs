using Drogecode.Blazor.Froala.Models;

namespace Drogecode.Blazor.FroalaDemo.Pages;
public sealed partial class Index
{
    private readonly FroalaEditorConfig _config = new();
    private readonly FroalaEditorDetail _detail = new();

    protected override void OnInitialized()
    {
        _config.Key = ""; //Set your api key. When empty it shows an error in production.
        _detail.HtmlContent = @"<H1>Demo</H1>
This will show in the editor when you load the component!";
    }

    public void OnClick()
    {
        Console.WriteLine($"Onclick called for {_detail?.FroalaId}");
    }

    public void OnBlur()
    {
        Console.WriteLine($"OnBlur called for {_detail?.FroalaId}");
    }
}
