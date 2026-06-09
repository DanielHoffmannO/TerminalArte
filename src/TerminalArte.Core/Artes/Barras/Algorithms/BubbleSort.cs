using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Barras.Algorithms;

public class BubbleSort : ISortingAlgorithm
{
    public string Nome => "Bubble Sort";

    public IEnumerable<int[]> Ordenar(int[] array)
    {
        var arr = (int[])array.Clone();
        for (int i = 0; i < arr.Length - 1; i++)
            for (int j = 0; j < arr.Length - 1 - i; j++)
                if (arr[j] > arr[j + 1])
                {
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                    yield return (int[])arr.Clone();
                }
    }
}
