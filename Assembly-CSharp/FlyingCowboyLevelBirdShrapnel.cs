using System;
using UnityEngine;

// Token: 0x02000253 RID: 595
public class FlyingCowboyLevelBirdShrapnel : BasicProjectile
{
	// Token: 0x06001B20 RID: 6944 RVA: 0x000AA758 File Offset: 0x000A8958
	public override AbstractProjectile Create()
	{
		AbstractProjectile abstractProjectile = base.Create();
		abstractProjectile.animator.Update(0f);
		abstractProjectile.animator.Play(0, 0, Random.Range(0f, 1f));
		abstractProjectile.animator.Update(0f);
		abstractProjectile.animator.RoundFrame(0);
		abstractProjectile.GetComponent<SpriteRenderer>().flipY = Rand.Bool();
		return abstractProjectile;
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x0001701F File Offset: 0x0001521F
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		AudioManager.Play("sfx_dlc_cowgirl_p1_dynamitehitplayer");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_dynamitehitplayer");
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x00017043 File Offset: 0x00015243
	public void animationEvent_LoopMiddleReached()
	{
		this.trailBRenderer.enabled = true;
		this.trailBRenderer.flipY = Rand.Bool();
	}

	// Token: 0x06001B23 RID: 6947 RVA: 0x00017061 File Offset: 0x00015261
	public void animationEvent_LoopEndReached()
	{
		this.trailARenderer.flipY = Rand.Bool();
	}

	// Token: 0x040015EA RID: 5610
	[SerializeField]
	public SpriteRenderer trailARenderer;

	// Token: 0x040015EB RID: 5611
	[SerializeField]
	public SpriteRenderer trailBRenderer;
}
