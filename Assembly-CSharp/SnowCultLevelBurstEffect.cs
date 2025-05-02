using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200038A RID: 906
public class SnowCultLevelBurstEffect : Effect
{
	// Token: 0x06002816 RID: 10262 RVA: 0x000CD62C File Offset: 0x000CB82C
	public SnowCultLevelBurstEffect Create(Vector3 pos, float direction)
	{
		SnowCultLevelBurstEffect snowCultLevelBurstEffect = base.Create(pos) as SnowCultLevelBurstEffect;
		snowCultLevelBurstEffect.direction = direction;
		return snowCultLevelBurstEffect;
	}

	// Token: 0x06002817 RID: 10263 RVA: 0x000CD650 File Offset: 0x000CB850
	public void Start()
	{
		this.startPosY = base.transform.position.y;
		if (this.isSnowFall)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x06002818 RID: 10264 RVA: 0x000CD690 File Offset: 0x000CB890
	public void SpawnEffect()
	{
		Vector3 pos;
		pos..ctor(base.transform.position.x + 127f * this.direction, (!this.isSnowFall) ? 95f : this.startPosY);
		if (pos.x > -740f && pos.x < 740f)
		{
			if (this.isTypeA)
			{
				this.typeA.Create(pos, this.direction);
			}
			else
			{
				this.typeB.Create(pos, this.direction);
			}
		}
	}

	// Token: 0x06002819 RID: 10265 RVA: 0x000CD738 File Offset: 0x000CB938
	public IEnumerator move_cr()
	{
		while (base.transform.position.y > -360f)
		{
			base.transform.position += Vector3.down * 150f * CupheadTime.Delta;
			yield return null;
		}
		this.OnEffectComplete();
		yield return null;
		yield break;
	}

	// Token: 0x04002139 RID: 8505
	public const float DIST_X_TO_MOVE = 127f;

	// Token: 0x0400213A RID: 8506
	public const float Y_TO_SPAWN = 95f;

	// Token: 0x0400213B RID: 8507
	public const float MOVE_SPEED = 150f;

	// Token: 0x0400213C RID: 8508
	[SerializeField]
	public bool isSnowFall;

	// Token: 0x0400213D RID: 8509
	[SerializeField]
	public bool isTypeA;

	// Token: 0x0400213E RID: 8510
	[SerializeField]
	public SnowCultLevelBurstEffect typeA;

	// Token: 0x0400213F RID: 8511
	[SerializeField]
	public SnowCultLevelBurstEffect typeB;

	// Token: 0x04002140 RID: 8512
	public float startPosY;

	// Token: 0x04002141 RID: 8513
	public float direction;
}
