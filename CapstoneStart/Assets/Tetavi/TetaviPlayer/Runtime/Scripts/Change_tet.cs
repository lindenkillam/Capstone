using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Change_tet : MonoBehaviour
{
    public TetaviPlayerBase player;
    public InputField tet_path;
    public Toggle is_lit;
    public Material lit;
    public Material unlit;
    public Button exit_btn;
    private bool change;
    // Start is called before the first frame update
    void Start()
    {
        tet_path.onEndEdit.AddListener(SubmitName);
        is_lit.onValueChanged.AddListener(delegate {
            ToggleValueChanged(is_lit);
        });
        exit_btn.onClick.AddListener(delegate
        {
            doExitGame();
        });
    }

    private void SubmitName(string arg0)
    {
        player.ChangeTo(arg0);
        Debug.Log(arg0);
    }

    // Update is called once per frame
    void ToggleValueChanged(Toggle change)
    {
        if (change.isOn)
        {
            player.GetComponent<Renderer>().material = lit;
        }
        else
        {
            player.GetComponent<Renderer>().material = unlit;
        }
    }

    void doExitGame()
    {
        Application.Quit();
    }


}
