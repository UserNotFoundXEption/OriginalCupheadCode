using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000279 RID: 633
public class FlyingGenieLevelSphinxPiece : AbstractProjectile
{
	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x000187C0 File Offset: 0x000169C0
	public override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001CE8 RID: 7400 RVA: 0x000AF680 File Offset: 0x000AD880
	public void StartMoving(LevelProperties.FlyingGenie.Sphinx properties, AbstractPlayerController player, int maxCounter, bool moveRight, string[] pinkPattern, int pinkIndex)
	{
		this.properties = properties;
		this.player = player;
		base.GetComponent<Collider2D>().enabled = true;
		this.maxCounter = maxCounter;
		this.moveRight = moveRight;
		this.pinkPattern = pinkPattern;
		this.pinkIndex = pinkIndex;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x000AF6D4 File Offset: 0x000AD8D4
	public IEnumerator move_cr()
	{
		base.StartCoroutine(this.spawn_minis_cr());
		for (;;)
		{
			base.transform.position += base.transform.right * this.properties.sphinxSplitSpeed * CupheadTime.Delta * (float)((!this.moveRight) ? -1 : 1);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CEA RID: 7402 RVA: 0x000187C3 File Offset: 0x000169C3
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CEB RID: 7403 RVA: 0x000187E1 File Offset: 0x000169E1
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CEC RID: 7404 RVA: 0x000AF6F0 File Offset: 0x000AD8F0
	public IEnumerator spawn_minis_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.miniInitialSpawnDelay);
		int counter = 0;
		while (base.transform.position.y < 720f && base.transform.position.y > -360f)
		{
			if (counter >= this.maxCounter)
			{
				break;
			}
			FlyingGenieLevelMiniCat p = this.miniCat.Create(base.transform.position, 0f, this.player, this.properties);
			p.SetParryable(this.pinkPattern[this.pinkIndex][0] == 'P');
			this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
			counter++;
			yield return CupheadTime.WaitForSeconds(this, this.properties.miniSpawnDelay);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x000187FF File Offset: 0x000169FF
	public override void RandomizeVariant()
	{
	}

	// Token: 0x06001CEE RID: 7406 RVA: 0x00018801 File Offset: 0x00016A01
	public override void SetTrigger(string trigger)
	{
	}

	// Token: 0x04001780 RID: 6016
	[SerializeField]
	public FlyingGenieLevelMiniCat miniCat;

	// Token: 0x04001781 RID: 6017
	public LevelProperties.FlyingGenie.Sphinx properties;

	// Token: 0x04001782 RID: 6018
	public AbstractPlayerController player;

	// Token: 0x04001783 RID: 6019
	public bool moveRight;

	// Token: 0x04001784 RID: 6020
	public int maxCounter;

	// Token: 0x04001785 RID: 6021
	public string[] pinkPattern;

	// Token: 0x04001786 RID: 6022
	public int pinkIndex;
}
