🌐 [English](README.en.md) | [Español](README.es.md)

# 🎨 TerminalArte

[![.NET CI](https://github.com/DanielHoffmannO/TerminalArte/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DanielHoffmannO/TerminalArte/actions/workflows/dotnet.yml)
![.NET 9](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![License MIT](https://img.shields.io/badge/License-MIT-green)

> Animações hipnotizantes e visualizações artísticas direto no terminal.

## 🖼️ Artes Disponíveis

| Arte | Descrição |
|------|-----------|
| 📊 **Sorting Algorithms** | Visualização de Bubble Sort, Selection Sort, Insertion Sort e Quick Sort |
| 🟢 **Matrix Rain** | Efeito cascata de caracteres inspirado no filme Matrix |
| 🧬 **Conway's Game of Life** | Autômato celular com padrões emergentes |
| 🔥 **Fire Effect** | Simulação de fogo com propagação de calor |
| 🎯 **Pinball** | Simulação de pinball com física de colisão |
| 🧊 **Cubo 3D** | Cubo rotacionando em 3 dimensões com projeção ASCII |
| ✨ **Starfield** | Campo estelar com efeito de profundidade e velocidade |

## 🛠️ Tech Stack

- **.NET 9** — Console App
- **C#** — Linguagem principal
- **GitHub Actions** — CI/CD

## 🚀 Como Rodar

```bash
# Clone o repositório
git clone https://github.com/DanielHoffmannO/TerminalArte.git
cd TerminalArte

# Execute
dotnet run
```

## 🏗️ Arquitetura

```
TerminalArte/
├── Core/        → Algoritmos e lógica das animações
└── Console/     → Renderização e interface no terminal
```

- **Core** — Contém a lógica pura de cada arte (sorting, simulações, cálculos 3D)
- **Console** — Responsável pela renderização dos frames no terminal

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).

## 👤 Autor

**Daniel Hoffmann** — [GitHub](https://github.com/DanielHoffmannO)
