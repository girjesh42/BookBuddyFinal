using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BookBuddyFinal.Models
{
    public partial class Category
    {
        public Category()
        {
            InverseParent = new HashSet<Category>();
            ProductCategory = new HashSet<ProductCategory>();
            Products = new HashSet<Products>();
        }

        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }

        public virtual Category Parent { get; set; }
        public virtual ICollection<Category> InverseParent { get; set; }
        public virtual ICollection<ProductCategory> ProductCategory { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}
