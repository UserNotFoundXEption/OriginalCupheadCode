using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000360 RID: 864
public class SallyStagePlayLevelMeteor : AbstractProjectile
{
	// Token: 0x1700031B RID: 795
	// (get) Token: 0x0600261A RID: 9754 RVA: 0x0001FFC9 File Offset: 0x0001E1C9
	// (set) Token: 0x0600261B RID: 9755 RVA: 0x0001FFD1 File Offset: 0x0001E1D1
	public float spawnPosition { get; set; }

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x0600261C RID: 9756 RVA: 0x0001FFDA File Offset: 0x0001E1DA
	// (set) Token: 0x0600261D RID: 9757 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public SallyStagePlayLevelMeteor.State state { get; set; }

	// Token: 0x1700031D RID: 797
	// (get) Token: 0x0600261E RID: 9758 RVA: 0x0001FFEB File Offset: 0x0001E1EB
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x1700031E RID: 798
	// (get) Token: 0x0600261F RID: 9759 RVA: 0x0001FFF2 File Offset: 0x0001E1F2
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002620 RID: 9760 RVA: 0x000C83A4 File Offset: 0x000C65A4
	public SallyStagePlayLevelMeteor Create(float pos, float hp, LevelProperties.SallyStagePlay.Meteor properties)
	{
		SallyStagePlayLevelMeteor sallyStagePlayLevelMeteor = base.Create() as SallyStagePlayLevelMeteor;
		sallyStagePlayLevelMeteor.properties = properties;
		sallyStagePlayLevelMeteor.spawnPosition = pos;
		sallyStagePlayLevelMeteor.hp = hp;
		return sallyStagePlayLevelMeteor;
	}

	// Token: 0x06002621 RID: 9761 RVA: 0x000C83D4 File Offset: 0x000C65D4
	public override void Awake()
	{
		base.Awake();
		this.star.OnActivate += this.ParryStar;
		this.star.GetComponent<Collider2D>().enabled = false;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002622 RID: 9762 RVA: 0x0001FFF9 File Offset: 0x0001E1F9
	public override void Start()
	{
		base.Start();
		base.transform.position = new Vector2(-640f + this.spawnPosition, 360f);
		base.StartCoroutine(this.move_down_cr());
	}

	// Token: 0x06002623 RID: 9763 RVA: 0x00020034 File Offset: 0x0001E234
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002624 RID: 9764 RVA: 0x00020052 File Offset: 0x0001E252
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f && this.state == SallyStagePlayLevelMeteor.State.Meteor)
		{
			this.state = SallyStagePlayLevelMeteor.State.Hook;
			this.OnMeteorDie();
		}
	}

	// Token: 0x06002625 RID: 9765 RVA: 0x0002008F File Offset: 0x0001E28F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.state == SallyStagePlayLevelMeteor.State.Meteor && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002626 RID: 9766 RVA: 0x000C8434 File Offset: 0x000C6634
	public IEnumerator move_down_cr()
	{
		AudioManager.Play("sally_meteor_ascend_decend");
		this.emitAudioFromObject.Add("sally_meteor_ascend_decend");
		this.state = SallyStagePlayLevelMeteor.State.Meteor;
		while (base.transform.position.y > (float)Level.Current.Ground + 100f)
		{
			base.transform.position -= base.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002627 RID: 9767 RVA: 0x000C8450 File Offset: 0x000C6650
	public IEnumerator move_up_cr()
	{
		AudioManager.Play("sally_meteor_ascend_decend");
		this.emitAudioFromObject.Add("sally_meteor_ascend_decend");
		while (this.star.transform.position.y < 360f - this.properties.hookMaxHeight)
		{
			this.star.transform.position += this.star.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002628 RID: 9768 RVA: 0x000C846C File Offset: 0x000C666C
	public IEnumerator leave_cr()
	{
		while (this.star.transform.position.y < 460f)
		{
			this.star.transform.position += this.star.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002629 RID: 9769 RVA: 0x000C8488 File Offset: 0x000C6688
	public IEnumerator leave_all_cr()
	{
		while (base.transform.position.y < 460f)
		{
			base.transform.position += base.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x0600262A RID: 9770 RVA: 0x000C84A4 File Offset: 0x000C66A4
	public IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.hookParryExitDelay);
		base.StartCoroutine(this.leave_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600262B RID: 9771 RVA: 0x000C84C0 File Offset: 0x000C66C0
	public void OnMeteorDie()
	{
		this.star.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<Animator>().SetTrigger("OpenMeteor");
		AudioManager.Play("sally_meteor_open");
		this.emitAudioFromObject.Add("sally_meteor_open");
		this.damageReceiver.enabled = false;
		base.StartCoroutine(this.move_up_cr());
		base.StartCoroutine(this.slide_meteor_cr());
	}

	// Token: 0x0600262C RID: 9772 RVA: 0x000C853C File Offset: 0x000C673C
	public void ParryStar()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player2 != null && !player2.IsDead && !player.IsDead && this.parryCounter < 1)
		{
			this.parryCounter++;
			return;
		}
		base.GetComponent<Animator>().SetTrigger("SpinStar");
		this.state = SallyStagePlayLevelMeteor.State.Leaving;
		base.StartCoroutine(this.leave_cr());
		this.star.StartParryCooldown();
	}

	// Token: 0x0600262D RID: 9773 RVA: 0x000C85C4 File Offset: 0x000C67C4
	public override void Die()
	{
		base.Die();
		this.state = SallyStagePlayLevelMeteor.State.Leaving;
		this.spawnPosition = 0f;
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.enabled = false;
		}
	}

	// Token: 0x0600262E RID: 9774 RVA: 0x000200B8 File Offset: 0x0001E2B8
	public override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		if (hit.GetComponent<SallyStagePlayLevelWave>())
		{
			base.StartCoroutine(this.leave_all_cr());
		}
	}

	// Token: 0x0600262F RID: 9775 RVA: 0x000C8628 File Offset: 0x000C6828
	public IEnumerator slide_meteor_cr()
	{
		float t = 0f;
		float time = 1f;
		Vector3 start = this.meteor.transform.position;
		Vector3 end = new Vector3(this.meteor.transform.position.x, this.meteor.transform.position.y + 700f);
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			this.meteor.transform.position = Vector3.Lerp(start, end, val);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002630 RID: 9776 RVA: 0x000200DF File Offset: 0x0001E2DF
	public void MeteorChangePhase()
	{
		base.StartCoroutine(this.change_phase_cr());
	}

	// Token: 0x06002631 RID: 9777 RVA: 0x000C8644 File Offset: 0x000C6844
	public IEnumerator change_phase_cr()
	{
		AudioManager.Play("sally_meteor_ascend_decend");
		this.emitAudioFromObject.Add("sally_meteor_ascend_decend");
		this.state = SallyStagePlayLevelMeteor.State.Meteor;
		while (base.transform.position.y < (float)Level.Current.Ceiling + 100f)
		{
			base.transform.position += base.transform.up * this.properties.meteorSpeed * CupheadTime.Delta;
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001F8E RID: 8078
	[SerializeField]
	public GameObject meteor;

	// Token: 0x04001F8F RID: 8079
	[SerializeField]
	public ParrySwitch star;

	// Token: 0x04001F91 RID: 8081
	public DamageReceiver damageReceiver;

	// Token: 0x04001F92 RID: 8082
	public LevelProperties.SallyStagePlay.Meteor properties;

	// Token: 0x04001F93 RID: 8083
	public float hp;

	// Token: 0x04001F94 RID: 8084
	public int parryCounter;

	// Token: 0x02000F08 RID: 3848
	public enum State
	{
		// Token: 0x04006C5B RID: 27739
		Meteor,
		// Token: 0x04006C5C RID: 27740
		Hook,
		// Token: 0x04006C5D RID: 27741
		Leaving
	}
}
