using System.Collections.Generic;

namespace SistemaBancario
{
    public class GerenciadorDeContas
    {
        private List<ContaBancaria> contas = new List<ContaBancaria>();

        public void AdicionarConta(ContaBancaria conta)
        {
            contas.Add(conta);
        }

        public ContaBancaria BuscarConta(string numeroConta)
        {
        foreach (var conta in contas)
        {
        if (conta.NumeroConta == numeroConta)
        {
        return conta;
                }
        }
        return null;
        }

        public List<ContaBancaria> ObterTodas()
        {
        return contas;
        }
    }
}