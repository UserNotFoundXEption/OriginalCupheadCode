using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000680 RID: 1664
	public class TMP_Text : MaskableGraphic
	{
		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06004634 RID: 17972 RVA: 0x00037D22 File Offset: 0x00035F22
		// (set) Token: 0x06004635 RID: 17973 RVA: 0x0014F6C4 File Offset: 0x0014D8C4
		public string text
		{
			get
			{
				return this.m_text;
			}
			set
			{
				if (this.m_text == value)
				{
					return;
				}
				this.m_text = value;
				this.m_inputSource = TMP_Text.TextInputSources.Text;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06004636 RID: 17974 RVA: 0x00037D2A File Offset: 0x00035F2A
		// (set) Token: 0x06004637 RID: 17975 RVA: 0x0014F714 File Offset: 0x0014D914
		public TMP_FontAsset font
		{
			get
			{
				return this.m_fontAsset;
			}
			set
			{
				if (this.m_fontAsset == value)
				{
					return;
				}
				this.m_fontAsset = value;
				this.LoadFontAsset();
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x06004638 RID: 17976 RVA: 0x00037D32 File Offset: 0x00035F32
		// (set) Token: 0x06004639 RID: 17977 RVA: 0x00037D3A File Offset: 0x00035F3A
		public virtual Material fontSharedMaterial
		{
			get
			{
				return this.m_sharedMaterial;
			}
			set
			{
				if (this.m_sharedMaterial == value)
				{
					return;
				}
				this.SetSharedMaterial(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000668 RID: 1640
		// (get) Token: 0x0600463A RID: 17978 RVA: 0x00037D6F File Offset: 0x00035F6F
		// (set) Token: 0x0600463B RID: 17979 RVA: 0x00037D77 File Offset: 0x00035F77
		public virtual Material[] fontSharedMaterials
		{
			get
			{
				return this.GetSharedMaterials();
			}
			set
			{
				this.SetSharedMaterials(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000669 RID: 1641
		// (get) Token: 0x0600463C RID: 17980 RVA: 0x00037D9A File Offset: 0x00035F9A
		// (set) Token: 0x0600463D RID: 17981 RVA: 0x0014F764 File Offset: 0x0014D964
		public Material fontMaterial
		{
			get
			{
				return this.GetMaterial(this.m_sharedMaterial);
			}
			set
			{
				if (this.m_sharedMaterial != null && this.m_sharedMaterial.GetInstanceID() == value.GetInstanceID())
				{
					return;
				}
				this.m_sharedMaterial = value;
				this.m_padding = this.GetPaddingForMaterial();
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x1700066A RID: 1642
		// (get) Token: 0x0600463E RID: 17982 RVA: 0x00037DA8 File Offset: 0x00035FA8
		// (set) Token: 0x0600463F RID: 17983 RVA: 0x00037DB6 File Offset: 0x00035FB6
		public virtual Material[] fontMaterials
		{
			get
			{
				return this.GetMaterials(this.m_fontSharedMaterials);
			}
			set
			{
				this.SetSharedMaterials(value);
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x1700066B RID: 1643
		// (get) Token: 0x06004640 RID: 17984 RVA: 0x00037DD9 File Offset: 0x00035FD9
		// (set) Token: 0x06004641 RID: 17985 RVA: 0x00037DE1 File Offset: 0x00035FE1
		public Color color
		{
			get
			{
				return this.m_fontColor;
			}
			set
			{
				if (this.m_fontColor == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_fontColor = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x06004642 RID: 17986 RVA: 0x00037E09 File Offset: 0x00036009
		// (set) Token: 0x06004643 RID: 17987 RVA: 0x00037E16 File Offset: 0x00036016
		public float alpha
		{
			get
			{
				return this.m_fontColor.a;
			}
			set
			{
				if (this.m_fontColor.a == value)
				{
					return;
				}
				this.m_fontColor.a = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x06004644 RID: 17988 RVA: 0x00037E43 File Offset: 0x00036043
		// (set) Token: 0x06004645 RID: 17989 RVA: 0x00037E4B File Offset: 0x0003604B
		public bool enableVertexGradient
		{
			get
			{
				return this.m_enableVertexGradient;
			}
			set
			{
				if (this.m_enableVertexGradient == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_enableVertexGradient = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06004646 RID: 17990 RVA: 0x00037E6E File Offset: 0x0003606E
		// (set) Token: 0x06004647 RID: 17991 RVA: 0x00037E76 File Offset: 0x00036076
		public VertexGradient colorGradient
		{
			get
			{
				return this.m_fontColorGradient;
			}
			set
			{
				this.m_havePropertiesChanged = true;
				this.m_fontColorGradient = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06004648 RID: 17992 RVA: 0x00037E8C File Offset: 0x0003608C
		// (set) Token: 0x06004649 RID: 17993 RVA: 0x00037E94 File Offset: 0x00036094
		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return this.m_spriteAsset;
			}
			set
			{
				this.m_spriteAsset = value;
			}
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x0600464A RID: 17994 RVA: 0x00037E9D File Offset: 0x0003609D
		// (set) Token: 0x0600464B RID: 17995 RVA: 0x00037EA5 File Offset: 0x000360A5
		public bool tintAllSprites
		{
			get
			{
				return this.m_tintAllSprites;
			}
			set
			{
				if (this.m_tintAllSprites == value)
				{
					return;
				}
				this.m_tintAllSprites = value;
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x0600464C RID: 17996 RVA: 0x00037EC8 File Offset: 0x000360C8
		// (set) Token: 0x0600464D RID: 17997 RVA: 0x00037ED0 File Offset: 0x000360D0
		public bool overrideColorTags
		{
			get
			{
				return this.m_overrideHtmlColors;
			}
			set
			{
				if (this.m_overrideHtmlColors == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_overrideHtmlColors = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x00037EF3 File Offset: 0x000360F3
		// (set) Token: 0x0600464F RID: 17999 RVA: 0x00037F2E File Offset: 0x0003612E
		public Color32 faceColor
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_faceColor;
				}
				this.m_faceColor = this.m_sharedMaterial.GetColor(ShaderUtilities.ID_FaceColor);
				return this.m_faceColor;
			}
			set
			{
				if (this.m_faceColor.Compare(value))
				{
					return;
				}
				this.SetFaceColor(value);
				this.m_havePropertiesChanged = true;
				this.m_faceColor = value;
				this.SetVerticesDirty();
				this.SetMaterialDirty();
			}
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x06004650 RID: 18000 RVA: 0x00037F63 File Offset: 0x00036163
		// (set) Token: 0x06004651 RID: 18001 RVA: 0x00037F9E File Offset: 0x0003619E
		public Color32 outlineColor
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_outlineColor;
				}
				this.m_outlineColor = this.m_sharedMaterial.GetColor(ShaderUtilities.ID_OutlineColor);
				return this.m_outlineColor;
			}
			set
			{
				if (this.m_outlineColor.Compare(value))
				{
					return;
				}
				this.SetOutlineColor(value);
				this.m_havePropertiesChanged = true;
				this.m_outlineColor = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x06004652 RID: 18002 RVA: 0x00037FCD File Offset: 0x000361CD
		// (set) Token: 0x06004653 RID: 18003 RVA: 0x00038003 File Offset: 0x00036203
		public float outlineWidth
		{
			get
			{
				if (this.m_sharedMaterial == null)
				{
					return this.m_outlineWidth;
				}
				this.m_outlineWidth = this.m_sharedMaterial.GetFloat(ShaderUtilities.ID_OutlineWidth);
				return this.m_outlineWidth;
			}
			set
			{
				if (this.m_outlineWidth == value)
				{
					return;
				}
				this.SetOutlineThickness(value);
				this.m_havePropertiesChanged = true;
				this.m_outlineWidth = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06004654 RID: 18004 RVA: 0x0003802D File Offset: 0x0003622D
		// (set) Token: 0x06004655 RID: 18005 RVA: 0x0014F7C8 File Offset: 0x0014D9C8
		public float fontSize
		{
			get
			{
				return this.m_fontSize;
			}
			set
			{
				if (this.m_fontSize == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_fontSize = value;
				if (!this.m_enableAutoSizing)
				{
					this.m_fontSizeBase = this.m_fontSize;
				}
			}
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x06004656 RID: 18006 RVA: 0x00038035 File Offset: 0x00036235
		public float fontScale
		{
			get
			{
				return this.m_fontScale;
			}
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06004657 RID: 18007 RVA: 0x0003803D File Offset: 0x0003623D
		// (set) Token: 0x06004658 RID: 18008 RVA: 0x00038045 File Offset: 0x00036245
		public int fontWeight
		{
			get
			{
				return this.m_fontWeight;
			}
			set
			{
				if (this.m_fontWeight == value)
				{
					return;
				}
				this.m_fontWeight = value;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06004659 RID: 18009 RVA: 0x0014F81C File Offset: 0x0014DA1C
		public float pixelsPerUnit
		{
			get
			{
				Canvas canvas = base.canvas;
				if (!canvas)
				{
					return 1f;
				}
				if (!this.font)
				{
					return canvas.scaleFactor;
				}
				if (this.m_currentFontAsset == null || this.m_currentFontAsset.fontInfo.PointSize <= 0f || this.m_fontSize <= 0f)
				{
					return 1f;
				}
				return this.m_fontSize / this.m_currentFontAsset.fontInfo.PointSize;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x0600465A RID: 18010 RVA: 0x0003806E File Offset: 0x0003626E
		// (set) Token: 0x0600465B RID: 18011 RVA: 0x00038076 File Offset: 0x00036276
		public bool enableAutoSizing
		{
			get
			{
				return this.m_enableAutoSizing;
			}
			set
			{
				if (this.m_enableAutoSizing == value)
				{
					return;
				}
				this.m_enableAutoSizing = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x0600465C RID: 18012 RVA: 0x00038098 File Offset: 0x00036298
		// (set) Token: 0x0600465D RID: 18013 RVA: 0x000380A0 File Offset: 0x000362A0
		public float fontSizeMin
		{
			get
			{
				return this.m_fontSizeMin;
			}
			set
			{
				if (this.m_fontSizeMin == value)
				{
					return;
				}
				this.m_fontSizeMin = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x0600465E RID: 18014 RVA: 0x000380C2 File Offset: 0x000362C2
		// (set) Token: 0x0600465F RID: 18015 RVA: 0x000380CA File Offset: 0x000362CA
		public float fontSizeMax
		{
			get
			{
				return this.m_fontSizeMax;
			}
			set
			{
				if (this.m_fontSizeMax == value)
				{
					return;
				}
				this.m_fontSizeMax = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x06004660 RID: 18016 RVA: 0x000380EC File Offset: 0x000362EC
		// (set) Token: 0x06004661 RID: 18017 RVA: 0x000380F4 File Offset: 0x000362F4
		public FontStyles fontStyle
		{
			get
			{
				return this.m_fontStyle;
			}
			set
			{
				if (this.m_fontStyle == value)
				{
					return;
				}
				this.m_fontStyle = value;
				this.m_havePropertiesChanged = true;
				this.checkPaddingRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06004662 RID: 18018 RVA: 0x00038124 File Offset: 0x00036324
		public bool isUsingBold
		{
			get
			{
				return this.m_isUsingBold;
			}
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x06004663 RID: 18019 RVA: 0x0003812C File Offset: 0x0003632C
		// (set) Token: 0x06004664 RID: 18020 RVA: 0x00038134 File Offset: 0x00036334
		public TextAlignmentOptions alignment
		{
			get
			{
				return this.m_textAlignment;
			}
			set
			{
				if (this.m_textAlignment == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_textAlignment = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x06004665 RID: 18021 RVA: 0x00038157 File Offset: 0x00036357
		// (set) Token: 0x06004666 RID: 18022 RVA: 0x0003815F File Offset: 0x0003635F
		public float characterSpacing
		{
			get
			{
				return this.m_characterSpacing;
			}
			set
			{
				if (this.m_characterSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_characterSpacing = value;
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x06004667 RID: 18023 RVA: 0x0003818F File Offset: 0x0003638F
		// (set) Token: 0x06004668 RID: 18024 RVA: 0x00038197 File Offset: 0x00036397
		public float lineSpacing
		{
			get
			{
				return this.m_lineSpacing;
			}
			set
			{
				if (this.m_lineSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_lineSpacing = value;
			}
		}

		// Token: 0x17000681 RID: 1665
		// (get) Token: 0x06004669 RID: 18025 RVA: 0x000381C7 File Offset: 0x000363C7
		// (set) Token: 0x0600466A RID: 18026 RVA: 0x000381CF File Offset: 0x000363CF
		public float paragraphSpacing
		{
			get
			{
				return this.m_paragraphSpacing;
			}
			set
			{
				if (this.m_paragraphSpacing == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_paragraphSpacing = value;
			}
		}

		// Token: 0x17000682 RID: 1666
		// (get) Token: 0x0600466B RID: 18027 RVA: 0x000381FF File Offset: 0x000363FF
		// (set) Token: 0x0600466C RID: 18028 RVA: 0x00038207 File Offset: 0x00036407
		public float characterWidthAdjustment
		{
			get
			{
				return this.m_charWidthMaxAdj;
			}
			set
			{
				if (this.m_charWidthMaxAdj == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_charWidthMaxAdj = value;
			}
		}

		// Token: 0x17000683 RID: 1667
		// (get) Token: 0x0600466D RID: 18029 RVA: 0x00038237 File Offset: 0x00036437
		// (set) Token: 0x0600466E RID: 18030 RVA: 0x0003823F File Offset: 0x0003643F
		public bool enableWordWrapping
		{
			get
			{
				return this.m_enableWordWrapping;
			}
			set
			{
				if (this.m_enableWordWrapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_isCalculateSizeRequired = true;
				this.m_enableWordWrapping = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000684 RID: 1668
		// (get) Token: 0x0600466F RID: 18031 RVA: 0x00038276 File Offset: 0x00036476
		// (set) Token: 0x06004670 RID: 18032 RVA: 0x0003827E File Offset: 0x0003647E
		public float wordWrappingRatios
		{
			get
			{
				return this.m_wordWrappingRatios;
			}
			set
			{
				if (this.m_wordWrappingRatios == value)
				{
					return;
				}
				this.m_wordWrappingRatios = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000685 RID: 1669
		// (get) Token: 0x06004671 RID: 18033 RVA: 0x000382AE File Offset: 0x000364AE
		// (set) Token: 0x06004672 RID: 18034 RVA: 0x000382B6 File Offset: 0x000364B6
		public TextOverflowModes OverflowMode
		{
			get
			{
				return this.m_overflowMode;
			}
			set
			{
				if (this.m_overflowMode == value)
				{
					return;
				}
				this.m_overflowMode = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000686 RID: 1670
		// (get) Token: 0x06004673 RID: 18035 RVA: 0x000382E6 File Offset: 0x000364E6
		// (set) Token: 0x06004674 RID: 18036 RVA: 0x000382EE File Offset: 0x000364EE
		public bool enableKerning
		{
			get
			{
				return this.m_enableKerning;
			}
			set
			{
				if (this.m_enableKerning == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_enableKerning = value;
			}
		}

		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x06004675 RID: 18037 RVA: 0x0003831E File Offset: 0x0003651E
		// (set) Token: 0x06004676 RID: 18038 RVA: 0x00038326 File Offset: 0x00036526
		public bool extraPadding
		{
			get
			{
				return this.m_enableExtraPadding;
			}
			set
			{
				if (this.m_enableExtraPadding == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_enableExtraPadding = value;
				this.UpdateMeshPadding();
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x06004677 RID: 18039 RVA: 0x0003834F File Offset: 0x0003654F
		// (set) Token: 0x06004678 RID: 18040 RVA: 0x00038357 File Offset: 0x00036557
		public bool richText
		{
			get
			{
				return this.m_isRichText;
			}
			set
			{
				if (this.m_isRichText == value)
				{
					return;
				}
				this.m_isRichText = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_isInputParsingRequired = true;
			}
		}

		// Token: 0x17000689 RID: 1673
		// (get) Token: 0x06004679 RID: 18041 RVA: 0x0003838E File Offset: 0x0003658E
		// (set) Token: 0x0600467A RID: 18042 RVA: 0x00038396 File Offset: 0x00036596
		public bool parseCtrlCharacters
		{
			get
			{
				return this.m_parseCtrlCharacters;
			}
			set
			{
				if (this.m_parseCtrlCharacters == value)
				{
					return;
				}
				this.m_parseCtrlCharacters = value;
				this.m_havePropertiesChanged = true;
				this.m_isCalculateSizeRequired = true;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
				this.m_isInputParsingRequired = true;
			}
		}

		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x0600467B RID: 18043 RVA: 0x000383CD File Offset: 0x000365CD
		// (set) Token: 0x0600467C RID: 18044 RVA: 0x000383D5 File Offset: 0x000365D5
		public bool isOverlay
		{
			get
			{
				return this.m_isOverlay;
			}
			set
			{
				if (this.m_isOverlay == value)
				{
					return;
				}
				this.m_isOverlay = value;
				this.SetShaderDepth();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x0600467D RID: 18045 RVA: 0x000383FE File Offset: 0x000365FE
		// (set) Token: 0x0600467E RID: 18046 RVA: 0x00038406 File Offset: 0x00036606
		public bool isOrthographic
		{
			get
			{
				return this.m_isOrthographic;
			}
			set
			{
				if (this.m_isOrthographic == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isOrthographic = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x0600467F RID: 18047 RVA: 0x00038429 File Offset: 0x00036629
		// (set) Token: 0x06004680 RID: 18048 RVA: 0x00038431 File Offset: 0x00036631
		public bool enableCulling
		{
			get
			{
				return this.m_isCullingEnabled;
			}
			set
			{
				if (this.m_isCullingEnabled == value)
				{
					return;
				}
				this.m_isCullingEnabled = value;
				this.SetCulling();
				this.m_havePropertiesChanged = true;
			}
		}

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06004681 RID: 18049 RVA: 0x00038454 File Offset: 0x00036654
		// (set) Token: 0x06004682 RID: 18050 RVA: 0x0003845C File Offset: 0x0003665C
		public bool ignoreVisibility
		{
			get
			{
				return this.m_ignoreCulling;
			}
			set
			{
				if (this.m_ignoreCulling == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_ignoreCulling = value;
			}
		}

		// Token: 0x1700068E RID: 1678
		// (get) Token: 0x06004683 RID: 18051 RVA: 0x00038479 File Offset: 0x00036679
		// (set) Token: 0x06004684 RID: 18052 RVA: 0x00038481 File Offset: 0x00036681
		public TextureMappingOptions horizontalMapping
		{
			get
			{
				return this.m_horizontalMapping;
			}
			set
			{
				if (this.m_horizontalMapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_horizontalMapping = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x1700068F RID: 1679
		// (get) Token: 0x06004685 RID: 18053 RVA: 0x000384A4 File Offset: 0x000366A4
		// (set) Token: 0x06004686 RID: 18054 RVA: 0x000384AC File Offset: 0x000366AC
		public TextureMappingOptions verticalMapping
		{
			get
			{
				return this.m_verticalMapping;
			}
			set
			{
				if (this.m_verticalMapping == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_verticalMapping = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000690 RID: 1680
		// (get) Token: 0x06004687 RID: 18055 RVA: 0x000384CF File Offset: 0x000366CF
		// (set) Token: 0x06004688 RID: 18056 RVA: 0x000384D7 File Offset: 0x000366D7
		public TextRenderFlags renderMode
		{
			get
			{
				return this.m_renderMode;
			}
			set
			{
				if (this.m_renderMode == value)
				{
					return;
				}
				this.m_renderMode = value;
				this.m_havePropertiesChanged = true;
			}
		}

		// Token: 0x17000691 RID: 1681
		// (get) Token: 0x06004689 RID: 18057 RVA: 0x000384F4 File Offset: 0x000366F4
		// (set) Token: 0x0600468A RID: 18058 RVA: 0x000384FC File Offset: 0x000366FC
		public int maxVisibleCharacters
		{
			get
			{
				return this.m_maxVisibleCharacters;
			}
			set
			{
				if (this.m_maxVisibleCharacters == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_maxVisibleCharacters = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000692 RID: 1682
		// (get) Token: 0x0600468B RID: 18059 RVA: 0x0003851F File Offset: 0x0003671F
		// (set) Token: 0x0600468C RID: 18060 RVA: 0x00038527 File Offset: 0x00036727
		public int maxVisibleWords
		{
			get
			{
				return this.m_maxVisibleWords;
			}
			set
			{
				if (this.m_maxVisibleWords == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_maxVisibleWords = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000693 RID: 1683
		// (get) Token: 0x0600468D RID: 18061 RVA: 0x0003854A File Offset: 0x0003674A
		// (set) Token: 0x0600468E RID: 18062 RVA: 0x00038552 File Offset: 0x00036752
		public int maxVisibleLines
		{
			get
			{
				return this.m_maxVisibleLines;
			}
			set
			{
				if (this.m_maxVisibleLines == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_isInputParsingRequired = true;
				this.m_maxVisibleLines = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x0600468F RID: 18063 RVA: 0x0003857C File Offset: 0x0003677C
		// (set) Token: 0x06004690 RID: 18064 RVA: 0x00038584 File Offset: 0x00036784
		public int pageToDisplay
		{
			get
			{
				return this.m_pageToDisplay;
			}
			set
			{
				if (this.m_pageToDisplay == value)
				{
					return;
				}
				this.m_havePropertiesChanged = true;
				this.m_pageToDisplay = value;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06004691 RID: 18065 RVA: 0x000385A7 File Offset: 0x000367A7
		// (set) Token: 0x06004692 RID: 18066 RVA: 0x000385AF File Offset: 0x000367AF
		public virtual Vector4 margin
		{
			get
			{
				return this.m_margin;
			}
			set
			{
				if (this.m_margin == value)
				{
					return;
				}
				this.m_margin = value;
				this.ComputeMarginSize();
				this.m_havePropertiesChanged = true;
				this.SetVerticesDirty();
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06004693 RID: 18067 RVA: 0x000385DD File Offset: 0x000367DD
		public TMP_TextInfo textInfo
		{
			get
			{
				return this.m_textInfo;
			}
		}

		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x06004694 RID: 18068 RVA: 0x000385E5 File Offset: 0x000367E5
		// (set) Token: 0x06004695 RID: 18069 RVA: 0x000385ED File Offset: 0x000367ED
		public bool havePropertiesChanged
		{
			get
			{
				return this.m_havePropertiesChanged;
			}
			set
			{
				if (this.m_havePropertiesChanged == value)
				{
					return;
				}
				this.m_havePropertiesChanged = value;
				this.SetVerticesDirty();
				this.SetLayoutDirty();
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x06004696 RID: 18070 RVA: 0x0003860F File Offset: 0x0003680F
		// (set) Token: 0x06004697 RID: 18071 RVA: 0x00038617 File Offset: 0x00036817
		public bool isUsingLegacyAnimationComponent
		{
			get
			{
				return this.m_isUsingLegacyAnimationComponent;
			}
			set
			{
				this.m_isUsingLegacyAnimationComponent = value;
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06004698 RID: 18072 RVA: 0x00038620 File Offset: 0x00036820
		public Transform transform
		{
			get
			{
				if (this.m_transform == null)
				{
					this.m_transform = base.GetComponent<Transform>();
				}
				return this.m_transform;
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x06004699 RID: 18073 RVA: 0x00038645 File Offset: 0x00036845
		public RectTransform rectTransform
		{
			get
			{
				if (this.m_rectTransform == null)
				{
					this.m_rectTransform = base.GetComponent<RectTransform>();
				}
				return this.m_rectTransform;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600469A RID: 18074 RVA: 0x0003866A File Offset: 0x0003686A
		// (set) Token: 0x0600469B RID: 18075 RVA: 0x00038672 File Offset: 0x00036872
		public virtual bool autoSizeTextContainer { get; set; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600469C RID: 18076 RVA: 0x0003867B File Offset: 0x0003687B
		public virtual Mesh mesh
		{
			get
			{
				return this.m_mesh;
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x0600469D RID: 18077 RVA: 0x00038683 File Offset: 0x00036883
		// (set) Token: 0x0600469E RID: 18078 RVA: 0x0003868B File Offset: 0x0003688B
		public virtual Bounds bounds { get; set; }

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x0600469F RID: 18079 RVA: 0x00038694 File Offset: 0x00036894
		public float flexibleHeight
		{
			get
			{
				return this.m_flexibleHeight;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060046A0 RID: 18080 RVA: 0x0003869C File Offset: 0x0003689C
		public float flexibleWidth
		{
			get
			{
				return this.m_flexibleWidth;
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060046A1 RID: 18081 RVA: 0x000386A4 File Offset: 0x000368A4
		public float minHeight
		{
			get
			{
				return this.m_minHeight;
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060046A2 RID: 18082 RVA: 0x000386AC File Offset: 0x000368AC
		public float minWidth
		{
			get
			{
				return this.m_minWidth;
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060046A3 RID: 18083 RVA: 0x000386B4 File Offset: 0x000368B4
		public virtual float preferredWidth
		{
			get
			{
				return (this.m_preferredWidth != 9999f) ? this.m_preferredWidth : this.GetPreferredWidth();
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060046A4 RID: 18084 RVA: 0x000386D7 File Offset: 0x000368D7
		public virtual float preferredHeight
		{
			get
			{
				return (this.m_preferredHeight != 9999f) ? this.m_preferredHeight : this.GetPreferredHeight();
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060046A5 RID: 18085 RVA: 0x000386FA File Offset: 0x000368FA
		public int layoutPriority
		{
			get
			{
				return this.m_layoutPriority;
			}
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x00038702 File Offset: 0x00036902
		public virtual void LoadFontAsset()
		{
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x00038704 File Offset: 0x00036904
		public virtual void SetSharedMaterial(Material mat)
		{
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x00038706 File Offset: 0x00036906
		public virtual Material GetMaterial(Material mat)
		{
			return null;
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x00038709 File Offset: 0x00036909
		public virtual void SetFontBaseMaterial(Material mat)
		{
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x0003870B File Offset: 0x0003690B
		public virtual Material[] GetSharedMaterials()
		{
			return null;
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x0003870E File Offset: 0x0003690E
		public virtual void SetSharedMaterials(Material[] materials)
		{
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x00038710 File Offset: 0x00036910
		public virtual Material[] GetMaterials(Material[] mats)
		{
			return null;
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x0014F8B0 File Offset: 0x0014DAB0
		public virtual Material CreateMaterialInstance(Material source)
		{
			Material material = new Material(source);
			material.shaderKeywords = source.shaderKeywords;
			Material material2 = material;
			material2.name += " (Instance)";
			return material;
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x00038713 File Offset: 0x00036913
		public virtual void SetFaceColor(Color32 color)
		{
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x00038715 File Offset: 0x00036915
		public virtual void SetOutlineColor(Color32 color)
		{
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x00038717 File Offset: 0x00036917
		public virtual void SetOutlineThickness(float thickness)
		{
		}

		// Token: 0x060046B1 RID: 18097 RVA: 0x00038719 File Offset: 0x00036919
		public virtual void SetShaderDepth()
		{
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x0003871B File Offset: 0x0003691B
		public virtual void SetCulling()
		{
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x0003871D File Offset: 0x0003691D
		public virtual float GetPaddingForMaterial()
		{
			return 0f;
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x00038724 File Offset: 0x00036924
		public virtual float GetPaddingForMaterial(Material mat)
		{
			return 0f;
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x0003872B File Offset: 0x0003692B
		public virtual Vector3[] GetTextContainerLocalCorners()
		{
			return null;
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x0003872E File Offset: 0x0003692E
		public virtual void ForceMeshUpdate()
		{
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x00038730 File Offset: 0x00036930
		public virtual void UpdateGeometry(Mesh mesh, int index)
		{
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x00038732 File Offset: 0x00036932
		public virtual void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x00038734 File Offset: 0x00036934
		public virtual void UpdateVertexData()
		{
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x00038736 File Offset: 0x00036936
		public virtual void SetVertices(Vector3[] vertices)
		{
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x00038738 File Offset: 0x00036938
		public virtual void UpdateMeshPadding()
		{
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x0003873A File Offset: 0x0003693A
		public void SetText(string text)
		{
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x00038771 File Offset: 0x00036971
		public void SetText(string text, float arg0)
		{
			this.SetText(text, arg0, 255f, 255f);
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x00038785 File Offset: 0x00036985
		public void SetText(string text, float arg0, float arg1)
		{
			this.SetText(text, arg0, arg1, 255f);
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x0014F8E8 File Offset: 0x0014DAE8
		public void SetText(string text, float arg0, float arg1, float arg2)
		{
			if (text == this.old_text && arg0 == this.old_arg0 && arg1 == this.old_arg1 && arg2 == this.old_arg2)
			{
				return;
			}
			this.old_text = text;
			this.old_arg1 = 255f;
			this.old_arg2 = 255f;
			int precision = 0;
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '{')
				{
					if (text[i + 2] == ':')
					{
						precision = (int)(text[i + 3] - '0');
					}
					int num2 = (int)(text[i + 1] - '0');
					if (num2 != 0)
					{
						if (num2 != 1)
						{
							if (num2 == 2)
							{
								this.old_arg2 = arg2;
								this.AddFloatToCharArray(arg2, ref num, precision);
							}
						}
						else
						{
							this.old_arg1 = arg1;
							this.AddFloatToCharArray(arg1, ref num, precision);
						}
					}
					else
					{
						this.old_arg0 = arg0;
						this.AddFloatToCharArray(arg0, ref num, precision);
					}
					if (text[i + 2] == ':')
					{
						i += 4;
					}
					else
					{
						i += 2;
					}
				}
				else
				{
					this.m_input_CharArray[num] = c;
					num++;
				}
			}
			this.m_input_CharArray[num] = '\0';
			this.m_charArray_Length = num;
			this.m_inputSource = TMP_Text.TextInputSources.SetText;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x00038795 File Offset: 0x00036995
		public void SetText(StringBuilder text)
		{
			this.StringBuilderToIntArray(text, ref this.m_char_buffer);
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_isInputParsingRequired = true;
			this.m_havePropertiesChanged = true;
			this.m_isCalculateSizeRequired = true;
			this.SetVerticesDirty();
			this.SetLayoutDirty();
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x0014FA68 File Offset: 0x0014DC68
		public void SetCharArray(char[] charArray)
		{
			if (charArray == null || charArray.Length == 0)
			{
				return;
			}
			if (this.m_char_buffer.Length <= charArray.Length)
			{
				int num = Mathf.NextPowerOfTwo(charArray.Length + 1);
				this.m_char_buffer = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < charArray.Length)
			{
				if (charArray[i] != '\\' || i >= charArray.Length - 1)
				{
					goto IL_BC;
				}
				int num3 = (int)charArray[i + 1];
				if (num3 != 110)
				{
					if (num3 != 114)
					{
						if (num3 != 116)
						{
							goto IL_BC;
						}
						this.m_char_buffer[num2] = 9;
						i++;
						num2++;
					}
					else
					{
						this.m_char_buffer[num2] = 13;
						i++;
						num2++;
					}
				}
				else
				{
					this.m_char_buffer[num2] = 10;
					i++;
					num2++;
				}
				IL_CB:
				i++;
				continue;
				IL_BC:
				this.m_char_buffer[num2] = (int)charArray[i];
				num2++;
				goto IL_CB;
			}
			this.m_char_buffer[num2] = 0;
			this.m_inputSource = TMP_Text.TextInputSources.SetCharArray;
			this.m_havePropertiesChanged = true;
			this.m_isInputParsingRequired = true;
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x0014FB6C File Offset: 0x0014DD6C
		public void SetTextArrayToCharArray(char[] charArray, ref int[] charBuffer)
		{
			if (charArray == null || this.m_charArray_Length == 0)
			{
				return;
			}
			if (charBuffer.Length <= this.m_charArray_Length)
			{
				int num = (this.m_charArray_Length <= 1024) ? Mathf.NextPowerOfTwo(this.m_charArray_Length + 1) : (this.m_charArray_Length + 256);
				charBuffer = new int[num];
			}
			int num2 = 0;
			for (int i = 0; i < this.m_charArray_Length; i++)
			{
				if (char.IsHighSurrogate(charArray[i]) && char.IsLowSurrogate(charArray[i + 1]))
				{
					charBuffer[num2] = char.ConvertToUtf32(charArray[i], charArray[i + 1]);
					i++;
					num2++;
				}
				else
				{
					charBuffer[num2] = (int)charArray[i];
					num2++;
				}
			}
			charBuffer[num2] = 0;
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x0014FC34 File Offset: 0x0014DE34
		public void StringToCharArray(string text, ref int[] chars)
		{
			if (text == null)
			{
				chars[0] = 0;
				return;
			}
			if (chars == null || chars.Length <= text.Length)
			{
				int num = (text.Length <= 1024) ? Mathf.NextPowerOfTwo(text.Length + 1) : (text.Length + 256);
				chars = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < text.Length)
			{
				if (!this.m_parseCtrlCharacters || text[i] != '\\' || text.Length <= i + 1)
				{
					goto IL_19B;
				}
				int num3 = (int)text[i + 1];
				switch (num3)
				{
				case 114:
					chars[num2] = 13;
					i++;
					num2++;
					break;
				default:
					if (num3 != 85)
					{
						if (num3 != 92)
						{
							if (num3 != 110)
							{
								goto IL_19B;
							}
							chars[num2] = 10;
							i++;
							num2++;
						}
						else
						{
							if (text.Length <= i + 2)
							{
								goto IL_19B;
							}
							chars[num2] = (int)text[i + 1];
							chars[num2 + 1] = (int)text[i + 2];
							i += 2;
							num2 += 2;
						}
					}
					else
					{
						if (text.Length <= i + 9)
						{
							goto IL_19B;
						}
						chars[num2] = this.GetUTF32(i + 2);
						i += 9;
						num2++;
					}
					break;
				case 116:
					chars[num2] = 9;
					i++;
					num2++;
					break;
				case 117:
					if (text.Length <= i + 5)
					{
						goto IL_19B;
					}
					chars[num2] = (int)((ushort)this.GetUTF16(i + 2));
					i += 5;
					num2++;
					break;
				}
				IL_1F4:
				i++;
				continue;
				IL_19B:
				if (char.IsHighSurrogate(text[i]) && char.IsLowSurrogate(text[i + 1]))
				{
					chars[num2] = char.ConvertToUtf32(text[i], text[i + 1]);
					i++;
					num2++;
					goto IL_1F4;
				}
				chars[num2] = (int)text[i];
				num2++;
				goto IL_1F4;
			}
			chars[num2] = 0;
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x0014FE4C File Offset: 0x0014E04C
		public void StringBuilderToIntArray(StringBuilder text, ref int[] chars)
		{
			if (text == null)
			{
				chars[0] = 0;
				return;
			}
			if (chars == null || chars.Length <= text.Length)
			{
				int num = (text.Length <= 1024) ? Mathf.NextPowerOfTwo(text.Length + 1) : (text.Length + 256);
				chars = new int[num];
			}
			int num2 = 0;
			int i = 0;
			while (i < text.Length)
			{
				if (!this.m_parseCtrlCharacters || text[i] != '\\' || text.Length <= i + 1)
				{
					goto IL_19B;
				}
				int num3 = (int)text[i + 1];
				switch (num3)
				{
				case 114:
					chars[num2] = 13;
					i++;
					num2++;
					break;
				default:
					if (num3 != 85)
					{
						if (num3 != 92)
						{
							if (num3 != 110)
							{
								goto IL_19B;
							}
							chars[num2] = 10;
							i++;
							num2++;
						}
						else
						{
							if (text.Length <= i + 2)
							{
								goto IL_19B;
							}
							chars[num2] = (int)text[i + 1];
							chars[num2 + 1] = (int)text[i + 2];
							i += 2;
							num2 += 2;
						}
					}
					else
					{
						if (text.Length <= i + 9)
						{
							goto IL_19B;
						}
						chars[num2] = this.GetUTF32(i + 2);
						i += 9;
						num2++;
					}
					break;
				case 116:
					chars[num2] = 9;
					i++;
					num2++;
					break;
				case 117:
					if (text.Length <= i + 5)
					{
						goto IL_19B;
					}
					chars[num2] = (int)((ushort)this.GetUTF16(i + 2));
					i += 5;
					num2++;
					break;
				}
				IL_1F4:
				i++;
				continue;
				IL_19B:
				if (char.IsHighSurrogate(text[i]) && char.IsLowSurrogate(text[i + 1]))
				{
					chars[num2] = char.ConvertToUtf32(text[i], text[i + 1]);
					i++;
					num2++;
					goto IL_1F4;
				}
				chars[num2] = (int)text[i];
				num2++;
				goto IL_1F4;
			}
			chars[num2] = 0;
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x00150064 File Offset: 0x0014E264
		public void AddFloatToCharArray(float number, ref int index, int precision)
		{
			if (number < 0f)
			{
				this.m_input_CharArray[index++] = '-';
				number = -number;
			}
			number += this.k_Power[Mathf.Min(9, precision)];
			int num = (int)number;
			this.AddIntToCharArray(num, ref index, precision);
			if (precision > 0)
			{
				this.m_input_CharArray[index++] = '.';
				number -= (float)num;
				for (int i = 0; i < precision; i++)
				{
					number *= 10f;
					int num2 = (int)number;
					this.m_input_CharArray[index++] = (char)(num2 + 48);
					number -= (float)num2;
				}
			}
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x0015010C File Offset: 0x0014E30C
		public void AddIntToCharArray(int number, ref int index, int precision)
		{
			if (number < 0)
			{
				this.m_input_CharArray[index++] = '-';
				number = -number;
			}
			int num = index;
			do
			{
				this.m_input_CharArray[num++] = (char)(number % 10 + 48);
				number /= 10;
			}
			while (number > 0);
			int num2 = num;
			while (index + 1 < num)
			{
				num--;
				char c = this.m_input_CharArray[index];
				this.m_input_CharArray[index] = this.m_input_CharArray[num];
				this.m_input_CharArray[num] = c;
				index++;
			}
			index = num2;
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x000387CC File Offset: 0x000369CC
		public virtual int SetArraySizes(int[] chars)
		{
			return 0;
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x0015019C File Offset: 0x0014E39C
		public void ParseInputText()
		{
			this.m_isInputParsingRequired = false;
			TMP_Text.TextInputSources inputSource = this.m_inputSource;
			if (inputSource != TMP_Text.TextInputSources.Text)
			{
				if (inputSource != TMP_Text.TextInputSources.SetText)
				{
					if (inputSource != TMP_Text.TextInputSources.SetCharArray)
					{
					}
				}
				else
				{
					this.SetTextArrayToCharArray(this.m_input_CharArray, ref this.m_char_buffer);
				}
			}
			else
			{
				this.StringToCharArray(this.m_text, ref this.m_char_buffer);
			}
			this.SetArraySizes(this.m_char_buffer);
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x000387CF File Offset: 0x000369CF
		public virtual void GenerateTextMesh()
		{
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x00150210 File Offset: 0x0014E410
		public Vector2 GetPreferredValues()
		{
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.ParseInputText();
			}
			float preferredWidth = this.GetPreferredWidth();
			float preferredHeight = this.GetPreferredHeight();
			return new Vector2(preferredWidth, preferredHeight);
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x00150250 File Offset: 0x0014E450
		public Vector2 GetPreferredValues(float width, float height)
		{
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.ParseInputText();
			}
			Vector2 margin;
			margin..ctor(width, height);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x0015029C File Offset: 0x0014E49C
		public Vector2 GetPreferredValues(string text)
		{
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			Vector2 margin;
			margin..ctor(float.PositiveInfinity, float.PositiveInfinity);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x001502EC File Offset: 0x0014E4EC
		public Vector2 GetPreferredValues(string text, float width, float height)
		{
			this.StringToCharArray(text, ref this.m_char_buffer);
			this.SetArraySizes(this.m_char_buffer);
			Vector2 margin;
			margin..ctor(width, height);
			float preferredWidth = this.GetPreferredWidth(margin);
			float preferredHeight = this.GetPreferredHeight(margin);
			return new Vector2(preferredWidth, preferredHeight);
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x00150334 File Offset: 0x0014E534
		public float GetPreferredWidth()
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			Vector2 marginSize;
			marginSize..ctor(float.PositiveInfinity, float.PositiveInfinity);
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.ParseInputText();
			}
			return this.CalculatePreferredValues(defaultFontSize, marginSize).x;
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x001503A0 File Offset: 0x0014E5A0
		public float GetPreferredWidth(Vector2 margin)
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			return this.CalculatePreferredValues(defaultFontSize, margin).x;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x001503DC File Offset: 0x0014E5DC
		public float GetPreferredHeight()
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			Vector2 marginSize;
			marginSize..ctor((this.m_marginWidth == 0f) ? float.PositiveInfinity : this.m_marginWidth, float.PositiveInfinity);
			if (this.m_isInputParsingRequired || this.m_isTextTruncated)
			{
				this.ParseInputText();
			}
			return this.CalculatePreferredValues(defaultFontSize, marginSize).y;
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x00150460 File Offset: 0x0014E660
		public float GetPreferredHeight(Vector2 margin)
		{
			float defaultFontSize = (!this.m_enableAutoSizing) ? this.m_fontSize : this.m_fontSizeMax;
			return this.CalculatePreferredValues(defaultFontSize, margin).y;
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x0015049C File Offset: 0x0014E69C
		public virtual Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize)
		{
			if (this.m_fontAsset == null || this.m_fontAsset.characterDictionary == null)
			{
				return Vector2.zero;
			}
			if (this.m_char_buffer == null || this.m_char_buffer.Length == 0 || this.m_char_buffer[0] == 0)
			{
				return Vector2.zero;
			}
			this.m_currentFontAsset = this.m_fontAsset;
			this.m_currentMaterial = this.m_sharedMaterial;
			this.m_currentMaterialIndex = 0;
			this.m_materialReferenceStack.SetDefault(new MaterialReference(0, this.m_currentFontAsset, null, this.m_currentMaterial, this.m_padding));
			int totalCharacterCount = this.m_totalCharacterCount;
			if (this.m_internalCharacterInfo == null || totalCharacterCount > this.m_internalCharacterInfo.Length)
			{
				this.m_internalCharacterInfo = new TMP_CharacterInfo[(totalCharacterCount <= 1024) ? Mathf.NextPowerOfTwo(totalCharacterCount) : (totalCharacterCount + 256)];
			}
			this.m_fontScale = defaultFontSize / this.m_currentFontAsset.fontInfo.PointSize * ((!this.m_isOrthographic) ? 0.1f : 1f);
			this.m_fontScaleMultiplier = 1f;
			float num = defaultFontSize / this.m_fontAsset.fontInfo.PointSize * this.m_fontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
			float num2 = this.m_fontScale;
			this.m_currentFontSize = defaultFontSize;
			this.m_sizeStack.SetDefault(this.m_currentFontSize);
			this.m_style = this.m_fontStyle;
			this.m_baselineOffset = 0f;
			this.m_styleStack.Clear();
			this.m_lineOffset = 0f;
			this.m_lineHeight = 0f;
			float num3 = this.m_currentFontAsset.fontInfo.LineHeight - (this.m_currentFontAsset.fontInfo.Ascender - this.m_currentFontAsset.fontInfo.Descender);
			this.m_cSpacing = 0f;
			this.m_monoSpacing = 0f;
			this.m_xAdvance = 0f;
			float num4 = 0f;
			this.tag_LineIndent = 0f;
			this.tag_Indent = 0f;
			this.m_indentStack.SetDefault(0f);
			this.tag_NoParsing = false;
			this.m_characterCount = 0;
			this.m_firstCharacterOfLine = 0;
			this.m_maxLineAscender = float.NegativeInfinity;
			this.m_maxLineDescender = float.PositiveInfinity;
			this.m_lineNumber = 0;
			float x = marginSize.x;
			this.m_marginLeft = 0f;
			this.m_marginRight = 0f;
			this.m_width = -1f;
			float num5 = 0f;
			float num6 = 0f;
			this.m_maxAscender = 0f;
			this.m_maxDescender = 0f;
			bool flag = true;
			bool flag2 = false;
			WordWrapState wordWrapState = default(WordWrapState);
			this.SaveWordWrappingState(ref wordWrapState, 0, 0);
			WordWrapState wordWrapState2 = default(WordWrapState);
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			while (this.m_char_buffer[num9] != 0)
			{
				int num10 = this.m_char_buffer[num9];
				this.m_textElementType = TMP_TextElementType.Character;
				this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
				this.m_currentFontAsset = this.m_materialReferences[this.m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = this.m_currentMaterialIndex;
				if (!this.m_isRichText || num10 != 60)
				{
					goto IL_38C;
				}
				this.m_isParsingText = true;
				if (!this.ValidateHtmlTag(this.m_char_buffer, num9 + 1, out num8))
				{
					goto IL_38C;
				}
				num9 = num8;
				if (this.m_textElementType != TMP_TextElementType.Character)
				{
					goto IL_38C;
				}
				IL_FE9:
				num9++;
				continue;
				IL_38C:
				this.m_isParsingText = false;
				float num11 = 1f;
				if (this.m_textElementType == TMP_TextElementType.Character)
				{
					if ((this.m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num10))
						{
							num10 = (int)char.ToUpper((char)num10);
						}
					}
					else if ((this.m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num10))
						{
							num10 = (int)char.ToLower((char)num10);
						}
					}
					else if (((this.m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (this.m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num10))
					{
						num11 = 0.8f;
						num10 = (int)char.ToUpper((char)num10);
					}
				}
				if (this.m_textElementType == TMP_TextElementType.Sprite)
				{
					TMP_Sprite tmp_Sprite = this.m_currentSpriteAsset.spriteInfoList[this.m_spriteIndex];
					if (tmp_Sprite == null)
					{
						goto IL_FE9;
					}
					num10 = 57344 + this.m_spriteIndex;
					this.m_cached_TextElement = tmp_Sprite;
					num2 = this.m_fontAsset.fontInfo.Ascender / tmp_Sprite.height * tmp_Sprite.scale * num;
					this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Sprite;
					this.m_currentMaterialIndex = currentMaterialIndex;
				}
				else if (this.m_textElementType == TMP_TextElementType.Character)
				{
					this.m_cached_TextElement = this.m_textInfo.characterInfo[this.m_characterCount].textElement;
					this.m_currentFontAsset = this.m_textInfo.characterInfo[this.m_characterCount].fontAsset;
					this.m_currentMaterialIndex = this.m_textInfo.characterInfo[this.m_characterCount].materialReferenceIndex;
					this.m_fontScale = this.m_currentFontSize * num11 / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
					num2 = this.m_fontScale * this.m_fontScaleMultiplier;
					this.m_internalCharacterInfo[this.m_characterCount].elementType = TMP_TextElementType.Character;
				}
				this.m_internalCharacterInfo[this.m_characterCount].character = (char)num10;
				if (this.m_enableKerning && this.m_characterCount >= 1)
				{
					int character = (int)this.m_internalCharacterInfo[this.m_characterCount - 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey(character, num10);
					KerningPair kerningPair;
					this.m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out kerningPair);
					if (kerningPair != null)
					{
						this.m_xAdvance += kerningPair.XadvanceOffset * num2;
					}
				}
				float num12 = 0f;
				if (this.m_monoSpacing != 0f)
				{
					num12 = this.m_monoSpacing / 2f - (this.m_cached_TextElement.width / 2f + this.m_cached_TextElement.xOffset) * num2;
					this.m_xAdvance += num12;
				}
				float num13;
				if ((this.m_style & FontStyles.Bold) == FontStyles.Bold || (this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
				{
					num13 = 1f + this.m_currentFontAsset.boldSpacing * 0.01f;
				}
				else
				{
					num13 = 1f;
				}
				this.m_internalCharacterInfo[this.m_characterCount].baseLine = 0f - this.m_lineOffset + this.m_baselineOffset;
				float num14 = this.m_currentFontAsset.fontInfo.Ascender * ((this.m_textElementType != TMP_TextElementType.Character) ? num : num2) + this.m_baselineOffset;
				this.m_internalCharacterInfo[this.m_characterCount].ascender = num14 - this.m_lineOffset;
				this.m_maxLineAscender = ((num14 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num14);
				float num15 = this.m_currentFontAsset.fontInfo.Descender * ((this.m_textElementType != TMP_TextElementType.Character) ? num : num2) + this.m_baselineOffset;
				float num16 = this.m_internalCharacterInfo[this.m_characterCount].descender = num15 - this.m_lineOffset;
				this.m_maxLineDescender = ((num15 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num15);
				if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript || (this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num17 = (num14 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num14 = this.m_maxLineAscender;
					this.m_maxLineAscender = ((num17 <= this.m_maxLineAscender) ? this.m_maxLineAscender : num17);
					float num18 = (num15 - this.m_baselineOffset) / this.m_currentFontAsset.fontInfo.SubSize;
					num15 = this.m_maxLineDescender;
					this.m_maxLineDescender = ((num18 >= this.m_maxLineDescender) ? this.m_maxLineDescender : num18);
				}
				if (this.m_lineNumber == 0)
				{
					this.m_maxAscender = ((this.m_maxAscender <= num14) ? num14 : this.m_maxAscender);
				}
				if (num10 == 9 || !char.IsWhiteSpace((char)num10) || this.m_textElementType == TMP_TextElementType.Sprite)
				{
					float num19 = (this.m_width == -1f) ? (x + 0.0001f - this.m_marginLeft - this.m_marginRight) : Mathf.Min(x + 0.0001f - this.m_marginLeft - this.m_marginRight, this.m_width);
					if (this.m_xAdvance + this.m_cached_TextElement.xAdvance * num2 > num19 && this.enableWordWrapping && this.m_characterCount != this.m_firstCharacterOfLine)
					{
						if (num7 == wordWrapState2.previous_WordBreak || flag)
						{
							if (!this.m_isCharacterWrappingEnabled)
							{
								this.m_isCharacterWrappingEnabled = true;
							}
							else
							{
								flag2 = true;
							}
						}
						num9 = this.RestoreWordWrappingState(ref wordWrapState2);
						num7 = num9;
						if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f)
						{
							float num20 = this.m_maxLineAscender - this.m_startOfLineAscender;
							this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num20);
							this.m_lineOffset += num20;
							wordWrapState2.lineOffset = this.m_lineOffset;
							wordWrapState2.previousLineAscender = this.m_maxLineAscender;
						}
						float num21 = this.m_maxLineAscender - this.m_lineOffset;
						float num22 = this.m_maxLineDescender - this.m_lineOffset;
						this.m_maxDescender = ((this.m_maxDescender >= num22) ? num22 : this.m_maxDescender);
						this.m_firstCharacterOfLine = this.m_characterCount;
						num5 += this.m_xAdvance;
						if (this.m_enableWordWrapping)
						{
							num6 = this.m_maxAscender - this.m_maxDescender;
						}
						else
						{
							num6 = Mathf.Max(num6, num21 - num22);
						}
						this.SaveWordWrappingState(ref wordWrapState, num9, this.m_characterCount - 1);
						this.m_lineNumber++;
						if (this.m_lineHeight == 0f)
						{
							float num23 = this.m_internalCharacterInfo[this.m_characterCount].ascender - this.m_internalCharacterInfo[this.m_characterCount].baseLine;
							float num24 = 0f - this.m_maxLineDescender + num23 + (num3 + this.m_lineSpacing + this.m_lineSpacingDelta) * num;
							this.m_lineOffset += num24;
							this.m_startOfLineAscender = num23;
						}
						else
						{
							this.m_lineOffset += this.m_lineHeight + this.m_lineSpacing * num;
						}
						this.m_maxLineAscender = float.NegativeInfinity;
						this.m_maxLineDescender = float.PositiveInfinity;
						this.m_xAdvance = this.tag_Indent;
						goto IL_FE9;
					}
				}
				if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f && !this.m_isNewPage)
				{
					float num25 = this.m_maxLineAscender - this.m_startOfLineAscender;
					this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num25);
					num16 -= num25;
					this.m_lineOffset += num25;
					this.m_startOfLineAscender += num25;
					wordWrapState2.lineOffset = this.m_lineOffset;
					wordWrapState2.previousLineAscender = this.m_startOfLineAscender;
				}
				if (num10 == 9)
				{
					this.m_xAdvance += this.m_currentFontAsset.fontInfo.TabWidth * num2;
				}
				else if (this.m_monoSpacing != 0f)
				{
					this.m_xAdvance += this.m_monoSpacing - num12 + (this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing;
				}
				else
				{
					this.m_xAdvance += (this.m_cached_TextElement.xAdvance * num13 + this.m_characterSpacing + this.m_currentFontAsset.normalSpacingOffset) * num2 + this.m_cSpacing;
				}
				if (num10 == 13)
				{
					num4 = Mathf.Max(num4, num5 + this.m_xAdvance);
					num5 = 0f;
					this.m_xAdvance = this.tag_Indent;
				}
				if (num10 == 10 || this.m_characterCount == totalCharacterCount - 1)
				{
					if (this.m_lineNumber > 0 && !TMP_Math.Approximately(this.m_maxLineAscender, this.m_startOfLineAscender) && this.m_lineHeight == 0f)
					{
						float num26 = this.m_maxLineAscender - this.m_startOfLineAscender;
						this.AdjustLineOffset(this.m_firstCharacterOfLine, this.m_characterCount, num26);
						num16 -= num26;
						this.m_lineOffset += num26;
					}
					float num27 = this.m_maxLineDescender - this.m_lineOffset;
					this.m_maxDescender = ((this.m_maxDescender >= num27) ? num27 : this.m_maxDescender);
					this.m_firstCharacterOfLine = this.m_characterCount + 1;
					if (num10 == 10 && this.m_characterCount != totalCharacterCount - 1)
					{
						num4 = Mathf.Max(num4, num5 + this.m_xAdvance);
						num5 = 0f;
					}
					else
					{
						num5 = Mathf.Max(num4, num5 + this.m_xAdvance);
					}
					num6 = this.m_maxAscender - this.m_maxDescender;
					if (num10 == 10)
					{
						this.SaveWordWrappingState(ref wordWrapState, num9, this.m_characterCount);
						this.SaveWordWrappingState(ref wordWrapState2, num9, this.m_characterCount);
						this.m_lineNumber++;
						if (this.m_lineHeight == 0f)
						{
							float num24 = 0f - this.m_maxLineDescender + num14 + (num3 + this.m_lineSpacing + this.m_paragraphSpacing + this.m_lineSpacingDelta) * num;
							this.m_lineOffset += num24;
						}
						else
						{
							this.m_lineOffset += this.m_lineHeight + (this.m_lineSpacing + this.m_paragraphSpacing) * num;
						}
						this.m_maxLineAscender = float.NegativeInfinity;
						this.m_maxLineDescender = float.PositiveInfinity;
						this.m_startOfLineAscender = num14;
						this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
					}
				}
				if (this.m_enableWordWrapping || this.m_overflowMode == TextOverflowModes.Truncate || this.m_overflowMode == TextOverflowModes.Ellipsis)
				{
					if ((num10 == 9 || num10 == 32) && !this.m_isNonBreakingSpace)
					{
						this.SaveWordWrappingState(ref wordWrapState2, num9, this.m_characterCount);
						this.m_isCharacterWrappingEnabled = false;
						flag = false;
					}
					else if (num10 > 11904 && num10 < 40959)
					{
						if (!this.m_currentFontAsset.lineBreakingInfo.leadingCharacters.ContainsKey(num10) && this.m_characterCount < totalCharacterCount - 1 && !this.m_currentFontAsset.lineBreakingInfo.followingCharacters.ContainsKey((int)this.m_internalCharacterInfo[this.m_characterCount + 1].character))
						{
							this.SaveWordWrappingState(ref wordWrapState2, num9, this.m_characterCount);
							this.m_isCharacterWrappingEnabled = false;
							flag = false;
						}
					}
					else if (flag || this.m_isCharacterWrappingEnabled || flag2)
					{
						this.SaveWordWrappingState(ref wordWrapState2, num9, this.m_characterCount);
					}
				}
				this.m_characterCount++;
				goto IL_FE9;
			}
			this.m_isCharacterWrappingEnabled = false;
			num5 += ((this.m_margin.x <= 0f) ? 0f : this.m_margin.x);
			num5 += ((this.m_margin.z <= 0f) ? 0f : this.m_margin.z);
			num6 += ((this.m_margin.y <= 0f) ? 0f : this.m_margin.y);
			num6 += ((this.m_margin.w <= 0f) ? 0f : this.m_margin.w);
			return new Vector2(num5, num6);
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x000387D1 File Offset: 0x000369D1
		public virtual void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x00151574 File Offset: 0x0014F774
		public void ResizeLineExtents(int size)
		{
			size = ((size <= 1024) ? Mathf.NextPowerOfTwo(size + 1) : (size + 256));
			TMP_LineInfo[] array = new TMP_LineInfo[size];
			for (int i = 0; i < size; i++)
			{
				if (i < this.m_textInfo.lineInfo.Length)
				{
					array[i] = this.m_textInfo.lineInfo[i];
				}
				else
				{
					array[i].lineExtents.min = TMP_Text.k_InfinityVectorPositive;
					array[i].lineExtents.max = TMP_Text.k_InfinityVectorNegative;
					array[i].ascender = TMP_Text.k_InfinityVectorNegative.x;
					array[i].descender = TMP_Text.k_InfinityVectorPositive.x;
				}
			}
			this.m_textInfo.lineInfo = array;
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x000387D3 File Offset: 0x000369D3
		public virtual TMP_TextInfo GetTextInfo(string text)
		{
			return null;
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x000387D6 File Offset: 0x000369D6
		public virtual void ComputeMarginSize()
		{
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x0015165C File Offset: 0x0014F85C
		public int GetArraySizes(int[] chars)
		{
			int num = 0;
			this.m_totalCharacterCount = 0;
			this.m_isUsingBold = false;
			this.m_isParsingText = false;
			int num2 = 0;
			while (chars[num2] != 0)
			{
				int num3 = chars[num2];
				if (this.m_isRichText && num3 == 60 && this.ValidateHtmlTag(chars, num2 + 1, out num))
				{
					num2 = num;
					if ((this.m_style & FontStyles.Bold) == FontStyles.Bold)
					{
						this.m_isUsingBold = true;
					}
				}
				else
				{
					if (!char.IsWhiteSpace((char)num3))
					{
					}
					this.m_totalCharacterCount++;
				}
				num2++;
			}
			return this.m_totalCharacterCount;
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x001516F8 File Offset: 0x0014F8F8
		public void SaveWordWrappingState(ref WordWrapState state, int index, int count)
		{
			state.currentFontAsset = this.m_currentFontAsset;
			state.currentSpriteAsset = this.m_currentSpriteAsset;
			state.currentMaterial = this.m_currentMaterial;
			state.currentMaterialIndex = this.m_currentMaterialIndex;
			state.previous_WordBreak = index;
			state.total_CharacterCount = count;
			state.visible_CharacterCount = this.m_visibleCharacterCount;
			state.visible_SpriteCount = this.m_visibleSpriteCount;
			state.visible_LinkCount = this.m_textInfo.linkCount;
			state.firstCharacterIndex = this.m_firstCharacterOfLine;
			state.firstVisibleCharacterIndex = this.m_firstVisibleCharacterOfLine;
			state.lastVisibleCharIndex = this.m_lastVisibleCharacterOfLine;
			state.fontStyle = this.m_style;
			state.fontScale = this.m_fontScale;
			state.fontScaleMultiplier = this.m_fontScaleMultiplier;
			state.currentFontSize = this.m_currentFontSize;
			state.xAdvance = this.m_xAdvance;
			state.maxAscender = this.m_maxAscender;
			state.maxDescender = this.m_maxDescender;
			state.maxLineAscender = this.m_maxLineAscender;
			state.maxLineDescender = this.m_maxLineDescender;
			state.previousLineAscender = this.m_startOfLineAscender;
			state.preferredWidth = this.m_preferredWidth;
			state.preferredHeight = this.m_preferredHeight;
			state.meshExtents = this.m_meshExtents;
			state.lineNumber = this.m_lineNumber;
			state.lineOffset = this.m_lineOffset;
			state.baselineOffset = this.m_baselineOffset;
			state.vertexColor = this.m_htmlColor;
			state.tagNoParsing = this.tag_NoParsing;
			state.colorStack = this.m_colorStack;
			state.sizeStack = this.m_sizeStack;
			state.fontWeightStack = this.m_fontWeightStack;
			state.styleStack = this.m_styleStack;
			state.actionStack = this.m_actionStack;
			state.materialReferenceStack = this.m_materialReferenceStack;
			if (this.m_lineNumber < this.m_textInfo.lineInfo.Length)
			{
				state.lineInfo = this.m_textInfo.lineInfo[this.m_lineNumber];
			}
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x001518EC File Offset: 0x0014FAEC
		public int RestoreWordWrappingState(ref WordWrapState state)
		{
			int previous_WordBreak = state.previous_WordBreak;
			this.m_currentFontAsset = state.currentFontAsset;
			this.m_currentSpriteAsset = state.currentSpriteAsset;
			this.m_currentMaterial = state.currentMaterial;
			this.m_currentMaterialIndex = state.currentMaterialIndex;
			this.m_characterCount = state.total_CharacterCount + 1;
			this.m_visibleCharacterCount = state.visible_CharacterCount;
			this.m_visibleSpriteCount = state.visible_SpriteCount;
			this.m_textInfo.linkCount = state.visible_LinkCount;
			this.m_firstCharacterOfLine = state.firstCharacterIndex;
			this.m_firstVisibleCharacterOfLine = state.firstVisibleCharacterIndex;
			this.m_lastVisibleCharacterOfLine = state.lastVisibleCharIndex;
			this.m_style = state.fontStyle;
			this.m_fontScale = state.fontScale;
			this.m_fontScaleMultiplier = state.fontScaleMultiplier;
			this.m_currentFontSize = state.currentFontSize;
			this.m_xAdvance = state.xAdvance;
			this.m_maxAscender = state.maxAscender;
			this.m_maxDescender = state.maxDescender;
			this.m_maxLineAscender = state.maxLineAscender;
			this.m_maxLineDescender = state.maxLineDescender;
			this.m_startOfLineAscender = state.previousLineAscender;
			this.m_preferredWidth = state.preferredWidth;
			this.m_preferredHeight = state.preferredHeight;
			this.m_meshExtents = state.meshExtents;
			this.m_lineNumber = state.lineNumber;
			this.m_lineOffset = state.lineOffset;
			this.m_baselineOffset = state.baselineOffset;
			this.m_htmlColor = state.vertexColor;
			this.tag_NoParsing = state.tagNoParsing;
			this.m_colorStack = state.colorStack;
			this.m_sizeStack = state.sizeStack;
			this.m_fontWeightStack = state.fontWeightStack;
			this.m_styleStack = state.styleStack;
			this.m_actionStack = state.actionStack;
			this.m_materialReferenceStack = state.materialReferenceStack;
			if (this.m_lineNumber < this.m_textInfo.lineInfo.Length)
			{
				this.m_textInfo.lineInfo[this.m_lineNumber] = state.lineInfo;
			}
			return previous_WordBreak;
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x00151AE8 File Offset: 0x0014FCE8
		public virtual void SaveGlyphVertexInfo(float padding, float style_padding, Color32 vertexColor)
		{
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.position = this.m_textInfo.characterInfo[this.m_characterCount].topLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.position = this.m_textInfo.characterInfo[this.m_characterCount].topRight;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomRight;
			vertexColor.a = ((this.m_fontColor32.a >= vertexColor.a) ? vertexColor.a : this.m_fontColor32.a);
			if (!this.m_enableVertexGradient)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = vertexColor;
			}
			else if (!this.m_overrideHtmlColors && !this.m_htmlColor.CompareRGB(this.m_fontColor32))
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = vertexColor;
			}
			else
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = this.m_fontColorGradient.bottomLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = this.m_fontColorGradient.topLeft * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = this.m_fontColorGradient.topRight * vertexColor;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = this.m_fontColorGradient.bottomRight * vertexColor;
			}
			if (!this.m_isSDFShader)
			{
				style_padding = 0f;
			}
			FaceInfo fontInfo = this.m_currentFontAsset.fontInfo;
			Vector2 uv;
			uv.x = (this.m_cached_TextElement.x - padding - style_padding) / fontInfo.AtlasWidth;
			uv.y = 1f - (this.m_cached_TextElement.y + padding + style_padding + this.m_cached_TextElement.height) / fontInfo.AtlasHeight;
			Vector2 uv2;
			uv2.x = uv.x;
			uv2.y = 1f - (this.m_cached_TextElement.y - padding - style_padding) / fontInfo.AtlasHeight;
			Vector2 uv3;
			uv3.x = (this.m_cached_TextElement.x + padding + style_padding + this.m_cached_TextElement.width) / fontInfo.AtlasWidth;
			uv3.y = uv2.y;
			Vector2 uv4;
			uv4.x = uv3.x;
			uv4.y = uv.y;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = uv;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = uv2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = uv3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv4;
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00151FB4 File Offset: 0x001501B4
		public virtual void SaveSpriteVertexInfo(Color32 vertexColor)
		{
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.position = this.m_textInfo.characterInfo[this.m_characterCount].topLeft;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.position = this.m_textInfo.characterInfo[this.m_characterCount].topRight;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.position = this.m_textInfo.characterInfo[this.m_characterCount].bottomRight;
			if (this.m_tintAllSprites)
			{
				this.m_tintSprite = true;
			}
			Color32 color = (!this.m_tintSprite) ? this.m_spriteColor : this.m_spriteColor.Multiply(vertexColor);
			color.a = ((color.a >= this.m_fontColor32.a) ? this.m_fontColor32.a : (color.a = ((color.a >= vertexColor.a) ? vertexColor.a : color.a)));
			if (!this.m_enableVertexGradient)
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = color;
			}
			else if (!this.m_overrideHtmlColors && !this.m_htmlColor.CompareRGB(this.m_fontColor32))
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = color;
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = color;
			}
			else
			{
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.bottomLeft));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.topLeft));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.topRight));
				this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.color = ((!this.m_tintSprite) ? color : color.Multiply(this.m_fontColorGradient.bottomRight));
			}
			Vector2 uv;
			uv..ctor(this.m_cached_TextElement.x / (float)this.m_currentSpriteAsset.spriteSheet.width, this.m_cached_TextElement.y / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv2;
			uv2..ctor(uv.x, (this.m_cached_TextElement.y + this.m_cached_TextElement.height) / (float)this.m_currentSpriteAsset.spriteSheet.height);
			Vector2 uv3;
			uv3..ctor((this.m_cached_TextElement.x + this.m_cached_TextElement.width) / (float)this.m_currentSpriteAsset.spriteSheet.width, uv2.y);
			Vector2 uv4;
			uv4..ctor(uv3.x, uv.y);
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BL.uv = uv;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TL.uv = uv2;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_TR.uv = uv3;
			this.m_textInfo.characterInfo[this.m_characterCount].vertex_BR.uv = uv4;
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x001524E4 File Offset: 0x001506E4
		public virtual void FillCharacterVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = (short)index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x00152898 File Offset: 0x00150A98
		public virtual void FillSpriteVertexBuffers(int i, int index_X4)
		{
			int materialReferenceIndex = this.m_textInfo.characterInfo[i].materialReferenceIndex;
			index_X4 = this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount;
			TMP_CharacterInfo[] characterInfo = this.m_textInfo.characterInfo;
			this.m_textInfo.characterInfo[i].vertexIndex = (short)index_X4;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[index_X4] = characterInfo[i].vertex_BL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[1 + index_X4] = characterInfo[i].vertex_TL.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[2 + index_X4] = characterInfo[i].vertex_TR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertices[3 + index_X4] = characterInfo[i].vertex_BR.position;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[index_X4] = characterInfo[i].vertex_BL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[1 + index_X4] = characterInfo[i].vertex_TL.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[2 + index_X4] = characterInfo[i].vertex_TR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs0[3 + index_X4] = characterInfo[i].vertex_BR.uv;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[index_X4] = characterInfo[i].vertex_BL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[1 + index_X4] = characterInfo[i].vertex_TL.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[2 + index_X4] = characterInfo[i].vertex_TR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].uvs2[3 + index_X4] = characterInfo[i].vertex_BR.uv2;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[index_X4] = characterInfo[i].vertex_BL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[1 + index_X4] = characterInfo[i].vertex_TL.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[2 + index_X4] = characterInfo[i].vertex_TR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].colors32[3 + index_X4] = characterInfo[i].vertex_BR.color;
			this.m_textInfo.meshInfo[materialReferenceIndex].vertexCount = index_X4 + 4;
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x00152C4C File Offset: 0x00150E4C
		public virtual void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, Color32 underlineColor)
		{
			if (this.m_cached_Underline_GlyphInfo == null)
			{
				if (!TMP_Settings.warningsDisabled)
				{
				}
				return;
			}
			int num = index + 12;
			if (num > this.m_textInfo.meshInfo[0].vertices.Length)
			{
				this.m_textInfo.meshInfo[0].ResizeMeshInfo(num / 4);
			}
			start.y = Mathf.Min(start.y, end.y);
			end.y = Mathf.Min(start.y, end.y);
			float num2 = this.m_cached_Underline_GlyphInfo.width / 2f * maxScale;
			if (end.x - start.x < this.m_cached_Underline_GlyphInfo.width * maxScale)
			{
				num2 = (end.x - start.x) / 2f;
			}
			float num3 = this.m_padding * startScale / maxScale;
			float num4 = this.m_padding * endScale / maxScale;
			float height = this.m_cached_Underline_GlyphInfo.height;
			Vector3[] vertices = this.m_textInfo.meshInfo[0].vertices;
			vertices[index] = start + new Vector3(0f, 0f - (height + this.m_padding) * maxScale, 0f);
			vertices[index + 1] = start + new Vector3(0f, this.m_padding * maxScale, 0f);
			vertices[index + 2] = vertices[index + 1] + new Vector3(num2, 0f, 0f);
			vertices[index + 3] = vertices[index] + new Vector3(num2, 0f, 0f);
			vertices[index + 4] = vertices[index + 3];
			vertices[index + 5] = vertices[index + 2];
			vertices[index + 6] = end + new Vector3(-num2, this.m_padding * maxScale, 0f);
			vertices[index + 7] = end + new Vector3(-num2, -(height + this.m_padding) * maxScale, 0f);
			vertices[index + 8] = vertices[index + 7];
			vertices[index + 9] = vertices[index + 6];
			vertices[index + 10] = end + new Vector3(0f, this.m_padding * maxScale, 0f);
			vertices[index + 11] = end + new Vector3(0f, -(height + this.m_padding) * maxScale, 0f);
			Vector2[] uvs = this.m_textInfo.meshInfo[0].uvs0;
			Vector2 vector;
			vector..ctor((this.m_cached_Underline_GlyphInfo.x - num3) / this.m_fontAsset.fontInfo.AtlasWidth, 1f - (this.m_cached_Underline_GlyphInfo.y + this.m_padding + this.m_cached_Underline_GlyphInfo.height) / this.m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector2;
			vector2..ctor(vector.x, 1f - (this.m_cached_Underline_GlyphInfo.y - this.m_padding) / this.m_fontAsset.fontInfo.AtlasHeight);
			Vector2 vector3;
			vector3..ctor((this.m_cached_Underline_GlyphInfo.x - num3 + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector4;
			vector4..ctor(vector3.x, vector.y);
			Vector2 vector5;
			vector5..ctor((this.m_cached_Underline_GlyphInfo.x + num4 + this.m_cached_Underline_GlyphInfo.width / 2f) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector6;
			vector6..ctor(vector5.x, vector.y);
			Vector2 vector7;
			vector7..ctor((this.m_cached_Underline_GlyphInfo.x + num4 + this.m_cached_Underline_GlyphInfo.width) / this.m_fontAsset.fontInfo.AtlasWidth, vector2.y);
			Vector2 vector8;
			vector8..ctor(vector7.x, vector.y);
			uvs[index] = vector;
			uvs[1 + index] = vector2;
			uvs[2 + index] = vector3;
			uvs[3 + index] = vector4;
			uvs[4 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector.y);
			uvs[5 + index] = new Vector2(vector3.x - vector3.x * 0.001f, vector2.y);
			uvs[6 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector2.y);
			uvs[7 + index] = new Vector2(vector3.x + vector3.x * 0.001f, vector.y);
			uvs[8 + index] = vector6;
			uvs[9 + index] = vector5;
			uvs[10 + index] = vector7;
			uvs[11 + index] = vector8;
			float x = (vertices[index + 2].x - start.x) / (end.x - start.x);
			float num5 = (maxScale * this.m_rectTransform.lossyScale.y != 0f) ? this.m_rectTransform.lossyScale.y : 1f;
			float scale = num5;
			Vector2[] uvs2 = this.m_textInfo.meshInfo[0].uvs2;
			uvs2[index] = this.PackUV(0f, 0f, num5);
			uvs2[1 + index] = this.PackUV(0f, 1f, num5);
			uvs2[2 + index] = this.PackUV(x, 1f, num5);
			uvs2[3 + index] = this.PackUV(x, 0f, num5);
			float x2 = (vertices[index + 4].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[4 + index] = this.PackUV(x2, 0f, scale);
			uvs2[5 + index] = this.PackUV(x2, 1f, scale);
			uvs2[6 + index] = this.PackUV(x, 1f, scale);
			uvs2[7 + index] = this.PackUV(x, 0f, scale);
			x2 = (vertices[index + 8].x - start.x) / (end.x - start.x);
			x = (vertices[index + 6].x - start.x) / (end.x - start.x);
			uvs2[8 + index] = this.PackUV(x2, 0f, num5);
			uvs2[9 + index] = this.PackUV(x2, 1f, num5);
			uvs2[10 + index] = this.PackUV(1f, 1f, num5);
			uvs2[11 + index] = this.PackUV(1f, 0f, num5);
			Color32[] colors = this.m_textInfo.meshInfo[0].colors32;
			colors[index] = underlineColor;
			colors[1 + index] = underlineColor;
			colors[2 + index] = underlineColor;
			colors[3 + index] = underlineColor;
			colors[4 + index] = underlineColor;
			colors[5 + index] = underlineColor;
			colors[6 + index] = underlineColor;
			colors[7 + index] = underlineColor;
			colors[8 + index] = underlineColor;
			colors[9 + index] = underlineColor;
			colors[10 + index] = underlineColor;
			colors[11 + index] = underlineColor;
			index += 12;
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x00153614 File Offset: 0x00151814
		public void GetSpecialCharacters(TMP_FontAsset fontAsset)
		{
			if (fontAsset.characterDictionary.TryGetValue(95, out this.m_cached_Underline_GlyphInfo) || !TMP_Settings.warningsDisabled)
			{
			}
			if (fontAsset.characterDictionary.TryGetValue(8230, out this.m_cached_Ellipsis_GlyphInfo) || !TMP_Settings.warningsDisabled)
			{
			}
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x00153668 File Offset: 0x00151868
		public int GetMaterialReferenceForFontWeight()
		{
			this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentFontAsset.fontWeights[0].italicTypeface.material, this.m_currentFontAsset.fontWeights[0].italicTypeface, this.m_materialReferences, this.m_materialReferenceIndexLookup);
			return 0;
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x001536B8 File Offset: 0x001518B8
		public TMP_FontAsset GetAlternativeFontAsset()
		{
			bool flag = (this.m_style & FontStyles.Italic) == FontStyles.Italic || (this.m_fontStyle & FontStyles.Italic) == FontStyles.Italic;
			int num = this.m_fontWeightInternal / 100;
			TMP_FontAsset tmp_FontAsset;
			if (flag)
			{
				tmp_FontAsset = this.m_currentFontAsset.fontWeights[num].italicTypeface;
			}
			else
			{
				tmp_FontAsset = this.m_currentFontAsset.fontWeights[num].regularTypeface;
			}
			if (tmp_FontAsset == null)
			{
				return this.m_currentFontAsset;
			}
			this.m_currentFontAsset = tmp_FontAsset;
			return this.m_currentFontAsset;
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x00153740 File Offset: 0x00151940
		public Vector2 PackUV(float x, float y, float scale)
		{
			Vector2 result;
			result.x = Mathf.Floor(x * 511f);
			result.y = Mathf.Floor(y * 511f);
			result.x = result.x * 4096f + result.y;
			result.y = scale;
			return result;
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x00153798 File Offset: 0x00151998
		public float PackUV(float x, float y)
		{
			double num = Math.Floor((double)(x * 511f));
			double num2 = Math.Floor((double)(y * 511f));
			return (float)(num * 4096.0 + num2);
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x001537D0 File Offset: 0x001519D0
		public int HexToInt(char hex)
		{
			switch (hex)
			{
			case '0':
				return 0;
			case '1':
				return 1;
			case '2':
				return 2;
			case '3':
				return 3;
			case '4':
				return 4;
			case '5':
				return 5;
			case '6':
				return 6;
			case '7':
				return 7;
			case '8':
				return 8;
			case '9':
				return 9;
			default:
				switch (hex)
				{
				case 'a':
					return 10;
				case 'b':
					return 11;
				case 'c':
					return 12;
				case 'd':
					return 13;
				case 'e':
					return 14;
				case 'f':
					return 15;
				default:
					return 15;
				}
				break;
			case 'A':
				return 10;
			case 'B':
				return 11;
			case 'C':
				return 12;
			case 'D':
				return 13;
			case 'E':
				return 14;
			case 'F':
				return 15;
			}
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x001538A4 File Offset: 0x00151AA4
		public int GetUTF16(int i)
		{
			int num = this.HexToInt(this.m_text[i]) * 4096;
			num += this.HexToInt(this.m_text[i + 1]) * 256;
			num += this.HexToInt(this.m_text[i + 2]) * 16;
			return num + this.HexToInt(this.m_text[i + 3]);
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x0015391C File Offset: 0x00151B1C
		public int GetUTF32(int i)
		{
			int num = 0;
			num += this.HexToInt(this.m_text[i]) * 268435456;
			num += this.HexToInt(this.m_text[i + 1]) * 16777216;
			num += this.HexToInt(this.m_text[i + 2]) * 1048576;
			num += this.HexToInt(this.m_text[i + 3]) * 65536;
			num += this.HexToInt(this.m_text[i + 4]) * 4096;
			num += this.HexToInt(this.m_text[i + 5]) * 256;
			num += this.HexToInt(this.m_text[i + 6]) * 16;
			return num + this.HexToInt(this.m_text[i + 7]);
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x00153A0C File Offset: 0x00151C0C
		public Color32 HexCharsToColor(char[] hexChars, int tagCount)
		{
			if (tagCount == 7)
			{
				byte b = (byte)(this.HexToInt(hexChars[1]) * 16 + this.HexToInt(hexChars[2]));
				byte b2 = (byte)(this.HexToInt(hexChars[3]) * 16 + this.HexToInt(hexChars[4]));
				byte b3 = (byte)(this.HexToInt(hexChars[5]) * 16 + this.HexToInt(hexChars[6]));
				return new Color32(b, b2, b3, byte.MaxValue);
			}
			if (tagCount == 9)
			{
				byte b4 = (byte)(this.HexToInt(hexChars[1]) * 16 + this.HexToInt(hexChars[2]));
				byte b5 = (byte)(this.HexToInt(hexChars[3]) * 16 + this.HexToInt(hexChars[4]));
				byte b6 = (byte)(this.HexToInt(hexChars[5]) * 16 + this.HexToInt(hexChars[6]));
				byte b7 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				return new Color32(b4, b5, b6, b7);
			}
			if (tagCount == 13)
			{
				byte b8 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				byte b9 = (byte)(this.HexToInt(hexChars[9]) * 16 + this.HexToInt(hexChars[10]));
				byte b10 = (byte)(this.HexToInt(hexChars[11]) * 16 + this.HexToInt(hexChars[12]));
				return new Color32(b8, b9, b10, byte.MaxValue);
			}
			if (tagCount == 15)
			{
				byte b11 = (byte)(this.HexToInt(hexChars[7]) * 16 + this.HexToInt(hexChars[8]));
				byte b12 = (byte)(this.HexToInt(hexChars[9]) * 16 + this.HexToInt(hexChars[10]));
				byte b13 = (byte)(this.HexToInt(hexChars[11]) * 16 + this.HexToInt(hexChars[12]));
				byte b14 = (byte)(this.HexToInt(hexChars[13]) * 16 + this.HexToInt(hexChars[14]));
				return new Color32(b11, b12, b13, b14);
			}
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x00153BF0 File Offset: 0x00151DF0
		public Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
		{
			if (length == 7)
			{
				byte b = (byte)(this.HexToInt(hexChars[startIndex + 1]) * 16 + this.HexToInt(hexChars[startIndex + 2]));
				byte b2 = (byte)(this.HexToInt(hexChars[startIndex + 3]) * 16 + this.HexToInt(hexChars[startIndex + 4]));
				byte b3 = (byte)(this.HexToInt(hexChars[startIndex + 5]) * 16 + this.HexToInt(hexChars[startIndex + 6]));
				return new Color32(b, b2, b3, byte.MaxValue);
			}
			if (length == 9)
			{
				byte b4 = (byte)(this.HexToInt(hexChars[startIndex + 1]) * 16 + this.HexToInt(hexChars[startIndex + 2]));
				byte b5 = (byte)(this.HexToInt(hexChars[startIndex + 3]) * 16 + this.HexToInt(hexChars[startIndex + 4]));
				byte b6 = (byte)(this.HexToInt(hexChars[startIndex + 5]) * 16 + this.HexToInt(hexChars[startIndex + 6]));
				byte b7 = (byte)(this.HexToInt(hexChars[startIndex + 7]) * 16 + this.HexToInt(hexChars[startIndex + 8]));
				return new Color32(b4, b5, b6, b7);
			}
			return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x00153D08 File Offset: 0x00151F08
		public float ConvertToFloat(char[] chars, int startIndex, int length, int decimalPointIndex)
		{
			if (startIndex == 0)
			{
				return -9999f;
			}
			int num = startIndex + length - 1;
			float num2 = 0f;
			float num3 = 1f;
			decimalPointIndex = ((decimalPointIndex <= 0) ? (num + 1) : decimalPointIndex);
			if (chars[startIndex] == '-')
			{
				startIndex++;
				num3 = -1f;
			}
			if (chars[startIndex] == '+' || chars[startIndex] == '%')
			{
				startIndex++;
			}
			for (int i = startIndex; i < num + 1; i++)
			{
				if (!char.IsDigit(chars[i]) && chars[i] != '.')
				{
					return -9999f;
				}
				int num4 = decimalPointIndex - i;
				switch (num4 + 3)
				{
				case 0:
					num2 += (float)(chars[i] - '0') * 0.001f;
					break;
				case 1:
					num2 += (float)(chars[i] - '0') * 0.01f;
					break;
				case 2:
					num2 += (float)(chars[i] - '0') * 0.1f;
					break;
				case 4:
					num2 += (float)(chars[i] - '0');
					break;
				case 5:
					num2 += (float)((chars[i] - '0') * '\n');
					break;
				case 6:
					num2 += (float)((chars[i] - '0') * 'd');
					break;
				case 7:
					num2 += (float)((chars[i] - '0') * 'Ϩ');
					break;
				}
			}
			return num2 * num3;
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x00153E64 File Offset: 0x00152064
		public bool ValidateHtmlTag(int[] chars, int startIndex, out int endIndex)
		{
			int num = 0;
			byte b = 0;
			TagUnits tagUnits = TagUnits.Pixels;
			TagType tagType = TagType.None;
			int num2 = 0;
			this.m_xmlAttribute[num2].nameHashCode = 0;
			this.m_xmlAttribute[num2].valueType = TagType.None;
			this.m_xmlAttribute[num2].valueHashCode = 0;
			this.m_xmlAttribute[num2].valueStartIndex = 0;
			this.m_xmlAttribute[num2].valueLength = 0;
			this.m_xmlAttribute[num2].valueDecimalIndex = 0;
			endIndex = startIndex;
			bool flag = false;
			bool flag2 = false;
			int num3 = startIndex;
			while (num3 < chars.Length && chars[num3] != 0 && num < this.m_htmlTag.Length && chars[num3] != 60)
			{
				if (chars[num3] == 62)
				{
					flag2 = true;
					endIndex = num3;
					this.m_htmlTag[num] = '\0';
					break;
				}
				this.m_htmlTag[num] = (char)chars[num3];
				num++;
				if (b == 1)
				{
					if (this.m_xmlAttribute[num2].valueStartIndex == 0)
					{
						if (chars[num3] == 43 || chars[num3] == 45 || char.IsDigit((char)chars[num3]))
						{
							tagType = TagType.NumericalValue;
							this.m_xmlAttribute[num2].valueType = TagType.NumericalValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							XML_TagAttribute[] xmlAttribute = this.m_xmlAttribute;
							int num4 = num2;
							xmlAttribute[num4].valueLength = xmlAttribute[num4].valueLength + 1;
						}
						else if (chars[num3] == 35)
						{
							tagType = TagType.ColorValue;
							this.m_xmlAttribute[num2].valueType = TagType.ColorValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							XML_TagAttribute[] xmlAttribute2 = this.m_xmlAttribute;
							int num5 = num2;
							xmlAttribute2[num5].valueLength = xmlAttribute2[num5].valueLength + 1;
						}
						else if (chars[num3] != 34)
						{
							tagType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueType = TagType.StringValue;
							this.m_xmlAttribute[num2].valueStartIndex = num - 1;
							this.m_xmlAttribute[num2].valueHashCode = ((this.m_xmlAttribute[num2].valueHashCode << 5) + this.m_xmlAttribute[num2].valueHashCode ^ chars[num3]);
							XML_TagAttribute[] xmlAttribute3 = this.m_xmlAttribute;
							int num6 = num2;
							xmlAttribute3[num6].valueLength = xmlAttribute3[num6].valueLength + 1;
						}
					}
					else if (tagType == TagType.NumericalValue)
					{
						if (chars[num3] == 46)
						{
							this.m_xmlAttribute[num2].valueDecimalIndex = num - 1;
						}
						if (chars[num3] == 112 || chars[num3] == 101 || chars[num3] == 37 || chars[num3] == 32)
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
							this.m_xmlAttribute[num2].valueDecimalIndex = 0;
							if (chars[num3] == 101)
							{
								tagUnits = TagUnits.FontUnits;
							}
							else if (chars[num3] == 37)
							{
								tagUnits = TagUnits.Percentage;
							}
						}
						else if (b != 2)
						{
							XML_TagAttribute[] xmlAttribute4 = this.m_xmlAttribute;
							int num7 = num2;
							xmlAttribute4[num7].valueLength = xmlAttribute4[num7].valueLength + 1;
						}
					}
					else if (tagType == TagType.ColorValue)
					{
						if (chars[num3] != 32)
						{
							XML_TagAttribute[] xmlAttribute5 = this.m_xmlAttribute;
							int num8 = num2;
							xmlAttribute5[num8].valueLength = xmlAttribute5[num8].valueLength + 1;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
							this.m_xmlAttribute[num2].valueDecimalIndex = 0;
						}
					}
					else if (tagType == TagType.StringValue)
					{
						if (chars[num3] != 34)
						{
							this.m_xmlAttribute[num2].valueHashCode = ((this.m_xmlAttribute[num2].valueHashCode << 5) + this.m_xmlAttribute[num2].valueHashCode ^ chars[num3]);
							XML_TagAttribute[] xmlAttribute6 = this.m_xmlAttribute;
							int num9 = num2;
							xmlAttribute6[num9].valueLength = xmlAttribute6[num9].valueLength + 1;
						}
						else
						{
							b = 2;
							tagType = TagType.None;
							num2++;
							this.m_xmlAttribute[num2].nameHashCode = 0;
							this.m_xmlAttribute[num2].valueType = TagType.None;
							this.m_xmlAttribute[num2].valueHashCode = 0;
							this.m_xmlAttribute[num2].valueStartIndex = 0;
							this.m_xmlAttribute[num2].valueLength = 0;
							this.m_xmlAttribute[num2].valueDecimalIndex = 0;
						}
					}
				}
				if (chars[num3] == 61)
				{
					b = 1;
				}
				if (b == 0 && chars[num3] == 32)
				{
					if (flag)
					{
						return false;
					}
					flag = true;
					b = 2;
					tagType = TagType.None;
					num2++;
					this.m_xmlAttribute[num2].nameHashCode = 0;
					this.m_xmlAttribute[num2].valueType = TagType.None;
					this.m_xmlAttribute[num2].valueHashCode = 0;
					this.m_xmlAttribute[num2].valueStartIndex = 0;
					this.m_xmlAttribute[num2].valueLength = 0;
					this.m_xmlAttribute[num2].valueDecimalIndex = 0;
				}
				if (b == 0)
				{
					this.m_xmlAttribute[num2].nameHashCode = (this.m_xmlAttribute[num2].nameHashCode << 3) - this.m_xmlAttribute[num2].nameHashCode + chars[num3];
				}
				if (b == 2 && chars[num3] == 32)
				{
					b = 0;
				}
				num3++;
			}
			if (!flag2)
			{
				return false;
			}
			if (this.tag_NoParsing && this.m_xmlAttribute[0].nameHashCode != 53822163)
			{
				return false;
			}
			if (this.m_xmlAttribute[0].nameHashCode == 53822163)
			{
				this.tag_NoParsing = false;
				return true;
			}
			if (this.m_htmlTag[0] == '#' && num == 7)
			{
				this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
				this.m_colorStack.Add(this.m_htmlColor);
				return true;
			}
			if (this.m_htmlTag[0] == '#' && num == 9)
			{
				this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
				this.m_colorStack.Add(this.m_htmlColor);
				return true;
			}
			int nameHashCode = this.m_xmlAttribute[0].nameHashCode;
			switch (nameHashCode)
			{
			case 115:
				this.m_style |= FontStyles.Strikethrough;
				return true;
			default:
				if (nameHashCode == 426)
				{
					return true;
				}
				if (nameHashCode == 427)
				{
					if ((this.m_fontStyle & FontStyles.Bold) != FontStyles.Bold)
					{
						this.m_style &= (FontStyles)(-2);
						this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
					}
					return true;
				}
				switch (nameHashCode)
				{
				case 444:
					if ((this.m_fontStyle & FontStyles.Strikethrough) != FontStyles.Strikethrough)
					{
						this.m_style &= (FontStyles)(-65);
					}
					return true;
				default:
					if (nameHashCode != 13526026)
					{
						if (nameHashCode == 730022849)
						{
							this.m_style |= FontStyles.LowerCase;
							return true;
						}
						if (nameHashCode == 766244328)
						{
							this.m_style |= FontStyles.SmallCaps;
							return true;
						}
						if (nameHashCode != 781906058)
						{
							if (nameHashCode != 1100728678)
							{
								if (nameHashCode != 1109349752)
								{
									if (nameHashCode != 1109386397)
									{
										if (nameHashCode == -1885698441)
										{
											this.m_fontWeightInternal = this.m_fontWeightStack.Remove();
											return true;
										}
										if (nameHashCode == -1668324918)
										{
											this.m_style &= (FontStyles)(-9);
											return true;
										}
										if (nameHashCode != -1632103439)
										{
											if (nameHashCode != -1616441709)
											{
												if (nameHashCode != -884817987)
												{
													if (nameHashCode == -445573839)
													{
														this.m_lineHeight = 0f;
														return true;
													}
													if (nameHashCode == -445537194)
													{
														this.tag_LineIndent = 0f;
														return true;
													}
													if (nameHashCode != -330774850)
													{
														if (nameHashCode == 98)
														{
															this.m_style |= FontStyles.Bold;
															this.m_fontWeightInternal = 700;
															this.m_fontWeightStack.Add(700);
															return true;
														}
														if (nameHashCode == 105)
														{
															this.m_style |= FontStyles.Italic;
															return true;
														}
														if (nameHashCode == 434)
														{
															this.m_style &= (FontStyles)(-3);
															return true;
														}
														if (nameHashCode != 6380)
														{
															if (nameHashCode == 6552)
															{
																this.m_fontScaleMultiplier = ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																this.m_baselineOffset = this.m_currentFontAsset.fontInfo.SubscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																this.m_style |= FontStyles.Subscript;
																return true;
															}
															if (nameHashCode == 6566)
															{
																this.m_fontScaleMultiplier = ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																this.m_baselineOffset = this.m_currentFontAsset.fontInfo.SuperscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																this.m_style |= FontStyles.Superscript;
																return true;
															}
															if (nameHashCode == 22501)
															{
																this.m_isIgnoringAlignment = false;
																return true;
															}
															if (nameHashCode == 22673)
															{
																if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
																{
																	if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
																	{
																		this.m_fontScaleMultiplier = ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																		this.m_baselineOffset = this.m_currentFontAsset.fontInfo.SuperscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																	}
																	else
																	{
																		this.m_baselineOffset = 0f;
																		this.m_fontScaleMultiplier = 1f;
																	}
																	this.m_style &= (FontStyles)(-257);
																}
																return true;
															}
															if (nameHashCode == 22687)
															{
																if ((this.m_style & FontStyles.Superscript) == FontStyles.Superscript)
																{
																	if ((this.m_style & FontStyles.Subscript) == FontStyles.Subscript)
																	{
																		this.m_fontScaleMultiplier = ((this.m_currentFontAsset.fontInfo.SubSize <= 0f) ? 1f : this.m_currentFontAsset.fontInfo.SubSize);
																		this.m_baselineOffset = this.m_currentFontAsset.fontInfo.SubscriptOffset * this.m_fontScale * this.m_fontScaleMultiplier;
																	}
																	else
																	{
																		this.m_baselineOffset = 0f;
																		this.m_fontScaleMultiplier = 1f;
																	}
																	this.m_style &= (FontStyles)(-129);
																}
																return true;
															}
															if (nameHashCode != 41311)
															{
																if (nameHashCode == 43066)
																{
																	if (this.m_isParsingText)
																	{
																		int num10 = this.m_textInfo.linkInfo.Length;
																		if (this.m_textInfo.linkCount + 1 > num10)
																		{
																			TMP_TextInfo.Resize<TMP_LinkInfo>(ref this.m_textInfo.linkInfo, num10 + 1);
																		}
																		int linkCount = this.m_textInfo.linkCount;
																		this.m_textInfo.linkInfo[linkCount].textComponent = this;
																		this.m_textInfo.linkInfo[linkCount].hashCode = this.m_xmlAttribute[0].valueHashCode;
																		this.m_textInfo.linkInfo[linkCount].linkTextfirstCharacterIndex = this.m_characterCount;
																		this.m_textInfo.linkInfo[linkCount].linkIdFirstCharacterIndex = startIndex + this.m_xmlAttribute[0].valueStartIndex;
																		this.m_textInfo.linkInfo[linkCount].linkIdLength = this.m_xmlAttribute[0].valueLength;
																		this.m_textInfo.linkInfo[linkCount].SetLinkID(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength);
																	}
																	return true;
																}
																if (nameHashCode == 43969)
																{
																	this.m_isNonBreakingSpace = true;
																	return true;
																}
																if (nameHashCode == 43991)
																{
																	if (this.m_overflowMode == TextOverflowModes.Page)
																	{
																		this.m_xAdvance = this.tag_LineIndent + this.tag_Indent;
																		this.m_lineOffset = 0f;
																		this.m_pageNumber++;
																		this.m_isNewPage = true;
																	}
																	return true;
																}
																if (nameHashCode != 45545)
																{
																	if (nameHashCode == 154158)
																	{
																		MaterialReference materialReference = this.m_materialReferenceStack.Remove();
																		this.m_currentFontAsset = materialReference.fontAsset;
																		this.m_currentMaterial = materialReference.material;
																		this.m_currentMaterialIndex = materialReference.index;
																		this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																		return true;
																	}
																	if (nameHashCode == 155913)
																	{
																		if (this.m_isParsingText)
																		{
																			this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextLength = this.m_characterCount - this.m_textInfo.linkInfo[this.m_textInfo.linkCount].linkTextfirstCharacterIndex;
																			this.m_textInfo.linkCount++;
																		}
																		return true;
																	}
																	if (nameHashCode == 156816)
																	{
																		this.m_isNonBreakingSpace = false;
																		return true;
																	}
																	if (nameHashCode == 158392)
																	{
																		this.m_currentFontSize = this.m_sizeStack.Remove();
																		this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																		return true;
																	}
																	if (nameHashCode != 275917)
																	{
																		if (nameHashCode != 276254)
																		{
																			if (nameHashCode == 280416)
																			{
																				return false;
																			}
																			if (nameHashCode != 281955)
																			{
																				if (nameHashCode != 320078)
																				{
																					if (nameHashCode != 322689)
																					{
																						if (nameHashCode != 327550)
																						{
																							if (nameHashCode == 1065846)
																							{
																								this.m_lineJustification = this.m_textAlignment;
																								return true;
																							}
																							if (nameHashCode == 1071884)
																							{
																								this.m_htmlColor = this.m_colorStack.Remove();
																								return true;
																							}
																							if (nameHashCode != 1112618)
																							{
																								if (nameHashCode == 1117479)
																								{
																									this.m_width = -1f;
																									return true;
																								}
																								if (nameHashCode == 1750458)
																								{
																									return false;
																								}
																								if (nameHashCode == 1913798)
																								{
																									int valueHashCode = this.m_xmlAttribute[0].valueHashCode;
																									if (this.m_isParsingText)
																									{
																										this.m_actionStack.Add(valueHashCode);
																									}
																									return true;
																								}
																								if (nameHashCode != 1983971)
																								{
																									if (nameHashCode != 2068980)
																									{
																										if (nameHashCode != 2109854)
																										{
																											if (nameHashCode != 2152041)
																											{
																												if (nameHashCode == 2246877)
																												{
																													int valueHashCode2 = this.m_xmlAttribute[0].valueHashCode;
																													TMP_SpriteAsset tmp_SpriteAsset;
																													if (this.m_xmlAttribute[0].valueType == TagType.None || this.m_xmlAttribute[0].valueType == TagType.NumericalValue)
																													{
																														if (this.m_defaultSpriteAsset == null)
																														{
																															if (TMP_Settings.defaultSpriteAsset != null)
																															{
																																this.m_defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
																															}
																															else
																															{
																																this.m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
																															}
																														}
																														this.m_currentSpriteAsset = this.m_defaultSpriteAsset;
																														if (this.m_currentSpriteAsset == null)
																														{
																															return false;
																														}
																													}
																													else if (MaterialReferenceManager.TryGetSpriteAsset(valueHashCode2, out tmp_SpriteAsset))
																													{
																														this.m_currentSpriteAsset = tmp_SpriteAsset;
																													}
																													else
																													{
																														if (tmp_SpriteAsset == null)
																														{
																															tmp_SpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprites/" + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																														}
																														if (tmp_SpriteAsset == null)
																														{
																															return false;
																														}
																														MaterialReferenceManager.AddSpriteAsset(valueHashCode2, tmp_SpriteAsset);
																														this.m_currentSpriteAsset = tmp_SpriteAsset;
																													}
																													if (this.m_xmlAttribute[0].valueType == TagType.NumericalValue)
																													{
																														int num11 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																														if (num11 == -9999)
																														{
																															return false;
																														}
																														if (num11 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																														{
																															return false;
																														}
																														this.m_spriteIndex = num11;
																													}
																													else if (this.m_xmlAttribute[1].nameHashCode == 43347)
																													{
																														int spriteIndex = this.m_currentSpriteAsset.GetSpriteIndex(this.m_xmlAttribute[1].valueHashCode);
																														if (spriteIndex == -1)
																														{
																															return false;
																														}
																														this.m_spriteIndex = spriteIndex;
																													}
																													else
																													{
																														if (this.m_xmlAttribute[1].nameHashCode != 295562)
																														{
																															return false;
																														}
																														int num12 = (int)this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength, this.m_xmlAttribute[1].valueDecimalIndex);
																														if (num12 == -9999)
																														{
																															return false;
																														}
																														if (num12 > this.m_currentSpriteAsset.spriteInfoList.Count - 1)
																														{
																															return false;
																														}
																														this.m_spriteIndex = num12;
																													}
																													this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentSpriteAsset.material, this.m_currentSpriteAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																													this.m_spriteColor = TMP_Text.s_colorWhite;
																													this.m_tintSprite = false;
																													if (this.m_xmlAttribute[1].nameHashCode == 45819)
																													{
																														this.m_tintSprite = (this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength, this.m_xmlAttribute[1].valueDecimalIndex) != 0f);
																													}
																													else if (this.m_xmlAttribute[2].nameHashCode == 45819)
																													{
																														this.m_tintSprite = (this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[2].valueStartIndex, this.m_xmlAttribute[2].valueLength, this.m_xmlAttribute[2].valueDecimalIndex) != 0f);
																													}
																													if (this.m_xmlAttribute[1].nameHashCode == 281955)
																													{
																														this.m_spriteColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength);
																													}
																													else if (this.m_xmlAttribute[2].nameHashCode == 281955)
																													{
																														this.m_spriteColor = this.HexCharsToColor(this.m_htmlTag, this.m_xmlAttribute[2].valueStartIndex, this.m_xmlAttribute[2].valueLength);
																													}
																													this.m_xmlAttribute[1].nameHashCode = 0;
																													this.m_xmlAttribute[2].nameHashCode = 0;
																													this.m_textElementType = TMP_TextElementType.Sprite;
																													return true;
																												}
																												if (nameHashCode == 7443301)
																												{
																													if (this.m_isParsingText)
																													{
																													}
																													this.m_actionStack.Remove();
																													return true;
																												}
																												if (nameHashCode == 7513474)
																												{
																													this.m_cSpacing = 0f;
																													return true;
																												}
																												if (nameHashCode == 7598483)
																												{
																													this.tag_Indent = this.m_indentStack.Remove();
																													return true;
																												}
																												if (nameHashCode == 7639357)
																												{
																													this.m_marginLeft = 0f;
																													this.m_marginRight = 0f;
																													return true;
																												}
																												if (nameHashCode == 7681544)
																												{
																													this.m_monoSpacing = 0f;
																													return true;
																												}
																												if (nameHashCode == 15115642)
																												{
																													this.tag_NoParsing = true;
																													return true;
																												}
																												if (nameHashCode != 16034505)
																												{
																													if (nameHashCode != 52232547)
																													{
																														if (nameHashCode != 54741026)
																														{
																															return false;
																														}
																														this.m_baselineOffset = 0f;
																														return true;
																													}
																												}
																												else
																												{
																													float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																													if (num13 == -9999f || num13 == 0f)
																													{
																														return false;
																													}
																													if (tagUnits == TagUnits.Pixels)
																													{
																														this.m_baselineOffset = num13;
																														return true;
																													}
																													if (tagUnits != TagUnits.FontUnits)
																													{
																														return tagUnits != TagUnits.Percentage && false;
																													}
																													this.m_baselineOffset = num13 * this.m_fontScale * this.m_fontAsset.fontInfo.Ascender;
																													return true;
																												}
																											}
																											else
																											{
																												float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																												if (num13 == -9999f || num13 == 0f)
																												{
																													return false;
																												}
																												if (tagUnits != TagUnits.Pixels)
																												{
																													if (tagUnits != TagUnits.FontUnits)
																													{
																														if (tagUnits == TagUnits.Percentage)
																														{
																															return false;
																														}
																													}
																													else
																													{
																														this.m_monoSpacing = num13;
																														this.m_monoSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																													}
																												}
																												else
																												{
																													this.m_monoSpacing = num13;
																												}
																												return true;
																											}
																										}
																										else
																										{
																											float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																											if (num13 == -9999f || num13 == 0f)
																											{
																												return false;
																											}
																											this.m_marginLeft = num13;
																											if (tagUnits != TagUnits.Pixels)
																											{
																												if (tagUnits != TagUnits.FontUnits)
																												{
																													if (tagUnits == TagUnits.Percentage)
																													{
																														this.m_marginLeft = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginLeft / 100f;
																													}
																												}
																												else
																												{
																													this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																												}
																											}
																											this.m_marginLeft = ((this.m_marginLeft < 0f) ? 0f : this.m_marginLeft);
																											this.m_marginRight = this.m_marginLeft;
																											return true;
																										}
																									}
																									else
																									{
																										float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																										if (num13 == -9999f || num13 == 0f)
																										{
																											return false;
																										}
																										if (tagUnits != TagUnits.Pixels)
																										{
																											if (tagUnits != TagUnits.FontUnits)
																											{
																												if (tagUnits == TagUnits.Percentage)
																												{
																													this.tag_Indent = this.m_marginWidth * num13 / 100f;
																												}
																											}
																											else
																											{
																												this.tag_Indent = num13;
																												this.tag_Indent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																											}
																										}
																										else
																										{
																											this.tag_Indent = num13;
																										}
																										this.m_indentStack.Add(this.tag_Indent);
																										this.m_xAdvance = this.tag_Indent;
																										return true;
																									}
																								}
																								else
																								{
																									float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																									if (num13 == -9999f || num13 == 0f)
																									{
																										return false;
																									}
																									if (tagUnits != TagUnits.Pixels)
																									{
																										if (tagUnits != TagUnits.FontUnits)
																										{
																											if (tagUnits == TagUnits.Percentage)
																											{
																												return false;
																											}
																										}
																										else
																										{
																											this.m_cSpacing = num13;
																											this.m_cSpacing *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																										}
																									}
																									else
																									{
																										this.m_cSpacing = num13;
																									}
																									return true;
																								}
																							}
																							else
																							{
																								TMP_Style style = TMP_StyleSheet.GetStyle(this.m_xmlAttribute[0].valueHashCode);
																								if (style == null)
																								{
																									int hashCode = this.m_styleStack.Remove();
																									style = TMP_StyleSheet.GetStyle(hashCode);
																								}
																								if (style == null)
																								{
																									return false;
																								}
																								for (int i = 0; i < style.styleClosingTagArray.Length; i++)
																								{
																									if (style.styleClosingTagArray[i] == 60)
																									{
																										this.ValidateHtmlTag(style.styleClosingTagArray, i + 1, out i);
																									}
																								}
																								return true;
																							}
																						}
																						else
																						{
																							float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																							if (num13 == -9999f || num13 == 0f)
																							{
																								return false;
																							}
																							if (tagUnits != TagUnits.Pixels)
																							{
																								if (tagUnits == TagUnits.FontUnits)
																								{
																									return false;
																								}
																								if (tagUnits == TagUnits.Percentage)
																								{
																									this.m_width = this.m_marginWidth * num13 / 100f;
																								}
																							}
																							else
																							{
																								this.m_width = num13;
																							}
																							return true;
																						}
																					}
																					else
																					{
																						TMP_Style style = TMP_StyleSheet.GetStyle(this.m_xmlAttribute[0].valueHashCode);
																						if (style == null)
																						{
																							return false;
																						}
																						this.m_styleStack.Add(style.hashCode);
																						for (int j = 0; j < style.styleOpeningTagArray.Length; j++)
																						{
																							if (style.styleOpeningTagArray[j] == 60)
																							{
																								this.ValidateHtmlTag(style.styleOpeningTagArray, j + 1, out j);
																							}
																						}
																						return true;
																					}
																				}
																				else
																				{
																					float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																					if (num13 == -9999f || num13 == 0f)
																					{
																						return false;
																					}
																					if (tagUnits == TagUnits.Pixels)
																					{
																						this.m_xAdvance += num13;
																						return true;
																					}
																					if (tagUnits != TagUnits.FontUnits)
																					{
																						return tagUnits != TagUnits.Percentage && false;
																					}
																					this.m_xAdvance += num13 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																					return true;
																				}
																			}
																			else
																			{
																				if (this.m_htmlTag[6] == '#' && num == 13)
																				{
																					this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (this.m_htmlTag[6] == '#' && num == 15)
																				{
																					this.m_htmlColor = this.HexCharsToColor(this.m_htmlTag, num);
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				int valueHashCode3 = this.m_xmlAttribute[0].valueHashCode;
																				if (valueHashCode3 == -36881330)
																				{
																					this.m_htmlColor = new Color32(160, 32, 240, byte.MaxValue);
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 125395)
																				{
																					this.m_htmlColor = Color.red;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 3573310)
																				{
																					this.m_htmlColor = Color.blue;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 26556144)
																				{
																					this.m_htmlColor = new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 117905991)
																				{
																					this.m_htmlColor = Color.black;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 121463835)
																				{
																					this.m_htmlColor = Color.green;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 == 140357351)
																				{
																					this.m_htmlColor = Color.white;
																					this.m_colorStack.Add(this.m_htmlColor);
																					return true;
																				}
																				if (valueHashCode3 != 554054276)
																				{
																					return false;
																				}
																				this.m_htmlColor = Color.yellow;
																				this.m_colorStack.Add(this.m_htmlColor);
																				return true;
																			}
																		}
																		else
																		{
																			if (this.m_xmlAttribute[0].valueLength != 3)
																			{
																				return false;
																			}
																			this.m_htmlColor.a = (byte)(this.HexToInt(this.m_htmlTag[7]) * 16 + this.HexToInt(this.m_htmlTag[8]));
																			return true;
																		}
																	}
																	else
																	{
																		int valueHashCode4 = this.m_xmlAttribute[0].valueHashCode;
																		if (valueHashCode4 == -523808257)
																		{
																			this.m_lineJustification = TextAlignmentOptions.Justified;
																			return true;
																		}
																		if (valueHashCode4 == -458210101)
																		{
																			this.m_lineJustification = TextAlignmentOptions.Center;
																			return true;
																		}
																		if (valueHashCode4 == 3774683)
																		{
																			this.m_lineJustification = TextAlignmentOptions.Left;
																			return true;
																		}
																		if (valueHashCode4 != 136703040)
																		{
																			return false;
																		}
																		this.m_lineJustification = TextAlignmentOptions.Right;
																		return true;
																	}
																}
																else
																{
																	float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
																	if (num13 == -9999f || num13 == 0f)
																	{
																		return false;
																	}
																	if (tagUnits != TagUnits.Pixels)
																	{
																		if (tagUnits == TagUnits.FontUnits)
																		{
																			this.m_currentFontSize = this.m_fontSize * num13;
																			this.m_sizeStack.Add(this.m_currentFontSize);
																			this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																			return true;
																		}
																		if (tagUnits != TagUnits.Percentage)
																		{
																			return false;
																		}
																		this.m_currentFontSize = this.m_fontSize * num13 / 100f;
																		this.m_sizeStack.Add(this.m_currentFontSize);
																		this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																		return true;
																	}
																	else
																	{
																		if (this.m_htmlTag[5] == '+')
																		{
																			this.m_currentFontSize = this.m_fontSize + num13;
																			this.m_sizeStack.Add(this.m_currentFontSize);
																			this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																			return true;
																		}
																		if (this.m_htmlTag[5] == '-')
																		{
																			this.m_currentFontSize = this.m_fontSize + num13;
																			this.m_sizeStack.Add(this.m_currentFontSize);
																			this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																			return true;
																		}
																		this.m_currentFontSize = num13;
																		this.m_sizeStack.Add(this.m_currentFontSize);
																		this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																		return true;
																	}
																}
															}
															else
															{
																int valueHashCode5 = this.m_xmlAttribute[0].valueHashCode;
																int nameHashCode2 = this.m_xmlAttribute[1].nameHashCode;
																int valueHashCode6 = this.m_xmlAttribute[1].valueHashCode;
																if (valueHashCode5 == 764638571 || valueHashCode5 == 523367755)
																{
																	this.m_currentFontAsset = this.m_materialReferences[0].fontAsset;
																	this.m_currentMaterial = this.m_materialReferences[0].material;
																	this.m_currentMaterialIndex = 0;
																	this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																	this.m_materialReferenceStack.Add(this.m_materialReferences[0]);
																	return true;
																}
																TMP_FontAsset tmp_FontAsset;
																if (!MaterialReferenceManager.TryGetFontAsset(valueHashCode5, out tmp_FontAsset))
																{
																	tmp_FontAsset = Resources.Load<TMP_FontAsset>("Fonts & Materials/" + new string(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength));
																	if (tmp_FontAsset == null)
																	{
																		return false;
																	}
																	MaterialReferenceManager.AddFontAsset(tmp_FontAsset);
																}
																if (nameHashCode2 == 0 && valueHashCode6 == 0)
																{
																	this.m_currentMaterial = tmp_FontAsset.material;
																	this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																	this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																}
																else
																{
																	if (nameHashCode2 != 103415287)
																	{
																		return false;
																	}
																	Material material;
																	if (MaterialReferenceManager.TryGetMaterial(valueHashCode6, out material))
																	{
																		this.m_currentMaterial = material;
																		this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																		this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																	}
																	else
																	{
																		material = Resources.Load<Material>("Fonts & Materials/" + new string(this.m_htmlTag, this.m_xmlAttribute[1].valueStartIndex, this.m_xmlAttribute[1].valueLength));
																		if (material == null)
																		{
																			return false;
																		}
																		MaterialReferenceManager.AddFontMaterial(valueHashCode6, material);
																		this.m_currentMaterial = material;
																		this.m_currentMaterialIndex = MaterialReference.AddMaterialReference(this.m_currentMaterial, tmp_FontAsset, this.m_materialReferences, this.m_materialReferenceIndexLookup);
																		this.m_materialReferenceStack.Add(this.m_materialReferences[this.m_currentMaterialIndex]);
																	}
																}
																this.m_currentFontAsset = tmp_FontAsset;
																this.m_fontScale = this.m_currentFontSize / this.m_currentFontAsset.fontInfo.PointSize * this.m_currentFontAsset.fontInfo.Scale * ((!this.m_isOrthographic) ? 0.1f : 1f);
																return true;
															}
														}
														else
														{
															float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
															if (num13 == -9999f)
															{
																return false;
															}
															if (tagUnits == TagUnits.Pixels)
															{
																this.m_xAdvance = num13;
																return true;
															}
															if (tagUnits == TagUnits.FontUnits)
															{
																this.m_xAdvance = num13 * this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
																return true;
															}
															if (tagUnits != TagUnits.Percentage)
															{
																return false;
															}
															this.m_xAdvance = this.m_marginWidth * num13 / 100f;
															return true;
														}
													}
													else
													{
														float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
														if (num13 == -9999f || num13 == 0f)
														{
															return false;
														}
														if ((this.m_fontStyle & FontStyles.Bold) == FontStyles.Bold)
														{
															return true;
														}
														this.m_style &= (FontStyles)(-2);
														int num14 = (int)num13;
														if (num14 != 100)
														{
															if (num14 != 200)
															{
																if (num14 != 300)
																{
																	if (num14 != 400)
																	{
																		if (num14 != 500)
																		{
																			if (num14 != 600)
																			{
																				if (num14 != 700)
																				{
																					if (num14 != 800)
																					{
																						if (num14 == 900)
																						{
																							this.m_fontWeightInternal = 900;
																						}
																					}
																					else
																					{
																						this.m_fontWeightInternal = 800;
																					}
																				}
																				else
																				{
																					this.m_fontWeightInternal = 700;
																					this.m_style |= FontStyles.Bold;
																				}
																			}
																			else
																			{
																				this.m_fontWeightInternal = 600;
																			}
																		}
																		else
																		{
																			this.m_fontWeightInternal = 500;
																		}
																	}
																	else
																	{
																		this.m_fontWeightInternal = 400;
																	}
																}
																else
																{
																	this.m_fontWeightInternal = 300;
																}
															}
															else
															{
																this.m_fontWeightInternal = 200;
															}
														}
														else
														{
															this.m_fontWeightInternal = 100;
														}
														this.m_fontWeightStack.Add(this.m_fontWeightInternal);
														return true;
													}
												}
												else
												{
													float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
													if (num13 == -9999f || num13 == 0f)
													{
														return false;
													}
													this.m_marginRight = num13;
													if (tagUnits != TagUnits.Pixels)
													{
														if (tagUnits != TagUnits.FontUnits)
														{
															if (tagUnits == TagUnits.Percentage)
															{
																this.m_marginRight = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginRight / 100f;
															}
														}
														else
														{
															this.m_marginRight *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
														}
													}
													this.m_marginRight = ((this.m_marginRight < 0f) ? 0f : this.m_marginRight);
													return true;
												}
											}
											this.m_style &= (FontStyles)(-17);
											return true;
										}
										this.m_style &= (FontStyles)(-33);
										return true;
									}
									else
									{
										float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
										if (num13 == -9999f || num13 == 0f)
										{
											return false;
										}
										if (tagUnits != TagUnits.Pixels)
										{
											if (tagUnits != TagUnits.FontUnits)
											{
												if (tagUnits == TagUnits.Percentage)
												{
													this.tag_LineIndent = this.m_marginWidth * num13 / 100f;
												}
											}
											else
											{
												this.tag_LineIndent = num13;
												this.tag_LineIndent *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
											}
										}
										else
										{
											this.tag_LineIndent = num13;
										}
										this.m_xAdvance += this.tag_LineIndent;
										return true;
									}
								}
								else
								{
									float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
									if (num13 == -9999f || num13 == 0f)
									{
										return false;
									}
									this.m_lineHeight = num13;
									if (tagUnits != TagUnits.Pixels)
									{
										if (tagUnits != TagUnits.FontUnits)
										{
											if (tagUnits == TagUnits.Percentage)
											{
												this.m_lineHeight = this.m_fontAsset.fontInfo.LineHeight * this.m_lineHeight / 100f * this.m_fontScale;
											}
										}
										else
										{
											this.m_lineHeight *= this.m_fontAsset.fontInfo.LineHeight * this.m_fontScale;
										}
									}
									return true;
								}
							}
							else
							{
								float num13 = this.ConvertToFloat(this.m_htmlTag, this.m_xmlAttribute[0].valueStartIndex, this.m_xmlAttribute[0].valueLength, this.m_xmlAttribute[0].valueDecimalIndex);
								if (num13 == -9999f || num13 == 0f)
								{
									return false;
								}
								this.m_marginLeft = num13;
								if (tagUnits != TagUnits.Pixels)
								{
									if (tagUnits != TagUnits.FontUnits)
									{
										if (tagUnits == TagUnits.Percentage)
										{
											this.m_marginLeft = (this.m_marginWidth - ((this.m_width == -1f) ? 0f : this.m_width)) * this.m_marginLeft / 100f;
										}
									}
									else
									{
										this.m_marginLeft *= this.m_fontScale * this.m_fontAsset.fontInfo.TabWidth / (float)this.m_fontAsset.tabSize;
									}
								}
								this.m_marginLeft = ((this.m_marginLeft < 0f) ? 0f : this.m_marginLeft);
								return true;
							}
						}
					}
					this.m_style |= FontStyles.UpperCase;
					return true;
				case 446:
					if ((this.m_fontStyle & FontStyles.Underline) != FontStyles.Underline)
					{
						this.m_style &= (FontStyles)(-5);
					}
					return true;
				}
				break;
			case 117:
				this.m_style |= FontStyles.Underline;
				return true;
			}
		}

		// Token: 0x04003636 RID: 13878
		[SerializeField]
		public string m_text;

		// Token: 0x04003637 RID: 13879
		[SerializeField]
		public TMP_FontAsset m_fontAsset;

		// Token: 0x04003638 RID: 13880
		public TMP_FontAsset m_currentFontAsset;

		// Token: 0x04003639 RID: 13881
		public bool m_isSDFShader;

		// Token: 0x0400363A RID: 13882
		[SerializeField]
		public Material m_sharedMaterial;

		// Token: 0x0400363B RID: 13883
		public Material m_currentMaterial;

		// Token: 0x0400363C RID: 13884
		public MaterialReference[] m_materialReferences = new MaterialReference[32];

		// Token: 0x0400363D RID: 13885
		public Dictionary<int, int> m_materialReferenceIndexLookup = new Dictionary<int, int>();

		// Token: 0x0400363E RID: 13886
		public TMP_XmlTagStack<MaterialReference> m_materialReferenceStack = new TMP_XmlTagStack<MaterialReference>(new MaterialReference[16]);

		// Token: 0x0400363F RID: 13887
		public int m_currentMaterialIndex;

		// Token: 0x04003640 RID: 13888
		public int m_sharedMaterialHashCode;

		// Token: 0x04003641 RID: 13889
		[SerializeField]
		public Material[] m_fontSharedMaterials;

		// Token: 0x04003642 RID: 13890
		[SerializeField]
		public Material m_fontMaterial;

		// Token: 0x04003643 RID: 13891
		[SerializeField]
		public Material[] m_fontMaterials;

		// Token: 0x04003644 RID: 13892
		public bool m_isMaterialDirty;

		// Token: 0x04003645 RID: 13893
		[FormerlySerializedAs("m_fontColor")]
		[SerializeField]
		public Color32 m_fontColor32 = Color.white;

		// Token: 0x04003646 RID: 13894
		[SerializeField]
		public Color m_fontColor = Color.white;

		// Token: 0x04003647 RID: 13895
		public static Color32 s_colorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04003648 RID: 13896
		[SerializeField]
		public bool m_enableVertexGradient;

		// Token: 0x04003649 RID: 13897
		[SerializeField]
		public VertexGradient m_fontColorGradient = new VertexGradient(Color.white);

		// Token: 0x0400364A RID: 13898
		public TMP_SpriteAsset m_spriteAsset;

		// Token: 0x0400364B RID: 13899
		[SerializeField]
		public bool m_tintAllSprites;

		// Token: 0x0400364C RID: 13900
		public bool m_tintSprite;

		// Token: 0x0400364D RID: 13901
		public Color32 m_spriteColor;

		// Token: 0x0400364E RID: 13902
		[SerializeField]
		public bool m_overrideHtmlColors;

		// Token: 0x0400364F RID: 13903
		[SerializeField]
		public Color32 m_faceColor = Color.white;

		// Token: 0x04003650 RID: 13904
		[SerializeField]
		public Color32 m_outlineColor = Color.black;

		// Token: 0x04003651 RID: 13905
		public float m_outlineWidth;

		// Token: 0x04003652 RID: 13906
		[SerializeField]
		public float m_fontSize = 36f;

		// Token: 0x04003653 RID: 13907
		public float m_currentFontSize;

		// Token: 0x04003654 RID: 13908
		[SerializeField]
		public float m_fontSizeBase = 36f;

		// Token: 0x04003655 RID: 13909
		public TMP_XmlTagStack<float> m_sizeStack = new TMP_XmlTagStack<float>(new float[16]);

		// Token: 0x04003656 RID: 13910
		[SerializeField]
		public int m_fontWeight = 400;

		// Token: 0x04003657 RID: 13911
		public int m_fontWeightInternal;

		// Token: 0x04003658 RID: 13912
		public TMP_XmlTagStack<int> m_fontWeightStack = new TMP_XmlTagStack<int>(new int[16]);

		// Token: 0x04003659 RID: 13913
		[SerializeField]
		public bool m_enableAutoSizing;

		// Token: 0x0400365A RID: 13914
		public float m_maxFontSize;

		// Token: 0x0400365B RID: 13915
		public float m_minFontSize;

		// Token: 0x0400365C RID: 13916
		[SerializeField]
		public float m_fontSizeMin;

		// Token: 0x0400365D RID: 13917
		[SerializeField]
		public float m_fontSizeMax;

		// Token: 0x0400365E RID: 13918
		[SerializeField]
		public FontStyles m_fontStyle;

		// Token: 0x0400365F RID: 13919
		public FontStyles m_style;

		// Token: 0x04003660 RID: 13920
		public bool m_isUsingBold;

		// Token: 0x04003661 RID: 13921
		[SerializeField]
		[FormerlySerializedAs("m_lineJustification")]
		public TextAlignmentOptions m_textAlignment;

		// Token: 0x04003662 RID: 13922
		public TextAlignmentOptions m_lineJustification;

		// Token: 0x04003663 RID: 13923
		public Vector3[] m_textContainerLocalCorners = new Vector3[4];

		// Token: 0x04003664 RID: 13924
		[SerializeField]
		public float m_characterSpacing;

		// Token: 0x04003665 RID: 13925
		public float m_cSpacing;

		// Token: 0x04003666 RID: 13926
		public float m_monoSpacing;

		// Token: 0x04003667 RID: 13927
		[SerializeField]
		public float m_lineSpacing;

		// Token: 0x04003668 RID: 13928
		public float m_lineSpacingDelta;

		// Token: 0x04003669 RID: 13929
		public float m_lineHeight;

		// Token: 0x0400366A RID: 13930
		[SerializeField]
		public float m_lineSpacingMax;

		// Token: 0x0400366B RID: 13931
		[SerializeField]
		public float m_paragraphSpacing;

		// Token: 0x0400366C RID: 13932
		[SerializeField]
		public float m_charWidthMaxAdj;

		// Token: 0x0400366D RID: 13933
		public float m_charWidthAdjDelta;

		// Token: 0x0400366E RID: 13934
		[SerializeField]
		public bool m_enableWordWrapping;

		// Token: 0x0400366F RID: 13935
		public bool m_isCharacterWrappingEnabled;

		// Token: 0x04003670 RID: 13936
		public bool m_isNonBreakingSpace;

		// Token: 0x04003671 RID: 13937
		public bool m_isIgnoringAlignment;

		// Token: 0x04003672 RID: 13938
		[SerializeField]
		public float m_wordWrappingRatios = 0.4f;

		// Token: 0x04003673 RID: 13939
		[SerializeField]
		public TextOverflowModes m_overflowMode;

		// Token: 0x04003674 RID: 13940
		public bool m_isTextTruncated;

		// Token: 0x04003675 RID: 13941
		[SerializeField]
		public bool m_enableKerning;

		// Token: 0x04003676 RID: 13942
		[SerializeField]
		public bool m_enableExtraPadding;

		// Token: 0x04003677 RID: 13943
		[SerializeField]
		public bool checkPaddingRequired;

		// Token: 0x04003678 RID: 13944
		[SerializeField]
		public bool m_isRichText = true;

		// Token: 0x04003679 RID: 13945
		public bool m_parseCtrlCharacters = true;

		// Token: 0x0400367A RID: 13946
		public bool m_isOverlay;

		// Token: 0x0400367B RID: 13947
		[SerializeField]
		public bool m_isOrthographic;

		// Token: 0x0400367C RID: 13948
		[SerializeField]
		public bool m_isCullingEnabled;

		// Token: 0x0400367D RID: 13949
		[SerializeField]
		public bool m_ignoreCulling = true;

		// Token: 0x0400367E RID: 13950
		[SerializeField]
		public TextureMappingOptions m_horizontalMapping;

		// Token: 0x0400367F RID: 13951
		[SerializeField]
		public TextureMappingOptions m_verticalMapping;

		// Token: 0x04003680 RID: 13952
		public TextRenderFlags m_renderMode = TextRenderFlags.Render;

		// Token: 0x04003681 RID: 13953
		public int m_maxVisibleCharacters = 99999;

		// Token: 0x04003682 RID: 13954
		public int m_maxVisibleWords = 99999;

		// Token: 0x04003683 RID: 13955
		public int m_maxVisibleLines = 99999;

		// Token: 0x04003684 RID: 13956
		[SerializeField]
		public int m_pageToDisplay = 1;

		// Token: 0x04003685 RID: 13957
		public bool m_isNewPage;

		// Token: 0x04003686 RID: 13958
		[SerializeField]
		public Vector4 m_margin = new Vector4(0f, 0f, 0f, 0f);

		// Token: 0x04003687 RID: 13959
		public float m_marginLeft;

		// Token: 0x04003688 RID: 13960
		public float m_marginRight;

		// Token: 0x04003689 RID: 13961
		public float m_marginWidth;

		// Token: 0x0400368A RID: 13962
		public float m_marginHeight;

		// Token: 0x0400368B RID: 13963
		public float m_width = -1f;

		// Token: 0x0400368C RID: 13964
		[SerializeField]
		public TMP_TextInfo m_textInfo;

		// Token: 0x0400368D RID: 13965
		[SerializeField]
		public bool m_havePropertiesChanged;

		// Token: 0x0400368E RID: 13966
		[SerializeField]
		public bool m_isUsingLegacyAnimationComponent;

		// Token: 0x0400368F RID: 13967
		public Transform m_transform;

		// Token: 0x04003690 RID: 13968
		public RectTransform m_rectTransform;

		// Token: 0x04003692 RID: 13970
		public Mesh m_mesh;

		// Token: 0x04003694 RID: 13972
		public float m_flexibleHeight = -1f;

		// Token: 0x04003695 RID: 13973
		public float m_flexibleWidth = -1f;

		// Token: 0x04003696 RID: 13974
		public float m_minHeight;

		// Token: 0x04003697 RID: 13975
		public float m_minWidth;

		// Token: 0x04003698 RID: 13976
		public float m_preferredWidth = 9999f;

		// Token: 0x04003699 RID: 13977
		public float m_renderedWidth;

		// Token: 0x0400369A RID: 13978
		public float m_preferredHeight = 9999f;

		// Token: 0x0400369B RID: 13979
		public float m_renderedHeight;

		// Token: 0x0400369C RID: 13980
		public int m_layoutPriority;

		// Token: 0x0400369D RID: 13981
		public bool m_isCalculateSizeRequired;

		// Token: 0x0400369E RID: 13982
		public bool m_isLayoutDirty;

		// Token: 0x0400369F RID: 13983
		public bool m_verticesAlreadyDirty;

		// Token: 0x040036A0 RID: 13984
		public bool m_layoutAlreadyDirty;

		// Token: 0x040036A1 RID: 13985
		[SerializeField]
		public bool m_isInputParsingRequired;

		// Token: 0x040036A2 RID: 13986
		[SerializeField]
		public bool m_isRightToLeft;

		// Token: 0x040036A3 RID: 13987
		[SerializeField]
		public TMP_Text.TextInputSources m_inputSource;

		// Token: 0x040036A4 RID: 13988
		public string old_text;

		// Token: 0x040036A5 RID: 13989
		public float old_arg0;

		// Token: 0x040036A6 RID: 13990
		public float old_arg1;

		// Token: 0x040036A7 RID: 13991
		public float old_arg2;

		// Token: 0x040036A8 RID: 13992
		public float m_fontScale;

		// Token: 0x040036A9 RID: 13993
		public float m_fontScaleMultiplier;

		// Token: 0x040036AA RID: 13994
		public char[] m_htmlTag = new char[64];

		// Token: 0x040036AB RID: 13995
		public XML_TagAttribute[] m_xmlAttribute = new XML_TagAttribute[8];

		// Token: 0x040036AC RID: 13996
		public float tag_LineIndent;

		// Token: 0x040036AD RID: 13997
		public float tag_Indent;

		// Token: 0x040036AE RID: 13998
		public TMP_XmlTagStack<float> m_indentStack = new TMP_XmlTagStack<float>(new float[16]);

		// Token: 0x040036AF RID: 13999
		public bool tag_NoParsing;

		// Token: 0x040036B0 RID: 14000
		public bool m_isParsingText;

		// Token: 0x040036B1 RID: 14001
		public int[] m_char_buffer;

		// Token: 0x040036B2 RID: 14002
		public TMP_CharacterInfo[] m_internalCharacterInfo;

		// Token: 0x040036B3 RID: 14003
		public char[] m_input_CharArray = new char[256];

		// Token: 0x040036B4 RID: 14004
		public int m_charArray_Length;

		// Token: 0x040036B5 RID: 14005
		public int m_totalCharacterCount;

		// Token: 0x040036B6 RID: 14006
		public int m_characterCount;

		// Token: 0x040036B7 RID: 14007
		public int m_visibleCharacterCount;

		// Token: 0x040036B8 RID: 14008
		public int m_visibleSpriteCount;

		// Token: 0x040036B9 RID: 14009
		public int m_firstCharacterOfLine;

		// Token: 0x040036BA RID: 14010
		public int m_firstVisibleCharacterOfLine;

		// Token: 0x040036BB RID: 14011
		public int m_lastCharacterOfLine;

		// Token: 0x040036BC RID: 14012
		public int m_lastVisibleCharacterOfLine;

		// Token: 0x040036BD RID: 14013
		public int m_lineNumber;

		// Token: 0x040036BE RID: 14014
		public int m_pageNumber;

		// Token: 0x040036BF RID: 14015
		public float m_maxAscender;

		// Token: 0x040036C0 RID: 14016
		public float m_maxDescender;

		// Token: 0x040036C1 RID: 14017
		public float m_maxLineAscender;

		// Token: 0x040036C2 RID: 14018
		public float m_maxLineDescender;

		// Token: 0x040036C3 RID: 14019
		public float m_startOfLineAscender;

		// Token: 0x040036C4 RID: 14020
		public float m_lineOffset;

		// Token: 0x040036C5 RID: 14021
		public Extents m_meshExtents;

		// Token: 0x040036C6 RID: 14022
		public Color32 m_htmlColor = new Color(255f, 255f, 255f, 128f);

		// Token: 0x040036C7 RID: 14023
		public TMP_XmlTagStack<Color32> m_colorStack = new TMP_XmlTagStack<Color32>(new Color32[16]);

		// Token: 0x040036C8 RID: 14024
		public float m_tabSpacing;

		// Token: 0x040036C9 RID: 14025
		public float m_spacing;

		// Token: 0x040036CA RID: 14026
		public bool IsRectTransformDriven;

		// Token: 0x040036CB RID: 14027
		public TMP_XmlTagStack<int> m_styleStack = new TMP_XmlTagStack<int>(new int[16]);

		// Token: 0x040036CC RID: 14028
		public TMP_XmlTagStack<int> m_actionStack = new TMP_XmlTagStack<int>(new int[16]);

		// Token: 0x040036CD RID: 14029
		public float m_padding;

		// Token: 0x040036CE RID: 14030
		public float m_baselineOffset;

		// Token: 0x040036CF RID: 14031
		public float m_xAdvance;

		// Token: 0x040036D0 RID: 14032
		public TMP_TextElementType m_textElementType;

		// Token: 0x040036D1 RID: 14033
		public TMP_TextElement m_cached_TextElement;

		// Token: 0x040036D2 RID: 14034
		public TMP_Glyph m_cached_Underline_GlyphInfo;

		// Token: 0x040036D3 RID: 14035
		public TMP_Glyph m_cached_Ellipsis_GlyphInfo;

		// Token: 0x040036D4 RID: 14036
		public TMP_SpriteAsset m_defaultSpriteAsset;

		// Token: 0x040036D5 RID: 14037
		public TMP_SpriteAsset m_currentSpriteAsset;

		// Token: 0x040036D6 RID: 14038
		public int m_spriteCount;

		// Token: 0x040036D7 RID: 14039
		public int m_spriteIndex;

		// Token: 0x040036D8 RID: 14040
		public InlineGraphicManager m_inlineGraphics;

		// Token: 0x040036D9 RID: 14041
		public readonly float[] k_Power = new float[]
		{
			0.5f,
			0.05f,
			0.005f,
			0.0005f,
			5E-05f,
			5E-06f,
			5E-07f,
			5E-08f,
			5E-09f,
			5E-10f
		};

		// Token: 0x040036DA RID: 14042
		public static Vector2 k_InfinityVectorPositive = new Vector2(1000000f, 1000000f);

		// Token: 0x040036DB RID: 14043
		public static Vector2 k_InfinityVectorNegative = new Vector2(-1000000f, -1000000f);

		// Token: 0x020012FB RID: 4859
		public enum TextInputSources
		{
			// Token: 0x040081F7 RID: 33271
			Text,
			// Token: 0x040081F8 RID: 33272
			SetText,
			// Token: 0x040081F9 RID: 33273
			SetCharArray
		}
	}
}
