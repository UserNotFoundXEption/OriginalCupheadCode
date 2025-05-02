using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200027B RID: 635
public class FlyingGenieLevelTinyMarionette : AbstractCollidableObject
{
	// Token: 0x06001CF7 RID: 7415 RVA: 0x000AF7A4 File Offset: 0x000AD9A4
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver.enabled = false;
	}

	// Token: 0x06001CF8 RID: 7416 RVA: 0x00018888 File Offset: 0x00016A88
	public void FixedUpdate()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x000188A0 File Offset: 0x00016AA0
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && !this.isDead)
		{
			this.isDead = true;
			this.Die();
		}
	}

	// Token: 0x06001CFA RID: 7418 RVA: 0x000188DD File Offset: 0x00016ADD
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CFB RID: 7419 RVA: 0x000188FB File Offset: 0x00016AFB
	public void Activate(Vector3 endPos, LevelProperties.FlyingGenie.Scan properties, bool movingClockwise)
	{
		this.properties = properties;
		this.hp = properties.miniHP;
		this.isClockwise = movingClockwise;
		base.StartCoroutine(this.tiny_marionette(endPos));
	}

	// Token: 0x06001CFC RID: 7420 RVA: 0x000AF7F4 File Offset: 0x000AD9F4
	public IEnumerator bounce_marionette_cr()
	{
		float t = 0f;
		float time = 0.5f;
		float start = base.transform.position.y;
		float end = base.transform.position.y + 100f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutBounce, 0f, 1f, t / time);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CFD RID: 7421 RVA: 0x000AF810 File Offset: 0x000ADA10
	public IEnumerator tiny_marionette(Vector3 endPos)
	{
		base.StartCoroutine(this.bounce_marionette_cr());
		yield return base.animator.WaitForAnimationToEnd(this, "Puppet_Intro", false, true);
		this.damageReceiver.enabled = true;
		float t = 0f;
		float time = this.properties.movementSpeed;
		Vector3 start = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			base.transform.position = Vector3.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return new WaitForFixedUpdate();
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.startedDown = Rand.Bool();
		this.turningDown = !this.startedDown;
		base.animator.SetBool("OnTurningDown", this.turningDown);
		string dirString = (!this.startedDown) ? "Up_" : "Down_";
		base.animator.SetTrigger("OnStartCycle");
		base.animator.SetBool("IsDown", this.startedDown);
		this.bulletMainIndex = Random.Range(0, this.properties.bulletString.Length);
		string[] bulletString = this.properties.bulletString[this.bulletMainIndex].Split(new char[]
		{
			','
		});
		this.bulletIndex = Random.Range(0, bulletString.Length);
		yield return base.animator.WaitForAnimationToEnd(this, dirString + "Warning_Shoot", false, true);
		for (;;)
		{
			bulletString = this.properties.bulletString[this.bulletMainIndex].Split(new char[]
			{
				','
			});
			yield return CupheadTime.WaitForSeconds(this, this.properties.shootDelay);
			base.animator.SetTrigger("OnShoot");
			yield return base.animator.WaitForAnimationToEnd(this, false);
			if (this.bulletIndex < bulletString.Length - 1)
			{
				this.bulletIndex++;
			}
			else
			{
				this.bulletMainIndex = (this.bulletMainIndex + 1) % this.properties.bulletString.Length;
				this.bulletIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x000AF834 File Offset: 0x000ADA34
	public void ShootBullet(Vector3 pos, float rotation)
	{
		string[] array = this.properties.bulletString[this.bulletMainIndex].Split(new char[]
		{
			','
		});
		if (array[this.bulletIndex][0] == 'P')
		{
			this.pinkProjectile.Create(pos, rotation + 90f, this.properties.bulletSpeed);
		}
		else
		{
			this.projectile.Create(pos, rotation + 90f, this.properties.bulletSpeed);
		}
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x000AF8C8 File Offset: 0x000ADAC8
	public void AniEventCheckFlip()
	{
		if (this.hasStarted)
		{
			base.transform.SetScale(new float?(base.transform.localScale.x * -1f), null, null);
			this.turningDown = !this.turningDown;
			base.animator.SetBool("OnTurningDown", this.turningDown);
		}
		else
		{
			this.hasStarted = true;
			if (!this.isClockwise && !this.startedDown)
			{
				base.transform.SetScale(new float?(base.transform.localScale.x * -1f), null, null);
			}
			else if (this.isClockwise && this.startedDown)
			{
				base.transform.SetScale(new float?(base.transform.localScale.x * -1f), null, null);
			}
		}
	}

	// Token: 0x06001D00 RID: 7424 RVA: 0x000AF9F4 File Offset: 0x000ADBF4
	public void AniEventShoot()
	{
		Effect effect = this.shootFX.Create(this.shootRoot.transform.position);
		AudioManager.Play("genie_puppetsmall_shoot");
		this.emitAudioFromObject.Add("genie_puppetsmall_shoot");
		effect.transform.SetEulerAngles(null, null, new float?(this.shootRoot.transform.eulerAngles.z));
		this.ShootBullet(this.shootRoot.transform.position, this.shootRoot.transform.eulerAngles.z);
	}

	// Token: 0x06001D01 RID: 7425 RVA: 0x00018925 File Offset: 0x00016B25
	public void Die()
	{
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("genie_puppetsmall_death");
		this.emitAudioFromObject.Add("genie_puppetsmall_death");
		this.StopAllCoroutines();
		base.StartCoroutine(this.dead_move_cr());
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x000AFAA0 File Offset: 0x000ADCA0
	public IEnumerator dead_move_cr()
	{
		float t = 0f;
		float timer = 0.5f;
		float downTimer = 0.5f;
		float start = base.transform.position.y;
		float end = 660f;
		float downEnd = base.transform.position.y - 50f;
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		yield return base.animator.WaitForAnimationToEnd(this, "Death_Start", false, true);
		while (t < downTimer)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / downTimer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, downEnd, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		start = base.transform.position.y;
		while (t < timer)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / timer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, val2)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x00018964 File Offset: 0x00016B64
	public void SoundPuppetSmallEnterPuppet()
	{
		AudioManager.Play("genie_puppetsmall_enter_puppetsmall");
		this.emitAudioFromObject.Add("genie_puppetsmall_enter_puppetsmall");
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x00018980 File Offset: 0x00016B80
	public void SoundPuppetSmallDance()
	{
		AudioManager.Play("genie_puppetsmall_move");
		this.emitAudioFromObject.Add("genie_puppetsmall_move");
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x0001899C File Offset: 0x00016B9C
	public void SoundPuppetShootWarning()
	{
		AudioManager.Play("genie_puppetsmall_shootwarning");
		this.emitAudioFromObject.Add("genie_puppetsmall_shootwarning");
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x000189B8 File Offset: 0x00016BB8
	public void SoundPuppetWarningShot()
	{
		AudioManager.Play("genie_puppetsmall_warningshot");
		this.emitAudioFromObject.Add("genie_puppetsmall_warningshot");
	}

	// Token: 0x04001795 RID: 6037
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x04001796 RID: 6038
	[SerializeField]
	public BasicProjectile pinkProjectile;

	// Token: 0x04001797 RID: 6039
	[SerializeField]
	public Effect shootFX;

	// Token: 0x04001798 RID: 6040
	[SerializeField]
	public Transform shootRoot;

	// Token: 0x04001799 RID: 6041
	public DamageDealer damageDealer;

	// Token: 0x0400179A RID: 6042
	public DamageReceiver damageReceiver;

	// Token: 0x0400179B RID: 6043
	public float hp;

	// Token: 0x0400179C RID: 6044
	public bool turningDown;

	// Token: 0x0400179D RID: 6045
	public bool isClockwise;

	// Token: 0x0400179E RID: 6046
	public bool startedDown;

	// Token: 0x0400179F RID: 6047
	public bool hasStarted;

	// Token: 0x040017A0 RID: 6048
	public bool isDead;

	// Token: 0x040017A1 RID: 6049
	public int bulletMainIndex;

	// Token: 0x040017A2 RID: 6050
	public int bulletIndex;

	// Token: 0x040017A3 RID: 6051
	public LevelProperties.FlyingGenie.Scan properties;
}
