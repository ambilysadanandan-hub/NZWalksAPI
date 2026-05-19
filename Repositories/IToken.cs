using Microsoft.AspNetCore.Identity;

namespace NZWalksAPI.Repositories
{
    public interface IToken
    {
        string CreateJWTToken(IdentityUser identityUser, List<string>roles  );
    }
}
