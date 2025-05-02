using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200021E RID: 542
public class FlowerLevelEnemySeed : AbstractProjectile
{
	// Token: 0x060018A8 RID: 6312 RVA: 0x000A44C8 File Offset: 0x000A26C8
	public void OnSeedSpawn(LevelProperties.Flower properties, FlowerLevelFlower parent, char type, bool isActive)
	{
		this.isActive = isActive;
		this.properties = properties;
		switch (type)
		{
		case 'A':
			base.animator.SetInteger("Type", 1);
			break;
		case 'B':
			base.animator.SetInteger("Type", 0);
			break;
		case 'C':
			base.animator.SetInteger("Type", 2);
			this.SetParryable(true);
			break;
		}
		this.fallingSpeed = properties.CurrentState.enemyPlants.fallingSeedSpeed;
		this.type = type;
		this.parent = parent;
		this.parent.OnDeathEvent += this.KillSeed;
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x000150D6 File Offset: 0x000132D6
	public void OnSeedLand()
	{
		base.StartCoroutine(this.onSeedLand_cr());
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x000A4588 File Offset: 0x000A2788
	public IEnumerator onSeedLand_cr()
	{
		if (this.type == 'B')
		{
			if (!this.plantSpawned)
			{
				base.animator.Play("Chomper_Landing");
				yield return base.animator.WaitForAnimationToEnd(this, "Chomper_Landing", false, true);
				if (this.isActive)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.chomperSpawn);
					gameObject.transform.position = base.transform.position;
					gameObject.GetComponent<FlowerLevelChomperSeed>().OnChomperStart(this.parent, this.properties.CurrentState.enemyPlants);
					this.plantSpawned = true;
					base.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			base.animator.SetTrigger("Landed");
			yield return new WaitForEndOfFrame();
			yield return base.animator.WaitForAnimationToEnd(this, true);
			base.animator.Play("Ground_Burst_Start");
		}
		yield break;
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x000150E5 File Offset: 0x000132E5
	public void KillSeed()
	{
		this.isActive = false;
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x000A45A4 File Offset: 0x000A27A4
	public override void Awake()
	{
		this.isActive = true;
		base.transform.localScale = new Vector3(base.transform.localScale.x * (float)MathUtils.PlusOrMinus(), base.transform.localScale.y, base.transform.localScale.z);
		base.Awake();
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x000150EE File Offset: 0x000132EE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x0001510C File Offset: 0x0001330C
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x0001512A File Offset: 0x0001332A
	public override void FixedUpdate()
	{
		base.transform.position += -Vector3.up * ((float)this.fallingSpeed * CupheadTime.FixedDelta);
		base.FixedUpdate();
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x000A4610 File Offset: 0x000A2810
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (!this.plantSpawned)
		{
			if (this.isActive)
			{
				this.OnSeedLand();
			}
			else if (this.type == 'C')
			{
				this.type = 'A';
				this.OnSeedLand();
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
			this.fallingSpeed = 0;
			if (base.CanParry)
			{
				this.SetParryable(false);
			}
			this.plantSpawned = true;
		}
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x060018B1 RID: 6321 RVA: 0x000A4694 File Offset: 0x000A2894
	public override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (hit.GetComponent<FlowerLevelFlowerDamageRegion>() != null)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			switch (this.type)
			{
			case 'A':
				if (hit.GetComponent<FlowerLevelVenusSpawn>() != null)
				{
					this.isActive = false;
				}
				break;
			case 'B':
				if (hit.GetComponent<FlowerLevelChomperSeed>() != null)
				{
					this.isActive = false;
				}
				break;
			case 'C':
				if (hit.GetComponent<FlowerLevelMiniFlowerSpawn>() != null)
				{
					this.isActive = false;
				}
				break;
			}
		}
		base.OnCollisionEnemyProjectile(hit, phase);
	}

	// Token: 0x060018B2 RID: 6322 RVA: 0x00015164 File Offset: 0x00013364
	public override void Die()
	{
		base.Die();
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		this.parent.OnMiniFlowerDeath();
	}

	// Token: 0x060018B3 RID: 6323 RVA: 0x00015189 File Offset: 0x00013389
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.KillSeed;
		this.homingVenusFlyTrapSpawn = null;
		this.chomperSpawn = null;
		this.miniFlowerSpawn = null;
	}

	// Token: 0x060018B4 RID: 6324 RVA: 0x000A4748 File Offset: 0x000A2948
	public void OnSpawnPlant()
	{
		char c = this.type;
		if (c != 'A')
		{
			if (c == 'C')
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.miniFlowerSpawn);
				gameObject.transform.position = this.spawnPoint.transform.position;
				gameObject.GetComponent<FlowerLevelMiniFlowerSpawn>().OnMiniFlowerSpawn(this.parent, this.properties.CurrentState.enemyPlants);
				gameObject.transform.localScale = base.transform.localScale;
			}
		}
		else
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.homingVenusFlyTrapSpawn);
			gameObject.transform.position = this.spawnPoint.transform.position;
			gameObject.GetComponent<FlowerLevelVenusSpawn>().OnVenusSpawn(this.parent, this.properties.CurrentState.enemyPlants.venusPlantHP, (float)this.properties.CurrentState.enemyPlants.venusTurningSpeed, this.properties.CurrentState.enemyPlants.venusMovmentSpeed, this.properties.CurrentState.enemyPlants.venusTurningDelay);
			gameObject.transform.localScale = base.transform.localScale;
		}
		this.plantSpawned = true;
	}

	// Token: 0x060018B5 RID: 6325 RVA: 0x000151BD File Offset: 0x000133BD
	public void TriggerVine()
	{
		base.StartCoroutine(this.triggerVine_cr());
	}

	// Token: 0x060018B6 RID: 6326 RVA: 0x000A4888 File Offset: 0x000A2A88
	public IEnumerator triggerVine_cr()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		base.animator.Play("Trigger_Vine", 1);
		yield break;
	}

	// Token: 0x060018B7 RID: 6327 RVA: 0x000151CC File Offset: 0x000133CC
	public void OnDeath()
	{
		if (this.plantSpawned)
		{
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060018B8 RID: 6328 RVA: 0x000151E4 File Offset: 0x000133E4
	public void GroundPopAudio()
	{
		AudioManager.Play("flower_vine_groundburst_start");
		this.emitAudioFromObject.Add("flower_vine_groundburst_start");
	}

	// Token: 0x060018B9 RID: 6329 RVA: 0x00015200 File Offset: 0x00013400
	public void VineGrowLargeAudio()
	{
		AudioManager.Play("flower_venus_vine_grow_large");
		this.emitAudioFromObject.Add("flower_venus_vine_grow_large");
	}

	// Token: 0x060018BA RID: 6330 RVA: 0x0001521C File Offset: 0x0001341C
	public void VineGrowMediumAudio()
	{
		AudioManager.Play("flower_venus_vine_grow_medium");
		this.emitAudioFromObject.Add("flower_venus_vine_grow_medium");
	}

	// Token: 0x060018BB RID: 6331 RVA: 0x00015238 File Offset: 0x00013438
	public void VineGrowSmallAudio()
	{
		AudioManager.Play("flower_venus_vine_grow_small");
		this.emitAudioFromObject.Add("flower_venus_vine_grow_small");
	}

	// Token: 0x040013F6 RID: 5110
	public int fallingSpeed;

	// Token: 0x040013F7 RID: 5111
	public char type;

	// Token: 0x040013F8 RID: 5112
	public bool isActive;

	// Token: 0x040013F9 RID: 5113
	public bool plantSpawned;

	// Token: 0x040013FA RID: 5114
	public LevelProperties.Flower properties;

	// Token: 0x040013FB RID: 5115
	public FlowerLevelFlower parent;

	// Token: 0x040013FC RID: 5116
	[SerializeField]
	public GameObject spawnPoint;

	// Token: 0x040013FD RID: 5117
	[Space(10f)]
	[Header("Venus Fly Trap")]
	[SerializeField]
	public Sprite venusSeedTex;

	// Token: 0x040013FE RID: 5118
	[SerializeField]
	public GameObject homingVenusFlyTrapSpawn;

	// Token: 0x040013FF RID: 5119
	[Space(10f)]
	[Header("Chomper")]
	[SerializeField]
	public Sprite chomperSeedTex;

	// Token: 0x04001400 RID: 5120
	[SerializeField]
	public GameObject chomperSpawn;

	// Token: 0x04001401 RID: 5121
	[Space(10f)]
	[Header("Mini Flower")]
	[SerializeField]
	public Sprite miniFlowerSeedTex;

	// Token: 0x04001402 RID: 5122
	[SerializeField]
	public GameObject miniFlowerSpawn;
}
