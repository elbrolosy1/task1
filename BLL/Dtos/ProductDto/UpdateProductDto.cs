using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dtos.ProductDto
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;

        public string? OldImageUrl { get; set; } 
        public IFormFile? NewCover { get; set; } 
    }
}
