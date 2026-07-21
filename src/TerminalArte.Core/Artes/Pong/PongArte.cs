using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Pong;

public class PongArte : IArte
{
    public string Nome => "Pong";
    public string Descricao => "Pong clássico contra IA (W/S para mover)";

    private const int PaddleH = 5;

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth - 1;
        int altura = Console.WindowHeight - 1;

        Console.CursorVisible = false;
        Console.Clear();

        // Estado
        double ballX = largura / 2.0, ballY = altura / 2.0;
        double ballVX = 1.0, ballVY = 0.5;
        int playerY = altura / 2 - PaddleH / 2;
        int aiY = altura / 2 - PaddleH / 2;
        int scorePlayer = 0, scoreAi = 0;
        int prevBX = -1, prevBY = -1;

        while (!ct.IsCancellationRequested)
        {
            // Input do jogador
            if (Console.KeyAvailable)
            {
                var tecla = Console.ReadKey(true).Key;
                if ((tecla == ConsoleKey.W || tecla == ConsoleKey.UpArrow) && playerY > 1)
                    playerY--;
                if ((tecla == ConsoleKey.S || tecla == ConsoleKey.DownArrow) && playerY + PaddleH < altura - 1)
                    playerY++;
            }

            // IA simples — segue a bola com delay
            int aiCenter = aiY + PaddleH / 2;
            if (aiCenter < (int)ballY - 1 && aiY + PaddleH < altura - 1)
                aiY++;
            else if (aiCenter > (int)ballY + 1 && aiY > 1)
                aiY--;

            // Mover bola
            ballX += ballVX;
            ballY += ballVY;

            // Colisão topo/fundo
            if (ballY <= 1 || ballY >= altura - 1)
                ballVY = -ballVY;

            // Colisão paddle jogador (esquerda x=2)
            if (ballX <= 3 && (int)ballY >= playerY && (int)ballY < playerY + PaddleH)
            {
                ballVX = Math.Abs(ballVX) * 1.05;
                ballVY += ((int)ballY - playerY - PaddleH / 2.0) * 0.3;
            }

            // Colisão paddle IA (direita x=largura-3)
            if (ballX >= largura - 4 && (int)ballY >= aiY && (int)ballY < aiY + PaddleH)
            {
                ballVX = -Math.Abs(ballVX) * 1.05;
                ballVY += ((int)ballY - aiY - PaddleH / 2.0) * 0.3;
            }

            // Gol
            if (ballX <= 0)
            {
                scoreAi++;
                ResetBola(ref ballX, ref ballY, ref ballVX, ref ballVY, largura, altura);
            }
            else if (ballX >= largura)
            {
                scorePlayer++;
                ResetBola(ref ballX, ref ballY, ref ballVX, ref ballVY, largura, altura);
            }

            // Limitar velocidade
            ballVX = Math.Clamp(ballVX, -2.5, 2.5);
            ballVY = Math.Clamp(ballVY, -2.0, 2.0);

            // --- Renderizar ---

            // Apagar bola anterior
            if (prevBX >= 0 && prevBX < largura && prevBY >= 0 && prevBY <= altura)
            {
                Console.SetCursorPosition(prevBX, prevBY);
                Console.Write(' ');
            }

            // Linha central
            Console.ForegroundColor = ConsoleColor.DarkGray;
            for (int y = 0; y <= altura; y++)
            {
                Console.SetCursorPosition(largura / 2, y);
                Console.Write(y % 2 == 0 ? '│' : ' ');
            }

            // Paddles
            Console.ForegroundColor = ConsoleColor.Green;
            for (int y = 0; y <= altura; y++)
            {
                Console.SetCursorPosition(2, y);
                Console.Write(y >= playerY && y < playerY + PaddleH ? '█' : ' ');
            }
            Console.ForegroundColor = ConsoleColor.Red;
            for (int y = 0; y <= altura; y++)
            {
                Console.SetCursorPosition(largura - 3, y);
                Console.Write(y >= aiY && y < aiY + PaddleH ? '█' : ' ');
            }

            // Bola
            int bx = (int)ballX, by = (int)ballY;
            bx = Math.Clamp(bx, 0, largura - 1);
            by = Math.Clamp(by, 0, altura);
            Console.SetCursorPosition(bx, by);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write('●');
            prevBX = bx; prevBY = by;

            // Placar
            Console.SetCursorPosition(largura / 2 - 5, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" {scorePlayer} : {scoreAi} ");

            Console.ResetColor();
            Thread.Sleep(30);
        }

        Console.ResetColor();
    }

    private static void ResetBola(ref double x, ref double y, ref double vx, ref double vy, int largura, int altura)
    {
        x = largura / 2.0;
        y = altura / 2.0;
        vx = vx > 0 ? -1.0 : 1.0;
        vy = 0.5 * (new Random().Next(2) == 0 ? 1 : -1);
    }
}
