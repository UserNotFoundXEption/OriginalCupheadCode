using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044D RID: 1101
public class MountainPlatformingLevelSatyr : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002F2A RID: 12074 RVA: 0x000274BC File Offset: 0x000256BC
	public override void Start()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.Start();
		base.StartCoroutine(this.satyr_land_cr());
	}

	// Token: 0x06002F2B RID: 12075 RVA: 0x000E0ED4 File Offset: 0x000DF0D4
	public void Init(PlatformingLevelGroundMovementEnemy.Direction direction, bool isForeground)
	{
		this._direction = direction;
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
	}

	// Token: 0x06002F2C RID: 12076 RVA: 0x000E0F04 File Offset: 0x000DF104
	public IEnumerator satyr_land_cr()
	{
		AudioManager.Play("castle_imp_spawn");
		this.emitAudioFromObject.Add("castle_imp_spawn");
		this.floating = false;
		base.Jump();
		base.StartCoroutine(this.change_layer_cr());
		while (!base.Grounded)
		{
			yield return null;
		}
		this.landing = true;
		AudioManager.Play("castle_imp_land");
		this.emitAudioFromObject.Add("castle_imp_land");
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_Continue", false, true);
		this.landing = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002F2D RID: 12077 RVA: 0x000E0F20 File Offset: 0x000DF120
	public IEnumerator change_layer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Enemies.ToString();
		base.GetComponent<SpriteRenderer>().sortingOrder = 20;
		base.GetComponent<Collider2D>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002F2E RID: 12078 RVA: 0x000274DD File Offset: 0x000256DD
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<MountainPlatformingLevelWall>())
		{
			this.Turn();
		}
	}

	// Token: 0x06002F2F RID: 12079 RVA: 0x00027504 File Offset: 0x00025704
	public override void Die()
	{
		AudioManager.Play("castle_generic_death_honk");
		this.emitAudioFromObject.Add("castle_generic_death_honk");
		base.Die();
	}
}
