using System;
using System.Collections;
using System.Xml;
using UnityEngine;
using System.Text.RegularExpressions;

public class LanguageReader 
{
    
    Hashtable XML_Strings;

    public LanguageReader(TextAsset xmlFile, string language) 
    {
        SetLocalLanguage(xmlFile.text, language);
    }

    public void SetLocalLanguage(string xmlContent, string language) 
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlContent);

        var comments = xml.SelectNodes("//comment()");
        foreach (XmlNode comment in comments)
        {
            comment.ParentNode.RemoveChild(comment);
        }

        XML_Strings = new Hashtable();
        XmlElement element = xml.DocumentElement[language];
        if (element != null) 
        {
            var elemEnum = element.GetEnumerator();

            while (elemEnum.MoveNext()) 
            {
                var elem = elemEnum.Current as XmlElement;
                string name = elem.GetAttribute("name");
                string text = elem.InnerText.Replace(@"\n", Environment.NewLine);

                Regex colorTagRegex = new Regex("§color=#([0-9A-Fa-f]{6})§");
                text = colorTagRegex.Replace(text, "<color=#$1>");
                text = text.Replace("§/color§", "</color>");

                XML_Strings.Add(name, text);
            }
        } 
        else 
        { 
            Debug.LogError("O idioma especificado não existe: " + language);
        }
    }

    public string getString(string _name) 
    {
        if (!XML_Strings.ContainsKey(_name)) 
        {
            Debug.LogWarning("Esta string não está presente no arquivo XML onde você está lendo: " + _name);
            return "";
        }
        return (string)XML_Strings[_name];
    }

}