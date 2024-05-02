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

    public GameObject spotlight;
    public bool drawerChecked;
    float drawerMoveDistance = 5.5f; 
    Transform drawerTrans;
    public LayerMask keyLayer, specialWallLayer, noteLayer, tvButtonLayer, waterFaucetLayer;
    public GameObject[] TrueBelievers, SadBois, OverworkedGuys, BossComponents;
    public GameObject textPrefab;
    public Canvas canvas;
    public GameObject boss;
    public GameObject logoText;
    public ParticleSystem logoParticle;
    public TextMeshProUGUI onKeyObtainText;
    public CanvasGroup cg; 
    DoorUICheck DUC;
    private bool mFaded = false;
    public float Duration = 1f;
    public GameObject guestRoomKeyImage;  
    public GameObject[] hintPaperImage;

    public int curSilverKeyNum, curGoldKeyNum;
    public TextMeshProUGUI silverKeyNum, goldKeyNum; 

    [SerializeField] private NoteManager noteManager;

    void Start()
    {
        cam = Camera.main;
        DUC = GetComponent<DoorUICheck>();
    }

    void Update()
    {
        silverKeyNum.text = "x " + curSilverKeyNum.ToString();
        goldKeyNum.text = "x " + curGoldKeyNum.ToString();

        if (Input.GetMouseButtonDown(0))
        {
            if (noteManager.isOpen)
            {
                noteManager.DisableNote();
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
            }
            else if (Physics.Raycast(ray, out hit, 10, keyLayer))
            {
                StartCoroutine(CollectKey(hit.transform.tag));
                onKeyObtainText.text = hit.transform.name.ToString() + " obtained";
                Destroy(hit.collider.gameObject);
            }
            else if (Physics.Raycast(ray, out hit, 10, specialWallLayer))
            {
                CheckSpecialWall(hit.transform.tag);
            }
            else if(Physics.Raycast(ray, out hit, 10, tvButtonLayer))
            {
                LectureVideoPlayerScript videoScript = hit.collider.gameObject.GetComponent<LectureVideoPlayerScript>();
                videoScript.PlayVideo();
                if (!drawerChecked && videoScript.hasKey)
                {
                    drawerTrans = hit.transform.GetChild(0).gameObject.transform;
                    Vector3 targetPos = drawerTrans.localPosition + new Vector3(0, 0, -drawerMoveDistance);
                    StartCoroutine(MoveDrawer(targetPos));
                }
            }
        }
    }

    IEnumerator CollectKey(string keyTag)
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
                ActivateObjects(BossComponents);
                boss.SetActive(true);
                break;
            case "GuestRoomKey":
                DUC.playerHasGuestKey = true;
                guestRoomKeyImage.SetActive(true); 
                StartCoroutine(LogoParticle()); 
                break;
            case "SpecialRoomKey":
                DUC.playerHasSpecialKey = true; 
                break;
            case "Spotlight":
                spotlight.SetActive(true); 
                break;
            case "HintPaper1":
                hintPaperImage[0].SetActive(true); 
                break;
            case "HintPaper2":
                hintPaperImage[1].SetActive(true);
                break;
            case "HintPaper3":
                hintPaperImage[2].SetActive(true);
                break;
            case "HintPaper4":
                hintPaperImage[3].SetActive(true);
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

    void CheckSpecialWall(string wallTag)
    {
        if (goldKeyCollected && redKeyCollected && blueKeyCollected && yellowKeyCollected)
        {
            GameObject winText = Instantiate(textPrefab, canvas.transform, false);
            winText.GetComponent<TextMeshProUGUI>().text = "You escaped!!";

            RectTransform rectTransform = winText.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
