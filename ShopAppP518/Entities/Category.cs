﻿namespace ShopAppP518.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDelete { get; set; }
    }
}
