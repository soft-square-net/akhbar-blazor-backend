using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using FSH.Starter.WebApi.Document.Domain;

namespace FSH.Starter.WebApi.Document.Appication.Buckets.Specs;
public class GetBucketByNameSpec: SingleResultSpecification<Bucket>
{
    public GetBucketByNameSpec(string bucketName)
    {
        Query.Where(b => b.Name == bucketName);
    }
}
