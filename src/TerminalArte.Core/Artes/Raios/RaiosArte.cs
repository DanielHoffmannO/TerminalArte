using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Raios;

public class RaiosArte : IArte
{
    public string Nome => "Raios";
    public string Descricao => "Relâmpagos procedurais no céu noturno";

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var rng = new Random();

        Console.CursorVisible = false;
        Console.Clear();

        // Céu escuro
        Console.BackgroundColor = ConsoleColor.Black;

        while (!ct.IsCancellationRequested)
        {
            // Desenhar nuvens no topo
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int y = 0; y < 3; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < largura; x++)
                    Console.Write(rng.Next(3) == 0 ? '░' : '▒');
            }

            // Gerar raio
            int startX = rng.Next(5, largura - 5);
            var raio = GerarRaio(startX, 3, altura - 2, rng);

            // Flash! Clarear o fundo
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            for (int y = 0; y < altura; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write(new string(' ', largura));
            }
            Thread.Sleep(30);

            // Voltar ao escuro e desenhar raio
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            // Redesenhar nuvens
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int y = 0; y < 3; y++)
            {
                Console.SetCursorPosition(0, y);
                for (int x = 0; x < largura; x++)
                    Console.Write(rng.Next(3) == 0 ? '░' : '▒');
            }

            // Desenhar raio brilhante
            foreach (var (x, y) in raio)
            {
                if (x >= 0 && x < largura && y >= 0 && y < altura)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write('│');
                }
            }
            Thread.Sleep(80);

            // Raio com brilho menor
            foreach (var (x, y) in raio)
            {
                if (x >= 0 && x < largura && y >= 0 && y < altura)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write('╎');
                }
            }
            Thread.Sleep(100);

            // Raio desaparecendo
            foreach (var (x, y) in raio)
            {
                if (x >= 0 && x < largura && y >= 0 && y < altura)
                {
                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write('¦');
                }
            }
            Thread.Sleep(120);

            // Apagar raio
            foreach (var (x, y) in raio)
            {
                if (x >= 0 && x < largura && y >= 0 && y < altura)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(' ');
                }
            }

            // Pausa entre raios (variável)
            Thread.Sleep(rng.Next(400, 2000));
        }

        Console.ResetColor();
    }

    private static List<(int X, int Y)> GerarRaio(int startX, int startY, int maxY, Random rng)
    {
        var pontos = new List<(int X, int Y)>();
        int x = startX;

        for (int y = startY; y <= maxY; y++)
        {
            pontos.Add((x, y));

            // Ramificação aleatória
            x += rng.Next(-2, 3);

            // Ramificação lateral (sub-raios)
            if (rng.Next(6) == 0)
            {
                int branchX = x;
                int branchDir = rng.Next(2) == 0 ? -1 : 1;
                for (int b = 0; b < rng.Next(2, 6); b++)
                {
                    branchX += branchDir;
                    int branchY = y + b;
                    if (branchY <= maxY)
                        pontos.Add((branchX, branchY));
                }
            }
        }

        return pontos;
    }
}
