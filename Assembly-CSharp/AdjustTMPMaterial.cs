using System;
using TMPro;
using UnityEngine;

// Token: 0x02000466 RID: 1126
public class AdjustTMPMaterial : MonoBehaviour
{
	// Token: 0x06002FE4 RID: 12260 RVA: 0x000E2CC4 File Offset: 0x000E0EC4
	public void Update()
	{
		if (!this.initialSetupComplete || Localization.language != this.previousLanguage)
		{
			this.initialSetupComplete = true;
			this.previousLanguage = Localization.language;
			Localization.Languages language = Localization.language;
			Material material = this.getMaterial(language);
			if (material != null)
			{
				this.text.fontMaterial = material;
			}
		}
	}

	// Token: 0x06002FE5 RID: 12261 RVA: 0x000E2D24 File Offset: 0x000E0F24
	public Material getMaterial(Localization.Languages language)
	{
		foreach (AdjustTMPMaterial.MaterialData materialData in this.materials)
		{
			if (materialData.language == language)
			{
				return FontLoader.GetTMPMaterial(materialData.materialName);
			}
		}
		return this.defaultMaterial;
	}

	// Token: 0x040027A2 RID: 10146
	[SerializeField]
	public TextMeshProUGUI text;

	// Token: 0x040027A3 RID: 10147
	[SerializeField]
	public Material defaultMaterial;

	// Token: 0x040027A4 RID: 10148
	[SerializeField]
	public AdjustTMPMaterial.MaterialData[] materials;

	// Token: 0x040027A5 RID: 10149
	public Localization.Languages previousLanguage;

	// Token: 0x040027A6 RID: 10150
	public bool initialSetupComplete;

	// Token: 0x020010E0 RID: 4320
	[Serializable]
	public struct MaterialData
	{
		// Token: 0x0400778C RID: 30604
		public Localization.Languages language;

		// Token: 0x0400778D RID: 30605
		public string materialName;
	}
}
