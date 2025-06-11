using FluentValidation;
using Pagamento.Domain.Contracts;
using Pagamento.Domain.DTOs;
using Pagamento.Domain.Interfaces.Service;
using Pagamento.Domain.Interfaces.UseCase;

namespace Pagamento.Domain.UseCases
{
    public class PagamentoUseCase : IPagamentoUseCase
    {
        private readonly IRedeItauPagamento _redeItauPagamento;
        private readonly IValidator<TransacaoDTO> _validator;

        public PagamentoUseCase(IRedeItauPagamento redeItauPagamento, IValidator<TransacaoDTO> validator)
        {
            _redeItauPagamento = redeItauPagamento;
            _validator = validator;
        }

        public async Task<TransacaoDTOResponse> RealizarTransacaoAsync(TransacaoDTO request)
        {
            TransacaoDTOResponse? response = null;

            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var mensagens = string.Join(" | ", validationResult.Errors.Select(e => e.ErrorMessage));

                response = new() { MensagemRetorno = mensagens, CodigoRetorno = "-1" };

                return response;
            }

            TransactionsRquest transactionsRquest = new TransactionsRquest
            {
                capture = false,
                kind = request.TipoTransacao,
                reference = CodigoPedidoGenerator.GerarCodigoPedido(),
                amount = request.Valor,
                installments = request.NumeroParcelas,
                cardholderName = request.NomeImpressoCartao,
                cardNumber = request.NumeroCartao,
                expirationMonth = request.MesVencimentoCartao,
                expirationYear = request.AnoVencimentoCartao,
                securityCode = request.CodigoSegurancaCartao,
                softDescriptor = "Compra automovel efetuada com sucesso",
                subscription = true,
                origin = 1,
                distributorAffiliation = 0,
                storageCard = "0",
                transactionCredentials = new()
                {
                    credentialId = request.CategoriaTransacao
                }
            };

            var result = await _redeItauPagamento.CriarTransacaoAsync(transactionsRquest);

            return response = new()
            {
                IdTransacao = result.tid,   
                Nsu = result.nsu,
                CodigoAutorizacao = result.authorizationCode,
                NumeroPedido = result.reference,
                CodigoRetorno = result.returnCode,
                MensagemRetorno = result.returnMessage
            };
        }
    }
}