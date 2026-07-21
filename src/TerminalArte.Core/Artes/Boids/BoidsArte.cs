using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Boids;

public class BoidsArte : IArte
{
    public string Nome => "Boids";
    public string Descricao => "Simulação de bando de pássaros (flocking)";

    private record struct Boid(double X, double Y, double VX, double VY);

    private const int NumBoids = 40;
    private const double MaxSpeed = 1.2;
    private const double VisualRange = 15;

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var rng = new Random();

        Console.CursorVisible = false;
        Console.Clear();

        var boids = new Boid[NumBoids];
        var prev = new (int X, int Y)[NumBoids];

        for (int i = 0; i < NumBoids; i++)
        {
            boids[i] = new Boid(
                rng.Next(0, largura), rng.Next(0, altura),
                (rng.NextDouble() - 0.5) * 2, (rng.NextDouble() - 0.5) * 2);
            prev[i] = (-1, -1);
        }

        while (!ct.IsCancellationRequested)
        {
            for (int i = 0; i < NumBoids; i++)
            {
                // Apagar posição anterior
                if (prev[i].X >= 0 && prev[i].X < largura && prev[i].Y >= 0 && prev[i].Y < altura)
                {
                    Console.SetCursorPosition(prev[i].X, prev[i].Y);
                    Console.Write(' ');
                }

                var b = boids[i];

                // Regras de flocking
                double sepX = 0, sepY = 0; // Separation
                double aliVX = 0, aliVY = 0; int aliCount = 0; // Alignment
                double cohX = 0, cohY = 0; int cohCount = 0; // Cohesion

                for (int j = 0; j < NumBoids; j++)
                {
                    if (i == j) continue;
                    double dx = boids[j].X - b.X;
                    double dy = boids[j].Y - b.Y;
                    double dist = Math.Sqrt(dx * dx + dy * dy);

                    if (dist < 3) // Muito perto — separar
                    {
                        sepX -= dx;
                        sepY -= dy;
                    }

                    if (dist < VisualRange)
                    {
                        aliVX += boids[j].VX;
                        aliVY += boids[j].VY;
                        aliCount++;
                        cohX += boids[j].X;
                        cohY += boids[j].Y;
                        cohCount++;
                    }
                }

                double nvx = b.VX + sepX * 0.05;
                double nvy = b.VY + sepY * 0.05;

                if (aliCount > 0)
                {
                    nvx += (aliVX / aliCount - b.VX) * 0.03;
                    nvy += (aliVY / aliCount - b.VY) * 0.03;
                }
                if (cohCount > 0)
                {
                    nvx += (cohX / cohCount - b.X) * 0.005;
                    nvy += (cohY / cohCount - b.Y) * 0.005;
                }

                // Bordas — empurrar de volta
                if (b.X < 5) nvx += 0.3;
                if (b.X > largura - 5) nvx -= 0.3;
                if (b.Y < 3) nvy += 0.2;
                if (b.Y > altura - 3) nvy -= 0.2;

                // Limitar velocidade
                double speed = Math.Sqrt(nvx * nvx + nvy * nvy);
                if (speed > MaxSpeed)
                {
                    nvx = nvx / speed * MaxSpeed;
                    nvy = nvy / speed * MaxSpeed;
                }

                double nx = b.X + nvx;
                double ny = b.Y + nvy;

                boids[i] = new Boid(nx, ny, nvx, nvy);

                // Desenhar
                int sx = (int)nx, sy = (int)ny;
                if (sx >= 0 && sx < largura && sy >= 0 && sy < altura)
                {
                    Console.SetCursorPosition(sx, sy);

                    // Direção determina char
                    char c = (Math.Abs(nvx) > Math.Abs(nvy))
                        ? (nvx > 0 ? '>' : '<')
                        : (nvy > 0 ? 'v' : '^');

                    Console.ForegroundColor = speed > 0.8 ? ConsoleColor.White : ConsoleColor.Gray;
                    Console.Write(c);

                    prev[i] = (sx, sy);
                }
            }

            Thread.Sleep(40);
        }

        Console.ResetColor();
    }
}
