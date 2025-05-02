using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class DicePalaceFlyingMemoryLevelGameManager : LevelProperties.DicePalaceFlyingMemory.Entity
{
	// Token: 0x1700027B RID: 635
	// (get) Token: 0x060016E9 RID: 5865 RVA: 0x000A032C File Offset: 0x0009E52C
	public static DicePalaceFlyingMemoryLevelGameManager Instance
	{
		get
		{
			if (DicePalaceFlyingMemoryLevelGameManager.singletonGameManager == null)
			{
				DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = new GameObject
				{
					name = "GameManager"
				}.AddComponent<DicePalaceFlyingMemoryLevelGameManager>();
			}
			return DicePalaceFlyingMemoryLevelGameManager.singletonGameManager;
		}
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x00013826 File Offset: 0x00011A26
	public override void Awake()
	{
		base.Awake();
		this.hiddenPosition = base.transform.position;
		DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = this;
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x000A036C File Offset: 0x0009E56C
	public override void LevelInit(LevelProperties.DicePalaceFlyingMemory properties)
	{
		base.LevelInit(properties);
		this.patternOrder = properties.CurrentState.flippyCard.patternOrder.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.maxHP = properties.CurrentHealth;
		this.contactDimX = this.GridDimX + 1;
		this.contactDimY = this.GridDimY + 1;
		this.GenerateGrid();
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x00013845 File Offset: 0x00011A45
	public void Update()
	{
		if (this.checkForFlipped)
		{
			this.CheckIfFlipped();
		}
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x00013858 File Offset: 0x00011A58
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.contactPointPrefab = null;
		this.cardPrefab = null;
		this.botPrefab = null;
		DicePalaceFlyingMemoryLevelGameManager.singletonGameManager = null;
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x000A03DC File Offset: 0x0009E5DC
	public void GenerateGrid()
	{
		float x = this.cardPrefab.GetComponent<Renderer>().bounds.size.x;
		float y = this.cardPrefab.GetComponent<Renderer>().bounds.size.y;
		float num = x + 10f;
		float num2 = y + 10f;
		this.cards = new DicePalaceFlyingMemoryLevelCard[this.GridDimX, this.GridDimY];
		this.contactPoints = new DicePalaceFlyingMemoryLevelContactPoint[this.contactDimX, this.contactDimY];
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				Vector3 vector;
				vector..ctor((float)j * num, (float)(-(float)i) * num2);
				this.cards[j, i] = Object.Instantiate<DicePalaceFlyingMemoryLevelCard>(this.cardPrefab);
				this.cards[j, i].transform.position = vector + base.transform.position;
				this.cards[j, i].transform.parent = base.transform;
				this.AssignCards(j, i);
			}
		}
		for (int k = 0; k < this.contactDimY; k++)
		{
			for (int l = 0; l < this.contactDimX; l++)
			{
				Vector3 vector2;
				vector2..ctor((float)l * num - num / 2f, (float)(-(float)k) * num2 + num2 / 2f);
				this.contactPoints[l, k] = Object.Instantiate<DicePalaceFlyingMemoryLevelContactPoint>(this.contactPointPrefab);
				this.contactPoints[l, k].transform.position = vector2 + base.transform.position;
				this.contactPoints[l, k].transform.parent = base.transform;
				this.contactPoints[l, k].Xcoord = l;
				this.contactPoints[l, k].Ycoord = k;
			}
		}
		base.StartCoroutine(this.start_game_cr());
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x000A0620 File Offset: 0x0009E820
	public void AssignCards(int x, int y)
	{
		DicePalaceFlyingMemoryLevelCard.Card card = DicePalaceFlyingMemoryLevelCard.Card.Flowers;
		if (this.patternOrder[this.patternOrderIndex] == "1A")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Cuphead;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "1B")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Chips;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "2A")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Flowers;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "2B")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Shield;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "3A")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Spindle;
		}
		else if (this.patternOrder[this.patternOrderIndex] == "3B")
		{
			card = DicePalaceFlyingMemoryLevelCard.Card.Mugman;
		}
		int index = Random.Range(0, this.chosenFlippedDownCards.Count);
		this.cards[x, y].card = card;
		this.cards[x, y].GetComponent<SpriteRenderer>().sprite = this.chosenFlippedDownCards[index];
		this.chosenFlippedDownCards.Remove(this.chosenFlippedDownCards[index]);
		this.patternOrderIndex = (this.patternOrderIndex + 1) % this.patternOrder.Length;
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x000A0778 File Offset: 0x0009E978
	public IEnumerator start_game_cr()
	{
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				this.cards[j, i].FlipUp();
			}
		}
		float t = 0f;
		float time = 1.3f;
		Vector2 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.cardStopRoot.position, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.cardStopRoot.position;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.flippyCard.initialRevealTime);
		for (int k = 0; k < this.GridDimY; k++)
		{
			for (int l = 0; l < this.GridDimX; l++)
			{
				this.cards[l, k].EnableCards();
			}
		}
		this.checkForFlipped = true;
		if (base.properties.CurrentState.bots.botsOn)
		{
			base.StartCoroutine(this.spawning_bots_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x000A0794 File Offset: 0x0009E994
	public IEnumerator slide_cr(Vector3 endPosition)
	{
		float t = 0f;
		float time = 1.3f;
		Vector2 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPosition, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = endPosition;
		yield break;
	}

	// Token: 0x060016F2 RID: 5874 RVA: 0x000A07B8 File Offset: 0x0009E9B8
	public void CheckIfFlipped()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				if (this.cards[j, i].flippedUp && !this.cards[j, i].permanentlyFlipped)
				{
					num++;
					this.cards[j, i].GetComponent<Collider2D>().enabled = false;
					if (num >= 2)
					{
						this.checkForFlipped = false;
						if (this.cards[num2, num3].card == this.cards[j, i].card)
						{
							this.matchMade = true;
							this.cards[j, i].permanentlyFlipped = true;
							this.cards[num2, num3].permanentlyFlipped = true;
						}
						else
						{
							this.matchMade = false;
						}
						base.StartCoroutine(this.disable_all_cards_cr());
						num = 0;
					}
					else
					{
						num2 = j;
						num3 = i;
					}
				}
			}
		}
	}

	// Token: 0x060016F3 RID: 5875 RVA: 0x000A08D8 File Offset: 0x0009EAD8
	public IEnumerator disable_all_cards_cr()
	{
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				if (!this.cards[j, i].permanentlyFlipped)
				{
					this.cards[j, i].DisableCard();
				}
				else
				{
					this.cards[j, i].GetComponent<Collider2D>().enabled = false;
				}
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		base.StartCoroutine(this.open_timer_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060016F4 RID: 5876 RVA: 0x000A08F4 File Offset: 0x0009EAF4
	public IEnumerator open_timer_cr()
	{
		float HPToLower = this.maxHP / (float)(this.GridDimX * this.GridDimY / 2);
		if (this.matchMade)
		{
			base.StartCoroutine(this.slide_cr(this.hiddenPosition));
			this.matchCounter++;
			while (this.stuffedToy.currentlyColliding)
			{
				yield return null;
			}
			this.stuffedToy.Open();
			if (this.matchCounter == this.GridDimX * this.GridDimY / 2)
			{
				yield break;
			}
			while (base.properties.CurrentHealth >= this.maxHP - HPToLower * (float)this.matchCounter)
			{
				yield return null;
			}
		}
		else
		{
			this.stuffedToy.guessedWrong = true;
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		for (int i = 0; i < this.GridDimY; i++)
		{
			for (int j = 0; j < this.GridDimX; j++)
			{
				if (!this.cards[j, i].permanentlyFlipped)
				{
					this.cards[j, i].EnableCards();
				}
			}
		}
		base.StartCoroutine(this.slide_cr(this.cardStopRoot.position));
		this.stuffedToy.Closed();
		this.checkForFlipped = true;
		yield return null;
		yield break;
	}

	// Token: 0x060016F5 RID: 5877 RVA: 0x000A0910 File Offset: 0x0009EB10
	public void SpawnBot(int xCoord, int yCoord, bool moveOnY)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		DicePalaceFlyingMemoryLevelBot dicePalaceFlyingMemoryLevelBot = Object.Instantiate<DicePalaceFlyingMemoryLevelBot>(this.botPrefab);
		dicePalaceFlyingMemoryLevelBot.Init(base.properties.CurrentState.bots, this.contactPoints[xCoord, yCoord], moveOnY, base.properties.CurrentState.bots.botsHP, next);
	}

	// Token: 0x060016F6 RID: 5878 RVA: 0x000A096C File Offset: 0x0009EB6C
	public IEnumerator spawning_bots_cr()
	{
		LevelProperties.DicePalaceFlyingMemory.Bots p = base.properties.CurrentState.bots;
		string[] spawnPattern = p.spawnOrder.GetRandom<string>().Split(new char[]
		{
			','
		});
		int number = 0;
		int Xcoord = 0;
		int Ycoord = 0;
		bool Yset = false;
		for (;;)
		{
			for (int i = 0; i < spawnPattern.Length; i++)
			{
				string[] spawnLocation = spawnPattern[i].Split(new char[]
				{
					':'
				});
				foreach (string text in spawnLocation)
				{
					if (text[0] == 'U')
					{
						Ycoord = 0;
						Yset = true;
					}
					else if (text[0] == 'D')
					{
						Ycoord = this.contactDimY - 1;
						Yset = true;
					}
					else if (text[0] == 'L')
					{
						Xcoord = 0;
						Yset = false;
					}
					else if (text[0] == 'R')
					{
						Xcoord = this.contactDimX - 1;
						Yset = false;
					}
					else
					{
						Parser.IntTryParse(text, out number);
					}
				}
				if (Yset)
				{
					Xcoord = number;
				}
				else
				{
					Ycoord = number;
				}
				this.SpawnBot(Xcoord, Ycoord, Yset);
				yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040012A4 RID: 4772
	public static DicePalaceFlyingMemoryLevelGameManager singletonGameManager;

	// Token: 0x040012A5 RID: 4773
	public DicePalaceFlyingMemoryLevelContactPoint[,] contactPoints;

	// Token: 0x040012A6 RID: 4774
	public int contactDimX;

	// Token: 0x040012A7 RID: 4775
	public int contactDimY;

	// Token: 0x040012A8 RID: 4776
	[SerializeField]
	public Transform cardStopRoot;

	// Token: 0x040012A9 RID: 4777
	[SerializeField]
	public List<Sprite> chosenFlippedDownCards;

	// Token: 0x040012AA RID: 4778
	[SerializeField]
	public DicePalaceFlyingMemoryLevelContactPoint contactPointPrefab;

	// Token: 0x040012AB RID: 4779
	[SerializeField]
	public DicePalaceFlyingMemoryLevelStuffedToy stuffedToy;

	// Token: 0x040012AC RID: 4780
	[SerializeField]
	public DicePalaceFlyingMemoryLevelCard cardPrefab;

	// Token: 0x040012AD RID: 4781
	[SerializeField]
	public DicePalaceFlyingMemoryLevelBot botPrefab;

	// Token: 0x040012AE RID: 4782
	public DicePalaceFlyingMemoryLevelCard[,] cards;

	// Token: 0x040012AF RID: 4783
	public Vector3 hiddenPosition;

	// Token: 0x040012B0 RID: 4784
	public int GridDimX = 4;

	// Token: 0x040012B1 RID: 4785
	public int GridDimY = 3;

	// Token: 0x040012B2 RID: 4786
	public int patternOrderIndex;

	// Token: 0x040012B3 RID: 4787
	public int matchCounter;

	// Token: 0x040012B4 RID: 4788
	public float maxHP;

	// Token: 0x040012B5 RID: 4789
	public float space;

	// Token: 0x040012B6 RID: 4790
	public bool checkForFlipped;

	// Token: 0x040012B7 RID: 4791
	public bool matchMade;

	// Token: 0x040012B8 RID: 4792
	public string[] patternOrder;
}
