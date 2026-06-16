using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Pinball;

public class PinballArte : IArte
{
    public string Nome => "Pinball Rain";
    public string Descricao => "Gotas caindo e quicando em pinos";

    private record struct Gota(double X, double Y, double VelX, double VelY);

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth - 1;
        int altura = Console.WindowHeight - 1;
        var random = new Random();
        var gotas = new List<Gota>();
        var pinos = GerarPinos(largura, altura);

        Console.CursorVisible = false;

        while (!ct.IsCancellationRequested)
        {
            // Spawnar gotas
            if (random.Next(100) < 40)
                gotas.Add(new Gota(random.Next(2, largura - 2), 0, random.NextDouble() * 0.6 - 0.3, 0.5));

            // Limpar e desenhar
            Console.SetCursorPosition(0, 0);
            var tela = new char[altura, largura];

            // Desenhar pinos
            foreach (var (px, py) in pinos)
            {
                if (px < largura && py < altura)
                    tela[py, px] = '●';
            }

            // Atualizar gotas
            for (int i = gotas.Count - 1; i >= 0; i--)
            {
                var g = gotas[i];
                // Gravidade
                g = g with { VelY = g.VelY + 0.3, X = g.X + g.VelX, Y = g.Y + g.VelY };

                // Colisão com pinos
                foreach (var (px, py) in pinos)
                {
                    double dx = g.X - px, dy = g.Y - py;
                    if (dx * dx + dy * dy < 1.5)
                    {
                        g = g with { VelY = -g.VelY * 0.4, VelX = g.VelX + (dx > 0 ? 0.8 : -0.8), Y = g.Y - 1 };
                        break;
                    }
                }

                // Paredes
                if (g.X < 0) g = g with { X = 0, VelX = -g.VelX * 0.5 };
                if (g.X >= largura - 1) g = g with { X = largura - 2, VelX = -g.VelX * 0.5 };

                // Remover se saiu por baixo
                if (g.Y >= altura)
                {
                    gotas.RemoveAt(i);
                    continue;
                }

                gotas[i] = g;

                int gx = (int)g.X, gy = (int)g.Y;
                if (gx >= 0 && gx < largura && gy >= 0 && gy < altura)
                    tela[gy, gx] = '○';
            }

            // Limitar gotas
            if (gotas.Count > 60)
                gotas.RemoveRange(0, gotas.Count - 60);

            // Render
            for (int r = 0; r < altura; r++)
            {
                Console.SetCursorPosition(0, r);
                for (int c = 0; c < largura; c++)
                {
                    char ch = tela[r, c];
                    if (ch == '●')
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write('●');
                    }
                    else if (ch == '○')
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write('○');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }

            Console.ResetColor();
            Thread.Sleep(60);
        }
    }

    private static List<(int X, int Y)> GerarPinos(int largura, int altura)
    {
        var pinos = new List<(int, int)>();
        int startY = altura / 4;
        int spacing = 6;

        for (int row = 0; row < 6; row++)
        {
            int y = startY + row * (altura / 8);
            int offset = row % 2 == 0 ? 0 : spacing / 2;
            for (int x = offset + spacing; x < largura - spacing; x += spacing)
                pinos.Add((x, y));
        }

        return pinos;
    }
}
