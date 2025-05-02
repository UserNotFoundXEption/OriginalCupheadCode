using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002BE RID: 702
public class MausoleumLevelGhostBase : BasicProjectile
{
	// Token: 0x170002D1 RID: 721
	// (get) Token: 0x06001F27 RID: 7975 RVA: 0x0001A403 File Offset: 0x00018603
	// (set) Token: 0x06001F28 RID: 7976 RVA: 0x0001A40B File Offset: 0x0001860B
	public bool isDead { get; set; }

	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06001F29 RID: 7977 RVA: 0x0001A414 File Offset: 0x00018614
	public override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x170002D3 RID: 723
	// (get) Token: 0x06001F2A RID: 7978 RVA: 0x0001A41B File Offset: 0x0001861B
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x000B58DC File Offset: 0x000B3ADC
	public override void Start()
	{
		base.Start();
		this.isDead = false;
		if (base.transform.position.x > 0f)
		{
			base.GetComponent<SpriteRenderer>().flipY = true;
		}
		this.SetParryable(true);
		if (this.Counts)
		{
			MausoleumLevel.SPAWNCOUNTER++;
		}
		base.StartCoroutine(this.check_dist_cr());
		if (this.hasIdleSFX)
		{
		}
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x0001A422 File Offset: 0x00018622
	public void GetParent(MausoleumLevel parent)
	{
		this.parent = parent;
	}

	// Token: 0x06001F2D RID: 7981 RVA: 0x0001A42B File Offset: 0x0001862B
	public override void OnParry(AbstractPlayerController player)
	{
		this.Die();
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x0001A433 File Offset: 0x00018633
	public void OnBossDeath()
	{
		this.Die();
	}

	// Token: 0x06001F2F RID: 7983 RVA: 0x0001A43B File Offset: 0x0001863B
	public override void Die()
	{
		this.StopAllCoroutines();
		this.isDead = true;
		base.Die();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x06001F30 RID: 7984 RVA: 0x0001A45C File Offset: 0x0001865C
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06001F31 RID: 7985 RVA: 0x000B5958 File Offset: 0x000B3B58
	public IEnumerator check_dist_cr()
	{
		while (Vector3.Distance(base.transform.position, MausoleumLevelUrn.URN_POS) > 30f)
		{
			yield return null;
		}
		if (this.parent.LoseGame != null)
		{
			this.parent.LoseGame();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001F32 RID: 7986 RVA: 0x000B5974 File Offset: 0x000B3B74
	public IEnumerator idle_sound_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.idleDelay.RandomFloat());
			AudioManager.Play(this.idleSound);
			this.emitAudioFromObject.Add(this.idleSound);
			while (AudioManager.CheckIfPlaying(this.idleSound))
			{
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x0001A464 File Offset: 0x00018664
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x04001971 RID: 6513
	[SerializeField]
	public MinMax idleDelay;

	// Token: 0x04001972 RID: 6514
	[SerializeField]
	public string idleSound;

	// Token: 0x04001973 RID: 6515
	[SerializeField]
	public bool hasIdleSFX;

	// Token: 0x04001974 RID: 6516
	public bool Counts;

	// Token: 0x04001976 RID: 6518
	public const float DIST_TO_DIE = 30f;

	// Token: 0x04001977 RID: 6519
	public MausoleumLevel parent;
}
