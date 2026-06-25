[>] [English](README.en.md) | [Espanol](README.es.md)

# {~} TerminalArte

[![.NET CI](https://github.com/DanielHoffmannO/TerminalArte/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DanielHoffmannO/TerminalArte/actions)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green)

> Animacoes hipnotizantes e visualizacoes artisticas direto no terminal.

## [#] Artes Disponiveis

| Arte | O que faz |
|------|-----------|
| `[|||]` **Sorting Algorithms** | Bubble, Selection, Insertion e Quick Sort visual |
| `[///]` **Matrix Rain** | Cascata de caracteres estilo Matrix |
| `[oOo]` **Conway's Game of Life** | Automato celular com padroes emergentes |
| `[^^^]` **Fire Effect** | Simulacao de fogo com propagacao de calor |
| `[*o*]` **Pinball** | Simulacao com fisica de colisao |
| `[<>]` **Cubo 3D** | Cubo rotacionando com projecao ASCII |
| `[...]` **Starfield** | Campo estelar com efeito de profundidade |

## {=} Tech Stack

- .NET 9 / Console App
- C# puro (sem dependencias externas)
- GitHub Actions CI

## [!] Como Rodar

```bash
dotnet run --project src/TerminalArte.Console
```

## {/} Arquitetura

```
src/
+-- TerminalArte.Core      <- Algoritmos e logica das animacoes
+-- TerminalArte.Console   <- Renderizacao e interface no terminal
```

- **Core** -- Logica pura de cada arte (sorting, simulacoes, calculos 3D)
- **Console** -- Renderizacao dos frames no terminal

## [$] Licenca

Este projeto esta sob a licenca [MIT](LICENSE).
