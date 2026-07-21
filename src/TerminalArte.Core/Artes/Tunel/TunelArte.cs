using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Tunel;

public class TunelArte : IArte
{
    public string Nome => "Túnel";
    public string Descricao => "Efeito de túnel infinito (warp speed)";

    private static readonly char[] Densidade = " ·∙░▒▓█".ToCharArray();
    private static readonly ConsoleColor[] CoresTunel =
    [
        ConsoleColor.DarkBlue, ConsoleColor.Blue, ConsoleColor.DarkCyan,
        ConsoleColor.Cyan, ConsoleColor.White
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        double tempo = 0;

        Console.CursorVisible = false;

        double cx = largura / 2.0;
        double cy = altura / 2.0;

        while (!ct.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, 0);

            for (int y = 0; y < altura; y++)
            {
                for (int x = 0; x < largura; x++)
                {
                    // Coordenadas relativas ao centro
                    double dx = (x - cx) / (largura / 2.0);
                    double dy = (y - cy) / (altura / 2.0) * 2; // Compensar aspecto

                    // Coordenadas polares
                    double dist = Math.Sqrt(dx * dx + dy * dy);
                    double angle = Math.Atan2(dy, dx);

                    if (dist < 0.01) dist = 0.01;

                    // Efeito túnel — profundidade inversamente proporcional à distância
                    double z = 1.0 / dist + tempo;
                    double u = angle / Math.PI;

                    // Padrão de textura
                    double textura = Math.Sin(z * 3) * Math.Cos(u * 8 + tempo * 0.5);

                    // Brilho diminui com a distância
                    double brilho = (1.0 - Math.Min(dist, 1.0)) * 0.7 + textura * 0.3;
                    brilho = Math.Clamp(brilho, 0, 1);

                    int charIdx = (int)(brilho * (Densidade.Length - 1));
                    charIdx = Math.Clamp(charIdx, 0, Densidade.Length - 1);

                    int corIdx = (int)(brilho * (CoresTunel.Length - 1));
                    corIdx = Math.Clamp(corIdx, 0, CoresTunel.Length - 1);

                    Console.ForegroundColor = CoresTunel[corIdx];
                    Console.Write(Densidade[charIdx]);
                }
            }

            tempo += 0.1;
            Thread.Sleep(40);
        }

        Console.ResetColor();
    }
}
