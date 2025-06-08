using InventoryManagerFrontEnd.Models;

namespace InventoryManagerFrontEnd.Interfaces
{
    public interface ITokenService
    {
        public Task<TokenResponse?> GetToken();
        public Task SetToken(TokenResponse tokenResponse);
        public Task<bool> IsTokenValid();
        public Task OnInitialize();
        public Task LogOut();


    }
}
