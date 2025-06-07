using Domain.Enums;

namespace Pagamento.Domain.DTOs
{
    public class TransacaoDTO
    {
        public string TipoTransacao { get; set; }
        public int Valor { get; set; }
        public int NumeroParcelas { get; set; }
        public string NomeImpressoCartao { get; set; }
        public string NumeroCartao { get; set; }
        public int MesVencimentoCartao { get; set; }
        public int AnoVencimentoCartao { get; set; }
        public string CodigoSegurancaCartao { get; set; }
        public int CategoriaTransacao { get; set; }
    }

    public static class CodigoPedidoGenerator
    {
        public static string GerarCodigoPedido()
        {
            string prefixo = "g";

            string timestamp = DateTime.Now.ToString("ddMMyyHHmmssfff");

            string codigo = prefixo + timestamp;

            if (codigo.Length < 16)
            {
                codigo = codigo.PadRight(16, '0');
            }

            return codigo;
        }
    }
}