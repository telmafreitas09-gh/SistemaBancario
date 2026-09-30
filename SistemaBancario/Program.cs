using System;
using SistemaBancario.Interfaces;

namespace SistemaBancario
{
    public class Program
    {
        static void Main(string[] args)
        {
            GerenciadorDeContas gerenciador = new GerenciadorDeContas();
            bool executando = true;

            while (executando)
            {
                Console.Clear();
                ExibirCabecalho("SISTEMA BANCÁRIO");
                Console.WriteLine(" 1 - Cadastrar Conta Corrente");
                Console.WriteLine(" 2 - Cadastrar Conta Poupança");
                Console.WriteLine(" 3 - Realizar Depósito");
                Console.WriteLine(" 4 - Realizar Saque");
                Console.WriteLine(" 5 - Simular Rendimento Poupança");
                Console.WriteLine(" 6 - Consultar Extrato / Saldo");
                Console.WriteLine(" 7 - Relatório Geral de Contas");
                Console.WriteLine(" 0 - Sair");
                DesenharLinha('-');
                Console.Write(" Opção desejada: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CadastrarConta(gerenciador, "CORRENTE");
                        break;

                    case "2":
                        CadastrarConta(gerenciador, "POUPANÇA");
                        break;

                    case "3":
                        RealizarDeposito(gerenciador);
                        break;

                    case "4":
                        RealizarSaque(gerenciador);
                        break;

                    case "5":
                        SimularRendimentoPoupanca(gerenciador);
                        break;

                    case "6":
                        ConsultarExtrato(gerenciador);
                        break;

                    case "7":
                        ExibirRelatorioContasSeparadas(gerenciador);
                        break;

                    case "0":
                        executando = false;
                        Console.Clear();
                        ExibirCabecalho("SISTEMA ENCERRADO");
                        Console.WriteLine(" Obrigado por utilizar o nosso sistema!");
                        DesenharLinha('=');
                        break;

                    default:
                        Console.WriteLine("\n [ERRO] Opção inválida!");
                        PressionarParaContinuar();
                        break;
                }
            }
        }

   
        private static void CadastrarConta(GerenciadorDeContas gerenciador, string tipo)
        {
            Console.Clear();
            ExibirCabecalho($"CADASTRO - CONTA {tipo}");

            // 1. Verificação do Número e Bloqueio de Duplicidade
            Console.Write(" Número da Conta: ");
            string numeroConta = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(numeroConta))
            {
                Console.WriteLine("\n [ERRO] O número da conta não pode ser vazio!");
                DesenharLinha('=');
                PressionarParaContinuar();
                return;
            }

            if (gerenciador.BuscarConta(numeroConta) != null)
            {
                Console.WriteLine($"\n [ERRO] A conta número '{numeroConta}' já está cadastrada!");
                Console.WriteLine(" Operação cancelada para evitar duplicidade.");
                DesenharLinha('=');
                PressionarParaContinuar();
                return;
            }

            
            string titular = "";
            while (string.IsNullOrWhiteSpace(titular))
            {
                Console.Write(" Nome do Titular (obrigatório): ");
                titular = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(titular))
                {
                    Console.WriteLine(" [ERRO] O nome do titular não pode ser vazio!\n");
                }
            }

          
            Console.Write(" Saldo Inicial: R$ ");
            decimal.TryParse(Console.ReadLine(), out decimal saldoInicial);

            
            ContaBancaria novaConta;
            if (tipo == "POUPANÇA")
            {
                novaConta = new ContaPoupanca(numeroConta, titular.Trim(), saldoInicial);
            }
            else
            {
                novaConta = new ContaCorrente(numeroConta, titular.Trim(), saldoInicial);
            }

            gerenciador.AdicionarConta(novaConta);

            Console.WriteLine($"\n [SUCESSO] Conta {tipo} cadastrada com sucesso!");
            DesenharLinha('=');
            PressionarParaContinuar();
        }

        private static void RealizarDeposito(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("REALIZAR DEPÓSITO");

            Console.Write(" Número da Conta: ");
            string numero = Console.ReadLine()?.Trim();

            var conta = gerenciador.BuscarConta(numero);
            if (conta != null)
            {
                Console.WriteLine($" Titular: {conta.Titular}");
                Console.Write(" Valor do Depósito: R$ ");
                if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
                {
                    conta.Depositar(valor);
                    Console.WriteLine($"\n Depósito realizado! Novo saldo: R$ {conta.Saldo:F2}");
                }
                else
                {
                    Console.WriteLine("\n [ERRO] Valor de depósito inválido!");
                }
            }
            else
            {
                Console.WriteLine("\n [ERRO] Conta não encontrada!");
            }

            DesenharLinha('=');
            PressionarParaContinuar();
        }

        private static void RealizarSaque(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("REALIZAR SAQUE");

            Console.Write(" Número da Conta: ");
            string numero = Console.ReadLine()?.Trim();

            var conta = gerenciador.BuscarConta(numero);
            if (conta != null)
            {
                Console.WriteLine($" Titular: {conta.Titular}");
                Console.WriteLine($" Saldo Disponível: R$ {conta.Saldo:F2}");
                Console.Write(" Valor do Saque: R$ ");
                if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
                {
                    if (conta.Saldo >= valor)
                    {
                        conta.Sacar(valor);
                        Console.WriteLine($"\n Saque realizado! Saldo atual: R$ {conta.Saldo:F2}");
                    }
                    else
                    {
                        Console.WriteLine("\n [ERRO] Saldo insuficiente para o saque!");
                    }
                }
                else
                {
                    Console.WriteLine("\n [ERRO] Valor de saque inválido!");
                }
            }
            else
            {
                Console.WriteLine("\n [ERRO] Conta não encontrada!");
            }

            DesenharLinha('=');
            PressionarParaContinuar();
        }

        private static void SimularRendimentoPoupanca(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("RENDIMENTO POUPANÇA");

            Console.Write(" Número da Conta Poupança: ");
            string numero = Console.ReadLine()?.Trim();

            var conta = gerenciador.BuscarConta(numero);

            if (conta != null && conta is ContaPoupanca)
            {
                Console.WriteLine($" Titular: {conta.Titular}");
                Console.WriteLine($" Saldo Atual: R$ {conta.Saldo:F2}");

                Console.Write(" Meses em rendimento: ");
                int.TryParse(Console.ReadLine(), out int meses);

                if (meses > 0)
                {
                    decimal taxaMensal = 0.005m; // 0.5% ao mês
                    decimal saldoProjetado = conta.Saldo * (decimal)Math.Pow((double)(1 + taxaMensal), meses);
                    decimal rendimentoTotal = saldoProjetado - conta.Saldo;

                    DesenharLinha('-');
                    Console.WriteLine($" Taxa Mensal: 0,5% a.m.");
                    Console.WriteLine($" Rendimento Estimado: R$ {rendimentoTotal:F2}");
                    Console.WriteLine($" Saldo Final Projetado: R$ {saldoProjetado:F2}");
                }
                else
                {
                    Console.WriteLine("\n [ERRO] Quantidade de meses inválida!");
                }
            }
            else if (conta != null && !(conta is ContaPoupanca))
            {
                Console.WriteLine("\n [ERRO] A conta informada não é uma Conta Poupança!");
            }
            else
            {
                Console.WriteLine("\n [ERRO] Conta não encontrada!");
            }

            DesenharLinha('=');
            PressionarParaContinuar();
        }

        private static void ConsultarExtrato(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("EXTRATO / SALDO");

            Console.Write(" Número da Conta: ");
            string numero = Console.ReadLine()?.Trim();

            var conta = gerenciador.BuscarConta(numero);
            if (conta != null)
            {
                DesenharLinha('-');
                Console.WriteLine($" Titular: {conta.Titular}");
                Console.WriteLine($" Número: {conta.NumeroConta}");
                Console.WriteLine($" Saldo Atual: R$ {conta.Saldo:F2}");
            }
            else
            {
                Console.WriteLine("\n [ERRO] Conta não encontrada!");
            }

            DesenharLinha('=');
            PressionarParaContinuar();
        }

        private static void ExibirRelatorioContasSeparadas(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("RELATÓRIO GERAL DE CONTAS");

            var todasContas = gerenciador.ObterTodas();

            if (todasContas == null || todasContas.Count == 0)
            {
                Console.WriteLine(" [AVISO] Nenhuma conta cadastrada.");
                DesenharLinha('=');
                PressionarParaContinuar();
                return;
            }

            // Bloco de Contas Correntes
            Console.WriteLine(" --- CONTAS CORRENTES ---");
            bool encontrouCorrente = false;

            foreach (var conta in todasContas)
            {
                if (conta is ContaCorrente)
                {
                    Console.WriteLine($" Cta: {conta.NumeroConta} | Titular: {conta.Titular}");
                    Console.WriteLine($" Saldo: R$ {conta.Saldo:F2}");
                    DesenharLinha('.');
                    encontrouCorrente = true;
                }
            }

            if (!encontrouCorrente)
            {
                Console.WriteLine(" Nenhuma Conta Corrente cadastrada.");
                DesenharLinha('.');
            }

            // Bloco de Contas Poupança
            Console.WriteLine("\n --- CONTAS POUPANÇA ---");
            bool encontrouPoupanca = false;

            foreach (var conta in todasContas)
            {
                if (conta is ContaPoupanca)
                {
                    Console.WriteLine($" Cta: {conta.NumeroConta} | Titular: {conta.Titular}");
                    Console.WriteLine($" Saldo: R$ {conta.Saldo:F2}");
                    DesenharLinha('.');
                    encontrouPoupanca = true;
                }
            }

            if (!encontrouPoupanca)
            {
                Console.WriteLine(" Nenhuma Conta Poupança cadastrada.");
                DesenharLinha('.');
            }

            DesenharLinha('=');
            PressionarParaContinuar();
        }

        // =======================================================
        // MÉTODOS AUXILIARES DE INTERFACE (RÉGUA 40 CHARS)
        // =======================================================

        public static void DesenharLinha(char caractere = '=') =>
            Console.WriteLine(new string(caractere, 40));

        public static void ExibirCabecalho(string titulo)
        {
            DesenharLinha('=');
            int espacos = Math.Max(0, (40 - titulo.Length) / 2);
            Console.WriteLine($"{new string(' ', espacos)}{titulo.ToUpper()}");
            DesenharLinha('=');
        }

        private static void PressionarParaContinuar()
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}