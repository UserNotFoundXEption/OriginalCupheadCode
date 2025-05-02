using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000469 RID: 1129
public class CustomLanguageLayoutGroup : MonoBehaviour
{
	// Token: 0x06002FF7 RID: 12279 RVA: 0x000E32EC File Offset: 0x000E14EC
	public void Awake()
	{
		this.englishBasicLayout = default(CustomLanguageLayoutGroup.LanguageLayoutGroup);
		this.englishBasicLayout.needPadding = true;
		this.englishBasicLayout.padding = this.layoutComponent.padding;
		this.englishBasicLayout.needSpacing = true;
		this.englishBasicLayout.spacing = this.layoutComponent.spacing;
	}

	// Token: 0x06002FF8 RID: 12280 RVA: 0x00027F37 File Offset: 0x00026137
	public void Start()
	{
		Localization.OnLanguageChangedEvent += this.ReviewLayout;
	}

	// Token: 0x06002FF9 RID: 12281 RVA: 0x00027F4A File Offset: 0x0002614A
	public void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.ReviewLayout;
	}

	// Token: 0x06002FFA RID: 12282 RVA: 0x00027F5D File Offset: 0x0002615D
	public void OnEnable()
	{
		this.ReviewLayout();
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x000E334C File Offset: 0x000E154C
	public void ReviewLayout()
	{
		if (this.layoutComponent == null)
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
			if (this.customLayouts[num].needPadding)
			{
				this.ApplyPaddingChanges(this.customLayouts[num]);
			}
		}
		else
		{
			this.ApplySpacingChanges(this.englishBasicLayout);
			this.ApplyPaddingChanges(this.englishBasicLayout);
		}
	}

	// Token: 0x06002FFC RID: 12284 RVA: 0x00027F65 File Offset: 0x00026165
	public void ApplySpacingChanges(CustomLanguageLayoutGroup.LanguageLayoutGroup languageLayout)
	{
		this.layoutComponent.spacing = languageLayout.spacing;
	}

	// Token: 0x06002FFD RID: 12285 RVA: 0x00027F79 File Offset: 0x00026179
	public void ApplyPaddingChanges(CustomLanguageLayoutGroup.LanguageLayoutGroup languageLayout)
	{
		this.layoutComponent.padding = languageLayout.padding;
	}

	// Token: 0x040027AF RID: 10159
	[SerializeField]
	public HorizontalOrVerticalLayoutGroup layoutComponent;

	// Token: 0x040027B0 RID: 10160
	[SerializeField]
	public List<CustomLanguageLayoutGroup.LanguageLayoutGroup> customLayouts;

	// Token: 0x040027B1 RID: 10161
	public CustomLanguageLayoutGroup.LanguageLayoutGroup englishBasicLayout;

	// Token: 0x020010E3 RID: 4323
	[Serializable]
	public struct LanguageLayoutGroup
	{
		// Token: 0x0400779D RID: 30621
		public Localization.Languages languageApplied;

		// Token: 0x0400779E RID: 30622
		public bool needPadding;

		// Token: 0x0400779F RID: 30623
		public RectOffset padding;

		// Token: 0x040077A0 RID: 30624
		public bool needSpacing;

		// Token: 0x040077A1 RID: 30625
		public float spacing;
	}
}
