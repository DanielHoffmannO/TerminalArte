# ==============================================================
# Script: commits-programados.ps1
# Cria commits com datas manipuladas (1 por dia)
# Uso: .\commits-programados.ps1
# ==============================================================

# ⚠️ CONFIGURAÇÃO - Ajuste aqui!
# Data de início (o primeiro commit será nessa data)
$DataInicio = [DateTime]::Parse("2026-06-12")

# Hora aleatória entre esses limites (parece mais natural)
$HoraMin = 9   # 9h da manhã
$HoraMax = 22  # 10h da noite

# ==============================================================
# LISTA DE COMMITS (1 por dia, na ordem)
# Formato: @{ Msg = "mensagem do commit"; Files = @("caminho1", "caminho2") }
# ==============================================================

$Commits = @(
    @{ Msg = "feat: add plasma effect with psychedelic sin/cos patterns"; Files = @(
        "src/TerminalArte.Core/Artes/Plasma/PlasmaArte.cs"
    )},
    @{ Msg = "feat: add snake game with scoring and collision"; Files = @(
        "src/TerminalArte.Core/Artes/Snake/SnakeArte.cs"
    )},
    @{ Msg = "feat: add ASCII digital clock with day progress bar"; Files = @(
        "src/TerminalArte.Core/Artes/Relogio/RelogioArte.cs"
    )},
    @{ Msg = "feat: add spirograph animation with color trails"; Files = @(
        "src/TerminalArte.Core/Artes/Espiral/EspiralArte.cs"
    )},
    @{ Msg = "feat: add aquarium with fish, bubbles and plants"; Files = @(
        "src/TerminalArte.Core/Artes/Aquario/AquarioArte.cs"
    )},
    @{ Msg = "feat: add playable tetris with rotation and scoring"; Files = @(
        "src/TerminalArte.Core/Artes/Tetris/TetrisArte.cs"
    )},
    @{ Msg = "feat: add procedural maze generation with BFS solver"; Files = @(
        "src/TerminalArte.Core/Artes/Labirinto/LabirintoArte.cs"
    )},
    @{ Msg = "feat: add lightning bolts with flash effect"; Files = @(
        "src/TerminalArte.Core/Artes/Raios/RaiosArte.cs"
    )},
    @{ Msg = "feat: add oscilloscope with multiple wave channels"; Files = @(
        "src/TerminalArte.Core/Artes/Osciloscopio/OsciloscopioArte.cs"
    )},
    @{ Msg = "feat: add rain simulation with splashes"; Files = @(
        "src/TerminalArte.Core/Artes/Chuva/ChuvaArte.cs"
    )},
    @{ Msg = "feat: add rotating DNA double helix with base pairs"; Files = @(
        "src/TerminalArte.Core/Artes/Dna/DnaArte.cs"
    )},
    @{ Msg = "feat: add fireworks with particle physics"; Files = @(
        "src/TerminalArte.Core/Artes/Fogos/FogosArte.cs"
    )},
    @{ Msg = "feat: add pong game vs AI opponent"; Files = @(
        "src/TerminalArte.Core/Artes/Pong/PongArte.cs"
    )},
    @{ Msg = "feat: add boids flocking simulation"; Files = @(
        "src/TerminalArte.Core/Artes/Boids/BoidsArte.cs"
    )},
    @{ Msg = "feat: add fractal tree growing with wind effect"; Files = @(
        "src/TerminalArte.Core/Artes/Fractal/FractalArte.cs"
    )},
    @{ Msg = "feat: add infinite tunnel warp effect"; Files = @(
        "src/TerminalArte.Core/Artes/Tunel/TunelArte.cs"
    )},
    @{ Msg = "feat: add ocean waves with gradient"; Files = @(
        "src/TerminalArte.Core/Artes/Ondas/OndasArte.cs"
    )},
    @{ Msg = "feat: add hacker typing effect with fake code"; Files = @(
        "src/TerminalArte.Core/Artes/Typing/TypingArte.cs"
    )},
    @{ Msg = "feat: add DVD bouncing logo with corner hit counter"; Files = @(
        "src/TerminalArte.Core/Artes/Dvd/DvdArte.cs"
    )},
    @{ Msg = "feat: register all new arts in menu and update README"; Files = @(
        "src/TerminalArte.Console/Program.cs",
        "README.md"
    )}
)

# ==============================================================
# EXECUÇÃO
# ==============================================================

Write-Host ""
Write-Host "═══════════════════════════════════════════" -ForegroundColor Magenta
Write-Host "  Commits Programados - TerminalArte" -ForegroundColor Magenta
Write-Host "═══════════════════════════════════════════" -ForegroundColor Magenta
Write-Host ""
Write-Host "  Total de commits: $($Commits.Count)" -ForegroundColor Cyan
Write-Host "  Data inicio: $($DataInicio.ToString('dd/MM/yyyy'))" -ForegroundColor Cyan
Write-Host "  Data fim:    $($DataInicio.AddDays($Commits.Count - 1).ToString('dd/MM/yyyy'))" -ForegroundColor Cyan
Write-Host ""

$Confirmacao = Read-Host "  Prosseguir? (s/n)"
if ($Confirmacao -ne "s") {
    Write-Host "  Cancelado." -ForegroundColor Yellow
    exit
}

Write-Host ""

$rng = [System.Random]::new()

for ($i = 0; $i -lt $Commits.Count; $i++) {
    $commit = $Commits[$i]
    $data = $DataInicio.AddDays($i)

    # Hora aleatória para parecer natural
    $hora = $rng.Next($HoraMin, $HoraMax)
    $minuto = $rng.Next(0, 60)
    $segundo = $rng.Next(0, 60)
    $dataCompleta = $data.AddHours($hora).AddMinutes($minuto).AddSeconds($segundo)

    # Formato ISO 8601 que o git aceita
    $dataGit = $dataCompleta.ToString("yyyy-MM-ddTHH:mm:ss")

    # Stage arquivos
    foreach ($file in $commit.Files) {
        git add $file 2>$null
    }

    # Commit com data manipulada
    $env:GIT_AUTHOR_DATE = $dataGit
    $env:GIT_COMMITTER_DATE = $dataGit

    git commit -m $commit.Msg 2>$null

    # Limpar variáveis de ambiente
    Remove-Item Env:\GIT_AUTHOR_DATE
    Remove-Item Env:\GIT_COMMITTER_DATE

    $status = if ($LASTEXITCODE -eq 0) { "✅" } else { "⚠️" }
    Write-Host "  $status [$($dataCompleta.ToString('dd/MM HH:mm'))] $($commit.Msg)" -ForegroundColor Green
}

Write-Host ""
Write-Host "═══════════════════════════════════════════" -ForegroundColor Green
Write-Host "  Pronto! $($Commits.Count) commits criados." -ForegroundColor Green
Write-Host "═══════════════════════════════════════════" -ForegroundColor Green
Write-Host ""
Write-Host "  Para verificar: git log --oneline -20" -ForegroundColor DarkGray
Write-Host "  Para enviar:    git push" -ForegroundColor DarkGray
Write-Host ""
