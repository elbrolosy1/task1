using BLL.Dtos.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ProductServices
{
    public interface IProductServices
    {
        Task<IEnumerable<ReadProductDto>> GetAllProductsAsync();
        Task<ReadProductDto> GetProductByIdAsync(int id);
        Task CreateProductAsync(CreateProductDto productDto);
        Task<UpdateProductDto> UpdateProductAsync(UpdateProductDto productDto);
        Task DeleteProductAsync(int id);
    }
}
