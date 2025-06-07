using Microsoft.Extensions.DependencyInjection;

namespace Cooperativa.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Adicione seus serviços, handlers, validators, etc aqui
            // Ex: services.AddScoped<IContatoFavoritoService, ContatoFavoritoService>();

            return services;
        }
    }
}
