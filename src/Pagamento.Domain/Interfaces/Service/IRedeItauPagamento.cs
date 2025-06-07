using Pagamento.Domain.Contracts;

namespace Pagamento.Domain.Interfaces.Service
{
    public interface IRedeItauPagamento
    {
        public Task<TransactionsResponse> CriarTransacaoAsync(TransactionsRquest request);
    }
}