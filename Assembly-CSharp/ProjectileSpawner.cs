using System;
using UnityEngine;

// Token: 0x02000595 RID: 1429
public class ProjectileSpawner : AbstractPausableComponent
{
	// Token: 0x06003C54 RID: 15444 RVA: 0x0011546C File Offset: 0x0011366C
	public void Start()
	{
		Level.Current.OnLevelStartEvent += this.OnStart;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x00030CBE File Offset: 0x0002EEBE
	public void OnStart()
	{
		this.started = true;
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x00030CC7 File Offset: 0x0002EEC7
	public void OnStop()
	{
		this.started = false;
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x001154C0 File Offset: 0x001136C0
	public void Update()
	{
		if (!this.started)
		{
			return;
		}
		if (Level.Current == null || PlayerManager.Count < 1)
		{
			return;
		}
		if (this.projectilePrefab == null)
		{
			return;
		}
		if (this.timer >= this.delay)
		{
			if (this.type == ProjectileSpawner.Type.Aimed)
			{
				this.aim.LookAt2D(PlayerManager.GetNext().transform);
				this.angle = this.aim.transform.eulerAngles.z;
			}
			BasicProjectile basicProjectile = this.projectilePrefab.Create(base.transform.position, this.angle, this.speed);
			if (this.parryable)
			{
				basicProjectile.SetParryable(this.parryable);
			}
			if (this.stoneTime > 0f)
			{
				basicProjectile.SetStoneTime(this.stoneTime);
			}
			this.timer = 0f;
		}
		else
		{
			this.timer += CupheadTime.Delta;
		}
	}

	// Token: 0x04002FDC RID: 12252
	public ProjectileSpawner.Type type;

	// Token: 0x04002FDD RID: 12253
	public float delay = 1f;

	// Token: 0x04002FDE RID: 12254
	public float speed = 500f;

	// Token: 0x04002FDF RID: 12255
	public bool parryable;

	// Token: 0x04002FE0 RID: 12256
	[Space(10f)]
	public float stoneTime;

	// Token: 0x04002FE1 RID: 12257
	[Space(10f)]
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x04002FE2 RID: 12258
	public float angle;

	// Token: 0x04002FE3 RID: 12259
	public float timer;

	// Token: 0x04002FE4 RID: 12260
	public bool started;

	// Token: 0x04002FE5 RID: 12261
	public Transform aim;

	// Token: 0x02001215 RID: 4629
	public enum Type
	{
		// Token: 0x04007D74 RID: 32116
		Straight,
		// Token: 0x04007D75 RID: 32117
		Aimed
	}
}
