using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pagamento.Domain.DTOs
{
    public class TransacaoDTOResponse
    {
        public string NumeroPedido { get; set; }
        public string IdTransacao  { get; set; }
        public string Nsu { get; set; }
        public string CodigoAutorizacao { get; set; }
        public string CodigoRetorno { get; set; }
        public string MensagemRetorno { get; set; }
    }
}