using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003EF RID: 1007
public class TreePlatformingLevelBeetle : PlatformingLevelPathMovementEnemy
{
	// Token: 0x17000350 RID: 848
	// (get) Token: 0x06002C3D RID: 11325 RVA: 0x000250F9 File Offset: 0x000232F9
	// (set) Token: 0x06002C3E RID: 11326 RVA: 0x00025101 File Offset: 0x00023301
	public bool isActivated { get; set; }

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x06002C3F RID: 11327 RVA: 0x0002510A File Offset: 0x0002330A
	// (set) Token: 0x06002C40 RID: 11328 RVA: 0x00025112 File Offset: 0x00023312
	public bool onCamera { get; set; }

	// Token: 0x06002C41 RID: 11329 RVA: 0x0002511B File Offset: 0x0002331B
	public override void Awake()
	{
		base.Awake();
		this.isActivated = false;
	}

	// Token: 0x06002C42 RID: 11330 RVA: 0x0002512A File Offset: 0x0002332A
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.check_hit_box_cr());
		if (this.sprite != null)
		{
			base.StartCoroutine(this.motion_cr());
		}
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x000D9614 File Offset: 0x000D7814
	public override void Die()
	{
		if (this.explosion != null)
		{
			this.explosion.Create(base.transform.position);
		}
		this.hasStarted = false;
		this.onCamera = false;
		AudioManager.Stop("level_platform_beetle_idle_loop");
		AudioManager.Play("level_platform_beetle_death");
		this.emitAudioFromObject.Add("level_platform_beetle_death");
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x0002515D File Offset: 0x0002335D
	public void Activate()
	{
		this.isActivated = true;
		this.PrepareBeetle();
	}

	// Token: 0x06002C45 RID: 11333 RVA: 0x0002516C File Offset: 0x0002336C
	public void Deactivate()
	{
		this.isActivated = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.ResetStartingCondition();
	}

	// Token: 0x06002C46 RID: 11334 RVA: 0x000D9688 File Offset: 0x000D7888
	public void PrepareBeetle()
	{
		this.pathIndex = 1;
		this.startPosition = base.allValues[0];
		base.StartFromCustom();
		base.StartCoroutine(this.check_for_death_cr());
		if ((this.pathIndex - 1) % 2 != 0)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		else
		{
			base.transform.SetScale(new float?(1f), null, null);
		}
	}

	// Token: 0x06002C47 RID: 11335 RVA: 0x00025187 File Offset: 0x00023387
	public override void OnStart()
	{
		base.OnStart();
		base.GetComponent<SpriteRenderer>().enabled = true;
	}

	// Token: 0x06002C48 RID: 11336 RVA: 0x0002519B File Offset: 0x0002339B
	public override void EndPath()
	{
		base.EndPath();
		this.Deactivate();
	}

	// Token: 0x06002C49 RID: 11337 RVA: 0x000D9720 File Offset: 0x000D7920
	public void Flip()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(base.transform.localScale.y), new float?(base.transform.localScale.z));
	}

	// Token: 0x06002C4A RID: 11338 RVA: 0x000251A9 File Offset: 0x000233A9
	public void PlayIdleSFX()
	{
		if (!AudioManager.CheckIfPlaying("level_platform_beetle_idle_loop"))
		{
			AudioManager.PlayLoop("level_platform_beetle_idle_loop");
		}
		this.emitAudioFromObject.Add("level_platform_beetle_idle_loop");
	}

	// Token: 0x06002C4B RID: 11339 RVA: 0x000D9784 File Offset: 0x000D7984
	public IEnumerator check_for_death_cr()
	{
		while (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 50f)
		{
			yield return null;
		}
		this.hasStarted = false;
		this.onCamera = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002C4C RID: 11340 RVA: 0x000D97A0 File Offset: 0x000D79A0
	public IEnumerator check_hit_box_cr()
	{
		for (;;)
		{
			if (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 50f && this.hasStarted)
			{
				this.sprite.GetComponent<SpriteRenderer>().enabled = true;
				if (base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax + 50f)
				{
					if (this.hasStarted)
					{
						this.onCamera = true;
					}
				}
				else
				{
					this.onCamera = false;
				}
			}
			else
			{
				this.sprite.GetComponent<SpriteRenderer>().enabled = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C4D RID: 11341 RVA: 0x000D97BC File Offset: 0x000D79BC
	public IEnumerator motion_cr()
	{
		float time = 0.1f;
		float t = 0f;
		float amount = 2f;
		for (;;)
		{
			while (t < time)
			{
				while (!this.isActivated || PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
				t += CupheadTime.Delta;
				this.sprite.transform.AddPosition(0f, amount, 0f);
				yield return null;
			}
			t = 0f;
			amount = -amount;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002488 RID: 9352
	[SerializeField]
	public Effect explosion;

	// Token: 0x04002489 RID: 9353
	[SerializeField]
	public SpriteRenderer sprite;
}
