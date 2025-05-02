using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000274 RID: 628
public class FlyingGenieLevelPyramid : AbstractCollidableObject
{
	// Token: 0x06001CC1 RID: 7361 RVA: 0x000AF0C4 File Offset: 0x000AD2C4
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		foreach (GameObject gameObject in this.beams)
		{
			gameObject.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		}
	}

	// Token: 0x06001CC2 RID: 7362 RVA: 0x00018633 File Offset: 0x00016833
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CC3 RID: 7363 RVA: 0x00018651 File Offset: 0x00016851
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CC4 RID: 7364 RVA: 0x000AF11C File Offset: 0x000AD31C
	public void Init(LevelProperties.FlyingGenie.Pyramids properties, Vector2 startPos, float startAngle, float speed, Transform pivot, int number, bool isClockWise)
	{
		base.transform.position = startPos;
		this.angle = startAngle;
		this.speed = speed;
		this.pivotPoint = pivot;
		this.number = number;
		this.properties = properties;
		this.isClockwise = isClockWise;
		base.StartCoroutine(this.path_cr());
	}

	// Token: 0x06001CC5 RID: 7365 RVA: 0x000AF178 File Offset: 0x000AD378
	public IEnumerator beam_cr()
	{
		this.finishedATK = false;
		AudioManager.Play("genie_pyramid_attack");
		this.emitAudioFromObject.Add("genie_pyramid_attack");
		base.animator.SetTrigger("OnStartAttack");
		yield return base.animator.WaitForAnimationToEnd(this, "Open_Start", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.warningDuration);
		base.animator.SetTrigger("OnShoot");
		yield return base.animator.WaitForAnimationToEnd(this, "Shoot_Start", false, true);
		foreach (GameObject gameObject in this.beams)
		{
			gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.beamDuration);
		base.animator.SetTrigger("OnEnd");
		foreach (GameObject gameObject2 in this.beams)
		{
			gameObject2.GetComponent<Animator>().SetBool("IsAttacking", false);
		}
		this.finishedATK = true;
		yield break;
	}

	// Token: 0x06001CC6 RID: 7366 RVA: 0x000AF194 File Offset: 0x000AD394
	public IEnumerator path_cr()
	{
		for (;;)
		{
			this.PathMovement();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CC7 RID: 7367 RVA: 0x000AF1B0 File Offset: 0x000AD3B0
	public void PathMovement()
	{
		this.angle += this.speed * CupheadTime.Delta;
		Vector3 vector;
		if (this.isClockwise)
		{
			vector..ctor(Mathf.Sin(this.angle) * this.properties.pyramidLoopSize, 0f, 0f);
		}
		else
		{
			vector..ctor(-Mathf.Sin(this.angle) * this.properties.pyramidLoopSize, 0f, 0f);
		}
		Vector3 vector2;
		vector2..ctor(0f, Mathf.Cos(this.angle) * this.properties.pyramidLoopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += vector + vector2;
	}

	// Token: 0x0400175C RID: 5980
	public int number;

	// Token: 0x0400175D RID: 5981
	public bool finishedATK;

	// Token: 0x0400175E RID: 5982
	[SerializeField]
	public GameObject[] beams;

	// Token: 0x0400175F RID: 5983
	public LevelProperties.FlyingGenie.Pyramids properties;

	// Token: 0x04001760 RID: 5984
	public DamageDealer damageDealer;

	// Token: 0x04001761 RID: 5985
	public DamageReceiver damageReceiver;

	// Token: 0x04001762 RID: 5986
	public Transform pivotPoint;

	// Token: 0x04001763 RID: 5987
	public float speed;

	// Token: 0x04001764 RID: 5988
	public float angle;

	// Token: 0x04001765 RID: 5989
	public bool isClockwise;
}
