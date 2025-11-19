using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSH.Framework.Core.Paging;
using FSH.Starter.WebApi.Document.Application.Documents.Get.v1;
using MediatR;

namespace FSH.Starter.WebApi.Document.Application.Storage.AWS.List.v1;
public class ListAwsBucketCommand : PaginationFilter, IRequest<PagedList<AwsBucketResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
