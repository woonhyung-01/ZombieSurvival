using UnityEngine;
using UnityEngine.Events;

public class HealthItem : MonoBehaviour, IItem
{
    private float healthAdd = 35f;

    public UnityEvent OnUse { get; set;} = new UnityEvent();

    public void Use(GameObject target)
    {
        LivingEntity living = target.GetComponent<LivingEntity>();
        if(living != null)
        {
            living.OnHeal(healthAdd);
            
        }
        gameObject.SetActive(false);
        OnUse.Invoke();
    }
}
