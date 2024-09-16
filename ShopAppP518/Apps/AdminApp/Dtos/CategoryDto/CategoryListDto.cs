namespace ShopAppP518.Apps.AdminApp.Dtos.CategoryDto
{
    public class CategoryListDto
    {
        public int Page { get; set; }
        public int TotalCount { get; set; }
        public List<CategoryListItemDto> categories { get; set; }
    }
}
