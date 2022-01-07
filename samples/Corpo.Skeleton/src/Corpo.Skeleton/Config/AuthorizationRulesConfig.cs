using Miru.Security;

namespace Corpo.Skeleton.Config;

public class AuthorizationRulesConfig : IAuthorizationRules
{
    private readonly Current _current;
        
    public AuthorizationRulesConfig(Current current) => _current = current;

    public AuthorizationResult Evaluate<TRequest>(TRequest request, FeatureInfo feature)
    {
        // if (_current.IsAuthenticated == false)
        // {
        //     return AuthorizationResult.Fail("Authentication is required");
        // }

        return AuthorizationResult.Succeed();
    }
}