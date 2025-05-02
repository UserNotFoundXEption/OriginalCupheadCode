using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class DicePalaceRabbitLevelMagic : AbstractProjectile
{
	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06001799 RID: 6041 RVA: 0x00014143 File Offset: 0x00012343
	// (set) Token: 0x0600179A RID: 6042 RVA: 0x0001414B File Offset: 0x0001234B
	public float AppearTime { get; set; }

	// Token: 0x0600179B RID: 6043 RVA: 0x00014154 File Offset: 0x00012354
	public override void Start()
	{
		base.Start();
		this.initialPosition = base.transform.position;
		this.idleRoutine = this.wait_activation_cr();
		base.StartCoroutine(this.idleRoutine);
		base.StartCoroutine(this.FadeIn());
	}

	// Token: 0x0600179C RID: 6044 RVA: 0x00014193 File Offset: 0x00012393
	public override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x0600179D RID: 6045 RVA: 0x000141B1 File Offset: 0x000123B1
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600179E RID: 6046 RVA: 0x000141CF File Offset: 0x000123CF
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetBool("CanParry", parryable);
	}

	// Token: 0x0600179F RID: 6047 RVA: 0x000A237C File Offset: 0x000A057C
	public void ActivateOrb()
	{
		this.circleCollider.enabled = true;
		Color color = this.spriteRenderer.color;
		color.a = 1f;
		this.spriteRenderer.color = color;
		base.StopCoroutine(this.idleRoutine);
		base.transform.position = this.initialPosition;
		base.animator.SetTrigger("Attack");
		if (!this.StartMagicLaserSFX)
		{
			AudioManager.Play("projectile_laser");
			this.emitAudioFromObject.Add("projectile_laser");
			this.StartMagicLaserSFX = true;
		}
	}

	// Token: 0x060017A0 RID: 6048 RVA: 0x000141E9 File Offset: 0x000123E9
	public void Move(float startY, bool down, float speed)
	{
		base.StartCoroutine(this.move_cr(startY, down, speed));
	}

	// Token: 0x060017A1 RID: 6049 RVA: 0x000A2414 File Offset: 0x000A0614
	public IEnumerator wait_activation_cr()
	{
		for (;;)
		{
			Vector3 vector;
			vector..ctor((float)Random.Range(-1, 1), (float)Random.Range(-1, 1), 0f);
			Vector3 newDirection = vector.normalized * 10f;
			float progress = 0f;
			while (progress < 0.1f)
			{
				base.transform.position += newDirection * CupheadTime.Delta;
				progress += CupheadTime.Delta;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017A2 RID: 6050 RVA: 0x000A2430 File Offset: 0x000A0630
	public IEnumerator move_cr(float startY, bool down, float speed)
	{
		this.StartMagicSFX = false;
		this.StartMagicLaserSFX = false;
		Vector3 velocity = speed * ((!down) ? Vector3.up : Vector3.down);
		float progress = 0f;
		while ((!down && startY + progress < 360f) || (down && startY + progress > -360f))
		{
			base.transform.position += velocity * CupheadTime.Delta;
			progress += velocity.y * CupheadTime.Delta;
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060017A3 RID: 6051 RVA: 0x000A2460 File Offset: 0x000A0660
	public IEnumerator FadeIn()
	{
		if (!this.StartMagicSFX)
		{
			AudioManager.Play("projectile_magic_start");
			this.emitAudioFromObject.Add("projectile_magic_start");
			this.StartMagicSFX = true;
		}
		while (this.spriteRenderer.color.a < 1f)
		{
			Color c = this.spriteRenderer.color;
			c.a += CupheadTime.Delta / this.AppearTime;
			this.spriteRenderer.color = c;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060017A4 RID: 6052 RVA: 0x000141FB File Offset: 0x000123FB
	public void SetSuit(int suit)
	{
		base.animator.SetInteger("Suit", suit);
	}

	// Token: 0x060017A5 RID: 6053 RVA: 0x0001420E File Offset: 0x0001240E
	public void IsOffset(bool offset)
	{
		base.animator.SetFloat("CycleOffset", (!offset) ? 0f : 0.5f);
	}

	// Token: 0x04001331 RID: 4913
	public const float IdleSpeed = 10f;

	// Token: 0x04001332 RID: 4914
	[SerializeField]
	public SpriteRenderer spriteRenderer;

	// Token: 0x04001333 RID: 4915
	[SerializeField]
	public CircleCollider2D circleCollider;

	// Token: 0x04001334 RID: 4916
	public IEnumerator idleRoutine;

	// Token: 0x04001335 RID: 4917
	public Vector3 initialPosition;

	// Token: 0x04001337 RID: 4919
	public bool StartMagicSFX;

	// Token: 0x04001338 RID: 4920
	public bool StartMagicLaserSFX;
}
