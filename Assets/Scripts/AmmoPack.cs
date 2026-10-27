using UnityEngine;
using UnityEngine.Events;

public class AmmoPack : MonoBehaviour, IItem
{
    public int ammo = 25;
    public UnityEvent OnUse { get; set;} = new UnityEvent();
    public void Use(GameObject target)
    {
        PlayerShooter playerShooter = target.GetComponent<PlayerShooter>();
        if (playerShooter == null || playerShooter.gun == null)
        {
            return;
        }
        playerShooter.gun.AddAmmo(ammo);
        gameObject.SetActive(false);
        OnUse.Invoke();
    }
}
