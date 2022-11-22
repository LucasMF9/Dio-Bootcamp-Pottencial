using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tech_test_payment_api.Models
{
    public class Vendedor
    {
        public Vendedor()
        {

        }
        public Vendedor(int id, string cpf, string nome, string telefone, string email )
        {
            this.Id = id;
            this.Cpf = cpf;
            this.Nome = nome;
            this.Telefone = telefone;
            this.Email = email;
            

        }


        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        
       
    }
}