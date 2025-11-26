
using System.ComponentModel;
using MediatR;
using Shared.Enums;

namespace FSH.Starter.WebApi.Document.Appication.StorageAccounts.Create.v1;
public sealed record CreateStorageAccountCommand(
    StorageProvider StorageProvider,
    [property: DefaultValue("Sample Bucket Name")] string AccountName,
    string AccessKey,
    string SecretKey,
    [property: DefaultValue("Descriptive Description")] string Description
) : IRequest<CreateStorageAccountResponse>;
