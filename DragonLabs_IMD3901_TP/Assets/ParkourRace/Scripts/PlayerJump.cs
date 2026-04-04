using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public float jumpHeight = 1.2f;
    public float gravity = -20f;
    public float groundCheckDistance = 0.2f;

    private CharacterController controller;
    private float verticalVelocity;

    public float jumpCooldown = 1f;
    private float jumpCooldownTimer = 0f;

    public AudioManagerSinglePlayer jumpAudio;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //jumpAudio = FindFirstObjectByType<AudioManagerSinglePlayer>();
    }

    void Update()
    {
        if (jumpAudio == null)
        {
            jumpAudio = FindFirstObjectByType<AudioManagerSinglePlayer>();
        }
        else if (jumpAudio!=null)
        {
            if (jumpCooldownTimer > 0f)
                jumpCooldownTimer -= Time.deltaTime;

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame && jumpCooldownTimer <= 0f)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
                jumpCooldownTimer = jumpCooldown;
                jumpAudio.PlaySFX(jumpAudio.parkour_jumpSound);
            }

            verticalVelocity += gravity * Time.deltaTime;
            controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
        }
    }
}
