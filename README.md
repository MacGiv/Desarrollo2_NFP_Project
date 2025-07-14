# Desarrollo2_NFP_Project
Nullframe Protocol Project - Desarrollo 2

# 🎮 Nullframe Protocol

Nullframe Protocol is a third-person action-platformer built in **Unity 3D**, featuring responsive movement, precise combat mechanics, and a strange story told through dialogue fragments. This project was developed in _Prácticas Profesionales II: Desarrollo de Videojuegos II_.

---

## 🧠 Core Features

- ✅ Built with Unity 3D (URP)
- ✅ Uses Unity's new **Input System**
- ✅ Fully playable with **keyboard/mouse** and **gamepad**
- ✅ Fully navigable UI (menus, HUD) with **both input methods**
- ✅ Additive **Scene Management** with `SceneLoader` and `SceneFlowHandler`
- ✅ Game loop with progressive levels, combat, platforming, and cheats
- ✅ Application of **S.O.L.I.D.** principles and multiple design patterns

---

## 🎮 Gameplay Mechanics

### 🕹️ Player Movement
- **Smooth third-person movement** with acceleration & deceleration.
- **Jump & double jump**, with:
  - ✅ **Coyote Time**
  - ✅ **Jump Buffering**
- Optional **Run (Sprint)** mode with visual and animation feedback.

### ⚔️ Combat System
- **Combo-based melee combat** (3-hit chain).
- Directional attacks:
  - If an enemy is nearby, the character auto-rotates to face them.
  - If no enemy is nearby, the attack is directed toward the camera direction.
- **Lock-On System**:
  - Toggleable targeting system that locks the camera and the player's facing direction toward the closest enemy in front of the screen center.
- **Special Attack**:
  - Consumes 3 charges.
  - Dashes the player directly toward the target, even in vertical space.
  - Arc movement with landing and damage on impact.
  - Charges gained from defeated enemies.
- **Air Attack** (future extension planned).

### 💀 Damage System
- Both **player and enemies** have health systems with UI feedback.
- Traps deal damage to the player on trigger.
- Enemies are destroyed with a death event.
- On player death, a **Defeat Screen** allows retrying or returning to menu.

---

## 🧩 Cheats (for debug/fun)
| Key | Effect                  |
|-----|--------------------------|
| F9  | Skip to the next level   |
| F10 | Toggle God Mode          |
| F11 | Toggle Speed Boost       |

---

## 📖 Game Structure

### 🎬 Scenes
- **Boot**: Initializes all persistent systems (SceneLoader, Audio, Services).
- **MainMenu**: Fully navigable menu with UI + controller support.
- **Credits**: Standalone scene.
- **GameScene_1**: Tutorial - Movement (platforms, traps).
- **GameScene_2**: Tutorial - Combat.
- **GameScene_3**: First real level combining combat and movement.
- **GameScene_4**: Final level with increased challenge or boss.

### ✅ Scene Management
- `SceneLoader` loads/unloads scenes asynchronously.
- `SceneFlowHandler` manages transitions and current/last scene references.
- Persistent managers registered using **Service Locator (ServiceProvider)**.

---

## 🧠 Design Patterns & Principles

### ✅ SOLID
- **S**ingle Responsibility: Each component handles one job (movement, input, health, etc.).
- **O**pen/Closed: Easy to add new states, attacks, particles without modifying old code.
- **L**iskov Substitution: `PlayerState` hierarchy ensures interchangeable behaviors.
- **I**nterface Segregation: Custom interfaces could be added (e.g. IDamageable).
- **D**ependency Inversion: Services accessed via `ServiceProvider`.

### ✅ Design Patterns
- **State Pattern**: FSM (`PlayerStateMachine`) with clear transitions for grounded, airborne, attack, special attack, etc.
- **Observer Pattern**: 
  - `OnDeath`, `OnHealthChanged` events.
  - UI listens to updates.
- **Service Locator**: 
  - Centralized access to services (`SceneLoader`, `MusicHandler`, `ParticleHandler`, etc.)
- **Command Pattern** _(input)_:
  - Input Actions mapped to events decoupled from logic.
- **Strategy Pattern** (Planned): For future enemy AI behavior.

---

## 🖼️ Visual & Audio

- ✅ All particle effects instantiated via `ParticleHandler` (centralized).
- ✅ Hit particles, special aura, attack VFX.
- ✅ Audio Manager system with scene-dependent music themes:
  - `St_Menu_Theme`
  - `St_Level_Theme_1`
  - `St_Level_Theme_2`
  - `St_Level_Theme_3`
- ⚠️ Assets are placeholders – to be polished.

---

## 🧪 Input Setup

- Uses Unity **Input System** v1+
- Two Action Maps:
  - `Gameplay`: Movement, Attack, SpecialAttack, Cheats, Pause
  - `UI`: Navigation, Submit/Cancel, Pointing
- Fully supports:
  - **Keyboard + Mouse**
  - **Gamepad (Xbox / DualShock)**

---

## 🧪 Known Bugs / Pending

- No menu for settings or volume adjustment (planned).
- Boss AI not implemented yet (planned for `GameScene_4`).

---

## 📦 Build & Distribution

- 🎮 Built with Unity 6
- 💾 Build ready for Windows
- 📂 Available on [itch.io](pending)
- 🔗 [GitHub Repository](https://github.com/MacGiv/Desarrollo2_NFP_Project)

---

## 💬 Credits

Developed by: **Tomas Francisco Luchelli**  
As part of **Práctica Profesional II: Desarrollo de Videojuegos II**  

---
