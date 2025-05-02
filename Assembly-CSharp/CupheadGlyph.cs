using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000EB RID: 235
public class CupheadGlyph : MonoBehaviour
{
	// Token: 0x170001C5 RID: 453
	// (get) Token: 0x06000B0F RID: 2831 RVA: 0x0007D0CC File Offset: 0x0007B2CC
	public float preferredWidth
	{
		get
		{
			return Mathf.Max(this.glyphText.preferredWidth + this.paddingText, this.glyphChar.rectTransform.sizeDelta.y);
		}
	}

	// Token: 0x06000B10 RID: 2832 RVA: 0x0007D108 File Offset: 0x0007B308
	public void Awake()
	{
		this.initialFontSize = this.glyphChar.fontSize;
		this.initialCharColor = this.glyphChar.color;
		this.initialScale = base.transform.localScale;
		this.initialCharWrapMode = this.glyphChar.verticalOverflow;
		if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstruction || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Shop || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.ShmupTutorial)
		{
			this.initialCharMaterial = this.glyphChar.material;
		}
	}

	// Token: 0x06000B11 RID: 2833 RVA: 0x00009F06 File Offset: 0x00008106
	public void Start()
	{
		this.Init();
		PlayerManager.OnControlsChanged += this.OnControlsChanged;
		Localization.OnLanguageChangedEvent += this.OnLanguageChanged;
	}

	// Token: 0x06000B12 RID: 2834 RVA: 0x00009F30 File Offset: 0x00008130
	public void OnLanguageChanged()
	{
		this.Init();
	}

	// Token: 0x06000B13 RID: 2835 RVA: 0x00009F38 File Offset: 0x00008138
	public void OnControlsChanged()
	{
		this.Init();
	}

	// Token: 0x06000B14 RID: 2836 RVA: 0x0007D19C File Offset: 0x0007B39C
	public void Init()
	{
		Localization.Translation translation = CupheadInput.InputDisplayForButton(this.button, this.rewiredPlayerId);
		this.AlignDashInstructions(translation);
		string text = translation.text;
		bool flag = text.Length > 1;
		this.glyphSymbolText.gameObject.SetActive(flag);
		this.glyphText.gameObject.SetActive(flag);
		this.glyphChar.gameObject.SetActive(!flag);
		this.glyphSymbolChar.gameObject.SetActive(!flag);
		this.glyphText.text = text;
		this.glyphChar.text = text;
		this.glyphText.font = ((translation.fonts == null) ? Localization.Instance.fonts[(int)Localization.language][29].font : translation.fonts.font);
		for (int i = 0; i < this.rectTransformTexts.Length; i++)
		{
			if (flag)
			{
				float preferredWidth = this.preferredWidth;
				if (this.maxSize > 0f && preferredWidth > this.maxSize)
				{
					preferredWidth = this.maxSize;
				}
				this.rectTransformTexts[i].sizeDelta = new Vector2(preferredWidth, this.rectTransformTexts[i].sizeDelta.y);
			}
			else
			{
				RectTransform component = this.glyphChar.GetComponent<RectTransform>();
				if (component != null)
				{
					byte[] bytes = Encoding.ASCII.GetBytes(text);
					if (bytes.Length > 0)
					{
						int num = (int)(bytes[0] - 65);
						if (this.letterOffset == CupheadGlyph.LetterOffset.Normal)
						{
							if (num >= 0 && num < CupheadGlyph.letterSpecificOffset.Length)
							{
								component.anchoredPosition = CupheadGlyph.letterSpecificOffset[num];
							}
							else
							{
								num = this.PS4CharToIndex((char)bytes[0]);
								if (num >= 0)
								{
									component.anchoredPosition = this.ps4NormalOffset[num];
								}
							}
						}
						else if (num >= 0 && num < CupheadGlyph.letterSpecificSmallOffset.Length)
						{
							component.anchoredPosition = CupheadGlyph.letterSpecificSmallOffset[num];
						}
						else
						{
							num = this.PS4CharToIndex((char)bytes[0]);
							if (num >= 0)
							{
								component.anchoredPosition = CupheadGlyph.ps4SmallOffset[num];
							}
						}
					}
				}
				this.rectTransformTexts[i].sizeDelta = new Vector2(Mathf.Max(this.preferredWidth, this.rectTransformTexts[i].sizeDelta.y), this.rectTransformTexts[i].sizeDelta.y);
			}
		}
		LayoutElement component2 = base.GetComponent<LayoutElement>();
		if (component2 != null)
		{
			component2.preferredWidth = ((!flag) ? (this.preferredWidth - this.paddingText) : this.preferredWidth);
		}
		if (flag && this.maxSize > 0f)
		{
			this.glyphText.resizeTextMaxSize = this.glyphText.fontSize * 4;
			this.glyphText.resizeTextForBestFit = true;
			RectTransform component3 = this.glyphText.GetComponent<RectTransform>();
			component3.sizeDelta *= 4f;
			component3.localScale = Vector3.one * 0.25f;
		}
	}

	// Token: 0x06000B15 RID: 2837 RVA: 0x00009F40 File Offset: 0x00008140
	public void OnDestroy()
	{
		PlayerManager.OnControlsChanged -= this.OnControlsChanged;
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
	}

	// Token: 0x06000B16 RID: 2838 RVA: 0x00009F64 File Offset: 0x00008164
	public int PS4CharToIndex(char c)
	{
		return -1;
	}

	// Token: 0x06000B17 RID: 2839 RVA: 0x00009F67 File Offset: 0x00008167
	public int SwitchCharToIndex(char c)
	{
		if (c == CupheadGlyph.NintendoSwitchUp)
		{
			return 0;
		}
		if (c == CupheadGlyph.NintendoSwitchDown)
		{
			return 1;
		}
		if (c == CupheadGlyph.NintendoSwitchLeft)
		{
			return 2;
		}
		if (c == CupheadGlyph.NintendoSwitchRight)
		{
			return 3;
		}
		return -1;
	}

	// Token: 0x06000B18 RID: 2840 RVA: 0x0007D4FC File Offset: 0x0007B6FC
	public void SetSwitchGlyph(bool isSwitchGlyph, RectTransform rectTransform)
	{
		if (isSwitchGlyph)
		{
			this.glyphSymbolChar.gameObject.SetActive(false);
			this.glyphChar.fontSize = CupheadGlyph.NintendoSwitchFontSize;
			this.glyphChar.color = CupheadGlyph.NintendoSwitchColor;
			this.glyphChar.verticalOverflow = 1;
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstruction || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend)
			{
				this.glyphChar.material = null;
				this.glyphChar.color = CupheadGlyph.NintendoSwitchTutorialInstructionColor;
			}
			else if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Shop || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.ShmupTutorial)
			{
				this.glyphChar.material = null;
			}
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.SwitchWeapon)
			{
				base.transform.localScale = Vector3.one;
			}
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Equip)
			{
				this.glyphChar.GetComponent<Shadow>().enabled = true;
				this.glyphChar.GetComponent<Outline>().enabled = true;
			}
			Vector2 anchoredPosition = CupheadGlyph.SwitchOffsetMapping[(int)this.platformGlyphType];
			rectTransform.anchoredPosition = anchoredPosition;
		}
		else
		{
			this.glyphSymbolChar.gameObject.SetActive(true);
			this.glyphChar.fontSize = this.initialFontSize;
			this.glyphChar.color = this.initialCharColor;
			this.glyphChar.verticalOverflow = this.initialCharWrapMode;
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstruction || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.TutorialInstructionDescend || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Shop || this.platformGlyphType == CupheadGlyph.PlatformGlyphType.ShmupTutorial)
			{
				this.glyphChar.material = this.initialCharMaterial;
			}
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.SwitchWeapon)
			{
				base.transform.localScale = this.initialScale;
			}
			if (this.platformGlyphType == CupheadGlyph.PlatformGlyphType.Equip)
			{
				this.glyphChar.GetComponent<Shadow>().enabled = false;
				this.glyphChar.GetComponent<Outline>().enabled = false;
			}
		}
	}

	// Token: 0x06000B19 RID: 2841 RVA: 0x0007D6E0 File Offset: 0x0007B8E0
	public void AlignDashInstructions(Localization.Translation translation)
	{
		if (this.glyphLayouts != null)
		{
			bool enabled = !translation.text.Equals("Y");
			for (int i = 0; i < this.glyphLayouts.Length; i++)
			{
				this.glyphLayouts[i].enabled = enabled;
			}
		}
	}

	// Token: 0x040008A7 RID: 2215
	public static readonly char NintendoSwitchUp = '{';

	// Token: 0x040008A8 RID: 2216
	public static readonly char NintendoSwitchDown = '}';

	// Token: 0x040008A9 RID: 2217
	public static readonly char NintendoSwitchLeft = '<';

	// Token: 0x040008AA RID: 2218
	public static readonly char NintendoSwitchRight = '>';

	// Token: 0x040008AB RID: 2219
	public static readonly char PlayStation4Cross = '†';

	// Token: 0x040008AC RID: 2220
	public static readonly char PlayStation4Circle = '‡';

	// Token: 0x040008AD RID: 2221
	public static readonly char PlayStation4Square = '°';

	// Token: 0x040008AE RID: 2222
	public static readonly char PlayStation4Triangle = '~';

	// Token: 0x040008AF RID: 2223
	public static readonly int NintendoSwitchFontSize = 24;

	// Token: 0x040008B0 RID: 2224
	public static readonly Color NintendoSwitchColor = Color.white;

	// Token: 0x040008B1 RID: 2225
	public static readonly Color NintendoSwitchTutorialInstructionColor = new Color(0.254901975f, 0.254901975f, 0.254901975f, 1f);

	// Token: 0x040008B2 RID: 2226
	public static readonly Vector2[] letterSpecificOffset = new Vector2[]
	{
		new Vector2(1.6f, -0.4f),
		new Vector2(1.29f, -0.85f),
		new Vector2(1.3f, -1f),
		new Vector2(1.81f, -1.18f),
		new Vector2(0.9f, -1f),
		new Vector2(0.9f, -1f),
		new Vector2(1.2f, -1f),
		new Vector2(1f, -1f),
		new Vector2(0.8f, -1.2f),
		new Vector2(1.3f, -1.2f),
		new Vector2(0.5f, -1.2f),
		new Vector2(1.1f, -1f),
		new Vector2(0.8f, -1f),
		new Vector2(1.2f, -1.2f),
		new Vector2(1.1f, -1f),
		new Vector2(1.3f, -1.2f),
		new Vector2(1.1f, 0f),
		new Vector2(1.5f, -1.2f),
		new Vector2(1.5f, -1.2f),
		new Vector2(1.3f, -1.8f),
		new Vector2(0.9f, -1.4f),
		new Vector2(1.35f, -1.6f),
		new Vector2(0.6f, -2f),
		new Vector2(0.8f, -1.3f),
		new Vector2(0.95f, -1.8f),
		new Vector2(1.6f, -1f)
	};

	// Token: 0x040008B3 RID: 2227
	public static readonly Vector2[] letterSpecificSmallOffset = new Vector2[]
	{
		new Vector2(1.2f, 0f),
		new Vector2(0.5f, -0.2f),
		new Vector2(0.9f, -0.6f),
		new Vector2(1.1f, -0.3f),
		new Vector2(0.32f, -0.27f),
		new Vector2(0.32f, -0.85f),
		new Vector2(0.93f, -0.64f),
		new Vector2(0.64f, -0.56f),
		new Vector2(0.69f, -0.56f),
		new Vector2(0.53f, -0.38f),
		new Vector2(1.01f, -0.38f),
		new Vector2(0.77f, -0.19f),
		new Vector2(0.93f, -0.49f),
		new Vector2(0.79f, -0.67f),
		new Vector2(0.92f, -0.47f),
		new Vector2(1.34f, -0.44f),
		new Vector2(0.97f, 0.63f),
		new Vector2(1.01f, -0.3f),
		new Vector2(0.81f, -0.8f),
		new Vector2(0.48f, -1.02f),
		new Vector2(0.23f, -0.81f),
		new Vector2(0.44f, -0.81f),
		new Vector2(0.94f, -1.36f),
		new Vector2(1.19f, -0.73f),
		new Vector2(1.19f, -0.62f),
		new Vector2(0.89f, -0.62f)
	};

	// Token: 0x040008B4 RID: 2228
	public static readonly Vector2[] ps4SmallOffset = new Vector2[]
	{
		new Vector2(0.69f, -0.48f),
		new Vector2(0.97f, -0.38f),
		new Vector2(0.91f, -0.34f),
		new Vector2(1.11f, 0.41f)
	};

	// Token: 0x040008B5 RID: 2229
	public Vector2[] ps4NormalOffset = new Vector2[]
	{
		new Vector2(1.74f, -1f),
		new Vector2(2.27f, -0.97f),
		new Vector2(2.78f, -1.13f),
		new Vector2(1.55f, 0.55f)
	};

	// Token: 0x040008B6 RID: 2230
	public static readonly Dictionary<int, Vector2> SwitchOffsetMapping = new Dictionary<int, Vector2>
	{
		{
			0,
			new Vector2(-0.97f, -1f)
		},
		{
			1,
			new Vector2(0f, 9.06f)
		},
		{
			2,
			new Vector2(3.24f, 9.06f)
		},
		{
			3,
			new Vector2(-8.3f, -0.8f)
		},
		{
			4,
			new Vector2(0f, 9.06f)
		},
		{
			5,
			new Vector2(-1.61f, -1f)
		},
		{
			6,
			new Vector2(-0.97f, 9.06f)
		},
		{
			7,
			new Vector2(0f, 0f)
		},
		{
			8,
			new Vector2(0f, 6f)
		}
	};

	// Token: 0x040008B7 RID: 2231
	public static readonly Dictionary<int, Vector2> PlayStation4OffsetMapping = new Dictionary<int, Vector2>
	{
		{
			0,
			new Vector2(1.1f, -1f)
		},
		{
			1,
			new Vector2(0.8f, -0.4f)
		},
		{
			2,
			new Vector2(0.8f, -0.4f)
		},
		{
			3,
			new Vector2(1f, -1f)
		},
		{
			4,
			new Vector2(0.1f, -0.1f)
		},
		{
			5,
			new Vector2(0.3f, -0.35f)
		},
		{
			6,
			new Vector2(0.5f, -0.4f)
		},
		{
			7,
			new Vector2(0.5f, 0f)
		},
		{
			8,
			new Vector2(1.2f, -1.09f)
		}
	};

	// Token: 0x040008B8 RID: 2232
	public const float PADDINGH = 25f;

	// Token: 0x040008B9 RID: 2233
	public int rewiredPlayerId;

	// Token: 0x040008BA RID: 2234
	public CupheadButton button;

	// Token: 0x040008BB RID: 2235
	[SerializeField]
	public Image glyphSymbolText;

	// Token: 0x040008BC RID: 2236
	[SerializeField]
	public Text glyphText;

	// Token: 0x040008BD RID: 2237
	[SerializeField]
	public Image glyphSymbolChar;

	// Token: 0x040008BE RID: 2238
	[SerializeField]
	public Text glyphChar;

	// Token: 0x040008BF RID: 2239
	[SerializeField]
	public RectTransform[] rectTransformTexts;

	// Token: 0x040008C0 RID: 2240
	[SerializeField]
	public Vector2 startSize = new Vector2(37f, 37f);

	// Token: 0x040008C1 RID: 2241
	[SerializeField]
	public float paddingText = 10.7f;

	// Token: 0x040008C2 RID: 2242
	[SerializeField]
	public float maxSize;

	// Token: 0x040008C3 RID: 2243
	[SerializeField]
	public CupheadGlyph.LetterOffset letterOffset;

	// Token: 0x040008C4 RID: 2244
	[SerializeField]
	public CupheadGlyph.PlatformGlyphType platformGlyphType;

	// Token: 0x040008C5 RID: 2245
	[SerializeField]
	public CustomLanguageLayout[] glyphLayouts;

	// Token: 0x040008C6 RID: 2246
	public int initialFontSize;

	// Token: 0x040008C7 RID: 2247
	public Color initialCharColor;

	// Token: 0x040008C8 RID: 2248
	public Vector3 initialScale;

	// Token: 0x040008C9 RID: 2249
	public VerticalWrapMode initialCharWrapMode;

	// Token: 0x040008CA RID: 2250
	public Material initialCharMaterial;

	// Token: 0x02000963 RID: 2403
	public enum LetterOffset
	{
		// Token: 0x0400466A RID: 18026
		Normal,
		// Token: 0x0400466B RID: 18027
		Small
	}

	// Token: 0x02000964 RID: 2404
	public enum PlatformGlyphType
	{
		// Token: 0x0400466D RID: 18029
		Normal,
		// Token: 0x0400466E RID: 18030
		TutorialInstruction,
		// Token: 0x0400466F RID: 18031
		TutorialInstructionDescend,
		// Token: 0x04004670 RID: 18032
		LevelUIInteractionDialogue,
		// Token: 0x04004671 RID: 18033
		Shop,
		// Token: 0x04004672 RID: 18034
		SwitchWeapon,
		// Token: 0x04004673 RID: 18035
		ShmupTutorial,
		// Token: 0x04004674 RID: 18036
		Equip,
		// Token: 0x04004675 RID: 18037
		OffsetPrompt
	}
}
