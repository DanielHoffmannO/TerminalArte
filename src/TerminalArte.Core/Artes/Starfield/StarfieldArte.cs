using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Starfield;

public class StarfieldArte : IArte
{
    public string Nome => "Starfield";
    public string Descricao => "Campo de estrelas 3D (screensaver clássico)";

    private const int NumStars = 150;
    private const double Speed = 0.04;

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var rng = new Random();
        Console.CursorVisible = false;

        var stars = new (double X, double Y, double Z)[NumStars];
        var prev = new (int X, int Y)[NumStars];

        for (int i = 0; i < NumStars; i++)
        {
            stars[i] = NewStar(rng);
            prev[i] = (-1, -1);
        }

        while (!ct.IsCancellationRequested)
        {
            for (int i = 0; i < NumStars; i++)
            {
                // Apagar posição anterior
                if (prev[i].X >= 0 && prev[i].X < largura && prev[i].Y >= 0 && prev[i].Y < altura)
                {
                    Console.SetCursorPosition(prev[i].X, prev[i].Y);
                    Console.Write(' ');
                }

                // Mover estrela
                var s = stars[i];
                s.Z -= Speed;

                if (s.Z <= 0.01)
                {
                    s = NewStar(rng);
                    prev[i] = (-1, -1);
                    stars[i] = s;
                    continue;
                }

                stars[i] = s;

                // Projetar em 2D
                int sx = (int)(s.X / s.Z * (largura / 4.0) + largura / 2.0);
                int sy = (int)(s.Y / s.Z * (altura / 4.0) + altura / 2.0);

                if (sx < 0 || sx >= largura || sy < 0 || sy >= altura)
                {
                    stars[i] = NewStar(rng);
                    prev[i] = (-1, -1);
                    continue;
                }

                // Brilho por distância
                char c;
                ConsoleColor cor;
                if (s.Z < 0.3) { c = '@'; cor = ConsoleColor.White; }
                else if (s.Z < 0.6) { c = '*'; cor = ConsoleColor.Gray; }
                else { c = '.'; cor = ConsoleColor.DarkGray; }

                Console.SetCursorPosition(sx, sy);
                Console.ForegroundColor = cor;
                Console.Write(c);

                prev[i] = (sx, sy);
            }

            Thread.Sleep(30);
        }

        Console.ResetColor();
    }

    private static (double X, double Y, double Z) NewStar(Random rng) =>
        ((rng.NextDouble() - 0.5) * 2, (rng.NextDouble() - 0.5) * 2, rng.NextDouble() * 0.8 + 0.2);
}
