using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using tech_test_payment_api.Entities;
using tech_test_payment_api.Models;

namespace tech_test_payment_api.Bank
{
    public class BankDb : DbContext
    {
        public BankDb(DbContextOptions<BankDb> options) : base(options)
        {

        }
        public DbSet<Venda> Vendas { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Vendedor> Vendedores { get; set; }


    }

}