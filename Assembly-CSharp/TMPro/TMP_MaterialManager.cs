using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x0200066B RID: 1643
	public static class TMP_MaterialManager
	{
		// Token: 0x060045AC RID: 17836 RVA: 0x0014E5E0 File Offset: 0x0014C7E0
		public static Material GetStencilMaterial(Material baseMaterial, int stencilID)
		{
			if (!baseMaterial.HasProperty(ShaderUtilities.ID_StencilID))
			{
				return baseMaterial;
			}
			int instanceID = baseMaterial.GetInstanceID();
			for (int i = 0; i < TMP_MaterialManager.m_materialList.Count; i++)
			{
				if (TMP_MaterialManager.m_materialList[i].baseMaterial.GetInstanceID() == instanceID && TMP_MaterialManager.m_materialList[i].stencilID == stencilID)
				{
					TMP_MaterialManager.m_materialList[i].count++;
					return TMP_MaterialManager.m_materialList[i].stencilMaterial;
				}
			}
			Material material = new Material(baseMaterial);
			material.hideFlags = 61;
			Material material2 = material;
			material2.name = material2.name + " Masking ID:" + stencilID;
			material.shaderKeywords = baseMaterial.shaderKeywords;
			ShaderUtilities.GetShaderPropertyIDs();
			material.SetFloat(ShaderUtilities.ID_StencilID, (float)stencilID);
			material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
			TMP_MaterialManager.MaskingMaterial maskingMaterial = new TMP_MaterialManager.MaskingMaterial();
			maskingMaterial.baseMaterial = baseMaterial;
			maskingMaterial.stencilMaterial = material;
			maskingMaterial.stencilID = stencilID;
			maskingMaterial.count = 1;
			TMP_MaterialManager.m_materialList.Add(maskingMaterial);
			return material;
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x0014E704 File Offset: 0x0014C904
		public static void ReleaseStencilMaterial(Material stencilMaterial)
		{
			int instanceID = stencilMaterial.GetInstanceID();
			for (int i = 0; i < TMP_MaterialManager.m_materialList.Count; i++)
			{
				if (TMP_MaterialManager.m_materialList[i].stencilMaterial.GetInstanceID() == instanceID)
				{
					if (TMP_MaterialManager.m_materialList[i].count > 1)
					{
						TMP_MaterialManager.m_materialList[i].count--;
					}
					else
					{
						Object.DestroyImmediate(TMP_MaterialManager.m_materialList[i].stencilMaterial);
						TMP_MaterialManager.m_materialList.RemoveAt(i);
						stencilMaterial = null;
					}
					break;
				}
			}
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x0014E7AC File Offset: 0x0014C9AC
		public static Material GetBaseMaterial(Material stencilMaterial)
		{
			int num = TMP_MaterialManager.m_materialList.FindIndex((TMP_MaterialManager.MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num == -1)
			{
				return null;
			}
			return TMP_MaterialManager.m_materialList[num].baseMaterial;
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x000375B7 File Offset: 0x000357B7
		public static Material SetStencil(Material material, int stencilID)
		{
			material.SetFloat(ShaderUtilities.ID_StencilID, (float)stencilID);
			if (stencilID == 0)
			{
				material.SetFloat(ShaderUtilities.ID_StencilComp, 8f);
			}
			else
			{
				material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
			}
			return material;
		}

		// Token: 0x060045B0 RID: 17840 RVA: 0x0014E7F8 File Offset: 0x0014C9F8
		public static void AddMaskingMaterial(Material baseMaterial, Material stencilMaterial, int stencilID)
		{
			int num = TMP_MaterialManager.m_materialList.FindIndex((TMP_MaterialManager.MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num == -1)
			{
				TMP_MaterialManager.MaskingMaterial maskingMaterial = new TMP_MaterialManager.MaskingMaterial();
				maskingMaterial.baseMaterial = baseMaterial;
				maskingMaterial.stencilMaterial = stencilMaterial;
				maskingMaterial.stencilID = stencilID;
				maskingMaterial.count = 1;
				TMP_MaterialManager.m_materialList.Add(maskingMaterial);
			}
			else
			{
				stencilMaterial = TMP_MaterialManager.m_materialList[num].stencilMaterial;
				TMP_MaterialManager.m_materialList[num].count++;
			}
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x0014E898 File Offset: 0x0014CA98
		public static void RemoveStencilMaterial(Material stencilMaterial)
		{
			int num = TMP_MaterialManager.m_materialList.FindIndex((TMP_MaterialManager.MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num != -1)
			{
				TMP_MaterialManager.m_materialList.RemoveAt(num);
			}
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x0014E8DC File Offset: 0x0014CADC
		public static void ReleaseBaseMaterial(Material baseMaterial)
		{
			int num = TMP_MaterialManager.m_materialList.FindIndex((TMP_MaterialManager.MaskingMaterial item) => item.baseMaterial == baseMaterial);
			if (num != -1)
			{
				if (TMP_MaterialManager.m_materialList[num].count > 1)
				{
					TMP_MaterialManager.m_materialList[num].count--;
				}
				else
				{
					Object.DestroyImmediate(TMP_MaterialManager.m_materialList[num].stencilMaterial);
					TMP_MaterialManager.m_materialList.RemoveAt(num);
				}
			}
		}

		// Token: 0x060045B3 RID: 17843 RVA: 0x0014E96C File Offset: 0x0014CB6C
		public static void ClearMaterials()
		{
			if (TMP_MaterialManager.m_materialList.Count<TMP_MaterialManager.MaskingMaterial>() == 0)
			{
				return;
			}
			for (int i = 0; i < TMP_MaterialManager.m_materialList.Count<TMP_MaterialManager.MaskingMaterial>(); i++)
			{
				Material stencilMaterial = TMP_MaterialManager.m_materialList[i].stencilMaterial;
				Object.DestroyImmediate(stencilMaterial);
				TMP_MaterialManager.m_materialList.RemoveAt(i);
			}
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x0014E9C8 File Offset: 0x0014CBC8
		public static int GetStencilID(GameObject obj)
		{
			int num = 0;
			List<Mask> list = TMP_ListPool<Mask>.Get();
			obj.GetComponentsInParent<Mask>(false, list);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].MaskEnabled())
				{
					num++;
				}
			}
			TMP_ListPool<Mask>.Release(list);
			return Mathf.Min((1 << num) - 1, 255);
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x0014EA2C File Offset: 0x0014CC2C
		public static Material GetFallbackMaterial(Material source, Texture mainTex)
		{
			int instanceID = source.GetInstanceID();
			int instanceID2 = mainTex.GetInstanceID();
			for (int i = 0; i < TMP_MaterialManager.m_fallbackMaterialList.Count; i++)
			{
				if (TMP_MaterialManager.m_fallbackMaterialList[i].fallbackMaterial == null)
				{
					TMP_MaterialManager.m_fallbackMaterialList.RemoveAt(i);
				}
				else if (TMP_MaterialManager.m_fallbackMaterialList[i].baseMaterial.GetInstanceID() == instanceID && TMP_MaterialManager.m_fallbackMaterialList[i].fallbackMaterial.mainTexture.GetInstanceID() == instanceID2)
				{
					TMP_MaterialManager.m_fallbackMaterialList[i].count++;
					return TMP_MaterialManager.m_fallbackMaterialList[i].fallbackMaterial;
				}
			}
			Material material = new Material(source);
			Material material2 = material;
			material2.name += " (Fallback Instance)";
			material.mainTexture = mainTex;
			TMP_MaterialManager.FallbackMaterial fallbackMaterial = new TMP_MaterialManager.FallbackMaterial();
			fallbackMaterial.baseID = instanceID;
			fallbackMaterial.baseMaterial = source;
			fallbackMaterial.fallbackMaterial = material;
			fallbackMaterial.count = 1;
			TMP_MaterialManager.m_fallbackMaterialList.Add(fallbackMaterial);
			return material;
		}

		// Token: 0x040035B1 RID: 13745
		public static List<TMP_MaterialManager.MaskingMaterial> m_materialList = new List<TMP_MaterialManager.MaskingMaterial>();

		// Token: 0x040035B2 RID: 13746
		public static List<TMP_MaterialManager.FallbackMaterial> m_fallbackMaterialList = new List<TMP_MaterialManager.FallbackMaterial>();

		// Token: 0x020012F5 RID: 4853
		public class FallbackMaterial
		{
			// Token: 0x040081EA RID: 33258
			public int baseID;

			// Token: 0x040081EB RID: 33259
			public Material baseMaterial;

			// Token: 0x040081EC RID: 33260
			public Material fallbackMaterial;

			// Token: 0x040081ED RID: 33261
			public int count;
		}

		// Token: 0x020012F6 RID: 4854
		public class MaskingMaterial
		{
			// Token: 0x040081EE RID: 33262
			public Material baseMaterial;

			// Token: 0x040081EF RID: 33263
			public Material stencilMaterial;

			// Token: 0x040081F0 RID: 33264
			public int count;

			// Token: 0x040081F1 RID: 33265
			public int stencilID;
		}
	}
}
