using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uIManager;
    public ZombieSpawner zombieSpawner;
    public ItemSpawner itemSpawner;

    private int score = 0;
    public bool IsGameOver {get; private set;}

    private void Start()
    {
        uIManager.SetScore(score);
    }

    public void AddScore(int add)
    {
        if(IsGameOver)
        {
            return;
        }

        score += add;
        uIManager.SetScore(score);
    }

    public void EndGame()
    {
        IsGameOver = true;
        uIManager.SetActiveGameOverUI(true);

        itemSpawner.enabled = false;
        zombieSpawner.OnEndGame();
        zombieSpawner.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

}
