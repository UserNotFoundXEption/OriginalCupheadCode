using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Token: 0x0200008D RID: 141
public static class FontLoader
{
	// Token: 0x0600069F RID: 1695 RVA: 0x0006FF68 File Offset: 0x0006E168
	public static Coroutine[] Initialize()
	{
		Array values = Enum.GetValues(typeof(FontLoader.FontType));
		Array values2 = Enum.GetValues(typeof(FontLoader.TMPFontType));
		Coroutine[] array = new Coroutine[values.Length + values2.Length - 2];
		for (int i = 1; i < values.Length; i++)
		{
			FontLoader.FontType fontType = (FontLoader.FontType)values.GetValue(i);
			string bundleName = fontType.ToString();
			Action<Font> completionHandler = delegate(Font font)
			{
				FontLoader.FontType fontType = fontType;
				FontLoader.FontCache.Add(fontType, font);
			};
			array[i - 1] = AssetBundleLoader.LoadFont(bundleName, FontLoader.GetFilename(fontType), completionHandler);
		}
		for (int j = 1; j < values2.Length; j++)
		{
			FontLoader.TMPFontType fontType = (FontLoader.TMPFontType)values2.GetValue(j);
			string bundleName2 = fontType.ToString();
			Action<Object[]> completionHandler2 = delegate(Object[] objects)
			{
				foreach (Object @object in objects)
				{
					if (@object is TMP_FontAsset)
					{
						FontLoader.TMPFontType fontType = fontType;
						FontLoader.TMPFontCache.Add(fontType, (TMP_FontAsset)@object);
					}
					else
					{
						if (!(@object is Material))
						{
							throw new Exception("Unhandled object type: " + @object.GetType());
						}
						FontLoader.TMPMaterialCache.Add(@object.name, (Material)@object);
					}
				}
			};
			array[j - 1 + (values.Length - 1)] = AssetBundleLoader.LoadTMPFont(bundleName2, completionHandler2);
		}
		return array;
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00006BC2 File Offset: 0x00004DC2
	public static Font GetFont(FontLoader.FontType fontType)
	{
		if (fontType == FontLoader.FontType.None)
		{
			return null;
		}
		return FontLoader.FontCache[fontType];
	}

	// Token: 0x060006A1 RID: 1697 RVA: 0x00006BD7 File Offset: 0x00004DD7
	public static TMP_FontAsset GetTMPFont(FontLoader.TMPFontType fontType)
	{
		if (fontType == FontLoader.TMPFontType.None)
		{
			return null;
		}
		return FontLoader.TMPFontCache[fontType];
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00006BEC File Offset: 0x00004DEC
	public static Material GetTMPMaterial(string materialName)
	{
		return FontLoader.TMPMaterialCache[materialName];
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x0007008C File Offset: 0x0006E28C
	public static string ConvertAssetNameToEnumName(string assetName)
	{
		assetName = assetName.Replace("-", "_");
		assetName = assetName.Replace(" ", "__");
		assetName = assetName.Replace("(", "66");
		assetName = assetName.Replace(")", "99");
		return assetName;
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x00006BF9 File Offset: 0x00004DF9
	public static string GetFilename(FontLoader.FontType fontType)
	{
		return FontLoader.FontTypeMapping[fontType];
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x00006C06 File Offset: 0x00004E06
	public static string GetFilename(FontLoader.TMPFontType fontType)
	{
		return FontLoader.TMPFontTypeMapping[fontType];
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x00006C13 File Offset: 0x00004E13
	public static FontLoader.FontType ConvertFontToFontType(Font font)
	{
		return FontLoader.ConvertAssetNameToFontType(font.name);
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000700E4 File Offset: 0x0006E2E4
	public static FontLoader.FontType ConvertAssetNameToFontType(string assetName)
	{
		string value = FontLoader.ConvertAssetNameToEnumName(assetName);
		return (FontLoader.FontType)Enum.Parse(typeof(FontLoader.FontType), value);
	}

	// Token: 0x060006A8 RID: 1704 RVA: 0x00006C20 File Offset: 0x00004E20
	public static FontLoader.TMPFontType ConvertTMPFontAssetToTMPFontType(TMP_FontAsset fontAsset)
	{
		return FontLoader.ConvertAssetNameToTMPFontType(fontAsset.name);
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00070110 File Offset: 0x0006E310
	public static FontLoader.TMPFontType ConvertAssetNameToTMPFontType(string assetName)
	{
		string text = FontLoader.ConvertAssetNameToEnumName(assetName);
		if (text == "rounded_mgenplus_1c_medium__SDF")
		{
			text = "rounded_mgenplus_1c_meduim__SDF";
		}
		return (FontLoader.TMPFontType)Enum.Parse(typeof(FontLoader.TMPFontType), text);
	}

	// Token: 0x040004F3 RID: 1267
	public static Dictionary<FontLoader.FontType, string> FontTypeMapping = new Dictionary<FontLoader.FontType, string>
	{
		{
			FontLoader.FontType.CupheadFelix_Regular_merged,
			"CupheadFelix-Regular-merged"
		},
		{
			FontLoader.FontType.CupheadHenriette_A_merged,
			"CupheadHenriette-A-merged"
		},
		{
			FontLoader.FontType.CupheadMemphis_Medium_merged,
			"CupheadMemphis-Medium-merged"
		},
		{
			FontLoader.FontType.CupheadPoster_Regular66Cyr_Lat_English99,
			"CupheadPoster-Regular(Cyr_Lat_English)"
		},
		{
			FontLoader.FontType.CupheadVogue_Bold_merged,
			"CupheadVogue-Bold-merged"
		},
		{
			FontLoader.FontType.CupheadVogue_ExtraBold_merged,
			"CupheadVogue-ExtraBold-merged"
		},
		{
			FontLoader.FontType.DFBrushRDStd_W7,
			"DFBrushRDStd-W7"
		},
		{
			FontLoader.FontType.DFBrushSQStd_W5,
			"DFBrushSQStd-W5"
		},
		{
			FontLoader.FontType.DSRefinedLetterB,
			"DSRefinedLetterB"
		},
		{
			FontLoader.FontType.FBBlue,
			"FBBlue"
		},
		{
			FontLoader.FontType.hyk2gjm,
			"hyk2gjm"
		},
		{
			FontLoader.FontType.jpchw00u,
			"jpchw00u"
		},
		{
			FontLoader.FontType.MComicPRC_Medium,
			"MComicPRC-Medium"
		},
		{
			FontLoader.FontType.YoonBackjaeM,
			"YoonBackjaeM"
		},
		{
			FontLoader.FontType.rounded_mgenplus_1c_medium,
			"rounded-mgenplus-1c-medium"
		},
		{
			FontLoader.FontType.hisikusa_A,
			"hisikusa-A"
		},
		{
			FontLoader.FontType.FGPotego__2,
			"FGPotego 2"
		},
		{
			FontLoader.FontType.FGPotegoBold__2,
			"FGPotegoBold 2"
		},
		{
			FontLoader.FontType.FGNewRetro,
			"FGNewRetro"
		},
		{
			FontLoader.FontType.ElegantHeiseiMinchoMono_9W,
			"ElegantHeiseiMinchoMono-9W"
		}
	};

	// Token: 0x040004F4 RID: 1268
	public static Dictionary<FontLoader.TMPFontType, string> TMPFontTypeMapping = new Dictionary<FontLoader.TMPFontType, string>
	{
		{
			FontLoader.TMPFontType.CupheadFelix_Regular_merged__SDF,
			"CupheadFelix-Regular-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadHenriette_A_merged__SDF,
			"CupheadHenriette-A-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadMemphis_Medium_merged__SDF,
			"CupheadMemphis-Medium-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadPoster_Regular66Cyr_Lat_English99__SDF,
			"CupheadPoster-Regular(Cyr_Lat_English) SDF"
		},
		{
			FontLoader.TMPFontType.CupheadVogue_Bold_merged__SDF,
			"CupheadVogue-Bold-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadVogue_ExtraBold_merged__outline__SDF,
			"CupheadVogue-ExtraBold-merged outline SDF"
		},
		{
			FontLoader.TMPFontType.CupheadVogue_ExtraBold_merged__SDF,
			"CupheadVogue-ExtraBold-merged SDF"
		},
		{
			FontLoader.TMPFontType.CupheadVogue_ExtraBold_merged__shadow__SDF,
			"CupheadVogue-ExtraBold-merged shadow SDF"
		},
		{
			FontLoader.TMPFontType.DFBrushRDStd_W7__outline__SDF,
			"DFBrushRDStd-W7 outline SDF"
		},
		{
			FontLoader.TMPFontType.DFBrushRDStd_W7__SDF,
			"DFBrushRDStd-W7 SDF"
		},
		{
			FontLoader.TMPFontType.DFBrushRDStd_W7__shadow__SDF,
			"DFBrushRDStd-W7 shadow SDF"
		},
		{
			FontLoader.TMPFontType.DFBrushSQStd_W5__SDF,
			"DFBrushSQStd-W5 SDF"
		},
		{
			FontLoader.TMPFontType.DSRefinedLetterB__SDF,
			"DSRefinedLetterB SDF"
		},
		{
			FontLoader.TMPFontType.FBBlue__SDF,
			"FBBlue SDF"
		},
		{
			FontLoader.TMPFontType.hyk2gjm__outline__SDF,
			"hyk2gjm outline SDF"
		},
		{
			FontLoader.TMPFontType.hyk2gjm__SDF,
			"hyk2gjm SDF"
		},
		{
			FontLoader.TMPFontType.hyk2gjm__shadow__SDF,
			"hyk2gjm shadow SDF"
		},
		{
			FontLoader.TMPFontType.jpchw00u__SDF,
			"jpchw00u SDF"
		},
		{
			FontLoader.TMPFontType.MComicPRC_Medium__SDF,
			"MComicPRC-Medium SDF"
		},
		{
			FontLoader.TMPFontType.YoonBackjaeM__outline__SDF,
			"YoonBackjaeM outline SDF"
		},
		{
			FontLoader.TMPFontType.YoonBackjaeM__SDF,
			"YoonBackjaeM SDF"
		},
		{
			FontLoader.TMPFontType.YoonBackjaeM__shadow__SDF,
			"YoonBackjaeM shadow SDF"
		},
		{
			FontLoader.TMPFontType.YoonBackjaeM__bold__SDF,
			"YoonBackjaeM bold SDF"
		},
		{
			FontLoader.TMPFontType.rounded_mgenplus_1c_meduim__SDF,
			"rounded-mgenplus-1c-meduim SDF"
		},
		{
			FontLoader.TMPFontType.hisikusa_A__SDF,
			"hisikusa-A SDF"
		},
		{
			FontLoader.TMPFontType.FGPotego__2__SDF,
			"FGPotego 2 SDF"
		},
		{
			FontLoader.TMPFontType.FGPotegoBold__2__SDF,
			"FGPotegoBold 2 SDF"
		},
		{
			FontLoader.TMPFontType.FGNewRetro__SDF,
			"FGNewRetro SDF"
		},
		{
			FontLoader.TMPFontType.ElegantHeiseiMinchoMono_9W__SDF,
			"ElegantHeiseiMinchoMono-9W SDF"
		},
		{
			FontLoader.TMPFontType.FGPotegoBold__2__outline__SDF,
			"FGPotegoBold 2 outline SDF"
		}
	};

	// Token: 0x040004F5 RID: 1269
	public static Dictionary<FontLoader.FontType, Font> FontCache = new Dictionary<FontLoader.FontType, Font>();

	// Token: 0x040004F6 RID: 1270
	public static Dictionary<FontLoader.TMPFontType, TMP_FontAsset> TMPFontCache = new Dictionary<FontLoader.TMPFontType, TMP_FontAsset>();

	// Token: 0x040004F7 RID: 1271
	public static Dictionary<string, Material> TMPMaterialCache = new Dictionary<string, Material>();

	// Token: 0x020008CE RID: 2254
	public enum FontType
	{
		// Token: 0x04004359 RID: 17241
		None,
		// Token: 0x0400435A RID: 17242
		CupheadFelix_Regular_merged,
		// Token: 0x0400435B RID: 17243
		CupheadHenriette_A_merged,
		// Token: 0x0400435C RID: 17244
		CupheadMemphis_Medium_merged,
		// Token: 0x0400435D RID: 17245
		CupheadPoster_Regular66Cyr_Lat_English99,
		// Token: 0x0400435E RID: 17246
		CupheadVogue_Bold_merged,
		// Token: 0x0400435F RID: 17247
		CupheadVogue_ExtraBold_merged,
		// Token: 0x04004360 RID: 17248
		DFBrushRDStd_W7,
		// Token: 0x04004361 RID: 17249
		DFBrushSQStd_W5,
		// Token: 0x04004362 RID: 17250
		DSRefinedLetterB,
		// Token: 0x04004363 RID: 17251
		FBBlue,
		// Token: 0x04004364 RID: 17252
		hyk2gjm,
		// Token: 0x04004365 RID: 17253
		jpchw00u,
		// Token: 0x04004366 RID: 17254
		MComicPRC_Medium,
		// Token: 0x04004367 RID: 17255
		YoonBackjaeM,
		// Token: 0x04004368 RID: 17256
		rounded_mgenplus_1c_medium,
		// Token: 0x04004369 RID: 17257
		hisikusa_A,
		// Token: 0x0400436A RID: 17258
		FGPotego__2,
		// Token: 0x0400436B RID: 17259
		FGPotegoBold__2,
		// Token: 0x0400436C RID: 17260
		FGNewRetro,
		// Token: 0x0400436D RID: 17261
		ElegantHeiseiMinchoMono_9W
	}

	// Token: 0x020008CF RID: 2255
	public enum TMPFontType
	{
		// Token: 0x0400436F RID: 17263
		None,
		// Token: 0x04004370 RID: 17264
		CupheadFelix_Regular_merged__SDF,
		// Token: 0x04004371 RID: 17265
		CupheadHenriette_A_merged__SDF,
		// Token: 0x04004372 RID: 17266
		CupheadMemphis_Medium_merged__SDF,
		// Token: 0x04004373 RID: 17267
		CupheadPoster_Regular66Cyr_Lat_English99__SDF,
		// Token: 0x04004374 RID: 17268
		CupheadVogue_Bold_merged__SDF,
		// Token: 0x04004375 RID: 17269
		CupheadVogue_ExtraBold_merged__outline__SDF,
		// Token: 0x04004376 RID: 17270
		CupheadVogue_ExtraBold_merged__SDF,
		// Token: 0x04004377 RID: 17271
		CupheadVogue_ExtraBold_merged__shadow__SDF,
		// Token: 0x04004378 RID: 17272
		DFBrushRDStd_W7__outline__SDF,
		// Token: 0x04004379 RID: 17273
		DFBrushRDStd_W7__SDF,
		// Token: 0x0400437A RID: 17274
		DFBrushRDStd_W7__shadow__SDF,
		// Token: 0x0400437B RID: 17275
		DFBrushSQStd_W5__SDF,
		// Token: 0x0400437C RID: 17276
		DSRefinedLetterB__SDF,
		// Token: 0x0400437D RID: 17277
		FBBlue__SDF,
		// Token: 0x0400437E RID: 17278
		hyk2gjm__outline__SDF,
		// Token: 0x0400437F RID: 17279
		hyk2gjm__SDF,
		// Token: 0x04004380 RID: 17280
		hyk2gjm__shadow__SDF,
		// Token: 0x04004381 RID: 17281
		jpchw00u__SDF,
		// Token: 0x04004382 RID: 17282
		MComicPRC_Medium__SDF,
		// Token: 0x04004383 RID: 17283
		YoonBackjaeM__outline__SDF,
		// Token: 0x04004384 RID: 17284
		YoonBackjaeM__SDF,
		// Token: 0x04004385 RID: 17285
		YoonBackjaeM__shadow__SDF,
		// Token: 0x04004386 RID: 17286
		YoonBackjaeM__bold__SDF,
		// Token: 0x04004387 RID: 17287
		rounded_mgenplus_1c_meduim__SDF,
		// Token: 0x04004388 RID: 17288
		hisikusa_A__SDF,
		// Token: 0x04004389 RID: 17289
		FGPotego__2__SDF,
		// Token: 0x0400438A RID: 17290
		FGPotegoBold__2__SDF,
		// Token: 0x0400438B RID: 17291
		FGNewRetro__SDF,
		// Token: 0x0400438C RID: 17292
		ElegantHeiseiMinchoMono_9W__SDF,
		// Token: 0x0400438D RID: 17293
		FGPotegoBold__2__outline__SDF
	}
}
