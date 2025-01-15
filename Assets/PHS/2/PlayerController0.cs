using UnityEngine;

public class PlayerController0 : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public Animator playerAnimator;
    public float maxForce = 10f;

    private JoystickInput joystickInput;

    void Start()
    {
        joystickInput = GetComponent<JoystickInput>();
    }

    void Update()
    {
        joystickInput.ProcessInput();

        if (Input.GetMouseButtonUp(0))
        {
            playerAnimator.SetTrigger("Attack");
            Vector3 launchDirection = new Vector3(joystickInput.CurrentDirection.x, 0, joystickInput.CurrentDirection.y) * joystickInput.DragValue * maxForce;
            playerRigidbody.AddForce(launchDirection, ForceMode.Impulse);
        }
    }
}
