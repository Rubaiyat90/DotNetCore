using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RazorMVC.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [Range(1, 100, ErrorMessage = "Range must be between 1 to 100")]
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }

    }
}
