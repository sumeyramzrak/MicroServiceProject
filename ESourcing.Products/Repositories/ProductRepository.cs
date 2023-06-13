using ESourcing.Products.Data.Interfaces;
using ESourcing.Products.Entities;
using ESourcing.Products.Repositories.Interfaces;
using MongoDB.Driver;

namespace ESourcing.Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IProductContext _context;
        public ProductRepository(IProductContext context)
        {
            _context = context;
        }
        public async Task Create(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> Delete(string id)
        {
            //Builders nesnesi mongodb üzeinde işlemlerin immutable olarak yapılabilmesini sağlar
            var filter = Builders<Product>.Filter.Eq(m => m.Id, id);
            DeleteResult deleteResult=await _context.Products.DeleteOneAsync(filter);
            //Silme işlemi başarılı olduysa ve silinen kayıt sayısı sıfırdan büyükse bunu geriye dön.
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetPtoductByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(m => m.Category, category);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            //ElemMatch : Eleyerek getirir
            var filter = Builders<Product>.Filter.ElemMatch(m => m.Name, name);
            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task<bool> Update(Product product)
        {
            var updateResult = await _context.Products.ReplaceOneAsync(filter: g => g.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount> 0;
        }
    }
}
