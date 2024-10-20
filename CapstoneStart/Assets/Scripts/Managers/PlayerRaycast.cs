using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerRaycast : MonoBehaviour
{
    Camera cam;
    public PlayerMovement PM;
    bool yellowKeyCollected = false;
    bool blueKeyCollected = false;
    bool redKeyCollected = false;
    bool goldKeyCollected = false;

    public bool drawerChecked;
    float drawerMoveDistance = 5.5f; 
    Transform drawerTrans;
    public LayerMask keyLayer, specialWallLayer, noteLayer, tvButtonLayer;
    public GameObject[] TrueBelievers, SadBois, OverworkedGuys, BossComponents;
    public GameObject boss;
    public GameObject logoText;
    public ParticleSystem logoParticle;
    public TextMeshProUGUI onKeyObtainText;
    public CanvasGroup cg; 
    DoorUICheck DUC;
    private bool mFaded = false;
    public float Duration = 1f;
    public GameObject specialRoomKeyImage, guestRoomKeyImage, flashLightImage, ashtrayImage, ashtrayNumText, noteImage;  
    public GameObject[] hintPaperImage;
    public GameObject flashLight;
    public GameObject doorOpenAudio; 
    public Animator pyramidAnim; 
    public int curSilverKeyNum, curGoldKeyNum, curAshtrayNum;
    public TextMeshProUGUI silverKeyNum, goldKeyNum, ashtrayNum;
    public GameObject bagExclamation; 
    bool unlockAshtrayNum; 
    public bool altarCheck;
    public Door door1, door2;
    public bool atHoffman; 
    [SerializeField] private NoteManager noteManager;
    public EventObserver EO;
    bool newItemObtained; 

    void Start()
    {
        cam = Camera.main;
        DUC = GetComponent<DoorUICheck>();
        unlockAshtrayNum = false; 
    }

    void Update()
    {
        silverKeyNum.text = ":" + curSilverKeyNum.ToString() + "/3";
        goldKeyNum.text = ":" + curGoldKeyNum.ToString() + "/1";
        if (unlockAshtrayNum)
        {
            ashtrayNum.text = ":" + curAshtrayNum.ToString() + "/3";
        }
        else
        {
            ashtrayNum.text = ""; 
        }

        if(curAshtrayNum == 3)
        {
            altarCheck = true;
        }
        else
        {
            altarCheck = false;
        }

        if (newItemObtained)
        {
            bagExclamation.SetActive(true);
        }
        else
        {
            bagExclamation.SetActive(false);
        }

        if (EO.bagUIOn)
        {
            newItemObtained = false; 
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (noteManager.isOpen)
            {
                noteManager.StartCoroutine("DisableNote");

                doorOpenAudio.SetActive(true);
                return;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 10, noteLayer))
            {
                var readableItem = hit.collider.GetComponent<NoteManager>();
                if (readableItem != null)
                {
                    noteManager = readableItem;
                }
                noteManager.ShowNote();
                noteImage.SetActive(true);
                door1.requireConditionToOpen = false;
                door2.requireConditionToOpen = false;
            }
            else if (Physics.Raycast(ray, out hit, 10, keyLayer))
            {
                StartCoroutine(CollectKey(hit.transform.tag, hit.transform.gameObject));
                onKeyObtainText.text = hit.transform.name.ToString() + " obtained";
                newItemObtained = true; 
                Destroy(hit.collider.gameObject);
            }
            else if (Physics.Raycast(ray, out hit, 10, specialWallLayer))
            {
                CheckSpecialWall();
            }
            else if (Physics.Raycast(ray, out hit, 10, tvButtonLayer))
            {
                StartCoroutine(CheckTV(hit.transform.tag, hit.transform.gameObject));
            }
        }
    }

    IEnumerator CheckTV(string tvTag, GameObject hitObject)
    {
        switch (tvTag)
        {
            case "TV":
                LectureVideoPlayerScript videoScript = hitObject.GetComponent<Collider>().gameObject.GetComponent<LectureVideoPlayerScript>();
                videoScript.PlayVideo();
                if (!drawerChecked && videoScript.hasKey)
                {
                    drawerTrans = hitObject.transform.GetChild(0).gameObject.transform;
                    Vector3 targetPos = drawerTrans.localPosition + new Vector3(0, 0, -drawerMoveDistance);
                    StartCoroutine(MoveDrawer(targetPos));
                }
                break;
            case "WelcomeTV":
                WelcomeVideoPlayer welcScript = hitObject.GetComponent<Collider>().gameObject.GetComponent<WelcomeVideoPlayer>();
                welcScript.PlayVideo(); 
                break; 
        }
        yield return null;
    }

    IEnumerator CollectKey(string keyTag, GameObject hitObject)
    {
        switch (keyTag)
        {
            case "YellowKey":
                yellowKeyCollected = true;
                curSilverKeyNum += 1; 
                ActivateObjects(SadBois);
                break;
            case "BlueKey":
                blueKeyCollected = true;
                curSilverKeyNum += 1;
                ActivateObjects(OverworkedGuys);
                break;
            case "RedKey":
                redKeyCollected = true;
                curSilverKeyNum += 1;
                ActivateObjects(TrueBelievers);
                break;
            case "GoldKey":
                goldKeyCollected = true;
                curGoldKeyNum += 1;
                //ActivateObjects(BossComponents);
                //boss.SetActive(true);
                break;
            case "GuestRoomKey":
                DUC.playerHasGuestKey = true;
                guestRoomKeyImage.SetActive(true); 
                StartCoroutine(LogoParticle()); 
                break;
            case "SpecialRoomKey":
                DUC.playerHasSpecialKey = true;
                specialRoomKeyImage.SetActive(true);
                DisablePost disablePostOne = hitObject.GetComponent<DisablePost>();
                if (disablePostOne.deletePost)
                {
                    disablePostOne.PostDisable();
                }
                break;
            case "HintPaper1":
                hintPaperImage[0].SetActive(true); 
                break;
            case "HintPaper2":
                hintPaperImage[1].SetActive(true);
                break;
            case "HintPaper3":
                hintPaperImage[2].SetActive(true);
                DisablePost disablePostThree = hitObject.GetComponent<DisablePost>();
                if (disablePostThree.deletePost)
                {
                    disablePostThree.PostDisable();
                }
                break;
            case "HintPaper4":
                hintPaperImage[3].SetActive(true);
                break;
            case "Flashlight":
                flashLight.SetActive(true);
                flashLightImage.SetActive(true);
                break;
            case "Ashtray":
                ashtrayImage.SetActive(true);
                ashtrayNumText.SetActive(true);
                unlockAshtrayNum = true; 
                curAshtrayNum += 1;
                DisablePost disablePostTwo = hitObject.GetComponent<DisablePost>();
                if (disablePostTwo.deletePost)
                {
                    disablePostTwo.PostDisable(); 
                }
                break;
        }

        yield return null; 
        FadeIn();
        yield return new WaitForSeconds(2f);
        FadeOut(); 
    }

    IEnumerator LogoParticle()
    {
        logoText.SetActive(true);
        logoParticle.Play();
        yield return new WaitForSeconds(4.5f);

        logoText.SetActive(false);
    }

    public void FadeIn()
    {
        StartCoroutine(ActionOne(cg, cg.alpha, mFaded ? 0 : 1));
    }

    public void FadeOut()
    {
        StartCoroutine(ActionOne(cg, cg.alpha, mFaded ? 1 : 0));
    }

    public IEnumerator ActionOne(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        yield return new WaitForSeconds(0.5f);
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / Duration);
            yield return null;
        }
    }

    IEnumerator MoveDrawer(Vector3 targetPos)
    {
        drawerChecked = true;

        float duration = 1f;
        float elapsedTime = 0f;
        Vector3 initialPos = drawerTrans.localPosition;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            drawerTrans.localPosition = Vector3.Lerp(initialPos, targetPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the drawer reaches the exact target position
        drawerTrans.localPosition = targetPos;

        yield return new WaitForSeconds(2f);

        // Calculate the new target position for opening the drawer
        Vector3 openTargetPos = initialPos;
        float openElapsedTime = 0f;

        while (openElapsedTime < duration)
        {
            float t = openElapsedTime / duration;
            drawerTrans.localPosition = Vector3.Lerp(targetPos, openTargetPos, t);
            openElapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the drawer reaches the initial position
        drawerTrans.localPosition = initialPos;
        drawerChecked = false;
    }

    void ActivateObjects(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(true);
        }
    }

    void CheckSpecialWall()
    {
        if (goldKeyCollected && redKeyCollected && blueKeyCollected && yellowKeyCollected)
        {
            pyramidAnim.SetTrigger("Trigger");
        }
    }
}
