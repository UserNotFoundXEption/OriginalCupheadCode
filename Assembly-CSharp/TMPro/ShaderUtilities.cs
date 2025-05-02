using System;
using System.Linq;
using UnityEngine;

namespace TMPro
{
	// Token: 0x020006A5 RID: 1701
	public static class ShaderUtilities
	{
		// Token: 0x06004768 RID: 18280 RVA: 0x0015A5AC File Offset: 0x001587AC
		public static void GetShaderPropertyIDs()
		{
			if (!ShaderUtilities.isInitialized)
			{
				ShaderUtilities.isInitialized = true;
				ShaderUtilities.ID_MainTex = Shader.PropertyToID("_MainTex");
				ShaderUtilities.ID_FaceTex = Shader.PropertyToID("_FaceTex");
				ShaderUtilities.ID_FaceColor = Shader.PropertyToID("_FaceColor");
				ShaderUtilities.ID_FaceDilate = Shader.PropertyToID("_FaceDilate");
				ShaderUtilities.ID_Shininess = Shader.PropertyToID("_FaceShininess");
				ShaderUtilities.ID_UnderlayColor = Shader.PropertyToID("_UnderlayColor");
				ShaderUtilities.ID_UnderlayOffsetX = Shader.PropertyToID("_UnderlayOffsetX");
				ShaderUtilities.ID_UnderlayOffsetY = Shader.PropertyToID("_UnderlayOffsetY");
				ShaderUtilities.ID_UnderlayDilate = Shader.PropertyToID("_UnderlayDilate");
				ShaderUtilities.ID_UnderlaySoftness = Shader.PropertyToID("_UnderlaySoftness");
				ShaderUtilities.ID_WeightNormal = Shader.PropertyToID("_WeightNormal");
				ShaderUtilities.ID_WeightBold = Shader.PropertyToID("_WeightBold");
				ShaderUtilities.ID_OutlineTex = Shader.PropertyToID("_OutlineTex");
				ShaderUtilities.ID_OutlineWidth = Shader.PropertyToID("_OutlineWidth");
				ShaderUtilities.ID_OutlineSoftness = Shader.PropertyToID("_OutlineSoftness");
				ShaderUtilities.ID_OutlineColor = Shader.PropertyToID("_OutlineColor");
				ShaderUtilities.ID_GradientScale = Shader.PropertyToID("_GradientScale");
				ShaderUtilities.ID_ScaleX = Shader.PropertyToID("_ScaleX");
				ShaderUtilities.ID_ScaleY = Shader.PropertyToID("_ScaleY");
				ShaderUtilities.ID_PerspectiveFilter = Shader.PropertyToID("_PerspectiveFilter");
				ShaderUtilities.ID_TextureWidth = Shader.PropertyToID("_TextureWidth");
				ShaderUtilities.ID_TextureHeight = Shader.PropertyToID("_TextureHeight");
				ShaderUtilities.ID_BevelAmount = Shader.PropertyToID("_Bevel");
				ShaderUtilities.ID_LightAngle = Shader.PropertyToID("_LightAngle");
				ShaderUtilities.ID_EnvMap = Shader.PropertyToID("_Cube");
				ShaderUtilities.ID_EnvMatrix = Shader.PropertyToID("_EnvMatrix");
				ShaderUtilities.ID_EnvMatrixRotation = Shader.PropertyToID("_EnvMatrixRotation");
				ShaderUtilities.ID_GlowColor = Shader.PropertyToID("_GlowColor");
				ShaderUtilities.ID_GlowOffset = Shader.PropertyToID("_GlowOffset");
				ShaderUtilities.ID_GlowPower = Shader.PropertyToID("_GlowPower");
				ShaderUtilities.ID_GlowOuter = Shader.PropertyToID("_GlowOuter");
				ShaderUtilities.ID_MaskCoord = Shader.PropertyToID("_MaskCoord");
				ShaderUtilities.ID_ClipRect = Shader.PropertyToID("_ClipRect");
				ShaderUtilities.ID_UseClipRect = Shader.PropertyToID("_UseClipRect");
				ShaderUtilities.ID_MaskSoftnessX = Shader.PropertyToID("_MaskSoftnessX");
				ShaderUtilities.ID_MaskSoftnessY = Shader.PropertyToID("_MaskSoftnessY");
				ShaderUtilities.ID_VertexOffsetX = Shader.PropertyToID("_VertexOffsetX");
				ShaderUtilities.ID_VertexOffsetY = Shader.PropertyToID("_VertexOffsetY");
				ShaderUtilities.ID_StencilID = Shader.PropertyToID("_Stencil");
				ShaderUtilities.ID_StencilOp = Shader.PropertyToID("_StencilOp");
				ShaderUtilities.ID_StencilComp = Shader.PropertyToID("_StencilComp");
				ShaderUtilities.ID_StencilReadMask = Shader.PropertyToID("_StencilReadMask");
				ShaderUtilities.ID_StencilWriteMask = Shader.PropertyToID("_StencilWriteMask");
				ShaderUtilities.ID_ShaderFlags = Shader.PropertyToID("_ShaderFlags");
				ShaderUtilities.ID_ScaleRatio_A = Shader.PropertyToID("_ScaleRatioA");
				ShaderUtilities.ID_ScaleRatio_B = Shader.PropertyToID("_ScaleRatioB");
				ShaderUtilities.ID_ScaleRatio_C = Shader.PropertyToID("_ScaleRatioC");
			}
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x0015A88C File Offset: 0x00158A8C
		public static void UpdateShaderRatios(Material mat, bool isBold)
		{
			bool flag = !mat.shaderKeywords.Contains(ShaderUtilities.Keyword_Ratios);
			float @float = mat.GetFloat(ShaderUtilities.ID_GradientScale);
			float float2 = mat.GetFloat(ShaderUtilities.ID_FaceDilate);
			float float3 = mat.GetFloat(ShaderUtilities.ID_OutlineWidth);
			float float4 = mat.GetFloat(ShaderUtilities.ID_OutlineSoftness);
			float num = isBold ? (mat.GetFloat(ShaderUtilities.ID_WeightBold) * 2f / @float) : (mat.GetFloat(ShaderUtilities.ID_WeightNormal) * 2f / @float);
			float num2 = Mathf.Max(1f, num + float2 + float3 + float4);
			float num3 = (!flag) ? 1f : ((@float - ShaderUtilities.m_clamp) / (@float * num2));
			mat.SetFloat(ShaderUtilities.ID_ScaleRatio_A, num3);
			if (mat.HasProperty(ShaderUtilities.ID_GlowOffset))
			{
				float float5 = mat.GetFloat(ShaderUtilities.ID_GlowOffset);
				float float6 = mat.GetFloat(ShaderUtilities.ID_GlowOuter);
				float num4 = (num + float2) * (@float - ShaderUtilities.m_clamp);
				num2 = Mathf.Max(1f, float5 + float6);
				float num5 = (!flag) ? 1f : (Mathf.Max(0f, @float - ShaderUtilities.m_clamp - num4) / (@float * num2));
				mat.SetFloat(ShaderUtilities.ID_ScaleRatio_B, num5);
			}
			if (mat.HasProperty(ShaderUtilities.ID_UnderlayOffsetX))
			{
				float float7 = mat.GetFloat(ShaderUtilities.ID_UnderlayOffsetX);
				float float8 = mat.GetFloat(ShaderUtilities.ID_UnderlayOffsetY);
				float float9 = mat.GetFloat(ShaderUtilities.ID_UnderlayDilate);
				float float10 = mat.GetFloat(ShaderUtilities.ID_UnderlaySoftness);
				float num6 = (num + float2) * (@float - ShaderUtilities.m_clamp);
				num2 = Mathf.Max(1f, Mathf.Max(Mathf.Abs(float7), Mathf.Abs(float8)) + float9 + float10);
				float num7 = (!flag) ? 1f : (Mathf.Max(0f, @float - ShaderUtilities.m_clamp - num6) / (@float * num2));
				mat.SetFloat(ShaderUtilities.ID_ScaleRatio_C, num7);
			}
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x00038C14 File Offset: 0x00036E14
		public static Vector4 GetFontExtent(Material material)
		{
			return Vector4.zero;
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x0015AAA0 File Offset: 0x00158CA0
		public static bool IsMaskingEnabled(Material material)
		{
			return !(material == null) && material.HasProperty(ShaderUtilities.ID_ClipRect) && (material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_SOFT) || material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_HARD) || material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_TEX));
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x0015AB10 File Offset: 0x00158D10
		public static float GetPadding(Material material, bool enableExtraPadding, bool isBold)
		{
			if (!ShaderUtilities.isInitialized)
			{
				ShaderUtilities.GetShaderPropertyIDs();
			}
			if (material == null)
			{
				return 0f;
			}
			int num = (!enableExtraPadding) ? 0 : 4;
			if (!material.HasProperty(ShaderUtilities.ID_GradientScale))
			{
				return (float)num;
			}
			Vector4 vector = Vector4.zero;
			Vector4 zero = Vector4.zero;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			ShaderUtilities.UpdateShaderRatios(material, isBold);
			string[] shaderKeywords = material.shaderKeywords;
			if (material.HasProperty(ShaderUtilities.ID_ScaleRatio_A))
			{
				num5 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
			}
			if (material.HasProperty(ShaderUtilities.ID_FaceDilate))
			{
				num2 = material.GetFloat(ShaderUtilities.ID_FaceDilate) * num5;
			}
			if (material.HasProperty(ShaderUtilities.ID_OutlineSoftness))
			{
				num3 = material.GetFloat(ShaderUtilities.ID_OutlineSoftness) * num5;
			}
			if (material.HasProperty(ShaderUtilities.ID_OutlineWidth))
			{
				num4 = material.GetFloat(ShaderUtilities.ID_OutlineWidth) * num5;
			}
			float num10 = num4 + num3 + num2;
			if (material.HasProperty(ShaderUtilities.ID_GlowOffset) && shaderKeywords.Contains(ShaderUtilities.Keyword_Glow))
			{
				if (material.HasProperty(ShaderUtilities.ID_ScaleRatio_B))
				{
					num6 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_B);
				}
				num8 = material.GetFloat(ShaderUtilities.ID_GlowOffset) * num6;
				num9 = material.GetFloat(ShaderUtilities.ID_GlowOuter) * num6;
			}
			num10 = Mathf.Max(num10, num2 + num8 + num9);
			if (material.HasProperty(ShaderUtilities.ID_UnderlaySoftness) && shaderKeywords.Contains(ShaderUtilities.Keyword_Underlay))
			{
				if (material.HasProperty(ShaderUtilities.ID_ScaleRatio_C))
				{
					num7 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_C);
				}
				float num11 = material.GetFloat(ShaderUtilities.ID_UnderlayOffsetX) * num7;
				float num12 = material.GetFloat(ShaderUtilities.ID_UnderlayOffsetY) * num7;
				float num13 = material.GetFloat(ShaderUtilities.ID_UnderlayDilate) * num7;
				float num14 = material.GetFloat(ShaderUtilities.ID_UnderlaySoftness) * num7;
				vector.x = Mathf.Max(vector.x, num2 + num13 + num14 - num11);
				vector.y = Mathf.Max(vector.y, num2 + num13 + num14 - num12);
				vector.z = Mathf.Max(vector.z, num2 + num13 + num14 + num11);
				vector.w = Mathf.Max(vector.w, num2 + num13 + num14 + num12);
			}
			vector.x = Mathf.Max(vector.x, num10);
			vector.y = Mathf.Max(vector.y, num10);
			vector.z = Mathf.Max(vector.z, num10);
			vector.w = Mathf.Max(vector.w, num10);
			vector.x += (float)num;
			vector.y += (float)num;
			vector.z += (float)num;
			vector.w += (float)num;
			vector.x = Mathf.Min(vector.x, 1f);
			vector.y = Mathf.Min(vector.y, 1f);
			vector.z = Mathf.Min(vector.z, 1f);
			vector.w = Mathf.Min(vector.w, 1f);
			zero.x = ((zero.x >= vector.x) ? zero.x : vector.x);
			zero.y = ((zero.y >= vector.y) ? zero.y : vector.y);
			zero.z = ((zero.z >= vector.z) ? zero.z : vector.z);
			zero.w = ((zero.w >= vector.w) ? zero.w : vector.w);
			float @float = material.GetFloat(ShaderUtilities.ID_GradientScale);
			vector *= @float;
			num10 = Mathf.Max(vector.x, vector.y);
			num10 = Mathf.Max(vector.z, num10);
			num10 = Mathf.Max(vector.w, num10);
			return num10 + 0.5f;
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x0015AF98 File Offset: 0x00159198
		public static float GetPadding(Material[] materials, bool enableExtraPadding, bool isBold)
		{
			if (!ShaderUtilities.isInitialized)
			{
				ShaderUtilities.GetShaderPropertyIDs();
			}
			if (materials == null)
			{
				return 0f;
			}
			int num = (!enableExtraPadding) ? 0 : 4;
			if (!materials[0].HasProperty(ShaderUtilities.ID_GradientScale))
			{
				return (float)num;
			}
			Vector4 vector = Vector4.zero;
			Vector4 zero = Vector4.zero;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			float num10;
			for (int i = 0; i < materials.Length; i++)
			{
				ShaderUtilities.UpdateShaderRatios(materials[i], isBold);
				string[] shaderKeywords = materials[i].shaderKeywords;
				if (materials[i].HasProperty(ShaderUtilities.ID_ScaleRatio_A))
				{
					num5 = materials[i].GetFloat(ShaderUtilities.ID_ScaleRatio_A);
				}
				if (materials[i].HasProperty(ShaderUtilities.ID_FaceDilate))
				{
					num2 = materials[i].GetFloat(ShaderUtilities.ID_FaceDilate) * num5;
				}
				if (materials[i].HasProperty(ShaderUtilities.ID_OutlineSoftness))
				{
					num3 = materials[i].GetFloat(ShaderUtilities.ID_OutlineSoftness) * num5;
				}
				if (materials[i].HasProperty(ShaderUtilities.ID_OutlineWidth))
				{
					num4 = materials[i].GetFloat(ShaderUtilities.ID_OutlineWidth) * num5;
				}
				num10 = num4 + num3 + num2;
				if (materials[i].HasProperty(ShaderUtilities.ID_GlowOffset) && shaderKeywords.Contains(ShaderUtilities.Keyword_Glow))
				{
					if (materials[i].HasProperty(ShaderUtilities.ID_ScaleRatio_B))
					{
						num6 = materials[i].GetFloat(ShaderUtilities.ID_ScaleRatio_B);
					}
					num8 = materials[i].GetFloat(ShaderUtilities.ID_GlowOffset) * num6;
					num9 = materials[i].GetFloat(ShaderUtilities.ID_GlowOuter) * num6;
				}
				num10 = Mathf.Max(num10, num2 + num8 + num9);
				if (materials[i].HasProperty(ShaderUtilities.ID_UnderlaySoftness) && shaderKeywords.Contains(ShaderUtilities.Keyword_Underlay))
				{
					if (materials[i].HasProperty(ShaderUtilities.ID_ScaleRatio_C))
					{
						num7 = materials[i].GetFloat(ShaderUtilities.ID_ScaleRatio_C);
					}
					float num11 = materials[i].GetFloat(ShaderUtilities.ID_UnderlayOffsetX) * num7;
					float num12 = materials[i].GetFloat(ShaderUtilities.ID_UnderlayOffsetY) * num7;
					float num13 = materials[i].GetFloat(ShaderUtilities.ID_UnderlayDilate) * num7;
					float num14 = materials[i].GetFloat(ShaderUtilities.ID_UnderlaySoftness) * num7;
					vector.x = Mathf.Max(vector.x, num2 + num13 + num14 - num11);
					vector.y = Mathf.Max(vector.y, num2 + num13 + num14 - num12);
					vector.z = Mathf.Max(vector.z, num2 + num13 + num14 + num11);
					vector.w = Mathf.Max(vector.w, num2 + num13 + num14 + num12);
				}
				vector.x = Mathf.Max(vector.x, num10);
				vector.y = Mathf.Max(vector.y, num10);
				vector.z = Mathf.Max(vector.z, num10);
				vector.w = Mathf.Max(vector.w, num10);
				vector.x += (float)num;
				vector.y += (float)num;
				vector.z += (float)num;
				vector.w += (float)num;
				vector.x = Mathf.Min(vector.x, 1f);
				vector.y = Mathf.Min(vector.y, 1f);
				vector.z = Mathf.Min(vector.z, 1f);
				vector.w = Mathf.Min(vector.w, 1f);
				zero.x = ((zero.x >= vector.x) ? zero.x : vector.x);
				zero.y = ((zero.y >= vector.y) ? zero.y : vector.y);
				zero.z = ((zero.z >= vector.z) ? zero.z : vector.z);
				zero.w = ((zero.w >= vector.w) ? zero.w : vector.w);
			}
			float @float = materials[0].GetFloat(ShaderUtilities.ID_GradientScale);
			vector *= @float;
			num10 = Mathf.Max(vector.x, vector.y);
			num10 = Mathf.Max(vector.z, num10);
			num10 = Mathf.Max(vector.w, num10);
			return num10 + 0.25f;
		}

		// Token: 0x040037E6 RID: 14310
		public static int ID_MainTex;

		// Token: 0x040037E7 RID: 14311
		public static int ID_FaceTex;

		// Token: 0x040037E8 RID: 14312
		public static int ID_FaceColor;

		// Token: 0x040037E9 RID: 14313
		public static int ID_FaceDilate;

		// Token: 0x040037EA RID: 14314
		public static int ID_Shininess;

		// Token: 0x040037EB RID: 14315
		public static int ID_UnderlayColor;

		// Token: 0x040037EC RID: 14316
		public static int ID_UnderlayOffsetX;

		// Token: 0x040037ED RID: 14317
		public static int ID_UnderlayOffsetY;

		// Token: 0x040037EE RID: 14318
		public static int ID_UnderlayDilate;

		// Token: 0x040037EF RID: 14319
		public static int ID_UnderlaySoftness;

		// Token: 0x040037F0 RID: 14320
		public static int ID_WeightNormal;

		// Token: 0x040037F1 RID: 14321
		public static int ID_WeightBold;

		// Token: 0x040037F2 RID: 14322
		public static int ID_OutlineTex;

		// Token: 0x040037F3 RID: 14323
		public static int ID_OutlineWidth;

		// Token: 0x040037F4 RID: 14324
		public static int ID_OutlineSoftness;

		// Token: 0x040037F5 RID: 14325
		public static int ID_OutlineColor;

		// Token: 0x040037F6 RID: 14326
		public static int ID_GradientScale;

		// Token: 0x040037F7 RID: 14327
		public static int ID_ScaleX;

		// Token: 0x040037F8 RID: 14328
		public static int ID_ScaleY;

		// Token: 0x040037F9 RID: 14329
		public static int ID_PerspectiveFilter;

		// Token: 0x040037FA RID: 14330
		public static int ID_TextureWidth;

		// Token: 0x040037FB RID: 14331
		public static int ID_TextureHeight;

		// Token: 0x040037FC RID: 14332
		public static int ID_BevelAmount;

		// Token: 0x040037FD RID: 14333
		public static int ID_GlowColor;

		// Token: 0x040037FE RID: 14334
		public static int ID_GlowOffset;

		// Token: 0x040037FF RID: 14335
		public static int ID_GlowPower;

		// Token: 0x04003800 RID: 14336
		public static int ID_GlowOuter;

		// Token: 0x04003801 RID: 14337
		public static int ID_LightAngle;

		// Token: 0x04003802 RID: 14338
		public static int ID_EnvMap;

		// Token: 0x04003803 RID: 14339
		public static int ID_EnvMatrix;

		// Token: 0x04003804 RID: 14340
		public static int ID_EnvMatrixRotation;

		// Token: 0x04003805 RID: 14341
		public static int ID_MaskCoord;

		// Token: 0x04003806 RID: 14342
		public static int ID_ClipRect;

		// Token: 0x04003807 RID: 14343
		public static int ID_MaskSoftnessX;

		// Token: 0x04003808 RID: 14344
		public static int ID_MaskSoftnessY;

		// Token: 0x04003809 RID: 14345
		public static int ID_VertexOffsetX;

		// Token: 0x0400380A RID: 14346
		public static int ID_VertexOffsetY;

		// Token: 0x0400380B RID: 14347
		public static int ID_UseClipRect;

		// Token: 0x0400380C RID: 14348
		public static int ID_StencilID;

		// Token: 0x0400380D RID: 14349
		public static int ID_StencilOp;

		// Token: 0x0400380E RID: 14350
		public static int ID_StencilComp;

		// Token: 0x0400380F RID: 14351
		public static int ID_StencilReadMask;

		// Token: 0x04003810 RID: 14352
		public static int ID_StencilWriteMask;

		// Token: 0x04003811 RID: 14353
		public static int ID_ShaderFlags;

		// Token: 0x04003812 RID: 14354
		public static int ID_ScaleRatio_A;

		// Token: 0x04003813 RID: 14355
		public static int ID_ScaleRatio_B;

		// Token: 0x04003814 RID: 14356
		public static int ID_ScaleRatio_C;

		// Token: 0x04003815 RID: 14357
		public static string Keyword_Bevel = "BEVEL_ON";

		// Token: 0x04003816 RID: 14358
		public static string Keyword_Glow = "GLOW_ON";

		// Token: 0x04003817 RID: 14359
		public static string Keyword_Underlay = "UNDERLAY_ON";

		// Token: 0x04003818 RID: 14360
		public static string Keyword_Ratios = "RATIOS_OFF";

		// Token: 0x04003819 RID: 14361
		public static string Keyword_MASK_SOFT = "MASK_SOFT";

		// Token: 0x0400381A RID: 14362
		public static string Keyword_MASK_HARD = "MASK_HARD";

		// Token: 0x0400381B RID: 14363
		public static string Keyword_MASK_TEX = "MASK_TEX";

		// Token: 0x0400381C RID: 14364
		public static string ShaderTag_ZTestMode = "unity_GUIZTestMode";

		// Token: 0x0400381D RID: 14365
		public static string ShaderTag_CullMode = "_CullMode";

		// Token: 0x0400381E RID: 14366
		public static float m_clamp = 1f;

		// Token: 0x0400381F RID: 14367
		public static bool isInitialized;
	}
}
