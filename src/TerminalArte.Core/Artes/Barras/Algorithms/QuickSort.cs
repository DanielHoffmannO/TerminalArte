using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Barras.Algorithms;

public class QuickSort : ISortingAlgorithm
{
    public string Nome => "Quick Sort";

    public IEnumerable<int[]> Ordenar(int[] array)
    {
        var arr = (int[])array.Clone();
        var steps = new List<int[]>();
        Sort(arr, 0, arr.Length - 1, steps);
        return steps;
    }

    private void Sort(int[] arr, int low, int high, List<int[]> steps)
    {
        if (low >= high) return;
        int pivot = arr[high], i = low - 1;

        for (int j = low; j < high; j++)
            if (arr[j] < pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
                steps.Add((int[])arr.Clone());
            }

        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        steps.Add((int[])arr.Clone());

        int pi = i + 1;
        Sort(arr, low, pi - 1, steps);
        Sort(arr, pi + 1, high, steps);
    }
}
