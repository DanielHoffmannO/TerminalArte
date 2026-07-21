using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Fractal;

public class FractalArte : IArte
{
    public string Nome => "Árvore Fractal";
    public string Descricao => "Árvore fractal crescendo com o vento";

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;

        Console.CursorVisible = false;

        var rng = new Random();
        double vento = 0;
        int profMax = 1;

        while (!ct.IsCancellationRequested)
        {
            Console.Clear();
            var buffer = new char[altura, largura];
            var cores = new ConsoleColor[altura, largura];

            // Preencher com espaço
            for (int y = 0; y < altura; y++)
                for (int x = 0; x < largura; x++)
                    buffer[y, x] = ' ';

            // Chão
            for (int x = 0; x < largura; x++)
            {
                buffer[altura - 1, x] = '▓';
                cores[altura - 1, x] = ConsoleColor.DarkYellow;
            }

            // Desenhar árvore recursivamente
            int trunkX = largura / 2;
            int trunkY = altura - 2;
            DesenharRamo(buffer, cores, trunkX, trunkY, -Math.PI / 2, altura / 4.0, profMax, 0, vento, rng);

            // Renderizar buffer
            for (int y = 0; y < altura; y++)
            {
                Console.SetCursorPosition(0, y);
                ConsoleColor corAtual = ConsoleColor.White;
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

            // Crescer gradualmente
            if (profMax < 9)
            {
                profMax++;
                Thread.Sleep(600);
            }
            else
            {
                // Vento muda
                vento = Math.Sin(DateTime.Now.Ticks * 0.0000001) * 0.3;
                Thread.Sleep(300);

                // Resetar de vez em quando
                if (rng.Next(20) == 0)
                    profMax = 1;
            }
        }

        Console.ResetColor();
    }

    private static void DesenharRamo(char[,] buf, ConsoleColor[,] cores, int x, int y,
        double angulo, double comprimento, int profMax, int profAtual, double vento, Random rng)
    {
        if (profAtual >= profMax || comprimento < 1) return;

        int altura = buf.GetLength(0);
        int largura = buf.GetLength(1);

        double anguloAjustado = angulo + vento * (profAtual * 0.3);

        int endX = x + (int)(Math.Cos(anguloAjustado) * comprimento);
        int endY = y + (int)(Math.Sin(anguloAjustado) * comprimento);

        // Desenhar linha (Bresenham simplificado)
        int steps = (int)Math.Max(Math.Abs(endX - x), Math.Abs(endY - y));
        if (steps == 0) steps = 1;

        for (int i = 0; i <= steps; i++)
        {
            int px = x + (endX - x) * i / steps;
            int py = y + (endY - y) * i / steps;

            if (px >= 0 && px < largura && py >= 0 && py < altura)
            {
                if (profAtual < 3)
                {
                    buf[py, px] = '║';
                    cores[py, px] = ConsoleColor.DarkYellow;
                }
                else if (profAtual < 5)
                {
                    buf[py, px] = '│';
                    cores[py, px] = ConsoleColor.DarkYellow;
                }
                else
                {
                    buf[py, px] = '♣';
                    cores[py, px] = ConsoleColor.Green;
                }
            }
        }

        // Folhas nos ramos finais
        if (profAtual >= profMax - 2 && endX >= 0 && endX < largura && endY >= 0 && endY < altura)
        {
            buf[endY, endX] = '✿';
            cores[endY, endX] = rng.Next(3) == 0 ? ConsoleColor.Magenta : ConsoleColor.Green;
        }

        // Bifurcar
        double novoComp = comprimento * 0.7;
        double spread = 0.4 + rng.NextDouble() * 0.2;

        DesenharRamo(buf, cores, endX, endY, anguloAjustado - spread, novoComp, profMax, profAtual + 1, vento, rng);
        DesenharRamo(buf, cores, endX, endY, anguloAjustado + spread, novoComp, profMax, profAtual + 1, vento, rng);

        // Terceiro ramo ocasional
        if (rng.Next(3) == 0)
            DesenharRamo(buf, cores, endX, endY, anguloAjustado, novoComp * 0.8, profMax, profAtual + 1, vento, rng);
    }
}
