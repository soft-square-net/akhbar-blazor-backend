using FSH.Starter.WebApi.Document.Application.Buckets.CreateFile.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.DownloadFile.v1;
using FSH.Starter.WebApi.Document.Application.Buckets.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Buckets.GetBucketFile.v1;
public sealed record GetBucketFileRequest( Guid BucketId, Guid FolderId, Guid FileId  ) : IRequest<GetBucketFileResponse> ;

