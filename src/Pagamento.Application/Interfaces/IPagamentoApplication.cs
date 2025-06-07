using Pagamento.Domain.DTOs;

namespace Application.Interfaces
{
    public interface IPagamentoApplication
    {
        public Task<TransacaoDTOResponse> RealizarTransacaoApplicationAsync(TransacaoDTO request);
    }
}