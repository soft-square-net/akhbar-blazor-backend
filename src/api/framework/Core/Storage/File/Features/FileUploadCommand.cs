using System.Net.Mime;
using MediatR;

namespace FSH.Framework.Core.Storage.File.Features;

public class FileUploadCommand : IRequest<FileUploadResponse>
{
    public string Name { get; set; } = default!;
    public string Bucket { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public string Extension { get; set; } = default!;
    public string Data { get; set; } = default!;
    public string Prefix { get; set; } = default!;
}

