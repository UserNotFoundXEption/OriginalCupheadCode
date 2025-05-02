using System;
using UnityEngine.UI;

// Token: 0x0200046F RID: 1135
public class TextAutoLocalize : Text
{
	// Token: 0x1700037E RID: 894
	// (get) Token: 0x06003025 RID: 12325 RVA: 0x000280C7 File Offset: 0x000262C7
	// (set) Token: 0x06003026 RID: 12326 RVA: 0x000E4C88 File Offset: 0x000E2E88
	public override string text
	{
		get
		{
			return base.text;
		}
		set
		{
			TranslationElement translationElement = Localization.Find(value);
			base.text = ((translationElement == null) ? value : translationElement.translations[(int)Localization.language].text);
		}
	}
}
