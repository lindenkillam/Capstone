using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed; 
    public float normalSpeed = 5f;
    public float crunchSpeed = 2f;
    public float gravity = -9.81f;

    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;
    private bool isGrounded = true;

    public float normalHeight;
    public float crunchHeight;
    public float heightLerpSpeed = 2f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Movement
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.C))
        {
            speed = crunchSpeed;
            Crunch(crunchHeight);
            //CrunchCenter(1f); 
        }
        else
        {
            speed = normalSpeed;
            Stand(normalHeight);
            //StandCenter(0f); 
        }

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Crunch(float targetHeight)
    {
        float currentHeight = controller.height;
        controller.height = Mathf.Lerp(currentHeight, targetHeight, heightLerpSpeed * Time.deltaTime);
        if(controller.height <= targetHeight + 0.05f)
        {
            controller.height = targetHeight;
        }
    }

    void Stand(float targetHeight)
    {
        float currentHeight = controller.height;
        controller.height = Mathf.Lerp(currentHeight, targetHeight, heightLerpSpeed * Time.deltaTime);
        if (controller.height >= targetHeight - 0.05f)
        {
            controller.height = targetHeight;
        }
    }

    void CrunchCenter(float targetOffset)
    {
        Vector3 newCenter = controller.center;
        newCenter.y = Mathf.Lerp(newCenter.y, targetOffset, heightLerpSpeed * Time.deltaTime);
        controller.center = newCenter;

        if (newCenter.y >= targetOffset - 0.05f)
        {
            newCenter.y = targetOffset;
        }
    }

    void StandCenter(float targetOffset)
    {
        Vector3 newCenter = controller.center;
        newCenter.y = Mathf.Lerp(newCenter.y, targetOffset, heightLerpSpeed * Time.deltaTime);
        controller.center = newCenter;

        if (newCenter.y <= targetOffset + 0.05f)
        {
            newCenter.y = targetOffset;
        }
    }
}
