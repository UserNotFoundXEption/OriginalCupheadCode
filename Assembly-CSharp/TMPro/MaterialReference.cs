using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000661 RID: 1633
	public struct MaterialReference
	{
		// Token: 0x060044E3 RID: 17635 RVA: 0x0013DBA8 File Offset: 0x0013BDA8
		public MaterialReference(int index, TMP_FontAsset fontAsset, TMP_SpriteAsset spriteAsset, Material material, float padding)
		{
			this.index = index;
			this.fontAsset = fontAsset;
			this.spriteAsset = spriteAsset;
			this.material = material;
			this.isDefaultMaterial = (material.GetInstanceID() == fontAsset.material.GetInstanceID());
			this.isFallbackFont = false;
			this.padding = padding;
			this.referenceCount = 0;
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x0013DC0C File Offset: 0x0013BE0C
		public static bool Contains(MaterialReference[] materialReferences, TMP_FontAsset fontAsset)
		{
			int instanceID = fontAsset.GetInstanceID();
			int num = 0;
			while (num < materialReferences.Length && materialReferences[num].fontAsset != null)
			{
				if (materialReferences[num].fontAsset.GetInstanceID() == instanceID)
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x0013DC68 File Offset: 0x0013BE68
		public static int AddMaterialReference(Material material, TMP_FontAsset fontAsset, MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int num = 0;
			if (materialReferenceIndexLookup.TryGetValue(instanceID, out num))
			{
				return num;
			}
			num = materialReferenceIndexLookup.Count;
			materialReferenceIndexLookup[instanceID] = num;
			materialReferences[num].index = num;
			materialReferences[num].fontAsset = fontAsset;
			materialReferences[num].spriteAsset = null;
			materialReferences[num].material = material;
			materialReferences[num].isDefaultMaterial = (instanceID == fontAsset.material.GetInstanceID());
			materialReferences[num].referenceCount = 0;
			return num;
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x0013DD04 File Offset: 0x0013BF04
		public static int AddMaterialReference(Material material, TMP_SpriteAsset spriteAsset, MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int num = 0;
			if (materialReferenceIndexLookup.TryGetValue(instanceID, out num))
			{
				return num;
			}
			num = materialReferenceIndexLookup.Count;
			materialReferenceIndexLookup[instanceID] = num;
			materialReferences[num].index = num;
			materialReferences[num].fontAsset = materialReferences[0].fontAsset;
			materialReferences[num].spriteAsset = spriteAsset;
			materialReferences[num].material = material;
			materialReferences[num].isDefaultMaterial = true;
			materialReferences[num].referenceCount = 0;
			return num;
		}

		// Token: 0x04003543 RID: 13635
		public int index;

		// Token: 0x04003544 RID: 13636
		public TMP_FontAsset fontAsset;

		// Token: 0x04003545 RID: 13637
		public TMP_SpriteAsset spriteAsset;

		// Token: 0x04003546 RID: 13638
		public Material material;

		// Token: 0x04003547 RID: 13639
		public bool isDefaultMaterial;

		// Token: 0x04003548 RID: 13640
		public bool isFallbackFont;

		// Token: 0x04003549 RID: 13641
		public float padding;

		// Token: 0x0400354A RID: 13642
		public int referenceCount;
	}
}
