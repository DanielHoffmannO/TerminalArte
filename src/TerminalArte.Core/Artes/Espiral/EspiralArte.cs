using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Espiral;

public class EspiralArte : IArte
{
    public string Nome => "Spirograph";
    public string Descricao => "Espirais animadas tipo spirograph hipnótico";

    private static readonly ConsoleColor[] Cores =
    [
        ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green,
        ConsoleColor.Cyan, ConsoleColor.Blue, ConsoleColor.Magenta
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        double cx = largura / 2.0;
        double cy = altura / 2.0;

        Console.CursorVisible = false;
        Console.Clear();

        // Parâmetros do spirograph: R = raio externo, r = raio interno, d = distância
        double R = Math.Min(largura / 3.0, altura / 1.5);
        double r = R * 0.38;
        double d = R * 0.65;

        double angulo = 0;
        double velocidade = 0.05;
        int corIndex = 0;
        int frame = 0;
        int trailLength = 200;

        var pontos = new List<(int X, int Y, ConsoleColor Cor)>();

        while (!ct.IsCancellationRequested)
        {
            // Calcular novo ponto no spirograph
            // Parametric: x = (R-r)*cos(t) + d*cos((R-r)/r * t)
            //             y = (R-r)*sin(t) - d*sin((R-r)/r * t)
            double t = angulo;
            double px = (R - r) * Math.Cos(t) + d * Math.Cos((R - r) / r * t);
            double py = (R - r) * Math.Sin(t) - d * Math.Sin((R - r) / r * t);

            // Escalar para aspecto do terminal (chars são ~2x mais altos que largos)
            int sx = (int)(cx + px);
            int sy = (int)(cy + py * 0.5);

            if (sx >= 0 && sx < largura && sy >= 0 && sy < altura)
            {
                var cor = Cores[corIndex % Cores.Length];
                pontos.Add((sx, sy, cor));

                Console.SetCursorPosition(sx, sy);
                Console.ForegroundColor = cor;
                Console.Write('●');
            }

            // Fade de pontos antigos
            if (pontos.Count > trailLength)
            {
                var velho = pontos[0];
                pontos.RemoveAt(0);
                if (velho.X >= 0 && velho.X < largura && velho.Y >= 0 && velho.Y < altura)
                {
                    Console.SetCursorPosition(velho.X, velho.Y);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write('·');
                }

                // Apagar os mais antigos
                if (pontos.Count > trailLength)
                {
                    var muitoVelho = pontos[0];
                    pontos.RemoveAt(0);
                    if (muitoVelho.X >= 0 && muitoVelho.X < largura && muitoVelho.Y >= 0 && muitoVelho.Y < altura)
                    {
                        Console.SetCursorPosition(muitoVelho.X, muitoVelho.Y);
                        Console.Write(' ');
                    }
                }
            }

            angulo += velocidade;
            frame++;

            // Mudar cor gradualmente
            if (frame % 60 == 0)
                corIndex++;

            // Mudar parâmetros sutilmente para variar o padrão
            if (frame % 500 == 0)
            {
                r = R * (0.25 + (Math.Sin(frame * 0.001) + 1) * 0.15);
                d = R * (0.5 + (Math.Cos(frame * 0.002) + 1) * 0.15);
                Console.Clear();
                pontos.Clear();
            }

            Thread.Sleep(10);
        }

        Console.ResetColor();
    }
}
