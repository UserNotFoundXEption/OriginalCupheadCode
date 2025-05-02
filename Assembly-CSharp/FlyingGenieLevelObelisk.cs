using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000271 RID: 625
public class FlyingGenieLevelObelisk : AbstractCollidableObject
{
	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06001CA6 RID: 7334 RVA: 0x00018498 File Offset: 0x00016698
	// (set) Token: 0x06001CA7 RID: 7335 RVA: 0x000184A0 File Offset: 0x000166A0
	public bool isOn { get; set; }

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06001CA8 RID: 7336 RVA: 0x000184A9 File Offset: 0x000166A9
	// (set) Token: 0x06001CA9 RID: 7337 RVA: 0x000184B1 File Offset: 0x000166B1
	public bool isFirst { get; set; }

	// Token: 0x06001CAA RID: 7338 RVA: 0x000AEA0C File Offset: 0x000ACC0C
	public void Init(Vector2 pos, LevelProperties.FlyingGenie.Obelisk properties, FlyingGenieLevelGenie parent, bool isFirst)
	{
		this.isFirst = isFirst;
		base.transform.position = pos;
		this.startPosition = pos;
		this.properties = properties;
		this.parent = parent;
		if (isFirst)
		{
			AudioManager.PlayLoop("genie_pillar_main_loop");
			AudioManager.PlayLoop("genie_pillar_destructable_loop");
			this.emitAudioFromObject.Add("genie_pillar_main_loop");
			this.emitAudioFromObject.Add("genie_pillar_destructable_loop");
		}
	}

	// Token: 0x06001CAB RID: 7339 RVA: 0x000184BA File Offset: 0x000166BA
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		if (Rand.Bool())
		{
			this.baseA.SetActive(true);
		}
		else
		{
			this.baseB.SetActive(true);
		}
	}

	// Token: 0x06001CAC RID: 7340 RVA: 0x000AEA88 File Offset: 0x000ACC88
	public void ActivateObelisk(string[] genieHeadNums)
	{
		this.isOn = true;
		this.bouncerWall.enabled = true;
		base.transform.position = this.startPosition;
		float num = this.obeliskBlock.GetComponent<Renderer>().bounds.size.y / 1.95f;
		int num2 = 100;
		int num3 = 0;
		int num4 = 5;
		bool flag = false;
		this.obeliskBlocks = new List<FlyingGenieLevelObeliskBlock>();
		this.genieHeads = new List<FlyingGenieLevelGenieHead>();
		bool[] array = new bool[num4];
		int[] array2 = new int[num4];
		for (int i = 0; i < num4; i++)
		{
			bool flag2 = false;
			foreach (string s in genieHeadNums)
			{
				Parser.IntTryParse(s, out num3);
				if (num3 == i + 1)
				{
					flag2 = true;
				}
				if (genieHeadNums.Length < 2 && num3 == 2)
				{
					flag = true;
				}
			}
			array[i] = flag2;
		}
		int num5 = (num3 <= 1) ? (num4 - 1) : (num3 - 1 - 1);
		if (genieHeadNums.Length > 1)
		{
			if (genieHeadNums[0][0] == '2' && genieHeadNums[1][0] == '5')
			{
				array2[0] = 1;
				array2[2] = 5;
				array2[3] = 1;
			}
			else if (genieHeadNums[0][0] == '1' && genieHeadNums[1][0] == '4')
			{
				array2[1] = 5;
				array2[2] = 1;
				array2[4] = 5;
			}
			else if (genieHeadNums[0][0] == '1' && genieHeadNums[1][0] == '5')
			{
				array2[1] = 4;
				array2[2] = 5;
				array2[3] = 1;
			}
		}
		else
		{
			for (int k = 0; k < num4; k++)
			{
				array2[num5] = k + 1;
				num5 = (num5 + 1) % num4;
			}
		}
		bool flag3 = Rand.Bool();
		for (int l = 0; l < num4; l++)
		{
			Vector3 vector;
			vector..ctor(0f, (float)(-(float)l) * num - num / 1.5f, 0f);
			if (array[l])
			{
				FlyingGenieLevelGenieHead flyingGenieLevelGenieHead = Object.Instantiate<FlyingGenieLevelGenieHead>(this.genieHead);
				flyingGenieLevelGenieHead.Init(base.transform.position + vector, this.properties.obeliskGenieHP, this.parent);
				flyingGenieLevelGenieHead.transform.parent = base.transform;
				flyingGenieLevelGenieHead.GetComponent<SpriteRenderer>().sortingOrder = num2;
				flyingGenieLevelGenieHead.animator.SetBool("GoClockwise", flag3);
				this.genieHeads.Add(flyingGenieLevelGenieHead);
			}
			else
			{
				FlyingGenieLevelObeliskBlock flyingGenieLevelObeliskBlock = Object.Instantiate<FlyingGenieLevelObeliskBlock>(this.obeliskBlock);
				flyingGenieLevelObeliskBlock.Init(base.transform.position + vector, this.properties);
				flyingGenieLevelObeliskBlock.transform.parent = base.transform;
				flyingGenieLevelObeliskBlock.GetComponent<SpriteRenderer>().sortingOrder = num2;
				flyingGenieLevelObeliskBlock.animator.SetBool("GoClockwise", flag3);
				flyingGenieLevelObeliskBlock.animator.SetInteger("PickBlock", array2[l]);
				this.obeliskBlocks.Add(flyingGenieLevelObeliskBlock);
			}
			if (!flag)
			{
				flag3 = !flag3;
			}
			num2 -= 2;
		}
		base.StartCoroutine(this.move_cr());
		if (this.properties.normalShotOn)
		{
			base.StartCoroutine(this.shoot_cr());
		}
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x000AEDF0 File Offset: 0x000ACFF0
	public void SetColliders(float width, float offset)
	{
		this.ceiling.transform.position = new Vector3(offset, this.ceiling.transform.position.y);
		this.ceiling.size = new Vector3(width, this.ceiling.size.y);
		this.ground.transform.position = new Vector3(offset, -360f);
		this.ground.size = new Vector3(width, this.ground.size.y);
	}

	// Token: 0x06001CAE RID: 7342 RVA: 0x000AEE98 File Offset: 0x000AD098
	public IEnumerator shoot_cr()
	{
		string[] anglePattern = this.properties.obeliskShotDirection.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] pinkPattern = this.properties.obeliskPinkString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int angleIndex = Random.Range(0, anglePattern.Length);
		int pinkIndex = Random.Range(0, pinkPattern.Length);
		float angle = 0f;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.obeliskShootDelay);
			yield return null;
			foreach (FlyingGenieLevelObeliskBlock block in this.obeliskBlocks)
			{
				Parser.FloatTryParse(anglePattern[angleIndex], out angle);
				if (pinkPattern[pinkIndex][0] == 'P')
				{
					block.ShootPink(angle);
				}
				else
				{
					block.ShootRegular(angle);
				}
				yield return null;
			}
			angleIndex %= anglePattern.Length;
			pinkIndex %= pinkPattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CAF RID: 7343 RVA: 0x000184EE File Offset: 0x000166EE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CB0 RID: 7344 RVA: 0x000AEEB4 File Offset: 0x000AD0B4
	public IEnumerator move_cr()
	{
		this.isFirst = true;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > -840f)
		{
			base.transform.AddPosition(-this.properties.obeliskMovementSpeed * CupheadTime.Delta, 0f, 0f);
			yield return wait;
		}
		this.isFirst = false;
		this.End();
		yield return null;
		yield break;
	}

	// Token: 0x06001CB1 RID: 7345 RVA: 0x000AEED0 File Offset: 0x000AD0D0
	public void End()
	{
		this.isOn = false;
		foreach (FlyingGenieLevelObeliskBlock flyingGenieLevelObeliskBlock in this.obeliskBlocks)
		{
			Object.Destroy(flyingGenieLevelObeliskBlock.gameObject);
		}
		foreach (FlyingGenieLevelGenieHead flyingGenieLevelGenieHead in this.genieHeads)
		{
			if (flyingGenieLevelGenieHead != null)
			{
				Object.Destroy(flyingGenieLevelGenieHead.gameObject);
			}
		}
		this.obeliskBlocks.Clear();
		this.genieHeads.Clear();
		this.StopAllCoroutines();
		this.bouncerWall.enabled = false;
	}

	// Token: 0x04001743 RID: 5955
	[SerializeField]
	public GameObject baseA;

	// Token: 0x04001744 RID: 5956
	[SerializeField]
	public GameObject baseB;

	// Token: 0x04001745 RID: 5957
	[SerializeField]
	public BoxCollider2D bouncerWall;

	// Token: 0x04001746 RID: 5958
	[SerializeField]
	public BoxCollider2D ceiling;

	// Token: 0x04001747 RID: 5959
	[SerializeField]
	public BoxCollider2D ground;

	// Token: 0x04001748 RID: 5960
	[SerializeField]
	public FlyingGenieLevelGenieHead genieHead;

	// Token: 0x04001749 RID: 5961
	[SerializeField]
	public FlyingGenieLevelObeliskBlock obeliskBlock;

	// Token: 0x0400174A RID: 5962
	public List<FlyingGenieLevelObeliskBlock> obeliskBlocks;

	// Token: 0x0400174B RID: 5963
	public List<FlyingGenieLevelGenieHead> genieHeads;

	// Token: 0x0400174C RID: 5964
	public LevelProperties.FlyingGenie.Obelisk properties;

	// Token: 0x0400174D RID: 5965
	public FlyingGenieLevelGenie parent;

	// Token: 0x0400174E RID: 5966
	public DamageDealer damageDealer;

	// Token: 0x0400174F RID: 5967
	public Vector3 startPosition;

	// Token: 0x04001750 RID: 5968
	public Vector3 newEmitPosition;

	// Token: 0x04001751 RID: 5969
	public float health;

	// Token: 0x04001752 RID: 5970
	public float shootAngle;
}
