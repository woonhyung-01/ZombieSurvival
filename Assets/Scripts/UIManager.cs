using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text ammoText;
    public Text waveInfoText;

    private void OnEnable()
    {
        SetAmmoText(0, 0);
    }

    public void SetAmmoText(int magAmmo, int remainAmmo)
    {
        ammoText.text = $"{magAmmo} / {remainAmmo}";
    }

        public void SetWaveText(int wave, int count)
    {
        waveInfoText.text = $"Wave:{wave}\nEnemy Left: {count}";
    }
}
