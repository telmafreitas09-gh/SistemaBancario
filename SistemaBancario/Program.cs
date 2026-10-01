using System;
using System.Linq;
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
         try
               {
                    Console.Clear();
                    ExibirCabecalho("SISTEMA BANCÁRIO");

                    Console.WriteLine("\n 1 - Cadastrar Conta Corrente");
                    Console.WriteLine(" 2 - Cadastrar Conta Poupança");
                    Console.WriteLine(" 3 - Realizar Depósito");
                    Console.WriteLine(" 4 - Realizar Saque");
                    Console.WriteLine(" 5 - Simular Rendimento Poupança");
                    Console.WriteLine(" 6 - Consultar Extrato / Saldo");
                    Console.WriteLine(" 7 - Relatório Geral de Contas");
                    Console.WriteLine(" 0 - Sair");
                    DesenharLinha('-');
                    Console.Write(" Opção desejada: ");

                    switch (Console.ReadLine())
                    {
                        case "1": CadastrarConta(gerenciador, "CORRENTE"); break;
                        case "2": CadastrarConta(gerenciador, "POUPANÇA"); break;
                        case "3": RealizarDeposito(gerenciador); break;
                        case "4": RealizarSaque(gerenciador); break;
                        case "5": SimularRendimentoPoupanca(gerenciador); break;
                        case "6": ConsultarExtrato(gerenciador); break;
                        case "7": ExibirRelatorioContasSeparadas(gerenciador); break;
                        case "0": executando = EncerrarSistema(); break;
                        default: ExibirMensagemErro("Opção inválida!"); break;
                    }
                }
                catch (Exception ex)
                {
                    ExibirMensagemErro($"Ocorreu um erro inesperado: {ex.Message}");
                }
            }
        }

        private static void CadastrarConta(GerenciadorDeContas gerenciador, string tipo)
        {
            Console.Clear();
            ExibirCabecalho($"CADASTRO - CONTA {tipo}");

            string numeroConta = LerNumeroConta();

            if (gerenciador.BuscarConta(numeroConta) != null)
            {
                ExibirMensagemErro($"A conta '{numeroConta}' já está cadastrada!");
                return;
            }

            string titular = LerNomeTitular();
            decimal saldoInicial = LerSaldoInicial();

            ContaBancaria novaConta = tipo == "POUPANÇA"
                ? new ContaPoupanca(numeroConta, titular, saldoInicial)
                : new ContaCorrente(numeroConta, titular, saldoInicial);

            gerenciador.AdicionarConta(novaConta);

            Console.WriteLine($"\n [SUCESSO] Conta {tipo} cadastrada com sucesso!");
            FinalizarOpcao();
        }

        private static void RealizarDeposito(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("REALIZAR DEPÓSITO");

            var conta = ObterContaOuExibirErro(gerenciador);
            if (conta == null) return;

            Console.WriteLine($" Titular: {conta.Titular}");
            decimal valor = LerDecimalPositivo("Valor do Depósito");

            if (valor > 0)
            {
                conta.Depositar(valor);
                Console.WriteLine($"\n Depósito realizado! Novo saldo: R$ {conta.Saldo:F2}");
            }

            FinalizarOpcao();
        }

        private static void RealizarSaque(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("REALIZAR SAQUE");

            var conta = ObterContaOuExibirErro(gerenciador);
            if (conta == null) return;

            Console.WriteLine($" Titular: {conta.Titular}\n Saldo Disponível: R$ {conta.Saldo:F2}");
            decimal valor = LerDecimalPositivo("Valor do Saque");

            if (valor > 0)
            {
                if (conta.Saldo >= valor)
                {
                    conta.Sacar(valor);
                    Console.WriteLine($"\n Saque realizado! Saldo atual: R$ {conta.Saldo:F2}");
                }
                else
                {
                    ExibirMensagemErro("Saldo insuficiente para o saque!", pausar: false);
                }
            }

            FinalizarOpcao();
        }

        private static void SimularRendimentoPoupanca(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("RENDIMENTO POUPANÇA");

            var conta = ObterContaOuExibirErro(gerenciador);
            if (conta == null) return;

            if (conta is ContaPoupanca poupanca)
            {
                Console.WriteLine($" Titular: {poupanca.Titular}\n Saldo Atual: R$ {poupanca.Saldo:F2}");
                Console.Write(" Meses em rendimento: ");

                if (int.TryParse(Console.ReadLine(), out int meses) && meses > 0)
                {
                    decimal saldoProjetado = poupanca.Saldo * (decimal)Math.Pow(1.005, meses);

                    DesenharLinha('-');
                    Console.WriteLine($" Taxa Mensal: 0,5% a.m.");
                    Console.WriteLine($" Rendimento Estimado: R$ {(saldoProjetado - poupanca.Saldo):F2}");
                    Console.WriteLine($" Saldo Final Projetado: R$ {saldoProjetado:F2}");
                }
                else
                {
                    ExibirMensagemErro("Quantidade de meses inválida!", pausar: false);
                }
            }
            else
            {
                ExibirMensagemErro("A conta informada não é uma Conta Poupança!", pausar: false);
            }

            FinalizarOpcao();
        }

        private static void ConsultarExtrato(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("EXTRATO / SALDO");

            var conta = ObterContaOuExibirErro(gerenciador);
            if (conta != null)
            {
                DesenharLinha('-');
                Console.WriteLine($" Titular: {conta.Titular}\n Número: {conta.NumeroConta}\n Saldo Atual: R$ {conta.Saldo:F2}");
            }

            FinalizarOpcao();
        }

        private static void ExibirRelatorioContasSeparadas(GerenciadorDeContas gerenciador)
        {
            Console.Clear();
            ExibirCabecalho("RELATÓRIO GERAL DE CONTAS");

            var todasContas = gerenciador.ObterTodas();
            if (todasContas == null || todasContas.Count == 0)
            {
                Console.WriteLine(" [AVISO] Nenhuma conta cadastrada.");
                FinalizarOpcao();
                return;
            }

            ExibirBlocoRelatorio("CONTAS CORRENTES", todasContas, c => c is ContaCorrente);
            ExibirBlocoRelatorio("CONTAS POUPANÇA", todasContas, c => c is ContaPoupanca);

            FinalizarOpcao();
        }

        private static string LerNumeroConta()
        {
            while (true)
            {
                Console.Write(" Número da Conta (somente números): ");
                string entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    ExibirMensagemErro("O número da conta não pode ser vazio!", pausar: false);
                    continue;
                }

                if (!entrada.All(char.IsDigit))
                {
                    ExibirMensagemErro("O número da conta deve conter APENAS números!", pausar: false);
                    continue;
                }

                return entrada;
            }
        }

        private static string LerNomeTitular()
        {
            while (true)
            {
                Console.Write(" Nome do Titular (somente letras): ");
                string entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    ExibirMensagemErro("O nome do titular não pode ser vazio!", pausar: false);
                    continue;
                }

                if (!entrada.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
                {
                    ExibirMensagemErro("O nome do titular deve conter APENAS letras!", pausar: false);
                    continue;
                }

                return entrada;
            }
        }

        private static decimal LerSaldoInicial()
        {
            while (true)
            {
                Console.Write(" Saldo Inicial: R$ ");
                string entrada = Console.ReadLine()?.Trim();

                if (string.IsNullOrWhiteSpace(entrada))
                {
                    ExibirMensagemErro("O saldo inicial não pode ser vazio!", pausar: false);
                    continue;
                }

                if (!decimal.TryParse(entrada, out decimal saldo) || saldo < 0)
                {
                    ExibirMensagemErro("Digite um valor numérico válido maior ou igual a zero!", pausar: false);
                    continue;
                }

                return saldo;
            }
        }

        private static decimal LerDecimalPositivo(string campo)
        {
            Console.Write($" {campo}: R$ ");
            if (decimal.TryParse(Console.ReadLine(), out decimal valor) && valor > 0)
                return valor;

            ExibirMensagemErro($"Valor de {campo.ToLower()} inválido!", pausar: false);
            return 0;
        }

        private static ContaBancaria ObterContaOuExibirErro(GerenciadorDeContas gerenciador)
        {
            string numero = LerNumeroConta();
            var conta = gerenciador.BuscarConta(numero);

            if (conta == null) ExibirMensagemErro("Conta não encontrada!", pausar: false);
            return conta;
        }

        public static void DesenharLinha(char caractere = '=') =>
            Console.WriteLine(new string(caractere, 40));

        public static void ExibirCabecalho(string titulo)
        {
            DesenharLinha('=');
            int espacos = Math.Max(0, (40 - titulo.Length) / 2);
            Console.WriteLine($"{new string(' ', espacos)}{titulo.ToUpper()}");
            DesenharLinha('=');
        }

        private static void ExibirBlocoRelatorio(string titulo, System.Collections.Generic.List<ContaBancaria> contas, Func<ContaBancaria, bool> filtro)
        {
            Console.WriteLine($" --- {titulo} ---");
            bool encontrou = false;

            foreach (var conta in contas)
            {
                if (filtro(conta))
                {
                    Console.WriteLine($" Cta: {conta.NumeroConta} | Titular: {conta.Titular}\n Saldo: R$ {conta.Saldo:F2}");
                    DesenharLinha('.');
                    encontrou = true;
                }
            }

            if (!encontrou)
            {
                Console.WriteLine($" Nenhuma {titulo.ToLower()} cadastrada.");
                DesenharLinha('.');
            }
            Console.WriteLine();
        }

        private static bool EncerrarSistema()
        {
            Console.Clear();
            ExibirCabecalho("SISTEMA ENCERRADO");
            Console.WriteLine(" Obrigado por utilizar o nosso sistema!");
            DesenharLinha('=');
            return false;
        }

        private static void ExibirMensagemErro(string msg, bool pausar = true)
        {
            Console.WriteLine($"\n [ERRO] {msg}");
            if (pausar) PressionarParaContinuar();
        }

        private static void FinalizarOpcao()
        {
            DesenharLinha('=');
            PressionarParaContinuar();
        }

        private static void PressionarParaContinuar()
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}