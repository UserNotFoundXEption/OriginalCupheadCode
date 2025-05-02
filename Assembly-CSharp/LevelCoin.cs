using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000114 RID: 276
[DefaultExecutionOrder(100)]
[RequireComponent(typeof(Animator))]
public class LevelCoin : AbstractCollidableObject
{
	// Token: 0x06000D4F RID: 3407 RVA: 0x0000B6BF File Offset: 0x000098BF
	public static void OnLevelStart()
	{
		PlayerData.Data.ResetLevelCoinManager();
	}

	// Token: 0x06000D50 RID: 3408 RVA: 0x0000B6CB File Offset: 0x000098CB
	public static void OnLevelComplete()
	{
		PlayerData.Data.ApplyLevelCoins();
	}

	// Token: 0x17000220 RID: 544
	// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00086ECC File Offset: 0x000850CC
	public string GlobalID
	{
		get
		{
			return SceneManager.GetActiveScene().name + "::" + base.gameObject.name;
		}
	}

	// Token: 0x06000D52 RID: 3410 RVA: 0x00086EFC File Offset: 0x000850FC
	public override void Awake()
	{
		PlatformingLevel platformingLevel = Level.Current as PlatformingLevel;
		if (platformingLevel)
		{
			platformingLevel.LevelCoinsIDs.Add(new CoinPositionAndID(this.GlobalID, base.transform.position.x));
		}
		base.Awake();
		if (PlayerData.Data.GetCoinCollected(this))
		{
			this._collected = true;
			Object.Destroy(base.gameObject);
			return;
		}
		this._spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000D53 RID: 3411 RVA: 0x00086F80 File Offset: 0x00085180
	public void Update()
	{
		if (this._collected)
		{
			return;
		}
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null && Vector2.Distance(base.transform.position, player.center) < 100f)
		{
			this.Collect(player.id);
			return;
		}
		if (player2 != null && Vector2.Distance(base.transform.position, player2.center) < 100f)
		{
			this.Collect(player2.id);
			return;
		}
	}

	// Token: 0x06000D54 RID: 3412 RVA: 0x00087030 File Offset: 0x00085230
	public void Collect(PlayerId player)
	{
		if (this._collected)
		{
			return;
		}
		PlayerData.Data.SetLevelCoinCollected(this, true, player);
		this._collected = true;
		AudioManager.Play("level_coin_pickup");
		base.animator.SetTrigger("OnDeath");
		base.transform.localScale *= 1.2f;
		this._spriteRenderer.flipX = MathUtils.RandomBool();
		this._spriteRenderer.flipY = MathUtils.RandomBool();
	}

	// Token: 0x06000D55 RID: 3413 RVA: 0x0000B6D7 File Offset: 0x000098D7
	public void OnDeathAnimComplete()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000A6D RID: 2669
	public const float COLLECT_RANGE = 100f;

	// Token: 0x04000A6E RID: 2670
	public const int NUM_COINS = 40;

	// Token: 0x04000A6F RID: 2671
	public SpriteRenderer _spriteRenderer;

	// Token: 0x04000A70 RID: 2672
	public bool _collected;
}
