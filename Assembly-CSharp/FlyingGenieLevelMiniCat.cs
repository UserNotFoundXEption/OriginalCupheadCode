using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200026E RID: 622
public class FlyingGenieLevelMiniCat : HomingProjectile
{
	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06001C8E RID: 7310 RVA: 0x00018377 File Offset: 0x00016577
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001C8F RID: 7311 RVA: 0x000AE5FC File Offset: 0x000AC7FC
	public FlyingGenieLevelMiniCat Create(Vector3 pos, float rotation, AbstractPlayerController player, LevelProperties.FlyingGenie.Sphinx properties)
	{
		FlyingGenieLevelMiniCat flyingGenieLevelMiniCat = base.Create(pos, rotation, properties.homingSpeed, properties.homingSpeed, properties.homingRotation, 20f, 0f, player) as FlyingGenieLevelMiniCat;
		flyingGenieLevelMiniCat.properties = properties;
		flyingGenieLevelMiniCat.transform.position = pos;
		return flyingGenieLevelMiniCat;
	}

	// Token: 0x06001C90 RID: 7312 RVA: 0x0001837A File Offset: 0x0001657A
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06001C91 RID: 7313 RVA: 0x0001838F File Offset: 0x0001658F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.properties.dieOnCollisionPlayer)
		{
			this.Die();
		}
	}

	// Token: 0x06001C92 RID: 7314 RVA: 0x000183AF File Offset: 0x000165AF
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C93 RID: 7315 RVA: 0x000AE654 File Offset: 0x000AC854
	public IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.miniHomingDurationRange.RandomFloat());
		base.HomingEnabled = false;
		for (;;)
		{
			base.transform.position += base.transform.right * this.properties.homingSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C94 RID: 7316 RVA: 0x000183CD File Offset: 0x000165CD
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetFloat("Pink", (float)((!parryable) ? 0 : 1));
	}

	// Token: 0x04001734 RID: 5940
	public const string PinkParameterName = "Pink";

	// Token: 0x04001735 RID: 5941
	public LevelProperties.FlyingGenie.Sphinx properties;
}
