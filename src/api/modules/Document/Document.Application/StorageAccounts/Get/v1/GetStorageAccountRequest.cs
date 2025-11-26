using MediatR;

namespace FSH.Starter.WebApi.Document.Application.StorageAccounts.Get.v1;
public class GetStorageAccountRequest : IRequest<StorageAccountResponse>
{
    public Guid Id { get; set; }
    public GetStorageAccountRequest(Guid id) => Id = id;
}
