using BLL.Attribute;
using BLL.Setting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ProductDto
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Cover image is required")]
        [Display(Name = "Cover Image")]
        [AllowedExtentionValidation(FileSetting.AllowedImages)]
        [Size(FileSetting.MaxImageSizeByte)]
        public IFormFile? Cover { get; set; }

    }
}
