using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Tetris;

public class TetrisArte : IArte
{
    public string Nome => "Tetris";
    public string Descricao => "Jogo Tetris jogável (WASD/setas, espaço=drop)";

    private const int BoardW = 10;
    private const int BoardH = 20;

    private static readonly int[][][] Pecas =
    [
        [[ 1,1,1,1 ]], // I
        [[ 1,1 ], [ 1,1 ]], // O
        [[ 0,1,0 ], [ 1,1,1 ]], // T
        [[ 1,0,0 ], [ 1,1,1 ]], // L
        [[ 0,0,1 ], [ 1,1,1 ]], // J
        [[ 0,1,1 ], [ 1,1,0 ]], // S
        [[ 1,1,0 ], [ 0,1,1 ]], // Z
    ];

    private static readonly ConsoleColor[] CoresPecas =
    [
        ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Magenta,
        ConsoleColor.DarkYellow, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Red
    ];

    public void Executar(CancellationToken ct)
    {
        var board = new int[BoardH, BoardW];
        var cores = new ConsoleColor[BoardH, BoardW];
        var rng = new Random();
        int pontos = 0, linhas = 0;
        bool gameOver = false;

        int pecaIdx = rng.Next(Pecas.Length);
        var peca = CopiarPeca(Pecas[pecaIdx]);
        var cor = CoresPecas[pecaIdx];
        int px = BoardW / 2 - peca[0].Length / 2, py = 0;

        int offsetX, offsetY;
        CalcularOffset(out offsetX, out offsetY);

        Console.CursorVisible = false;
        Console.Clear();
        DesenharMoldura(offsetX, offsetY);

        int tickCount = 0;
        int velocidade = 15; // Frames para queda automática

        while (!ct.IsCancellationRequested && !gameOver)
        {
            if (Console.KeyAvailable)
            {
                var tecla = Console.ReadKey(true).Key;
                switch (tecla)
                {
                    case ConsoleKey.LeftArrow or ConsoleKey.A:
                        if (Cabe(board, peca, px - 1, py)) px--;
                        break;
                    case ConsoleKey.RightArrow or ConsoleKey.D:
                        if (Cabe(board, peca, px + 1, py)) px++;
                        break;
                    case ConsoleKey.DownArrow or ConsoleKey.S:
                        if (Cabe(board, peca, px, py + 1)) py++;
                        break;
                    case ConsoleKey.UpArrow or ConsoleKey.W:
                        var rotacionada = Rotacionar(peca);
                        if (Cabe(board, rotacionada, px, py)) peca = rotacionada;
                        break;
                    case ConsoleKey.Spacebar:
                        while (Cabe(board, peca, px, py + 1)) py++;
                        break;
                }
            }

            tickCount++;
            if (tickCount >= velocidade)
            {
                tickCount = 0;
                if (Cabe(board, peca, px, py + 1))
                {
                    py++;
                }
                else
                {
                    // Fixar peça
                    Fixar(board, cores, peca, px, py, cor);
                    int cleared = LimparLinhas(board, cores);
                    linhas += cleared;
                    pontos += cleared * cleared * 100;
                    velocidade = Math.Max(3, 15 - linhas / 5);

                    // Nova peça
                    pecaIdx = rng.Next(Pecas.Length);
                    peca = CopiarPeca(Pecas[pecaIdx]);
                    cor = CoresPecas[pecaIdx];
                    px = BoardW / 2 - peca[0].Length / 2;
                    py = 0;

                    if (!Cabe(board, peca, px, py))
                        gameOver = true;
                }
            }

            // Renderizar
            DesenharBoard(board, cores, peca, px, py, cor, offsetX, offsetY);
            DesenharInfo(offsetX, offsetY, pontos, linhas);

            Thread.Sleep(30);
        }

        if (gameOver && !ct.IsCancellationRequested)
        {
            Console.SetCursorPosition(offsetX + 2, offsetY + BoardH / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(" GAME OVER! ");
            Console.SetCursorPosition(offsetX + 2, offsetY + BoardH / 2 + 1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" Score: {pontos} ");
            while (!ct.IsCancellationRequested && !Console.KeyAvailable) Thread.Sleep(100);
            if (Console.KeyAvailable) Console.ReadKey(true);
        }

        Console.ResetColor();
    }

    private static void CalcularOffset(out int x, out int y)
    {
        x = (Console.WindowWidth - BoardW * 2 - 2) / 2;
        y = (Console.WindowHeight - BoardH - 2) / 2;
        x = Math.Max(0, x);
        y = Math.Max(0, y);
    }

    private static void DesenharMoldura(int ox, int oy)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        for (int y = 0; y <= BoardH + 1; y++)
        {
            Console.SetCursorPosition(ox, oy + y);
            Console.Write('│');
            Console.SetCursorPosition(ox + BoardW * 2 + 1, oy + y);
            Console.Write('│');
        }
        Console.SetCursorPosition(ox, oy + BoardH + 1);
        Console.Write('└' + new string('─', BoardW * 2) + '┘');
    }

    private static void DesenharBoard(int[,] board, ConsoleColor[,] cores, int[][] peca, int px, int py, ConsoleColor cor, int ox, int oy)
    {
        for (int y = 0; y < BoardH; y++)
        {
            Console.SetCursorPosition(ox + 1, oy + y + 1);
            for (int x = 0; x < BoardW; x++)
            {
                bool isPeca = false;
                for (int pr = 0; pr < peca.Length && !isPeca; pr++)
                    for (int pc = 0; pc < peca[pr].Length && !isPeca; pc++)
                        if (peca[pr][pc] == 1 && py + pr == y && px + pc == x)
                            isPeca = true;

                if (isPeca)
                {
                    Console.ForegroundColor = cor;
                    Console.Write("██");
                }
                else if (board[y, x] != 0)
                {
                    Console.ForegroundColor = cores[y, x];
                    Console.Write("██");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("··");
                }
            }
        }
    }

    private static void DesenharInfo(int ox, int oy, int pontos, int linhas)
    {
        int infoX = ox + BoardW * 2 + 4;
        Console.SetCursorPosition(infoX, oy + 2);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"Pontos: {pontos}   ");
        Console.SetCursorPosition(infoX, oy + 4);
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"Linhas: {linhas}   ");
        Console.SetCursorPosition(infoX, oy + 7);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("← → Mover");
        Console.SetCursorPosition(infoX, oy + 8);
        Console.Write("↑   Rotacionar");
        Console.SetCursorPosition(infoX, oy + 9);
        Console.Write("↓   Acelerar");
        Console.SetCursorPosition(infoX, oy + 10);
        Console.Write("SPC Drop");
    }

    private static bool Cabe(int[,] board, int[][] peca, int px, int py)
    {
        for (int r = 0; r < peca.Length; r++)
            for (int c = 0; c < peca[r].Length; c++)
                if (peca[r][c] == 1)
                {
                    int nx = px + c, ny = py + r;
                    if (nx < 0 || nx >= BoardW || ny >= BoardH) return false;
                    if (ny >= 0 && board[ny, nx] != 0) return false;
                }
        return true;
    }

    private static void Fixar(int[,] board, ConsoleColor[,] cores, int[][] peca, int px, int py, ConsoleColor cor)
    {
        for (int r = 0; r < peca.Length; r++)
            for (int c = 0; c < peca[r].Length; c++)
                if (peca[r][c] == 1 && py + r >= 0)
                {
                    board[py + r, px + c] = 1;
                    cores[py + r, px + c] = cor;
                }
    }

    private static int LimparLinhas(int[,] board, ConsoleColor[,] cores)
    {
        int count = 0;
        for (int y = BoardH - 1; y >= 0; y--)
        {
            bool cheia = true;
            for (int x = 0; x < BoardW; x++)
                if (board[y, x] == 0) { cheia = false; break; }

            if (cheia)
            {
                count++;
                for (int yr = y; yr > 0; yr--)
                    for (int x = 0; x < BoardW; x++)
                    {
                        board[yr, x] = board[yr - 1, x];
                        cores[yr, x] = cores[yr - 1, x];
                    }
                for (int x = 0; x < BoardW; x++) { board[0, x] = 0; cores[0, x] = ConsoleColor.Black; }
                y++; // Re-check same row
            }
        }
        return count;
    }

    private static int[][] Rotacionar(int[][] peca)
    {
        int rows = peca.Length, cols = peca[0].Length;
        var nova = new int[cols][];
        for (int c = 0; c < cols; c++)
        {
            nova[c] = new int[rows];
            for (int r = 0; r < rows; r++)
                nova[c][rows - 1 - r] = peca[r][c];
        }
        return nova;
    }

    private static int[][] CopiarPeca(int[][] peca) =>
        peca.Select(r => r.ToArray()).ToArray();
}
