using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.List.v1;
public sealed record BucketResponse(Guid? Id, string Name, string? Description);

