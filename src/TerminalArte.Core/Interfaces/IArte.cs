namespace TerminalArte.Core.Interfaces;

public interface IArte
{
    string Nome { get; }
    string Descricao { get; }
    void Executar(CancellationToken ct);
}
