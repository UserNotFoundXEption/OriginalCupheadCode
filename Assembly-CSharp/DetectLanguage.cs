using System;
using System.Globalization;

// Token: 0x0200046A RID: 1130
public static class DetectLanguage
{
	// Token: 0x06002FFE RID: 12286 RVA: 0x000E342C File Offset: 0x000E162C
	public static Localization.Languages GetDefaultLanguage()
	{
		Localization.Languages result = Localization.Languages.English;
		DetectLanguage.getDefaultLanguage(ref result);
		return result;
	}

	// Token: 0x06002FFF RID: 12287 RVA: 0x000E3444 File Offset: 0x000E1644
	public static void getDefaultLanguage(ref Localization.Languages defaultLanguage)
	{
		CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
		string twoLetterISOLanguageName = currentUICulture.TwoLetterISOLanguageName;
		if (twoLetterISOLanguageName == "fr")
		{
			defaultLanguage = Localization.Languages.French;
		}
		else if (twoLetterISOLanguageName == "de")
		{
			defaultLanguage = Localization.Languages.German;
		}
		else if (twoLetterISOLanguageName == "it")
		{
			defaultLanguage = Localization.Languages.Italian;
		}
		else if (twoLetterISOLanguageName == "ja")
		{
			defaultLanguage = Localization.Languages.Japanese;
		}
		else if (twoLetterISOLanguageName == "zh")
		{
			defaultLanguage = Localization.Languages.SimplifiedChinese;
		}
		else if (twoLetterISOLanguageName == "ru")
		{
			defaultLanguage = Localization.Languages.Russian;
		}
		else if (twoLetterISOLanguageName == "es")
		{
			if (currentUICulture.Name == "es-ES" || currentUICulture.Name == "es")
			{
				defaultLanguage = Localization.Languages.SpanishSpain;
			}
			else
			{
				defaultLanguage = Localization.Languages.SpanishAmerica;
			}
		}
		else if (twoLetterISOLanguageName == "ko")
		{
			defaultLanguage = Localization.Languages.Korean;
		}
		else if (twoLetterISOLanguageName == "po")
		{
			defaultLanguage = Localization.Languages.Polish;
		}
		else if (currentUICulture.Name == "pt-BR")
		{
			defaultLanguage = Localization.Languages.PortugueseBrazil;
		}
		else
		{
			defaultLanguage = Localization.Languages.English;
		}
	}
}
