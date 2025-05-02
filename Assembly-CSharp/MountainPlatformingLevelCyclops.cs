using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043A RID: 1082
public class MountainPlatformingLevelCyclops : PlatformingLevelAutoscrollObject
{
	// Token: 0x06002E80 RID: 11904 RVA: 0x00026C62 File Offset: 0x00024E62
	public override void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.checkToLock = true;
		this.lockDistance = -1300f;
		this.IsEnabled(false);
		base.StartCoroutine(this.start_scrolling_cr());
		base.Start();
	}

	// Token: 0x06002E81 RID: 11905 RVA: 0x00026C9B File Offset: 0x00024E9B
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x00026CB9 File Offset: 0x00024EB9
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x00026CD7 File Offset: 0x00024ED7
	public void IsEnabled(bool isenabled)
	{
		base.GetComponent<Collider2D>().enabled = isenabled;
		base.GetComponent<SpriteRenderer>().enabled = isenabled;
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x00026CF1 File Offset: 0x00024EF1
	public override void StartAutoscroll()
	{
		base.StartAutoscroll();
		CupheadLevelCamera.Current.OffsetCamera(true, false);
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x000DFBE0 File Offset: 0x000DDDE0
	public IEnumerator start_scrolling_cr()
	{
		while (!this.isLocked)
		{
			yield return null;
		}
		this.cyclopsMoving = true;
		base.StartCoroutine(this.start_moving_cr());
		this.IsEnabled(true);
		PlatformingLevel level = (PlatformingLevel)Level.Current;
		level.useAltQuote = true;
		while (base.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 650f)
		{
			yield return null;
		}
		this.autoscrollMoving = true;
		base.StartCoroutine(this.check_to_move_forward_cr());
		this.StartAutoscroll();
		yield break;
	}

	// Token: 0x06002E86 RID: 11910 RVA: 0x000DFBFC File Offset: 0x000DDDFC
	public IEnumerator start_moving_cr()
	{
		base.animator.Play("Run");
		while (this.cyclopsMoving)
		{
			base.transform.position += Vector3.right * (200f * this.autoScrollMultiplier) * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E87 RID: 11911 RVA: 0x000DFC18 File Offset: 0x000DDE18
	public IEnumerator check_to_move_forward_cr()
	{
		while (this.autoscrollMoving)
		{
			float dist = PlayerManager.Center.x - base.transform.position.x;
			if (base.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 1600f)
			{
				base.transform.position = new Vector3(CupheadLevelCamera.Current.transform.position.x - 1600f, base.transform.position.y);
			}
			if (dist > 1300f)
			{
				if (CupheadLevelCamera.Current.autoScrolling)
				{
					CupheadLevelCamera.Current.SetAutoScroll(false);
				}
			}
			else if (!CupheadLevelCamera.Current.autoScrolling)
			{
				CupheadLevelCamera.Current.SetAutoScroll(true);
				CupheadLevelCamera.Current.SetAutoscrollSpeedMultiplier(this.autoScrollMultiplier);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E88 RID: 11912 RVA: 0x00026D05 File Offset: 0x00024F05
	public override void StartEndingAutoscroll()
	{
		base.StartEndingAutoscroll();
		this.autoscrollMoving = false;
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x00026D14 File Offset: 0x00024F14
	public override void EndAutoscroll()
	{
		base.EndAutoscroll();
		base.StartCoroutine(this.fall_cr());
	}

	// Token: 0x06002E8A RID: 11914 RVA: 0x000DFC34 File Offset: 0x000DDE34
	public IEnumerator fall_cr()
	{
		while (base.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 1300f)
		{
			yield return null;
		}
		CupheadLevelCamera.Current.LockCamera(true);
		this.cyclopsMoving = false;
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Map.ToString();
		base.animator.SetTrigger("OnFall");
		yield return base.animator.WaitForAnimationToEnd(this, "Fall", false, true);
		CupheadLevelCamera.Current.LockCamera(false);
		CupheadLevelCamera.Current.OffsetCamera(false, false);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002E8B RID: 11915 RVA: 0x00026D29 File Offset: 0x00024F29
	public void CameraShake()
	{
		CupheadLevelCamera.Current.Shake(10f, 0.5f, false);
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x00026D40 File Offset: 0x00024F40
	public void SoundCyclopsFall()
	{
		AudioManager.Play("castle_giant_rock_chase_death");
	}

	// Token: 0x06002E8D RID: 11917 RVA: 0x00026D4C File Offset: 0x00024F4C
	public void SoundCyclopsFootstep()
	{
		AudioManager.Play("castle_giant_rock_chase_footstep");
		this.emitAudioFromObject.Add("castle_giant_rock_chase_footstep");
	}

	// Token: 0x0400269A RID: 9882
	[SerializeField]
	public float autoScrollMultiplier;

	// Token: 0x0400269B RID: 9883
	public const float MAX_AUTOSCROLL_DIST = 1300f;

	// Token: 0x0400269C RID: 9884
	public const float MAX_CYCLOPS_DIST = 1600f;

	// Token: 0x0400269D RID: 9885
	public bool autoscrollMoving;

	// Token: 0x0400269E RID: 9886
	public bool cyclopsMoving;

	// Token: 0x0400269F RID: 9887
	public DamageDealer damageDealer;
}
