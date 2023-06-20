namespace ESourcing.Products.Settings
{
    public class ProductDatabaseSettings : IProductDatabaseSettings
    {
        public string ConnectionSetting { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
