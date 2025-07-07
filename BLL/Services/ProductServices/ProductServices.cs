using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using BLL.Dtos.ProductDto;
using BLL.Setting;
using DAL.Models;
using DAL.Repo.GenericRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace BLL.Services.ProductServices
{
    public class ProductServices : IProductServices
    {
        private readonly IGenericRepo<Product> _repo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public ProductServices(
            IGenericRepo<Product> repo,
            IWebHostEnvironment webHostEnvironment)
        {
            _repo = repo;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = Path.Combine(_webHostEnvironment.WebRootPath, FileSetting.ImagesPath);

            if (!Directory.Exists(_imagesPath))
            {
                Directory.CreateDirectory(_imagesPath);
            }
        }

        public async Task<IEnumerable<ReadProductDto>> GetAllProductsAsync()
        {
            var query = await _repo.GetAllAsync();

            return await query
                .AsNoTracking()
                .Select(product => new ReadProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    ImageUrl = product.imgUrl
                })
                .ToListAsync();
        }

        public async Task<ReadProductDto> GetProductByIdAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            return new ReadProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.imgUrl
            };
        }

        public async Task CreateProductAsync(CreateProductDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (model.Cover == null || model.Cover.Length == 0)
                throw new ArgumentException("Image file is required.");

            var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";
            var path = Path.Combine(_imagesPath, imageName);

            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Cover.CopyToAsync(stream);
                }

                var newProduct = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    imgUrl = imageName
                };

                await _repo.AddAsync(newProduct);
                await _repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                throw new Exception("Failed to create product.", ex);
            }
        }
        public async Task<UpdateProductDto> UpdateProductAsync(UpdateProductDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var product = await _repo.GetByIdAsync(model.Id);
            if (product == null) throw new KeyNotFoundException("Product not found");

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;

            if (model.NewCover != null && model.NewCover.Length > 0)
            {
                var newImageName = $"{Guid.NewGuid()}{Path.GetExtension(model.NewCover.FileName)}";
                var newImagePath = Path.Combine(_imagesPath, newImageName);

                try
                {
                    using (var stream = new FileStream(newImagePath, FileMode.Create))
                    {
                        await model.NewCover.CopyToAsync(stream);
                    }

                    // حذف الصورة القديمة
                    if (!string.IsNullOrEmpty(product.imgUrl))
                    {
                        var oldImagePath = Path.Combine(_imagesPath, product.imgUrl);
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }

                    product.imgUrl = newImageName;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to update product image.", ex);
                }
            }

            try
            {
                await _repo.UpdateAsync(product);
                await _repo.SaveChangesAsync();
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update product.", ex);
            }
        }


        public async Task DeleteProductAsync(int id)
        {
            var product = await _repo.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found");

            try
            {
                await _repo.DeleteAsync(product.Id);
                await _repo.SaveChangesAsync();

                if (!string.IsNullOrEmpty(product.imgUrl))
                {
                    var imagePath = Path.Combine(_imagesPath, product.imgUrl);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete product.", ex);
            }
        }
    }
}
