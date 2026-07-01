### **How to Play**
* **Objective:** Pilot the Millennium Falcon, destroy incoming Tie Fighters, and survive as long as possible to accumulate a high score.
* **Controls:**
    * **Rotate:** Press **A** to steer left and **D** to steer right.
    * **Thrust:** Press **W** to accelerate forward.
    * **Fire:** Press **Space** to shoot lasers.
* **Mechanics:**
    * **Starting the Game:** Click the "Start Game" button on the main menu to begin the match.
    * **Screen Wrap:** Flying off any edge of the screen will seamlessly transport the ship to the opposite side.
    * **Enemies:** Tie Fighters come in varying sizes and move continuously. Destroying them splits them and increases the score.
    * **Health:** Colliding with an enemy results in losing a life.
* **Download**
    * **Itch.io:** The game can be downloaded from [itch.io](https://nicosoftwareengineer.itch.io/asteroids).

---

### **Game States**
* **Start Menu:** The initial landing screen. The game waits for the player to click the "Start Game" button to trigger the start sequence and initialize the match.
* **Active Gameplay:** The core loop where the player controls the ship, enemies spawn continuously, and the HUD actively tracks the current score and remaining lives.
* **Invulnerability Window:** A brief 0.5-second safe state immediately after the player is destroyed and respawns, preventing instant consecutive deaths.
* **Game Over:** Triggered when the player loses their final life. The active gameplay halts, the screen displays the final score, and the player is given the prompt to restart, which wipes the board and begins a new session.