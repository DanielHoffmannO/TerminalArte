namespace TerminalArte.Core.Interfaces;

public interface ISortingAlgorithm
{
    string Nome { get; }
    IEnumerable<int[]> Ordenar(int[] array);
}
