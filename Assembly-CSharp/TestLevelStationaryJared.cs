using System;
using UnityEngine;

// Token: 0x0200011F RID: 287
public class TestLevelStationaryJared : LevelProperties.Test.Entity
{
	// Token: 0x06000D92 RID: 3474 RVA: 0x0000B964 File Offset: 0x00009B64
	public override void LevelInit(LevelProperties.Test properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06000D93 RID: 3475 RVA: 0x0000B96D File Offset: 0x00009B6D
	public void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06000D94 RID: 3476 RVA: 0x0000B992 File Offset: 0x00009B92
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		AudioManager.Play("test_sound_2");
	}

	// Token: 0x06000D95 RID: 3477 RVA: 0x0000B99E File Offset: 0x00009B9E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06000D96 RID: 3478 RVA: 0x0000B9A8 File Offset: 0x00009BA8
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		player.stats.OnParry(1f, true);
	}

	// Token: 0x04000A97 RID: 2711
	[SerializeField]
	public Transform childSprite;

	// Token: 0x04000A98 RID: 2712
	public DamageReceiver damageReceiver;
}
