using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Barras.Algorithms;

public class InsertionSort : ISortingAlgorithm
{
    public string Nome => "Insertion Sort";

    public IEnumerable<int[]> Ordenar(int[] array)
    {
        var arr = (int[])array.Clone();
        for (int i = 1; i < arr.Length; i++)
        {
            int key = arr[i], j = i - 1;
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j--;
                yield return (int[])arr.Clone();
            }
            arr[j + 1] = key;
            yield return (int[])arr.Clone();
        }
    }
}
