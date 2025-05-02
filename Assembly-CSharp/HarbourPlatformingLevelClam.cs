using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000428 RID: 1064
public class HarbourPlatformingLevelClam : PlatformingLevelShootingEnemy
{
	// Token: 0x06002E00 RID: 11776 RVA: 0x000265A1 File Offset: 0x000247A1
	public override void Start()
	{
		base.Start();
		this.startPos = base.transform.position;
	}

	// Token: 0x06002E01 RID: 11777 RVA: 0x000265BA File Offset: 0x000247BA
	public override void StartShoot()
	{
		if (this.counter < base.Properties.ClamShotCount)
		{
			this.counter++;
			base.StartShoot();
			this.AttackSFX();
		}
	}

	// Token: 0x06002E02 RID: 11778 RVA: 0x000DE278 File Offset: 0x000DC478
	public override void Update()
	{
		if (!this.startClam && this.octopus != null && this.octopus.Started())
		{
			this.Popup();
			this.startClam = true;
		}
		if (this._target != null)
		{
			if (!this.startClam && this.octopus == null)
			{
				this.dist = this._target.transform.position.x - this.onTrigger.transform.position.x;
				if (this.dist > 0f && !this.startClam)
				{
					this.Popup();
					this.startClam = true;
				}
			}
			else
			{
				this.dist = this._target.transform.position.x - this.offTrigger.transform.position.x;
				if (this.dist > 0f && !this.endClam)
				{
					this.startClam = false;
					this.endClam = true;
				}
			}
		}
	}

	// Token: 0x06002E03 RID: 11779 RVA: 0x000265EC File Offset: 0x000247EC
	public void Popup()
	{
		base.animator.SetTrigger("OnPopup");
		base.transform.parent = CupheadLevelCamera.Current.transform;
		base.StartCoroutine(this.pop_up_cr());
	}

	// Token: 0x06002E04 RID: 11780 RVA: 0x000DE3B0 File Offset: 0x000DC5B0
	public IEnumerator pop_up_cr()
	{
		for (;;)
		{
			if (!this.isDead)
			{
				EaseUtils.EaseType ease = EaseUtils.EaseType.easeInOutSine;
				float t = 0f;
				float time = base.Properties.ClamTimeSpeedUp;
				float startY = this.startPos.y;
				float endY = base.Properties.ClamMaxPointRange.RandomFloat();
				base.transform.SetPosition(new float?(CupheadLevelCamera.Current.Bounds.xMin + this.offset), null, null);
				this.Show();
				while (t < time)
				{
					float val = t / time;
					base.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, startY, endY, val)), null);
					t += CupheadTime.Delta;
					yield return null;
				}
				base.animator.SetTrigger("OnSlowdown");
				base.transform.SetPosition(null, new float?(endY), null);
				t = 0f;
				time = base.Properties.ClamTimeSpeedDown;
				yield return null;
				while (t < time)
				{
					float val2 = t / time;
					base.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, endY, startY, val2)), null);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.Hide();
				base.transform.SetPosition(null, new float?(startY), null);
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.ProjectileDelay.RandomFloat());
			if (!this.endClam)
			{
				this.isDead = false;
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E05 RID: 11781 RVA: 0x000DE3CC File Offset: 0x000DC5CC
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.offTrigger.transform.position, new Vector3(this.offTrigger.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.onTrigger.transform.position, new Vector3(this.onTrigger.transform.position.x, 5000f, 0f));
	}

	// Token: 0x06002E06 RID: 11782 RVA: 0x000DE478 File Offset: 0x000DC678
	public override void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<DamageReceiver>().enabled = false;
		base.animator.Play("Off");
		this.DeathParts();
		this.isDead = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.fall_cr());
		base.Explode();
	}

	// Token: 0x06002E07 RID: 11783 RVA: 0x000DE4D4 File Offset: 0x000DC6D4
	public IEnumerator fall_cr()
	{
		Vector2 velocity = new Vector2(0f, 200f);
		float accumulatedGravity = 0f;
		float speed = 400f;
		while (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 100f)
		{
			base.transform.position += (velocity + new Vector2(-speed, accumulatedGravity)) * Time.fixedDeltaTime;
			accumulatedGravity += -100f;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, base.Properties.ClamDespawnDelayRange.RandomFloat());
		base.transform.SetPosition(new float?(CupheadLevelCamera.Current.Bounds.xMin + this.offset), new float?(this.startPos.y), null);
		this.Hide();
		yield return null;
		yield break;
	}

	// Token: 0x06002E08 RID: 11784 RVA: 0x000DE4F0 File Offset: 0x000DC6F0
	public void Hide()
	{
		this.counter = 0;
		base.animator.Play("Off");
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<DamageReceiver>().enabled = false;
		this.OnStart();
		if (this.isDead)
		{
			this.Popup();
		}
	}

	// Token: 0x06002E09 RID: 11785 RVA: 0x00026620 File Offset: 0x00024820
	public void Show()
	{
		base.animator.SetTrigger("OnPopup");
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<DamageReceiver>().enabled = true;
	}

	// Token: 0x06002E0A RID: 11786 RVA: 0x000DE544 File Offset: 0x000DC744
	public void DeathParts()
	{
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x06002E0B RID: 11787 RVA: 0x0002664A File Offset: 0x0002484A
	public void AttackSFX()
	{
		AudioManager.Play("harbour_clam_attack");
		this.emitAudioFromObject.Add("harbour_clam_attack");
	}

	// Token: 0x06002E0C RID: 11788 RVA: 0x00026666 File Offset: 0x00024866
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.octopus = null;
		this.deathParts = null;
	}

	// Token: 0x04002615 RID: 9749
	public const float GRAVITY = -100f;

	// Token: 0x04002616 RID: 9750
	[SerializeField]
	public SpriteDeathParts[] deathParts;

	// Token: 0x04002617 RID: 9751
	[SerializeField]
	public Transform main;

	// Token: 0x04002618 RID: 9752
	[SerializeField]
	public Transform onTrigger;

	// Token: 0x04002619 RID: 9753
	[SerializeField]
	public Transform offTrigger;

	// Token: 0x0400261A RID: 9754
	[SerializeField]
	public HarbourPlatformingLevelOctopus octopus;

	// Token: 0x0400261B RID: 9755
	public bool startClam;

	// Token: 0x0400261C RID: 9756
	public bool endClam;

	// Token: 0x0400261D RID: 9757
	public bool isDead;

	// Token: 0x0400261E RID: 9758
	public float dist = -1000f;

	// Token: 0x0400261F RID: 9759
	public float offset = 100f;

	// Token: 0x04002620 RID: 9760
	public int counter;

	// Token: 0x04002621 RID: 9761
	public Vector3 startPos;
}
