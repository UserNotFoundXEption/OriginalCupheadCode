using System;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class DevilLevelSplitDevil : LevelProperties.Devil.Entity
{
	// Token: 0x060014E3 RID: 5347 RVA: 0x00011B8C File Offset: 0x0000FD8C
	public override void Awake()
	{
		base.Awake();
		base.animator.Play("Idle");
		this.state = DevilLevelSplitDevil.State.Idle;
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x0009AC18 File Offset: 0x00098E18
	public void LateUpdate()
	{
		LevelPlayerController levelPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
		LevelPlayerController levelPlayerController2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController;
		bool flag = levelPlayerController == null || levelPlayerController.transform.localScale.x > 0f;
		this.headsControler.SetBool("LookRight", flag);
		base.animator.SetBool("LookRight", flag);
		this.headsControler.SetBool("DevilLeft", this.DevilLeft);
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x0009ACA0 File Offset: 0x00098EA0
	public void OnIdleLeftEnd()
	{
		bool @bool = base.animator.GetBool("Shoot");
		bool bool2 = base.animator.GetBool("LookRight");
		string text = "DevilLeftShootTransition_2_3_4";
		string text2 = "DevilLeftIdleBody_2_3_4";
		if (@bool)
		{
			if (bool2)
			{
				text = "DevilToAngel_Transition_2_3_4";
				text2 = "DevilToAngelIdleBody_2_3_4";
			}
			this.headsControler.enabled = true;
			this.headsControler.SetBool("Shoot", true);
			this.headsControler.Play(text, -1, 1f);
			base.animator.Play(text2, -1, 1f);
		}
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x0009AD34 File Offset: 0x00098F34
	public void OnIdleRightEnd()
	{
		bool @bool = base.animator.GetBool("Shoot");
		bool bool2 = base.animator.GetBool("LookRight");
		string text = "DevilRightShootTransition_2_3_4";
		string text2 = "DevilRightIdleBody_2_3_4";
		if (@bool)
		{
			if (!bool2)
			{
				text = "AngelToDevil_transition_2_3_4";
				text2 = "AngelToDevilIdleBody_2_3_4";
			}
			this.headsControler.enabled = true;
			this.headsControler.SetBool("Shoot", true);
			this.headsControler.Play(text, -1, 1f);
			base.animator.Play(text2, -1, 1f);
		}
	}

	// Token: 0x060014E7 RID: 5351 RVA: 0x00011BAB File Offset: 0x0000FDAB
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x00011BBE File Offset: 0x0000FDBE
	public void StartTransform()
	{
		base.animator.SetTrigger("IsDead");
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x00011BD0 File Offset: 0x0000FDD0
	public void OnDeadAnimationDone()
	{
		this.SplitDevilAnimationDone = true;
		base.gameObject.SetActive(false);
	}

	// Token: 0x0400111B RID: 4379
	public DevilLevelSplitDevil.State state;

	// Token: 0x0400111C RID: 4380
	[SerializeField]
	public Animator headsControler;

	// Token: 0x0400111D RID: 4381
	public bool DevilLeft = true;

	// Token: 0x0400111E RID: 4382
	public bool SplitDevilAnimationDone;

	// Token: 0x0400111F RID: 4383
	[SerializeField]
	public DevilLevelSplitDevilProjectile projectilePrefab;

	// Token: 0x04001120 RID: 4384
	public DevilLevelSplitDevilProjectile AngelprojectilePrefab;

	// Token: 0x04001121 RID: 4385
	[SerializeField]
	public Transform projectileRootLeft;

	// Token: 0x04001122 RID: 4386
	[SerializeField]
	public Transform projectileRootRight;

	// Token: 0x04001123 RID: 4387
	public int patternIndex;

	// Token: 0x04001124 RID: 4388
	public LevelProperties.Devil.Pattern pattern;

	// Token: 0x02000B4E RID: 2894
	public enum State
	{
		// Token: 0x040052DA RID: 21210
		Idle,
		// Token: 0x040052DB RID: 21211
		Shoot,
		// Token: 0x040052DC RID: 21212
		summon
	}
}
