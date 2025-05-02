using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003C9 RID: 969
public class VeggiesLevelCarrot : LevelProperties.Veggies.Entity
{
	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06002AA0 RID: 10912 RVA: 0x00023D96 File Offset: 0x00021F96
	// (set) Token: 0x06002AA1 RID: 10913 RVA: 0x00023D9E File Offset: 0x00021F9E
	public VeggiesLevelCarrot.State state { get; set; }

	// Token: 0x1400005B RID: 91
	// (add) Token: 0x06002AA2 RID: 10914 RVA: 0x000D4778 File Offset: 0x000D2978
	// (remove) Token: 0x06002AA3 RID: 10915 RVA: 0x000D47B0 File Offset: 0x000D29B0
	public event Action OnDeathEvent;

	// Token: 0x1400005C RID: 92
	// (add) Token: 0x06002AA4 RID: 10916 RVA: 0x000D47E8 File Offset: 0x000D29E8
	// (remove) Token: 0x06002AA5 RID: 10917 RVA: 0x000D4820 File Offset: 0x000D2A20
	public event VeggiesLevelCarrot.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x06002AA6 RID: 10918 RVA: 0x000D4858 File Offset: 0x000D2A58
	public void Start()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.mindLoop = Object.Instantiate<AudioSource>(this.mindLoopPrefab);
		List<Transform> list = new List<Transform>(this.homingRoot.GetComponentsInChildren<Transform>());
		list.Remove(this.homingRoot);
		this.homingRoots = list.ToArray();
		this.SfxGround();
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x00023DA7 File Offset: 0x00021FA7
	public void Update()
	{
		if (PauseManager.state == PauseManager.State.Paused)
		{
			this.mindLoop.Pause();
		}
		else
		{
			this.mindLoop.UnPause();
		}
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x00023DCF File Offset: 0x00021FCF
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.mindLoop.Stop();
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x000D48B4 File Offset: 0x000D2AB4
	public override void LevelInit(LevelProperties.Veggies properties)
	{
		base.LevelInit(properties);
		this.carrot = base.properties.CurrentState.carrot;
		this.hp = (float)this.carrot.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x000D4908 File Offset: 0x000D2B08
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
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x00023DE2 File Offset: 0x00021FE2
	public void OnInAnimComplete()
	{
		base.transform.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.rings_cr());
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x00023E02 File Offset: 0x00022002
	public void Die()
	{
		this.dead = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x00023E1E File Offset: 0x0002201E
	public void SfxGround()
	{
		AudioManager.Play("level_veggies_carrot_rise");
	}

	// Token: 0x06002AAE RID: 10926 RVA: 0x000D4968 File Offset: 0x000D2B68
	public void ShootRegular()
	{
		this.spark.Create(this.straightRoot.position);
		this.straightRoot.LookAt2D(PlayerManager.GetNext().center);
		this.straightPrefab.Create(this, this.straightRoot.position, this.carrot.bulletSpeed, this.straightRoot.eulerAngles.z);
	}

	// Token: 0x06002AAF RID: 10927 RVA: 0x00023E2A File Offset: 0x0002202A
	public void ShootHoming()
	{
		this.homingPrefab.Create(PlayerManager.GetNext(), this, this.GetHomingRoot(), this.carrot.homingSpeed, this.carrot.homingRotation, (float)this.carrot.homingHP);
	}

	// Token: 0x06002AB0 RID: 10928 RVA: 0x000D49DC File Offset: 0x000D2BDC
	public Vector2 GetHomingRoot()
	{
		Vector3 position = this.homingRoots[Random.Range(0, this.homingRoots.Length)].position;
		this.homingRoot.SetScale(new float?(this.homingRoot.localScale.x * -1f), null, null);
		return position;
	}

	// Token: 0x06002AB1 RID: 10929 RVA: 0x000D4A48 File Offset: 0x000D2C48
	public IEnumerator rings_cr()
	{
		for (;;)
		{
			base.StartCoroutine(this.carrot_cr());
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.carrot.idleRange.RandomFloat());
			int count = 0;
			base.animator.SetTrigger("AttackStart");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Start", false, true);
			while (count < this.carrot.bulletCount)
			{
				yield return CupheadTime.WaitForSeconds(this, this.carrot.bulletDelay * 0.5f);
				this.ringEffectPrefab.Create(this.straightRoot.position);
				yield return CupheadTime.WaitForSeconds(this, this.carrot.bulletDelay * 0.5f);
				this.straightRoot.LookAt2D(PlayerManager.GetNext().center);
				for (int i = 0; i < 5; i++)
				{
					AudioManager.Play("level_veggies_carrot_beam");
					this.ringPrefab.Create(this.straightRoot.position, this.straightRoot.eulerAngles.z, this.carrot.bulletSpeed);
					yield return CupheadTime.WaitForSeconds(this, 0.1f);
				}
				count++;
			}
			base.animator.SetTrigger("AttackEnd");
		}
		yield break;
	}

	// Token: 0x06002AB2 RID: 10930 RVA: 0x000D4A64 File Offset: 0x000D2C64
	public IEnumerator carrot_cr()
	{
		int bgCount = 0;
		LevelProperties.Veggies.Carrot p = base.properties.CurrentState.carrot;
		AudioManager.Play("level_veggies_mindmeld_start");
		this.mindLoop.Play();
		yield return CupheadTime.WaitForSeconds(this, this.carrot.startIdleTime);
		bool side = false;
		bgCount = 0;
		int numOfCarrots = p.homingNumOfCarrots.RandomInt();
		while (bgCount < numOfCarrots)
		{
			this.bgPrefab.Create((!side) ? -1 : 1, this.carrot.homingBgSpeed, this);
			side = !side;
			bgCount++;
			yield return CupheadTime.WaitForSeconds(this, this.carrot.homingDelay);
		}
		yield break;
	}

	// Token: 0x06002AB3 RID: 10931 RVA: 0x000D4A80 File Offset: 0x000D2C80
	public IEnumerator die_cr()
	{
		this.mindLoop.Stop();
		AudioManager.Play("level_veggies_carrot_die");
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("Dead");
		yield return null;
		base.properties.WinInstantly();
		yield break;
	}

	// Token: 0x0400238B RID: 9099
	[SerializeField]
	public AudioSource mindLoopPrefab;

	// Token: 0x0400238C RID: 9100
	public AudioSource mindLoop;

	// Token: 0x0400238D RID: 9101
	[SerializeField]
	public Transform homingRoot;

	// Token: 0x0400238E RID: 9102
	[SerializeField]
	public Transform straightRoot;

	// Token: 0x0400238F RID: 9103
	[SerializeField]
	public VeggiesLevelCarrotHomingProjectile homingPrefab;

	// Token: 0x04002390 RID: 9104
	[SerializeField]
	public VeggiesLevelCarrotRegularProjectile straightPrefab;

	// Token: 0x04002391 RID: 9105
	[SerializeField]
	public BasicProjectile ringPrefab;

	// Token: 0x04002392 RID: 9106
	[SerializeField]
	public Effect ringEffectPrefab;

	// Token: 0x04002393 RID: 9107
	[SerializeField]
	public VeggiesLevelCarrotBgCarrot bgPrefab;

	// Token: 0x04002394 RID: 9108
	[SerializeField]
	public Effect spark;

	// Token: 0x04002395 RID: 9109
	public LevelProperties.Veggies.Carrot carrot;

	// Token: 0x04002396 RID: 9110
	public Transform[] homingRoots;

	// Token: 0x04002397 RID: 9111
	public bool dead;

	// Token: 0x04002398 RID: 9112
	public float hp;

	// Token: 0x04002399 RID: 9113
	public IEnumerator floatingCoroutine;

	// Token: 0x02000FDC RID: 4060
	public enum Direction
	{
		// Token: 0x040071EB RID: 29163
		Down,
		// Token: 0x040071EC RID: 29164
		DownLeft,
		// Token: 0x040071ED RID: 29165
		DownRight
	}

	// Token: 0x02000FDD RID: 4061
	public enum State
	{
		// Token: 0x040071EF RID: 29167
		Start,
		// Token: 0x040071F0 RID: 29168
		Complete
	}

	// Token: 0x02000FDE RID: 4062
	// (Invoke) Token: 0x06007669 RID: 30313
	public delegate void OnAttackHandler(VeggiesLevelCarrot.Direction direction);

	// Token: 0x02000FDF RID: 4063
	// (Invoke) Token: 0x0600766D RID: 30317
	public delegate void OnDamageTakenHandler(float damage);
}
