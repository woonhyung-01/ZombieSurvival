using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Text waveInfoText;
    public Text socreText;

    public GameObject gameOverUI;

    private void OnEnable()
    {
        SetActiveGameOverUI(false);
    }

    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = $"{magAmmo} / {remainAmmo}";
    }

        public void SetWaveText(int wave, int count)
    {
        waveInfoText.text = $"Wave:{wave}\nEnemy Left: {count}";
    }
    
    public void SetScore(int score)
    {
        socreText.text = $"Score: {score}";
    }

    public void SetActiveGameOverUI(bool active)
    {
        gameOverUI.SetActive(active);
    }
}
