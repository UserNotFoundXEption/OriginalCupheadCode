using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000146 RID: 326
public class BaronessLevelBaronessProjectileBunch : AbstractProjectile
{
	// Token: 0x06000F61 RID: 3937 RVA: 0x0008D644 File Offset: 0x0008B844
	public void Init(Vector2 pos, float velocity, float pointAt, LevelProperties.Baroness.BaronessVonBonbon properties, BaronessLevelCastle parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.pointAt = MathUtils.AngleToDirection(pointAt);
		this.velocity = velocity;
		this.parent = parent;
		this.parent.OnDeathEvent += this.KillProjectileBunch;
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x0000D0F9 File Offset: 0x0000B2F9
	public void KillProjectileBunch()
	{
		this.isActive = false;
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x0000D102 File Offset: 0x0000B302
	public override void Awake()
	{
		base.Awake();
		this.isActive = true;
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x0000D111 File Offset: 0x0000B311
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.scale_up_cr());
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x0000D126 File Offset: 0x0000B326
	public override void Update()
	{
		base.Update();
		if (!this.isActive)
		{
			this.Dying();
		}
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x0000D13F File Offset: 0x0000B33F
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		base.transform.position += this.pointAt * this.velocity * CupheadTime.FixedDelta;
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x0008D6A4 File Offset: 0x0008B8A4
	public IEnumerator scale_up_cr()
	{
		float t = 0f;
		float time = 0.3f;
		base.transform.SetScale(new float?(0f), new float?(0f), new float?(0f));
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetScale(new float?(t / time), new float?(t / time), new float?(t / time));
			yield return new WaitForFixedUpdate();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x0000D178 File Offset: 0x0000B378
	public void Dying()
	{
		if (base.GetComponent<SpriteRenderer>() != null)
		{
			base.GetComponent<SpriteRenderer>().enabled = false;
		}
		base.Die();
	}

	// Token: 0x04000C8F RID: 3215
	public LevelProperties.Baroness.BaronessVonBonbon properties;

	// Token: 0x04000C90 RID: 3216
	public BaronessLevelCastle parent;

	// Token: 0x04000C91 RID: 3217
	public float velocity;

	// Token: 0x04000C92 RID: 3218
	public bool isActive;

	// Token: 0x04000C93 RID: 3219
	public Vector3 pointAt;
}
