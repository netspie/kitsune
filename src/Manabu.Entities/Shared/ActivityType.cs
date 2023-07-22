using Dunet;

namespace Manabu.Entities.Shared;

[Union]
public partial record ActivityType
{
    partial record Session;
    partial record Rest;
}