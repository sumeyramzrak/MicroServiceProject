using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Events.Interfaces
{
    public abstract class IEvent
    {
        //Abstract olarak tanımlanmasının sebebi constructorında bazı işlemler yapılacak olması.
        //Interfacelerde constructor olmadığı için abstract class kullandık.

        public Guid RequestId { get; private init; } //Her event oluştuğunda uniq bir guid üzerinden takip edebilmemizi sağlayacak.
        public DateTime CreationDate { get; private init; } //Eventler oluşturulduğu tarih üzerinden de takip edilecek.

        public IEvent()
        {
            RequestId = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

    }
}
