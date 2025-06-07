using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using System.Net;
using Pagamento.Domain.DTOs;

namespace Veiculo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagamentoController : ControllerBase
    {
        private readonly ILogger<PagamentoController> _logger;

        private readonly IPagamentoApplication _pagamentoApplication;

        public PagamentoController(ILogger<PagamentoController> logger, IPagamentoApplication pagamentoApplication)
        {
            _logger = logger;
            _pagamentoApplication = pagamentoApplication;
        }

        [HttpPost("Efeturar/Transacao")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AdicionarVeiculoAVenda([FromBody] TransacaoDTO request)
        {
            var response = await _pagamentoApplication.RealizarTransacaoApplicationAsync(request);

            if(response.CodigoRetorno != "00" && response.MensagemRetorno != "Success.")
                 return BadRequest(response);

            return Ok(response);
        }
    }
}