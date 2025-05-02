using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200023A RID: 570
public class FlyingBirdLevelTurret : AbstractCollidableObject
{
	// Token: 0x06001A15 RID: 6677 RVA: 0x000A7C10 File Offset: 0x000A5E10
	public FlyingBirdLevelTurret Create(Vector2 pos, FlyingBirdLevelTurret.Properties properties)
	{
		FlyingBirdLevelTurret flyingBirdLevelTurret = this.InstantiatePrefab<FlyingBirdLevelTurret>();
		flyingBirdLevelTurret.transform.position = pos;
		flyingBirdLevelTurret.properties = properties;
		flyingBirdLevelTurret.Init();
		return flyingBirdLevelTurret;
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06001A16 RID: 6678 RVA: 0x0001629F File Offset: 0x0001449F
	// (set) Token: 0x06001A17 RID: 6679 RVA: 0x000162A7 File Offset: 0x000144A7
	public FlyingBirdLevelTurret.State state { get; set; }

	// Token: 0x06001A18 RID: 6680 RVA: 0x000162B0 File Offset: 0x000144B0
	public override void Awake()
	{
		base.Awake();
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
	}

	// Token: 0x06001A19 RID: 6681 RVA: 0x000162E9 File Offset: 0x000144E9
	public void Init()
	{
		this.startPos = base.transform.position;
		base.StartCoroutine(this.go_cr());
		base.StartCoroutine(this.y_cr());
	}

	// Token: 0x06001A1A RID: 6682 RVA: 0x000A7C44 File Offset: 0x000A5E44
	public void Shoot()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		if (next == null || next.transform == null)
		{
			return;
		}
		this.aim.LookAt2D(next.transform);
		BasicProjectile basicProjectile = this.childPrefab.Create(base.transform.position, this.aim.transform.eulerAngles.z, this.properties.bulletSpeed);
		basicProjectile.CollisionDeath.OnlyPlayer();
		basicProjectile.DamagesType.OnlyPlayer();
	}

	// Token: 0x06001A1B RID: 6683 RVA: 0x000A7CDC File Offset: 0x000A5EDC
	public IEnumerator y_cr()
	{
		float start = this.startPos.y + this.properties.floatRange / 2f;
		float end = this.startPos.y - this.properties.floatRange / 2f;
		base.transform.SetPosition(null, new float?(start), null);
		float t = 0f;
		for (;;)
		{
			t = 0f;
			while (t < this.properties.floatTime)
			{
				float val = t / this.properties.floatTime;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			while (t < this.properties.floatTime)
			{
				float val2 = t / this.properties.floatTime;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, end, start, val2)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A1C RID: 6684 RVA: 0x000A7CF8 File Offset: 0x000A5EF8
	public IEnumerator go_cr()
	{
		float t = 0f;
		while (t < this.properties.inTime)
		{
			float val = t / this.properties.inTime;
			base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, this.startPos.x, this.properties.x, val)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetPosition(new float?(this.properties.x), null, null);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.bulletDelay);
			this.Shoot();
		}
		yield break;
	}

	// Token: 0x040014EA RID: 5354
	[SerializeField]
	public BasicProjectile childPrefab;

	// Token: 0x040014EB RID: 5355
	public Vector2 startPos;

	// Token: 0x040014EC RID: 5356
	public FlyingBirdLevelTurret.Properties properties;

	// Token: 0x040014ED RID: 5357
	public Transform aim;

	// Token: 0x02000C68 RID: 3176
	public enum State
	{
		// Token: 0x040059BB RID: 22971
		Alive,
		// Token: 0x040059BC RID: 22972
		Dying,
		// Token: 0x040059BD RID: 22973
		Dead,
		// Token: 0x040059BE RID: 22974
		Respawn
	}

	// Token: 0x02000C69 RID: 3177
	public class Properties
	{
		// Token: 0x06006478 RID: 25720 RVA: 0x00047E0F File Offset: 0x0004600F
		public Properties(float health, float inTime, float x, float bulletSpeed, float bulletDelay, float floatRange, float floatTime)
		{
			this.health = health;
			this.inTime = inTime;
			this.x = x;
			this.bulletSpeed = bulletSpeed;
			this.bulletDelay = bulletDelay;
			this.floatRange = floatRange;
			this.floatTime = floatTime;
		}

		// Token: 0x040059BF RID: 22975
		public readonly float health;

		// Token: 0x040059C0 RID: 22976
		public readonly float inTime;

		// Token: 0x040059C1 RID: 22977
		public readonly float x;

		// Token: 0x040059C2 RID: 22978
		public readonly float bulletSpeed;

		// Token: 0x040059C3 RID: 22979
		public readonly float bulletDelay;

		// Token: 0x040059C4 RID: 22980
		public readonly float floatRange;

		// Token: 0x040059C5 RID: 22981
		public readonly float floatTime;
	}
}
