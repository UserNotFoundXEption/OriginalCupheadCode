using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200027A RID: 634
public class FlyingGenieLevelSword : AbstractProjectile
{
	// Token: 0x06001CF0 RID: 7408 RVA: 0x000AF70C File Offset: 0x000AD90C
	public void Init(Vector3 startPos, Vector3 endPos, LevelProperties.FlyingGenie.Swords properties, AbstractPlayerController player)
	{
		this.startPos = startPos;
		base.transform.position = startPos;
		this.properties = properties;
		this.endPos = endPos;
		this.player = player;
		base.StartCoroutine(this.move_to_pos_cr());
		AudioManager.Play("genie_chest_sword_spawn");
		this.emitAudioFromObject.Add("genie_chest_sword_spawn");
	}

	// Token: 0x06001CF1 RID: 7409 RVA: 0x0001880B File Offset: 0x00016A0B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001CF2 RID: 7410 RVA: 0x00018834 File Offset: 0x00016A34
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CF3 RID: 7411 RVA: 0x000AF76C File Offset: 0x000AD96C
	public IEnumerator move_to_pos_cr()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, 90f);
		while (base.transform.position.y < (this.startPos + Vector3.up * this.outOfChestY).y)
		{
			base.transform.AddPosition(0f, this.outOfChestY * this.outOfChestSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		this.swordRenderer.sortingLayerName = "Projectiles";
		this.swordRenderer.sortingOrder = 2;
		base.transform.eulerAngles = new Vector3(0f, 0f, MathUtils.DirectionToAngle(this.endPos - base.transform.position));
		while (base.transform.position != this.endPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.endPos, this.properties.swordSpeed * CupheadTime.Delta);
			yield return null;
		}
		float t = 0f;
		while (t < this.properties.attackDelay)
		{
			base.transform.Rotate(Vector3.forward, this.swordRotationSpeed * CupheadTime.Delta);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.StartCoroutine(this.move_continue_cr());
		AudioManager.Play("genie_chest_sword_attack");
		this.emitAudioFromObject.Add("genie_chest_sword_attack");
		yield return null;
		yield break;
	}

	// Token: 0x06001CF4 RID: 7412 RVA: 0x000AF788 File Offset: 0x000AD988
	public IEnumerator move_continue_cr()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, 35f);
		base.animator.SetTrigger("Spin");
		yield return CupheadTime.WaitForSeconds(this, this.fastSpinTime);
		base.animator.SetTrigger("Attack");
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 direction = this.player.transform.position - base.transform.position;
		base.transform.SetEulerAngles(null, null, new float?(MathUtils.DirectionToAngle(direction)));
		for (;;)
		{
			base.transform.position += base.transform.right * this.properties.swordSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CF5 RID: 7413 RVA: 0x00018852 File Offset: 0x00016A52
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetFloat("Pink", (!parryable) ? 0f : 1f);
	}

	// Token: 0x04001787 RID: 6023
	public const string PinkParameterName = "Pink";

	// Token: 0x04001788 RID: 6024
	public const string SpinParameterName = "Spin";

	// Token: 0x04001789 RID: 6025
	public const string AttackParameterName = "Attack";

	// Token: 0x0400178A RID: 6026
	public const string ProjectilesLayer = "Projectiles";

	// Token: 0x0400178B RID: 6027
	public const float spinRotationOffset = 35f;

	// Token: 0x0400178C RID: 6028
	[SerializeField]
	public float outOfChestY;

	// Token: 0x0400178D RID: 6029
	[SerializeField]
	public float outOfChestSpeed;

	// Token: 0x0400178E RID: 6030
	[SerializeField]
	public float swordRotationSpeed;

	// Token: 0x0400178F RID: 6031
	[SerializeField]
	public float fastSpinTime;

	// Token: 0x04001790 RID: 6032
	[SerializeField]
	public SpriteRenderer swordRenderer;

	// Token: 0x04001791 RID: 6033
	public LevelProperties.FlyingGenie.Swords properties;

	// Token: 0x04001792 RID: 6034
	public AbstractPlayerController player;

	// Token: 0x04001793 RID: 6035
	public Vector3 endPos;

	// Token: 0x04001794 RID: 6036
	public Vector3 startPos;
}
