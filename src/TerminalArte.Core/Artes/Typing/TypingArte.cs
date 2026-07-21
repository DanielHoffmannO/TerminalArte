using TerminalArte.Core.Interfaces;

namespace TerminalArte.Core.Artes.Typing;

public class TypingArte : IArte
{
    public string Nome => "Hacker Typing";
    public string Descricao => "Efeito de digitação hacker com código real";

    private static readonly string[] CodigoFonte =
    [
        "using System.Security.Cryptography;",
        "using System.Net.Sockets;",
        "",
        "namespace CyberOps.Core.Exploitation;",
        "",
        "public class NetworkScanner",
        "{",
        "    private readonly TcpClient _client;",
        "    private readonly byte[] _payload;",
        "",
        "    public async Task<bool> ScanPort(string host, int port)",
        "    {",
        "        try",
        "        {",
        "            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(2));",
        "            await _client.ConnectAsync(host, port, cts.Token);",
        "            var stream = _client.GetStream();",
        "            await stream.WriteAsync(_payload);",
        "            Logger.Info($\"[+] Port {port} OPEN on {host}\");",
        "            return true;",
        "        }",
        "        catch (SocketException)",
        "        {",
        "            return false;",
        "        }",
        "    }",
        "",
        "    public async Task<IEnumerable<int>> SweepRange(string subnet, int start, int end)",
        "    {",
        "        var openPorts = new ConcurrentBag<int>();",
        "        var tasks = Enumerable.Range(start, end - start)",
        "            .Select(port => Task.Run(async () =>",
        "            {",
        "                if (await ScanPort(subnet, port))",
        "                    openPorts.Add(port);",
        "            }));",
        "        await Task.WhenAll(tasks);",
        "        return openPorts.OrderBy(p => p);",
        "    }",
        "}",
        "",
        "// [SYSTEM] Decrypting AES-256 payload...",
        "// [SYSTEM] Establishing reverse shell connection...",
        "// [SYSTEM] Injecting shellcode into process memory...",
        "// [SYSTEM] Bypassing firewall rules...",
        "// [OK] Access granted. Welcome back, operator.",
    ];

    public void Executar(CancellationToken ct)
    {
        int largura = Console.WindowWidth;
        var rng = new Random();

        Console.CursorVisible = false;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;

        int linha = 0;
        int col = 0;
        int linhaAtual = 0;

        while (!ct.IsCancellationRequested)
        {
            string textoLinha = CodigoFonte[linhaAtual % CodigoFonte.Length];

            if (col < textoLinha.Length)
            {
                // Digitar caractere por caractere
                char c = textoLinha[col];

                // Cor especial para comentários e keywords
                if (textoLinha.TrimStart().StartsWith("//"))
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                else if (textoLinha.Contains("public") || textoLinha.Contains("private") ||
                         textoLinha.Contains("async") || textoLinha.Contains("await") ||
                         textoLinha.Contains("using") || textoLinha.Contains("namespace"))
                    Console.ForegroundColor = ConsoleColor.Cyan;
                else if (textoLinha.Contains("[+]") || textoLinha.Contains("[OK]"))
                    Console.ForegroundColor = ConsoleColor.Yellow;
                else if (textoLinha.Contains("[SYSTEM]"))
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.Write(c);
                col++;

                // Velocidade variável (mais lento em strings/comentários)
                Thread.Sleep(rng.Next(10, 40));
            }
            else
            {
                // Nova linha
                Console.WriteLine();
                linha++;
                col = 0;
                linhaAtual++;

                // Scroll se necessário
                if (linha >= Console.WindowHeight - 1)
                {
                    linha = Console.WindowHeight - 2;
                }

                // Pausa entre linhas
                Thread.Sleep(rng.Next(50, 200));

                // Efeito "processando" aleatório
                if (rng.Next(10) == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    string loading = "[████████████████████] 100%";
                    foreach (char lc in loading)
                    {
                        Console.Write(lc);
                        Thread.Sleep(15);
                        if (ct.IsCancellationRequested) break;
                    }
                    Console.WriteLine();
                    linha++;
                    Thread.Sleep(200);
                }
            }
        }

        Console.ResetColor();
    }
}
