using System;
using UnityEngine;

// Token: 0x020000F5 RID: 245
public class SymbolPicker : MonoBehaviour
{
	// Token: 0x06000B8E RID: 2958 RVA: 0x0000A4F9 File Offset: 0x000086F9
	public void OnEnable()
	{
		this.ApplySymbol();
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0007FD44 File Offset: 0x0007DF44
	public void ApplySymbol()
	{
		TranslationElement translationElement = Localization.Find(this.button.ToString());
		this.localizationHelper.ApplyTranslation(translationElement, null);
	}

	// Token: 0x0400092B RID: 2347
	[SerializeField]
	public LocalizationHelper localizationHelper;

	// Token: 0x0400092C RID: 2348
	public CupheadButton button;
}
