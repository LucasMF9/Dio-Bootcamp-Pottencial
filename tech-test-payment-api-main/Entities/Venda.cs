using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tech_test_payment_api.StatusValidacao;
using tech_test_payment_api.Entities;
using tech_test_payment_api.Manager;

namespace tech_test_payment_api.Models
{
    public class Venda
    {
        private Venda venda;

        public Venda()
        {
            this.DataVenda = DateTime.Now;
            this.statusVenda = StatusVenda.AguardandoPagamento;

        }

        public Venda(Venda venda)
        {
            this.venda = venda;
        }

        public Venda(int id, int NumeroDoPedido, Vendedor vendedor, Produto produto)
        {
            this.Id = id;
            this.NumeroDoPedido = NumeroDoPedido;
            this.vendedor = vendedor;
            this.produto = produto;
            this.DataVenda = DateTime.Now;
            this.statusVenda = StatusVenda.AguardandoPagamento;
        }

        public bool AtualizaVenda(StatusVenda statusVenda)
        {
            this.statusVenda = statusVenda;
            return true;
        }

        public int Id { get; set; }
        public int NumeroDoPedido { get; set; }
        public Vendedor vendedor{ get; set; }
        public Produto produto { get; set; }
        public DateTime DataVenda { get; set; }
        public StatusVenda statusVenda { get; set; }
    

    }
}