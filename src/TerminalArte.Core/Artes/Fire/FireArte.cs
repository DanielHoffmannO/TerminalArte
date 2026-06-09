using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Fire;

public class FireArte : IArte
{
    public string Nome => "Fire Effect";
    public string Descricao => "Simulação de fogo no terminal";

    private static readonly char[] Chars = " .,:;+*?%S#@".ToCharArray();
    private static readonly ConsoleColor[] Cores =
    [
        ConsoleColor.Black, ConsoleColor.DarkRed, ConsoleColor.DarkRed,
        ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.DarkYellow,
        ConsoleColor.DarkYellow, ConsoleColor.Yellow, ConsoleColor.Yellow,
        ConsoleColor.White, ConsoleColor.White, ConsoleColor.White
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var buffer = new int[altura, largura];
        var random = new Random();

        Console.CursorVisible = false;

        while (!ct.IsCancellationRequested)
        {
            // Base do fogo (última linha) — valores aleatórios altos
            for (int c = 0; c < largura; c++)
                buffer[altura - 1, c] = random.Next(2) == 0 ? Chars.Length - 1 : 0;

            // Propagar fogo para cima
            for (int r = 0; r < altura - 1; r++)
                for (int c = 0; c < largura; c++)
                {
                    int decay = random.Next(3);
                    int src = (c + random.Next(-1, 2) + largura) % largura;
                    buffer[r, c] = Math.Max(0, buffer[r + 1, src] - decay);
                }

            // Renderizar
            Console.SetCursorPosition(0, 0);
            for (int r = 0; r < altura; r++)
            {
                for (int c = 0; c < largura; c++)
                {
                    int val = buffer[r, c];
                    Console.ForegroundColor = Cores[val];
                    Console.Write(Chars[val]);
                }
            }

            Console.ResetColor();
            Thread.Sleep(40);
        }
    }
}
