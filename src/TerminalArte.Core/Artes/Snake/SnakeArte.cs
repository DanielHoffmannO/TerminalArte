using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Snake;

public class SnakeArte : IArte
{
    public string Nome => "Snake";
    public string Descricao => "Jogo da cobrinha jogável (WASD ou setas)";

    private enum Direcao { Cima, Baixo, Esquerda, Direita }

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth - 1;
        int altura = Console.WindowHeight - 1;
        var rng = new Random();

        Console.CursorVisible = false;
        Console.Clear();

        // Estado do jogo
        var cobra = new LinkedList<(int X, int Y)>();
        cobra.AddFirst((largura / 2, altura / 2));

        var direcao = Direcao.Direita;
        var comida = GerarComida(cobra, largura, altura, rng);
        int pontos = 0;
        bool gameOver = false;

        DesenharBorda(largura, altura);
        DesenharComida(comida);

        while (!ct.IsCancellationRequested && !gameOver)
        {
            // Input não-bloqueante
            if (Console.KeyAvailable)
            {
                var tecla = Console.ReadKey(true).Key;
                direcao = tecla switch
                {
                    ConsoleKey.UpArrow or ConsoleKey.W when direcao != Direcao.Baixo => Direcao.Cima,
                    ConsoleKey.DownArrow or ConsoleKey.S when direcao != Direcao.Cima => Direcao.Baixo,
                    ConsoleKey.LeftArrow or ConsoleKey.A when direcao != Direcao.Direita => Direcao.Esquerda,
                    ConsoleKey.RightArrow or ConsoleKey.D when direcao != Direcao.Esquerda => Direcao.Direita,
                    _ => direcao
                };
            }

            // Mover cabeça
            var cabeca = cobra.First!.Value;
            var novaCabeca = direcao switch
            {
                Direcao.Cima => (cabeca.X, cabeca.Y - 1),
                Direcao.Baixo => (cabeca.X, cabeca.Y + 1),
                Direcao.Esquerda => (cabeca.X - 1, cabeca.Y),
                Direcao.Direita => (cabeca.X + 1, cabeca.Y),
                _ => cabeca
            };

            // Colisão com borda
            if (novaCabeca.Item1 <= 0 || novaCabeca.Item1 >= largura ||
                novaCabeca.Item2 <= 0 || novaCabeca.Item2 >= altura)
            {
                gameOver = true;
                break;
            }

            // Colisão consigo mesma
            if (cobra.Contains(novaCabeca))
            {
                gameOver = true;
                break;
            }

            cobra.AddFirst(novaCabeca);

            // Comeu a comida?
            if (novaCabeca == comida)
            {
                pontos += 10;
                comida = GerarComida(cobra, largura, altura, rng);
                DesenharComida(comida);
            }
            else
            {
                // Apagar cauda
                var cauda = cobra.Last!.Value;
                Console.SetCursorPosition(cauda.X, cauda.Y);
                Console.Write(' ');
                cobra.RemoveLast();
            }

            // Desenhar cabeça
            Console.SetCursorPosition(novaCabeca.Item1, novaCabeca.Item2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write('█');

            // Desenhar corpo (segundo segmento agora vira corpo)
            if (cobra.Count > 1)
            {
                var segundo = cobra.First!.Next!.Value;
                Console.SetCursorPosition(segundo.X, segundo.Y);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write('▓');
            }

            // Placar
            Console.SetCursorPosition(2, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" 🐍 Pontos: {pontos} ");

            Console.ResetColor();
            Thread.Sleep(80);
        }

        if (gameOver && !ct.IsCancellationRequested)
        {
            MostrarGameOver(largura, altura, pontos);
            // Esperar tecla para sair
            while (!ct.IsCancellationRequested && !Console.KeyAvailable)
                Thread.Sleep(100);
            if (Console.KeyAvailable) Console.ReadKey(true);
        }

        Console.ResetColor();
    }

    private static void DesenharBorda(int largura, int altura)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        for (int x = 0; x <= largura; x++)
        {
            Console.SetCursorPosition(x, 0);
            Console.Write('─');
            Console.SetCursorPosition(x, altura);
            Console.Write('─');
        }
        for (int y = 0; y <= altura; y++)
        {
            Console.SetCursorPosition(0, y);
            Console.Write('│');
            Console.SetCursorPosition(largura, y);
            Console.Write('│');
        }
        Console.SetCursorPosition(0, 0); Console.Write('┌');
        Console.SetCursorPosition(largura, 0); Console.Write('┐');
        Console.SetCursorPosition(0, altura); Console.Write('└');
        Console.SetCursorPosition(largura, altura); Console.Write('┘');
    }

    private static void DesenharComida((int X, int Y) pos)
    {
        Console.SetCursorPosition(pos.X, pos.Y);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write('●');
    }

    private static (int X, int Y) GerarComida(LinkedList<(int X, int Y)> cobra, int largura, int altura, Random rng)
    {
        (int X, int Y) pos;
        do
        {
            pos = (rng.Next(1, largura), rng.Next(1, altura));
        } while (cobra.Contains(pos));
        return pos;
    }

    private static void MostrarGameOver(int largura, int altura, int pontos)
    {
        int cx = largura / 2;
        int cy = altura / 2;

        Console.ForegroundColor = ConsoleColor.Red;
        string msg1 = "╔═══════════════════╗";
        string msg2 = "║   GAME OVER! 💀   ║";
        string msg3 = $"║   Pontos: {pontos,-6}  ║";
        string msg4 = "║ Tecle para voltar ║";
        string msg5 = "╚═══════════════════╝";

        int sx = cx - msg1.Length / 2;
        Console.SetCursorPosition(sx, cy - 2); Console.Write(msg1);
        Console.SetCursorPosition(sx, cy - 1); Console.Write(msg2);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.SetCursorPosition(sx, cy); Console.Write(msg3);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.SetCursorPosition(sx, cy + 1); Console.Write(msg4);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.SetCursorPosition(sx, cy + 2); Console.Write(msg5);
    }
}
