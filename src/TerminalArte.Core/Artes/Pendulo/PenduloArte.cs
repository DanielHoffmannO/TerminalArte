using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Pendulo;

public class PenduloArte : IArte
{
    public string Nome => "Pêndulo";
    public string Descricao => "Pêndulo duplo caótico com rastro";

    private static readonly ConsoleColor[] CoresTrail =
    [
        ConsoleColor.White, ConsoleColor.Yellow, ConsoleColor.DarkYellow,
        ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.DarkGray
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;

        Console.CursorVisible = false;
        Console.Clear();

        // Parâmetros do pêndulo duplo
        double a1 = Math.PI / 2, a2 = Math.PI / 1.5;
        double v1 = 0, v2 = 0;
        double m1 = 10, m2 = 10;
        double l1 = Math.Min(largura / 6.0, altura / 3.0);
        double l2 = l1 * 0.8;
        double g = 0.5;

        double cx = largura / 2.0;
        double cy = altura / 3.0;

        var trail = new Queue<(int X, int Y)>();
        int maxTrail = 150;

        while (!ct.IsCancellationRequested)
        {
            // Física do pêndulo duplo (Euler simplificado)
            double num1 = -g * (2 * m1 + m2) * Math.Sin(a1);
            double num2 = -m2 * g * Math.Sin(a1 - 2 * a2);
            double num3 = -2 * Math.Sin(a1 - a2) * m2;
            double num4 = v2 * v2 * l2 + v1 * v1 * l1 * Math.Cos(a1 - a2);
            double den = l1 * (2 * m1 + m2 - m2 * Math.Cos(2 * a1 - 2 * a2));
            double acc1 = (num1 + num2 + num3 * num4) / den;

            num1 = 2 * Math.Sin(a1 - a2);
            num2 = v1 * v1 * l1 * (m1 + m2);
            num3 = g * (m1 + m2) * Math.Cos(a1);
            num4 = v2 * v2 * l2 * m2 * Math.Cos(a1 - a2);
            den = l2 * (2 * m1 + m2 - m2 * Math.Cos(2 * a1 - 2 * a2));
            double acc2 = (num1 * (num2 + num3 + num4)) / den;

            v1 += acc1 * 0.1;
            v2 += acc2 * 0.1;
            a1 += v1 * 0.1;
            a2 += v2 * 0.1;

            // Posições
            int x1 = (int)(cx + l1 * Math.Sin(a1));
            int y1 = (int)(cy + l1 * Math.Cos(a1) * 0.5); // *0.5 para aspecto
            int x2 = (int)(x1 + l2 * Math.Sin(a2));
            int y2 = (int)(y1 + l2 * Math.Cos(a2) * 0.5);

            // Apagar trail mais antigo
            if (trail.Count >= maxTrail)
            {
                var old = trail.Dequeue();
                if (old.X >= 0 && old.X < largura && old.Y >= 0 && old.Y < altura)
                {
                    Console.SetCursorPosition(old.X, old.Y);
                    Console.Write(' ');
                }
            }

            // Adicionar ao trail
            if (x2 >= 0 && x2 < largura && y2 >= 0 && y2 < altura)
                trail.Enqueue((x2, y2));

            // Desenhar trail com fade
            int idx = 0;
            foreach (var (tx, ty) in trail)
            {
                if (tx >= 0 && tx < largura && ty >= 0 && ty < altura)
                {
                    Console.SetCursorPosition(tx, ty);
                    double ratio = (double)idx / trail.Count;
                    int corIdx = (int)((1 - ratio) * (CoresTrail.Length - 1));
                    Console.ForegroundColor = CoresTrail[Math.Clamp(corIdx, 0, CoresTrail.Length - 1)];
                    Console.Write(ratio > 0.7 ? '●' : ratio > 0.4 ? '◦' : '·');
                }
                idx++;
            }

            // Desenhar hastes
            Console.ForegroundColor = ConsoleColor.DarkGray;
            int icx = (int)cx, icy = (int)cy;
            if (icx >= 0 && icx < largura && icy >= 0 && icy < altura)
            {
                Console.SetCursorPosition(icx, icy);
                Console.Write('◆');
            }

            // Bobs
            if (x1 >= 0 && x1 < largura && y1 >= 0 && y1 < altura)
            {
                Console.SetCursorPosition(x1, y1);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write('●');
            }
            if (x2 >= 0 && x2 < largura && y2 >= 0 && y2 < altura)
            {
                Console.SetCursorPosition(x2, y2);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write('●');
            }

            Thread.Sleep(20);
        }

        Console.ResetColor();
    }
}
