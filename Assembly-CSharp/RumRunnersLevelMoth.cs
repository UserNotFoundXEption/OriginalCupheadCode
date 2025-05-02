using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200034C RID: 844
public class RumRunnersLevelMoth : AbstractCollidableObject
{
	// Token: 0x060024F5 RID: 9461 RVA: 0x000C5608 File Offset: 0x000C3808
	public void Start()
	{
		this.sparkWarning.SetActive(false);
		if (base.GetComponent<DamageReceiver>())
		{
			this.damageReceiver = base.GetComponent<DamageReceiver>();
			this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
	}

	// Token: 0x060024F6 RID: 9462 RVA: 0x0001F2E3 File Offset: 0x0001D4E3
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060024F7 RID: 9463 RVA: 0x000C5654 File Offset: 0x000C3854
	public void Init(Vector3 pos, LevelProperties.RumRunners.Moth properties, RumRunnersLevelSpider parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.hp = properties.hp;
		this.StartAttack();
		this.parent = parent;
		this.parent.OnDeathEvent += this.Die;
	}

	// Token: 0x060024F8 RID: 9464 RVA: 0x0001F30E File Offset: 0x0001D50E
	public void StartAttack()
	{
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.life_timer_cr());
	}

	// Token: 0x060024F9 RID: 9465 RVA: 0x000C56A4 File Offset: 0x000C38A4
	public IEnumerator move_cr()
	{
		this.goingLeft = Rand.Bool();
		float dist = (!this.goingLeft) ? Mathf.Abs(540f - base.transform.position.x) : Mathf.Abs(-540f - base.transform.position.x);
		float time = dist / this.properties.mothSpeed;
		float t = 0f;
		float start = base.transform.position.x;
		float end = (!this.goingLeft) ? 540f : -540f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(start, end, t / time)), null, null);
			yield return wait;
		}
		dist = Mathf.Abs(-1080f);
		time = dist / this.properties.mothSpeed;
		while (!this.dead)
		{
			t = 0f;
			this.goingLeft = !this.goingLeft;
			start = base.transform.position.x;
			end = ((!this.goingLeft) ? 540f : -540f);
			while (t < time)
			{
				t += CupheadTime.FixedDelta;
				base.transform.SetPosition(new float?(Mathf.Lerp(start, end, t / time)), null, null);
				yield return wait;
			}
			yield return wait;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060024FA RID: 9466 RVA: 0x000C56C0 File Offset: 0x000C38C0
	public IEnumerator shoot_cr()
	{
		while (!this.dead)
		{
			this.sparkWarning.SetActive(false);
			yield return CupheadTime.WaitForSeconds(this, this.properties.mothShootDelay);
			this.sparkWarning.SetActive(true);
			this.regularProjectile.Create(base.transform.position, -90f, this.properties.mothBulletSpeed);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060024FB RID: 9467 RVA: 0x000C56DC File Offset: 0x000C38DC
	public IEnumerator life_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.mothLifetime);
		this.Die();
		yield break;
	}

	// Token: 0x060024FC RID: 9468 RVA: 0x0001F337 File Offset: 0x0001D537
	public void Die()
	{
		this.dead = true;
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060024FD RID: 9469 RVA: 0x0001F351 File Offset: 0x0001D551
	public override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Die;
		base.OnDestroy();
	}

	// Token: 0x04001E99 RID: 7833
	public const float RIGHT_X = 540f;

	// Token: 0x04001E9A RID: 7834
	public const float LEFT_X = -540f;

	// Token: 0x04001E9B RID: 7835
	[SerializeField]
	public GameObject sparkWarning;

	// Token: 0x04001E9C RID: 7836
	[SerializeField]
	public BasicProjectile regularProjectile;

	// Token: 0x04001E9D RID: 7837
	public RumRunnersLevelSpider parent;

	// Token: 0x04001E9E RID: 7838
	public LevelProperties.RumRunners.Moth properties;

	// Token: 0x04001E9F RID: 7839
	public DamageReceiver damageReceiver;

	// Token: 0x04001EA0 RID: 7840
	public float hp;

	// Token: 0x04001EA1 RID: 7841
	public bool goingLeft;

	// Token: 0x04001EA2 RID: 7842
	public bool dead;
}
