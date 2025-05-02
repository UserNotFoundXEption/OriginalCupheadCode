using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000478 RID: 1144
public class MapCastleZones : MonoBehaviour
{
	// Token: 0x06003083 RID: 12419 RVA: 0x000E66E8 File Offset: 0x000E48E8
	public void OnEnable()
	{
		foreach (MapCastleZoneCollider mapCastleZoneCollider in this.zones)
		{
			mapCastleZoneCollider.OnMapCastleZoneCollision += this.onMapCastleZoneCollision;
		}
	}

	// Token: 0x06003084 RID: 12420 RVA: 0x000E6728 File Offset: 0x000E4928
	public void OnDisable()
	{
		foreach (MapCastleZoneCollider mapCastleZoneCollider in this.zones)
		{
			mapCastleZoneCollider.OnMapCastleZoneCollision -= this.onMapCastleZoneCollision;
		}
	}

	// Token: 0x06003085 RID: 12421 RVA: 0x0002842D File Offset: 0x0002662D
	public void showLadder(MapCastleZoneCollider zone)
	{
		this.ladder.transform.position = zone.interactionPoint.position;
		this.ladder.returnPositions = zone.returnPositions;
		base.StartCoroutine(this.showLadder_cr(zone));
	}

	// Token: 0x06003086 RID: 12422 RVA: 0x000E6768 File Offset: 0x000E4968
	public IEnumerator showLadder_cr(MapCastleZoneCollider zone)
	{
		this.ladder.animator.SetBool("Down", true);
		yield return this.ladder.animator.WaitForAnimationToStart(this, "Drop", false);
		AudioManager.Play("worldmap_kog_ladder_down");
		this.ladder.EnableShadow(zone.enableLadderShadow);
		yield return this.ladder.animator.WaitForAnimationToEnd(this, "Drop", false, true);
		this.ladder.enabled = true;
		yield break;
	}

	// Token: 0x06003087 RID: 12423 RVA: 0x00028469 File Offset: 0x00026669
	public void hideLadder()
	{
		base.StartCoroutine(this.hideLadder_cr());
	}

	// Token: 0x06003088 RID: 12424 RVA: 0x000E678C File Offset: 0x000E498C
	public IEnumerator hideLadder_cr()
	{
		this.ladder.animator.SetBool("Down", false);
		yield return this.ladder.animator.WaitForAnimationToStart(this, "Up", false);
		AudioManager.Play("worldmap_kog_ladder_up");
		this.ladder.enabled = false;
		yield break;
	}

	// Token: 0x06003089 RID: 12425 RVA: 0x000E67A8 File Offset: 0x000E49A8
	public void onMapCastleZoneCollision(MapCastleZoneCollider collider, GameObject other, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Enter)
		{
			if (this.currentZone == null)
			{
				PlayerData data = PlayerData.Data;
				if (data.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length)
				{
					if (collider.zone == MapCastleZones.Zone.Dock)
					{
						this.currentZone = collider;
					}
				}
				else if (collider.zone != MapCastleZones.Zone.Dock)
				{
					if (data.currentChessBossZone == MapCastleZones.Zone.None)
					{
						int num = data.CountLevelsCompleted(Level.worldDLCBossLevels);
						int count = data.usedChessBossZones.Count;
						if (count <= num && !data.usedChessBossZones.Contains(collider.zone))
						{
							this.currentZone = collider;
							data.currentChessBossZone = collider.zone;
							PlayerData.SaveCurrentFile();
						}
					}
					else if (data.currentChessBossZone == collider.zone)
					{
						this.currentZone = collider;
					}
				}
				if (this.currentZone != null)
				{
					this.showLadder(this.currentZone);
				}
			}
			if (collider == this.currentZone)
			{
				this.currentZonePlayerCount++;
			}
		}
		else if (phase == CollisionPhase.Exit && collider == this.currentZone)
		{
			this.currentZonePlayerCount--;
			if (this.currentZonePlayerCount == 0)
			{
				this.currentZone = null;
				this.hideLadder();
			}
		}
	}

	// Token: 0x04002822 RID: 10274
	public static readonly MapCastleZones.Zone[] RegularZones = new MapCastleZones.Zone[]
	{
		MapCastleZones.Zone.OldMan,
		MapCastleZones.Zone.RumRunners,
		MapCastleZones.Zone.Cowgirl,
		MapCastleZones.Zone.DogFight,
		MapCastleZones.Zone.SnowCult
	};

	// Token: 0x04002823 RID: 10275
	[SerializeField]
	public MapCastleZoneCollider[] zones;

	// Token: 0x04002824 RID: 10276
	[SerializeField]
	public MapLevelLoaderLadder ladder;

	// Token: 0x04002825 RID: 10277
	public MapCastleZoneCollider currentZone;

	// Token: 0x04002826 RID: 10278
	public int currentZonePlayerCount;

	// Token: 0x020010F5 RID: 4341
	public enum Zone
	{
		// Token: 0x04007810 RID: 30736
		None,
		// Token: 0x04007811 RID: 30737
		OldMan,
		// Token: 0x04007812 RID: 30738
		RumRunners,
		// Token: 0x04007813 RID: 30739
		Cowgirl,
		// Token: 0x04007814 RID: 30740
		DogFight,
		// Token: 0x04007815 RID: 30741
		SnowCult,
		// Token: 0x04007816 RID: 30742
		Dock
	}
}
