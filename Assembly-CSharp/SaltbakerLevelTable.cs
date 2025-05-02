using System;
using UnityEngine;

// Token: 0x02000382 RID: 898
public class SaltbakerLevelTable : MonoBehaviour
{
	// Token: 0x060027B4 RID: 10164 RVA: 0x000214EA File Offset: 0x0001F6EA
	public void Start()
	{
		base.GetComponent<MeshRenderer>().sortingOrder = -1;
		this.tableMesh = this.tableMeshFilter.mesh;
		this.vertices = this.tableMesh.vertices;
		this.cam = CupheadLevelCamera.Current;
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x000CC820 File Offset: 0x000CAA20
	public void Update()
	{
		float num = Mathf.InverseLerp(this.cam.Right, this.cam.Left, this.cam.transform.position.x) - 0.5f;
		this.vertices[0].x = -0.5f + num * this.skewFactor;
		this.vertices[2].x = 0.5f + num * this.skewFactor;
		this.tableMesh.vertices = this.vertices;
		this.tableMesh.RecalculateBounds();
	}

	// Token: 0x040020E6 RID: 8422
	[SerializeField]
	public float skewFactor = 0.02f;

	// Token: 0x040020E7 RID: 8423
	[SerializeField]
	public MeshFilter tableMeshFilter;

	// Token: 0x040020E8 RID: 8424
	public Mesh tableMesh;

	// Token: 0x040020E9 RID: 8425
	public Vector3[] vertices;

	// Token: 0x040020EA RID: 8426
	public CupheadLevelCamera cam;
}
