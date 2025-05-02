using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000438 RID: 1080
public class HarbourPlatformingLevelUrchin : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06002E78 RID: 11896 RVA: 0x00026BF5 File Offset: 0x00024DF5
	public override void Start()
	{
		base.Start();
		base.GetComponent<PlatformingLevelEnemyAnimationHandler>().SelectAnimation(this.type.ToString());
		base.StartCoroutine(this.play_loop_SFX());
	}

	// Token: 0x06002E79 RID: 11897 RVA: 0x000DFA9C File Offset: 0x000DDC9C
	public IEnumerator play_loop_SFX()
	{
		bool playerLeft = false;
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
			{
				playerLeft = false;
				if (!AudioManager.CheckIfPlaying("harbour_urchin_walk"))
				{
					AudioManager.PlayLoop("harbour_urchin_walk");
					this.emitAudioFromObject.Add("harbour_urchin_walk");
				}
			}
			else if (!playerLeft)
			{
				AudioManager.Stop("harbour_urchin_walk");
				playerLeft = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x000DFAB8 File Offset: 0x000DDCB8
	public override Coroutine Turn()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
		{
			AudioManager.Play("harbour_urchin_turn");
			this.emitAudioFromObject.Add("harbour_urchin_turn");
		}
		return base.Turn();
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x00026C26 File Offset: 0x00024E26
	public override void Die()
	{
		base.Die();
		AudioManager.Stop("harbour_urchin_walk");
		AudioManager.Play("harmour_urchin_death");
		this.emitAudioFromObject.Add("harmour_urchin_death");
	}

	// Token: 0x04002695 RID: 9877
	public const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x04002696 RID: 9878
	public HarbourPlatformingLevelUrchin.Type type;

	// Token: 0x04002697 RID: 9879
	public bool isInSight;

	// Token: 0x02001093 RID: 4243
	public enum Type
	{
		// Token: 0x040075D0 RID: 30160
		A,
		// Token: 0x040075D1 RID: 30161
		B
	}
}
