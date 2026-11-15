ROLL-A-BALL COIN COLLECTOR  (Unity 6000.3.9f1)

How to open
1. Unzip this folder anywhere (for example in Documents).
2. Open Unity Hub > Add > Add project from disk > choose the RollABall folder.
3. Open it. The first time takes a few minutes.
4. The scene is built for you automatically (Assets/Scenes/RollABall).
   If it is not, use the menu: Tools > Roll-a-Ball > Build Scene.
5. Press Play.

How to play
- Move the ball: W A S D or arrow keys
- Collect all 10 yellow coins to win
- Do not touch the red boxes
- Press R to restart after you win or lose

Scripts (Assets/Scripts)
- PlayerController : moves the ball, picks up coins, checks for obstacles
- Coin             : spins the coin
- Obstacle         : red box, some slide left and right
- CameraFollow     : camera follows the ball
- GameManager      : score, win/lose text, restart
- GameInput        : reads the keyboard
