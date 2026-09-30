using System;
using SistemaBancario.Interfaces;

namespace SistemaBancario
{
    public abstract class ContaBancaria : IConta
    {
        private static int _contadorId = 1;

        public int Id { get; private set; }
        public string NumeroConta { get; protected set; }
        public string Titular { get; protected set; }
        public decimal Saldo { get; protected set; }
        public DateTime DataAbertura { get; private set; }
        public bool Ativa { get; protected set; }

        public ContaBancaria(string numeroConta, string titular, decimal saldoInicial)
        {
            Id = _contadorId++;
            NumeroConta = numeroConta;
            Titular = titular;
            Saldo = saldoInicial;
            DataAbertura = DateTime.Now;
            Ativa = true;
        }

        public virtual void Depositar(decimal valor)
        {
            if (!Ativa)
            {
                Console.WriteLine("Operação não permitida: a conta está inativa.");
                return;
            }

            if (valor > 0)
            {
                Saldo += valor;
                Console.WriteLine($"Depósito de R$ {valor:F2} realizado com sucesso.");
                Console.WriteLine($"Novo saldo: R$ {Saldo:F2}");
            }
            else
            {
                Console.WriteLine("O valor do depósito deve ser maior que zero.");
            }
        }

        public abstract void Sacar(decimal valor);

              public virtual void ExibirDetalhes()
        {
            string status = Ativa ? "Ativa" : "Inativa";
            Console.WriteLine($"[ID: {Id}] Conta: {NumeroConta} | Titular: {Titular} | Saldo: R$ {Saldo:F2} | Status: {status}");
        }
    }
}