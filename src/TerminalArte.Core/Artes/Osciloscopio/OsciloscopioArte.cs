using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Osciloscopio;

public class OsciloscopioArte : IArte
{
    public string Nome => "Osciloscópio";
    public string Descricao => "Ondas senoidais animadas tipo oscilloscope";

    private static readonly ConsoleColor[] CoresOndas =
    [
        ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        double tempo = 0;

        Console.CursorVisible = false;
        Console.Clear();

        // Grade de fundo
        DesenharGrade(largura, altura);

        int numOndas = 3;
        var prevY = new int[numOndas][];
        for (int i = 0; i < numOndas; i++)
            prevY[i] = new int[largura];

        while (!ct.IsCancellationRequested)
        {
            for (int onda = 0; onda < numOndas; onda++)
            {
                double freq = 0.05 + onda * 0.03;
                double amp = (altura / 2.0 - 2) / (onda + 1.5);
                double fase = tempo * (1 + onda * 0.5);

                for (int x = 0; x < largura; x++)
                {
                    // Apagar posição anterior
                    int oldY = prevY[onda][x];
                    if (oldY > 0 && oldY < altura)
                    {
                        Console.SetCursorPosition(x, oldY);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        // Restaurar grade se necessário
                        bool isGridH = oldY == altura / 2;
                        bool isGridV = x % 10 == 0;
                        Console.Write(isGridH ? '─' : isGridV ? '│' : ' ');
                    }

                    // Calcular novo Y
                    double val = Math.Sin(x * freq + fase) * amp;
                    val += Math.Sin(x * freq * 2.1 + fase * 1.3) * amp * 0.3; // Harmônico
                    int ny = (int)(altura / 2.0 + val);
                    ny = Math.Clamp(ny, 0, altura - 1);

                    // Desenhar
                    Console.SetCursorPosition(x, ny);
                    Console.ForegroundColor = CoresOndas[onda];
                    Console.Write('─');

                    prevY[onda][x] = ny;
                }
            }

            // Info no canto
            Console.SetCursorPosition(1, 0);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($" CH1: {1 + Math.Sin(tempo):F2}V ");
            Console.SetCursorPosition(1, 1);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($" CH2: {0.5 + Math.Sin(tempo * 1.5):F2}V ");
            Console.SetCursorPosition(1, 2);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" CH3: {0.8 + Math.Sin(tempo * 0.7):F2}V ");

            tempo += 0.08;
            Thread.Sleep(30);
        }

        Console.ResetColor();
    }

    private static void DesenharGrade(int largura, int altura)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;

        // Linha central horizontal
        for (int x = 0; x < largura; x++)
        {
            Console.SetCursorPosition(x, altura / 2);
            Console.Write('─');
        }

        // Linhas verticais
        for (int x = 0; x < largura; x += 10)
        {
            for (int y = 0; y < altura; y++)
            {
                Console.SetCursorPosition(x, y);
                Console.Write('│');
            }
        }
    }
}
