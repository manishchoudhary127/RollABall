using UnityEngine;
using UnityEngine.SceneManagement;

// Keeps the score, shows the text on screen and restarts the game.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // so other scripts can find it easily

    public bool IsGameOver;

    private int score;
    private int totalCoins;
    private string message = "";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Count how many coins are in the scene.
        totalCoins = FindObjectsByType<Coin>(FindObjectsSortMode.None).Length;
    }

    void Update()
    {
        if (IsGameOver && GameInput.RestartPressed())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void CoinCollected()
    {
        score = score + 1;

        if (score >= totalCoins)
        {
            IsGameOver = true;
            message = "You Win!  Press R to play again";
        }
    }

    public void GameOver()
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
        message = "Game Over!  Press R to try again";
    }

    // Draws simple text on the screen.
    void OnGUI()
    {
        GUIStyle small = new GUIStyle(GUI.skin.label);
        small.fontSize = 28;
        small.normal.textColor = Color.white;
        GUI.Label(new Rect(20, 20, 400, 40), "Coins: " + score + " / " + totalCoins, small);

        if (message != "")
        {
            GUIStyle big = new GUIStyle(GUI.skin.label);
            big.fontSize = 44;
            big.alignment = TextAnchor.MiddleCenter;
            big.normal.textColor = Color.yellow;
            GUI.Label(new Rect(0, Screen.height / 2 - 40, Screen.width, 80), message, big);
        }
    }
}
