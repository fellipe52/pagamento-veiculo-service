using Application.Interfaces;
using Pagamento.Domain.DTOs;
using Pagamento.Domain.Interfaces.UseCase;

namespace Application
{
    public class PagamentoApplication : IPagamentoApplication
    {
        private readonly IPagamentoUseCase _pagamentoUseCase;
        public PagamentoApplication(IPagamentoUseCase pagamentoUseCase)
        {
            _pagamentoUseCase = pagamentoUseCase;
        }

        public async Task<TransacaoDTOResponse> RealizarTransacaoApplicationAsync(TransacaoDTO request)
        {
            return await _pagamentoUseCase.RealizarTransacaoAsync(request);
        }
    }
}