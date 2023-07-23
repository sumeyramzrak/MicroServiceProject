using System.ComponentModel.DataAnnotations.Schema;

namespace Ordering.Domain.Entities.Base
{
    /// <summary>
    /// Her entity için ortak olan metotlar geliştirmek istiyorsak bu abstact class Entity i kullanabiliriz.
    /// Böylece her entity bütün metotları kazanmış olacak.
    /// </summary>
    public abstract class Entity : IEntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; protected set; } //Id nesnesine en yüksek entity seviyesinden ulaşılabilmesi için protected set

        /// <summary>
        /// İlgili entityi klonlamamızı sağlar.
        /// Bu metot sayesinde ilgili entity e eriştiğimizde örneğin entityden türeyen Order için Order.Clone dediğimizde Orderı klonlayarak yeni bir Order nesnesi oluşturur.
        /// </summary>
        /// <returns>Entity</returns>
        public Entity Clone()
        {
            return (Entity)this.MemberwiseClone();
        }
    }
}
