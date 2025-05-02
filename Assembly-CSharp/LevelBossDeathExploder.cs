using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000111 RID: 273
public class LevelBossDeathExploder : AbstractMonoBehaviour
{
	// Token: 0x06000D3E RID: 3390 RVA: 0x0000B5CE File Offset: 0x000097CE
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000D3F RID: 3391 RVA: 0x00086A78 File Offset: 0x00084C78
	public virtual void Start()
	{
		if (this.ExplosionPrefabOverride)
		{
			this.effectPrefab = this.ExplosionPrefabOverride;
		}
		else
		{
			this.effectPrefab = Level.Current.LevelResources.levelBossDeathExplosion;
		}
		Level.Current.OnBossDeathExplosionsEvent += this.StartExplosion;
		Level.Current.OnBossDeathExplosionsFalloffEvent += this.OnExplosionsRand;
		Level.Current.OnBossDeathExplosionsEndEvent += this.StopExplosions;
	}

	// Token: 0x06000D40 RID: 3392 RVA: 0x00086B00 File Offset: 0x00084D00
	public void OnDestroy()
	{
		this.ExplosionPrefabOverride = null;
		this.effectPrefab = null;
		try
		{
			Level.Current.OnBossDeathExplosionsEvent -= this.StartExplosion;
		}
		catch
		{
		}
		try
		{
			Level.Current.OnBossDeathExplosionsFalloffEvent -= this.OnExplosionsRand;
		}
		catch
		{
		}
		try
		{
			Level.Current.OnBossDeathExplosionsEndEvent -= this.StopExplosions;
		}
		catch
		{
		}
	}

	// Token: 0x06000D41 RID: 3393 RVA: 0x0000B5D6 File Offset: 0x000097D6
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.offset, this.radius);
	}

	// Token: 0x06000D42 RID: 3394 RVA: 0x0000B613 File Offset: 0x00009813
	public void StartExplosion()
	{
		this.StartExplosion(false);
	}

	// Token: 0x06000D43 RID: 3395 RVA: 0x0000B61C File Offset: 0x0000981C
	public void StartExplosion(bool bypassCameraShakeEvent)
	{
		if (this == null || !base.enabled || !base.isActiveAndEnabled)
		{
			return;
		}
		base.StartCoroutine(this.go_cr(bypassCameraShakeEvent));
	}

	// Token: 0x06000D44 RID: 3396 RVA: 0x0000B64F File Offset: 0x0000984F
	public void OnExplosionsRand()
	{
		this.state = LevelBossDeathExploder.State.Random;
	}

	// Token: 0x06000D45 RID: 3397 RVA: 0x0000B658 File Offset: 0x00009858
	public void StopExplosions()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x06000D46 RID: 3398 RVA: 0x00086BA8 File Offset: 0x00084DA8
	public Vector2 GetRandomPoint()
	{
		Vector2 vector = base.transform.position + this.offset;
		Vector2 vector2;
		vector2..ctor((float)Random.Range(-1, 1), (float)Random.Range(-1, 1));
		Vector2 vector3 = vector2.normalized * (this.radius * Random.value) * 2f;
		vector3.x *= this.scaleFactor.x;
		vector3.y *= this.scaleFactor.y;
		return vector + vector3;
	}

	// Token: 0x06000D47 RID: 3399 RVA: 0x00086C44 File Offset: 0x00084E44
	public IEnumerator go_cr(bool bypassCameraShakeEvent)
	{
		HitFlash flash = base.GetComponent<HitFlash>();
		if (!this.disableSound)
		{
			AudioManager.Play("level_explosion_boss_death");
		}
		for (;;)
		{
			this.effectPrefab.Create(this.GetRandomPoint());
			if (flash != null)
			{
				flash.Flash(0.1f);
			}
			CupheadLevelCamera.Current.Shake(10f, 0.4f, bypassCameraShakeEvent);
			LevelBossDeathExploder.State state = this.state;
			if (state != LevelBossDeathExploder.State.Random)
			{
				yield return CupheadTime.WaitForSeconds(this, this.STEADY_DELAY);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, Random.Range(this.MIN_DELAY, this.MAX_DELAY));
			}
		}
		yield break;
	}

	// Token: 0x04000A5A RID: 2650
	public Effect ExplosionPrefabOverride;

	// Token: 0x04000A5B RID: 2651
	[SerializeField]
	public float STEADY_DELAY = 0.3f;

	// Token: 0x04000A5C RID: 2652
	[SerializeField]
	public float MIN_DELAY = 0.4f;

	// Token: 0x04000A5D RID: 2653
	[SerializeField]
	public float MAX_DELAY = 1f;

	// Token: 0x04000A5E RID: 2654
	public Vector2 offset = Vector2.zero;

	// Token: 0x04000A5F RID: 2655
	[SerializeField]
	public float radius = 100f;

	// Token: 0x04000A60 RID: 2656
	[SerializeField]
	public Vector2 scaleFactor = new Vector2(1f, 1f);

	// Token: 0x04000A61 RID: 2657
	public LevelBossDeathExploder.State state;

	// Token: 0x04000A62 RID: 2658
	public Effect effectPrefab;

	// Token: 0x04000A63 RID: 2659
	[SerializeField]
	public bool disableSound;

	// Token: 0x0200099B RID: 2459
	public enum State
	{
		// Token: 0x04004793 RID: 18323
		Steady,
		// Token: 0x04004794 RID: 18324
		Random
	}
}
