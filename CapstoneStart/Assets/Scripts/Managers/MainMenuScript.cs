using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public TMP_FontAsset[] menuFonts;
    public GameObject[] menuText;
    public GameObject menuImage; /*weComeBackText, newGame,
        loadGame, options, exitButton, menuImage;*/
    public bool fontsChanging;
    public string newGameLevel;
    private string levelToLoad;
    //[SerializeField] private GameObject noSavedGameDialog = null;

    private enum FontType
    {
        MomsTypeWriter = 0,
        AuthorizedSignature = 1,
        BadHandwriting = 2,
        Chalkboard = 3,
        HandOfTT = 4,
        handwrited = 5,
        Handwritten = 6,
        IAmTheCrayonMaster = 7,
        LatinaBold = 8,
        LTColoredPencil = 9,
        Lyon = 10,
        MyUnprofessionalHandwriting = 11,
        OldSharpie = 12,
        Quikhand = 13,
        WritingStuff = 14
    }

    FontType fontType;
    RectTransform menuRectTransform;

    void setMenuBoxSize(float menuX)
    {
        menuRectTransform.anchoredPosition = new Vector2(menuX, 170);
        menuRectTransform.sizeDelta = new Vector2(0.35f*menuX, 100);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    /*
    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }
    */

    public void ExitGame()
    {
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        fontType = FontType.MomsTypeWriter;
        menuRectTransform = menuImage.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(fontsChanging)
        {
            int fontInt = Random.Range(0, menuFonts.Length);
            fontType = (FontType)fontInt;

            foreach (GameObject i in menuText)
            {
                i.GetComponent<TextMeshProUGUI>().font =
                    menuFonts[fontInt];
            }
            
            switch(fontType)
            {
                case FontType.AuthorizedSignature:
                    setMenuBoxSize(200);
                    break;
                case FontType.BadHandwriting:
                    setMenuBoxSize(235);
                    break;
                case FontType.Chalkboard:
                    setMenuBoxSize(310);
                    break;
                case FontType.HandOfTT:
                    setMenuBoxSize(270);
                    break;
                case FontType.handwrited:
                    setMenuBoxSize(400);
                    break;
                case FontType.Handwritten:
                    setMenuBoxSize(220);
                    break;
                case FontType.IAmTheCrayonMaster:
                    setMenuBoxSize(300);
                    break;
                case FontType.LatinaBold:
                    setMenuBoxSize(265);
                    break;
                case FontType.LTColoredPencil:
                    setMenuBoxSize(235);
                    break;
                case FontType.Lyon:
                    setMenuBoxSize(265);
                    break;
                case FontType.MyUnprofessionalHandwriting:
                    setMenuBoxSize(220);
                    break;
                case FontType.OldSharpie:
                    setMenuBoxSize(350);
                    break;
                case FontType.Quikhand:
                    setMenuBoxSize(260);
                    break;
                case FontType.WritingStuff:
                    setMenuBoxSize(250);
                    break;
                default:
                    setMenuBoxSize(360);
                    break;
            }
        }  
    }
}
