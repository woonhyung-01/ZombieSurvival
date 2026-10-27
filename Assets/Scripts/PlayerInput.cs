using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string MoveAxis = "Vertical";
    public static readonly string RotateAxis = "Horizontal";
    public static readonly string FireAxis = "Fire1";
    public static readonly string ReloadAxis = "Reload";
    public float Move {get; private set;}
    public float Rotate {get; private set;}
    public bool Fire {get; private set;}
    public bool Reload {get; private set;}

    private void OnDisable()
    {
        Move = 0f;
        Rotate = 0f;
        Fire = false;
        Reload = false;
    }

    private void Update()
    {
        Move = Input.GetAxis(MoveAxis);
        Rotate = Input.GetAxis(RotateAxis);
        Fire = Input.GetButton(FireAxis);
        Reload = Input.GetButtonDown(ReloadAxis);

    }
}
