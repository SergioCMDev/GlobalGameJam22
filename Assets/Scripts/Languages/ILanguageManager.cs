public interface ILanguageManager
{
    LanguageText GetActualLanguageText();
    void SetActualLanguageText(LanguagesKeys languagesKeyToChange);

}