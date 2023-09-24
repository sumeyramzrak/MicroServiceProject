using ESourcing.Order.Consumers;

namespace ESourcing.Order.Extensions
{
    /*
     *Extension geliştirmemizin sebebi uygulamayı ayağa kaldırırken uygulamanın lifetimeında,
     * start/stop olduğunda uygulamayı bu consumer üzerinden başlatmak durdurmak gibi şilemleri yönetmek.
     */
    public static class ApplicationBuilderExtensions
    {
        public static EventBusOrderCreateConsumer Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<EventBusOrderCreateConsumer>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            //application ayağa kalktığında çalışacak.
            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            Listener.Consume();
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }
    }
}
