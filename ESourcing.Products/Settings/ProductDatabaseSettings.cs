namespace ESourcing.Products.Settings
{
    public class ProductDatabaseSettings : IProductDatabaseSettings
    {
        public string ConnectionSettings { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
