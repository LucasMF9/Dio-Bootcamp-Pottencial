using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tech_test_payment_api.Entities;
using tech_test_payment_api.Models;
using tech_test_payment_api.StatusValidacao;

namespace tech_test_payment_api.Manager
{
    public class VendaInsert
    {
        public VendaInsert()
        {
            this.DataVenda = DateTime.Now;
            this.statusVenda = StatusVenda.AguardandoPagamento;
        }
        public VendaInsert(int id, int numeroDoPedido, int vendedorId, int produtoId)
        {
            this.Id = id;
            this.NumeroDoPedido = numeroDoPedido;
            this.vendedorId = vendedorId;
            this.produtoId = produtoId;
            this.DataVenda = DateTime.Now;
            this.statusVenda = StatusVenda.AguardandoPagamento;
        }

        public VendaInsert(Venda venda)
        {
            this.Id = venda.Id;
            this.NumeroDoPedido = venda.NumeroDoPedido;
            this.vendedorId = venda.vendedor.Id;
            this.produtoId = venda.produto.Id;
            this.DataVenda = DateTime.Now;
            this.statusVenda = venda.statusVenda;
        }
        public int Id { get; set; }
        public DateTime  DataVenda { get; set; }
        public int NumeroDoPedido { get; set; }
        public int vendedorId { get; set; }
        public int produtoId{ get; set; }
        public StatusVenda statusVenda { get; set; }
    }
}