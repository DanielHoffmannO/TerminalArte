using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Cubo3D;

public class Cubo3DArte : IArte
{
    public string Nome => "Cubo 3D";
    public string Descricao => "Cubo wireframe rotacionando em 3D";

    // Vértices do cubo
    private static readonly double[][] Vertices =
    [
        [-1, -1, -1], [1, -1, -1], [1, 1, -1], [-1, 1, -1],
        [-1, -1,  1], [1, -1,  1], [1, 1,  1], [-1, 1,  1]
    ];

    // Arestas (pares de índices de vértices)
    private static readonly int[][] Arestas =
    [
        [0,1],[1,2],[2,3],[3,0], // face traseira
        [4,5],[5,6],[6,7],[7,4], // face frontal
        [0,4],[1,5],[2,6],[3,7]  // conectores
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight - 1;
        double angX = 0, angY = 0, angZ = 0;

        Console.CursorVisible = false;

        while (!ct.IsCancellationRequested)
        {
            var tela = new char[altura, largura];
            for (int r = 0; r < altura; r++)
                for (int c = 0; c < largura; c++)
                    tela[r, c] = ' ';

            // Projetar vértices
            var proj = new (int X, int Y)[8];
            for (int i = 0; i < 8; i++)
            {
                var (x, y, z) = Rotacionar(Vertices[i][0], Vertices[i][1], Vertices[i][2], angX, angY, angZ);
                double dist = 4.0;
                double scale = dist / (dist + z);
                proj[i] = ((int)(x * scale * altura / 2.5 * 2 + largura / 2),
                           (int)(y * scale * altura / 2.5 + altura / 2));
            }

            // Desenhar arestas
            foreach (var aresta in Arestas)
                DesenharLinha(tela, proj[aresta[0]], proj[aresta[1]], largura, altura);

            // Render
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("  Cubo 3D — Rotação");
            Console.ResetColor();

            for (int r = 0; r < altura; r++)
            {
                Console.SetCursorPosition(0, r);
                for (int c = 0; c < largura; c++)
                {
                    if (tela[r, c] != ' ')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(tela[r, c]);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
            }

            Console.ResetColor();
            angX += 0.03;
            angY += 0.05;
            angZ += 0.02;
            Thread.Sleep(50);
        }
    }

    private static (double X, double Y, double Z) Rotacionar(double x, double y, double z, double ax, double ay, double az)
    {
        // Rotação X
        double cosA = Math.Cos(ax), sinA = Math.Sin(ax);
        double y1 = y * cosA - z * sinA, z1 = y * sinA + z * cosA;

        // Rotação Y
        double cosB = Math.Cos(ay), sinB = Math.Sin(ay);
        double x2 = x * cosB + z1 * sinB, z2 = -x * sinB + z1 * cosB;

        // Rotação Z
        double cosC = Math.Cos(az), sinC = Math.Sin(az);
        double x3 = x2 * cosC - y1 * sinC, y3 = x2 * sinC + y1 * cosC;

        return (x3, y3, z2);
    }

    private static void DesenharLinha(char[,] tela, (int X, int Y) a, (int X, int Y) b, int largura, int altura)
    {
        int dx = Math.Abs(b.X - a.X), dy = Math.Abs(b.Y - a.Y);
        int steps = Math.Max(dx, dy);
        if (steps == 0) return;

        double xInc = (double)(b.X - a.X) / steps;
        double yInc = (double)(b.Y - a.Y) / steps;
        double x = a.X, y = a.Y;

        for (int i = 0; i <= steps; i++)
        {
            int px = (int)x, py = (int)y;
            if (px >= 0 && px < largura && py >= 0 && py < altura)
                tela[py, px] = '█';
            x += xInc;
            y += yInc;
        }
    }
}
