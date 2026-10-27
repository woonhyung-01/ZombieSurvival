using UnityEngine;
using UnityEngine.Events;

public interface IItem
{
    UnityEvent OnUse {get; set;}
    void Use(GameObject target);
}
