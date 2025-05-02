using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200021A RID: 538
public class DragonLevelTail : LevelProperties.Dragon.Entity
{
	// Token: 0x1700028E RID: 654
	// (get) Token: 0x0600187D RID: 6269 RVA: 0x00014EAD File Offset: 0x000130AD
	// (set) Token: 0x0600187E RID: 6270 RVA: 0x00014EB5 File Offset: 0x000130B5
	public DragonLevelTail.State state { get; set; }

	// Token: 0x0600187F RID: 6271 RVA: 0x000A3E20 File Offset: 0x000A2020
	public override void Awake()
	{
		base.Awake();
		base.RegisterCollisionChild(this.childCollider);
		base.transform.SetPosition(null, new float?(-1210f), null);
		this.damageDealer = new DamageDealer(1f, 0.1f, true, false, false);
	}

	// Token: 0x06001880 RID: 6272 RVA: 0x00014EBE File Offset: 0x000130BE
	public override void LevelInit(LevelProperties.Dragon properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001881 RID: 6273 RVA: 0x00014EC7 File Offset: 0x000130C7
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001882 RID: 6274 RVA: 0x00014EDF File Offset: 0x000130DF
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001883 RID: 6275 RVA: 0x00014F08 File Offset: 0x00013108
	public void TailStart(float warningTime, float inTime, float holdTime, float outTime)
	{
		base.StartCoroutine(this.go_cr(warningTime, inTime, holdTime, outTime));
	}

	// Token: 0x06001884 RID: 6276 RVA: 0x000A3E80 File Offset: 0x000A2080
	public IEnumerator go_cr(float warningTime, float inTime, float holdTime, float outTime)
	{
		this.state = DragonLevelTail.State.Tail;
		base.transform.SetPosition(new float?(PlayerManager.GetNext().transform.position.x), null, null);
		AudioManager.Play("level_dragon_left_dragon_tail_appear");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_tail_appear");
		yield return base.TweenPositionY(-1210f, -1045f, 0.3f, EaseUtils.EaseType.easeOutSine);
		yield return CupheadTime.WaitForSeconds(this, warningTime);
		AudioManager.Play("level_dragon_left_dragon_tail_attack");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_tail_attack");
		yield return base.TweenPositionY(-1045f, -465f, inTime, EaseUtils.EaseType.easeInSine);
		CupheadLevelCamera.Current.Shake(20f, 0.4f, false);
		yield return CupheadTime.WaitForSeconds(this, holdTime);
		yield return base.TweenPositionY(-465f, -1210f, outTime, EaseUtils.EaseType.easeInSine);
		this.state = DragonLevelTail.State.Idle;
		yield break;
	}

	// Token: 0x040013DD RID: 5085
	public const float OUT_Y = -1210f;

	// Token: 0x040013DE RID: 5086
	public const float IN_Y = -465f;

	// Token: 0x040013DF RID: 5087
	public const float START_Y = -1045f;

	// Token: 0x040013E0 RID: 5088
	public const float START_TIME = 0.3f;

	// Token: 0x040013E2 RID: 5090
	[SerializeField]
	public CollisionChild childCollider;

	// Token: 0x040013E3 RID: 5091
	public DamageDealer damageDealer;

	// Token: 0x02000C0A RID: 3082
	public enum State
	{
		// Token: 0x0400578A RID: 22410
		Idle,
		// Token: 0x0400578B RID: 22411
		Tail
	}
}
