using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Ondas;

public class OndasArte : IArte
{
    public string Nome => "Ondas";
    public string Descricao => "Ondas oceânicas ASCII com gradiente";

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;
        double tempo = 0;

        Console.CursorVisible = false;

        while (!ct.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, 0);

            int horizonte = altura / 3;

            // Céu
            for (int y = 0; y < horizonte; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.ForegroundColor = y < horizonte / 2 ? ConsoleColor.DarkBlue : ConsoleColor.Blue;
                Console.Write(new string(' ', largura));
            }

            // Sol/Lua
            int solX = largura / 2 + (int)(Math.Sin(tempo * 0.1) * 10);
            int solY = horizonte / 2;
            if (solX >= 0 && solX < largura - 2 && solY >= 0)
            {
                Console.SetCursorPosition(solX, solY);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("☀");
            }

            // Ondas
            for (int y = horizonte; y < altura; y++)
            {
                double profundidade = (double)(y - horizonte) / (altura - horizonte);

                for (int x = 0; x < largura; x++)
                {
                    double wave1 = Math.Sin(x * 0.05 + tempo + y * 0.1) * (1 - profundidade);
                    double wave2 = Math.Sin(x * 0.08 - tempo * 0.7 + y * 0.05) * 0.5;
                    double wave3 = Math.Sin(x * 0.12 + tempo * 1.3) * 0.3 * (1 - profundidade);
                    double val = wave1 + wave2 + wave3;

                    char c;
                    ConsoleColor cor;

                    if (val > 1.2)
                    {
                        c = '▓'; cor = ConsoleColor.White;
                    }
                    else if (val > 0.6)
                    {
                        c = '~'; cor = ConsoleColor.Cyan;
                    }
                    else if (val > 0.0)
                    {
                        c = '≈'; cor = ConsoleColor.DarkCyan;
                    }
                    else if (val > -0.6)
                    {
                        c = '~'; cor = ConsoleColor.Blue;
                    }
                    else
                    {
                        c = '≈'; cor = ConsoleColor.DarkBlue;
                    }

                    Console.SetCursorPosition(x, y);
                    Console.ForegroundColor = cor;
                    Console.Write(c);
                }
            }

            tempo += 0.08;
            Thread.Sleep(50);
        }

        Console.ResetColor();
    }
}
