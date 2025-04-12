using System.ComponentModel.DataAnnotations;

namespace MVCPrinciples.Models
{
    public class ProductEditViewModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(40, ErrorMessage = "Name cannot exceed 40 characters")]
        public string ProductName { get; set; }

        [Display(Name = "Category")]
        public int? CategoryID { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierID { get; set; }

        [StringLength(20, ErrorMessage = "Quantity per unit cannot exceed 20 characters")]
        public string? QuantityPerUnit { get; set; }

        [Range(0, 10000, ErrorMessage = "Price must be between 0 and 10,000")]
        public decimal? UnitPrice { get; set; }

        [Range(0, 1000, ErrorMessage = "Units in stock must be between 0 and 1,000")]
        public short? UnitsInStock { get; set; }

        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        // Dropdown options
        public List<Category>? AvailableCategories { get; set; }
        public List<Supplier>? AvailableSuppliers { get; set; }
    }
}
