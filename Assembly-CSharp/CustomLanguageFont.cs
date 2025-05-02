using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000467 RID: 1127
public class CustomLanguageFont : MonoBehaviour
{
	// Token: 0x06002FE7 RID: 12263 RVA: 0x000E2D7C File Offset: 0x000E0F7C
	public void Awake()
	{
		this.textMeshProComponent = base.GetComponent<TMP_Text>();
		this.textComponent = base.GetComponent<Text>();
		this.englishBasicFont = default(CustomLanguageFont.LanguageFont);
		this.englishBasicFont.characterSpacing = this.textMeshProComponent.characterSpacing;
		this.englishBasicFont.lineSpacing = this.textMeshProComponent.lineSpacing;
		this.englishBasicFont.paragraphSpacing = this.textMeshProComponent.paragraphSpacing;
		this.englishBasicFont.needFontSize = true;
		this.englishBasicFont.customFontSize = this.textMeshProComponent.fontSize;
		this.englishBasicFont.needKerning = this.textMeshProComponent.enableKerning;
	}

	// Token: 0x06002FE8 RID: 12264 RVA: 0x00027E71 File Offset: 0x00026071
	public void Start()
	{
		Localization.OnLanguageChangedEvent += this.ReviewFont;
		this.ReviewFont();
	}

	// Token: 0x06002FE9 RID: 12265 RVA: 0x00027E8A File Offset: 0x0002608A
	public void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.ReviewFont;
	}

	// Token: 0x06002FEA RID: 12266 RVA: 0x00027E9D File Offset: 0x0002609D
	public void OnEnable()
	{
		this.ReviewFont();
	}

	// Token: 0x06002FEB RID: 12267 RVA: 0x000E2E2C File Offset: 0x000E102C
	public void ReviewFont()
	{
		if (this.textMeshProComponent == null && this.textComponent == null)
		{
			return;
		}
		int num = 0;
		bool flag = false;
		while (!flag && num < this.customLayouts.Count)
		{
			flag = (this.customLayouts[num].languageApplied == Localization.language);
			num++;
		}
		num--;
		if (flag)
		{
			if (this.customLayouts[num].needSpacing)
			{
				this.ApplySpacingChanges(this.customLayouts[num]);
			}
			if (this.customLayouts[num].needFontSize)
			{
				this.ApplyFontSizeChanges(this.customLayouts[num]);
			}
			this.textMeshProComponent.enableKerning = this.customLayouts[num].needKerning;
		}
		else
		{
			this.ApplySpacingChanges(this.englishBasicFont);
			this.ApplyFontSizeChanges(this.englishBasicFont);
			this.textMeshProComponent.enableKerning = this.englishBasicFont.needKerning;
		}
	}

	// Token: 0x06002FEC RID: 12268 RVA: 0x000E2F50 File Offset: 0x000E1150
	public void ApplySpacingChanges(CustomLanguageFont.LanguageFont languageLayout)
	{
		if (this.textMeshProComponent != null)
		{
			this.textMeshProComponent.characterSpacing = languageLayout.characterSpacing;
			this.textMeshProComponent.lineSpacing = languageLayout.lineSpacing;
			this.textMeshProComponent.paragraphSpacing = languageLayout.paragraphSpacing;
		}
		else
		{
			this.textComponent.lineSpacing = languageLayout.lineSpacing;
		}
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x00027EA5 File Offset: 0x000260A5
	public void ApplyFontSizeChanges(CustomLanguageFont.LanguageFont languageLayout)
	{
		if (this.textMeshProComponent != null)
		{
			this.textMeshProComponent.fontSize = languageLayout.customFontSize;
		}
		else
		{
			this.textComponent.fontSize = (int)languageLayout.customFontSize;
		}
	}

	// Token: 0x040027A7 RID: 10151
	[SerializeField]
	public List<CustomLanguageFont.LanguageFont> customLayouts;

	// Token: 0x040027A8 RID: 10152
	public CustomLanguageFont.LanguageFont englishBasicFont;

	// Token: 0x040027A9 RID: 10153
	public TMP_Text textMeshProComponent;

	// Token: 0x040027AA RID: 10154
	public Text textComponent;

	// Token: 0x020010E1 RID: 4321
	[Serializable]
	public struct LanguageFont
	{
		// Token: 0x0400778E RID: 30606
		public Localization.Languages languageApplied;

		// Token: 0x0400778F RID: 30607
		public bool needSpacing;

		// Token: 0x04007790 RID: 30608
		public float characterSpacing;

		// Token: 0x04007791 RID: 30609
		public float lineSpacing;

		// Token: 0x04007792 RID: 30610
		public float paragraphSpacing;

		// Token: 0x04007793 RID: 30611
		public bool needFontSize;

		// Token: 0x04007794 RID: 30612
		public float customFontSize;

		// Token: 0x04007795 RID: 30613
		public bool needKerning;
	}
}
