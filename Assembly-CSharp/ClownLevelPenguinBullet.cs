using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class ClownLevelPenguinBullet : BasicProjectile
{
	// Token: 0x06001429 RID: 5161 RVA: 0x00010FA9 File Offset: 0x0000F1A9
	public override void Start()
	{
		base.Start();
		this.move = false;
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x00099954 File Offset: 0x00097B54
	public IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		this.move = true;
		this.bulletFX.Create(this.root.transform.position).transform.SetEulerAngles(null, null, new float?(base.transform.eulerAngles.z - 90f));
		yield return null;
		yield break;
	}

	// Token: 0x04001069 RID: 4201
	[SerializeField]
	public Effect bulletFX;

	// Token: 0x0400106A RID: 4202
	[SerializeField]
	public Transform root;
}
