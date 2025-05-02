using System;
using UnityEngine;

// Token: 0x020002F9 RID: 761
public class PirateLevelSquidProjectile : AbstractMonoBehaviour
{
	// Token: 0x060021DD RID: 8669 RVA: 0x000BB8BC File Offset: 0x000B9ABC
	public void Update()
	{
		if (this.state == PirateLevelSquidProjectile.State.Moving)
		{
			if (this.lifetime > 5f)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
		}
		this.lifetime += CupheadTime.Delta;
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000BB968 File Offset: 0x000B9B68
	public void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.name == PlayerId.PlayerOne.ToString() || collider.name == PlayerId.PlayerTwo.ToString())
		{
			PirateLevelSquidInkOverlay.Current.Hit();
			CupheadLevelCamera.Current.Shake(4f, 0.3f, false);
			this.Die();
		}
		else if (collider.name == "Level_Ground")
		{
			this.Die();
		}
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x0001CEFE File Offset: 0x0001B0FE
	public void Create(Vector2 pos, Vector2 velocity, float gravity)
	{
		this.InstantiatePrefab<PirateLevelSquidProjectile>().Init(pos, velocity, gravity);
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x0001CF0E File Offset: 0x0001B10E
	public void Init(Vector2 pos, Vector2 velocity, float gravity)
	{
		base.transform.position = pos;
		this.velocity = velocity;
		this.gravity = gravity;
		this.state = PirateLevelSquidProjectile.State.Moving;
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x0001CF36 File Offset: 0x0001B136
	public void Die()
	{
		base.animator.SetTrigger("OnDeath");
		base.GetComponent<Collider2D>().enabled = false;
		this.state = PirateLevelSquidProjectile.State.Dead;
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x0001CF5B File Offset: 0x0001B15B
	public void OnDeathAnimationComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001BE2 RID: 7138
	public const float MAX_LIFETIME = 5f;

	// Token: 0x04001BE3 RID: 7139
	public PirateLevelSquidProjectile.State state;

	// Token: 0x04001BE4 RID: 7140
	public Vector2 velocity;

	// Token: 0x04001BE5 RID: 7141
	public float gravity;

	// Token: 0x04001BE6 RID: 7142
	public float lifetime;

	// Token: 0x02000E1E RID: 3614
	public enum State
	{
		// Token: 0x04006628 RID: 26152
		Init,
		// Token: 0x04006629 RID: 26153
		Moving,
		// Token: 0x0400662A RID: 26154
		Dead
	}
}
