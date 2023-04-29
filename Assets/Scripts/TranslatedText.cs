using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TranslatedText : MonoBehaviour {

    private LanguageManager lang;
    private TextMeshProUGUI myText;

    [Tooltip("O nome da string(Índice) no campo XML.")]
    [SerializeField] private string param;
    
	void Start () {
        lang = LanguageManager.instance;
        LanguageManager.OnLanguageChange += OnLanguageChange;
        myText = GetComponent<TextMeshProUGUI>();
        //Every time that we 
        if (lang != null && lang.langReader != null) {
            UpdateText();
        }
    }
    
    void OnLanguageChange () {
        if (lang != null && lang.langReader != null) {
            UpdateText();
        }

	}

    void UpdateText() {
        if (myText != null) {
            myText.text = lang.langReader.getString(param);
        } else if (GetComponent<TextMesh>() != null)
            GetComponent<TextMesh>().text = lang.langReader.getString(param);
    }

    private void OnDestroy() {
        LanguageManager.OnLanguageChange -= OnLanguageChange;
    }
}
