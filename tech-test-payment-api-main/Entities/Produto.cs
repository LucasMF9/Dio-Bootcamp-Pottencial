using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tech_test_payment_api.Manager;

namespace tech_test_payment_api.Entities
{
    public class Produto
    {
        public Produto()
        {

        }
        public Produto(int id, string nome, decimal preco)
        {
            this.Id = id;
            this.Nome = nome;
            this.Preco = preco;
        }
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal  Preco { get; set; }

    }
}