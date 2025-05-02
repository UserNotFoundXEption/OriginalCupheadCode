using System;
using UnityEngine;

// Token: 0x020005BB RID: 1467
[ExecuteInEditMode]
public sealed class SortingLayerExposed : MonoBehaviour
{
	// Token: 0x06003D91 RID: 15761 RVA: 0x00031A50 File Offset: 0x0002FC50
	public void OnValidate()
	{
		this.Apply();
	}

	// Token: 0x06003D92 RID: 15762 RVA: 0x00031A58 File Offset: 0x0002FC58
	public void OnEnable()
	{
		this.Apply();
	}

	// Token: 0x06003D93 RID: 15763 RVA: 0x001192BC File Offset: 0x001174BC
	public void Apply()
	{
		MeshRenderer component = base.gameObject.GetComponent<MeshRenderer>();
		component.sortingLayerName = this.sortingLayerName;
		component.sortingOrder = this.sortingOrder;
	}

	// Token: 0x040030EE RID: 12526
	[SerializeField]
	public string sortingLayerName = "Default";

	// Token: 0x040030EF RID: 12527
	[SerializeField]
	public int sortingOrder;
}
