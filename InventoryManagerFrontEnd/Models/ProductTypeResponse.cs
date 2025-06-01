namespace InventoryManagerFrontEnd.Models
{

    public class ProductTypeListResponse
    {
        public List<ProductTypeResponse>? ProductTypes { get; set; }
    }


    public class ProductTypeResponse
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public byte[]? ConcurrencyStamp { get; set; }
    }
}
