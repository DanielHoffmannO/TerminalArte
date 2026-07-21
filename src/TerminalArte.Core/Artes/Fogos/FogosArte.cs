using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Fogos;

public class FogosArte : IArte
{
    public string Nome => "Fogos de Artifício";
    public string Descricao => "Fogos de artifício explodindo no céu";

    private record Particula(double X, double Y, double VX, double VY, ConsoleColor Cor, int Vida);
    private record Foguete(double X, double Y, double VY, int AlvoY);

    private static readonly ConsoleColor[] CoresFogos =
    [
        ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green,
        ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.White
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var rng = new Random();

        Console.CursorVisible = false;
        Console.Clear();

        var particulas = new List<Particula>();
        var foguetes = new List<Foguete>();
        int frameCount = 0;

        while (!ct.IsCancellationRequested)
        {
            // Lançar novo foguete aleatoriamente
            if (rng.Next(15) == 0)
            {
                int x = rng.Next(5, largura - 5);
                int alvoY = rng.Next(3, altura / 3);
                foguetes.Add(new Foguete(x, altura - 1, -1.5 - rng.NextDouble(), alvoY));
            }

            // Limpar tela (fade)
            if (frameCount % 2 == 0)
            {
                Console.SetCursorPosition(0, 0);
                Console.ForegroundColor = ConsoleColor.Black;
            }

            // Atualizar foguetes
            for (int i = foguetes.Count - 1; i >= 0; i--)
            {
                var f = foguetes[i];

                // Apagar posição anterior
                int fx = (int)f.X, fy = (int)f.Y;
                if (fx >= 0 && fx < largura && fy >= 0 && fy < altura)
                {
                    Console.SetCursorPosition(fx, fy);
                    Console.Write(' ');
                }

                // Mover
                var novoF = f with { Y = f.Y + f.VY };

                if ((int)novoF.Y <= f.AlvoY)
                {
                    // Explodir!
                    var cor = CoresFogos[rng.Next(CoresFogos.Length)];
                    int numPart = rng.Next(15, 35);
                    for (int p = 0; p < numPart; p++)
                    {
                        double angulo = (2 * Math.PI / numPart) * p;
                        double vel = 0.5 + rng.NextDouble() * 1.5;
                        particulas.Add(new Particula(
                            f.X, novoF.Y,
                            Math.Cos(angulo) * vel,
                            Math.Sin(angulo) * vel * 0.5,
                            cor, rng.Next(10, 20)));
                    }
                    foguetes.RemoveAt(i);
                }
                else
                {
                    foguetes[i] = novoF;
                    fx = (int)novoF.X; fy = (int)novoF.Y;
                    if (fx >= 0 && fx < largura && fy >= 0 && fy < altura)
                    {
                        Console.SetCursorPosition(fx, fy);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write('│');
                    }
                }
            }

            // Atualizar partículas
            for (int i = particulas.Count - 1; i >= 0; i--)
            {
                var p = particulas[i];

                // Apagar
                int px = (int)p.X, py = (int)p.Y;
                if (px >= 0 && px < largura && py >= 0 && py < altura)
                {
                    Console.SetCursorPosition(px, py);
                    Console.Write(' ');
                }

                if (p.Vida <= 0)
                {
                    particulas.RemoveAt(i);
                    continue;
                }

                // Mover com gravidade
                var novoP = p with
                {
                    X = p.X + p.VX,
                    Y = p.Y + p.VY,
                    VY = p.VY + 0.05, // gravidade
                    VX = p.VX * 0.98, // fricção
                    Vida = p.Vida - 1
                };
                particulas[i] = novoP;

                // Desenhar
                px = (int)novoP.X; py = (int)novoP.Y;
                if (px >= 0 && px < largura && py >= 0 && py < altura)
                {
                    Console.SetCursorPosition(px, py);
                    Console.ForegroundColor = novoP.Vida > 5 ? novoP.Cor : ConsoleColor.DarkGray;
                    Console.Write(novoP.Vida > 10 ? '*' : novoP.Vida > 5 ? '·' : '.');
                }
            }

            frameCount++;
            Console.ResetColor();
            Thread.Sleep(40);
        }

        Console.ResetColor();
    }
}
