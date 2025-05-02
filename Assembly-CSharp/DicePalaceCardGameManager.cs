using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001D1 RID: 465
public class DicePalaceCardGameManager : AbstractPausableComponent
{
	// Token: 0x060015C0 RID: 5568 RVA: 0x0009CD0C File Offset: 0x0009AF0C
	public void GameSetup(LevelProperties.DicePalaceCard cardProperties)
	{
		this.properties = cardProperties.CurrentState.blocks;
		this.GridDimX = this.properties.gridWidth;
		this.GridDimY = this.properties.gridHeight;
		this.SetSize();
		this.typePattern = this.properties.cardTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.amountPattern = this.properties.cardAmountString.GetRandom<string>().Split(new char[]
		{
			','
		});
		Vector3 position = base.transform.position;
		position.y = 360f - this.gridBlockPrefab.GetComponent<Renderer>().bounds.size.y;
		position.x = -640f + this.gridBlockPrefab.GetComponent<Renderer>().bounds.size.x;
		base.transform.position = position;
		this.selectedPrefab = new DicePalaceCardLevelBlock();
		this.totalColumns = new List<DicePalaceCardLevelColumn>();
		this.typeIndex = Random.Range(0, this.typePattern.Length);
		this.amountIndex = Random.Range(0, this.amountPattern.Length);
		this.GenerateGrid();
		this.startingPos = base.transform.position.y;
	}

	// Token: 0x060015C1 RID: 5569 RVA: 0x0009CE70 File Offset: 0x0009B070
	public IEnumerator start_game_cr()
	{
		for (;;)
		{
			this.SpawnColumn();
			yield return CupheadTime.WaitForSeconds(this, this.properties.attackDelayRange);
		}
		yield break;
	}

	// Token: 0x060015C2 RID: 5570 RVA: 0x0009CE8C File Offset: 0x0009B08C
	public void SetSize()
	{
		this.hearts.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.spades.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.clubs.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.diamonds.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.gridBlockPrefab.transform.SetScale(new float?(this.properties.blockSize), new float?(this.properties.blockSize), new float?(this.properties.blockSize));
		this.GridSpacing = this.properties.blockSize;
	}

	// Token: 0x060015C3 RID: 5571 RVA: 0x0009CFEC File Offset: 0x0009B1EC
	public void SpawnColumn()
	{
		int num = 1;
		int num2 = -1;
		float num3 = this.gridBlockPrefab.GetComponent<Renderer>().bounds.size.y / 2f;
		float num4 = 0f;
		Parser.FloatTryParse(this.amountPattern[this.amountIndex], out num4);
		DicePalaceCardLevelColumn item = Object.Instantiate<DicePalaceCardLevelColumn>(this.columnObject);
		this.totalColumns.Add(item);
		int index = this.totalColumns.Count - 1;
		Vector3 position = this.totalColumns[index].transform.position;
		position.x = 640f;
		position.y = 360f - num3;
		this.totalColumns[index].transform.position = position;
		int num5 = 0;
		while ((float)num5 < num4)
		{
			if (this.typePattern[this.typeIndex][0] == 'H')
			{
				this.selectedPrefab = this.hearts;
			}
			else if (this.typePattern[this.typeIndex][0] == 'S')
			{
				this.selectedPrefab = this.spades;
			}
			else if (this.typePattern[this.typeIndex][0] == 'D')
			{
				this.selectedPrefab = this.diamonds;
			}
			else if (this.typePattern[this.typeIndex][0] == 'C')
			{
				this.selectedPrefab = this.clubs;
			}
			this.typeIndex = (this.typeIndex + 1) % this.typePattern.Length;
			DicePalaceCardLevelBlock dicePalaceCardLevelBlock = Object.Instantiate<DicePalaceCardLevelBlock>(this.selectedPrefab);
			this.totalColumns[index].blockPieces.Add(dicePalaceCardLevelBlock);
			dicePalaceCardLevelBlock.transform.parent = this.totalColumns[index].transform;
			Vector3 position2 = this.totalColumns[index].blockPieces[num5].transform.position;
			if (num5 % 2 == 0 && num5 != 0)
			{
				this.totalColumns[index].blockPieces[num5].stopOffsetX = num2;
				position2.x = 640f - this.totalColumns[index].blockPieces[num5].GetComponent<Renderer>().bounds.size.x * (float)Mathf.Abs(this.totalColumns[index].blockPieces[num5].stopOffsetX);
				num2--;
			}
			else if (num5 % 2 == 1 && num5 != 0)
			{
				this.totalColumns[index].blockPieces[num5].stopOffsetX = num;
				position2.x = 640f + this.totalColumns[index].blockPieces[num5].GetComponent<Renderer>().bounds.size.x * (float)Mathf.Abs(this.totalColumns[index].blockPieces[num5].stopOffsetX);
				num++;
			}
			else
			{
				position2.x = 640f;
			}
			position2.y = 360f - num3;
			this.totalColumns[index].blockPieces[num5].transform.position = position2;
			num5++;
		}
		this.amountIndex = (this.amountIndex + 1) % this.amountPattern.Length;
		base.StartCoroutine(this.horizontal_moving_column(this.totalColumns[index]));
	}

	// Token: 0x060015C4 RID: 5572 RVA: 0x0009D3A4 File Offset: 0x0009B5A4
	public IEnumerator horizontal_moving_column(DicePalaceCardLevelColumn currentColumn)
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		float offset = this.gridBlocks[1, 0].transform.position.x - this.gridBlocks[0, 0].transform.position.x;
		int playerXPos = 0;
		int stopXPos = 0;
		bool selectStop = false;
		float dist = 0f;
		float distOffset = 20f;
		while (currentColumn.transform.position.x != this.gridBlocks[stopXPos, 0].transform.position.x)
		{
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			for (int i = 0; i < this.GridDimX - 1; i++)
			{
				if (player.transform.position.x > this.gridBlocks[i, 0].transform.position.x - offset / 2f)
				{
					if (player.transform.position.x < this.gridBlocks[i + 1, 0].transform.position.x - offset / 2f)
					{
						playerXPos = i;
					}
					else if (i + 1 == this.GridDimX - 1)
					{
						playerXPos = i + 1;
					}
				}
			}
			dist = this.gridBlocks[playerXPos, 0].transform.position.x - currentColumn.transform.position.x;
			Vector3 pos = currentColumn.transform.position;
			if (dist < distOffset)
			{
				selectStop = true;
			}
			int overFlow = this.GridDimX - playerXPos - currentColumn.blockPieces.Count;
			if (selectStop)
			{
				if (this.gridBlocks[1, 0].transform.position.x > player.transform.position.x && currentColumn.blockPieces.Count >= 1)
				{
					int num;
					if (currentColumn.blockPieces.Count % 2 == 1)
					{
						num = currentColumn.blockPieces[currentColumn.blockPieces.Count - 1].stopOffsetX - 1;
					}
					else
					{
						num = currentColumn.blockPieces[currentColumn.blockPieces.Count - 1].stopOffsetX;
					}
					stopXPos = Mathf.Abs(num) - 1;
					selectStop = false;
				}
				else if (this.gridBlocks[this.GridDimX - 1, 0].transform.position.x < player.transform.position.x || Mathf.Sign((float)overFlow) == -1f)
				{
					stopXPos = this.GridDimX - 1 - Mathf.Abs(currentColumn.blockPieces[currentColumn.blockPieces.Count - 1].stopOffsetX);
					selectStop = false;
				}
				else
				{
					stopXPos = playerXPos;
					selectStop = false;
				}
			}
			pos.x = Mathf.MoveTowards(currentColumn.transform.position.x, this.gridBlocks[stopXPos, this.GridDimY - 1].transform.position.x, this.properties.blockSpeed * CupheadTime.Delta);
			currentColumn.transform.position = pos;
			yield return null;
		}
		base.StartCoroutine(this.vertical_moving_column(currentColumn, stopXPos));
		yield return null;
		yield break;
	}

	// Token: 0x060015C5 RID: 5573 RVA: 0x0009D3C8 File Offset: 0x0009B5C8
	public IEnumerator vertical_moving_column(DicePalaceCardLevelColumn currentColumn, int stopXPos)
	{
		currentColumn.blockCounter = 0;
		currentColumn.blockXPos = new int[currentColumn.blockPieces.Count];
		currentColumn.columnStopYPos = new int[currentColumn.blockPieces.Count];
		for (int i = 0; i < currentColumn.blockPieces.Count; i++)
		{
			currentColumn.blockXPos[i] = stopXPos + currentColumn.blockPieces[i].stopOffsetX;
			for (int j = this.GridDimY - 1; j >= 0; j--)
			{
				if (!this.gridBlocks[currentColumn.blockXPos[i], j].hasBlock)
				{
					if (j > 0)
					{
						if (this.gridBlocks[currentColumn.blockXPos[i], j - 1].hasBlock)
						{
							currentColumn.columnStopYPos[i] = j;
							this.gridBlocks[currentColumn.blockXPos[i], j].hasBlock = true;
						}
					}
					else
					{
						currentColumn.columnStopYPos[i] = 0;
						this.gridBlocks[currentColumn.blockXPos[i], 0].hasBlock = true;
					}
				}
			}
			base.StartCoroutine(this.drop_block_cr(currentColumn, currentColumn.columnStopYPos[i], i));
		}
		while (currentColumn.blockCounter < currentColumn.blockPieces.Count)
		{
			yield return null;
		}
		currentColumn.blockPieces.Clear();
		currentColumn.transform.DetachChildren();
		Object.Destroy(currentColumn.gameObject);
		this.doneDropping = false;
		this.checkAgain = false;
		base.StartCoroutine(this.check_to_drop_blocks());
		while (!this.doneDropping)
		{
			yield return null;
		}
		this.CheckFullGrid();
		this.ScaleCheck();
		this.CheckForTop();
		base.StartCoroutine(this.check_to_drop_blocks());
		this.CheckFullGrid();
		this.ScaleCheck();
		yield break;
	}

	// Token: 0x060015C6 RID: 5574 RVA: 0x0009D3F4 File Offset: 0x0009B5F4
	public IEnumerator drop_block_cr(DicePalaceCardLevelColumn currentColumn, int indexToDropTo, int blockToDrop)
	{
		while (currentColumn.blockPieces[blockToDrop].transform.position.y > this.gridBlocks[currentColumn.blockXPos[blockToDrop], indexToDropTo].transform.position.y)
		{
			Vector3 pos = currentColumn.blockPieces[blockToDrop].transform.position;
			pos.y = Mathf.MoveTowards(currentColumn.blockPieces[blockToDrop].transform.position.y, this.gridBlocks[currentColumn.blockXPos[blockToDrop], indexToDropTo].transform.position.y, this.properties.blockDropSpeed * CupheadTime.Delta);
			currentColumn.blockPieces[blockToDrop].transform.position = pos;
			yield return null;
		}
		currentColumn.blockPieces[blockToDrop].transform.parent = base.transform;
		this.gridBlocks[currentColumn.blockXPos[blockToDrop], indexToDropTo].blockHeld = currentColumn.blockPieces[blockToDrop];
		currentColumn.blockCounter++;
		yield break;
	}

	// Token: 0x060015C7 RID: 5575 RVA: 0x0009D424 File Offset: 0x0009B624
	public IEnumerator check_to_drop_blocks()
	{
		for (int i = 0; i < this.GridDimX; i++)
		{
			for (int j = this.GridDimY - 1; j >= 0; j--)
			{
				if (this.gridBlocks[i, j].hasBlock && this.gridBlocks[i, j].Ycoordinate > 0f)
				{
					int num = j - 1;
					int num2 = j + 1;
					DicePalaceCardLevelBlock blockHeld = this.gridBlocks[i, j].blockHeld;
					if (!this.gridBlocks[i, num].hasBlock)
					{
						this.checkAgain = true;
						this.CheckFullGrid();
						base.StartCoroutine(this.drop_current_cr(i, j, num, blockHeld));
						if (this.gridBlocks[i, num2].hasBlock && this.gridBlocks[i, num2].Ycoordinate < (float)this.GridDimY)
						{
							base.StartCoroutine(this.drop_current_cr(i, num2, j, this.gridBlocks[i, num2].blockHeld));
							num2++;
						}
					}
					else
					{
						this.checkAgain = false;
					}
				}
			}
		}
		if (!this.checkAgain)
		{
			this.doneDropping = true;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060015C8 RID: 5576 RVA: 0x0009D440 File Offset: 0x0009B640
	public IEnumerator drop_current_cr(int x, int y, int spaceBelow, DicePalaceCardLevelBlock block)
	{
		if (this.gridBlocks[x, y].blockHeld != null && !this.gridBlocks[x, spaceBelow].hasBlock)
		{
			if (y >= 0 && this.gridBlocks[x, y].hasBlock)
			{
				this.gridBlocks[x, y].hasBlock = false;
				this.gridBlocks[x, spaceBelow].hasBlock = true;
			}
			while (this.gridBlocks[x, y].blockHeld.transform.position.y != this.gridBlocks[x, spaceBelow].transform.position.y)
			{
				Vector3 pos = this.gridBlocks[x, y].blockHeld.transform.position;
				pos.y = Mathf.MoveTowards(this.gridBlocks[x, y].blockHeld.transform.position.y, this.gridBlocks[x, spaceBelow].transform.position.y, this.properties.blockDropSpeed * CupheadTime.Delta);
				this.gridBlocks[x, y].blockHeld.transform.position = pos;
				yield return null;
			}
			this.gridBlocks[x, y].blockHeld = null;
			this.gridBlocks[x, spaceBelow].blockHeld = block;
			this.CheckFullGrid();
			base.StartCoroutine(this.check_to_drop_blocks());
			this.ScaleCheck();
		}
		yield break;
	}

	// Token: 0x060015C9 RID: 5577 RVA: 0x0009D478 File Offset: 0x0009B678
	public void GenerateGrid()
	{
		this.gridBlocks = new DicePalaceCardLevelGridBlock[this.GridDimX, this.GridDimY];
		for (int i = 0; i < this.GridDimX; i++)
		{
			for (int j = 0; j < this.GridDimY; j++)
			{
				Vector3 vector;
				vector..ctor((float)i * this.GridSpacing, (float)j * this.GridSpacing);
				this.gridBlocks[i, j] = Object.Instantiate<DicePalaceCardLevelGridBlock>(this.gridBlockPrefab);
				this.gridBlocks[i, j].transform.position = vector + base.transform.position;
				this.gridBlocks[i, j].transform.parent = base.transform;
				this.gridBlocks[i, j].Xcoordinate = (float)i;
				this.gridBlocks[i, j].Ycoordinate = (float)j;
			}
		}
	}

	// Token: 0x060015CA RID: 5578 RVA: 0x0009D568 File Offset: 0x0009B768
	public void CheckFullGrid()
	{
		for (int i = 0; i < this.GridDimX; i++)
		{
			for (int j = 0; j < this.GridDimY; j++)
			{
				if (this.gridBlocks[i, j].blockHeld != null)
				{
					if (this.gridBlocks[i, j].Xcoordinate < (float)(this.GridDimX - 2) && this.gridBlocks[i, j].Ycoordinate < (float)(this.GridDimY - 2) && this.gridBlocks[i, j].hasBlock)
					{
						this.DiagonalsUpCheck(i, j, this.gridBlocks[i, j].blockHeld.suit);
					}
					if (this.gridBlocks[i, j].Xcoordinate < (float)(this.GridDimX - 2) && this.gridBlocks[i, j].Ycoordinate >= 2f && this.gridBlocks[i, j].hasBlock)
					{
						this.DiagonalsDownCheck(i, j, this.gridBlocks[i, j].blockHeld.suit);
					}
					if (this.gridBlocks[i, j].Xcoordinate < (float)(this.GridDimX - 2) && this.gridBlocks[i, j].hasBlock)
					{
						this.RowsCheck(i, j, this.gridBlocks[i, j].blockHeld.suit);
					}
					if (this.gridBlocks[i, j].Ycoordinate < (float)(this.GridDimY - 2) && this.gridBlocks[i, j].hasBlock)
					{
						this.ColumnsCheck(i, j, this.gridBlocks[i, j].blockHeld.suit, true);
					}
				}
			}
		}
	}

	// Token: 0x060015CB RID: 5579 RVA: 0x0009D758 File Offset: 0x0009B958
	public void RowsCheck(int x, int y, DicePalaceCardLevelBlock.Suit suit)
	{
		int num = x + 1;
		int num2 = num + 1;
		int i = num2 + 1;
		if (this.gridBlocks[num, y].blockHeld != null && this.gridBlocks[num2, y].blockHeld != null && this.gridBlocks[num, y].blockHeld.suit == suit && this.gridBlocks[num2, y].blockHeld.suit == suit)
		{
			this.DeleteBlock(this.gridBlocks[x, y]);
			this.DeleteBlock(this.gridBlocks[num, y]);
			this.DeleteBlock(this.gridBlocks[num2, y]);
			if (y < this.GridDimY - 2)
			{
				this.ColumnsCheck(x, y, suit, true);
				this.ColumnsCheck(num, y, suit, true);
				this.ColumnsCheck(num2, y, suit, true);
			}
			if (y >= 2)
			{
				this.ColumnsCheck(x, y, suit, false);
				this.ColumnsCheck(num, y, suit, false);
				this.ColumnsCheck(num2, y, suit, false);
			}
			if (num2 < this.GridDimX - 2)
			{
				this.DiagonalsUpCheck(x, y, suit);
				this.DiagonalsUpCheck(num, y, suit);
				this.DiagonalsUpCheck(num2, y, suit);
			}
			if (y >= 2 && x < this.GridDimX - 2)
			{
				this.DiagonalsDownCheck(x, y, suit);
				this.DiagonalsDownCheck(num, y, suit);
				this.DiagonalsDownCheck(num2, y, suit);
			}
			while (i <= this.GridDimX - 1)
			{
				if (!this.gridBlocks[i, y].hasBlock || this.gridBlocks[i, y].blockHeld.suit != suit)
				{
					break;
				}
				this.DeleteBlock(this.gridBlocks[i, y]);
				if (i >= this.GridDimX)
				{
					break;
				}
				i++;
			}
		}
	}

	// Token: 0x060015CC RID: 5580 RVA: 0x0009D948 File Offset: 0x0009BB48
	public void ColumnsCheck(int x, int y, DicePalaceCardLevelBlock.Suit suit, bool checkingUp)
	{
		int num = y + 1;
		int num2 = num + 1;
		int num3 = y - 1;
		int num4 = y - 2;
		int num5;
		int num6;
		if (checkingUp)
		{
			num5 = num;
			num6 = num2;
		}
		else
		{
			num5 = num3;
			num6 = num4;
		}
		if (this.gridBlocks[x, num5].blockHeld != null && this.gridBlocks[x, num6].blockHeld != null && this.gridBlocks[x, num5].blockHeld.suit == suit && this.gridBlocks[x, num6].blockHeld.suit == suit)
		{
			this.DeleteBlock(this.gridBlocks[x, y]);
			this.DeleteBlock(this.gridBlocks[x, num5]);
			this.DeleteBlock(this.gridBlocks[x, num6]);
			if (x < this.GridDimX - 2)
			{
				if (y >= 2)
				{
					this.DiagonalsDownCheck(x, y, suit);
				}
				if (num >= 2)
				{
					this.DiagonalsDownCheck(x, num, suit);
				}
				if (num2 >= 2)
				{
					this.DiagonalsDownCheck(x, num2, suit);
				}
				this.RowsCheck(x, y, suit);
				this.RowsCheck(x, num, suit);
				this.RowsCheck(x, num2, suit);
				if (num2 < this.GridDimY - 2)
				{
					this.DiagonalsUpCheck(x, y, suit);
					this.DiagonalsUpCheck(x, num, suit);
					this.DiagonalsUpCheck(x, num2, suit);
				}
			}
			this.ExtraCheck(x, y, checkingUp, suit);
		}
	}

	// Token: 0x060015CD RID: 5581 RVA: 0x0009DABC File Offset: 0x0009BCBC
	public void DiagonalsUpCheck(int x, int y, DicePalaceCardLevelBlock.Suit suit)
	{
		int num = x + 1;
		int num2 = num + 1;
		int num3 = num2 + 1;
		int num4 = y + 1;
		int num5 = num4 + 1;
		int num6 = num5 + 1;
		if (this.gridBlocks[num, num4].blockHeld != null && this.gridBlocks[num2, num5].blockHeld != null && this.gridBlocks[num, num4].blockHeld.suit == suit && this.gridBlocks[num2, num5].blockHeld.suit == suit)
		{
			this.DeleteBlock(this.gridBlocks[x, y]);
			this.DeleteBlock(this.gridBlocks[num, num4]);
			this.DeleteBlock(this.gridBlocks[num2, num5]);
			if (num2 < this.GridDimX - 2)
			{
				this.RowsCheck(x, y, suit);
				this.RowsCheck(num, num4, suit);
				this.RowsCheck(num2, num5, suit);
			}
			if (y >= 2)
			{
				this.ColumnsCheck(x, y, suit, false);
			}
			if (num4 >= 2)
			{
				this.ColumnsCheck(num, num4, suit, false);
			}
			this.ColumnsCheck(num2, num5, suit, false);
			if (num5 >= 2)
			{
			}
			while (num3 <= this.GridDimX - 1 && num6 <= this.GridDimY - 1)
			{
				if (!this.gridBlocks[num3, num6].hasBlock || this.gridBlocks[num3, num6].blockHeld.suit != suit)
				{
					break;
				}
				this.DeleteBlock(this.gridBlocks[num3, num6]);
				if (num3 >= this.GridDimX || num6 >= this.GridDimY)
				{
					break;
				}
				num3++;
				num6++;
			}
		}
	}

	// Token: 0x060015CE RID: 5582 RVA: 0x0009DC98 File Offset: 0x0009BE98
	public void DiagonalsDownCheck(int x, int y, DicePalaceCardLevelBlock.Suit suit)
	{
		int num = x + 1;
		int num2 = num + 1;
		int num3 = num2 + 1;
		int num4 = y - 1;
		int num5 = num4 - 1;
		int num6 = num5 - 1;
		if (this.gridBlocks[num, num4].blockHeld != null && this.gridBlocks[num2, num5].blockHeld != null && this.gridBlocks[num, num4].blockHeld.suit == suit && this.gridBlocks[num2, num5].blockHeld.suit == suit)
		{
			this.DeleteBlock(this.gridBlocks[x, y]);
			this.DeleteBlock(this.gridBlocks[num, num4]);
			this.DeleteBlock(this.gridBlocks[num2, num5]);
			if (num2 < this.GridDimX - 2)
			{
				this.RowsCheck(x, y, suit);
				this.RowsCheck(num, num4, suit);
				this.RowsCheck(num2, num5, suit);
			}
			if (num4 >= 2)
			{
				this.ColumnsCheck(num, num4, suit, false);
			}
			if (num5 >= 2)
			{
				this.ColumnsCheck(num2, num5, suit, false);
			}
			this.ColumnsCheck(x, y, suit, false);
			while (num3 <= this.GridDimX - 1 && num6 >= 1)
			{
				if (!this.gridBlocks[num3, num6].hasBlock || this.gridBlocks[num3, num6].blockHeld.suit != suit)
				{
					break;
				}
				this.DeleteBlock(this.gridBlocks[num3, num6]);
				if (num3 >= this.GridDimX || num6 <= 1)
				{
					break;
				}
				num3++;
				num6--;
			}
		}
	}

	// Token: 0x060015CF RID: 5583 RVA: 0x0009DE60 File Offset: 0x0009C060
	public void ExtraCheck(int x, int y, bool checkingUp, DicePalaceCardLevelBlock.Suit suit)
	{
		int i;
		int num;
		if (checkingUp)
		{
			i = y + 1;
			num = 1;
		}
		else
		{
			i = y - 1;
			num = -1;
		}
		while (i <= this.GridDimY - 1)
		{
			if (!this.gridBlocks[x, i].hasBlock || this.gridBlocks[x, i].blockHeld.suit != suit)
			{
				break;
			}
			this.DeleteBlock(this.gridBlocks[x, i]);
			if (i >= this.GridDimY)
			{
				break;
			}
			i += num;
		}
	}

	// Token: 0x060015D0 RID: 5584 RVA: 0x0009DF08 File Offset: 0x0009C108
	public void ScaleCheck()
	{
		Vector3 position = base.transform.position;
		int i = 0;
		float y = 0f;
		bool flag = false;
		for (int j = 0; j < this.GridDimY; j++)
		{
			for (int k = 0; k < this.GridDimX; k++)
			{
				if (this.gridBlocks[k, j].blockHeld != null && (float)j != this.currentHeight)
				{
					y = (float)j;
					flag = true;
					this.currentHeight = (float)j;
				}
			}
			while (i < this.GridDimX - 1)
			{
				if (!(this.gridBlocks[i, j].blockHeld != null))
				{
					break;
				}
				i++;
				if (i == this.GridDimX - 1)
				{
					y = (float)j;
					flag = true;
				}
			}
		}
		if (flag)
		{
			base.StartCoroutine(this.move_scale_cr(y));
		}
		base.transform.position = position;
	}

	// Token: 0x060015D1 RID: 5585 RVA: 0x0009E014 File Offset: 0x0009C214
	public IEnumerator move_scale_cr(float y)
	{
		Vector3 pos = base.transform.position;
		float speed = 200f;
		this.targetPos = this.startingPos - this.gridBlockPrefab.GetComponent<Renderer>().bounds.size.y * (y + 1f);
		while (base.transform.position.y != this.targetPos)
		{
			pos.y = Mathf.MoveTowards(base.transform.position.y, this.targetPos, speed * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060015D2 RID: 5586 RVA: 0x0009E038 File Offset: 0x0009C238
	public void CheckForTop()
	{
		for (int i = 0; i < this.GridDimX; i++)
		{
			if (this.gridBlocks[i, this.GridDimY - 1].hasBlock)
			{
				this.KillAllBlocks();
			}
		}
		this.checkAgain = false;
	}

	// Token: 0x060015D3 RID: 5587 RVA: 0x0009E088 File Offset: 0x0009C288
	public void KillAllBlocks()
	{
		for (int i = 0; i < this.GridDimX; i++)
		{
			for (int j = 0; j < this.GridDimY; j++)
			{
				if (this.gridBlocks[i, j].hasBlock)
				{
					this.DeleteBlock(this.gridBlocks[i, j]);
				}
			}
		}
		this.targetPos = this.startingPos;
		Vector3 position = base.transform.position;
		position.y = this.startingPos;
		base.transform.position = position;
	}

	// Token: 0x060015D4 RID: 5588 RVA: 0x00012816 File Offset: 0x00010A16
	public void DeleteBlock(DicePalaceCardLevelGridBlock gridBlock)
	{
		if (gridBlock.blockHeld != null)
		{
			gridBlock.blockHeld.DestroyBlock();
			gridBlock.blockHeld = null;
			gridBlock.hasBlock = false;
		}
	}

	// Token: 0x040011AE RID: 4526
	[SerializeField]
	public DicePalaceCardLevelColumn columnObject;

	// Token: 0x040011AF RID: 4527
	[SerializeField]
	public DicePalaceCardLevelBlock hearts;

	// Token: 0x040011B0 RID: 4528
	[SerializeField]
	public DicePalaceCardLevelBlock spades;

	// Token: 0x040011B1 RID: 4529
	[SerializeField]
	public DicePalaceCardLevelBlock clubs;

	// Token: 0x040011B2 RID: 4530
	[SerializeField]
	public DicePalaceCardLevelBlock diamonds;

	// Token: 0x040011B3 RID: 4531
	[SerializeField]
	public DicePalaceCardLevelGridBlock gridBlockPrefab;

	// Token: 0x040011B4 RID: 4532
	public List<DicePalaceCardLevelColumn> totalColumns;

	// Token: 0x040011B5 RID: 4533
	public DicePalaceCardLevelGridBlock[,] gridBlocks;

	// Token: 0x040011B6 RID: 4534
	public LevelProperties.DicePalaceCard.Blocks properties;

	// Token: 0x040011B7 RID: 4535
	public float distanceToPlayerY;

	// Token: 0x040011B8 RID: 4536
	public float amountToDropBy;

	// Token: 0x040011B9 RID: 4537
	public float startingPos;

	// Token: 0x040011BA RID: 4538
	public float targetPos;

	// Token: 0x040011BB RID: 4539
	public float currentHeight = -1f;

	// Token: 0x040011BC RID: 4540
	public int GridDimX;

	// Token: 0x040011BD RID: 4541
	public int GridDimY;

	// Token: 0x040011BE RID: 4542
	public float GridSpacing;

	// Token: 0x040011BF RID: 4543
	public bool doneDropping;

	// Token: 0x040011C0 RID: 4544
	public bool checkAgain = true;

	// Token: 0x040011C1 RID: 4545
	public string[] typePattern;

	// Token: 0x040011C2 RID: 4546
	public string[] amountPattern;

	// Token: 0x040011C3 RID: 4547
	public List<int> currentStopYPos;

	// Token: 0x040011C4 RID: 4548
	public int amountIndex;

	// Token: 0x040011C5 RID: 4549
	public int typeIndex;

	// Token: 0x040011C6 RID: 4550
	public DicePalaceCardLevelBlock selectedPrefab;
}
