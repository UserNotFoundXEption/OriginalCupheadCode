using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000367 RID: 871
public class SallyStagePlayLevelWave : AbstractCollidableObject
{
	// Token: 0x17000320 RID: 800
	// (get) Token: 0x06002690 RID: 9872 RVA: 0x000205BB File Offset: 0x0001E7BB
	// (set) Token: 0x06002691 RID: 9873 RVA: 0x000205C3 File Offset: 0x0001E7C3
	public bool isMoving { get; set; }

	// Token: 0x06002692 RID: 9874 RVA: 0x000205CC File Offset: 0x0001E7CC
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.startPos = base.transform.position;
	}

	// Token: 0x06002693 RID: 9875 RVA: 0x000205EA File Offset: 0x0001E7EA
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002694 RID: 9876 RVA: 0x00020602 File Offset: 0x0001E802
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002695 RID: 9877 RVA: 0x00020620 File Offset: 0x0001E820
	public void StartWave(LevelProperties.SallyStagePlay.Tidal properties)
	{
		this.properties = properties;
		base.transform.position = this.startPos;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002696 RID: 9878 RVA: 0x000C9514 File Offset: 0x000C7714
	public IEnumerator move_cr()
	{
		float sizeX = base.GetComponent<Renderer>().bounds.size.x;
		this.isMoving = true;
		while (base.transform.position.x < 640f + sizeX)
		{
			base.transform.position += base.transform.right * this.properties.tidalSpeed * CupheadTime.Delta;
			yield return null;
		}
		this.isMoving = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002697 RID: 9879 RVA: 0x00020647 File Offset: 0x0001E847
	public void SoundBigWaveFeet()
	{
		if (this.isMoving)
		{
			AudioManager.Play("sally_wave");
			this.emitAudioFromObject.Add("sally_wave");
		}
	}

	// Token: 0x06002698 RID: 9880 RVA: 0x0002066E File Offset: 0x0001E86E
	public void SoundBigWaveVoice()
	{
		if (this.isMoving)
		{
			AudioManager.Play("sally_wave_sweet");
			this.emitAudioFromObject.Add("sally_wave_sweet");
		}
	}

	// Token: 0x04001FD2 RID: 8146
	public DamageDealer damageDealer;

	// Token: 0x04001FD3 RID: 8147
	public LevelProperties.SallyStagePlay.Tidal properties;

	// Token: 0x04001FD4 RID: 8148
	public Vector3 startPos;
}
