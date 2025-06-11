using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Pagamento.Domain.Contracts;
using Pagamento.Domain.DTOs;
using Pagamento.Domain.Interfaces.Service;
using Pagamento.Domain.UseCases;

namespace Test.UseCase
{
    public class PagamentoUseCaseTest
    {
        private readonly PagamentoUseCase _pagamentoUseCase;
        private readonly Mock<IValidator<TransacaoDTO>> _validator;

        private readonly Mock<IRedeItauPagamento> _redeItauClient;

        public PagamentoUseCaseTest()
        {
            _redeItauClient = new Mock<IRedeItauPagamento>();
            _validator = new Mock<IValidator<TransacaoDTO>>();

            _pagamentoUseCase = new PagamentoUseCase(
                _redeItauClient.Object,
                _validator.Object
                );
        }

        [Fact]
        public async void DeveRealizarTransacao()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 2000,
                NumeroParcelas = 2,
                NumeroCartao = "5448280000000007",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "123",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult());

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.Equal(transactionsMockResponse.nsu, result.Nsu);
        }

        [Fact]
        public async void DeveRetornarErroQuandoValorTransacaoZerado()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 0,
                NumeroParcelas = 2,
                NumeroCartao = "5448280000000007",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "123",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("Valor", "O valor da transação deve ser maior que zero.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O valor da transação deve ser maior que zero.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoNumeroCartaoNaoInformadoENulo()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 0,
                NumeroParcelas = 2,
                NumeroCartao = null,
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "123",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("NumeroCartao", "O número do cartão é obrigatório.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O número do cartão é obrigatório.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoNumeroCartaoNaoInformadoEVazio()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 0,
                NumeroParcelas = 2,
                NumeroCartao = string.Empty,
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "123",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("NumeroCartao", "O número do cartão é obrigatório.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O número do cartão é obrigatório.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoNumeroCartaoNaoInformadoTemEspacoVazio()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 0,
                NumeroParcelas = 2,
                NumeroCartao = "     ",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "123",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("NumeroCartao", "O número do cartão é obrigatório.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O número do cartão é obrigatório.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoNumeroCartaoInvalido()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 0,
                NumeroParcelas = 2,
                NumeroCartao = "1111111111",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "123",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("NumeroCartao", "O número do cartão é inválido.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O número do cartão é inválido.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoMesVencimentoCartaoEInvalido()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 2000,
                NumeroParcelas = 2,
                NumeroCartao = "5448280000000007",
                MesVencimentoCartao = 13,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "123",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("MesVencimentoCartao", "O mês de vencimento deve ser entre 1 e 12.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O mês de vencimento deve ser entre 1 e 12.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoCodigoSegurancaCartaoForNulo()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 2000,
                NumeroParcelas = 2,
                NumeroCartao = "5448280000000007",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = null,
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("CodigoSegurancaCartao", "O código de segurança é obrigatório.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O código de segurança é obrigatório.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoCodigoSegurancaCartaoForVazio()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 2000,
                NumeroParcelas = 2,
                NumeroCartao = "5448280000000007",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = string.Empty,
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("CodigoSegurancaCartao", "O código de segurança é obrigatório.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O código de segurança é obrigatório.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoCodigoSegurancaCartaoTemEspacoVazio()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 2000,
                NumeroParcelas = 2,
                NumeroCartao = "5448280000000007",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "      ",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("CodigoSegurancaCartao", "O código de segurança é obrigatório.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O código de segurança é obrigatório.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoCodigoSegurancaCartaoContemQuantidadeDigitosInvalido()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 2000,
                NumeroParcelas = 2,
                NumeroCartao = "5448280000000007",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "12",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("CodigoSegurancaCartao", "O código de segurança deve ter 3 ou 4 dígitos.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O código de segurança deve ter 3 ou 4 dígitos.");
        }

        [Fact]
        public async void DeveRetornarErroQuandoCodigoSegurancaCartaoCaracteresInvalido()
        {
            TransacaoDTO request = new()
            {
                TipoTransacao = "credit",
                Valor = 2000,
                NumeroParcelas = 2,
                NumeroCartao = "5448280000000007",
                MesVencimentoCartao = 1,
                AnoVencimentoCartao = 2028,
                NomeImpressoCartao = "John Snow",
                CodigoSegurancaCartao = "12A",
                CategoriaTransacao = "01"
            };

            TransacaoDTOResponse response = new()
            {
                CodigoRetorno = "00",
                MensagemRetorno = "Sucesso",
                Nsu = "1341",
                IdTransacao = "456",
                NumeroPedido = "3443423523",
                CodigoAutorizacao = "1"
            };

            TransactionsResponse transactionsMockResponse = new()
            {

                reference = "g070625115353140",
                tid = "10012506081807243857",
                nsu = "454924197",
                authorizationCode = "944742",
                brandTid = "MCS614134117043",
                dateTime = new DateTime(2025, 06, 8),
                amount = 2000,
                installments = 2,
                cardBin = "544828",
                last4 = "0007",
                returnCode = "00",
                returnMessage = "Success.",
                links = [
                    new() { method = "GET", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "transaction" },
                    new() { method = "POST", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857/refunds", rel = "refund" },
                    new() { method = "PUT", href = "https://sandbox-erede.useredecloud.com.br/v1/transactions/10012506081807243857", rel = "capture" },
                    ]
            };

            _redeItauClient.Setup(x => x.CriarTransacaoAsync(It.IsAny<TransactionsRquest>())).ReturnsAsync(transactionsMockResponse);

            _validator.Setup(v => v.ValidateAsync(It.IsAny<TransacaoDTO>(), default))
                     .ReturnsAsync(new ValidationResult
                     {
                         Errors = { new ValidationFailure("CodigoSegurancaCartao", "O código de segurança deve ter 3 ou 4 dígitos.") }
                     });

            var result = await _pagamentoUseCase.RealizarTransacaoAsync(request);

            Assert.True(result.CodigoRetorno == "-1");
            Assert.True(result.MensagemRetorno == "O código de segurança deve ter 3 ou 4 dígitos.");
        }
    }
}