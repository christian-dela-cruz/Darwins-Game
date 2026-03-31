# Darwin's Game

A C# Windows Forms sidescroller game inspired by the theory of evolution. Guide your character through five stages of life — from a primordial creature all the way to modern humanity — dodging obstacles and surviving each era.

---

## Gameplay

- **5 unique stages**, each representing a different epoch in evolutionary history:
  - **Stage 1 – Missing Link** (early hominid)
  - **Stage 2 – Lizard** (reptile era)
  - **Stage 3 – Dinosaur** (Mesozoic)
  - **Stage 4 – Monkey** (primate)
  - **Stage 5 – Human** (modern era)
- Dodge obstacles (rocks, logs, snakes, bombs, and more) as the world scrolls past.
- Survive long enough to reach the end of each stage and evolve to the next form.
- A **leaderboard** tracks the top three high scores between sessions.
- Background music and sound effects accompany each stage (toggle on/off via the speaker icon).

## Controls

| Key | Action |
|-----|--------|
| `Space` / `Up Arrow` | Jump |
| `Down Arrow` | Crouch |

## Requirements

- **Windows** (WinForms application)
- **.NET Framework 4.x** (Visual Studio 2022 recommended)

## How to Run

1. Open `Darwins_Game/Sidescroller Game.sln` in **Visual Studio**.
2. Set the build configuration to **Debug** or **Release**.
3. Press **F5** (or click **Start**) to build and run.

> **Note:** All game assets (images, sounds) are located in `Darwins_Game/assets/`. The game resolves asset paths at runtime relative to the executable, so make sure to run from within Visual Studio or from the `bin/Debug` output directory.

## Project Structure

```
Darwins_Game/
├── assets/          # Images, GIFs, and sound files
│   ├── STAGE 01/    # Missing-link character sprites
│   ├── STAGE 02/    # Lizard character sprites
│   ├── STAGE 03/    # Dinosaur character sprites
│   ├── STAGE 04/    # Monkey character sprites
│   ├── STAGE 05/    # Human character sprites
│   ├── STAGE BGS/   # Scrolling stage backgrounds
│   ├── Obstacles/   # Obstacle sprites
│   └── Sound/       # Background music & SFX
├── Entity.cs        # Base classes: Character, Obstacle, Platform
├── GameWindowForm.cs# Core game loop and rendering
├── Menu.cs          # Main menu and leaderboard UI
├── Leaderboards.cs  # Score persistence (saved data/leaderboards.txt)
├── Utility.cs       # Path helpers and image utilities
├── Stage1–5.cs      # Per-stage configuration
├── Program.cs       # Entry point
└── Sidescroller Game.sln
```

## License

This project is for educational purposes.
