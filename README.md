# 🎰 High-Fidelity Unity Slot Machine

A professional, high-performance slot machine game built in Unity, featuring smooth mechanical reel physics, a modular event-driven architecture, and dynamic betting logic.

## 📄 Overview
This project implements a classic 3-reel slot machine with a focus on **Game Feel** and **Technical Scalability**. Instead of simple frame-swapping, the reels utilize a physical coordinate system and distance-based easing for a truly mechanical feel.

## 🚀 How to Run
1. **Clone the Repo**: `git clone <your-repo-link>`
2. **Open in Unity**: Use Unity 2022.3 LTS or newer.
3. **WebGL Build**: Navigate to the `/Builds/WebGL` folder and open the `index.html` (or run the project directly in the Editor).

## 🛠️ Technical Highlights

### 1. The "Physical Reel" Engine
Unlike traditional slot games that use simple "stop-on-frame" logic, this engine calculates a **dynamic stop distance**.
*   **Seamless Transitions**: The stopping duration is calculated relative to current speed, ensuring the reel never "speeds up" to catch a target.
*   **DOTween Integration**: Utilizes `Ease.OutCubic` for a smooth deceleration that perfectly mimics real-world friction.
*   **Stable Wrapping**: A fixed-grid coordinate system ensures symbols wrap around the view perfectly without jitters or jerks.

### 2. Fair RNG System
The game uses a **ScriptableObject-driven Database** and standard `Random.Range` to ensure completely unpredictable and fair outcomes for every spin.
*   **Decoupled Logic**: The RNG result is determined *before* the reels stop, allowing the visual engine to "glide" precisely to the target.

### 3. Modular Architecture (MVC-ish)
The project is split into clean, logical layers:
*   **Core**: `SlotMachineController` (The Brain), `PayoutManager` (The Accountant).
*   **View**: `SlotReel` (Visual Motion), `UIManager` (HUD), `SlotLever` (Input Interaction).
*   **Data**: `SlotDatabaseSO`, `SlotSettingsSO` (Configuration).

## ✨ Bonus Features & "Juice"
*   **Tactile Lever**: A fully animated 111-degree mechanical pull that stays down during the spin.
*   **Dynamic Betting**: Seamlessly cycle through bet amounts (10, 50, 100, 200) with instant HUD updates.
*   **Winner Highlight**: Winning symbols scale and "pulse" in gold to provide satisfying feedback.
*   **Machine Shake**: The entire machine cabinet vibrates when a win is hit.
*   **Responsive Scaling**: The reel layout automatically adjusts symbol sizing to fit the window while maintaining perfect vertical spacing.

## 🎨 Asset Credits
*   UI & Symbols: Provided in the Assignment Asset Pack.
*   Animations: Custom DOTween implementation.

---
*Built as part of the Unity Game Developer Assessment.*
