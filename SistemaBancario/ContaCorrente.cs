using System;

namespace SistemaBancario
{
    public class ContaCorrente : ContaBancaria
    {
        public decimal TaxaManutencao { get; private set; } = 10.00m;

        public ContaCorrente(string numeroConta, string titular, decimal saldoInicial)
            : base(numeroConta, titular, saldoInicial)
        {
        }

        public override void Sacar(decimal valor)
        {
            if (!Ativa)
            {
                Console.WriteLine("Operação não permitida: a conta está inativa.");
                return;
            }

            if (valor <= 0)
            {
                Console.WriteLine("O valor do saque deve ser maior que zero.");
                return;
            }

            if (Saldo >= valor)
            {
                Saldo -= valor;
                Console.WriteLine($"Saque de R$ {valor:F2} realizado com sucesso.");
                Console.WriteLine($"Novo saldo: R$ {Saldo:F2}");
            }
            else
            {
                Console.WriteLine("Saldo insuficiente para realizar o saque.");
            }
        }
    }
}