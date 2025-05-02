using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020002F8 RID: 760
public class PirateLevelSquidInkOverlay : LevelProperties.Pirate.Entity
{
	// Token: 0x170002E8 RID: 744
	// (get) Token: 0x060021D0 RID: 8656 RVA: 0x0001CE94 File Offset: 0x0001B094
	// (set) Token: 0x060021D1 RID: 8657 RVA: 0x0001CE9B File Offset: 0x0001B09B
	public static PirateLevelSquidInkOverlay Current { get; set; }

	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060021D2 RID: 8658 RVA: 0x000BB5E0 File Offset: 0x000B97E0
	// (set) Token: 0x060021D3 RID: 8659 RVA: 0x0001CEA3 File Offset: 0x0001B0A3
	public float alpha
	{
		get
		{
			return this.spriteRenderer.color.a;
		}
		set
		{
			this.color.a = Mathf.Clamp(value, 0f, 1f);
			this.spriteRenderer.color = this.color;
		}
	}

	// Token: 0x060021D4 RID: 8660 RVA: 0x000BB600 File Offset: 0x000B9800
	public override void Awake()
	{
		base.Awake();
		PirateLevelSquidInkOverlay.Current = this;
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.spriteRenderer.enabled = false;
		this.alpha = 0f;
		this.color = this.spriteRenderer.color;
		this.splatGroups = new List<PirateLevelSquidInkOverlay.SplatGroup>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.name.ToLower().Contains("group"))
				{
					this.splatGroups.Add(new PirateLevelSquidInkOverlay.SplatGroup(transform));
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x060021D5 RID: 8661 RVA: 0x0001CED1 File Offset: 0x0001B0D1
	public override void OnDestroy()
	{
		base.OnDestroy();
		PirateLevelSquidInkOverlay.Current = null;
		this.smallSplat = null;
		this.largeSplat = null;
	}

	// Token: 0x060021D6 RID: 8662 RVA: 0x000BB6D4 File Offset: 0x000B98D4
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		IEnumerator enumerator = base.baseTransform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				if (transform.gameObject.activeInHierarchy)
				{
					IEnumerator enumerator2 = transform.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							Transform transform2 = (Transform)obj2;
							if (transform2.name.ToLower().Contains("small"))
							{
								Gizmos.DrawWireSphere(transform2.position, 20f);
							}
							else if (transform2.name.ToLower().Contains("large"))
							{
								Gizmos.DrawWireSphere(transform2.position, 40f);
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator2 as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
	}

	// Token: 0x060021D7 RID: 8663 RVA: 0x0001CEED File Offset: 0x0001B0ED
	public override void LevelInit(LevelProperties.Pirate properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x060021D8 RID: 8664 RVA: 0x000BB7F0 File Offset: 0x000B99F0
	public void Hit()
	{
		if (!this.SFXSplatScreenActive)
		{
			AudioManager.Play("level_pirate_squid_blackout_screen");
			this.SFXSplatScreenActive = true;
		}
		LevelProperties.Pirate.Squid squid = base.properties.CurrentState.squid;
		this.spriteRenderer.enabled = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.splats_cr());
		base.StartCoroutine(this.hit_cr(squid));
	}

	// Token: 0x060021D9 RID: 8665 RVA: 0x000BB858 File Offset: 0x000B9A58
	public IEnumerator splats_cr()
	{
		PirateLevelSquidInkOverlay.SplatGroup group = this.splatGroups[Random.Range(0, this.splatGroups.Count)];
		group.RandomizeDelay(10);
		for (int i = 0; i < 10; i++)
		{
			foreach (PirateLevelSquidInkOverlay.SplatGroup.Splat splat in group.splats)
			{
				if (splat.delay == i)
				{
					Vector3 position = splat.position;
					if (splat.type == PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Large)
					{
						this.largeSplat.Create(position);
					}
					else
					{
						this.smallSplat.Create(position);
					}
				}
			}
			yield return CupheadTime.WaitForSeconds(this, 0.025f);
		}
		yield break;
	}

	// Token: 0x060021DA RID: 8666 RVA: 0x000BB874 File Offset: 0x000B9A74
	public IEnumerator hit_cr(LevelProperties.Pirate.Squid p)
	{
		if (!this.SFXSplatScreenActive)
		{
			AudioManager.Play("level_pirate_squid_blackout_screen");
			this.SFXSplatScreenActive = true;
		}
		this.targetAlpha = Mathf.Clamp(this.targetAlpha + p.opacityAdd, 0f, 1f);
		float t = 0f;
		while (t < p.opacityAddTime)
		{
			float val = t / p.opacityAddTime;
			this.alpha = Mathf.Lerp(this.alpha, this.targetAlpha, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, p.darkHoldTime);
		yield return base.StartCoroutine(this.fade_cr(p));
		yield break;
	}

	// Token: 0x060021DB RID: 8667 RVA: 0x000BB898 File Offset: 0x000B9A98
	public IEnumerator fade_cr(LevelProperties.Pirate.Squid p)
	{
		float t = 0f;
		while (t < p.darkFadeTime)
		{
			float val = t / p.darkFadeTime;
			this.alpha = Mathf.Lerp(this.alpha, 0f, val);
			this.targetAlpha = this.alpha;
			t += CupheadTime.Delta;
			yield return null;
		}
		this.alpha = 0f;
		this.targetAlpha = this.alpha;
		this.spriteRenderer.enabled = false;
		this.SFXSplatScreenActive = false;
		yield break;
	}

	// Token: 0x04001BD8 RID: 7128
	public const int DELAY_MAX = 10;

	// Token: 0x04001BD9 RID: 7129
	public const float DELAY_WAIT = 0.025f;

	// Token: 0x04001BDB RID: 7131
	[SerializeField]
	public Effect largeSplat;

	// Token: 0x04001BDC RID: 7132
	[SerializeField]
	public Effect smallSplat;

	// Token: 0x04001BDD RID: 7133
	public SpriteRenderer spriteRenderer;

	// Token: 0x04001BDE RID: 7134
	public List<PirateLevelSquidInkOverlay.SplatGroup> splatGroups;

	// Token: 0x04001BDF RID: 7135
	public bool SFXSplatScreenActive;

	// Token: 0x04001BE0 RID: 7136
	public Color color;

	// Token: 0x04001BE1 RID: 7137
	public float targetAlpha;

	// Token: 0x02000E1A RID: 3610
	public class SplatGroup
	{
		// Token: 0x06006D41 RID: 27969 RVA: 0x0023EA8C File Offset: 0x0023CC8C
		public SplatGroup(Transform parent)
		{
			this.splats = new List<PirateLevelSquidInkOverlay.SplatGroup.Splat>();
			IEnumerator enumerator = parent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					this.splats.Add(new PirateLevelSquidInkOverlay.SplatGroup.Splat(transform));
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06006D42 RID: 27970 RVA: 0x0023EB08 File Offset: 0x0023CD08
		public void RandomizeDelay(int max)
		{
			foreach (PirateLevelSquidInkOverlay.SplatGroup.Splat splat in this.splats)
			{
				splat.delay = Random.Range(0, max);
			}
		}

		// Token: 0x04006611 RID: 26129
		public List<PirateLevelSquidInkOverlay.SplatGroup.Splat> splats;

		// Token: 0x020015D9 RID: 5593
		public class Splat
		{
			// Token: 0x06008761 RID: 34657 RVA: 0x002A50CC File Offset: 0x002A32CC
			public Splat(Transform transform)
			{
				this.position = transform.position;
				if (transform.name.ToLower().Contains("small"))
				{
					this.type = PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Small;
				}
				else
				{
					this.type = PirateLevelSquidInkOverlay.SplatGroup.Splat.Type.Large;
				}
			}

			// Token: 0x040091CA RID: 37322
			public readonly PirateLevelSquidInkOverlay.SplatGroup.Splat.Type type;

			// Token: 0x040091CB RID: 37323
			public readonly Vector2 position;

			// Token: 0x040091CC RID: 37324
			public int delay;

			// Token: 0x02001609 RID: 5641
			public enum Type
			{
				// Token: 0x04009296 RID: 37526
				Small,
				// Token: 0x04009297 RID: 37527
				Large
			}
		}
	}
}
