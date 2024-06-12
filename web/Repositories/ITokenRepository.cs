using Microsoft.AspNetCore.Identity;

namespace web.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
