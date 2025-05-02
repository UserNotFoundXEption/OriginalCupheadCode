using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000264 RID: 612
public class FlyingGenieLevelBomb : AbstractProjectile
{
	// Token: 0x06001BF2 RID: 7154 RVA: 0x000ACF8C File Offset: 0x000AB18C
	public FlyingGenieLevelBomb Create(Vector2 pos, Vector3 targetPos, LevelProperties.FlyingGenie.Bomb properties)
	{
		FlyingGenieLevelBomb flyingGenieLevelBomb = base.Create() as FlyingGenieLevelBomb;
		flyingGenieLevelBomb.transform.position = pos;
		flyingGenieLevelBomb.properties = properties;
		flyingGenieLevelBomb.targetPos = targetPos;
		return flyingGenieLevelBomb;
	}

	// Token: 0x06001BF3 RID: 7155 RVA: 0x000ACFC8 File Offset: 0x000AB1C8
	public override void Awake()
	{
		base.Awake();
		foreach (GameObject gameObject in this.explosionBeams)
		{
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
			gameObject.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		}
	}

	// Token: 0x06001BF4 RID: 7156 RVA: 0x00017B3E File Offset: 0x00015D3E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001BF5 RID: 7157 RVA: 0x00017B5C File Offset: 0x00015D5C
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001BF6 RID: 7158 RVA: 0x000AD02C File Offset: 0x000AB22C
	public override void Start()
	{
		base.Start();
		this.readyToDetonate = false;
		foreach (GameObject gameObject in this.explosionBeams)
		{
			if (this.bombType == FlyingGenieLevelBomb.BombType.Regular)
			{
				gameObject.transform.SetScale(new float?(this.properties.bombRegularSize), new float?(this.properties.bombRegularSize), null);
			}
			else if (this.bombType == FlyingGenieLevelBomb.BombType.Diagonal)
			{
				gameObject.transform.SetScale(new float?(this.properties.bombDiagonalSize), new float?(this.properties.bombDiagonalSize), null);
			}
			else if (this.bombType == FlyingGenieLevelBomb.BombType.PlusSized)
			{
				gameObject.transform.SetScale(new float?(this.properties.bombPlusSize), new float?(this.properties.bombPlusSize), null);
			}
		}
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06001BF7 RID: 7159 RVA: 0x000AD13C File Offset: 0x000AB33C
	public IEnumerator start_cr()
	{
		while (base.transform.position != this.targetPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPos, this.properties.bombSpeed * CupheadTime.Delta);
			yield return null;
		}
		this.readyToDetonate = true;
		yield return null;
		yield break;
	}

	// Token: 0x06001BF8 RID: 7160 RVA: 0x00017B7A File Offset: 0x00015D7A
	public void Explode()
	{
		base.StartCoroutine(this.explode_cr());
	}

	// Token: 0x06001BF9 RID: 7161 RVA: 0x000AD158 File Offset: 0x000AB358
	public IEnumerator explode_cr()
	{
		foreach (GameObject gameObject in this.explosionBeams)
		{
			gameObject.GetComponent<SpriteRenderer>().enabled = true;
			gameObject.GetComponent<Collider2D>().enabled = true;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.GetComponent<SpriteRenderer>().enabled = false;
		foreach (GameObject gameObject2 in this.explosionBeams)
		{
			gameObject2.gameObject.SetActive(false);
		}
		this.readyToDetonate = false;
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001BFA RID: 7162 RVA: 0x00017B89 File Offset: 0x00015D89
	public override void Die()
	{
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x040016B4 RID: 5812
	public FlyingGenieLevelBomb.BombType bombType;

	// Token: 0x040016B5 RID: 5813
	public bool readyToDetonate;

	// Token: 0x040016B6 RID: 5814
	[SerializeField]
	public GameObject[] explosionBeams;

	// Token: 0x040016B7 RID: 5815
	public LevelProperties.FlyingGenie.Bomb properties;

	// Token: 0x040016B8 RID: 5816
	public Vector3 targetPos;

	// Token: 0x02000CE8 RID: 3304
	public enum BombType
	{
		// Token: 0x04005D83 RID: 23939
		Regular,
		// Token: 0x04005D84 RID: 23940
		Diagonal,
		// Token: 0x04005D85 RID: 23941
		PlusSized
	}
}
