using Application;
using Application.Interfaces;
using Domain.Notification;
namespace Veiculo.API.Extensions
{
    public static class ApplicationExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IPagamentoApplication, PagamentoApplication>();

            services.AddScoped<NotificationContext>();

            return services;
        }
    }
}