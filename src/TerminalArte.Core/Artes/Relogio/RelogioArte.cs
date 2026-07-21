using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Relogio;

public class RelogioArte : IArte
{
    public string Nome => "Relógio Digital";
    public string Descricao => "Relógio gigante ASCII com segundos animados";

    // Cada dígito tem 5 linhas x 5 colunas
    private static readonly string[][] Digitos =
    [
        // 0
        ["█████", "█   █", "█   █", "█   █", "█████"],
        // 1
        ["    █", "    █", "    █", "    █", "    █"],
        // 2
        ["█████", "    █", "█████", "█    ", "█████"],
        // 3
        ["█████", "    █", "█████", "    █", "█████"],
        // 4
        ["█   █", "█   █", "█████", "    █", "    █"],
        // 5
        ["█████", "█    ", "█████", "    █", "█████"],
        // 6
        ["█████", "█    ", "█████", "█   █", "█████"],
        // 7
        ["█████", "    █", "    █", "    █", "    █"],
        // 8
        ["█████", "█   █", "█████", "█   █", "█████"],
        // 9
        ["█████", "█   █", "█████", "    █", "█████"],
    ];

    private static readonly string[] Separador = ["     ", "  █  ", "     ", "  █  ", "     "];

    private static readonly ConsoleColor[] CoresHora =
    [
        ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.Yellow,
        ConsoleColor.Magenta, ConsoleColor.Red, ConsoleColor.Blue
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        int altura = Console.WindowHeight;

        Console.CursorVisible = false;
        Console.Clear();

        int corIndex = 0;

        while (!ct.IsCancellationRequested)
        {
            var agora = DateTime.Now;
            string hora = agora.ToString("HH:mm:ss");

            // Construir linhas do display
            var linhas = new string[5];
            for (int linha = 0; linha < 5; linha++)
            {
                linhas[linha] = "";
                foreach (char c in hora)
                {
                    if (c == ':')
                    {
                        linhas[linha] += Separador[linha] + " ";
                    }
                    else
                    {
                        int digito = c - '0';
                        linhas[linha] += Digitos[digito][linha] + " ";
                    }
                }
            }

            // Centralizar no terminal
            int displayLargura = linhas[0].Length;
            int startX = Math.Max(0, (largura - displayLargura) / 2);
            int startY = Math.Max(0, (altura - 12) / 2);

            // Cor muda a cada segundo
            var corPrincipal = CoresHora[corIndex % CoresHora.Length];
            if (agora.Second != (agora.Second + 59) % 60)
                corIndex = agora.Second % CoresHora.Length;

            // Desenhar relógio
            Console.ForegroundColor = corPrincipal;
            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(startX, startY + i);
                Console.Write(linhas[i]);
            }

            // Data abaixo
            Console.SetCursorPosition(startX, startY + 7);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string data = agora.ToString("dddd, dd MMMM yyyy");
            int dataOffset = Math.Max(0, (displayLargura - data.Length) / 2);
            Console.Write(new string(' ', dataOffset) + data);

            // Barra de progresso do dia
            Console.SetCursorPosition(startX, startY + 9);
            double progresso = (agora.Hour * 3600 + agora.Minute * 60 + agora.Second) / 86400.0;
            int barraLargura = Math.Min(displayLargura, 40);
            int preenchido = (int)(barraLargura * progresso);

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(new string('█', preenchido));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('░', barraLargura - preenchido));
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"] {progresso * 100:F1}%");

            Console.ResetColor();
            Thread.Sleep(200);
        }

        Console.ResetColor();
    }
}
