using System;
using UnityEngine;

// Token: 0x020004A5 RID: 1189
public class MapSpritePlaySound : AbstractCollidableObject
{
	// Token: 0x0600317E RID: 12670 RVA: 0x000292A2 File Offset: 0x000274A2
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
	}

	// Token: 0x0600317F RID: 12671 RVA: 0x000E9F54 File Offset: 0x000E8154
	public void PlaySoundRight(bool isP1)
	{
		MapSpritePlaySound.SoundToPlay soundToPlay = this.getSound;
		if (soundToPlay != MapSpritePlaySound.SoundToPlay.Wood)
		{
			if (soundToPlay != MapSpritePlaySound.SoundToPlay.Rainbow)
			{
			}
		}
		else
		{
			AudioManager.Play((!isP1) ? "player_map_walk_wood_two_p2" : "player_map_walk_wood_two_p1");
		}
	}

	// Token: 0x06003180 RID: 12672 RVA: 0x000E9FA0 File Offset: 0x000E81A0
	public void PlaySoundLeft(bool isP1)
	{
		MapSpritePlaySound.SoundToPlay soundToPlay = this.getSound;
		if (soundToPlay != MapSpritePlaySound.SoundToPlay.Wood)
		{
			if (soundToPlay != MapSpritePlaySound.SoundToPlay.Rainbow)
			{
			}
		}
		else
		{
			AudioManager.Play((!isP1) ? "player_map_walk_wood_one_p2" : "player_map_walk_wood_one_p1");
		}
	}

	// Token: 0x040028C1 RID: 10433
	public MapSpritePlaySound.SoundToPlay getSound;

	// Token: 0x02001107 RID: 4359
	public enum SoundToPlay
	{
		// Token: 0x0400786D RID: 30829
		Wood,
		// Token: 0x0400786E RID: 30830
		Rainbow
	}
}
