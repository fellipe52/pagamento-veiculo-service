using FluentValidation;
using Pagamento.Domain.DTOs;
using Pagamento.Domain.Interfaces.UseCase;
using Pagamento.Domain.UseCases;
using Pagamento.Domain.Validators;

namespace Veiculo.API.Extensions
{
    public static class DomainExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<TransacaoDTO>, TransacaoDTOValidator>();

            services.AddScoped<IPagamentoUseCase, PagamentoUseCase>();

            return services;
        }
    }
}