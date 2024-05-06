using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class HandleController : MonoBehaviour
{
    public AudioSource deviceSound;
    public AudioClip[] clips; 
    public GameObject hitZone;
    public float rotationSpeed = 70f; 
    public GameObject[] hitPos;
    bool isHit = false; 
    public int isHitCount; 
    public Quaternion originalRotation;
    public HoffmanDeviceController HDC;
    public MouseLook ML; 
    public CharacterController characterC;
    public EventObserver EO;
    public GameObject cam; 
    int totalRotationCount;
    int displayRotationCount;
    public TextMeshProUGUI displayCount, cleansingText;
    public Animator anim;
    public Collider hoffmanCollider;
    public bool resetEverything;
    public CanvasGroup cg;
    private bool isRotatingForward = false;
    private bool stopRotating = false;
    bool playingAnim;
    public CheckChild CC;
    public AudioSource denySound;
    bool usedOnce; 

    void Start()
    {
        originalRotation = transform.localRotation;
        isHitCount = 0; 
        totalRotationCount = 0;
        displayRotationCount = 3;
        displayCount.text = displayRotationCount.ToString() + "/3"; ;
    }

    void Update()
    {
        if (HDC.cleansingCanStart)
        {
            resetEverything = false; 
            characterC.enabled = false;
            ML.enabled = false;
            EO.canCheckBag = false;
            cg.alpha = 0.2f; 
            cam.transform.localRotation = Quaternion.identity;

            if (!playingAnim && !isRotatingForward && Input.GetKeyDown(KeyCode.E))
            {
                deviceSound.clip = clips[0];
                deviceSound.Play();
                isHitCount = 0;
                SetInitialRandomHitZonePosition();
                StartHandleRotation();
             }
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && isHit && !playingAnim && isRotatingForward)
        {
            isHitCount += 1;
            deviceSound.clip = clips[1];
            deviceSound.Play();
            SetRandomHitZonePosition();
            Debug.Log("Hit once!");
            if (isHitCount == 2)
            {
                StopHandleRotation(); // Call StopHandleRotation when isHitCount reaches 2
                CC.CleansingTexts(); 
                StartCoroutine(PlayAnimation());
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!usedOnce)
            {
                HDC.TurnOffHoffman();
            }
            else
            {
                resetEverything = true; 
                HDC.TurnOffHoffman();
            }
        }
    }

    public void Reset()
    {
        cleansingText.text = "";
        EO.canCheckBag = true;
        characterC.enabled = true;
        ML.enabled = true;
        cg.alpha = 1f;
        isHitCount = 0;
        usedOnce = false; 
        isRotatingForward = false;
        hoffmanCollider.enabled = true;
        transform.localRotation = originalRotation; 
    }

    void SetInitialRandomHitZonePosition()
    {
        int randomIndex = Random.Range(1, 4);
        hitZone.transform.localPosition = hitPos[randomIndex].transform.localPosition;
        hitZone.transform.localRotation = hitPos[randomIndex].transform.localRotation;
        hitZone.SetActive(true);
    }

    void SetRandomHitZonePosition()
    {
        ShuffleArray(hitPos);

        hitZone.transform.localPosition = hitPos[0].transform.localPosition;
        hitZone.transform.localRotation = hitPos[0].transform.localRotation;
        hitZone.SetActive(true);
    }

    void ShuffleArray(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    void StartHandleRotation()
    {
        isHitCount = 0; // Reset isHitCount here
        StartCoroutine(RotateForward());
    }

    void StopHandleRotation()
    {
        totalRotationCount += 1;
        displayRotationCount -= 1;
        displayCount.text = displayRotationCount.ToString() + "/3";
        transform.localRotation = originalRotation;
        stopRotating = true; // Set stopRotating to true
    }

    IEnumerator PlayAnimation()
    {
        usedOnce = true; 
        anim.SetTrigger("Trig");
        playingAnim = true; 
        cleansingText.text = "Cleasing soul..."; 
        yield return new WaitForSeconds(5f);
        cleansingText.text = "Soul cleanse process over, " + displayRotationCount.ToString() + " rounds left";
        yield return new WaitForSeconds(2f);
        cleansingText.text = "";
        isRotatingForward = false; 
        playingAnim = false;

        if (totalRotationCount == 3 || displayRotationCount == 0)
        {
            resetEverything = true;
        }
    }

    IEnumerator RotateForward()
    {
        isRotatingForward = true; // Start rotation
        stopRotating = false; 
        while (isRotatingForward && !stopRotating) // Check if we should stop rotation
        {
            transform.Rotate(-Vector3.forward, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isRotatingForward && other.gameObject == hitZone)
        {
            isHit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isRotatingForward && other.gameObject == hitZone)
        {
            isHit = false;
        }
    }
}
