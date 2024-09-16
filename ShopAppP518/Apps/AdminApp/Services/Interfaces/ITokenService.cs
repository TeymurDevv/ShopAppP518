using ShopAppP518.Entities;

namespace ShopAppP518.Apps.AdminApp.Services.Interfaces
{
    public interface ITokenService
    {
        string GetToken(string SecretKey, string Audience, string Issuer, AppUser existUser, IList<string> roles);
    }
}
