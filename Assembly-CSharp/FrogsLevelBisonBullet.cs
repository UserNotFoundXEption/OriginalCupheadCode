using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000299 RID: 665
public class FrogsLevelBisonBullet : AbstractFrogsLevelSlotBullet
{
	// Token: 0x170002C0 RID: 704
	// (get) Token: 0x06001E03 RID: 7683 RVA: 0x00019547 File Offset: 0x00017747
	public override EaseUtils.EaseType Y_Ease
	{
		get
		{
			return EaseUtils.EaseType.easeOutElastic;
		}
	}

	// Token: 0x170002C1 RID: 705
	// (get) Token: 0x06001E04 RID: 7684 RVA: 0x0001954B File Offset: 0x0001774B
	public override float Y
	{
		get
		{
			return -60f;
		}
	}

	// Token: 0x170002C2 RID: 706
	// (get) Token: 0x06001E05 RID: 7685 RVA: 0x00019552 File Offset: 0x00017752
	public override float Y_Time
	{
		get
		{
			return 2f;
		}
	}

	// Token: 0x06001E06 RID: 7686 RVA: 0x000B23E4 File Offset: 0x000B05E4
	public FrogsLevelBisonBullet Create(Vector2 pos, float s, FrogsLevelBisonBullet.Direction direction, float bigX, float smallX)
	{
		FrogsLevelBisonBullet frogsLevelBisonBullet = base.Create(pos, s) as FrogsLevelBisonBullet;
		frogsLevelBisonBullet.Init(direction, bigX, smallX);
		return frogsLevelBisonBullet;
	}

	// Token: 0x06001E07 RID: 7687 RVA: 0x000B240C File Offset: 0x000B060C
	public void Init(FrogsLevelBisonBullet.Direction dir, float big, float small)
	{
		this.flame.GetComponent<Collider2D>().enabled = false;
		this.flame.GetComponent<CollisionChild>().OnPlayerCollision += base.DealDamage;
		this.direction = dir;
		this.bigX = big;
		base.StartCoroutine(this.bison_cr());
		base.StartCoroutine(this.small_cr());
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x000B2470 File Offset: 0x000B0670
	public IEnumerator small_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		this.flame.GetComponent<Collider2D>().enabled = true;
		base.animator.SetTrigger("Small");
		yield break;
	}

	// Token: 0x06001E09 RID: 7689 RVA: 0x000B248C File Offset: 0x000B068C
	public IEnumerator bison_cr()
	{
		if (this.direction == FrogsLevelBisonBullet.Direction.Down)
		{
			this.flame.SetEulerAngles(new float?(0f), new float?(0f), new float?(180f));
			this.flame.AddLocalPosition(0f, -115f, 0f);
			this.flame.GetComponent<SpriteRenderer>().sortingOrder = base.GetComponent<SpriteRenderer>().sortingOrder - 1;
		}
		yield return null;
		yield return null;
		yield return null;
		bool big = false;
		for (;;)
		{
			float distance = float.MaxValue;
			AbstractPlayerController p = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			AbstractPlayerController p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (p != null)
			{
				distance = Mathf.Min(distance, base.transform.position.x - p.center.x);
			}
			if (p2 != null)
			{
				distance = Mathf.Min(distance, base.transform.position.x - p2.center.x);
			}
			if (distance <= this.bigX && !big)
			{
				big = true;
				AudioManager.Play("level_frogs_flame_platform_fire_burst");
				this.emitAudioFromObject.Add("level_frogs_flame_platform_fire_burst");
				AudioManager.PlayLoop("level_frogs_flame_platform_fire_loop");
				this.emitAudioFromObject.Add("level_frogs_flame_platform_fire_loop");
				this.flame.GetComponent<Collider2D>().enabled = true;
				base.animator.SetTrigger("Big");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E0A RID: 7690 RVA: 0x00019559 File Offset: 0x00017759
	public override void End()
	{
		AudioManager.Stop("level_frogs_flame_platform_fire_loop");
		base.End();
	}

	// Token: 0x04001899 RID: 6297
	public Transform flame;

	// Token: 0x0400189A RID: 6298
	public FrogsLevelBisonBullet.Direction direction;

	// Token: 0x0400189B RID: 6299
	public float bigX;

	// Token: 0x02000D62 RID: 3426
	public enum Direction
	{
		// Token: 0x040060F6 RID: 24822
		Up,
		// Token: 0x040060F7 RID: 24823
		Down
	}
}
