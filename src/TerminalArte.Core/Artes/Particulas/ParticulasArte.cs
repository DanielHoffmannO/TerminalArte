using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Particulas;

public class ParticulasArte : IArte
{
    public string Nome => "Partículas";
    public string Descricao => "Sistema de partículas com gravidade e explosões";

    private record struct Particula(double X, double Y, double VX, double VY, int Vida, ConsoleColor Cor);

    private static readonly ConsoleColor[] Cores =
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
        double centroX = largura / 2.0;
        double centroY = altura / 2.0;
        double tempo = 0;
        int emissorFrame = 0;

        while (!ct.IsCancellationRequested)
        {
            // Emissor se move em círculo
            tempo += 0.05;
            double emX = centroX + Math.Cos(tempo) * (largura / 4.0);
            double emY = centroY + Math.Sin(tempo * 0.7) * (altura / 4.0);

            // Emitir novas partículas
            emissorFrame++;
            if (emissorFrame % 2 == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    double angulo = rng.NextDouble() * Math.PI * 2;
                    double vel = 0.3 + rng.NextDouble() * 1.2;
                    particulas.Add(new Particula(
                        emX, emY,
                        Math.Cos(angulo) * vel,
                        Math.Sin(angulo) * vel * 0.5,
                        rng.Next(20, 50),
                        Cores[rng.Next(Cores.Length)]
                    ));
                }
            }

            // Explosão periódica
            if (emissorFrame % 80 == 0)
            {
                var corExplosao = Cores[rng.Next(Cores.Length)];
                double expX = rng.Next(5, largura - 5);
                double expY = rng.Next(3, altura - 3);
                for (int i = 0; i < 25; i++)
                {
                    double angulo = (Math.PI * 2 / 25) * i;
                    double vel = 0.8 + rng.NextDouble() * 1.5;
                    particulas.Add(new Particula(
                        expX, expY,
                        Math.Cos(angulo) * vel,
                        Math.Sin(angulo) * vel * 0.5,
                        rng.Next(15, 35),
                        corExplosao
                    ));
                }
            }

            // Atualizar partículas
            for (int i = particulas.Count - 1; i >= 0; i--)
            {
                var p = particulas[i];

                // Apagar posição anterior
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

                // Gravidade leve
                double nvx = p.VX * 0.98;
                double nvy = p.VY + 0.02;

                // Atração fraca ao centro
                double dx = centroX - p.X;
                double dy = centroY - p.Y;
                double dist = Math.Sqrt(dx * dx + dy * dy);
                if (dist > 1)
                {
                    nvx += dx / dist * 0.01;
                    nvy += dy / dist * 0.005;
                }

                var novaP = new Particula(
                    p.X + nvx, p.Y + nvy,
                    nvx, nvy,
                    p.Vida - 1, p.Cor
                );
                particulas[i] = novaP;

                // Desenhar
                px = (int)novaP.X; py = (int)novaP.Y;
                if (px >= 0 && px < largura && py >= 0 && py < altura)
                {
                    Console.SetCursorPosition(px, py);
                    Console.ForegroundColor = novaP.Vida > 30 ? novaP.Cor :
                                              novaP.Vida > 15 ? ConsoleColor.DarkGray :
                                              ConsoleColor.DarkGray;
                    char c = novaP.Vida > 30 ? '●' : novaP.Vida > 15 ? '◦' : '.';
                    Console.Write(c);
                }
            }

            // Limitar
            if (particulas.Count > 500)
                particulas.RemoveRange(0, 100);

            Console.ResetColor();
            Thread.Sleep(25);
        }

        Console.ResetColor();
    }
}
