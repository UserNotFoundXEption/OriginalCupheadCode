using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000222 RID: 546
public class FlowerLevelFlowerVineHand : AbstractCollidableObject
{
	// Token: 0x06001905 RID: 6405 RVA: 0x000A5440 File Offset: 0x000A3640
	public void OnVineHandSpawn(float firstHold, float secondHold, int attackPosOne, int attackPosTwo = 0)
	{
		this.holdCount = 0;
		int num = attackPosOne;
		for (int i = 0; i < 2; i++)
		{
			if (i == 1)
			{
				if (attackPosTwo == 0)
				{
					break;
				}
				num = attackPosTwo;
			}
			switch (num)
			{
			case 1:
				this.spawnPosition = new Vector3((float)this.platformOneXPosition, (float)this.vineHandSpawnYPosition, 0f);
				break;
			case 2:
				this.spawnPosition = new Vector3((float)this.platformTwoXPosition, (float)this.vineHandSpawnYPosition, 0f);
				break;
			case 3:
				this.spawnPosition = new Vector3((float)this.platformThreeXPosition, (float)this.vineHandSpawnYPosition, 0f);
				break;
			}
			this.Create(firstHold, secondHold);
		}
	}

	// Token: 0x06001906 RID: 6406 RVA: 0x00015622 File Offset: 0x00013822
	public void InitVineHand(float first, float second)
	{
		this.firstHoldDelay = first;
		this.secondHoldDelay = second;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001907 RID: 6407 RVA: 0x000A5510 File Offset: 0x000A3710
	public void Create(float first, float second)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject, this.spawnPosition, Quaternion.identity);
		gameObject.GetComponent<FlowerLevelFlowerVineHand>().InitVineHand(first, second);
	}

	// Token: 0x06001908 RID: 6408 RVA: 0x0001563D File Offset: 0x0001383D
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001909 RID: 6409 RVA: 0x00015655 File Offset: 0x00013855
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600190A RID: 6410 RVA: 0x000A5544 File Offset: 0x000A3744
	public IEnumerator holdDelay(float delay)
	{
		base.animator.SetBool("OnHold", true);
		if (delay != 0f)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		base.animator.SetBool("OnHold", false);
		yield return null;
		yield break;
	}

	// Token: 0x0600190B RID: 6411 RVA: 0x000A5568 File Offset: 0x000A3768
	public void OnHold()
	{
		if (this.holdCount == 0)
		{
			base.StartCoroutine(this.holdDelay(this.firstHoldDelay));
			this.holdCount++;
		}
		else
		{
			base.StartCoroutine(this.holdDelay(this.secondHoldDelay));
		}
	}

	// Token: 0x0600190C RID: 6412 RVA: 0x0001567E File Offset: 0x0001387E
	public void OnRetracted()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x00015691 File Offset: 0x00013891
	public void ContinueBackAnimation()
	{
		base.animator.SetTrigger("ContinueBackAnimation");
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x000156A3 File Offset: 0x000138A3
	public void SoundVineHandGrow()
	{
		AudioManager.Play("flower_vinehand_grow");
		this.emitAudioFromObject.Add("flower_vinehand_grow");
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x000156BF File Offset: 0x000138BF
	public void SoundVineHandGrowContinue()
	{
		AudioManager.Play("flower_vinehand_grow_continue");
		this.emitAudioFromObject.Add("flower_vinehand_grow_continue");
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x000156DB File Offset: 0x000138DB
	public void SoundVineHandGrowRetract()
	{
		AudioManager.Play("flower_vinehand_grow_retract");
		this.emitAudioFromObject.Add("flower_vinehand_grow_retract");
	}

	// Token: 0x0400142A RID: 5162
	[SerializeField]
	public int platformOneXPosition;

	// Token: 0x0400142B RID: 5163
	[SerializeField]
	public int platformTwoXPosition;

	// Token: 0x0400142C RID: 5164
	[SerializeField]
	public int platformThreeXPosition;

	// Token: 0x0400142D RID: 5165
	[Space(10f)]
	[SerializeField]
	public int vineHandSpawnYPosition;

	// Token: 0x0400142E RID: 5166
	public int holdCount;

	// Token: 0x0400142F RID: 5167
	public float firstHoldDelay;

	// Token: 0x04001430 RID: 5168
	public float secondHoldDelay;

	// Token: 0x04001431 RID: 5169
	public Vector3 spawnPosition;

	// Token: 0x04001432 RID: 5170
	public DamageDealer damageDealer;
}
