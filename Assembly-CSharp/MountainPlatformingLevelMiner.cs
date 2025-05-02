using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000445 RID: 1093
public class MountainPlatformingLevelMiner : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002EE8 RID: 12008 RVA: 0x00027153 File Offset: 0x00025353
	public override void Start()
	{
		base.Start();
		this.startPos = base.transform.position;
		base.StartCoroutine(this.descend_cr());
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x000E07E0 File Offset: 0x000DE9E0
	public IEnumerator descend_cr()
	{
		this.floating = false;
		this.landing = true;
		float t = 0f;
		float time = base.Properties.minerDescendTime;
		Vector3 endPos = new Vector3(base.transform.position.x, base.transform.position.y - 400f);
		Vector3 startPos = base.transform.position;
		AudioManager.Play("castle_miner_spawn");
		this.emitAudioFromObject.Add("castle_miner_spawn");
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(startPos, endPos, val);
			yield return null;
		}
		this.rope.transform.parent = null;
		yield return CupheadTime.WaitForSeconds(this, 0.4f);
		base.animator.SetTrigger("Continue");
		this.rope.animator.SetTrigger("Jump");
		this.floating = true;
		this.landing = false;
		yield return base.animator.WaitForAnimationToEnd(this, "Jump_Start", false, true);
		this.rope.animator.SetTrigger("Pull");
		while (!base.Grounded)
		{
			yield return null;
		}
		base.animator.SetTrigger("Land");
		this.rope.transform.parent = null;
		this.rope.PullRope(base.Properties.minerRopeAscendTime, startPos);
		this.leftRope = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Jump_End", false, true);
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.look_direction_cr());
		base.StartCoroutine(this.face_direction_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x000E07FC File Offset: 0x000DE9FC
	public IEnumerator look_direction_cr()
	{
		float maxDist = 30f;
		while (this.player == null)
		{
			yield return null;
		}
		for (;;)
		{
			float dist = this.player.transform.position.y - this.lookAt.transform.position.y;
			if (dist < maxDist && dist > -maxDist)
			{
				this.straight.enabled = true;
				this.down.enabled = false;
				this.up.enabled = false;
			}
			else if (dist > maxDist)
			{
				this.straight.enabled = false;
				this.down.enabled = false;
				this.up.enabled = true;
			}
			else
			{
				this.straight.enabled = false;
				this.down.enabled = true;
				this.up.enabled = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x000E0818 File Offset: 0x000DEA18
	public IEnumerator face_direction_cr()
	{
		while (this.player == null)
		{
			yield return null;
		}
		for (;;)
		{
			if (!this.inAttack && ((this.player.transform.position.x > base.transform.position.x && base.direction == PlatformingLevelGroundMovementEnemy.Direction.Left) || (this.player.transform.position.x < base.transform.position.x && base.direction == PlatformingLevelGroundMovementEnemy.Direction.Right)) && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn"))
			{
				this.Turn();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x000E0834 File Offset: 0x000DEA34
	public IEnumerator shoot_cr()
	{
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			this.player = PlayerManager.GetNext();
			yield return CupheadTime.WaitForSeconds(this, base.Properties.minerShotDelay.RandomFloat());
			this.inAttack = true;
			base.animator.SetTrigger("Shoot");
			while (this.currentPickaxe != null || !this.isShooting)
			{
				yield return null;
			}
			base.animator.Play("Catch");
			this.isShooting = false;
			this.inAttack = false;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EED RID: 12013 RVA: 0x000E0850 File Offset: 0x000DEA50
	public void ShootPickaxe()
	{
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 vector = this.player.transform.position - this.root.transform.position;
		Vector3 targetPos = this.root.transform.position + vector.normalized * base.Properties.minerDistance;
		this.currentPickaxe = this.pickaxe.Create(this.root.transform.position, MathUtils.DirectionToAngle(vector), base.Properties.minerShootSpeed, this, targetPos, this.catchRoot.transform.position);
		this.isShooting = true;
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x00027179 File Offset: 0x00025379
	public void ShootSFX()
	{
		AudioManager.Play("castle_miner_throw");
		this.emitAudioFromObject.Add("castle_miner_throw");
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x00027195 File Offset: 0x00025395
	public void CatchSFX()
	{
		AudioManager.Play("castle_miner_catch_pick");
		this.emitAudioFromObject.Add("castle_miner_catch_pick");
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x000E0934 File Offset: 0x000DEB34
	public void Offset()
	{
		if (base.direction == PlatformingLevelGroundMovementEnemy.Direction.Left)
		{
			base.transform.AddPosition(47f, 0f, 0f);
		}
		else
		{
			base.transform.AddPosition(-47f, 0f, 0f);
		}
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x000E0988 File Offset: 0x000DEB88
	public override void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		if (!this.leftRope)
		{
			this.rope.animator.SetTrigger("Jump");
			this.rope.animator.SetTrigger("Pull");
			this.rope.transform.parent = null;
			this.rope.PullRope(base.Properties.minerRopeAscendTime, this.startPos);
		}
		AudioManager.Play("castle_generic_death");
		this.emitAudioFromObject.Add("castle_generic_death");
		base.Die();
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000271B1 File Offset: 0x000253B1
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.pickaxe = null;
	}

	// Token: 0x040026EA RID: 9962
	[SerializeField]
	public MountainPlatformingLevelPickaxeProjectile pickaxe;

	// Token: 0x040026EB RID: 9963
	[SerializeField]
	public SpriteRenderer straight;

	// Token: 0x040026EC RID: 9964
	[SerializeField]
	public SpriteRenderer up;

	// Token: 0x040026ED RID: 9965
	[SerializeField]
	public SpriteRenderer down;

	// Token: 0x040026EE RID: 9966
	[SerializeField]
	public Transform lookAt;

	// Token: 0x040026EF RID: 9967
	[SerializeField]
	public MountainPlatformingLevelMinerRope rope;

	// Token: 0x040026F0 RID: 9968
	[SerializeField]
	public Transform root;

	// Token: 0x040026F1 RID: 9969
	[SerializeField]
	public Transform catchRoot;

	// Token: 0x040026F2 RID: 9970
	public Vector3 startPos;

	// Token: 0x040026F3 RID: 9971
	public AbstractPlayerController player;

	// Token: 0x040026F4 RID: 9972
	public MountainPlatformingLevelPickaxeProjectile currentPickaxe;

	// Token: 0x040026F5 RID: 9973
	public bool isShooting;

	// Token: 0x040026F6 RID: 9974
	public bool inAttack;

	// Token: 0x040026F7 RID: 9975
	public bool leftRope;

	// Token: 0x040026F8 RID: 9976
	public float offset = 50f;
}
