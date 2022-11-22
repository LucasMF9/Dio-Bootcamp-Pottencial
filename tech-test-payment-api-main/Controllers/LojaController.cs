using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Bank;
using tech_test_payment_api.Entities;
using tech_test_payment_api.Manager;
using tech_test_payment_api.Models;
using tech_test_payment_api.StatusValidacao;

namespace tech_test_payment_api.Controllers
{
    [ApiController]
    [Route("controller")]
    public class LojaController : ControllerBase
    {
        private BankDb _context { get; set;  }
        public LojaController(BankDb context)
        {
            this._context = context;
        }

        
        [HttpPost("Registrar Vendedor")]
        public IActionResult RegistrarVendedor(Vendedor vendedor)
        {
            var vendedorExiste = _context.Vendedores.Any(v => v.Id == vendedor.Id);
            if (vendedorExiste) return BadRequest("Vendedor ja existe");

            _context.Add(vendedor);
            _context.SaveChanges();
            return CreatedAtAction(nameof(BuscarVendedorPorId), new { id = vendedor.Id }, vendedor);
        }

        [HttpGet("Buscar Vendedor por Id")]
        public IActionResult BuscarVendedorPorId(int id)
        {
            var vendedorExiste = _context.Vendedores.Find(id);
            if (vendedorExiste == null)
                return NotFound("Não foi possivel encontrar nenhum vendedor com este Id");
            return Ok(vendedorExiste);
        }

        [HttpDelete("Deletar Vendedor")]
        public IActionResult DeletarVendedor(int id)
        {
            var vendedorExiste = _context.Vendedores.Find(id);
            if (vendedorExiste == null) return NotFound("Vendedor não encontrado");

            _context.Vendedores.Remove(vendedorExiste);
            _context.SaveChanges();

            return Ok($"O vendedor de id {id} foi deletado");

        }

        [HttpPost("Registra Produto")]
        public IActionResult RegistrarProduto(Produto produto)
        {
            var produtoExiste = _context.Produtos.Any(v => v.Id == produto.Id);
            if (produtoExiste)  return BadRequest("Produto ja existe");

            _context.Add(produto);
            _context.SaveChanges();
            return CreatedAtAction(nameof(BuscarProdutoPorId), new { id = produto.Id }, produto);
        }

        [HttpGet("Buscar produto por Id")]
        public IActionResult BuscarProdutoPorId(int id)
        {
            var produtoExiste = _context.Produtos.Find(id);
            if (produtoExiste == null) 
                return NotFound("Não foi encontrado nenhum produto com este Id");


            return Ok(produtoExiste);
        }

        [HttpDelete("Deletar um Produto")]
        public IActionResult DeletarProduto(int id)
        {
            var produtoExiste = _context.Produtos.Find(id);
            if(produtoExiste == null) return NotFound("Produto não encontrado");

            _context.Produtos.Remove(produtoExiste);
            _context.SaveChanges();

            return Ok($"O produto de id {id} foi deletado");

        }

        [HttpPost("Registrar Pedido")]
        public IActionResult Registrar(int idVendedor, int idProduto, VendaInsert vendaInsert)
        {
            var vendedorBancoId = _context.Vendedores.Find(idVendedor);
            if (vendedorBancoId == null)
                return NotFound("Não foi possivel encontrar nenhum vendedor com este Id, venda não registrada");

            var produtoBancoId = _context.Produtos.Find(idProduto);
            if (produtoBancoId == null) 
                return NotFound("Não foi encontrado nenhum produto com este Id, venda não registrada");

            vendaInsert.statusVenda = StatusVenda.AguardandoPagamento;
            Venda venda = new Venda();
            copyVendaInsertToVenda(vendaInsert, venda);
         
            _context.Add(venda);
            _context.SaveChanges();

            return Ok (venda);
        }

        private void copyVendaInsertToVenda(VendaInsert vendaInsert, Venda venda)
        {
            venda.Id = vendaInsert.Id;
            venda.NumeroDoPedido = vendaInsert.NumeroDoPedido;
            venda.statusVenda = vendaInsert.statusVenda;
            venda.vendedor = _context.Vendedores.Find(vendaInsert.vendedorId);
            venda.produto = _context.Produtos.Find(vendaInsert.produtoId);
        }

        [HttpGet("Buscar Pedido por Id")]
        public IActionResult BuscarVendaPorId(int id)
        {
            var vendaExiste = _context.Vendas.Include(v => v.vendedor).Include(v => v.produto).SingleOrDefault(v => v.Id == id);
            if(vendaExiste==null) return NotFound("Pedido não encontrado");


            return Ok(vendaExiste);
        }

        [HttpGet("Verificar Status do Pedido")]
        public IActionResult VerificarStatusPedido(int id)
        {

            var vendaExiste1 = _context.Vendas.Include(v => v.vendedor).Include(v => v.produto).SingleOrDefault(v => v.Id == id);
            if(vendaExiste1 == null) return NotFound("Pedido não encontrado");
            if(vendaExiste1.statusVenda == StatusVenda.AguardandoPagamento) return NotFound($"O pedido de id {id} está com o status 'Aguardando Pagamento'");
            if(vendaExiste1.statusVenda == StatusVenda.PagamentoAprovado ) return NotFound($"O pedido de id {id} está com o status 'Pagamento Aprovado'");
            if(vendaExiste1.statusVenda == StatusVenda.Cancelado) return NotFound($"O pedido de id {id} está com o status 'Cancelado'");
            if(vendaExiste1.statusVenda == StatusVenda.Entregue) return NotFound($"O pedido de id {id} está com o status 'Entregue'");
            if(vendaExiste1.statusVenda == StatusVenda.EnviadoParaTransportadora) return NotFound($"O pedido de id {id} está com o status 'Enviado Para Transportadora'");


            return Ok();
        }


        [HttpPatch("Atualizar Pedido para Pago")]
        public IActionResult AtualizarVendaParaPago(int id)
        {

            var vendaExiste1 = _context.Vendas.Include(v => v.vendedor).Include(v => v.produto).SingleOrDefault(v => v.Id == id);
            if(vendaExiste1 == null) return NotFound("Pedido não encontrado");
            if(vendaExiste1.statusVenda == StatusVenda.PagamentoAprovado ) return NotFound("O pedido ja foi pago");
            if(vendaExiste1.statusVenda == StatusVenda.Cancelado) return NotFound("Este pedido foi cancelado");
            if(vendaExiste1.statusVenda == StatusVenda.Entregue) return NotFound("Este pedido ja foi entregue");
            vendaExiste1.AtualizaVenda(StatusVenda.PagamentoAprovado);

            _context.Vendas.Update(vendaExiste1);
            _context.SaveChanges();


            return Ok($"O pedido de id {id} foi alterado com sucesso para {vendaExiste1.statusVenda}");
        }

        [HttpPatch("Atualizar Pedido para Cancelado")]
        public IActionResult AtualizarVendaParaCancelado(int id)
        {
            var vendaExiste1 = _context.Vendas.Include(v => v.vendedor).Include(v => v.produto).SingleOrDefault(v => v.Id == id);
            if(vendaExiste1 == null) return NotFound("Pedido não encontrado");
            if(vendaExiste1.statusVenda == StatusVenda.Cancelado) return NotFound("Este pedido já foi cancelado");

            vendaExiste1.AtualizaVenda(StatusVenda.Cancelado);

            _context.Vendas.Update(vendaExiste1);
            _context.SaveChanges();

            return Ok($"O pedido de id {id} foi alterado com sucesso para {vendaExiste1.statusVenda}");
        }

        [HttpPatch("Atualizar Pedido para Entregue")]
        public IActionResult AtualizarVendaParaEntregue(int id)
        {
            var vendaExiste1 = _context.Vendas.Include(v => v.vendedor).Include(v => v.produto).SingleOrDefault(v => v.Id == id);
            if(vendaExiste1 == null) return NotFound("Pedido não encontrado");
            if(vendaExiste1.statusVenda == StatusVenda.Entregue) return NotFound("Este pedido ja foi entregue");
            if(vendaExiste1.statusVenda == StatusVenda.Cancelado) return NotFound("Este pedido foi cancelado");
            if(vendaExiste1.statusVenda == StatusVenda.AguardandoPagamento) return NotFound("Este pedido está aguardando pagamento");

            vendaExiste1.AtualizaVenda(StatusVenda.Entregue);

            _context.Vendas.Update(vendaExiste1);
            _context.SaveChanges();

            return Ok($"O pedido de id {id} foi alterado com sucesso para {vendaExiste1.statusVenda}");
        }
        [HttpPatch("Atualizar Pedido para Enviado Para a Transportadora")]
        public IActionResult AtualizarVendaParaEnviadoParaTransportadora(int id)
        {
            var vendaExiste1 = _context.Vendas.Include(v => v.vendedor).Include(v => v.produto).SingleOrDefault(v => v.Id == id);
            if(vendaExiste1 == null) return NotFound("Pedido não encontrado");
            if(vendaExiste1.statusVenda == StatusVenda.Entregue) return NotFound("Este pedido ja foi entregue");
            if(vendaExiste1.statusVenda == StatusVenda.Cancelado) return NotFound("Este pedido foi cancelado");
            if(vendaExiste1.statusVenda == StatusVenda.AguardandoPagamento) return NotFound("Este pedido está aguardando pagamento");
            if(vendaExiste1.statusVenda == StatusVenda.EnviadoParaTransportadora) return NotFound("Este pedido ja foi enviado para a transportadora ");

            vendaExiste1.AtualizaVenda(StatusVenda.EnviadoParaTransportadora);

            _context.Vendas.Update(vendaExiste1);
            _context.SaveChanges();

            return Ok($"O pedido de id {id} foi alterado com sucesso para {vendaExiste1.statusVenda}");
        }

        [HttpDelete("Deletar Pedido")]
        public IActionResult DeletarPedido(int id)
        {
            var pedidoExiste = _context.Vendas.Find(id);
            if(pedidoExiste == null) return NotFound("Pedido não encontrado");

            _context.Vendas.Remove(pedidoExiste);
            _context.SaveChanges();


            return Ok($"O pedido de id {id} foi deletado");
        }

    
    }

}
