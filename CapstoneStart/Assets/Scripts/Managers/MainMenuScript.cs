using System.Collections;
using System.Collections.Generic;
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
        Lilly = 9,
        LTColoredPencil = 10,
        Lyon = 11,
        MyUnprofessionalHandwriting = 12,
        OldSharpie = 13,
        Quikhand = 14,
        SummerPisces = 15,
        WritingStuff = 16
    }

    FontType fontType;

    // Start is called before the first frame update
    void Start()
    {
        fontType = FontType.MomsTypeWriter;
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
                    break;
                case FontType.BadHandwriting:
                    break;
                case FontType.Chalkboard:
                    break;
                case FontType.HandOfTT:
                    break;
                case FontType.handwrited:
                    break;
                case FontType.Handwritten:
                    break;
                case FontType.IAmTheCrayonMaster:
                    break;
                case FontType.LatinaBold:
                    break;
                case FontType.Lilly:
                    break;
                case FontType.LTColoredPencil:
                    break;
                case FontType.Lyon:
                    break;
                case FontType.MyUnprofessionalHandwriting:
                    break;
                case FontType.OldSharpie:
                    break;
                case FontType.Quikhand:
                    break;
                case FontType.SummerPisces:
                    break;
                case FontType.WritingStuff:
                    break;
                default:
                    break;
            }
        }  
    }
}
