namespace ESourcing.Products.Settings
{
    public interface IProductDatabaseSettings
    {
        public string ConnectionSetting { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }
    }
}
