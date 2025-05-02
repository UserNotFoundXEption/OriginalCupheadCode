using System;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public class HouseElderKettlePlayerRun : MonoBehaviour
{
	// Token: 0x060035E8 RID: 13800 RVA: 0x0002C276 File Offset: 0x0002A476
	public void Start()
	{
	}

	// Token: 0x060035E9 RID: 13801 RVA: 0x0002C278 File Offset: 0x0002A478
	public void Update()
	{
		base.transform.localPosition += new Vector3(-490f, 0f, 0f) * CupheadTime.FixedDelta;
	}

	// Token: 0x060035EA RID: 13802 RVA: 0x0002C2AE File Offset: 0x0002A4AE
	public void OnRunDust()
	{
		this.runEffect.Create(this.runDustRoot.position);
	}

	// Token: 0x04002BD0 RID: 11216
	[SerializeField]
	public Effect runEffect;

	// Token: 0x04002BD1 RID: 11217
	[SerializeField]
	public Transform runDustRoot;
}
