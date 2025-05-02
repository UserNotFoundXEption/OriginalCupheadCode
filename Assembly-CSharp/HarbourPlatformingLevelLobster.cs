using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000430 RID: 1072
public class HarbourPlatformingLevelLobster : PlatformingLevelShootingEnemy
{
	// Token: 0x06002E35 RID: 11829 RVA: 0x000DF068 File Offset: 0x000DD268
	public override void Start()
	{
		base.Start();
		this.startPositionY = base.transform.position.y;
		base.StartCoroutine(this.start_trigger_cr());
		this.previousY = base.transform.position.y;
		this.exploder = base.GetComponent<LevelBossDeathExploder>();
	}

	// Token: 0x06002E36 RID: 11830 RVA: 0x000DF0C8 File Offset: 0x000DD2C8
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.offTrigger.transform.position, new Vector3(this.offTrigger.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.onTrigger.transform.position, new Vector3(this.onTrigger.transform.position.x, 5000f, 0f));
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.DrawLine(this.leftBoundary.transform.position, new Vector3(this.leftBoundary.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.rightBoundary.transform.position, new Vector3(this.rightBoundary.transform.position.x, 5000f, 0f));
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x000DF208 File Offset: 0x000DD408
	public IEnumerator attack_cr()
	{
		base.animator.SetTrigger("OnAttackStart");
		yield return base.animator.WaitForAnimationToStart(this, "Warning_Loop", false);
		yield return CupheadTime.WaitForSeconds(this, base.Properties.lobsterWarningTime);
		base.animator.SetTrigger("Attack");
		this.AttackSFX();
		yield return null;
		yield return base.animator.WaitForAnimationToStart(this, "Attack_Trans_Idle", false);
		yield break;
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x000DF224 File Offset: 0x000DD424
	public IEnumerator start_trigger_cr()
	{
		while (this._target == null)
		{
			yield return null;
		}
		this.dist = this._target.transform.position.x - this.onTrigger.transform.position.x;
		while (this.dist < 0f)
		{
			this.dist = this._target.transform.position.x - this.onTrigger.transform.position.x;
			yield return null;
		}
		this.mainCoroutine = base.StartCoroutine(this.main_cr());
		this.dist = this._target.transform.position.x - this.offTrigger.transform.position.x;
		while (this.dist < 0f)
		{
			this.dist = this._target.transform.position.x - this.offTrigger.transform.position.x;
			yield return null;
		}
		this.isGone = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002E39 RID: 11833 RVA: 0x000DF240 File Offset: 0x000DD440
	public IEnumerator main_cr()
	{
		while (!this.isGone)
		{
			this.direction = ((this.direction != PlatformingLevelShootingEnemy.Direction.Right) ? PlatformingLevelShootingEnemy.Direction.Right : PlatformingLevelShootingEnemy.Direction.Left);
			base.transform.localScale = new Vector3((float)((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? 1 : -1), 1f, 1f);
			base.transform.SetPosition(new float?((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (CupheadLevelCamera.Current.Bounds.xMin - 350f) : (CupheadLevelCamera.Current.Bounds.xMax + 350f)), new float?(base.Properties.lobsterY), null);
			if ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x > this.rightBoundary.position.x) : (base.transform.position.x < this.leftBoundary.position.x))
			{
				base.transform.SetPosition(null, new float?(-5000f), null);
				yield return CupheadTime.WaitForSeconds(this, base.Properties.lobsterOffscreenTime);
			}
			else
			{
				if ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x < this.leftBoundary.position.x) : (base.transform.position.x > this.rightBoundary.position.x))
				{
					base.transform.SetPosition(new float?((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? this.leftBoundary.position.x : this.rightBoundary.position.x), null, null);
					yield return base.StartCoroutine(this.pop_up_cr());
				}
				else
				{
					base.animator.Play("Idle");
					this.IdleSFX();
					this.poppedUp = true;
				}
				while ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMax + -250f) : (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - -250f))
				{
					base.transform.AddPosition(base.Properties.lobsterSpeed * CupheadTime.Delta * (float)((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? 1 : -1), 0f, 0f);
					if ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x > this.rightBoundary.position.x) : (base.transform.position.x < this.leftBoundary.position.x))
					{
						this.Popdown(false);
						yield break;
					}
					yield return null;
				}
				yield return base.StartCoroutine(this.attack_cr());
				while ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMax + 350f) : (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - 350f))
				{
					base.transform.AddPosition(base.Properties.lobsterSpeed * CupheadTime.Delta * (float)((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? 1 : -1), 0f, 0f);
					if ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x > this.rightBoundary.position.x) : (base.transform.position.x < this.leftBoundary.position.x))
					{
						this.Popdown(false);
						yield break;
					}
					yield return null;
				}
				base.transform.SetPosition(null, new float?(-5000f), null);
				yield return CupheadTime.WaitForSeconds(this, base.Properties.lobsterOffscreenTime);
			}
		}
		Object.Destroy(this.main.gameObject);
		yield break;
		yield break;
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x000268C0 File Offset: 0x00024AC0
	public void Popup()
	{
		base.StartCoroutine(this.pop_up_cr());
	}

	// Token: 0x06002E3B RID: 11835 RVA: 0x000268CF File Offset: 0x00024ACF
	public void Popdown(bool dead)
	{
		AudioManager.Stop("harbour_lobster_idle");
		base.StartCoroutine(this.pop_down_cr(dead));
	}

	// Token: 0x06002E3C RID: 11836 RVA: 0x000DF25C File Offset: 0x000DD45C
	public IEnumerator pop_up_cr()
	{
		base.animator.SetTrigger("OnEmerge");
		this.EmergeSFX();
		float t = 0f;
		float time = 0.6f;
		float endY = base.Properties.lobsterY;
		Vector2 end = new Vector2(base.transform.position.x, endY);
		while (t < time)
		{
			Vector2 start = base.transform.position;
			end = new Vector2(base.transform.position.x, endY);
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		this.poppedUp = true;
		yield break;
	}

	// Token: 0x06002E3D RID: 11837 RVA: 0x000DF278 File Offset: 0x000DD478
	public IEnumerator pop_down_cr(bool dead)
	{
		if (!this.poppedUp)
		{
			yield break;
		}
		this.poppedUp = false;
		if (dead && this.mainCoroutine != null)
		{
			base.StopCoroutine(this.mainCoroutine);
		}
		if (this.isGone)
		{
			base.transform.parent = null;
			base.animator.SetTrigger("OnTuck");
		}
		else
		{
			base.animator.Play("Tuck");
			this.SinkSFX();
		}
		if (dead)
		{
			this.exploder.StartExplosion();
			yield return CupheadTime.WaitForSeconds(this, 1f);
			this.exploder.StopExplosions();
		}
		float t = 0f;
		float time = 1.5f;
		Vector2 start = base.transform.position;
		Vector2 end = new Vector2(base.transform.position.x, this.startPositionY);
		float splashDepth = -1200f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, end, val);
			if (base.transform.position.y <= splashDepth && this.previousY > splashDepth)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.splashPrefab, new Vector3(base.transform.position.x, splashDepth, base.transform.position.z), Quaternion.identity);
				gameObject.transform.SetParent(null);
				this.delay_destroy_cr(gameObject, 10f);
			}
			t += CupheadTime.Delta;
			this.previousY = base.transform.position.y;
			yield return null;
		}
		base.transform.position = end;
		if (this.isGone)
		{
			Object.Destroy(this.main.gameObject);
		}
		else
		{
			base.StartCoroutine(this.delay_cr(dead));
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002E3E RID: 11838 RVA: 0x000DF29C File Offset: 0x000DD49C
	public IEnumerator delay_destroy_cr(GameObject o, float t)
	{
		yield return CupheadTime.WaitForSeconds(this, t);
		Object.Destroy(o);
		yield break;
	}

	// Token: 0x06002E3F RID: 11839 RVA: 0x000268E9 File Offset: 0x00024AE9
	public override void Die()
	{
		this.Popdown(true);
	}

	// Token: 0x06002E40 RID: 11840 RVA: 0x000DF2C8 File Offset: 0x000DD4C8
	public IEnumerator delay_cr(bool dead)
	{
		yield return CupheadTime.WaitForSeconds(this, (!dead) ? base.Properties.lobsterOffscreenTime : base.Properties.lobsterTuckTime);
		if (this.isGone)
		{
			Object.Destroy(this.main.gameObject);
			yield break;
		}
		base.Health = base.Properties.Health;
		this.mainCoroutine = base.StartCoroutine(this.main_cr());
		yield break;
	}

	// Token: 0x06002E41 RID: 11841 RVA: 0x000268F2 File Offset: 0x00024AF2
	public void EmergeSFX()
	{
		AudioManager.Play("harbour_lobster_emerge");
		this.emitAudioFromObject.Add("harbour_lobster_emerge");
	}

	// Token: 0x06002E42 RID: 11842 RVA: 0x0002690E File Offset: 0x00024B0E
	public void SinkSFX()
	{
		AudioManager.Stop("harbour_lobster_idle");
		AudioManager.Play("harbour_lobster_sink");
		this.emitAudioFromObject.Add("harbour_lobster_sink");
	}

	// Token: 0x06002E43 RID: 11843 RVA: 0x00026934 File Offset: 0x00024B34
	public void AttackSFX()
	{
		AudioManager.Play("harbour_lobster_attack");
		this.emitAudioFromObject.Add("harbour_lobster_attack");
	}

	// Token: 0x06002E44 RID: 11844 RVA: 0x00026950 File Offset: 0x00024B50
	public void IdleSFX()
	{
		AudioManager.PlayLoop("harbour_lobster_idle");
		this.emitAudioFromObject.Add("harbour_lobster_idle");
	}

	// Token: 0x06002E45 RID: 11845 RVA: 0x0002696C File Offset: 0x00024B6C
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.splashPrefab = null;
	}

	// Token: 0x0400264D RID: 9805
	[SerializeField]
	public Transform main;

	// Token: 0x0400264E RID: 9806
	[SerializeField]
	public Transform onTrigger;

	// Token: 0x0400264F RID: 9807
	[SerializeField]
	public Transform offTrigger;

	// Token: 0x04002650 RID: 9808
	[SerializeField]
	public Transform leftBoundary;

	// Token: 0x04002651 RID: 9809
	[SerializeField]
	public Transform rightBoundary;

	// Token: 0x04002652 RID: 9810
	[SerializeField]
	public LevelBossDeathExploder exploder;

	// Token: 0x04002653 RID: 9811
	[SerializeField]
	public GameObject splashPrefab;

	// Token: 0x04002654 RID: 9812
	[SerializeField]
	public Transform splashTransform;

	// Token: 0x04002655 RID: 9813
	public bool poppedUp;

	// Token: 0x04002656 RID: 9814
	public bool isGone;

	// Token: 0x04002657 RID: 9815
	public float dist = 1000f;

	// Token: 0x04002658 RID: 9816
	public float startPositionY;

	// Token: 0x04002659 RID: 9817
	public const float OffScreenPadding = 350f;

	// Token: 0x0400265A RID: 9818
	public const float attackPadding = -250f;

	// Token: 0x0400265B RID: 9819
	public Coroutine mainCoroutine;

	// Token: 0x0400265C RID: 9820
	public float previousY;

	// Token: 0x0400265D RID: 9821
	public PlatformingLevelShootingEnemy.Direction direction = PlatformingLevelShootingEnemy.Direction.Right;
}
