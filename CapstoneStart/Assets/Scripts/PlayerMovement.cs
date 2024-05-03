using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float normalScale;
    public float crunchScale;
    public float scaleLerpSpeed = 2f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
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
            Crunch(crunchScale);
        }
        else
        {
            speed = normalSpeed;
            Stand(normalScale);
        }

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void Crunch(float targetScale)
    {
        float currentScale = transform.localScale.y;
        float newYScale = Mathf.Lerp(currentScale, targetScale, scaleLerpSpeed * Time.deltaTime);
        Vector3 newScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);

        transform.localScale = newScale;

        if (Mathf.Abs(transform.localScale.y - targetScale) <= 0.05f)
        {
            transform.localScale = new Vector3(transform.localScale.x, targetScale, transform.localScale.z);
        }
    }

    void Stand(float targetScale)
    {
        float currentScale = transform.localScale.y;
        float newYScale = Mathf.Lerp(currentScale, targetScale, scaleLerpSpeed * Time.deltaTime);
        Vector3 newScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);

        transform.localScale = newScale;

        if (Mathf.Abs(transform.localScale.y - targetScale) >= -0.05f)
        {
            transform.localScale = new Vector3(transform.localScale.x, targetScale, transform.localScale.z);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TeleportForward"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else if (other.CompareTag("TeleportBack"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(Mathf.Max(currentSceneIndex - 1, 0));
        }
    }
}
