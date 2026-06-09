using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Barras.Algorithms;

public class SelectionSort : ISortingAlgorithm
{
    public string Nome => "Selection Sort";

    public IEnumerable<int[]> Ordenar(int[] array)
    {
        var arr = (int[])array.Clone();
        for (int i = 0; i < arr.Length - 1; i++)
        {
            int min = i;
            for (int j = i + 1; j < arr.Length; j++)
                if (arr[j] < arr[min]) min = j;

            if (min != i)
            {
                (arr[i], arr[min]) = (arr[min], arr[i]);
                yield return (int[])arr.Clone();
            }
        }
    }
}
