# 🎰 Unity Slot Game Assignment

A professionally architected, event-driven Slot Machine simulation built in Unity. This project demonstrates clean code principles, decoupled visual systems, and smooth, responsive gameplay.

## 📄 Overview

This project implement a 3-reel slot machine with a focus on **Game Feel (Juice)** and **Software Architecture**. It features a fully randomized RNG system, dynamic betting tiers, and a cinematic win sequence.

## 🎮 Features

- **Event-Driven Architecture**: Core logic is decoupled from UI and Visual Effects using C# Actions.
- **Dynamic Betting**: Tiered betting system (10, 50, 100, 200) with real-time balance updates.
- **Cinematic Feedback**:
  - Screen Shakes on spin and win.
  - Symbol pulsing animations on win.
  - Interactive win notifications with auto-hide and dismiss-on-click functionality.
- **RNG Reliability**: Fair randomization using ScriptableObject-driven symbol databases.
- **Debug Tools**: Built-in `Force Win` toggle for rapid testing of win conditions.

## 🏗️ Technical Architecture

The project follows a **decoupled component-based design**:

- **SlotMachineController**: The central engine. Handles the lifecycle of a spin but knows nothing about visuals.
- **PayoutManager**: Handles the "Economy" and win calculations. Pure logic.
- **SlotEffectManager**: Listens to engine events to trigger visual feedback (shakes, vibration).
- **UIManager**: A standalone observer that updates the HUD based on economy events.

## 🛠️ Installation & Running

1. Clone this repository.
2. Open the project in **Unity 6000.0.62f1 LTS**.
3. Open the main scene located in `Assets/_SlotGames/Scenes/`.
4. Press **Play**!

### 🕹️ Controls

- **Spin**: Click the "Spin" button, pull the animated Lever, or press **Spacebar**.
- **Betting**: Use the **+** and **-** buttons to cycle through bet amounts.
- **Debug**: Toggle `Debug Force Win` on the `SlotMachineController` to test win animations.

## 🌐 Instructions to Run WebGL Build

1. Navigate to the `/Build/WebGL/` folder in this repository.
2. Open the `index.html` file in a modern web browser (Chrome, Firefox, or Edge).
3. **Note**: Most browsers require a local server to run WebGL (due to security policies). You can use a Unity local build, a VS Code extension like "Live Server," or Python's `http.server` to view it locally.

## 🎁 Bonus Features

- **Physical Interaction**: A fully animated mechanical **Lever** that triggers spins with a natural 111-degree pull.
- **Enhanced "Juice"**:
  - High-intensity **Machine Shakes** during wins and mild vibrations on spin-up.
  - **Pulsing Symbols**: Winning combinations physically pulse and glow to provide clear visual feedback.
- **Developer Debug Tools**: A serialized `Debug Force Win` toggle to instantly test win sequences and payouts without waiting for RNG.

## 🧠 Thought Process & Approach

My approach was centered on creating a **Production-Ready** foundation rather than just a simple prototype.

1. **Architecture (Decoupling)**: I implemented an **Event-Driven Design**. By using C# Actions, the core `SlotMachineController` remains ignorant of visual effects and UI. This ensures that if we wanted to change the UI or add a new Particle System, we wouldn't have to touch the game's core logic.
2. **Data-Driven Design**: I used **ScriptableObjects** (`SlotDatabaseSO`, `SlotSymbolSO`) to manage game data. This allows for easy balancing of payouts and adding new symbols without touching a single line of code.
3. **Game Feel (Juice)**: I prioritized "Tactile Feedback." Using **DOTween**, I added subtle bounces, shakes, and scale pulses. These micro-interactions are what separate a "basic" slot machine from a "premium" gaming experience.
4. **RNG Fairness**: The randomization is handled independently per reel, ensuring that outcomes are statistically fair and unpredictable, mimicking real-world slot mechanics.
