using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Plasma;

public class PlasmaArte : IArte
{
    public string Nome => "Plasma";
    public string Descricao => "Efeito de plasma colorido psicodélico";

    private static readonly ConsoleColor[] Paleta =
    [
        ConsoleColor.DarkBlue, ConsoleColor.Blue, ConsoleColor.DarkCyan,
        ConsoleColor.Cyan, ConsoleColor.DarkGreen, ConsoleColor.Green,
        ConsoleColor.Yellow, ConsoleColor.DarkYellow, ConsoleColor.Red,
        ConsoleColor.DarkRed, ConsoleColor.Magenta, ConsoleColor.DarkMagenta
    ];

    private static readonly char[] Chars = " ░▒▓█▓▒░".ToCharArray();

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        double tempo = 0;

        Console.CursorVisible = false;

        while (!ct.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < altura; y++)
            {
                for (int x = 0; x < largura; x++)
                {
                    double valor = PlasmaValor(x, y, tempo);

                    int corIndex = (int)((valor + 1) / 2 * (Paleta.Length - 1));
                    corIndex = Math.Clamp(corIndex, 0, Paleta.Length - 1);

                    int charIndex = (int)((valor + 1) / 2 * (Chars.Length - 1));
                    charIndex = Math.Clamp(charIndex, 0, Chars.Length - 1);

                    Console.ForegroundColor = Paleta[corIndex];
                    Console.Write(Chars[charIndex]);
                }
            }

            tempo += 0.08;
            Thread.Sleep(40);
        }

        Console.ResetColor();
    }

    private static double PlasmaValor(int x, int y, double t)
    {
        double v1 = Math.Sin(x * 0.08 + t);
        double v2 = Math.Sin(y * 0.12 + t * 0.7);
        double v3 = Math.Sin((x + y) * 0.06 + t * 0.5);
        double v4 = Math.Sin(Math.Sqrt(x * x + y * y) * 0.05 + t * 1.2);

        return (v1 + v2 + v3 + v4) / 4.0;
    }
}
