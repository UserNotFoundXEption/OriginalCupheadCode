using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200041A RID: 1050
public class FunhousePlatformingLevelJackinBox : PlatformingLevelShootingEnemy
{
	// Token: 0x06002D9A RID: 11674 RVA: 0x000DD230 File Offset: 0x000DB430
	public override void Start()
	{
		base.Start();
		this.directionIndex = Random.Range(0, base.Properties.jackinDirectionString.Split(new char[]
		{
			','
		}).Length);
		this.jack.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		AudioManager.PlayLoop("funhouse_jackbox_eye_spin_loop");
		this.emitAudioFromObject.Add("funhouse_jackbox_eye_spin_loop");
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x06002D9B RID: 11675 RVA: 0x000260F9 File Offset: 0x000242F9
	public override void OnStart()
	{
		base.OnStart();
		base.StartCoroutine(this.pop_up_cr());
	}

	// Token: 0x06002D9C RID: 11676 RVA: 0x000DD2B0 File Offset: 0x000DB4B0
	public IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x06002D9D RID: 11677 RVA: 0x0002610E File Offset: 0x0002430E
	public override void Shoot()
	{
		if (this.shootTime)
		{
			base.Shoot();
		}
	}

	// Token: 0x06002D9E RID: 11678 RVA: 0x000DD2CC File Offset: 0x000DB4CC
	public IEnumerator pop_up_cr()
	{
		string dir = string.Empty;
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			this.justDied = false;
			if (base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			})[this.directionIndex][0] == 'U')
			{
				base.animator.SetInteger("Direction", 1);
				this.jack.transform.position = this.topRoot.transform.position;
				this.jack.transform.SetEulerAngles(null, null, new float?(270f));
				this.jack.GetComponent<SpriteRenderer>().sortingOrder = -5;
				dir = "Up";
			}
			else if (base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			})[this.directionIndex][0] == 'L')
			{
				base.animator.SetInteger("Direction", 2);
				this.jack.transform.position = this.leftRoot.transform.position;
				this.jack.transform.SetEulerAngles(null, null, new float?(0f));
				this.jack.GetComponent<SpriteRenderer>().sortingOrder = 5;
				dir = "Left";
			}
			else if (base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			})[this.directionIndex][0] == 'D')
			{
				base.animator.SetInteger("Direction", 3);
				this.jack.transform.position = this.bottomRoot.transform.position;
				this.jack.transform.SetEulerAngles(null, null, new float?(90f));
				this.jack.GetComponent<SpriteRenderer>().sortingOrder = -5;
				dir = "Down";
			}
			else if (base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			})[this.directionIndex][0] == 'R')
			{
				base.animator.SetInteger("Direction", 4);
				this.jack.transform.position = this.rightRoot.transform.position;
				this.jack.transform.SetEulerAngles(null, null, new float?(180f));
				this.jack.GetComponent<SpriteRenderer>().sortingOrder = -5;
				dir = "Right";
			}
			base.animator.SetTrigger("OnDirection");
			yield return base.animator.WaitForAnimationToStart(this, "Eye_" + dir, 1, false);
			AudioManager.Stop("funhouse_jackbox_eye_spin_loop");
			yield return CupheadTime.WaitForSeconds(this, 0.3f);
			AudioManager.PlayLoop("funhouse_jackbox_eye_spin_loop");
			this.emitAudioFromObject.Add("funhouse_jackbox_eye_spin_loop");
			base.animator.SetTrigger("OnHead");
			yield return base.animator.WaitForAnimationToEnd(this, "Jack_Head", 3, false, true);
			if (!this.justDied)
			{
				this.shootTime = true;
				this.shootTime = false;
			}
			yield return CupheadTime.WaitForSeconds(this, this.DieTime());
			this.directionIndex = (this.directionIndex + 1) % base.Properties.jackinDirectionString.Split(new char[]
			{
				','
			}).Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002D9F RID: 11679 RVA: 0x000DD2E8 File Offset: 0x000DB4E8
	public void HideSprite()
	{
		switch (base.animator.GetInteger("Direction"))
		{
		case 1:
			this.top.GetComponent<SpriteRenderer>().enabled = false;
			break;
		case 2:
			this.left.GetComponent<SpriteRenderer>().enabled = false;
			break;
		case 3:
			this.bottom.GetComponent<SpriteRenderer>().enabled = false;
			break;
		case 4:
			this.right.GetComponent<SpriteRenderer>().enabled = false;
			break;
		}
	}

	// Token: 0x06002DA0 RID: 11680 RVA: 0x000DD37C File Offset: 0x000DB57C
	public void SlideSprite()
	{
		switch (base.animator.GetInteger("Direction"))
		{
		case 1:
			base.StartCoroutine(this.slide_in(this.top, Vector3.up));
			break;
		case 2:
			base.StartCoroutine(this.slide_in(this.left, Vector3.left));
			break;
		case 3:
			base.StartCoroutine(this.slide_in(this.bottom, Vector3.down));
			break;
		case 4:
			base.StartCoroutine(this.slide_in(this.right, Vector3.right));
			break;
		}
	}

	// Token: 0x06002DA1 RID: 11681 RVA: 0x000DD42C File Offset: 0x000DB62C
	public void ShootProjectile()
	{
		if (!this.justDied)
		{
			this.player = PlayerManager.GetNext();
			this.projectile.Create(this.jackRoot.transform.position, base.Properties.ProjectileSpeed, base.Properties.jackinShootDelay, this.player, base.animator.GetInteger("Direction"));
			AudioManager.Play("funhouse_jackbox_shoot");
			this.emitAudioFromObject.Add("funhouse_jackbox_shoot");
		}
	}

	// Token: 0x06002DA2 RID: 11682 RVA: 0x000DD4B4 File Offset: 0x000DB6B4
	public IEnumerator slide_in(Transform sprite, Vector3 direction)
	{
		Vector3 startPos = sprite.transform.position + -direction * 100f;
		Vector3 endPos = sprite.transform.position;
		sprite.transform.position = startPos;
		float t = 0f;
		float time = 1f;
		sprite.GetComponent<SpriteRenderer>().enabled = true;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0f, 1f, t / time);
			sprite.transform.position = Vector3.Lerp(startPos, endPos, val);
			yield return null;
		}
		AudioManager.Play("funhouse_jackbox_shoot_launch");
		this.emitAudioFromObject.Add("funhouse_jackbox_shoot_launch");
		yield return null;
		yield break;
	}

	// Token: 0x06002DA3 RID: 11683 RVA: 0x000DD4E0 File Offset: 0x000DB6E0
	public float DieTime()
	{
		return (!this.justDied) ? base.Properties.jackinAppearDelay : base.Properties.jackinDeathAppearDelay;
	}

	// Token: 0x06002DA4 RID: 11684 RVA: 0x00026121 File Offset: 0x00024321
	public override void Die()
	{
		this.justDied = true;
	}

	// Token: 0x06002DA5 RID: 11685 RVA: 0x0002612A File Offset: 0x0002432A
	public void SoundJackInBoxHeadPop()
	{
		AudioManager.Play("funhouse_jackbox_jack_head");
		this.emitAudioFromObject.Add("funhouse_jackbox_jack_head");
	}

	// Token: 0x06002DA6 RID: 11686 RVA: 0x00026146 File Offset: 0x00024346
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.projectile = null;
	}

	// Token: 0x040025CA RID: 9674
	[SerializeField]
	public FunhousePlatformingLevelJackinBoxProjectile projectile;

	// Token: 0x040025CB RID: 9675
	[SerializeField]
	public GameObject jack;

	// Token: 0x040025CC RID: 9676
	[SerializeField]
	public Transform jackRoot;

	// Token: 0x040025CD RID: 9677
	[SerializeField]
	public Transform topRoot;

	// Token: 0x040025CE RID: 9678
	[SerializeField]
	public Transform bottomRoot;

	// Token: 0x040025CF RID: 9679
	[SerializeField]
	public Transform rightRoot;

	// Token: 0x040025D0 RID: 9680
	[SerializeField]
	public Transform leftRoot;

	// Token: 0x040025D1 RID: 9681
	[SerializeField]
	public Transform top;

	// Token: 0x040025D2 RID: 9682
	[SerializeField]
	public Transform bottom;

	// Token: 0x040025D3 RID: 9683
	[SerializeField]
	public Transform right;

	// Token: 0x040025D4 RID: 9684
	[SerializeField]
	public Transform left;

	// Token: 0x040025D5 RID: 9685
	public int directionIndex;

	// Token: 0x040025D6 RID: 9686
	public bool justDied;

	// Token: 0x040025D7 RID: 9687
	public bool shootTime;

	// Token: 0x040025D8 RID: 9688
	public float offset = 50f;

	// Token: 0x040025D9 RID: 9689
	public AbstractPlayerController player;
}
