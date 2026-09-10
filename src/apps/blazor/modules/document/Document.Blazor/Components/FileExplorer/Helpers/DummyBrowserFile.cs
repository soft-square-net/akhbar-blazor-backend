
using Microsoft.AspNetCore.Components.Forms;

namespace FSH.Starter.Blazor.Modules.Document.Blazor.Components.FileExplorer.Helpers;

public class DummyBrowserFile : IBrowserFile
{
    public string Name { get; }
    public DateTimeOffset LastModified { get; }
    public long Size { get; }
    public string ContentType { get; }
    public byte[] Content { get; }

    public DummyBrowserFile(string name, DateTimeOffset lastModified, long size, string contentType, byte[] content)
    {
        Name = name;
        LastModified = lastModified;
        Size = size;
        ContentType = contentType;
        Content = content;
    }

    public Stream OpenReadStream(long maxAllowedSize = 512000, CancellationToken cancellationToken = default) => new MemoryStream(Content);

    //private async Task<string> ConvertImageToBase64(string filePath)
    //{
    //    try
    //    {
    //        // Remove leading wwwroot/ if present, as static files are served from root
    //        var relativePath = filePath.StartsWith("wwwroot/") ? filePath.Substring(7) : filePath;
    //        var response = await Http.GetAsync(relativePath);
    //        if (!response.IsSuccessStatusCode)
    //            return string.Empty;
    //        var imageBytes = await response.Content.ReadAsByteArrayAsync();
    //        // Try to get content type from response, fallback to jpeg
    //        var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/jpeg";
    //        return $"data:{contentType};base64,{Convert.ToBase64String(imageBytes)}";
    //    }
    //    catch
    //    {
    //        return string.Empty;
    //    }
    //}
}
