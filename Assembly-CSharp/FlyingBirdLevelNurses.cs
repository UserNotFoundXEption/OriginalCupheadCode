using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000236 RID: 566
public class FlyingBirdLevelNurses : AbstractCollidableObject
{
	// Token: 0x060019E5 RID: 6629 RVA: 0x000A7438 File Offset: 0x000A5638
	public void InitNurse(LevelProperties.FlyingBird.Nurses properties)
	{
		this.nurses = base.transform.GetChildTransforms();
		foreach (Transform transform in this.nurses)
		{
			transform.gameObject.SetActive(true);
		}
		this.leftSideShooting = (Random.Range(-1, 1) >= 0);
		this.properties = properties;
		this.attackIndex = Random.Range(0, properties.attackCount.Split(new char[]
		{
			','
		}).Length);
		this.pinkPattern = properties.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = 0;
		foreach (Transform transform2 in this.nurses)
		{
			if (transform2.GetComponent<Collider2D>() != null)
			{
				transform2.GetComponent<Collider2D>().enabled = true;
			}
		}
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x000A753C File Offset: 0x000A573C
	public IEnumerator attack_cr()
	{
		bool shootLeft = Rand.Bool();
		bool multiplayer = PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null;
		for (;;)
		{
			int max = Parser.IntParse(this.properties.attackCount.Split(new char[]
			{
				','
			})[this.attackIndex]);
			for (int i = 0; i < max; i++)
			{
				if (i != 0)
				{
					yield return CupheadTime.WaitForSeconds(this, this.properties.attackRepeatDelay);
				}
				if (shootLeft)
				{
					base.animator.SetBool("ANurseATK", true);
				}
				else
				{
					base.animator.SetBool("BNurseATK", true);
				}
				shootLeft = !shootLeft;
				if (multiplayer)
				{
					this.target++;
					if (this.target > PlayerId.PlayerTwo)
					{
						this.target = PlayerId.PlayerOne;
					}
				}
			}
			this.leftSideShooting = !this.leftSideShooting;
			yield return CupheadTime.WaitForSeconds(this, this.properties.attackMainDelay);
			this.attackIndex++;
			if (this.attackIndex >= this.properties.attackCount.Split(new char[]
			{
				','
			}).Length)
			{
				this.attackIndex = 0;
			}
		}
		yield break;
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x000A7558 File Offset: 0x000A5758
	public void ShootLeft()
	{
		this.spitFXLeft.SetActive(false);
		AbstractProjectile abstractProjectile = this.pillPrefab.Create(this.shootLeftPosRoot.position + base.transform.up.normalized * 0.1f);
		abstractProjectile.GetComponent<FlyingBirdLevelNursePill>().InitPill(this.properties, this.target, this.pinkPattern[this.pinkIndex] == "P");
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		base.animator.SetBool("ANurseATK", false);
		this.spitFXLeft.SetActive(true);
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x000A7614 File Offset: 0x000A5814
	public void ShootRight()
	{
		this.spitFXRight.SetActive(false);
		AbstractProjectile abstractProjectile = this.pillPrefab.Create(this.shootRightPosRoot.position + base.transform.up.normalized * 0.1f);
		abstractProjectile.GetComponent<FlyingBirdLevelNursePill>().InitPill(this.properties, this.target, this.pinkPattern[this.pinkIndex] == "P");
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
		base.animator.SetBool("BNurseATK", false);
		this.spitFXRight.SetActive(true);
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x000160DA File Offset: 0x000142DA
	public void ShootSFX()
	{
		AudioManager.Play("nurse_attack");
		this.emitAudioFromObject.Add("nurse_attack");
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x000160F6 File Offset: 0x000142F6
	public void Die()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x040014CC RID: 5324
	public const string Regular = "R";

	// Token: 0x040014CD RID: 5325
	public const string Parry = "P";

	// Token: 0x040014CE RID: 5326
	[SerializeField]
	public AbstractProjectile pillPrefab;

	// Token: 0x040014CF RID: 5327
	[SerializeField]
	public Transform shootRightPosRoot;

	// Token: 0x040014D0 RID: 5328
	[SerializeField]
	public Transform shootLeftPosRoot;

	// Token: 0x040014D1 RID: 5329
	[SerializeField]
	public GameObject spitFXLeft;

	// Token: 0x040014D2 RID: 5330
	[SerializeField]
	public GameObject spitFXRight;

	// Token: 0x040014D3 RID: 5331
	public bool leftSideShooting;

	// Token: 0x040014D4 RID: 5332
	public int attackIndex;

	// Token: 0x040014D5 RID: 5333
	public PlayerId target;

	// Token: 0x040014D6 RID: 5334
	public string[] pinkPattern;

	// Token: 0x040014D7 RID: 5335
	public int pinkIndex;

	// Token: 0x040014D8 RID: 5336
	public Transform[] nurses;

	// Token: 0x040014D9 RID: 5337
	public LevelProperties.FlyingBird.Nurses properties;
}
