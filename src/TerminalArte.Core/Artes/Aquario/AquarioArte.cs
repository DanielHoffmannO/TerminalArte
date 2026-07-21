using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Aquario;

public class AquarioArte : IArte
{
    public string Nome => "Aquário";
    public string Descricao => "Peixes ASCII nadando em um aquário";

    private static readonly string[][] PeixesDir =
    [
        ["><>"],
        ["><))°>"],
        [">°)))°>"],
        ["═><≈>"],
    ];

    private static readonly string[][] PeixesEsq =
    [
        ["<><"],
        ["<°((<>"],
        ["<°(((<°"],
        ["<≈><═"],
    ];

    private static readonly ConsoleColor[] CoresPeixes =
    [
        ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Red,
        ConsoleColor.Magenta, ConsoleColor.Green, ConsoleColor.White
    ];

    private record Peixe(double X, double Y, double Vel, int Tipo, ConsoleColor Cor, bool DirDireita);

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var rng = new Random();

        Console.CursorVisible = false;
        Console.Clear();

        // Criar peixes
        int numPeixes = Math.Min(20, (largura * altura) / 100);
        var peixes = new List<Peixe>();
        for (int i = 0; i < numPeixes; i++)
            peixes.Add(NovoPeixe(rng, largura, altura));

        // Plantas no fundo
        var plantas = new List<(int X, int Altura)>();
        for (int i = 0; i < largura / 8; i++)
            plantas.Add((rng.Next(0, largura), rng.Next(3, 7)));

        // Bolhas
        var bolhas = new List<(double X, double Y, double Vel)>();

        while (!ct.IsCancellationRequested)
        {
            // Fundo — areia
            Console.SetCursorPosition(0, altura - 1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(new string('~', largura));

            // Plantas
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            foreach (var (px, ph) in plantas)
            {
                for (int y = 0; y < ph && altura - 2 - y >= 0; y++)
                {
                    Console.SetCursorPosition(px, altura - 2 - y);
                    Console.Write(y % 2 == 0 ? '|' : '/');
                }
            }

            // Apagar peixes antigos e mover
            foreach (var p in peixes)
            {
                int px = (int)p.X;
                var sprite = p.DirDireita ? PeixesDir[p.Tipo][0] : PeixesEsq[p.Tipo][0];
                if (px >= 0 && px + sprite.Length < largura && (int)p.Y >= 0 && (int)p.Y < altura - 1)
                {
                    Console.SetCursorPosition(px, (int)p.Y);
                    Console.Write(new string(' ', sprite.Length));
                }
            }

            // Atualizar posições
            for (int i = 0; i < peixes.Count; i++)
            {
                var p = peixes[i];
                double novoX = p.X + (p.DirDireita ? p.Vel : -p.Vel);
                double novoY = p.Y + Math.Sin(novoX * 0.1) * 0.3;

                if (novoX > largura + 5 || novoX < -10)
                    peixes[i] = NovoPeixe(rng, largura, altura);
                else
                    peixes[i] = p with { X = novoX, Y = Math.Clamp(novoY, 1, altura - 3) };
            }

            // Desenhar peixes
            foreach (var p in peixes)
            {
                int px = (int)p.X;
                var sprite = p.DirDireita ? PeixesDir[p.Tipo][0] : PeixesEsq[p.Tipo][0];
                if (px >= 0 && px + sprite.Length < largura && (int)p.Y >= 0 && (int)p.Y < altura - 1)
                {
                    Console.SetCursorPosition(px, (int)p.Y);
                    Console.ForegroundColor = p.Cor;
                    Console.Write(sprite);
                }
            }

            // Bolhas
            if (rng.Next(5) == 0)
                bolhas.Add((rng.Next(0, largura), altura - 2, 0.3 + rng.NextDouble() * 0.5));

            for (int i = bolhas.Count - 1; i >= 0; i--)
            {
                var b = bolhas[i];
                int bx = (int)b.X, by = (int)b.Y;
                if (by >= 0 && by < altura && bx >= 0 && bx < largura)
                {
                    Console.SetCursorPosition(bx, by);
                    Console.Write(' ');
                }
                var novaB = (b.X + Math.Sin(b.Y * 0.5) * 0.3, b.Y - b.Vel, b.Vel);
                if (novaB.Item2 < 0) { bolhas.RemoveAt(i); continue; }
                bolhas[i] = novaB;
                bx = (int)novaB.Item1; by = (int)novaB.Item2;
                if (by >= 0 && by < altura && bx >= 0 && bx < largura)
                {
                    Console.SetCursorPosition(bx, by);
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write('°');
                }
            }

            Console.ResetColor();
            Thread.Sleep(60);
        }
    }

    private static Peixe NovoPeixe(Random rng, int largura, int altura)
    {
        bool dir = rng.Next(2) == 0;
        double x = dir ? -8 : largura + 5;
        return new Peixe(x, rng.Next(2, altura - 4), 0.3 + rng.NextDouble() * 0.7,
            rng.Next(PeixesDir.Length), CoresPeixes[rng.Next(CoresPeixes.Length)], dir);
    }
}
