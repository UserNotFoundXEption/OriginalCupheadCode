using System;
using UnityEngine;

// Token: 0x02000475 RID: 1141
public class MapAudioTimerZone : AbstractCollidableObject
{
	// Token: 0x06003074 RID: 12404 RVA: 0x0002838C File Offset: 0x0002658C
	public void Start()
	{
		this.waitTime = Random.Range(this.audioDelayRange.minimum, this.audioDelayRange.maximum);
	}

	// Token: 0x06003075 RID: 12405 RVA: 0x000E6188 File Offset: 0x000E4388
	public void Update()
	{
		if (this.playerCount > 0)
		{
			this.elapsedTime += CupheadTime.Delta;
			if (this.elapsedTime > this.waitTime)
			{
				AudioManager.Play(this.audioKey);
				this.elapsedTime = 0f;
				this.waitTime = Random.Range(this.audioDelayRange.minimum, this.audioDelayRange.maximum);
			}
		}
	}

	// Token: 0x06003076 RID: 12406 RVA: 0x000E6200 File Offset: 0x000E4400
	public override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (!hit.CompareTag("Player_Map"))
		{
			return;
		}
		if (phase == CollisionPhase.Enter)
		{
			this.playerCount++;
		}
		else if (phase == CollisionPhase.Exit)
		{
			this.playerCount--;
		}
	}

	// Token: 0x04002813 RID: 10259
	[SerializeField]
	public string audioKey;

	// Token: 0x04002814 RID: 10260
	[SerializeField]
	public Rangef audioDelayRange;

	// Token: 0x04002815 RID: 10261
	public int playerCount;

	// Token: 0x04002816 RID: 10262
	public float elapsedTime;

	// Token: 0x04002817 RID: 10263
	public float waitTime;
}
