[>] [English](README.en.md) | [Español](README.es.md)

# {~} TerminalArte

[![.NET CI](https://github.com/DanielHoffmannO/TerminalArte/actions/workflows/dotnet.yml/badge.svg)](https://github.com/DanielHoffmannO/TerminalArte/actions)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)
![License](https://img.shields.io/badge/license-MIT-green)
![Arts](https://img.shields.io/badge/artes-33-orange)

> Animações hipnotizantes, jogos e visualizações artísticas direto no terminal. Zero dependências externas.

---

## [!] Como Rodar

```bash
dotnet run --project src/TerminalArte.Console
```

Pressione qualquer tecla para voltar ao menu durante uma animação.

---

## [#] Artes Disponíveis (33)

### 🎮 Jogos

| Arte | Controles | Descrição |
|------|-----------|-----------|
| **Snake** | WASD / Setas | Jogo da cobrinha com pontuação e game over |
| **Tetris** | Setas + Espaço | Tetris completo com rotação, score e velocidade progressiva |
| **Pong** | W/S | Pong clássico contra IA adaptativa |

### 📊 Algoritmos e Simulações

| Arte | Descrição |
|------|-----------|
| **Sorting Algorithms** | Visualização de Bubble, Selection, Insertion e Quick Sort |
| **Conway's Game of Life** | Autômato celular com padrões emergentes |
| **Boids** | Simulação de bando de pássaros (flocking com separação, alinhamento e coesão) |
| **Pêndulo Duplo** | Pêndulo duplo caótico com física real e rastro colorido |
| **Labirinto** | Geração procedural (DFS) + resolução animada (BFS) |

### 🌌 Efeitos Visuais

| Arte | Descrição |
|------|-----------|
| **Matrix Rain** | Cascata de caracteres estilo Matrix |
| **Fire Effect** | Simulação de fogo com propagação de calor |
| **Plasma** | Efeito psicodélico com funções seno/cosseno |
| **Starfield** | Campo estelar 3D (screensaver clássico) |
| **Spirograph** | Espirais matemáticas animadas com trail |
| **Raios** | Relâmpagos procedurais com flash no céu noturno |
| **Fogos de Artifício** | Foguetes + explosões com partículas e gravidade |
| **Túnel** | Efeito warp speed infinito |
| **Partículas** | Sistema de partículas com emissor, gravidade e explosões |
| **Nebulosa** | Nebulosa espacial procedural com estrelas cintilantes |
| **DVD Bounce** | Logo DVD quicando na tela (muda cor ao bater, flash no canto!) |

### 🌊 Natureza

| Arte | Descrição |
|------|-----------|
| **Aquário** | Peixes ASCII nadando com bolhas e plantas no fundo |
| **Chuva** | Gotas caindo com velocidades diferentes + splashes + poças |
| **Ondas** | Mar com sol, gradiente de profundidade e ondas sobrepostas |
| **Árvore Fractal** | Árvore fractal que cresce e balança com o vento |
| **Lava Lamp** | Lava lamp retrô com bolhas que sobem por temperatura |

### 🔬 Ciência

| Arte | Descrição |
|------|-----------|
| **DNA Helix** | Dupla hélice rotacionando com pares de bases A/T/G/C |
| **Osciloscópio** | Múltiplos canais de ondas com grade e leitura de voltagem |
| **Cubo 3D** | Cubo rotacionando com projeção perspectiva ASCII |

### ⏱️ Utilitários

| Arte | Descrição |
|------|-----------|
| **Relógio Digital** | Relógio gigante ASCII + data + barra de progresso do dia |
| **Hacker Typing** | Efeito de digitação estilo Hollywood hacker |

### 🕹️ Clássicos

| Arte | Descrição |
|------|-----------|
| **Pinball** | Simulação com física de colisão e gravidade |

---

## {=} Tech Stack

- **.NET 9** — Console App
- **C# puro** — zero pacotes NuGet, zero dependências externas
- **GitHub Actions** — CI com build automático

---

## {/} Arquitetura

```
src/
├── TerminalArte.Core       ← Lógica pura de cada arte (IArte)
│   ├── Artes/              ← Uma pasta por arte
│   └── Interfaces/         ← IArte, ISortingAlgorithm
│
└── TerminalArte.Console    ← Menu interativo e execução
    └── Program.cs
```

Cada arte implementa `IArte` com:
- `Nome` — exibido no menu
- `Descricao` — descrição curta
- `Executar(CancellationToken)` — loop da animação (cancela ao pressionar tecla)

---

## [+] Contribuindo

Quer adicionar uma arte? Basta:

1. Criar uma pasta em `src/TerminalArte.Core/Artes/{NomeDaArte}/`
2. Implementar `IArte`
3. Registrar no `Program.cs`
4. PR!

---

## [$] Licença

Este projeto está sob a licença [MIT](LICENSE).
