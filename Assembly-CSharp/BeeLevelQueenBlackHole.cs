using System;
using UnityEngine;

// Token: 0x02000169 RID: 361
public class BeeLevelQueenBlackHole : AbstractProjectile
{
	// Token: 0x0600115B RID: 4443 RVA: 0x000923B8 File Offset: 0x000905B8
	public override void Start()
	{
		base.Start();
		this.direction = ((base.transform.position.x >= 0f) ? -1 : 1);
	}

	// Token: 0x0600115C RID: 4444 RVA: 0x000923F8 File Offset: 0x000905F8
	public override void Update()
	{
		base.Update();
		base.transform.AddPosition(this.speed * (float)this.direction * CupheadTime.Delta, 0f, 0f);
		this.timer += CupheadTime.Delta;
		if (this.timer >= this.childDelay)
		{
			this.childPrefab.Create(base.transform.position, 90f, this.childSpeed);
			this.childPrefab.Create(base.transform.position, -90f, this.childSpeed).GetComponent<Animator>().Play("Reverse");
			this.timer = 0f;
		}
	}

	// Token: 0x0600115D RID: 4445 RVA: 0x0000EB53 File Offset: 0x0000CD53
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.childPrefab = null;
	}

	// Token: 0x04000E03 RID: 3587
	[HideInInspector]
	public float health;

	// Token: 0x04000E04 RID: 3588
	[HideInInspector]
	public float speed;

	// Token: 0x04000E05 RID: 3589
	[HideInInspector]
	public float childDelay;

	// Token: 0x04000E06 RID: 3590
	[HideInInspector]
	public float childSpeed;

	// Token: 0x04000E07 RID: 3591
	[SerializeField]
	public BasicProjectile childPrefab;

	// Token: 0x04000E08 RID: 3592
	public int direction;

	// Token: 0x04000E09 RID: 3593
	public float timer;
}
