using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextM : MonoBehaviour
{
    public InputField search;
    public Text show;
 

    public void Textsearch()
    {
        
        if (search.text == "IU")
        {
            show.text = search.text +  "  is found.";
        }
        else if (search.text == "Sodiac")
        {
            show.text = search.text + "  is found.";
        }
        else if (search.text == "Hawk")
        {
            show.text = search.text + "  is found.";
        }
        else if (search.text == "Meliodas")
        {
            show.text = search.text + "  is found.";
        }
        else if (search.text == "Rimuru")
        {
            show.text = search.text + "  is found.";
        }
        else
        {
            show.text = search.text + "  not found.";
        }

    }

}
