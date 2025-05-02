using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x02000468 RID: 1128
public class CustomLanguageLayout : MonoBehaviour
{
	// Token: 0x06002FEF RID: 12271 RVA: 0x000E2FBC File Offset: 0x000E11BC
	public void Awake()
	{
		this.rectTransform = base.GetComponent<RectTransform>();
		this.textContainer = base.GetComponent<TextContainer>();
		this.englishBasicLayout = default(CustomLanguageLayout.LanguageLayout);
		this.englishBasicLayout.positionOffset = this.rectTransform.localPosition;
		this.englishBasicLayout.customWidth = this.rectTransform.sizeDelta.x;
		this.englishBasicLayout.customHeight = this.rectTransform.sizeDelta.y;
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x00027EEA File Offset: 0x000260EA
	public void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.ReviewLayout;
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x00027EFD File Offset: 0x000260FD
	public void OnEnable()
	{
		Localization.OnLanguageChangedEvent += this.ReviewLayout;
		this.ReviewLayout();
	}

	// Token: 0x06002FF2 RID: 12274 RVA: 0x00027F16 File Offset: 0x00026116
	public void OnDisable()
	{
		this.ResetToEnglish();
		Localization.OnLanguageChangedEvent -= this.ReviewLayout;
	}

	// Token: 0x06002FF3 RID: 12275 RVA: 0x000E3044 File Offset: 0x000E1244
	public void ReviewLayout()
	{
		if (this.rectTransform == null)
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
			CustomLanguageLayout.LanguageLayout languageLayout = this.customLayouts[num];
			this.ApplylayoutChanges(languageLayout);
		}
		else
		{
			this.ResetToEnglish();
		}
	}

	// Token: 0x06002FF4 RID: 12276 RVA: 0x000E30CC File Offset: 0x000E12CC
	public void ResetToEnglish()
	{
		this.rectTransform.localPosition = this.englishBasicLayout.positionOffset;
		if (this.textContainer != null)
		{
			this.textContainer.height = this.englishBasicLayout.customHeight;
			this.textContainer.width = this.englishBasicLayout.customWidth;
		}
		else
		{
			this.rectTransform.sizeDelta = new Vector2(this.englishBasicLayout.customWidth, this.englishBasicLayout.customHeight);
		}
	}

	// Token: 0x06002FF5 RID: 12277 RVA: 0x000E3158 File Offset: 0x000E1358
	public void ApplylayoutChanges(CustomLanguageLayout.LanguageLayout languageLayout)
	{
		if (languageLayout.needCustomOffset)
		{
			this.rectTransform.localPosition = new Vector3(this.englishBasicLayout.positionOffset.x + languageLayout.positionOffset.x, this.englishBasicLayout.positionOffset.y + languageLayout.positionOffset.y, this.englishBasicLayout.positionOffset.z + languageLayout.positionOffset.z);
		}
		else
		{
			this.rectTransform.localPosition = new Vector3(this.englishBasicLayout.positionOffset.x, this.englishBasicLayout.positionOffset.y, this.englishBasicLayout.positionOffset.z);
		}
		if (this.textContainer != null)
		{
			this.textContainer.width = ((!languageLayout.needCustomWidth) ? this.englishBasicLayout.customWidth : languageLayout.customWidth);
			this.textContainer.height = ((!languageLayout.needCustomHeight) ? this.englishBasicLayout.customHeight : languageLayout.customHeight);
		}
		else
		{
			float num = (!languageLayout.needCustomWidth) ? this.englishBasicLayout.customWidth : languageLayout.customWidth;
			float num2 = (!languageLayout.needCustomHeight) ? this.englishBasicLayout.customHeight : languageLayout.customHeight;
			this.rectTransform.sizeDelta = new Vector2(num, num2);
		}
	}

	// Token: 0x040027AB RID: 10155
	[SerializeField]
	public List<CustomLanguageLayout.LanguageLayout> customLayouts;

	// Token: 0x040027AC RID: 10156
	public RectTransform rectTransform;

	// Token: 0x040027AD RID: 10157
	public CustomLanguageLayout.LanguageLayout englishBasicLayout;

	// Token: 0x040027AE RID: 10158
	public TextContainer textContainer;

	// Token: 0x020010E2 RID: 4322
	[Serializable]
	public struct LanguageLayout
	{
		// Token: 0x04007796 RID: 30614
		public Localization.Languages languageApplied;

		// Token: 0x04007797 RID: 30615
		public bool needCustomOffset;

		// Token: 0x04007798 RID: 30616
		public Vector3 positionOffset;

		// Token: 0x04007799 RID: 30617
		public bool needCustomWidth;

		// Token: 0x0400779A RID: 30618
		public float customWidth;

		// Token: 0x0400779B RID: 30619
		public bool needCustomHeight;

		// Token: 0x0400779C RID: 30620
		public float customHeight;
	}
}
