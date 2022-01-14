using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LanguageManager : MonoBehaviour,ILanguageManager
{
    [SerializeField] private List<LanguageInfo> _languageInfos;
    private LanguageText _actualLanguageText;
    
    public LanguageManager()
    {
    }
    public LanguageText GetActualLanguageText()
    {
        return _actualLanguageText;
    }

    public void SetActualLanguageText(LanguagesKeys languagesKeyToChange)
    {
        _actualLanguageText = _languageInfos.Single(x => x.LanguagesKey == languagesKeyToChange).LanguageText;
    }
}