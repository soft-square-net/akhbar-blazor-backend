using System;

namespace FSH.Framework.Core.Storage.Dtos;
public class S3ObjectDto
{
    public string? Name { get; set; }
    public string? PresignedUrl { get; set; }
}
