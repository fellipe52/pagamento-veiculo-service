using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CategoriaTransacao
    {
        PeloClienteCredencialArmazenada = 01,
        PeloClienteOrdemPermanente = 02,
        PeloClienteAssinatura = 03,
        PeloClienteParcelamento = 04,
        PeloEstabelecimentoPagamentoRecorrenteCredencialArmazenadaNaoProgramada = 05,
        PeloEstabelecimentoPagamentoRecorrenteOrdemPermanente = 06,
        PeloEstabelecimentoPagamentoRecorrenteAssinatura = 07,
        PeloEstabelecimentoPagamentoRecorrenteParcelado = 08,
        PeloEstabelecimentoPraticasIndustriaRemessaParcial = 09,
        PeloEstabelecimentoPraticasIndustriaCobrancaAtrasada = 10,
        PeloEstabelecimentoPraticasIndustriaNoShow = 11,
        PeloEstabelecimentoPraticasIndustriaReenvio = 12
    }
}