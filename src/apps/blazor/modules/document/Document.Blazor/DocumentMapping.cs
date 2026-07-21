using FSH.Starter.Blazor.Infrastructure.Api;
using Mapster;
using Nextended.Core.Extensions;
using static FSH.Starter.Blazor.Modules.Document.Blazor.Pages.Storage.Buckets;

namespace FSH.Starter.Blazor.Modules.Document.Blazor;

internal class DocumentMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<BucketResponse,BucketVM>()
        //.Map(dest => dest.Region.SystemName, src => src.Region); 
    }
}
