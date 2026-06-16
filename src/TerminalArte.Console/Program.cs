using TerminalArte.Core.Artes.Barras;
using TerminalArte.Core.Artes.Barras.Algorithms;
using TerminalArte.Core.Artes.Cubo3D;
using TerminalArte.Core.Artes.Fire;
using TerminalArte.Core.Artes.GameOfLife;
using TerminalArte.Core.Artes.Matrix;
using TerminalArte.Core.Artes.Pinball;
using TerminalArte.Core.Artes.Starfield;
using TerminalArte.Core.Interfaces;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Registrar todas as artes disponíveis
var artes = new List<IArte>
{
    new BarrasArte(new BubbleSort()),
    new BarrasArte(new SelectionSort()),
    new BarrasArte(new InsertionSort()),
    new BarrasArte(new QuickSort()),
    new MatrixArte(),
    new GameOfLifeArte(),
    new FireArte(),
    new PinballArte(),
    new Cubo3DArte(),
    new StarfieldArte(),
};

while (true)
{
    Console.Clear();
    Console.CursorVisible = true;

    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔══════════════════════════════════╗");
    Console.WriteLine("║        🎨 TERMINAL ARTE 🎨       ║");
    Console.WriteLine("║    Arte interativa no terminal   ║");
    Console.WriteLine("╚══════════════════════════════════╝");
    Console.ResetColor();
    Console.WriteLine();

    for (int i = 0; i < artes.Count; i++)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write($"  [{i + 1}] ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write(artes[i].Nome);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  — {artes[i].Descricao}");
    }

    Console.ResetColor();
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine("  [0] Sair");
    Console.ResetColor();
    Console.WriteLine();
    Console.Write("  Escolha: ");

    var input = Console.ReadLine()?.Trim();
    if (input == "0") break;

    if (int.TryParse(input, out int escolha) && escolha >= 1 && escolha <= artes.Count)
    {
        Console.Clear();
        using var cts = new CancellationTokenSource();

        // Executar arte em thread separada, cancelar ao pressionar tecla
        var task = Task.Run(() => artes[escolha - 1].Executar(cts.Token));
        Console.ReadKey(true);
        cts.Cancel();
        task.Wait();
    }
}

Console.Clear();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\n  Até mais! 👋\n");
Console.ResetColor();
