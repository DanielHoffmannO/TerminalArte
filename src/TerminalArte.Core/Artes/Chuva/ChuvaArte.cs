using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Chuva;

public class ChuvaArte : IArte
{
    public string Nome => "Chuva";
    public string Descricao => "Simulação de chuva com gotas e poças";

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var rng = new Random();

        Console.CursorVisible = false;
        Console.Clear();

        var gotas = new List<(int X, double Y, double Vel, char C)>();
        var splashes = new List<(int X, int Y, int Frame)>();

        while (!ct.IsCancellationRequested)
        {
            // Adicionar novas gotas
            for (int i = 0; i < 3; i++)
            {
                int x = rng.Next(0, largura);
                double vel = 0.5 + rng.NextDouble() * 1.5;
                char c = vel > 1.5 ? '|' : vel > 1.0 ? '¦' : ':';
                gotas.Add((x, 0, vel, c));
            }

            // Limpar e atualizar gotas
            for (int i = gotas.Count - 1; i >= 0; i--)
            {
                var g = gotas[i];

                // Apagar posição anterior
                int py = (int)g.Y;
                if (py >= 0 && py < altura && g.X >= 0 && g.X < largura)
                {
                    Console.SetCursorPosition(g.X, py);
                    Console.Write(' ');
                }

                // Mover
                double novoY = g.Y + g.Vel;

                if ((int)novoY >= altura - 1)
                {
                    // Splash!
                    splashes.Add((g.X, altura - 1, 0));
                    gotas.RemoveAt(i);
                }
                else
                {
                    gotas[i] = (g.X, novoY, g.Vel, g.C);

                    // Desenhar nova posição
                    int ny = (int)novoY;
                    if (ny >= 0 && ny < altura && g.X >= 0 && g.X < largura)
                    {
                        Console.SetCursorPosition(g.X, ny);
                        Console.ForegroundColor = g.Vel > 1.5 ? ConsoleColor.White : ConsoleColor.DarkCyan;
                        Console.Write(g.C);
                    }
                }
            }

            // Animar splashes
            for (int i = splashes.Count - 1; i >= 0; i--)
            {
                var s = splashes[i];

                // Apagar anterior
                if (s.X - 1 >= 0 && s.X - 1 < largura)
                {
                    Console.SetCursorPosition(s.X - 1, s.Y);
                    Console.Write(' ');
                }
                if (s.X + 1 < largura)
                {
                    Console.SetCursorPosition(s.X + 1, s.Y);
                    Console.Write(' ');
                }
                Console.SetCursorPosition(s.X, s.Y);
                Console.Write(' ');

                splashes[i] = (s.X, s.Y, s.Frame + 1);

                if (s.Frame >= 3)
                {
                    splashes.RemoveAt(i);
                    continue;
                }

                // Desenhar splash
                Console.ForegroundColor = ConsoleColor.Cyan;
                switch (s.Frame)
                {
                    case 0:
                        Console.SetCursorPosition(s.X, s.Y);
                        Console.Write('*');
                        break;
                    case 1:
                        if (s.X - 1 >= 0) { Console.SetCursorPosition(s.X - 1, s.Y); Console.Write('~'); }
                        Console.SetCursorPosition(s.X, s.Y); Console.Write('·');
                        if (s.X + 1 < largura) { Console.SetCursorPosition(s.X + 1, s.Y); Console.Write('~'); }
                        break;
                    case 2:
                        if (s.X - 1 >= 0) { Console.SetCursorPosition(s.X - 1, s.Y); Console.Write('·'); }
                        if (s.X + 1 < largura) { Console.SetCursorPosition(s.X + 1, s.Y); Console.Write('·'); }
                        break;
                }
            }

            // Chão/poças
            Console.SetCursorPosition(0, altura - 1);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            for (int x = 0; x < largura; x++)
                Console.Write(rng.Next(10) < 2 ? '~' : '≈');

            Console.ResetColor();
            Thread.Sleep(40);

            // Limitar gotas na tela
            if (gotas.Count > 200)
                gotas.RemoveRange(0, 50);
        }

        Console.ResetColor();
    }
}
