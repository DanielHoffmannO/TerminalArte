using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Barras;

public class BarrasArte : IArte
{
    private readonly ISortingAlgorithm _algoritmo;
    private readonly int _quantidade;
    private readonly int _delayMs;

    public string Nome => $"Barras - {_algoritmo.Nome}";
    public string Descricao => $"Visualização de {_algoritmo.Nome} com {_quantidade} barras";

    public BarrasArte(ISortingAlgorithm algoritmo, int quantidade = 30, int delayMs = 30)
    {
        _algoritmo = algoritmo;
        _quantidade = quantidade;
        _delayMs = delayMs;
    }

    public void Executar(CancellationToken ct)
    {
        var random = new Random();

        while (!ct.IsCancellationRequested)
        {
            // Gerar array embaralhado (1 a N)
            var array = Enumerable.Range(1, _quantidade).OrderBy(_ => random.Next()).ToArray();
            Renderizar(array, $"{_algoritmo.Nome} — Pressione qualquer tecla para sair");

            Thread.Sleep(1000);
            if (ct.IsCancellationRequested) break;

            // Executar sorting com visualização
            foreach (var snapshot in _algoritmo.Ordenar(array))
            {
                if (ct.IsCancellationRequested) break;
                Renderizar(snapshot, $"{_algoritmo.Nome} — Ordenando...");
                Thread.Sleep(_delayMs);
            }

            if (ct.IsCancellationRequested) break;

            // Mostra resultado final
            var sorted = Enumerable.Range(1, _quantidade).ToArray();
            Renderizar(sorted, $"{_algoritmo.Nome} — Concluído! ✓");
            Thread.Sleep(2000);
        }
    }

    private void Renderizar(int[] array, string titulo)
    {
        Console.CursorVisible = false;
        Console.SetCursorPosition(0, 0);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  {titulo}");
        Console.WriteLine();
        Console.ResetColor();

        int max = _quantidade;
        for (int row = max; row >= 1; row--)
        {
            Console.Write("  ");
            for (int col = 0; col < array.Length; col++)
            {
                if (array[col] >= row)
                {
                    Console.ForegroundColor = CorPorAltura(array[col], max);
                    Console.Write("█ ");
                }
                else
                {
                    Console.Write("  ");
                }
            }
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    private static ConsoleColor CorPorAltura(int valor, int max)
    {
        var pct = (double)valor / max;
        return pct switch
        {
            < 0.25 => ConsoleColor.Blue,
            < 0.50 => ConsoleColor.Green,
            < 0.75 => ConsoleColor.Yellow,
            _ => ConsoleColor.Red
        };
    }
}
