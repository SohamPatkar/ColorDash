# Color Match Runner

Color Match Runner is a simple and engaging 2D mobile game built using Unity. The core gameplay revolves around a color-based obstacle dodging mechanic, rewarding reflexes and precision.

# Game Objective

The player controls a character that automatically moves forward and can only pass through obstacles if their colors match. If the colors do not match, the player collides and the game ends. The goal is to survive as long as possible and achieve a high score.

# Gameplay Features

🔴 Color Matching: Player changes color during the run. Passing through an obstacle is only allowed if the colors match.

🧱 Obstacle Spawning: Obstacles are spawned at regular intervals using an Object Pooling system to optimize performance.

🌈 Dynamic Obstacles: Obstacles change color randomly, forcing the player to adapt quickly.

🌟 Particles and Effects: Particle systems provide feedback on collision, color changes, and high score achievements.

📈 High Score System: PlayerPrefs are used to store and retrieve high scores, which persist even after the app is closed or restarted.

# Code Architecture

#  Singleton Pattern

The GameService class is implemented using the Singleton pattern to ensure that only one instance exists during runtime.
It persists essential game data and managers across the game session.

#  Object Pooling

Obstacles are instantiated using a custom Object Pooling system to avoid performance hits from frequent Instantiate() and Destroy() calls.
This makes spawning efficient and suitable for mobile platforms.

#  Observer Pattern

The Observer pattern is used for UI updates:
The score, color status, and game over events are communicated to UI elements via event listeners.
This results in a loosely coupled and easily maintainable UI system.

#  How to Play
Tap to change color of your character.

Match the color of your player to the upcoming obstacle to pass through it.

Avoid mismatched obstacles or the game ends.

Your score increases the longer you survive.

Try to beat your high score!

#  Mobile Compatibility
This game is fully compatible with Android builds.

PlayerPrefs are used for saving high scores and work seamlessly on Android devices.

🧩 Tools & Technologies
Unity (2D Mode)

C#

Particle Systems

Unity Animator

Unity UI System

