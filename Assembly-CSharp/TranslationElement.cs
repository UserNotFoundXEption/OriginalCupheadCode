using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000470 RID: 1136
[Serializable]
public class TranslationElement : ISerializationCallbackReceiver
{
	// Token: 0x06003027 RID: 12327 RVA: 0x000280CF File Offset: 0x000262CF
	public TranslationElement()
	{
	}

	// Token: 0x06003028 RID: 12328 RVA: 0x000280FA File Offset: 0x000262FA
	public TranslationElement(string key, int depth, int id)
	{
		this.key = key;
		this.m_ID = id;
		this.m_Depth = depth;
	}

	// Token: 0x06003029 RID: 12329 RVA: 0x000E4CC4 File Offset: 0x000E2EC4
	public TranslationElement(string key, Localization.Categories category, string description, string translation1, string translation2, int depth, int id)
	{
		this.m_ID = id;
		this.m_Depth = depth;
		this.key = key;
		this.category = category;
		this.description = description;
		this.translations[(int)Localization.language1].text = translation1;
		this.translations[(int)Localization.language2].text = translation2;
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x0600302A RID: 12330 RVA: 0x000E4D50 File Offset: 0x000E2F50
	// (set) Token: 0x0600302B RID: 12331 RVA: 0x000E4DEC File Offset: 0x000E2FEC
	public Localization.Translation translation
	{
		get
		{
			if (!PlayerManager.Multiplayer && this.translationsCuphead != null && (Localization.Languages)this.translationsCuphead.Length > Localization.language)
			{
				return this.translationsCuphead[(int)Localization.language];
			}
			if (!PlayerManager.Multiplayer && this.translationsMugman != null && (Localization.Languages)this.translationsMugman.Length > Localization.language)
			{
				return this.translationsMugman[(int)Localization.language];
			}
			return this.translations[(int)Localization.language];
		}
		set
		{
			if (!PlayerManager.Multiplayer && this.translationsCuphead != null && (Localization.Languages)this.translationsCuphead.Length > Localization.language)
			{
				this.translationsCuphead[(int)Localization.language] = value;
			}
			if (!PlayerManager.Multiplayer && this.translationsMugman != null && (Localization.Languages)this.translationsMugman.Length > Localization.language)
			{
				this.translationsMugman[(int)Localization.language] = value;
			}
			this.translations[(int)Localization.language] = value;
		}
	}

	// Token: 0x17000380 RID: 896
	// (get) Token: 0x0600302C RID: 12332 RVA: 0x0002813A File Offset: 0x0002633A
	// (set) Token: 0x0600302D RID: 12333 RVA: 0x00028142 File Offset: 0x00026342
	public int depth
	{
		get
		{
			return this.m_Depth;
		}
		set
		{
			this.m_Depth = value;
		}
	}

	// Token: 0x17000381 RID: 897
	// (get) Token: 0x0600302E RID: 12334 RVA: 0x0002814B File Offset: 0x0002634B
	// (set) Token: 0x0600302F RID: 12335 RVA: 0x00028153 File Offset: 0x00026353
	public TranslationElement parent
	{
		get
		{
			return this.m_Parent;
		}
		set
		{
			this.m_Parent = value;
		}
	}

	// Token: 0x17000382 RID: 898
	// (get) Token: 0x06003030 RID: 12336 RVA: 0x0002815C File Offset: 0x0002635C
	// (set) Token: 0x06003031 RID: 12337 RVA: 0x00028164 File Offset: 0x00026364
	public List<TranslationElement> children
	{
		get
		{
			return this.m_Children;
		}
		set
		{
			this.m_Children = value;
		}
	}

	// Token: 0x17000383 RID: 899
	// (get) Token: 0x06003032 RID: 12338 RVA: 0x0002816D File Offset: 0x0002636D
	public bool hasChildren
	{
		get
		{
			return this.children != null && this.children.Count > 0;
		}
	}

	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06003033 RID: 12339 RVA: 0x0002818B File Offset: 0x0002638B
	// (set) Token: 0x06003034 RID: 12340 RVA: 0x00028193 File Offset: 0x00026393
	public int id
	{
		get
		{
			return this.m_ID;
		}
		set
		{
			this.m_ID = value;
		}
	}

	// Token: 0x06003035 RID: 12341 RVA: 0x000E4E8C File Offset: 0x000E308C
	public Localization.Translation[] Grow(Localization.Translation[] oldTranslations, int newLength)
	{
		Localization.Translation[] array = new Localization.Translation[newLength];
		for (int i = 0; i < oldTranslations.Length; i++)
		{
			array[i].fonts = oldTranslations[i].fonts;
			array[i].image = oldTranslations[i].image;
			array[i].spriteAtlasName = oldTranslations[i].spriteAtlasName;
			array[i].spriteAtlasImageName = oldTranslations[i].spriteAtlasImageName;
			array[i].hasImage = oldTranslations[i].hasImage;
			array[i].text = oldTranslations[i].text;
		}
		for (int j = oldTranslations.Length; j < array.Length; j++)
		{
			array[j].fonts = null;
			array[j].image = null;
			array[j].hasImage = false;
			array[j].text = string.Empty;
			array[j].spriteAtlasName = string.Empty;
			array[j].spriteAtlasImageName = string.Empty;
		}
		return array;
	}

	// Token: 0x06003036 RID: 12342 RVA: 0x0002819C File Offset: 0x0002639C
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x000E4FB8 File Offset: 0x000E31B8
	public void OnAfterDeserialize()
	{
		int num = Enum.GetNames(typeof(Localization.Languages)).Length;
		if (this.translations.Length < num)
		{
			this.translations = this.Grow(this.translations, num);
			if (this.translationsCuphead != null)
			{
				this.translationsCuphead = this.Grow(this.translationsCuphead, num);
			}
			if (this.translationsMugman != null)
			{
				this.translationsMugman = this.Grow(this.translationsMugman, num);
			}
		}
	}

	// Token: 0x040027D2 RID: 10194
	[SerializeField]
	public int m_ID;

	// Token: 0x040027D3 RID: 10195
	[SerializeField]
	public int m_Depth;

	// Token: 0x040027D4 RID: 10196
	[NonSerialized]
	public TranslationElement m_Parent;

	// Token: 0x040027D5 RID: 10197
	[NonSerialized]
	public List<TranslationElement> m_Children;

	// Token: 0x040027D6 RID: 10198
	[SerializeField]
	public string key = string.Empty;

	// Token: 0x040027D7 RID: 10199
	[SerializeField]
	public Localization.Categories category;

	// Token: 0x040027D8 RID: 10200
	[SerializeField]
	public string description = string.Empty;

	// Token: 0x040027D9 RID: 10201
	[SerializeField]
	public Localization.Translation[] translations = new Localization.Translation[12];

	// Token: 0x040027DA RID: 10202
	[SerializeField]
	public Localization.Translation[] translationsCuphead;

	// Token: 0x040027DB RID: 10203
	[SerializeField]
	public Localization.Translation[] translationsMugman;

	// Token: 0x040027DC RID: 10204
	public bool enabled;

	// Token: 0x040027DD RID: 10205
	[NonSerialized]
	public bool multiplayerLock;
}
