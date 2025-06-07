using FluentValidation;
using Pagamento.Domain.DTOs;
using Pagamento.Domain.Interfaces.Service;
using Pagamento.Domain.Interfaces.UseCase;
using Pagamento.Domain.UseCases;
using Pagamento.Domain.Validators;
using Pagamento.Infrastructure.Service;

namespace Veiculo.API.Extensions
{
    public static class InfrastructureExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var pdv = configuration["eredeClient:pdv"];
            var token = configuration["eredeClient:token"];

            services.AddScoped<IRedeItauPagamento>(provider =>
                               new RedeItauPagamentoClient(pdv, token));

            services.AddScoped<IValidator<TransacaoDTO>, TransacaoDTOValidator>();

            services.AddScoped<IPagamentoUseCase, PagamentoUseCase>();

            return services;
        }
    }
}