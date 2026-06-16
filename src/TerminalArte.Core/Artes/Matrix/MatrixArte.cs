using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Matrix;

public class MatrixArte : IArte
{
    public string Nome => "Matrix Rain";
    public string Descricao => "Chuva de caracteres estilo Matrix";

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        var random = new Random();
        var drops = new int[largura];

        for (int i = 0; i < drops.Length; i++)
            drops[i] = random.Next(-altura, 0);

        Console.CursorVisible = false;

        while (!ct.IsCancellationRequested)
        {
            for (int col = 0; col < largura; col++)
            {
                int row = drops[col];

                if (row >= 0 && row < altura)
                {
                    Console.SetCursorPosition(col, row);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write((char)random.Next(0x30, 0x7A));

                    if (row > 0)
                    {
                        Console.SetCursorPosition(col, row - 1);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write((char)random.Next(0x30, 0x7A));
                    }

                    int tail = row - random.Next(8, 20);
                    if (tail >= 0 && tail < altura)
                    {
                        Console.SetCursorPosition(col, tail);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write(' ');
                    }
                }

                drops[col]++;
                if (drops[col] > altura + random.Next(10, 30))
                    drops[col] = random.Next(-10, 0);
            }

            Console.ResetColor();
            Thread.Sleep(50);
        }
    }
}
