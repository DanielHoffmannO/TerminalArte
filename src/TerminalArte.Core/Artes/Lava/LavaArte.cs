using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Lava;

public class LavaArte : IArte
{
    public string Nome => "Lava Lamp";
    public string Descricao => "Lava lamp retrô com bolhas subindo e descendo";

    private record struct Bolha(double X, double Y, double Raio, double VY, ConsoleColor Cor);

    private static readonly ConsoleColor[] CoresBolha =
    [
        ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Yellow,
        ConsoleColor.DarkYellow, ConsoleColor.Magenta, ConsoleColor.DarkMagenta
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var rng = new Random();

        Console.CursorVisible = false;

        // Criar bolhas iniciais
        int numBolhas = 8;
        var bolhas = new List<Bolha>();
        for (int i = 0; i < numBolhas; i++)
            bolhas.Add(NovaBolha(rng, largura, altura));

        double tempo = 0;

        while (!ct.IsCancellationRequested)
        {
            // Buffer para renderizar
            var buffer = new char[altura, largura];
            var cores = new ConsoleColor[altura, largura];

            // Fundo — gradiente escuro
            for (int y = 0; y < altura; y++)
            {
                double gradiente = (double)y / altura;
                for (int x = 0; x < largura; x++)
                {
                    buffer[y, x] = gradiente > 0.9 ? '▓' : gradiente > 0.7 ? '░' : ' ';
                    cores[y, x] = ConsoleColor.DarkBlue;
                }
            }

            // Moldura do "vidro"
            int margemX = largura / 4;
            for (int y = 0; y < altura; y++)
            {
                if (margemX - 1 >= 0)
                {
                    buffer[y, margemX - 1] = '│';
                    cores[y, margemX - 1] = ConsoleColor.DarkGray;
                }
                if (largura - margemX < largura)
                {
                    buffer[y, largura - margemX] = '│';
                    cores[y, largura - margemX] = ConsoleColor.DarkGray;
                }
            }

            // Atualizar bolhas
            for (int i = 0; i < bolhas.Count; i++)
            {
                var b = bolhas[i];

                // Movimento ondulante
                double novoY = b.Y + b.VY;
                double novoX = b.X + Math.Sin(tempo + i * 1.5) * 0.3;

                // Bolha chega ao topo — desacelera e volta
                if (novoY < 2)
                {
                    bolhas[i] = b with { VY = Math.Abs(b.VY) * 0.5 + 0.05, Y = 2 };
                }
                // Bolha chega ao fundo — esquenta e sobe
                else if (novoY > altura - 3)
                {
                    bolhas[i] = b with { VY = -(0.2 + rng.NextDouble() * 0.4), Y = altura - 3 };
                }
                else
                {
                    // Gravidade leve para baixo + impulso térmico
                    double nvY = b.VY;
                    if (b.Y > altura * 0.7) // Zona quente — sobe
                        nvY -= 0.015;
                    else if (b.Y < altura * 0.3) // Zona fria — desce
                        nvY += 0.01;

                    // Manter X dentro da moldura
                    novoX = Math.Clamp(novoX, margemX + 1, largura - margemX - 2);

                    bolhas[i] = new Bolha(novoX, novoY, b.Raio, nvY, b.Cor);
                }

                // Desenhar bolha no buffer (círculo preenchido)
                var bolhaAtual = bolhas[i];
                int bx = (int)bolhaAtual.X;
                int by = (int)bolhaAtual.Y;
                int raio = (int)bolhaAtual.Raio;

                for (int dy = -raio; dy <= raio; dy++)
                {
                    for (int dx = -raio * 2; dx <= raio * 2; dx++) // *2 para compensar aspecto
                    {
                        double distancia = Math.Sqrt((dx / 2.0) * (dx / 2.0) + dy * dy);
                        if (distancia <= raio)
                        {
                            int px = bx + dx;
                            int py = by + dy;
                            if (px > margemX && px < largura - margemX && py >= 0 && py < altura)
                            {
                                if (distancia < raio * 0.5)
                                {
                                    buffer[py, px] = '█';
                                    cores[py, px] = bolhaAtual.Cor;
                                }
                                else if (distancia < raio * 0.8)
                                {
                                    buffer[py, px] = '▓';
                                    cores[py, px] = bolhaAtual.Cor;
                                }
                                else
                                {
                                    buffer[py, px] = '░';
                                    cores[py, px] = bolhaAtual.Cor;
                                }
                            }
                        }
                    }
                }
            }

            // Renderizar
            Console.SetCursorPosition(0, 0);
            ConsoleColor corAtual = ConsoleColor.Black;
            for (int y = 0; y < altura; y++)
            {
                for (int x = 0; x < largura; x++)
                {
                    if (cores[y, x] != corAtual)
                    {
                        corAtual = cores[y, x];
                        Console.ForegroundColor = corAtual;
                    }
                    Console.Write(buffer[y, x]);
                }
            }

            tempo += 0.06;
            Thread.Sleep(50);
        }

        Console.ResetColor();
    }

    private static Bolha NovaBolha(Random rng, int largura, int altura)
    {
        int margemX = largura / 4;
        return new Bolha(
            rng.Next(margemX + 3, largura - margemX - 3),
            rng.Next(altura / 3, altura - 5),
            2 + rng.Next(2),
            (rng.NextDouble() - 0.5) * 0.3,
            CoresBolha[rng.Next(CoresBolha.Length)]
        );
    }
}
