using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001E2 RID: 482
public class DicePalaceDominoLevelFloor : AbstractCollidableObject
{
	// Token: 0x06001663 RID: 5731 RVA: 0x000130E0 File Offset: 0x000112E0
	public void InitFloor(LevelProperties.DicePalaceDomino properties)
	{
		this.properties = properties;
		this.tiles = new List<DicePalaceDominoLevelFloorTile>();
		this.preTiles = new List<DicePalaceDominoLevelFloorTile>();
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x000130FF File Offset: 0x000112FF
	public void StartSpawningTiles()
	{
		base.StartCoroutine(this.tileSpawn_cr());
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x0009EF64 File Offset: 0x0009D164
	public IEnumerator tileSpawn_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		for (int i = 0; i < this._floors.Length; i++)
		{
			this._floors[i].speed = this.properties.CurrentState.domino.floorSpeed;
		}
		this._teethSprite.speed = this.properties.CurrentState.domino.floorSpeed;
		this.AddForces();
		for (int j = 0; j < this.preTiles.Count; j++)
		{
			if (this.preTiles[j].currentColourIndex == (int)this.spikesColour)
			{
				this.preTiles[j].TriggerSpikes(true);
			}
			else
			{
				this.preTiles[j].TriggerSpikes(false);
			}
			this.preTiles[j].InitTile();
		}
		yield break;
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x0009EF80 File Offset: 0x0009D180
	public void AddForces()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				this.levelForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, -this.properties.CurrentState.domino.floorSpeed);
				levelPlayerController.motor.AddForce(this.levelForce);
			}
		}
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x0001310E File Offset: 0x0001130E
	public int ParseColour(char c)
	{
		if (c == 'B')
		{
			return 0;
		}
		if (c == 'G')
		{
			return 1;
		}
		if (c == 'R')
		{
			return 2;
		}
		if (c != 'Y')
		{
			return 0;
		}
		return 3;
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x0001313E File Offset: 0x0001133E
	public void CheckTiles(DicePalaceDominoLevelBouncyBall.Colour color)
	{
		this.spikesColour = color;
		base.StartCoroutine(this.check_tiles_cr(this.spikesColour));
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x0009F020 File Offset: 0x0009D220
	public IEnumerator check_tiles_cr(DicePalaceDominoLevelBouncyBall.Colour color)
	{
		foreach (DicePalaceDominoLevelFloorTile dicePalaceDominoLevelFloorTile in this.tiles)
		{
			if (dicePalaceDominoLevelFloorTile.isActivated)
			{
				if (dicePalaceDominoLevelFloorTile.currentColourIndex == (int)color)
				{
					dicePalaceDominoLevelFloorTile.TriggerSpikes(true);
				}
				else
				{
					dicePalaceDominoLevelFloorTile.TriggerSpikes(false);
				}
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x04001230 RID: 4656
	[Header("Floor")]
	[SerializeField]
	public DicePalaceDominoLevelScrollingFloor[] _floors;

	// Token: 0x04001231 RID: 4657
	[SerializeField]
	public ScrollingSprite _teethSprite;

	// Token: 0x04001232 RID: 4658
	public DicePalaceDominoLevelBouncyBall.Colour spikesColour = DicePalaceDominoLevelBouncyBall.Colour.none;

	// Token: 0x04001233 RID: 4659
	public Action OnToggleFlashEvent;

	// Token: 0x04001234 RID: 4660
	public Action OnColourChangeEvent;

	// Token: 0x04001235 RID: 4661
	public LevelProperties.DicePalaceDomino properties;

	// Token: 0x04001236 RID: 4662
	public List<DicePalaceDominoLevelFloorTile> tiles;

	// Token: 0x04001237 RID: 4663
	public List<DicePalaceDominoLevelFloorTile> preTiles;

	// Token: 0x04001238 RID: 4664
	public LevelPlayerMotor.VelocityManager.Force levelForce;
}
