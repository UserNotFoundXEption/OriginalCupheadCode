using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043C RID: 1084
public class MountainPlatformingLevelCyclopsPeek : AbstractPausableComponent
{
	// Token: 0x06002EA1 RID: 11937 RVA: 0x00026E3B File Offset: 0x0002503B
	public void Start()
	{
		base.StartCoroutine(this.check_player_cr());
		this.emitAudioFromObject.Add("castle_giant_head_peer");
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x000DFDE8 File Offset: 0x000DDFE8
	public IEnumerator check_player_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.player = PlayerManager.GetNext();
		float t = 0f;
		float time = Random.Range(3f, 6f);
		float laughTime = Random.Range(6f, 10f);
		float t_laugh = 0f;
		for (;;)
		{
			if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				t += CupheadTime.Delta;
				if (t >= time)
				{
					this.player = PlayerManager.GetNext();
					t = 0f;
					time = Random.Range(3f, 6f);
				}
			}
			if (Vector3.Distance(PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position, base.transform.position) < 1000f)
			{
				t_laugh += CupheadTime.Delta;
				if (t_laugh >= laughTime)
				{
					this.SoundGiantHeadPeer();
					t_laugh = 0f;
					laughTime = Random.Range(6f, 10f);
				}
			}
			if (this.player.transform.position.x < base.transform.position.x - this.offset)
			{
				if (this.currentEyeState != MountainPlatformingLevelCyclopsPeek.EyeState.left)
				{
					base.animator.SetInteger("SideOn", 0);
					this.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.left;
				}
			}
			else if (this.player.transform.position.x > base.transform.position.x + this.offset)
			{
				if (this.currentEyeState != MountainPlatformingLevelCyclopsPeek.EyeState.right)
				{
					base.animator.SetInteger("SideOn", 2);
					this.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.right;
				}
			}
			else if (this.currentEyeState != MountainPlatformingLevelCyclopsPeek.EyeState.middle)
			{
				base.animator.SetInteger("SideOn", 1);
				this.currentEyeState = MountainPlatformingLevelCyclopsPeek.EyeState.middle;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x00026E5A File Offset: 0x0002505A
	public void SoundGiantHeadPeer()
	{
		AudioManager.Play("castle_giant_head_peer");
		this.emitAudioFromObject.Add("castle_giant_head_peer");
	}

	// Token: 0x040026AD RID: 9901
	public MountainPlatformingLevelCyclopsPeek.EyeState currentEyeState;

	// Token: 0x040026AE RID: 9902
	public float offset = 300f;

	// Token: 0x040026AF RID: 9903
	public AbstractPlayerController player;

	// Token: 0x0200109C RID: 4252
	public enum EyeState
	{
		// Token: 0x040075FE RID: 30206
		left,
		// Token: 0x040075FF RID: 30207
		middle,
		// Token: 0x04007600 RID: 30208
		right
	}
}
