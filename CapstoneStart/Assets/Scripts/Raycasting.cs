using UnityEngine;
using TMPro;

public class Raycasting : MonoBehaviour
{
    Camera cam;
    bool yellowKeyCollected = false;
    bool blueKeyCollected = false;
    bool redKeyCollected = false;
    bool goldKeyCollected = false;
    //public GameObject yellowWall, blueWall, redWall;
    public LayerMask keyLayer, specialWallLayer, noteLayer;
    //public GameObject blueKey, redKey;
    public GameObject[] TrueBelievers, SadBois, OverworkedGuys, BossComponents;
    public GameObject textPrefab;
    public Canvas canvas;
    public GameObject boss;
    [SerializeField] private NoteManager noteManager;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        //boss = GameObject.FindWithTag("Boss");
    }

    // Update is called once per frame
    void Update()
    {
        //Raycasting
        if(Input.GetMouseButtonDown(0))
        {
            if(noteManager.isOpen)
            {
                noteManager.DisableNote();
                return;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 10, noteLayer))
            {
                var readableItem = hit.collider.GetComponent<NoteManager>();
                if(readableItem != null)
                {
                    noteManager = readableItem;
                    //Make an item aura, or highlight crosshair or something
                }

                noteManager.ShowNote();
            }
            else if(Physics.Raycast(ray, out hit, 20, keyLayer))
            {
                if(hit.transform.CompareTag("YellowKey"))
                {
                    yellowKeyCollected = true;
                    for(int i = 0; i < SadBois.Length; ++i)
                    {
                        SadBois[i].SetActive(true);
                    }
                }
                else if(hit.transform.CompareTag("BlueKey"))
                {
                    blueKeyCollected = true;
                    for(int i = 0; i < OverworkedGuys.Length; ++i)
                    {
                        OverworkedGuys[i].SetActive(true);
                    }
                }
                else if(hit.transform.CompareTag("RedKey"))
                {
                    redKeyCollected = true;
                    for(int i = 0; i < TrueBelievers.Length; ++i)
                    {
                        TrueBelievers[i].SetActive(true);
                    }
                }
                else if(hit.transform.CompareTag("GoldKey"))
                {
                    goldKeyCollected = true;
                    boss.SetActive(true);
                    for (int i = 0; i < BossComponents.Length; ++i)
                    {
                        BossComponents[i].SetActive(true);
                    }
                }
                Debug.Log(hit.transform.name);
                Destroy(hit.collider.gameObject);
                //hit.transform.GetComponent<Renderer>().material.color = Color.white;
            }
            else if(Physics.Raycast(ray, out hit, 20, specialWallLayer))
            {
                if(goldKeyCollected && redKeyCollected && blueKeyCollected && yellowKeyCollected)
                {
                    GameObject winText = Instantiate(textPrefab, canvas.transform, false);
                    winText.GetComponent<TextMeshProUGUI>().text = "You escaped!!";
                
                    winText.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); // anchor at center
                    winText.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f); // anchor at center
                    winText.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f); // pivot at center
                    winText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                }
                /*
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
                */
            }
        }
    }
}