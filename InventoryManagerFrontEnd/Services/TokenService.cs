using InventoryManagerFrontEnd.Interfaces;
using InventoryManagerFrontEnd.Models;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Net.Http.Json;

namespace InventoryManagerFrontEnd.Services
{
    public class TokenService : ITokenService
    {
        private TokenResponse? Token {  get; set; }

        private HttpClient _httpClient;
        private IConfiguration _configuration;
        private string? _apiUri;
        private IJSRuntime _js;
        public TokenService(HttpClient httpClient, IConfiguration configuration, IJSRuntime js)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _js = js;
            _apiUri = _configuration["ApiBaseUrl"];
        }
        public async Task OnInitialize()
        {
            string? accessToken = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
            string? refreshToken = await _js.InvokeAsync<string?>("localStorage.getItem", "refreshToken");
            string? expiresAtString = await _js.InvokeAsync<string?>("localStorage.getItem", "tokenExpiration");

            if (string.IsNullOrWhiteSpace(expiresAtString)
            || !DateTime.TryParse(expiresAtString, null, System.Globalization.DateTimeStyles.RoundtripKind, out var expiresAt))
            {
                Token = null;
                return;
            }

            Token = new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = expiresAt
            };
        }



        public async Task<TokenResponse?> GetToken()
        {
            if (Token == null)
            {
                return null;
            }

            // If the current token is expired, try to refresh
            if (Token.ExpiresAt < DateTime.UtcNow)
            {
                var didRefresh = await RefreshToken();
                if (!didRefresh)
                {
                    await RemoveToken();
                    return null;
                }
            }

            return Token;
        }

        public async Task SetToken(TokenResponse tokenResponse)
        {
            await _js.InvokeVoidAsync("localStorage.setItem", "authToken", tokenResponse.AccessToken);
            await _js.InvokeVoidAsync("localStorage.setItem", "refreshToken", tokenResponse.RefreshToken);
            await _js.InvokeVoidAsync("localStorage.setItem", "tokenExpiration", tokenResponse.ExpiresAt);

            Token = tokenResponse;
        }

        public async Task RemoveToken()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "refreshToken");
            await _js.InvokeVoidAsync("localStorage.removeItem", "tokenExpiration");
        }

        private async Task<bool> RefreshToken()
        {
            if(string.IsNullOrEmpty(_apiUri))
            {
                throw new Exception("API Uri is not included in appsettings");
            }

            if(Token == null)
            {
                return false;
            }

            var response = await _httpClient.PostAsJsonAsync($"{_apiUri}/Auth/refresh", Token);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var newToken = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (newToken == null)
            {
                return false;
            }

            await SetToken(newToken);
            return true;
        }

        public async Task<bool> IsTokenValid()
        {
            var token = await GetToken();

            if(token != null && token.ExpiresAt > DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }

        public async Task LogOut()
        {

            Token = null;
            await RemoveToken();

        }

    }
}
