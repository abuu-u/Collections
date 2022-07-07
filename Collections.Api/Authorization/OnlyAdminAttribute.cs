namespace Collections.Api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class OnlyAdminAttribute : Attribute
{
}
