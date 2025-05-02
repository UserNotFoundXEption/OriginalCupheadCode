using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;

// Token: 0x0200046C RID: 1132
[CreateAssetMenu(fileName = "LocalizationAsset", menuName = "Localization Asset", order = 1)]
public class Localization : ScriptableObject, ISerializationCallbackReceiver
{
	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06003003 RID: 12291 RVA: 0x00027FB5 File Offset: 0x000261B5
	public static Localization Instance
	{
		get
		{
			if (Localization._instance == null)
			{
				Localization._instance = Resources.Load<Localization>("LocalizationAsset");
			}
			return Localization._instance;
		}
	}

	// Token: 0x14000065 RID: 101
	// (add) Token: 0x06003004 RID: 12292 RVA: 0x000E358C File Offset: 0x000E178C
	// (remove) Token: 0x06003005 RID: 12293 RVA: 0x000E35C0 File Offset: 0x000E17C0
	public static event Localization.LanguageChanged OnLanguageChangedEvent;

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06003006 RID: 12294 RVA: 0x00027FDB File Offset: 0x000261DB
	// (set) Token: 0x06003007 RID: 12295 RVA: 0x00028006 File Offset: 0x00026206
	public static Localization.Languages language
	{
		get
		{
			if (SettingsData.Data.language == -1)
			{
				SettingsData.Data.language = (int)DetectLanguage.GetDefaultLanguage();
			}
			return (Localization.Languages)SettingsData.Data.language;
		}
		set
		{
			SettingsData.Data.language = (int)value;
			if (Localization.OnLanguageChangedEvent != null)
			{
				Localization.OnLanguageChangedEvent();
			}
		}
	}

	// Token: 0x06003008 RID: 12296 RVA: 0x000E35F4 File Offset: 0x000E17F4
	public static Localization.Translation Translate(string key)
	{
		int id;
		if (Parser.IntTryParse(key, out id))
		{
			return Localization.Translate(id);
		}
		Localization.Translation result = default(Localization.Translation);
		for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
		{
			if (Localization._instance.m_TranslationElements[i].key == key)
			{
				TranslationElement translationElement = Localization._instance.m_TranslationElements[i];
				result = translationElement.translation;
			}
		}
		return result;
	}

	// Token: 0x06003009 RID: 12297 RVA: 0x000E3678 File Offset: 0x000E1878
	public static Localization.Translation Translate(int id)
	{
		Localization.Translation result = default(Localization.Translation);
		for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
		{
			if (Localization._instance.m_TranslationElements[i].id == id)
			{
				TranslationElement translationElement = Localization._instance.m_TranslationElements[i];
				result = translationElement.translation;
			}
		}
		return result;
	}

	// Token: 0x0600300A RID: 12298 RVA: 0x000E36E4 File Offset: 0x000E18E4
	public static TranslationElement Find(string key)
	{
		for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
		{
			if (Localization._instance.m_TranslationElements[i].key == key)
			{
				return Localization._instance.m_TranslationElements[i];
			}
		}
		return null;
	}

	// Token: 0x0600300B RID: 12299 RVA: 0x000E3744 File Offset: 0x000E1944
	public static TranslationElement Find(int id)
	{
		for (int i = 0; i < Localization.Instance.m_TranslationElements.Count; i++)
		{
			if (Localization._instance.m_TranslationElements[i].id == id)
			{
				return Localization._instance.m_TranslationElements[i];
			}
		}
		return null;
	}

	// Token: 0x0600300C RID: 12300 RVA: 0x000E37A0 File Offset: 0x000E19A0
	public static void ExportCsv(string path)
	{
		string text = "|lang|";
		string text2 = "|lang|_cuphead";
		string text3 = "|lang|_mugman";
		char value = '@';
		string value2 = "\r\n";
		StringBuilder stringBuilder = new StringBuilder();
		int num = Enum.GetNames(typeof(Localization.Languages)).Length;
		int num2 = Enum.GetNames(typeof(Localization.Categories)).Length;
		for (int i = 0; i < Localization.csvKeys.Length; i++)
		{
			if (Localization.csvKeys[i].Contains(text))
			{
				string value3 = Localization.csvKeys[i].Replace(text, string.Empty);
				for (int j = 0; j < num; j++)
				{
					if (i > 0)
					{
						stringBuilder.Append(value);
					}
					StringBuilder stringBuilder2 = stringBuilder;
					Localization.Languages languages = (Localization.Languages)j;
					stringBuilder2.Append(languages.ToString());
					stringBuilder.Append(value3);
				}
			}
			else
			{
				if (i > 0)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.Append(Localization.csvKeys[i]);
			}
		}
		stringBuilder.Append(value2);
		string text4 = string.Empty;
		for (int k = 0; k < Localization.Instance.m_TranslationElements.Count; k++)
		{
			TranslationElement translationElement = Localization._instance.m_TranslationElements[k];
			if (translationElement.depth != -1)
			{
				for (int l = 0; l < Localization.csvKeys.Length; l++)
				{
					if (Localization.csvKeys[l].Contains(text))
					{
						for (int m = 0; m < num; m++)
						{
							if (l > 0)
							{
								stringBuilder.Append(value);
							}
							text4 = string.Empty;
							string a;
							Localization.Translation translation;
							if (Localization.csvKeys[l].Contains(text2))
							{
								a = Localization.csvKeys[l].Replace(text2, string.Empty);
								if (translationElement.translationsCuphead == null || translationElement.translationsCuphead.Length == 0)
								{
									translation = default(Localization.Translation);
								}
								else
								{
									translation = translationElement.translationsCuphead[m];
								}
							}
							else if (Localization.csvKeys[l].Contains(text3))
							{
								a = Localization.csvKeys[l].Replace(text3, string.Empty);
								if (translationElement.translationsMugman == null || translationElement.translationsMugman.Length == 0)
								{
									translation = default(Localization.Translation);
								}
								else
								{
									translation = translationElement.translationsMugman[m];
								}
							}
							else
							{
								a = Localization.csvKeys[l].Replace(text, string.Empty);
								translation = translationElement.translations[m];
							}
							if (a == "_text")
							{
								text4 = translation.text;
								if (!string.IsNullOrEmpty(text4))
								{
									text4 = text4.Replace('\n'.ToString(), '\\' + "n");
								}
							}
							else if (a == "_image")
							{
								if (translation.image != null)
								{
									text4 = translation.image.name;
								}
							}
							else if (a == "_spriteAtlasName")
							{
								text4 = translation.spriteAtlasName;
							}
							else if (a == "_spriteAtlasImageName")
							{
								text4 = translation.spriteAtlasImageName;
							}
							else if (a == "_font")
							{
								if (translation.fonts.fontType != FontLoader.FontType.None)
								{
									text4 = FontLoader.GetFilename(translation.fonts.fontType);
								}
							}
							else if (a == "_fontSize")
							{
								if (translation.fonts.fontSize > 0)
								{
									text4 = translation.fonts.fontSize.ToString();
								}
								else
								{
									text4 = string.Empty;
								}
							}
							else if (a == "_fontAsset")
							{
								if (translation.fonts.tmpFontType != FontLoader.TMPFontType.None)
								{
									text4 = FontLoader.GetFilename(translation.fonts.tmpFontType);
								}
							}
							else if (a == "_fontAssetSize")
							{
								if (translation.fonts.fontAssetSize > 0f)
								{
									text4 = translation.fonts.fontAssetSize.ToString();
								}
								else
								{
									text4 = string.Empty;
								}
							}
							if (text4 != null)
							{
								stringBuilder.Append(text4);
							}
						}
					}
					else
					{
						if (l > 0)
						{
							stringBuilder.Append(value);
						}
						text4 = string.Empty;
						string a = Localization.csvKeys[l];
						if (a == "id")
						{
							text4 = translationElement.id.ToString();
						}
						else if (a == "key")
						{
							text4 = translationElement.key;
						}
						else if (a == "category")
						{
							text4 = translationElement.category.ToString();
						}
						else if (a == "description")
						{
							text4 = translationElement.description;
						}
						if (text4 != null)
						{
							stringBuilder.Append(text4);
						}
					}
				}
				stringBuilder.Append(value2);
			}
		}
		Encoding encoding = new UTF8Encoding(true);
		byte[] bytes = encoding.GetBytes(stringBuilder.ToString());
		FileStream fileStream = new FileStream(path, FileMode.Create);
		byte[] preamble = encoding.GetPreamble();
		fileStream.Write(preamble, 0, preamble.Length);
		fileStream.Write(bytes, 0, bytes.Length);
		fileStream.Dispose();
	}

	// Token: 0x0600300D RID: 12301 RVA: 0x000E3D68 File Offset: 0x000E1F68
	public static void ImportCsv(string path)
	{
		char c = '@';
		string text = "\r\n";
		Encoding encoding = new UTF8Encoding(true);
		FileStream fileStream = new FileStream(path, FileMode.Open);
		byte[] preamble = encoding.GetPreamble();
		byte[] array = new byte[preamble.Length];
		fileStream.Read(array, 0, preamble.Length);
		bool flag = true;
		for (int i = 0; i < preamble.Length; i++)
		{
			if (preamble[i] != array[i])
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			array = new byte[fileStream.Length - (long)preamble.Length];
			fileStream.Read(array, 0, array.Length);
		}
		else
		{
			array = new byte[fileStream.Length];
			fileStream.Position = 0L;
			fileStream.Read(array, 0, (int)fileStream.Length);
		}
		fileStream.Dispose();
		string @string = encoding.GetString(array);
		string[] array2 = @string.Split(new string[]
		{
			text
		}, StringSplitOptions.RemoveEmptyEntries);
		string[] headers = array2[0].Split(new char[]
		{
			c
		});
		Localization.processImportedLines(headers, array2, c);
	}

	// Token: 0x0600300E RID: 12302 RVA: 0x000E3E7C File Offset: 0x000E207C
	public static void processImportedLines(string[] headers, string[] lines, char separator)
	{
		string text = "_cuphead";
		string text2 = "_mugman";
		string[] names = Enum.GetNames(typeof(Localization.Languages));
		string[] names2 = Enum.GetNames(typeof(Localization.Categories));
		Dictionary<string, Font> dictionary = new Dictionary<string, Font>();
		Dictionary<string, TMP_FontAsset> dictionary2 = new Dictionary<string, TMP_FontAsset>();
		Localization.Instance.m_TranslationElements.Clear();
		TranslationElement translationElement = new TranslationElement("Root", -1, 0);
		Localization._instance.m_TranslationElements.Add(translationElement);
		for (int i = 1; i < lines.Length; i++)
		{
			string[] array = lines[i].Split(new char[]
			{
				separator
			});
			if (array.Length != headers.Length)
			{
				if (lines[i] != string.Empty)
				{
				}
			}
			else
			{
				translationElement = Localization.Instance.AddKey();
				for (int j = 0; j < array.Length; j++)
				{
					if (!string.IsNullOrEmpty(array[j]))
					{
						string text3 = headers[j];
						if (text3 == "id")
						{
							translationElement.id = Parser.IntParse(array[j]);
						}
						else if (text3 == "key")
						{
							translationElement.key = array[j];
						}
						else if (text3 == "category")
						{
							int category = -1;
							for (int k = 0; k < names2.Length; k++)
							{
								if (names2[k] == array[j])
								{
									category = k;
								}
							}
							translationElement.category = (Localization.Categories)category;
						}
						else if (text3 == "description")
						{
							translationElement.description = array[j];
						}
						else
						{
							for (int l = 0; l < names.Length; l++)
							{
								if (text3.Contains(names[l]))
								{
									text3 = text3.Replace(names[l], string.Empty);
									bool flag = false;
									bool flag2 = false;
									Localization.Translation translation;
									if (text3.Contains(text))
									{
										flag = true;
										text3 = text3.Replace(text, string.Empty);
										if (translationElement.translationsCuphead == null || translationElement.translationsCuphead.Length == 0)
										{
											translationElement.translationsCuphead = new Localization.Translation[names.Length];
											translationElement.translationsMugman = new Localization.Translation[names.Length];
										}
										translation = translationElement.translationsCuphead[l];
									}
									else if (text3.Contains(text2))
									{
										flag2 = true;
										text3 = text3.Replace(text2, string.Empty);
										if (translationElement.translationsCuphead == null || translationElement.translationsCuphead.Length == 0)
										{
											translationElement.translationsCuphead = new Localization.Translation[names.Length];
											translationElement.translationsMugman = new Localization.Translation[names.Length];
										}
										translation = translationElement.translationsMugman[l];
									}
									else
									{
										translation = translationElement.translations[l];
									}
									if (translation.fonts == null)
									{
										translation.fonts = new Localization.CategoryLanguageFont();
									}
									if (text3 == "_text")
									{
										translation.text = array[j];
									}
									else if (text3 == "_image")
									{
										if (string.IsNullOrEmpty(array[j]))
										{
											break;
										}
									}
									else if (text3 == "_spriteAtlasName")
									{
										translation.spriteAtlasName = array[j];
									}
									else if (text3 == "_spriteAtlasImageName")
									{
										translation.spriteAtlasImageName = array[j];
									}
									else if (text3 == "_font")
									{
										if (string.IsNullOrEmpty(array[j]))
										{
											break;
										}
									}
									else if (text3 == "_fontSize")
									{
										if (!string.IsNullOrEmpty(array[j]))
										{
											int num = Convert.ToInt32(array[j]);
											if (num == 0)
											{
												break;
											}
											translation.fonts.fontSize = num;
										}
									}
									else if (text3 == "_fontAsset")
									{
										if (string.IsNullOrEmpty(array[j]))
										{
											break;
										}
									}
									else if (text3 == "_fontAssetSize" && !string.IsNullOrEmpty(array[j]))
									{
										float num2 = Convert.ToSingle(array[j]);
										if (num2 == 0f)
										{
											break;
										}
										translation.fonts.fontAssetSize = num2;
									}
									if (flag)
									{
										translationElement.translationsCuphead[l] = translation;
									}
									else if (flag2)
									{
										translationElement.translationsMugman[l] = translation;
									}
									else
									{
										translationElement.translations[l] = translation;
									}
									break;
								}
							}
						}
					}
				}
			}
		}
		if (Localization.OnLanguageChangedEvent != null)
		{
			Localization.OnLanguageChangedEvent();
		}
	}

	// Token: 0x1700037C RID: 892
	// (get) Token: 0x0600300F RID: 12303 RVA: 0x000E4378 File Offset: 0x000E2578
	// (set) Token: 0x06003010 RID: 12304 RVA: 0x00028027 File Offset: 0x00026227
	[SerializeField]
	public Localization.CategoryLanguageFonts[] fonts
	{
		get
		{
			if (this.m_Fonts == null)
			{
				int num = Enum.GetNames(typeof(Localization.Languages)).Length;
				int num2 = Enum.GetNames(typeof(Localization.Categories)).Length;
				this.m_Fonts = new Localization.CategoryLanguageFonts[num];
				for (int i = 0; i < num; i++)
				{
					this.m_Fonts[i].fonts = new Localization.CategoryLanguageFont[num2];
				}
			}
			return this.m_Fonts;
		}
		set
		{
			this.m_Fonts = value;
		}
	}

	// Token: 0x1700037D RID: 893
	// (get) Token: 0x06003011 RID: 12305 RVA: 0x00028030 File Offset: 0x00026230
	// (set) Token: 0x06003012 RID: 12306 RVA: 0x00028038 File Offset: 0x00026238
	[SerializeField]
	public List<TranslationElement> translationElements
	{
		get
		{
			return this.m_TranslationElements;
		}
		set
		{
			this.m_TranslationElements = value;
		}
	}

	// Token: 0x06003013 RID: 12307 RVA: 0x000E43F0 File Offset: 0x000E25F0
	public TranslationElement AddKey()
	{
		int num = -1;
		for (int i = 0; i < this.m_TranslationElements.Count; i++)
		{
			if (this.m_TranslationElements[i].id > num)
			{
				num = this.m_TranslationElements[i].id;
			}
		}
		num++;
		TranslationElement translationElement = new TranslationElement("Key" + num, Localization.Categories.NoCategory, string.Empty, string.Empty, string.Empty, 0, num);
		this.m_TranslationElements.Add(translationElement);
		return translationElement;
	}

	// Token: 0x06003014 RID: 12308 RVA: 0x000E4480 File Offset: 0x000E2680
	public void Awake()
	{
		if (this.m_TranslationElements.Count == 0)
		{
			this.m_TranslationElements = new List<TranslationElement>(1);
			TranslationElement item = new TranslationElement("Root", -1, 0);
			this.m_TranslationElements.Add(item);
		}
	}

	// Token: 0x06003015 RID: 12309 RVA: 0x00028041 File Offset: 0x00026241
	public void OnBeforeSerialize()
	{
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x000E44C4 File Offset: 0x000E26C4
	public void OnAfterDeserialize()
	{
		bool flag = false;
		int num = Enum.GetNames(typeof(Localization.Languages)).Length;
		if (this.fonts.Length < num)
		{
			flag = true;
		}
		int num2 = Enum.GetNames(typeof(Localization.Categories)).Length;
		if (this.fonts[0].fonts.Length < num2)
		{
			flag = true;
		}
		if (flag)
		{
			this.fonts = this.GrowFonts(this.fonts, num, num2);
		}
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x000E453C File Offset: 0x000E273C
	public Localization.CategoryLanguageFonts[] GrowFonts(Localization.CategoryLanguageFonts[] oldFonts, int newLanguagesLength, int newCategoriesLength)
	{
		Localization.CategoryLanguageFonts[] array = new Localization.CategoryLanguageFonts[newLanguagesLength];
		for (int i = 0; i < newLanguagesLength; i++)
		{
			array[i].fonts = new Localization.CategoryLanguageFont[newCategoriesLength];
		}
		for (int j = 0; j < oldFonts.Length; j++)
		{
			for (int k = 0; k < oldFonts[j].fonts.Length; k++)
			{
				array[j][k] = oldFonts[j][k];
			}
		}
		return array;
	}

	// Token: 0x040027B9 RID: 10169
	public const int LanguagesEnumSize = 12;

	// Token: 0x040027BA RID: 10170
	public const string PATH = "LocalizationAsset";

	// Token: 0x040027BB RID: 10171
	public static string[] csvKeys = new string[]
	{
		"id",
		"key",
		"category",
		"description",
		"|lang|_text",
		"|lang|_cuphead_text",
		"|lang|_mugman_text",
		"|lang|_image",
		"|lang|_spriteAtlasName",
		"|lang|_spriteAtlasImageName",
		"|lang|_cuphead_image",
		"|lang|_mugman_image",
		"|lang|_font",
		"|lang|_fontSize",
		"|lang|_fontAsset",
		"|lang|_fontAssetSize"
	};

	// Token: 0x040027BC RID: 10172
	public static Localization _instance;

	// Token: 0x040027BE RID: 10174
	public static Localization.Languages language1 = Localization.Languages.English;

	// Token: 0x040027BF RID: 10175
	public static Localization.Languages language2 = Localization.Languages.French;

	// Token: 0x040027C0 RID: 10176
	[SerializeField]
	public List<TranslationElement> m_TranslationElements = new List<TranslationElement>();

	// Token: 0x040027C1 RID: 10177
	[SerializeField]
	public Localization.CategoryLanguageFonts[] m_Fonts;

	// Token: 0x020010E4 RID: 4324
	[SerializeField]
	public enum Languages
	{
		// Token: 0x040077A3 RID: 30627
		English,
		// Token: 0x040077A4 RID: 30628
		French,
		// Token: 0x040077A5 RID: 30629
		Italian,
		// Token: 0x040077A6 RID: 30630
		German,
		// Token: 0x040077A7 RID: 30631
		SpanishSpain,
		// Token: 0x040077A8 RID: 30632
		SpanishAmerica,
		// Token: 0x040077A9 RID: 30633
		Korean,
		// Token: 0x040077AA RID: 30634
		Russian,
		// Token: 0x040077AB RID: 30635
		Polish,
		// Token: 0x040077AC RID: 30636
		PortugueseBrazil,
		// Token: 0x040077AD RID: 30637
		Japanese,
		// Token: 0x040077AE RID: 30638
		SimplifiedChinese
	}

	// Token: 0x020010E5 RID: 4325
	[SerializeField]
	public enum Categories
	{
		// Token: 0x040077B0 RID: 30640
		NoCategory,
		// Token: 0x040077B1 RID: 30641
		LevelSelectionName,
		// Token: 0x040077B2 RID: 30642
		LevelSelectionIn,
		// Token: 0x040077B3 RID: 30643
		LevelSelectionStage,
		// Token: 0x040077B4 RID: 30644
		LevelSelectionDifficultyHeader,
		// Token: 0x040077B5 RID: 30645
		LevelSelectionDifficultys,
		// Token: 0x040077B6 RID: 30646
		EquipCategoryNames,
		// Token: 0x040077B7 RID: 30647
		EquipWeaponNames,
		// Token: 0x040077B8 RID: 30648
		EquipCategoryBackName,
		// Token: 0x040077B9 RID: 30649
		EquipCategoryBackTitle,
		// Token: 0x040077BA RID: 30650
		EquipCategoryBackSubtitle,
		// Token: 0x040077BB RID: 30651
		EquipCategoryBackDescription,
		// Token: 0x040077BC RID: 30652
		ChecklistTitle,
		// Token: 0x040077BD RID: 30653
		ChecklistWorldNames,
		// Token: 0x040077BE RID: 30654
		ChecklistContractHeaders,
		// Token: 0x040077BF RID: 30655
		ChecklistContracts,
		// Token: 0x040077C0 RID: 30656
		PauseMenuItems,
		// Token: 0x040077C1 RID: 30657
		DeathMenuQuote,
		// Token: 0x040077C2 RID: 30658
		DeathMenuItems,
		// Token: 0x040077C3 RID: 30659
		ResultsMenuTitle,
		// Token: 0x040077C4 RID: 30660
		ResultsMenuCategories,
		// Token: 0x040077C5 RID: 30661
		ResultsMenuGrade,
		// Token: 0x040077C6 RID: 30662
		ResultsMenuNewRecord,
		// Token: 0x040077C7 RID: 30663
		ResultsMenuTryNormal,
		// Token: 0x040077C8 RID: 30664
		IntroEndingText,
		// Token: 0x040077C9 RID: 30665
		IntroEndingAction,
		// Token: 0x040077CA RID: 30666
		CutScenesText,
		// Token: 0x040077CB RID: 30667
		SpeechBalloons,
		// Token: 0x040077CC RID: 30668
		WorldMapTitles,
		// Token: 0x040077CD RID: 30669
		Glyphs,
		// Token: 0x040077CE RID: 30670
		TitleScreenSelection,
		// Token: 0x040077CF RID: 30671
		Notifications,
		// Token: 0x040077D0 RID: 30672
		Tutorials,
		// Token: 0x040077D1 RID: 30673
		OptionMenu,
		// Token: 0x040077D2 RID: 30674
		RemappingMenu,
		// Token: 0x040077D3 RID: 30675
		RemappingButton,
		// Token: 0x040077D4 RID: 30676
		XboxNotification,
		// Token: 0x040077D5 RID: 30677
		AttractScreen,
		// Token: 0x040077D6 RID: 30678
		JoinPrompt,
		// Token: 0x040077D7 RID: 30679
		ConfirmMenu,
		// Token: 0x040077D8 RID: 30680
		DifficultyMenu,
		// Token: 0x040077D9 RID: 30681
		ShopElement,
		// Token: 0x040077DA RID: 30682
		StageTitles,
		// Token: 0x040077DB RID: 30683
		NintendoSwitchNotification,
		// Token: 0x040077DC RID: 30684
		Achievements
	}

	// Token: 0x020010E6 RID: 4326
	[Serializable]
	public struct Translation
	{
		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x06007BAB RID: 31659 RVA: 0x000534BF File Offset: 0x000516BF
		public bool hasSpriteAtlasImage
		{
			get
			{
				return this.spriteAtlasName != null && this.spriteAtlasName.Length > 0 && this.spriteAtlasImageName != null && this.spriteAtlasImageName.Length > 0;
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x06007BAC RID: 31660 RVA: 0x000534F9 File Offset: 0x000516F9
		public bool hasCustomFont
		{
			get
			{
				return this.fonts.fontType != FontLoader.FontType.None;
			}
		}

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x06007BAD RID: 31661 RVA: 0x0005350C File Offset: 0x0005170C
		public bool hasCustomFontAsset
		{
			get
			{
				return this.fonts.tmpFontType != FontLoader.TMPFontType.None;
			}
		}

		// Token: 0x06007BAE RID: 31662 RVA: 0x0005351F File Offset: 0x0005171F
		public string SanitizedText()
		{
			return this.text.Replace("\\n", "\n");
		}

		// Token: 0x040077DD RID: 30685
		[SerializeField]
		public bool hasImage;

		// Token: 0x040077DE RID: 30686
		[SerializeField]
		public string text;

		// Token: 0x040077DF RID: 30687
		[SerializeField]
		public Localization.CategoryLanguageFont fonts;

		// Token: 0x040077E0 RID: 30688
		[SerializeField]
		public Sprite image;

		// Token: 0x040077E1 RID: 30689
		[SerializeField]
		public string spriteAtlasName;

		// Token: 0x040077E2 RID: 30690
		[SerializeField]
		public string spriteAtlasImageName;
	}

	// Token: 0x020010E7 RID: 4327
	[Serializable]
	public class CategoryLanguageFont
	{
		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x06007BB0 RID: 31664 RVA: 0x0005353E File Offset: 0x0005173E
		public Font font
		{
			get
			{
				return FontLoader.GetFont(this.fontType);
			}
		}

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x06007BB1 RID: 31665 RVA: 0x0005354B File Offset: 0x0005174B
		public TMP_FontAsset fontAsset
		{
			get
			{
				return FontLoader.GetTMPFont(this.tmpFontType);
			}
		}

		// Token: 0x040077E3 RID: 30691
		public int fontSize;

		// Token: 0x040077E4 RID: 30692
		public FontLoader.FontType fontType;

		// Token: 0x040077E5 RID: 30693
		public float fontAssetSize;

		// Token: 0x040077E6 RID: 30694
		public FontLoader.TMPFontType tmpFontType;

		// Token: 0x040077E7 RID: 30695
		public float charSpacing;
	}

	// Token: 0x020010E8 RID: 4328
	[Serializable]
	public struct CategoryLanguageFonts
	{
		// Token: 0x1700175F RID: 5983
		public Localization.CategoryLanguageFont this[int index]
		{
			get
			{
				return this.fonts[index];
			}
			set
			{
				this.fonts[index] = value;
			}
		}

		// Token: 0x040077E8 RID: 30696
		[SerializeField]
		public Localization.CategoryLanguageFont[] fonts;
	}

	// Token: 0x020010E9 RID: 4329
	// (Invoke) Token: 0x06007BB5 RID: 31669
	public delegate void LanguageChanged();
}
