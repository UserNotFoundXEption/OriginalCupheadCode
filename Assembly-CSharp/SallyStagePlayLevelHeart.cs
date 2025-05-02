using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035D RID: 861
public class SallyStagePlayLevelHeart : AbstractProjectile
{
	// Token: 0x06002602 RID: 9730 RVA: 0x0001FE70 File Offset: 0x0001E070
	public override void Update()
	{
		this.damageDealer.Update();
		base.Update();
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x000C8134 File Offset: 0x000C6334
	public void InitHeart(LevelProperties.SallyStagePlay properties, int direction, bool isParryable)
	{
		this.properties = properties;
		this.time = 0f;
		this.direction = direction;
		this.pos = base.transform.position;
		if (!isParryable)
		{
			base.GetComponent<SpriteRenderer>().color = Color.blue;
		}
		else
		{
			this.SetParryable(true);
		}
		base.StartCoroutine(this.wave_cr());
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x000C819C File Offset: 0x000C639C
	public IEnumerator wave_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		for (;;)
		{
			Vector3 newPos = this.pos;
			newPos.y = Mathf.Sin(this.time * this.properties.CurrentState.kiss.sineWaveSpeed) * this.properties.CurrentState.kiss.sineWaveStrength;
			base.transform.position = newPos;
			this.pos += Vector3.left * (float)this.direction * this.properties.CurrentState.kiss.heartSpeed * CupheadTime.Delta;
			this.time += CupheadTime.Delta;
			yield return null;
			if (base.transform.position.x < (float)(Level.Current.Left - 150) || base.transform.position.x > (float)(Level.Current.Right + 150))
			{
				Object.Destroy(base.gameObject);
			}
		}
		yield break;
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x0001FE83 File Offset: 0x0001E083
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002606 RID: 9734 RVA: 0x0001FEAC File Offset: 0x0001E0AC
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04001F7C RID: 8060
	public int direction;

	// Token: 0x04001F7D RID: 8061
	public float time;

	// Token: 0x04001F7E RID: 8062
	public Vector3 pos;

	// Token: 0x04001F7F RID: 8063
	public LevelProperties.SallyStagePlay properties;
}
