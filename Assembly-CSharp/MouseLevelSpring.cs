using System;
using UnityEngine;

// Token: 0x020002D3 RID: 723
public class MouseLevelSpring : ParrySwitch
{
	// Token: 0x0600201E RID: 8222 RVA: 0x000B7278 File Offset: 0x000B5478
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<MouseLevelCanMouse>() != null && !this.isLaunched)
		{
			this.smallExplosion.Create(base.transform.position);
			base.animator.SetTrigger("OnDeath");
		}
	}

	// Token: 0x0600201F RID: 8223 RVA: 0x0001B368 File Offset: 0x00019568
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		player.GetComponent<LevelPlayerMotor>().OnTrampolineKnockUp(this.knockUpHeight);
		if (!this.isLaunched)
		{
			base.animator.SetTrigger("OnLaunch");
		}
	}

	// Token: 0x06002020 RID: 8224 RVA: 0x0001B39D File Offset: 0x0001959D
	public void GotRunOver()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.gameObject.SetActive(false);
	}

	// Token: 0x06002021 RID: 8225 RVA: 0x000B72D8 File Offset: 0x000B54D8
	public void LaunchSpring(Vector2 position, Vector2 velocity, float gravity)
	{
		if (base.gameObject.activeSelf)
		{
			base.animator.Play("Flip");
		}
		base.transform.position = position;
		this.velocity = velocity;
		this.gravity = gravity;
		this.isLaunched = true;
		base.gameObject.SetActive(true);
		base.GetComponent<Collider2D>().enabled = true;
	}

	// Token: 0x06002022 RID: 8226 RVA: 0x000B7344 File Offset: 0x000B5544
	public void Update()
	{
		if (this.isLaunched)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			if (base.transform.position.y < (float)Level.Current.Ground + this.offset)
			{
				base.transform.SetPosition(null, new float?((float)Level.Current.Ground + this.offset), null);
				if (this.isLaunched)
				{
					this.Landed();
				}
				this.isLaunched = false;
			}
		}
	}

	// Token: 0x06002023 RID: 8227 RVA: 0x0001B3B7 File Offset: 0x000195B7
	public void Landed()
	{
		base.animator.SetTrigger("OnLand");
		AudioManager.Play("level_mouse_can_springboard_land");
		this.emitAudioFromObject.Add("level_mouse_can_springboard_land");
	}

	// Token: 0x06002024 RID: 8228 RVA: 0x0001B3E3 File Offset: 0x000195E3
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.smallExplosion = null;
	}

	// Token: 0x04001A20 RID: 6688
	[SerializeField]
	public Effect smallExplosion;

	// Token: 0x04001A21 RID: 6689
	public float knockUpHeight = -1.5f;

	// Token: 0x04001A22 RID: 6690
	public bool isLaunched;

	// Token: 0x04001A23 RID: 6691
	public Vector2 velocity;

	// Token: 0x04001A24 RID: 6692
	public float gravity;

	// Token: 0x04001A25 RID: 6693
	public float offset = 120f;
}
