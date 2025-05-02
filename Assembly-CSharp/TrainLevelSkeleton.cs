using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003BA RID: 954
public class TrainLevelSkeleton : LevelProperties.Train.Entity
{
	// Token: 0x14000057 RID: 87
	// (add) Token: 0x06002A3E RID: 10814 RVA: 0x000D39A4 File Offset: 0x000D1BA4
	// (remove) Token: 0x06002A3F RID: 10815 RVA: 0x000D39DC File Offset: 0x000D1BDC
	public event TrainLevelSkeleton.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000058 RID: 88
	// (add) Token: 0x06002A40 RID: 10816 RVA: 0x000D3A14 File Offset: 0x000D1C14
	// (remove) Token: 0x06002A41 RID: 10817 RVA: 0x000D3A4C File Offset: 0x000D1C4C
	public event Action OnDeathEvent;

	// Token: 0x06002A42 RID: 10818 RVA: 0x000238CB File Offset: 0x00021ACB
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = this.head.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x000D3A84 File Offset: 0x000D1C84
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x000238FB File Offset: 0x00021AFB
	public void Die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x00023923 File Offset: 0x00021B23
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.health = properties.CurrentState.skeleton.health;
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x00023942 File Offset: 0x00021B42
	public void StartSkeleton()
	{
		AudioManager.Play("train_passenger_car_explode");
		this.emitAudioFromObject.Add("train_passenger_car_explode");
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x0002396B File Offset: 0x00021B6B
	public void In()
	{
		AudioManager.Play("level_train_skeleton_up");
		this.head.In();
		this.leftHand.In();
		this.rightHand.In();
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x00023998 File Offset: 0x00021B98
	public void Out()
	{
		AudioManager.Play("train_skeleton_hand_out");
		this.emitAudioFromObject.Add("train_skeleton_hand_out");
		this.head.Out();
		this.leftHand.Out();
		this.rightHand.Out();
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x000D3AE4 File Offset: 0x000D1CE4
	public void RandomizeLocations()
	{
		TrainLevelSkeleton.Position position;
		for (position = this.currentPosition; position == this.currentPosition; position = (TrainLevelSkeleton.Position)Random.Range(0, 3))
		{
		}
		this.currentPosition = position;
		this.head.SetPosition(position);
		switch (position)
		{
		case TrainLevelSkeleton.Position.Right:
			this.leftHand.SetPosition(TrainLevelSkeleton.Position.Left);
			this.rightHand.SetPosition(TrainLevelSkeleton.Position.Center);
			break;
		case TrainLevelSkeleton.Position.Center:
			this.leftHand.SetPosition(TrainLevelSkeleton.Position.Left);
			this.rightHand.SetPosition(TrainLevelSkeleton.Position.Right);
			break;
		default:
			this.leftHand.SetPosition(TrainLevelSkeleton.Position.Center);
			this.rightHand.SetPosition(TrainLevelSkeleton.Position.Right);
			break;
		}
	}

	// Token: 0x06002A4A RID: 10826 RVA: 0x000D3B94 File Offset: 0x000D1D94
	public IEnumerator loop_cr()
	{
		this.currentPosition = TrainLevelSkeleton.Position.Center;
		float attackDelay = 0f;
		Animator handAnimator = this.rightHand.GetComponent<Animator>();
		for (;;)
		{
			attackDelay = Mathf.Lerp(base.properties.CurrentState.skeleton.attackDelay.max, base.properties.CurrentState.skeleton.attackDelay.min, this.health / base.properties.CurrentState.skeleton.health);
			this.In();
			yield return handAnimator.WaitForAnimationToEnd(this, "In", false, true);
			yield return CupheadTime.WaitForSeconds(this, attackDelay);
			AudioManager.Play("train_skeleton_hand_slap");
			this.leftHand.Slap();
			this.rightHand.Slap();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.skeleton.slapHoldTime);
			this.Out();
			yield return this.head.animator.WaitForAnimationToEnd(this, "Out", false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.skeleton.appearDelay);
			this.RandomizeLocations();
		}
		yield break;
	}

	// Token: 0x06002A4B RID: 10827 RVA: 0x000D3BB0 File Offset: 0x000D1DB0
	public IEnumerator die_cr()
	{
		Animator handAnimator = this.rightHand.GetComponent<Animator>();
		this.head.Die();
		this.rightHand.Die();
		this.leftHand.Die();
		AudioManager.Play("train_skeleton_hand_death");
		this.emitAudioFromObject.Add("train_skeleton_hand_death");
		yield return handAnimator.WaitForAnimationToEnd(this, "Death", false, true);
		this.head.EndDeath();
		this.rightHand.EndDeath();
		this.leftHand.EndDeath();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		yield break;
	}

	// Token: 0x0400233E RID: 9022
	[SerializeField]
	public TrainLevelSkeletonHead head;

	// Token: 0x0400233F RID: 9023
	[SerializeField]
	public TrainLevelSkeletonHand leftHand;

	// Token: 0x04002340 RID: 9024
	[SerializeField]
	public TrainLevelSkeletonHand rightHand;

	// Token: 0x04002341 RID: 9025
	public DamageReceiver damageReceiver;

	// Token: 0x04002342 RID: 9026
	public float health;

	// Token: 0x04002343 RID: 9027
	public bool dead;

	// Token: 0x04002344 RID: 9028
	public TrainLevelSkeleton.Position currentPosition;

	// Token: 0x02000FC9 RID: 4041
	public enum Position
	{
		// Token: 0x04007191 RID: 29073
		Right,
		// Token: 0x04007192 RID: 29074
		Center,
		// Token: 0x04007193 RID: 29075
		Left
	}

	// Token: 0x02000FCA RID: 4042
	// (Invoke) Token: 0x0600761F RID: 30239
	public delegate void OnDamageTakenHandler(float damage);
}
