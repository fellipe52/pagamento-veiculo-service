using Pagamento.Domain.DTOs;

namespace Pagamento.Domain.Interfaces.UseCase
{
    public interface IPagamentoUseCase
    {
        public Task<TransacaoDTOResponse> RealizarTransacaoAsync(TransacaoDTO request);
    }
}