using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Dvd;

public class DvdArte : IArte
{
    public string Nome => "DVD Bounce";
    public string Descricao => "Logo DVD quicando na tela (muda cor no canto!)";

    private static readonly string[] Logo =
    [
        "╔═══════════════╗",
        "║  ▄▄▄  ▄   ▄  ║",
        "║  █  █ █   █  ║",
        "║  █  █  █ █   ║",
        "║  ▀▀▀    ▀    ║",
        "║   V I D E O  ║",
        "╚═══════════════╝"
    ];

    private static readonly ConsoleColor[] Cores =
    [
        ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue,
        ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.Cyan,
        ConsoleColor.White, ConsoleColor.DarkYellow
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        int logoW = Logo[0].Length;
        int logoH = Logo.Length;

        Console.CursorVisible = false;
        Console.Clear();

        double x = largura / 4.0, y = altura / 4.0;
        double vx = 1.0, vy = 0.6;
        int corIdx = 0;
        int cornerHits = 0;

        while (!ct.IsCancellationRequested)
        {
            // Apagar posição anterior
            for (int ly = 0; ly < logoH; ly++)
            {
                int drawY = (int)y + ly;
                if (drawY >= 0 && drawY < altura)
                {
                    Console.SetCursorPosition(Math.Max(0, (int)x), drawY);
                    Console.Write(new string(' ', logoW));
                }
            }

            // Mover
            x += vx;
            y += vy;

            bool hitX = false, hitY = false;

            // Colisão com bordas
            if (x <= 0 || x + logoW >= largura)
            {
                vx = -vx;
                x = Math.Clamp(x, 0, largura - logoW);
                hitX = true;
            }
            if (y <= 0 || y + logoH >= altura)
            {
                vy = -vy;
                y = Math.Clamp(y, 0, altura - logoH);
                hitY = true;
            }

            // Mudou de cor ao bater
            if (hitX || hitY)
            {
                corIdx = (corIdx + 1) % Cores.Length;

                // Corner hit! Efeito especial
                if (hitX && hitY)
                {
                    cornerHits++;
                    // Flash
                    Console.BackgroundColor = Cores[corIdx];
                    Console.Clear();
                    Thread.Sleep(50);
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                }
            }

            // Desenhar logo
            Console.ForegroundColor = Cores[corIdx];
            for (int ly = 0; ly < logoH; ly++)
            {
                int drawY = (int)y + ly;
                int drawX = (int)x;
                if (drawY >= 0 && drawY < altura && drawX >= 0 && drawX + logoW <= largura)
                {
                    Console.SetCursorPosition(drawX, drawY);
                    Console.Write(Logo[ly]);
                }
            }

            // Contador de corner hits
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($" Corner hits: {cornerHits} ");

            Console.ResetColor();
            Thread.Sleep(30);
        }

        Console.ResetColor();
    }
}
