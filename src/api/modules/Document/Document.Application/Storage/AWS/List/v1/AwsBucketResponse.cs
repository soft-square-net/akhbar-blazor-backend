using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.WebApi.Document.Application.Storage.AWS.List.v1;
public sealed record AwsBucketResponse(Guid? Id, string Name, string? Description);

