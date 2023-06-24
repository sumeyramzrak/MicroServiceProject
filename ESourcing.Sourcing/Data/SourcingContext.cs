using ESourcing.Sourcing.Entities;
using ESourcing.Sourcing.Settings;
using MongoDB.Driver;

namespace ESourcing.Sourcing.Data
{
    public class SourcingContext : ISourcingContext
    {
        /*Sourcing context i çağırdığımız yerde MongoClient ın da oluşmasını istiyoruz.
         * Sourcing context içinde MongoClient ını en kapsüle edip farklı katmanlarda hiç MongoClientı tekrar oluşturmak için uğraşmadan
         * context nesnesi üzerinden ilgili collectionlara ulaşmasını sağlamak adına constructor içinde MongoClient ı oluşturuyoruz.
         */

        public SourcingContext(ISourcingDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            /*Collectionlar altındaki nesneler için referans oluşturuyoruz. Oradaki nesneleri Auctions/Bids altına eklemek demek değil.
            Sonrasında SourcingContext.Auctions diyerek o referansla servis katmanında queryler oluşturup 
            ihtiyacımıza göre servislerimizi apile aracılığı ile clienta sunmuş olacağız.
            */
            Auctions = database.GetCollection<Auction>(nameof(Auction));
            Bids = database.GetCollection<Bid>(nameof(Bid));

            //SourcingContext(MongoDb) ayağa kalktığında içerisindeki collectionlar boş ise preloading yapabilmeyi sağlar.
            SourcingContextSeed.SeedData(Auctions);

        }

        public IMongoCollection<Auction> Auctions { get; }

        public IMongoCollection<Bid> Bids { get; }
    }
}
