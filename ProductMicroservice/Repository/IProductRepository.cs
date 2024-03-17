using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using ProductMicroservice.Models;

namespace ProductMicroservice.Repository
{
    public interface IProductRepository
    {
        void DeleteProduct(int productId);
        Product GetProductByID(int productId);
        IEnumerable<Product> GetProducts();
        IPagedList<Product> GetProductsPerPage(int pageNumber, int pageSize);
        void InsertProduct(Product product);
        void Save();
        void UpdateProduct(Product product);
    }
}
