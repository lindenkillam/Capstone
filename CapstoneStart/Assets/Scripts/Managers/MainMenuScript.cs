using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TMP_FontAsset[] menuFonts;
    public GameObject weComeBackText, newGame,
        loadGame, options, credits, menuImage;
    int fontNum;
    public bool fontsChanging;

    // Start is called before the first frame update
    void Start()
    {
        fontNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(fontsChanging)
        {
            fontNum = Random.Range(0, menuFonts.Length);
            weComeBackText.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontNum];
            newGame.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontNum];
            loadGame.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontNum];
            options.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontNum];
            credits.GetComponent<TextMeshProUGUI>().font =
                menuFonts[fontNum];
        }  
    }
}
