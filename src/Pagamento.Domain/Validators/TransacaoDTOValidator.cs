using FluentValidation;
using Pagamento.Domain.DTOs;

namespace Pagamento.Domain.Validators
{
    public class TransacaoDTOValidator : AbstractValidator<TransacaoDTO>
    {
        public TransacaoDTOValidator()
        {
            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");

            RuleFor(x => x.NumeroCartao)
                .NotEmpty().WithMessage("O número do cartão é obrigatório.")
                .CreditCard().WithMessage("O número do cartão é inválido.");

            RuleFor(x => x.MesVencimentoCartao)
            .InclusiveBetween(1, 12).WithMessage("O mês de vencimento deve ser entre 1 e 12.");

            RuleFor(x => x.AnoVencimentoCartao)
                .GreaterThanOrEqualTo(DateTime.Now.Year).WithMessage("O ano de vencimento deve ser o ano atual ou posterior.");

            RuleFor(x => x.Valor)
                .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");

            RuleFor(x => x.CodigoSegurancaCartao)
                .NotEmpty().WithMessage("O código de segurança é obrigatório.")
                .Length(3, 4).WithMessage("O código de segurança deve ter 3 ou 4 dígitos.")
                .Matches(@"^\d+$").WithMessage("Código de segurança deve conter apenas números.");
        }
    }
}