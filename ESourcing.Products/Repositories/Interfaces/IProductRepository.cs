using ESourcing.Products.Entities;

namespace ESourcing.Products.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(string id);
        Task<IEnumerable<Product>> GetPtoductByName(string name);
        Task<IEnumerable<Product>> GetPtoductByCategory(string category);

        Task Create(Product product);
        Task<bool> Update(Product product);
        Task<bool> Delete(String id);

    }
}
