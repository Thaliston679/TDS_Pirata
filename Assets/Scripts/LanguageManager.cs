using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.IO;

public class LanguageManager : MonoBehaviour 
{
    [Header("Languages")]
    public static LanguageManager instance;

    public LanguageReader langReader { get; private set; }
    
    public string currentLanguage = "English";

    public delegate void LanguageChange();
    public static event LanguageChange OnLanguageChange;

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            OpenLocalXML(currentLanguage);
        } 
        else 
        {
            DestroyImmediate(gameObject);
        }
    }

    public void OpenLocalXML(string language) 
    {
        langReader = null;
        currentLanguage = null;

        switch (language) 
        {
            case "English":
                langReader = new LanguageReader(Resources.Load("Lang/ENGLISH") as TextAsset, "English");
                break;
            case "Brazilian":
                langReader = new LanguageReader(Resources.Load("Lang/BRAZILIAN") as TextAsset, "Brazilian");
                break;
            default:
#if UNITY_EDITOR
                Debug.LogWarning("This language doesn't exist: " + language);
#endif
                langReader = new LanguageReader(Resources.Load("Lang/ENGLISH") as TextAsset, "English");
                break;
        }

        currentLanguage = language;
        if (OnLanguageChange != null)
        {
            OnLanguageChange();
        }
    }

    public void SelectLanguage(string language) 
    {
        if (language != currentLanguage) 
        {
            OpenLocalXML(language);
        }
    }

}
