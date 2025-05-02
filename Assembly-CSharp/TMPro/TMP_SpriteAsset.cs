using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200066F RID: 1647
	public class TMP_SpriteAsset : TMP_Asset
	{
		// Token: 0x17000649 RID: 1609
		// (get) Token: 0x060045D0 RID: 17872 RVA: 0x00037778 File Offset: 0x00035978
		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				if (TMP_SpriteAsset.m_defaultSpriteAsset == null)
				{
					TMP_SpriteAsset.m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
				}
				return TMP_SpriteAsset.m_defaultSpriteAsset;
			}
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x0003779E File Offset: 0x0003599E
		public void OnEnable()
		{
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x0014EC58 File Offset: 0x0014CE58
		public Material GetDefaultSpriteMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			Shader shader = Shader.Find("TMPro/Sprite");
			Material material = new Material(shader);
			material.SetTexture(ShaderUtilities.ID_MainTex, this.spriteSheet);
			material.hideFlags = 1;
			return material;
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x0014EC98 File Offset: 0x0014CE98
		public int GetSpriteIndex(int hashCode)
		{
			for (int i = 0; i < this.spriteInfoList.Count; i++)
			{
				if (this.spriteInfoList[i].hashCode == hashCode)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x040035C6 RID: 13766
		public static TMP_SpriteAsset m_defaultSpriteAsset;

		// Token: 0x040035C7 RID: 13767
		public Texture spriteSheet;

		// Token: 0x040035C8 RID: 13768
		public List<TMP_Sprite> spriteInfoList;

		// Token: 0x040035C9 RID: 13769
		public List<Sprite> m_sprites;
	}
}
