using System;

namespace SistemaBancario
{
    public class ContaPoupanca : ContaBancaria
    {
        public ContaPoupanca(string numeroConta, string titular, decimal saldoInicial)
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

        public void CalcularRendimento(decimal percentual)
        {
            if (!Ativa)
            {
                Console.WriteLine("Operação não permitida: a conta está inativa.");
                return;
            }

            if (percentual > 0)
            {
                decimal rendimento = Saldo * (percentual / 100);
                Saldo += rendimento;
                Console.WriteLine($"Rendimento de {percentual}% aplicado com sucesso!");
                Console.WriteLine($"Valor rendido: R$ {rendimento:F2} | Novo saldo: R$ {Saldo:F2}");
            }
            else
            {
                Console.WriteLine("O percentual deve ser maior que zero.");
            }
        }
    }
}