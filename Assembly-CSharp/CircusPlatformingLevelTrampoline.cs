using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000411 RID: 1041
public class CircusPlatformingLevelTrampoline : AbstractCollidableObject
{
	// Token: 0x1700035A RID: 858
	// (get) Token: 0x06002D51 RID: 11601 RVA: 0x00025D2E File Offset: 0x00023F2E
	// (set) Token: 0x06002D52 RID: 11602 RVA: 0x00025D36 File Offset: 0x00023F36
	public CircusPlatformingLevelTrampoline.Direction MoveDirection { get; set; }

	// Token: 0x1700035B RID: 859
	// (get) Token: 0x06002D53 RID: 11603 RVA: 0x00025D3F File Offset: 0x00023F3F
	// (set) Token: 0x06002D54 RID: 11604 RVA: 0x00025D47 File Offset: 0x00023F47
	public AbstractPlayerController TrackingPlayer { get; set; }

	// Token: 0x06002D55 RID: 11605 RVA: 0x00025D50 File Offset: 0x00023F50
	public void Start()
	{
		this.startPos = base.transform.position;
		base.StartCoroutine(this.loop_cr());
		base.StartCoroutine(this.sleep_sfx_cr());
	}

	// Token: 0x06002D56 RID: 11606 RVA: 0x000DC820 File Offset: 0x000DAA20
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter || phase == CollisionPhase.Stay)
		{
			LevelPlayerMotor component = hit.GetComponent<LevelPlayerMotor>();
			if (component != null && component.Grounded)
			{
				base.animator.SetTrigger("Bounce");
				component.OnTrampolineKnockUp(this.knockUpHeight);
			}
		}
	}

	// Token: 0x06002D57 RID: 11607 RVA: 0x000DC87C File Offset: 0x000DAA7C
	public IEnumerator loop_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.TrackingPlayer = PlayerManager.GetNext();
		while (this.TrackingPlayer == null)
		{
			yield return null;
		}
		float minBounds = base.transform.localPosition.x - this.bounds;
		float maxBounds = base.transform.localPosition.x + this.bounds;
		for (;;)
		{
			if (!base.enabled)
			{
				yield return null;
			}
			else
			{
				if (this.TrackingPlayer == null)
				{
					this.TrackingPlayer = PlayerManager.GetNext();
				}
				if (this.TrackingPlayer.transform.position.x > base.transform.position.x)
				{
					this.MoveDirection = CircusPlatformingLevelTrampoline.Direction.Right;
				}
				else
				{
					this.MoveDirection = CircusPlatformingLevelTrampoline.Direction.Left;
				}
				if (this.MoveDirection == CircusPlatformingLevelTrampoline.Direction.Right)
				{
					if (base.transform.position.x < this.startPos.x + this.bounds)
					{
						this.velocityX += this.acceleration * CupheadTime.Delta;
					}
					else
					{
						this.velocityX = 0f;
					}
				}
				else if (base.transform.position.x > this.startPos.x - this.bounds)
				{
					this.velocityX -= this.acceleration * CupheadTime.Delta;
				}
				else
				{
					this.velocityX = 0f;
				}
				this.velocityX = Mathf.Clamp(this.velocityX, -this.maxSpeed, this.maxSpeed);
				this.position = base.transform.localPosition;
				this.position.x = this.position.x + this.velocityX * CupheadTime.Delta;
				if (this.position.x < minBounds)
				{
					this.position.x = minBounds;
					this.velocityX = 0f;
				}
				else if (this.position.x > maxBounds)
				{
					this.position.x = maxBounds;
					this.velocityX = 0f;
				}
				base.transform.localPosition = this.position;
				if (this.TrackingPlayer.IsDead)
				{
					this.TrackingPlayer = PlayerManager.GetNext();
				}
				this.CheckIfShouldSleep();
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002D58 RID: 11608 RVA: 0x000DC898 File Offset: 0x000DAA98
	public void CheckIfShouldSleep()
	{
		Transform transform = PlayerManager.GetPlayer(PlayerId.PlayerOne).transform;
		if (this.IsInBounds(transform))
		{
			base.animator.SetBool("Sleep", false);
			if (PlayerManager.Multiplayer && this.IsInBounds(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform))
			{
				this.TrackingPlayer = PlayerManager.GetNext();
			}
			else
			{
				this.TrackingPlayer = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			}
		}
		else if (PlayerManager.Multiplayer && this.IsInBounds(PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform))
		{
			base.animator.SetBool("Sleep", false);
			this.TrackingPlayer = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		}
		else
		{
			if (base.transform.position.x >= this.startPos.x + this.bounds || base.transform.position.x <= this.startPos.x - this.bounds)
			{
				base.animator.SetBool("Sleep", true);
			}
			this.TrackingPlayer = PlayerManager.GetNext();
		}
	}

	// Token: 0x06002D59 RID: 11609 RVA: 0x000DC9C4 File Offset: 0x000DABC4
	public IEnumerator sleep_sfx_cr()
	{
		for (;;)
		{
			if (base.animator.GetBool("Sleep"))
			{
				yield return CupheadTime.WaitForSeconds(this, Random.Range(2f, 5f));
				if (base.animator.GetBool("Sleep"))
				{
					AudioManager.Play("circus_trampoline_sleep_boil");
					this.emitAudioFromObject.Add("circus_trampoline_sleep_boil");
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002D5A RID: 11610 RVA: 0x000DC9E0 File Offset: 0x000DABE0
	public bool IsInBounds(Transform other)
	{
		float num = this.startPos.x - this.bounds;
		float num2 = this.startPos.x + this.bounds;
		return other.position.x < num2 + this.AwakeningZone && other.position.x > num - this.AwakeningZone;
	}

	// Token: 0x06002D5B RID: 11611 RVA: 0x000DCA4C File Offset: 0x000DAC4C
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector2(this.startPos.x + this.bounds, this.startPos.y), new Vector2(this.startPos.x - this.bounds, this.startPos.y));
	}

	// Token: 0x06002D5C RID: 11612 RVA: 0x000DCAB4 File Offset: 0x000DACB4
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.DrawLine(new Vector2(base.transform.position.x + this.bounds, base.transform.position.y), new Vector2(base.transform.position.x - this.bounds, base.transform.position.y));
	}

	// Token: 0x06002D5D RID: 11613 RVA: 0x00025D82 File Offset: 0x00023F82
	public void TrampolineBounceSFX()
	{
		AudioManager.Play("circus_trampoline_bounce");
		this.emitAudioFromObject.Add("circus_trampoline_bounce");
	}

	// Token: 0x06002D5E RID: 11614 RVA: 0x00025D9E File Offset: 0x00023F9E
	public void TrampolineIntroSFX()
	{
		AudioManager.Stop("circus_trampoline_idle_loop");
		AudioManager.Play("circus_trampoline_sleep_intro");
		this.emitAudioFromObject.Add("circus_trampoline_sleep_intro");
	}

	// Token: 0x06002D5F RID: 11615 RVA: 0x00025DC4 File Offset: 0x00023FC4
	public void TrampolineOutroSFX()
	{
		AudioManager.Play("circus_trampoline_sleep_outro");
		this.emitAudioFromObject.Add("circus_trampoline_sleep_outro");
	}

	// Token: 0x06002D60 RID: 11616 RVA: 0x00025DE0 File Offset: 0x00023FE0
	public void TrampolineStartIdleSFX()
	{
		AudioManager.PlayLoop("circus_trampoline_idle_loop");
		this.emitAudioFromObject.Add("circus_trampoline_idle_loop");
	}

	// Token: 0x0400258F RID: 9615
	public const string BounceTrigger = "Bounce";

	// Token: 0x04002590 RID: 9616
	public const string Sleep = "Sleep";

	// Token: 0x04002591 RID: 9617
	[SerializeField]
	public float bounds;

	// Token: 0x04002592 RID: 9618
	[SerializeField]
	public float AwakeningZone = 500f;

	// Token: 0x04002593 RID: 9619
	public float maxSpeed;

	// Token: 0x04002594 RID: 9620
	public float acceleration;

	// Token: 0x04002595 RID: 9621
	public float knockUpHeight = -1.95f;

	// Token: 0x04002596 RID: 9622
	public float velocityX;

	// Token: 0x04002597 RID: 9623
	public Vector2 startPos;

	// Token: 0x04002598 RID: 9624
	public Vector2 position;

	// Token: 0x0200105C RID: 4188
	public enum Direction
	{
		// Token: 0x0400747F RID: 29823
		Left,
		// Token: 0x04007480 RID: 29824
		Right
	}
}
