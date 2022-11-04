namespace Drogecode.Blazor.Froala.Models;

public sealed class FroalaEditorConfig
{
    public bool ToolbarInline { get; set; }
    public bool CharCounterCount { get; set; }
    public string Key { get; set; } = string.Empty;
    public string SaveUrl { get; set; } = string.Empty;
    public string SaveMethod { get; set; } = "POST";
    public string SaveParam { get; set; } = "body";
    public int SaveInterval { get; set; } = 10000;
    public Dictionary<string, string> TableStyles { get; set; } = new() {{"fr-highlighted", "Highlighted"}, {"fr-thick", "Thick"}};
}
