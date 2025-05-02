using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200046B RID: 1131
public class FontReplacer : MonoBehaviour
{
	// Token: 0x06003001 RID: 12289 RVA: 0x00027F95 File Offset: 0x00026195
	public void Awake()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040027B2 RID: 10162
	public Localization localizationAsset;

	// Token: 0x040027B3 RID: 10163
	public Localization.Languages sourceLanguage;

	// Token: 0x040027B4 RID: 10164
	public Localization.Languages destinationLanguage;

	// Token: 0x040027B5 RID: 10165
	public List<Font> allSourceFonts;

	// Token: 0x040027B6 RID: 10166
	public List<Font> allDestinationFonts;

	// Token: 0x040027B7 RID: 10167
	public List<TMP_FontAsset> allSourceFontAssets;

	// Token: 0x040027B8 RID: 10168
	public List<TMP_FontAsset> allDestinationFontAssets;
}
