namespace SistemaBancario.Interfaces
{
    public interface IConta
    {
        int Id { get; }
        string NumeroConta { get; }
        string Titular { get; }
        decimal Saldo { get; }
        bool Ativa { get; }

        void Depositar(decimal valor);
        void Sacar(decimal valor);
        void ExibirDetalhes();
    }
}