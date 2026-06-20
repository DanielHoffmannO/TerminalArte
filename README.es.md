🌐 [Português](README.md) | [English](README.en.md)

# 🎨 TerminalArte

Animaciones y visualizaciones artísticas en la terminal — algoritmos de ordenación, Matrix rain, Game of Life, fuego, pinball, campo estelar y cubo 3D.

## Tech Stack

- .NET 9 / Console App
- Arquitectura en capas (Core + Console)

## Cómo Ejecutar

```bash
dotnet run --project src/TerminalArte.Console
```

## Artes Disponibles

- 📊 Algoritmos de Ordenación (Bubble, Selection, Insertion, Quick Sort)
- 🟩 Matrix Rain
- 🧬 Conway's Game of Life
- 🔥 Efecto de Fuego
- 🎮 Pinball
- 🧊 Cubo 3D
- ⭐ Campo Estelar

## Arquitectura

```
src/
├── TerminalArte.Core      ← Algoritmos y lógica de las artes
└── TerminalArte.Console   ← UI y renderización en la terminal
```