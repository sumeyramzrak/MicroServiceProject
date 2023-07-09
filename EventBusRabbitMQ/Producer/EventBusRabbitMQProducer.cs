using EventBusRabbitMQ.Events.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBusRabbitMQ.Producer
{
    public class EventBusRabbitMQProducer
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection; //Producer üzerinde  event gönderebilmemiz için RabbitMQ ya bağlanabilmemiz gerekiyo.
        private readonly ILogger<EventBusRabbitMQProducer> _logger;
        private readonly int _retryCount; //Eventi connection üzerinden kullanarak Polly i kullanmak için ihtiyacımız olacak. Değişmediği durum içn default değer atıyoruz.

        public EventBusRabbitMQProducer(IRabbitMQPersistentConnection persistentConnection, ILogger<EventBusRabbitMQProducer> logger, int retryCount = 5)
        {
            _persistentConnection = persistentConnection;
            _logger = logger;
            _retryCount = retryCount;
        }

        //Producerın amacı bir tane event üretip bunu queueya bırakmak. Bu sebeple bir publish metoduna ihtiyacımız var.
        public void Publish(string queueName, IEvent @event)
        {
            /*
             Connectionı Singleton olarak ayağa kaldıracağız.Bu sebeple ilk connect olduktan sonra işlerine devam edecek.
             Ama ilk istekte daha connect olmadığı için bunun kontrol edilip connect değilse connectiona zorlanmalıdır.
            */
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {
                _logger.LogWarning(ex, "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})", @event.RequestId, $"{time.TotalSeconds:n1}", ex.Message);
            });

            //CreateModel metodu IModel tipinde bir nesne dönecek ve bu nesne üzerinden queue operasyonları gerçekleştirilecek.
            using (var channel = _persistentConnection.CreateModel()) //using içinde kullanıyoruz çünkü işimiz bittikten sonra dispose olmasını istiyoruz.
            {
                channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                #region QueueDeclareParametersDetail
                /*
         Queue oluşturup işlemleri bunun üzerinden yapmak istediğimizi belirtiyoruz.
        * durable: 
            -> false ise queueyu inmemory olarak kullanıyor ve gönderdiğimiz eventleri queue içindeki bilgileri inmemory içinde tutuyor. Restart olduğunda bilgiler queuedan siliniyor. kayboluyor.
            -> true ise fiziksel olarak rabbitmq sunucusunda bir yere kaydediyor ve restart olduğunda da silinmemesini sağlıyor.

        * exclusive
            -> true ise 
               - Queuenun tek bir connectiona sahip olmasını sağlıyor.
               - Tek bir consumer burayı connect edebilir. O consumerda silindiğinde connection kapanır ve queue silinir true ise

        * autoDelete
            ->  true ise 
               - queue en az bir consumer a sahipse son subscribe ortadan kalktığında queue otomatik silinir.
               - En az bir connectiona sahip olması gerekir.

        * arguments
            -> broker a özgü bazı parametreler(queue length vs) içerir. Bu parametrelerle konfigure edilebilir.

         */ 
                #endregion
                var message = JsonConvert.SerializeObject(@event); //Eventi json formatına çevirip mesaj olarak kuyruğa bırakacağız.
                var body = Encoding.UTF8.GetBytes(message);

                //publish işlemini bir kere deneyip hata aldığında bırakmasını istemediğimizden Polly ile oluşturduğumuz retry policy üzerinden execute ediyoruz.
                policy.Execute(() =>
                {
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = 2;

                    channel.ConfirmSelect();
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: queueName,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
                    channel.WaitForConfirmsOrDie();

                    channel.BasicAcks += (sender, eventArgs) =>
                    {
                        Console.WriteLine("Sent RabbitMQ");
                        //implement ack handle
                    };
                });
            }
        }
    }
}
