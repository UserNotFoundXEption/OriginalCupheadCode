using System;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x0200046D RID: 1133
public class LocalizationHelper : MonoBehaviour
{
	// Token: 0x0600301A RID: 12314 RVA: 0x000E4670 File Offset: 0x000E2870
	public void Init()
	{
		if (this.textComponent != null)
		{
			this.initialFontSize = this.textComponent.fontSize;
		}
		if (this.textMeshProComponent != null)
		{
			this.initialFontAssetSize = this.textMeshProComponent.fontSize;
		}
		this.isInit = true;
	}

	// Token: 0x0600301B RID: 12315 RVA: 0x00028059 File Offset: 0x00026259
	public void Awake()
	{
		this.platformOverride = base.GetComponent<LocalizationHelperPlatformOverride>();
		this.hasOverride = (this.platformOverride != null);
	}

	// Token: 0x0600301C RID: 12316 RVA: 0x00028079 File Offset: 0x00026279
	public void Start()
	{
		Localization.OnLanguageChangedEvent += this.ApplyTranslation;
	}

	// Token: 0x0600301D RID: 12317 RVA: 0x0002808C File Offset: 0x0002628C
	public void OnDestroy()
	{
		Localization.OnLanguageChangedEvent -= this.ApplyTranslation;
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x0002809F File Offset: 0x0002629F
	public void OnEnable()
	{
		this.ApplyTranslation();
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x000E46C8 File Offset: 0x000E28C8
	public void ApplyTranslation()
	{
		int id = this.currentID;
		int num;
		if (this.hasOverride && this.platformOverride.HasOverrideForCurrentPlatform(out num))
		{
			id = num;
		}
		this.ApplyTranslation(Localization.Find(id));
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x000280A7 File Offset: 0x000262A7
	public void ApplyTranslation(TranslationElement translationElement, LocalizationHelper.LocalizationSubtext[] subTranslations = null)
	{
		this.subTranslations = subTranslations;
		this.ApplyTranslation(translationElement);
	}

	// Token: 0x06003021 RID: 12321 RVA: 0x000E4708 File Offset: 0x000E2908
	public void ApplyTranslation(TranslationElement translationElement)
	{
		if (!this.isInit)
		{
			this.Init();
		}
		this.currentLanguage = Localization.language;
		if (this.currentLanguage == (Localization.Languages)(-1) || translationElement == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(translationElement.key))
		{
			return;
		}
		Localization.Translation translation = translationElement.translation;
		if (string.IsNullOrEmpty(translation.text))
		{
			translation = Localization.Translate(translationElement.key);
		}
		string text = translation.text;
		if (text != null)
		{
			text = text.Replace("\\n", "\n");
		}
		if (text != null && text.Contains("{") && text.Contains("}"))
		{
			if (this.subTranslations != null)
			{
				bool flag = true;
				while (flag)
				{
					flag = false;
					for (int i = 0; i < this.subTranslations.Length; i++)
					{
						if (text.Contains("{" + this.subTranslations[i].key + "}"))
						{
							flag = true;
							if (this.subTranslations[i].dontTranslate)
							{
								text = text.Replace("{" + this.subTranslations[i].key + "}", this.subTranslations[i].value);
							}
							else
							{
								Localization.Translation translation2 = Localization.Translate(this.subTranslations[i].value);
								if (string.IsNullOrEmpty(translation2.text))
								{
									text = text.Replace("{" + this.subTranslations[i].key + "}", this.subTranslations[i].value);
								}
								else
								{
									text = text.Replace("{" + this.subTranslations[i].key + "}", translation2.text);
								}
							}
						}
					}
				}
			}
			string[] array = text.Split(new char[]
			{
				'{'
			});
			if (array.Length > 1)
			{
				string[] array2 = array[1].Split(new char[]
				{
					'}'
				});
				if (array2.Length > 1)
				{
					string text2 = array2[0];
					Localization.Translation translation3 = Localization.Translate(text2);
					if (!string.IsNullOrEmpty(translation3.text))
					{
						text = text.Replace("{" + text2 + "}", translation3.text);
					}
				}
			}
		}
		if (this.textComponent != null)
		{
			this.textComponent.text = text;
			this.textComponent.enabled = !string.IsNullOrEmpty(text);
			if (translation.hasCustomFont)
			{
				this.textComponent.font = translation.fonts.font;
			}
			else if (Localization.Instance.fonts[(int)this.currentLanguage][(int)translationElement.category].fontType != FontLoader.FontType.None)
			{
				this.textComponent.font = Localization.Instance.fonts[(int)this.currentLanguage][(int)translationElement.category].font;
			}
			this.textComponent.fontSize = ((translation.fonts.fontSize <= 0) ? this.initialFontSize : translation.fonts.fontSize);
		}
		if (this.textMeshProComponent != null)
		{
			this.textMeshProComponent.text = text;
			this.textMeshProComponent.enabled = !string.IsNullOrEmpty(text);
			this.textMeshProComponent.characterSpacing = translation.fonts.charSpacing;
			if (translation.hasCustomFontAsset)
			{
				this.textMeshProComponent.font = translation.fonts.fontAsset;
			}
			else
			{
				this.textMeshProComponent.font = Localization.Instance.fonts[(int)this.currentLanguage][(int)translationElement.category].fontAsset;
			}
			this.textMeshProComponent.fontSize = ((translation.fonts.fontAssetSize <= 0f) ? this.initialFontAssetSize : translation.fonts.fontAssetSize);
		}
		if (this.spriteRendererComponent != null)
		{
			Sprite sprite;
			if (translation.hasSpriteAtlasImage)
			{
				SpriteAtlas cachedAsset = AssetLoader<SpriteAtlas>.GetCachedAsset(translation.spriteAtlasName);
				sprite = cachedAsset.GetSprite(translation.spriteAtlasImageName);
			}
			else
			{
				sprite = translation.image;
			}
			this.spriteRendererComponent.sprite = sprite;
			this.spriteRendererComponent.enabled = false;
			this.spriteRendererComponent.enabled = (sprite != null);
		}
		if (this.imageComponent != null)
		{
			Sprite sprite2;
			if (translation.hasSpriteAtlasImage)
			{
				SpriteAtlas cachedAsset2 = AssetLoader<SpriteAtlas>.GetCachedAsset(translation.spriteAtlasName);
				sprite2 = cachedAsset2.GetSprite(translation.spriteAtlasImageName);
			}
			else
			{
				sprite2 = translation.image;
			}
			this.imageComponent.sprite = sprite2;
			this.imageComponent.enabled = false;
			this.imageComponent.enabled = (sprite2 != null);
		}
	}

	// Token: 0x040027C2 RID: 10178
	public bool existingKey;

	// Token: 0x040027C3 RID: 10179
	public int currentID = -1;

	// Token: 0x040027C4 RID: 10180
	public Localization.Languages currentLanguage = (Localization.Languages)(-1);

	// Token: 0x040027C5 RID: 10181
	public Localization.Categories currentCategory;

	// Token: 0x040027C6 RID: 10182
	public bool currentCustomFont;

	// Token: 0x040027C7 RID: 10183
	public Text textComponent;

	// Token: 0x040027C8 RID: 10184
	public Image imageComponent;

	// Token: 0x040027C9 RID: 10185
	public SpriteRenderer spriteRendererComponent;

	// Token: 0x040027CA RID: 10186
	public TMP_Text textMeshProComponent;

	// Token: 0x040027CB RID: 10187
	public int initialFontSize;

	// Token: 0x040027CC RID: 10188
	public float initialFontAssetSize;

	// Token: 0x040027CD RID: 10189
	public bool isInit;

	// Token: 0x040027CE RID: 10190
	public LocalizationHelper.LocalizationSubtext[] subTranslations;

	// Token: 0x040027CF RID: 10191
	public bool hasOverride;

	// Token: 0x040027D0 RID: 10192
	public LocalizationHelperPlatformOverride platformOverride;

	// Token: 0x020010EA RID: 4330
	public struct LocalizationSubtext
	{
		// Token: 0x06007BB8 RID: 31672 RVA: 0x0005356D File Offset: 0x0005176D
		public LocalizationSubtext(string key, string value, bool dontTranslate = false)
		{
			this.key = key;
			this.value = value;
			this.dontTranslate = dontTranslate;
		}

		// Token: 0x040077E9 RID: 30697
		public string key;

		// Token: 0x040077EA RID: 30698
		public string value;

		// Token: 0x040077EB RID: 30699
		public bool dontTranslate;
	}
}
