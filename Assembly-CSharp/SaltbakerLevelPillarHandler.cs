using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200037D RID: 893
public class SaltbakerLevelPillarHandler : LevelProperties.Saltbaker.Entity
{
	// Token: 0x0600274C RID: 10060 RVA: 0x000210A7 File Offset: 0x0001F2A7
	public void TakeDamage(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600274D RID: 10061 RVA: 0x000CB38C File Offset: 0x000C958C
	public void StartPlatforms()
	{
		this.leftPillar.transform.position = new Vector3(base.transform.position.x - base.properties.CurrentState.doomPillar.pillarPosition.min, base.transform.position.y);
		this.rightPillar.transform.position = new Vector3(base.transform.position.x + base.properties.CurrentState.doomPillar.pillarPosition.min, base.transform.position.y);
		base.StartCoroutine(this.create_platforms_cr());
		base.StartCoroutine(this.create_glass_cr());
	}

	// Token: 0x0600274E RID: 10062 RVA: 0x000CB460 File Offset: 0x000C9660
	public void StartPillarOfDoom()
	{
		LevelProperties.Saltbaker.DoomPillar doomPillar = base.properties.CurrentState.doomPillar;
		base.properties.OnBossDeath += this.Die;
		this.leftAnimator.Play("IntroA", 0, 0f);
		this.rightAnimator.Play("IntroB", 0, 0f);
		this.SFX_SALTB_P4_TornadoPillars_Loop();
		base.StartCoroutine(this.move_horizontal_cr());
	}

	// Token: 0x0600274F RID: 10063 RVA: 0x000CB4D4 File Offset: 0x000C96D4
	public void StartHeart()
	{
		this.darkHeart.gameObject.SetActive(true);
		this.darkHeart.Init(Vector3.up * 500f, this.leftPillar, this.rightPillar, base.properties.CurrentState.darkHeart, this);
	}

	// Token: 0x06002750 RID: 10064 RVA: 0x000CB52C File Offset: 0x000C972C
	public float GetPlatformFallSpeed()
	{
		return Mathf.Lerp(base.properties.CurrentState.doomPillar.platformFallSpeed.min, base.properties.CurrentState.doomPillar.platformFallSpeed.max, Mathf.InverseLerp(0f, base.properties.CurrentState.doomPillar.phaseTime, this.phaseTimer));
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x000CB598 File Offset: 0x000C9798
	public IEnumerator create_glass_cr()
	{
		this.chunkOrderString = new PatternString(this.chunkOrder, true);
		this.chunkPositionString = new PatternString(this.chunkPosition, true);
		this.chunkSpawnTimeString = new PatternString(this.chunkSpawnTime, true);
		for (;;)
		{
			float t = this.chunkSpawnTimeString.PopFloat();
			while (t > 0f)
			{
				t -= CupheadTime.Delta * Mathf.Lerp(0.5f, 1f, Mathf.InverseLerp(0f, base.properties.CurrentState.doomPillar.phaseTime, this.phaseTimer));
				yield return null;
			}
			int usableChunk = -1;
			for (int i = 0; i < this.chunks.Count; i++)
			{
				if (this.chunks[i].transform.position.y < -520f)
				{
					usableChunk = i;
					break;
				}
			}
			if (usableChunk == -1)
			{
				this.chunks.Add(Object.Instantiate<SaltbakerLevelGlassChunk>(this.chunkPrefab));
				usableChunk = this.chunks.Count - 1;
			}
			float xPos = Mathf.Lerp((float)Level.Current.Left, (float)Level.Current.Right, this.chunkPositionString.PopFloat());
			int id = this.chunkOrderString.PopInt();
			bool inBack = Rand.Bool();
			float fallSpeed = this.GetPlatformFallSpeed() + (float)((!inBack) ? Random.Range(100, 200) : Random.Range(-100, -75));
			this.chunks[usableChunk].Reset(new Vector3(xPos, 520f), fallSpeed, id < 4, this.chunkFlip[id], Rand.Bool(), inBack, id % 4);
			this.chunkFlip[id] = !this.chunkFlip[id];
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x000CB5B4 File Offset: 0x000C97B4
	public IEnumerator create_platforms_cr()
	{
		int bigCounter = 0;
		int mediumCounter = 0;
		int smallCounter = 0;
		float pillarSpawnOffset = 100f;
		LevelProperties.Saltbaker.DoomPillar p = base.properties.CurrentState.doomPillar;
		float pillarXBuffer = 146f + this.leftPillar.GetComponent<BoxCollider2D>().size.x / 2f;
		YieldInstruction wait = new WaitForFixedUpdate();
		PatternString platformSize = new PatternString(p.platformSizeString, true, true);
		PatternString platformPosX = new PatternString(p.platformXSpawnString, true, true);
		PatternString platformPosY = new PatternString(p.platformYSpawnString, true, true);
		if (p.platformXYUnified)
		{
			platformPosY.SetMainStringIndex(platformPosX.GetMainStringIndex());
			platformPosY.SetSubStringIndex(platformPosX.GetSubStringIndex());
			if (platformPosX.SubStringLength() != platformPosY.SubStringLength())
			{
				Debug.Break();
			}
			platformPosY.PopFloat();
		}
		float spawnDistance = 0f;
		for (;;)
		{
			Vector3 spawnPos = new Vector3(Mathf.Lerp(this.leftPillar.transform.position.x + pillarXBuffer, this.rightPillar.transform.position.x - pillarXBuffer, (platformPosX.PopFloat() + 1f) / 2f), (float)Level.Current.Ceiling + pillarSpawnOffset + spawnDistance);
			if (this.suppressCenterPlatforms)
			{
				spawnPos = new Vector3((!Rand.Bool()) ? (this.rightPillar.transform.position.x - pillarXBuffer) : (this.leftPillar.transform.position.x + pillarXBuffer), spawnPos.y);
			}
			spawnDistance = platformPosY.PopFloat();
			GameObject platform = null;
			char c = platformSize.PopLetter();
			if (c != 'L')
			{
				if (c != 'M')
				{
					if (c != 'S')
					{
						Debug.LogError("Pattern string is incorrect.", null);
						Debug.Break();
					}
					else
					{
						platform = Object.Instantiate<GameObject>(this.smallPlatform[smallCounter % 2]);
						platform.transform.GetChild(0).localScale = new Vector3((float)((smallCounter % 4 >= 2) ? -1 : 1), 1f);
						smallCounter++;
					}
				}
				else
				{
					platform = Object.Instantiate<GameObject>(this.medPlatform[mediumCounter % 2]);
					platform.transform.GetChild(0).localScale = new Vector3((float)((mediumCounter % 4 >= 2) ? -1 : 1), 1f);
					mediumCounter++;
				}
			}
			else
			{
				platform = Object.Instantiate<GameObject>(this.bigPlatform[bigCounter % 2]);
				platform.transform.GetChild(0).localScale = new Vector3((float)((bigCounter % 4 >= 2) ? -1 : 1), 1f);
				bigCounter++;
			}
			platform.transform.position = spawnPos;
			this.platforms.Add(platform.gameObject);
			while (spawnDistance > 0f)
			{
				spawnDistance -= CupheadTime.FixedDelta * this.GetPlatformFallSpeed();
				yield return wait;
			}
		}
		yield break;
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x000CB5D0 File Offset: 0x000C97D0
	public IEnumerator move_horizontal_cr()
	{
		LevelProperties.Saltbaker.DoomPillar p = base.properties.CurrentState.doomPillar;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			float t = Mathf.InverseLerp(0f, p.phaseTime, this.phaseTimer);
			this.leftPillar.transform.position = Vector3.Lerp(new Vector3(base.transform.position.x - p.pillarPosition.min, base.transform.position.y), new Vector3(base.transform.position.x - p.pillarPosition.max, base.transform.position.y), t);
			this.rightPillar.transform.position = Vector3.Lerp(new Vector3(base.transform.position.x + p.pillarPosition.min, base.transform.position.y), new Vector3(base.transform.position.x + p.pillarPosition.max, base.transform.position.y), t);
			this.phaseTimer += CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x000CB5EC File Offset: 0x000C97EC
	public void Update()
	{
		this.platforms.RemoveAll((GameObject g) => g == null);
		foreach (GameObject gameObject in this.platforms)
		{
			if (gameObject.transform.position.y < (float)Level.Current.Ground - 400f)
			{
				Object.Destroy(gameObject.gameObject);
			}
			else
			{
				gameObject.transform.position += Vector3.down * this.GetPlatformFallSpeed() * CupheadTime.Delta;
			}
		}
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (levelPlayerController)
			{
				levelPlayerController.animationController.spriteRenderer.sortingOrder = ((levelPlayerController.motor.MoveDirection.y <= 0) ? 10 : 510);
			}
		}
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x000210BA File Offset: 0x0001F2BA
	public void Die()
	{
		this.leftAnimator.Play("Death", -1, 0f);
		this.rightAnimator.Play("Death", -1, 0.5f);
		this.SFX_SALTB_P4_TornadoPillar_LoopStop();
		this.darkHeart.Die();
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x000210F9 File Offset: 0x0001F2F9
	public void SFX_SALTB_P4_TornadoPillars_Loop()
	{
		AudioManager.PlayLoop("sfx_DLC_Saltbaker_P4_Tornado_Left_Loop");
		AudioManager.PlayLoop("sfx_DLC_Saltbaker_P4_Tornado_Right_Loop");
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x0002110F File Offset: 0x0001F30F
	public void SFX_SALTB_P4_TornadoPillar_LoopStop()
	{
		AudioManager.Stop("sfx_DLC_Saltbaker_P4_Tornado_Left_Loop");
		AudioManager.Stop("sfx_DLC_Saltbaker_P4_Tornado_Right_Loop");
	}

	// Token: 0x04002083 RID: 8323
	[Header("Other")]
	[SerializeField]
	public GameObject leftPillar;

	// Token: 0x04002084 RID: 8324
	[SerializeField]
	public GameObject rightPillar;

	// Token: 0x04002085 RID: 8325
	[SerializeField]
	public Animator leftAnimator;

	// Token: 0x04002086 RID: 8326
	[SerializeField]
	public Animator rightAnimator;

	// Token: 0x04002087 RID: 8327
	[SerializeField]
	public SaltbakerLevelHeart darkHeart;

	// Token: 0x04002088 RID: 8328
	[SerializeField]
	public GameObject[] smallPlatform;

	// Token: 0x04002089 RID: 8329
	[SerializeField]
	public GameObject[] medPlatform;

	// Token: 0x0400208A RID: 8330
	[SerializeField]
	public GameObject[] bigPlatform;

	// Token: 0x0400208B RID: 8331
	public List<GameObject> platforms = new List<GameObject>();

	// Token: 0x0400208C RID: 8332
	[SerializeField]
	public SaltbakerLevelGlassChunk chunkPrefab;

	// Token: 0x0400208D RID: 8333
	public List<SaltbakerLevelGlassChunk> chunks = new List<SaltbakerLevelGlassChunk>();

	// Token: 0x0400208E RID: 8334
	[SerializeField]
	public string chunkOrder;

	// Token: 0x0400208F RID: 8335
	public PatternString chunkOrderString;

	// Token: 0x04002090 RID: 8336
	[SerializeField]
	public string chunkPosition;

	// Token: 0x04002091 RID: 8337
	public PatternString chunkPositionString;

	// Token: 0x04002092 RID: 8338
	[SerializeField]
	public string chunkSpawnTime;

	// Token: 0x04002093 RID: 8339
	public PatternString chunkSpawnTimeString;

	// Token: 0x04002094 RID: 8340
	public bool[] chunkFlip = new bool[8];

	// Token: 0x04002095 RID: 8341
	public float phaseTimer;

	// Token: 0x04002096 RID: 8342
	public bool suppressCenterPlatforms;
}
