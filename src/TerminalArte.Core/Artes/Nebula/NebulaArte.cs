using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Nebula;

public class NebulaArte : IArte
{
    public string Nome => "Nebulosa";
    public string Descricao => "Nebulosa espacial colorida com estrelas";

    private static readonly char[] Densidade = " .·:+*#%@".ToCharArray();
    private static readonly ConsoleColor[] CoresNebula =
    [
        ConsoleColor.DarkBlue, ConsoleColor.DarkMagenta, ConsoleColor.Magenta,
        ConsoleColor.DarkCyan, ConsoleColor.Blue, ConsoleColor.DarkRed,
        ConsoleColor.Red, ConsoleColor.DarkYellow
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        double tempo = 0;
        var rng = new Random();

        Console.CursorVisible = false;

        // Pré-gerar estrelas fixas
        var estrelas = new List<(int X, int Y, char C)>();
        for (int i = 0; i < (largura * altura) / 50; i++)
            estrelas.Add((rng.Next(largura), rng.Next(altura), rng.Next(3) == 0 ? '*' : '.'));

        while (!ct.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < altura; y++)
            {
                for (int x = 0; x < largura; x++)
                {
                    // Noise simplificado (múltiplas camadas de seno)
                    double n1 = Math.Sin(x * 0.03 + tempo * 0.2) * Math.Cos(y * 0.05 + tempo * 0.15);
                    double n2 = Math.Sin((x + y) * 0.02 - tempo * 0.1) * 0.5;
                    double n3 = Math.Cos(x * 0.05 - y * 0.03 + tempo * 0.3) * 0.3;
                    double n4 = Math.Sin(Math.Sqrt((x - largura / 2.0) * (x - largura / 2.0) +
                                (y - altura / 2.0) * (y - altura / 2.0)) * 0.04 + tempo * 0.1) * 0.4;

                    double valor = (n1 + n2 + n3 + n4) / 2.0;
                    valor = (valor + 1) / 2.0; // Normalizar 0-1

                    int charIdx = (int)(valor * (Densidade.Length - 1));
                    charIdx = Math.Clamp(charIdx, 0, Densidade.Length - 1);

                    int corIdx = (int)(valor * (CoresNebula.Length - 1) + tempo * 0.5) % CoresNebula.Length;

                    Console.ForegroundColor = CoresNebula[corIdx];
                    Console.Write(Densidade[charIdx]);
                }
            }

            // Estrelas por cima (cintilando)
            foreach (var (sx, sy, sc) in estrelas)
            {
                if (rng.Next(3) == 0) continue; // Cintilação
                if (sx < largura && sy < altura)
                {
                    Console.SetCursorPosition(sx, sy);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(sc);
                }
            }

            tempo += 0.04;
            Thread.Sleep(50);
        }

        Console.ResetColor();
    }
}
