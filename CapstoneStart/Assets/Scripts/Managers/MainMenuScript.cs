using UnityEngine;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TMP_FontAsset[] menuFonts;
    public GameObject weComeBackText, newGame,
        loadGame, options, credits, menuImage;
    public bool fontsChanging;

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
        menuRectTransform.sizeDelta = new Vector2(0.35f*menuX, menuRectTransform.rect.height);
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

            weComeBackText.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontInt];
            newGame.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontInt];
            loadGame.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontInt];
            options.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontInt];
            credits.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontInt];
            
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
