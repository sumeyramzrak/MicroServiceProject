using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace EventBusRabbitMQ
{
    public class DefaultRabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        //RabbitMQ connection oluşturmak için yine RabbitMq kütüpphanesinden gelen ConnectionFactory interface ine ihtiyacımız var.
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private readonly int _retryCount;
        private readonly ILogger<DefaultRabbitMQPersistentConnection> _logger;
        private bool _disposed;

        public DefaultRabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount, ILogger<DefaultRabbitMQPersistentConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _retryCount = retryCount;
            _logger = logger;
        }

        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
                //connection null değilse, açıksa ve dispose olmamışsa connectionımız var diyebiliriz.
            }
        }

        public IModel CreateModel()
        {
            //IModel de queue management işlemlerini yaptığımız metotlar bulunuyor.
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            /*
            TryConnect ile connection factory üzerinden connection oluşturmasını sağladık.
            Ama bu connection üzerinden RabbitMq queuelara erişimi ile ilgili işlemler yapılamıyor. Bunun için IModel sınıfına ihtiyacımız var.
            Connection işelmleri başarılıysa _connection property si üzerinden CreateModel metodunu kullanarak queue management işlemlerini yapabilmek için IModel tipinde bir nesne geriye döndürüyoruz
            */
            return _connection.CreateModel();
        }

        //IDisposable dan türettiğimiz için dispose metoduna ihtiyaç duyuluyor.
        public void Dispose()
        {
            //dispose olmuşsa geri döndür
            if (_disposed) return;

            //değilse _disposed true olarak güncelle ve connectionı dispose et.
            _disposed = true;
            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");

            //Polly frameworkunu kullanarak bir policy tanımlıyoruz ve connectionı yapısını oluşturuyoruz.
            //Bir kere deneyip connect olamadığında hata alıp işlemin durmaması tekrar tekrar connect olmayı denemesi için Polly i kullanıyoruz.
            var policy = RetryPolicy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex, "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})", $"{time.TotalSeconds:n1}", ex.Message);
                });

            //ilgili policy üzerinden belirtilen metotlar çalıştırılacak.
            policy.Execute(() => {
                _connection = _connectionFactory.CreateConnection();
            });

            //işlemler başarılı değilse RabbitMq conneciton eventleri ile callbackler ile eventleri bağladık.
            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);

                return true;
            }
            else
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");

                return false;
            }
        }
        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }
    }
}
