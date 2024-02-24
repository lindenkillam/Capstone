using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 1f;
    public LayerMask groundMask;
    Vector3 velocity;
    public LayerMask keyLayer, specialWallLayer;
    bool isGrounded = true;
    bool yellowKeyCollected = false;
    bool blueKeyCollected = false;
    bool redKeyCollected = false;
    bool blueKeySpawned = false;
    bool redKeySpawned = false;
    public GameObject yellowWall, blueWall, redWall;
    public GameObject blueKey, redKey;
    public GameObject textPrefab;
    public Canvas canvas;
    //private void logDestroy(out hit, bool shouldDestroy);

    Camera cam;

    void Start()
    {
        cam = Camera.main;
        Debug.Log(cam.name);
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        //Raycasting
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 20, keyLayer))
            {
                if(hit.transform.CompareTag("YellowKey"))
                {
                    yellowKeyCollected = true;
                }
                else if(hit.transform.CompareTag("BlueKey"))
                {
                    blueKeyCollected = true;
                }
                else if(hit.transform.CompareTag("RedKey"))
                {
                    redKeyCollected = true;
                }

                //logDestroy(hit, true);
                Debug.Log(hit.transform.name);
                Destroy(hit.collider.gameObject);
                //hit.transform.GetComponent<Renderer>().material.color = Color.white;
            }
            else if(Physics.Raycast(ray, out hit, 20, specialWallLayer) && yellowKeyCollected)
            {
                //if(hit.collider.GameObject == yellowWall)
                if(hit.transform.CompareTag("YellowWall") && !blueKeySpawned)
                {
                    Instantiate(blueKey, yellowWall.transform.position + new Vector3(-0.5f,0.5f,0f), Quaternion.identity);
                    blueKeySpawned = true;
                }
                else if(hit.transform.CompareTag("BlueWall") && blueKeyCollected && !redKeySpawned)
                {
                    Instantiate(redKey, blueWall.transform.position + new Vector3(0.5f,0.5f,0f), Quaternion.identity);
                    redKeySpawned = true;
                }
                else if(hit.transform.CompareTag("RedWall") && redKeyCollected)
                {
                    GameObject winText = Instantiate(textPrefab, canvas.transform, false);
                    winText.GetComponent<TextMeshProUGUI>().text = "You escaped!!";
            
                    winText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); // anchor at center
                    winText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f); // anchor at center
                    winText.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f); // pivot at center
                    winText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                }

                Debug.Log(hit.transform.name);
                //logDestroy(hit, false);
            }
        }
    }

    /*
    private void logDestroy(out hit, bool shouldDestroy)
    {
        Debug.Log(hit.transform.name);
        if(shouldDestroy)
            Destroy(hit.collider.gameObject);
    }
    */
}