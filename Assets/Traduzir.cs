using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Traduzir : MonoBehaviour
{
    public PtBr traduz;
    // Start is called before the first frame update
    string texto;
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = PegTexto();
    }
    
    string PegTexto()
    {
        return traduz.texto;
    }

}
