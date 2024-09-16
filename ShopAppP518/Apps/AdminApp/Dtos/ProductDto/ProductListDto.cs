using ShopAppP518.Entities;

namespace ShopAppP518.Apps.AdminApp.Dtos.ProductDto
{
    public class ProductListDto
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public List<ProductListItemDto> Items { get; set; }
        public string productImageName { get; set; }
    }
}
