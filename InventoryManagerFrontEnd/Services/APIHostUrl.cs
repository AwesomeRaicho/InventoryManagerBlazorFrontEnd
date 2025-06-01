using InventoryManagerFrontEnd.Interfaces;

namespace InventoryManagerFrontEnd.Services
{
    public class APIHostUrl : IAPIHostUrl
    {
        private readonly string? _hostUrl;

        public APIHostUrl()
        {
            _hostUrl = "https://localhost:7200";
        }


        public string GetHostUrl() => _hostUrl ?? throw new Exception("Host Url is not added in APIHostUrl implementation service.");
    }
}
