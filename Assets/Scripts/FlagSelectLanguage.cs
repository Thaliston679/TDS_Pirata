using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlagSelectLanguage : MonoBehaviour {

    private LanguageManager lang;
    [SerializeField] private string language;

    void Start() {
        lang = LanguageManager.instance;
        GetComponent<Button>().onClick.AddListener(() => { lang.SelectLanguage(language); });
    }
}
