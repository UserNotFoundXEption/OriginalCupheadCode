using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003EE RID: 1006
public class TreePlatformingLevelAudioArea : AbstractPausableComponent
{
	// Token: 0x06002C35 RID: 11317 RVA: 0x000250B8 File Offset: 0x000232B8
	public void Start()
	{
		base.StartCoroutine(this.check_sound_cr());
	}

	// Token: 0x06002C36 RID: 11318 RVA: 0x000250C7 File Offset: 0x000232C7
	public void PlaySound()
	{
		AudioManager.PlayLoop("amb_treecave");
		base.StartCoroutine(this.fade_volume_cr(true));
	}

	// Token: 0x06002C37 RID: 11319 RVA: 0x000250E1 File Offset: 0x000232E1
	public void StopSound()
	{
		base.StartCoroutine(this.fade_volume_cr(false));
	}

	// Token: 0x06002C38 RID: 11320 RVA: 0x000D94A8 File Offset: 0x000D76A8
	public IEnumerator fade_volume_cr(bool fadeIn)
	{
		this.isFading = true;
		float time = 1f;
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			AudioManager.Attenuation("amb_treecave", true, (!fadeIn) ? (1f - t / time) : (t / time));
			yield return null;
		}
		if (!fadeIn)
		{
			AudioManager.Stop("amb_treecave");
		}
		this.isFading = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002C39 RID: 11321 RVA: 0x000D94CC File Offset: 0x000D76CC
	public IEnumerator check_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AbstractPlayerController player = PlayerManager.GetNext();
		for (;;)
		{
			if (player.transform.position.x > this.startPoint.transform.position.x && player.transform.position.x < this.endPoint.transform.position.x)
			{
				if (!AudioManager.CheckIfPlaying("amb_treecave"))
				{
					this.PlaySound();
				}
			}
			else if (AudioManager.CheckIfPlaying("amb_treecave") && !this.isFading)
			{
				this.StopSound();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C3A RID: 11322 RVA: 0x000D94E8 File Offset: 0x000D76E8
	public IEnumerator play_one_shots_cr()
	{
		MinMax delay = new MinMax(4f, 8f);
		for (;;)
		{
			if (AudioManager.CheckIfPlaying("amb_treecave"))
			{
				yield return CupheadTime.WaitForSeconds(this, delay);
				AudioManager.Play("NAME");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C3B RID: 11323 RVA: 0x000D9504 File Offset: 0x000D7704
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector2(this.startPoint.position.x, this.startPoint.position.y + 1000f), new Vector2(this.startPoint.position.x, this.startPoint.position.y - 1000f));
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(new Vector2(this.endPoint.position.x, this.endPoint.position.y + 1000f), new Vector2(this.endPoint.position.x, this.endPoint.position.y - 1000f));
	}

	// Token: 0x04002485 RID: 9349
	[SerializeField]
	public Transform startPoint;

	// Token: 0x04002486 RID: 9350
	[SerializeField]
	public Transform endPoint;

	// Token: 0x04002487 RID: 9351
	public bool isFading;
}
