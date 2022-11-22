using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tech_test_payment_api.StatusValidacao
{
    public enum StatusVenda
    {
        AguardandoPagamento,
        PagamentoAprovado,
        EnviadoParaTransportadora,
        Entregue,
        Cancelado

    }
}