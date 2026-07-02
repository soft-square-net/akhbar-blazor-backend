using System.Text.Json.Serialization;

namespace Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))] // System.Text.Json
public enum ResourceOwnerType
{
    Anonymous = 0,
    Group = 1,
    ManagedIdentity = 2,
    Role = 3,
    ServicePrincipal = 4,
    User = 5,
}
