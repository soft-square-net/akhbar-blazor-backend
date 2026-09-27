using FSH.Starter.WebApi.Document.Domain;
using Mapster.Models;

namespace FSH.Starter.WebApi.Document.Application.Buckets.GetFolder.v1;

public sealed record GetBucketFolderResponse(Guid? Id,
     Guid BucketId, string Name, IReadOnlyCollection<Folder> Folders, IReadOnlyCollection<Domain.File> Files, int Size, bool IsRoot, DateTimeOffset Created, DateTimeOffset LastModefied, bool AllowRead = true, bool AllowWrite = true, bool AllowDelete = false );

