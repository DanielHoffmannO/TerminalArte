using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Dna;

public class DnaArte : IArte
{
    public string Nome => "DNA Helix";
    public string Descricao => "Dupla hélice de DNA rotacionando";

    private static readonly char[] Bases = ['A', 'T', 'G', 'C'];
    private static readonly ConsoleColor[] CoresBases =
    [
        ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Yellow
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        double offset = 0;
        var rng = new Random();

        Console.CursorVisible = false;

        // Pré-gerar sequência de bases
        var sequencia = new int[altura];
        for (int i = 0; i < altura; i++)
            sequencia[i] = rng.Next(Bases.Length);

        double centroX = largura / 2.0;
        double raio = Math.Min(largura / 4.0, 20);

        while (!ct.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < altura; y++)
            {
                double angulo = y * 0.3 + offset;
                double x1 = centroX + Math.Sin(angulo) * raio;
                double x2 = centroX - Math.Sin(angulo) * raio;

                int ix1 = (int)x1;
                int ix2 = (int)x2;

                // Profundidade (qual strand está na frente)
                double z1 = Math.Cos(angulo);
                double z2 = -z1;

                // Limpar linha
                string linha = new string(' ', largura);
                char[] buf = linha.ToCharArray();

                // Conexão entre as strands (ponte de hidrogênio)
                int minX = Math.Min(ix1, ix2);
                int maxX = Math.Max(ix1, ix2);
                if (Math.Abs(z1) < 0.5) // Quando estão mais ou menos alinhados
                {
                    for (int x = minX + 1; x < maxX && x < largura && x >= 0; x++)
                        buf[x] = '─';
                }

                // Construir buffer de cores por posição
                Console.SetCursorPosition(0, y);

                for (int x = 0; x < largura; x++)
                {
                    if (x == ix1 && ix1 >= 0 && ix1 < largura)
                    {
                        Console.ForegroundColor = z1 > 0 ? CoresBases[sequencia[y]] : ConsoleColor.DarkGray;
                        Console.Write(z1 > 0 ? Bases[sequencia[y]] : '○');
                    }
                    else if (x == ix2 && ix2 >= 0 && ix2 < largura)
                    {
                        int par = sequencia[y] ^ 1; // Par complementar
                        Console.ForegroundColor = z2 > 0 ? CoresBases[par] : ConsoleColor.DarkGray;
                        Console.Write(z2 > 0 ? Bases[par] : '○');
                    }
                    else if (x > minX && x < maxX && Math.Abs(z1) < 0.4)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write('─');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }

            offset += 0.1;
            Thread.Sleep(50);
        }

        Console.ResetColor();
    }
}
