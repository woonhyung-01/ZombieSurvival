using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public static readonly int HashMove = Animator.StringToHash("Move");
    public float moveSpeed = 5f;
    public float rotateSpeed = 180f;

    private Animator playerAnimator;
    private PlayerInput playerInput;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        playerAnimator.SetFloat(HashMove, playerInput.Move);
    }

    private void FixedUpdate()
    {
        float angle = playerInput.Rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(0f, angle, 0f));

        Vector3 delta = playerInput.Move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + delta);

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            IItem item = other.GetComponent<IItem>();
            if(item != null)
            {
                item.Use(gameObject);
            }
        }      
    }
}
