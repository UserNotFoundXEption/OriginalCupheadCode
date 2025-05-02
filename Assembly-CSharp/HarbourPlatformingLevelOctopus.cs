using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000432 RID: 1074
public class HarbourPlatformingLevelOctopus : PlatformingLevelAutoscrollObject
{
	// Token: 0x06002E4A RID: 11850 RVA: 0x000DF390 File Offset: 0x000DD590
	public override void Awake()
	{
		base.Awake();
		this.anchor.OnActivate += this.Switched;
		this.yPosStart = base.transform.position.y;
		this.collisionChild.OnPlayerProjectileCollision += this.OnCollisionPlayerProjectile;
		this.collisionChild.OnAnyCollision += this.OnCollision;
		this.checkToLock = false;
		this.pinkGem.SetActive(true);
		base.StartCoroutine(this.gem_shine_switch_cr());
	}

	// Token: 0x06002E4B RID: 11851 RVA: 0x000269BA File Offset: 0x00024BBA
	public override void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayerProjectile(hit, phase);
		if (!this.tuckedDown)
		{
			this.timeSinceShot = 0f;
			base.animator.SetTrigger("PlayerShooting");
		}
	}

	// Token: 0x06002E4C RID: 11852 RVA: 0x000DF424 File Offset: 0x000DD624
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<HarbourPlatformingLevelIceberg>())
		{
			if (!this.tuckedDown)
			{
				base.StartCoroutine(this.disable_cr());
			}
			hit.GetComponent<HarbourPlatformingLevelIceberg>().DeathParts();
			Object.Destroy(hit.gameObject);
		}
	}

	// Token: 0x06002E4D RID: 11853 RVA: 0x000269EA File Offset: 0x00024BEA
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06002E4E RID: 11854 RVA: 0x000DF478 File Offset: 0x000DD678
	public override void Update()
	{
		base.Update();
		CupheadLevelCamera cupheadLevelCamera = CupheadLevelCamera.Current;
		float num = cupheadLevelCamera.autoScrollSpeedMultiplier;
		this.timeSinceShot += CupheadTime.Delta;
		if (this.tuckedDown || this.timeSinceShot > this.holdSpeedTime)
		{
			num -= CupheadTime.Delta * (this.speedupMultiplier - 1f) / this.deccelerationTime;
			num = Mathf.Max(1f, num);
		}
		else
		{
			num += CupheadTime.Delta * (this.speedupMultiplier - 1f) / this.accelerationTime;
			num = Mathf.Min(this.speedupMultiplier, num);
		}
		cupheadLevelCamera.SetAutoscrollSpeedMultiplier(num);
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			base.animator.speed = num;
		}
		else
		{
			base.animator.speed = 1f;
		}
	}

	// Token: 0x06002E4F RID: 11855 RVA: 0x000DF574 File Offset: 0x000DD774
	public void Switched()
	{
		this.pinkGem.SetActive(false);
		if (this.firstSwitch)
		{
			base.animator.SetTrigger("StartOctopus");
			this.StartAutoscroll();
			base.StartCoroutine(this.start_octopus_cr());
			this.firstSwitch = false;
		}
		else
		{
			base.animator.SetTrigger("Shoot");
			this.ShootSFX();
		}
	}

	// Token: 0x06002E50 RID: 11856 RVA: 0x000269F4 File Offset: 0x00024BF4
	public bool Started()
	{
		return base.isMoving;
	}

	// Token: 0x06002E51 RID: 11857 RVA: 0x000DF5E0 File Offset: 0x000DD7E0
	public IEnumerator start_octopus_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.transform.position.x + this.scrollMinMax.min)
		{
			yield return null;
		}
		CupheadLevelCamera.Current.OffsetCamera(true, true);
		base.StartCoroutine(this.idle_bounce_cr());
		base.animator.SetTrigger("StartTentacles");
		this.IdleTentaclesSFX();
		base.transform.parent = CupheadLevelCamera.Current.transform;
		yield return null;
		yield break;
	}

	// Token: 0x06002E52 RID: 11858 RVA: 0x000DF5FC File Offset: 0x000DD7FC
	public IEnumerator disable_cr()
	{
		this.HeadSquishSFX();
		base.animator.SetBool("IsHit", true);
		this.tuckedDown = true;
		float endPos = base.transform.position.y - 500f;
		float speed = 300f;
		Vector3 pos = base.transform.position;
		while (base.transform.position.y > endPos)
		{
			base.transform.AddPosition(0f, -speed * CupheadTime.FixedDelta, 0f);
			yield return null;
		}
		pos = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, this.tuckDownDelay);
		yield return null;
		while (base.transform.position.y < this.yPosStart)
		{
			base.transform.AddPosition(0f, speed * CupheadTime.FixedDelta, 0f);
			yield return null;
		}
		base.transform.position = new Vector3(base.transform.position.x, this.yPosStart);
		base.animator.SetBool("IsHit", false);
		this.HeadSquishSFX();
		this.tuckedDown = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002E53 RID: 11859 RVA: 0x000DF618 File Offset: 0x000DD818
	public IEnumerator end_octopus_cr()
	{
		this.MoveLoop();
		base.animator.SetTrigger("EndOctopus");
		base.transform.parent = null;
		float endPos = base.transform.position.y - 1000f;
		float speed = 100f;
		while (base.transform.position.y > endPos)
		{
			base.transform.AddPosition(0f, -speed * CupheadTime.FixedDelta, 0f);
			yield return null;
		}
		CupheadLevelCamera.Current.OffsetCamera(false, true);
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002E54 RID: 11860 RVA: 0x000269FC File Offset: 0x00024BFC
	public override void EndAutoscroll()
	{
		base.StartCoroutine(this.end_octopus_cr());
	}

	// Token: 0x06002E55 RID: 11861 RVA: 0x000DF634 File Offset: 0x000DD834
	public void Shoot()
	{
		this.ShootSFX();
		this.anchor.enabled = false;
		this.puff.Create(this.projectileRoot.transform.position);
		this.projectile.Create(this.projectileRoot.transform.position);
		base.StartCoroutine(this.gem_timer_cr());
	}

	// Token: 0x06002E56 RID: 11862 RVA: 0x000DF6A0 File Offset: 0x000DD8A0
	public IEnumerator gem_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.gemOffTime);
		this.pinkGem.SetActive(true);
		this.anchor.enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002E57 RID: 11863 RVA: 0x000DF6BC File Offset: 0x000DD8BC
	public IEnumerator gem_shine_switch_cr()
	{
		string order = "A1,B1,B2,A2,B1,A1,B2,A2";
		int orderIndex = 0;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.42f, 0.67f));
			base.animator.Play("Shine_" + order.Split(new char[]
			{
				','
			})[orderIndex], 1);
			orderIndex = (orderIndex + 1) % order.Split(new char[]
			{
				','
			}).Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E58 RID: 11864 RVA: 0x000DF6D8 File Offset: 0x000DD8D8
	public void TentacleBackSwitch()
	{
		Vector3 localPosition = this.tentacleBack.localPosition;
		localPosition.x = ((!this.moveTentacles) ? (this.tentacleBack.localPosition.x + this.tentacleOffset) : (this.tentacleBack.localPosition.x - this.tentacleOffset));
		this.tentacleBack.localPosition = localPosition;
	}

	// Token: 0x06002E59 RID: 11865 RVA: 0x000DF748 File Offset: 0x000DD948
	public void TentacleFrontSwitch()
	{
		this.moveTentacles = !this.moveTentacles;
		Vector3 localPosition = this.tentacleFront.localPosition;
		localPosition.x = ((!this.moveTentacles) ? (this.tentacleFront.localPosition.x + this.tentacleOffset) : (this.tentacleFront.localPosition.x - this.tentacleOffset));
		this.tentacleFront.localPosition = localPosition;
	}

	// Token: 0x06002E5A RID: 11866 RVA: 0x000DF7C8 File Offset: 0x000DD9C8
	public IEnumerator idle_bounce_cr()
	{
		float angle = 0f;
		float yVelocity = 7f;
		float sinSize = 2f;
		for (;;)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && CupheadTime.Delta != 0f)
			{
				angle += yVelocity * CupheadTime.Delta;
				Vector3 moveY = new Vector3(0f, Mathf.Sin(angle) * sinSize);
				base.transform.localPosition += moveY;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E5B RID: 11867 RVA: 0x00026A0B File Offset: 0x00024C0B
	public void HeadSquishSFX()
	{
		AudioManager.Play("harbour_octopus_head_squish");
		this.emitAudioFromObject.Add("harbour_octopus_head_squish");
	}

	// Token: 0x06002E5C RID: 11868 RVA: 0x00026A27 File Offset: 0x00024C27
	public void ShootSFX()
	{
		AudioManager.Play("harbour_octopus_shoot");
		this.emitAudioFromObject.Add("harbour_octopus_shoot");
	}

	// Token: 0x06002E5D RID: 11869 RVA: 0x00026A43 File Offset: 0x00024C43
	public void IdleTentaclesSFX()
	{
		AudioManager.Stop("harbour_octopus_move_loop");
		AudioManager.PlayLoop("harbour_octopus_idle_tentacles");
		this.emitAudioFromObject.Add("harbour_octopus_idle_tentacles");
	}

	// Token: 0x06002E5E RID: 11870 RVA: 0x00026A69 File Offset: 0x00024C69
	public void MoveLoop()
	{
		AudioManager.Stop("harbour_octopus_idle_tentacles");
		AudioManager.PlayLoop("harbour_octopus_move_loop");
		this.emitAudioFromObject.Add("harbour_octopus_move_loop");
	}

	// Token: 0x06002E5F RID: 11871 RVA: 0x00026A8F File Offset: 0x00024C8F
	public void RideStartSFX()
	{
		AudioManager.Stop("harbour_octopus_idle_tentacles");
		AudioManager.Stop("harbour_octopus_move_loop");
		AudioManager.Play("harbour_octopus_ride_start");
		this.emitAudioFromObject.Add("harbour_octopus_ride_start");
	}

	// Token: 0x06002E60 RID: 11872 RVA: 0x00026ABF File Offset: 0x00024CBF
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.puff = null;
		this.projectile = null;
	}

	// Token: 0x04002662 RID: 9826
	[SerializeField]
	public Effect puff;

	// Token: 0x04002663 RID: 9827
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x04002664 RID: 9828
	[SerializeField]
	public Transform tentacleFront;

	// Token: 0x04002665 RID: 9829
	[SerializeField]
	public Transform tentacleBack;

	// Token: 0x04002666 RID: 9830
	[SerializeField]
	public ParrySwitch anchor;

	// Token: 0x04002667 RID: 9831
	[SerializeField]
	public HarbourPlatformingLevelOctoProjectile projectile;

	// Token: 0x04002668 RID: 9832
	[SerializeField]
	public MinMax scrollMinMax = new MinMax(-300f, 200f);

	// Token: 0x04002669 RID: 9833
	[SerializeField]
	public GameObject pinkGem;

	// Token: 0x0400266A RID: 9834
	[SerializeField]
	public CollisionChild collisionChild;

	// Token: 0x0400266B RID: 9835
	[SerializeField]
	public float accelerationTime = 0.3f;

	// Token: 0x0400266C RID: 9836
	[SerializeField]
	public float holdSpeedTime = 2f;

	// Token: 0x0400266D RID: 9837
	[SerializeField]
	public float deccelerationTime = 1f;

	// Token: 0x0400266E RID: 9838
	[SerializeField]
	public float speedupMultiplier = 1.2f;

	// Token: 0x0400266F RID: 9839
	[SerializeField]
	public float tuckDownDelay = 2f;

	// Token: 0x04002670 RID: 9840
	[SerializeField]
	public float gemOffTime = 0.7f;

	// Token: 0x04002671 RID: 9841
	public bool firstSwitch = true;

	// Token: 0x04002672 RID: 9842
	public float timeSinceShot = 1000f;

	// Token: 0x04002673 RID: 9843
	public bool tuckedDown;

	// Token: 0x04002674 RID: 9844
	public bool moveTentacles;

	// Token: 0x04002675 RID: 9845
	public float tentacleOffset = 100f;

	// Token: 0x04002676 RID: 9846
	public float yPosStart;

	// Token: 0x04002677 RID: 9847
	public const float LOCK_DISTANCE = 600f;
}
