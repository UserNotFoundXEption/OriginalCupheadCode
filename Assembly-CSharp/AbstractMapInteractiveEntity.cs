using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000471 RID: 1137
public abstract class AbstractMapInteractiveEntity : MapSprite
{
	// Token: 0x06003038 RID: 12344 RVA: 0x000E5034 File Offset: 0x000E3234
	public AbstractMapInteractiveEntity()
	{
	}

	// Token: 0x14000066 RID: 102
	// (add) Token: 0x06003039 RID: 12345 RVA: 0x000E5090 File Offset: 0x000E3290
	// (remove) Token: 0x0600303A RID: 12346 RVA: 0x000E50C8 File Offset: 0x000E32C8
	public event Action OnActivateEvent;

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x0600303B RID: 12347 RVA: 0x0002819E File Offset: 0x0002639E
	// (set) Token: 0x0600303C RID: 12348 RVA: 0x000281A6 File Offset: 0x000263A6
	public AbstractMapInteractiveEntity.State state { get; set; }

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x0600303D RID: 12349 RVA: 0x000281AF File Offset: 0x000263AF
	// (set) Token: 0x0600303E RID: 12350 RVA: 0x000281B7 File Offset: 0x000263B7
	public MapPlayerController playerActivating { get; set; }

	// Token: 0x17000387 RID: 903
	// (get) Token: 0x0600303F RID: 12351 RVA: 0x000281C0 File Offset: 0x000263C0
	// (set) Token: 0x06003040 RID: 12352 RVA: 0x000281C8 File Offset: 0x000263C8
	public MapPlayerController playerChecking { get; set; }

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06003041 RID: 12353 RVA: 0x000281D1 File Offset: 0x000263D1
	public override bool ChangesDepth
	{
		get
		{
			return this.playerCanWalkBehind;
		}
	}

	// Token: 0x06003042 RID: 12354 RVA: 0x000281D9 File Offset: 0x000263D9
	public override void Awake()
	{
		base.Awake();
		AbstractMapInteractiveEntity.HasPopupOpened = false;
		this.lockInput = true;
		base.StartCoroutine(this.lock_input_cr());
	}

	// Token: 0x06003043 RID: 12355 RVA: 0x000E5100 File Offset: 0x000E3300
	public IEnumerator lock_input_cr()
	{
		yield return new WaitForSeconds(1f);
		this.lockInput = false;
		yield return null;
		yield break;
	}

	// Token: 0x06003044 RID: 12356 RVA: 0x000E511C File Offset: 0x000E331C
	public override void Update()
	{
		base.Update();
		if (this.lockInput)
		{
			return;
		}
		if (InterruptingPrompt.IsInterrupting())
		{
			return;
		}
		this.Check();
		if (this.state == AbstractMapInteractiveEntity.State.Activated)
		{
			return;
		}
		if (MapConfirmStartUI.Current.CurrentState != AbstractMapSceneStartUI.State.Inactive || MapDifficultySelectStartUI.Current.CurrentState != AbstractMapSceneStartUI.State.Inactive || MapEventNotification.Current.showing || (Map.Current != null && Map.Current.CurrentState == Map.State.Graveyard) || SceneLoader.IsInBlurTransition)
		{
			return;
		}
		switch (this.interactor)
		{
		default:
			if (this.PlayerWithinDistance(0) && Map.Current.players[0].input.actions.GetButtonDown(13))
			{
				this.Activate(Map.Current.players[0]);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			if (this.PlayerWithinDistance(1) && Map.Current.players[1].input.actions.GetButtonDown(13))
			{
				this.Activate(Map.Current.players[1]);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
			if (this.PlayerWithinDistance(0) && Map.Current.players[0].input.actions.GetButtonDown(13))
			{
				this.Activate(Map.Current.players[0]);
				return;
			}
			if (this.PlayerWithinDistance(1) && Map.Current.players[1].input.actions.GetButtonDown(13))
			{
				this.Activate(Map.Current.players[1]);
				return;
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Both:
			if (Map.Current.players[0] == null || Map.Current.players[1] == null)
			{
				return;
			}
			if (this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1))
			{
				if (Map.Current.players[0].input.actions.GetButtonDown(13) && Map.Current.players[1].input.actions.GetButton(13))
				{
					this.Activate(Map.Current.players[0]);
					return;
				}
				if (Map.Current.players[1].input.actions.GetButtonDown(13) && Map.Current.players[0].input.actions.GetButton(13))
				{
					this.Activate(Map.Current.players[1]);
					return;
				}
			}
			break;
		}
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x000E53E0 File Offset: 0x000E35E0
	public AbstractMapInteractiveEntity.MapActivateData PlayersAbleToActivate()
	{
		AbstractMapInteractiveEntity.MapActivateData result;
		result.length = 0;
		result.controller1 = null;
		result.controller2 = null;
		if (Map.Current.CurrentState != Map.State.Ready)
		{
			return result;
		}
		switch (this.interactor)
		{
		default:
			this.playerChecking = Map.Current.players[0];
			if (this.PlayerWithinDistance(0))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 1, this.playerChecking, null);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			this.playerChecking = Map.Current.players[1];
			if (this.PlayerWithinDistance(1))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 1, this.playerChecking, null);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
			this.playerChecking = Map.Current.players[0];
			if (this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 2, Map.Current.players[0], Map.Current.players[1]);
			}
			if (this.PlayerWithinDistance(0))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 1, Map.Current.players[0], null);
			}
			this.playerChecking = Map.Current.players[1];
			if (this.PlayerWithinDistance(1))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 1, Map.Current.players[1], null);
			}
			break;
		case AbstractMapInteractiveEntity.Interactor.Both:
			this.playerChecking = Map.Current.players[0];
			if (this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1))
			{
				return AbstractMapInteractiveEntity.MapActivateData.Fill(ref result, 2, Map.Current.players[0], Map.Current.players[1]);
			}
			break;
		}
		return result;
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x000E5598 File Offset: 0x000E3798
	public bool AbleToActivate()
	{
		return this.PlayersAbleToActivate().Length > 0;
	}

	// Token: 0x06003047 RID: 12359 RVA: 0x000E55B8 File Offset: 0x000E37B8
	public bool PlayerWithinDistance(int i)
	{
		if (Map.Current.players[i] == null || Map.Current.players[i].state != MapPlayerController.State.Walking || Map.Current.players[i].hideInteractionPrompts)
		{
			return false;
		}
		Vector2 vector = base.transform.position + this.interactionPoint;
		Vector2 vector2 = Map.Current.players[i].transform.position;
		return Vector2.Distance(vector, vector2) <= this.interactionDistance;
	}

	// Token: 0x06003048 RID: 12360 RVA: 0x000E5654 File Offset: 0x000E3854
	public virtual void Check()
	{
		AbstractMapInteractiveEntity.MapActivateData mapActivateData = this.PlayersAbleToActivate();
		this.showed.CopyTo(this.checkPrevious, 0);
		for (int i = 0; i < this.showed.Length; i++)
		{
			this.showed[i] = false;
		}
		for (int j = 0; j < mapActivateData.Length; j++)
		{
			if (mapActivateData[j].id < (PlayerId)this.showed.Length)
			{
				this.showed[(int)mapActivateData[j].id] = true;
			}
		}
		for (int k = 0; k < Map.Current.players.Length; k++)
		{
			if (!(Map.Current.players[k] == null))
			{
				int id = (int)Map.Current.players[k].id;
				if (this.checkPrevious[id] != this.showed[id])
				{
					if (this.showed[id])
					{
						this.dialogues[id] = this.Show(Map.Current.players[k].input);
					}
					else
					{
						this.Hide(this.dialogues[id]);
						this.dialogues[id] = null;
					}
				}
			}
		}
	}

	// Token: 0x06003049 RID: 12361 RVA: 0x000E5794 File Offset: 0x000E3994
	public virtual void ReCheck()
	{
		AbstractMapInteractiveEntity.MapActivateData mapActivateData = this.PlayersAbleToActivate();
		this.CleanUpHiddenPrompts();
		this.showed.CopyTo(this.recheckPrevious, 0);
		for (int i = 0; i < this.showed.Length; i++)
		{
			this.showed[i] = false;
		}
		for (int j = 0; j < mapActivateData.Length; j++)
		{
			if (mapActivateData[j].id < (PlayerId)this.showed.Length)
			{
				this.showed[(int)mapActivateData[j].id] = true;
			}
		}
		for (int k = 0; k < Map.Current.players.Length; k++)
		{
			if (!(Map.Current.players[k] == null))
			{
				int id = (int)Map.Current.players[k].id;
				if (this.recheckPrevious[id] != this.showed[id])
				{
					if (this.showed[id])
					{
						this.dialogues[id] = this.Show(Map.Current.players[k].input);
					}
					else
					{
						this.Hide(this.dialogues[id]);
						this.dialogues[id] = null;
					}
				}
			}
		}
	}

	// Token: 0x0600304A RID: 12362 RVA: 0x000E58D8 File Offset: 0x000E3AD8
	public virtual void CleanUpHiddenPrompts()
	{
		for (int i = 0; i < this.showed.Length; i++)
		{
			if (this.showed[i] && this.dialogues[i] == null)
			{
				this.showed[i] = false;
			}
		}
	}

	// Token: 0x0600304B RID: 12363 RVA: 0x000E5928 File Offset: 0x000E3B28
	public virtual void Activate(MapPlayerController player)
	{
		MapUIInteractionDialogue mapUIInteractionDialogue = this.dialogues[(int)player.id];
		if (mapUIInteractionDialogue == null)
		{
			return;
		}
		this.playerActivating = player;
		mapUIInteractionDialogue.Close();
		this.state = AbstractMapInteractiveEntity.State.Activated;
		if (this.OnActivateEvent != null)
		{
			this.OnActivateEvent();
		}
		this.Activate();
	}

	// Token: 0x0600304C RID: 12364 RVA: 0x000281FB File Offset: 0x000263FB
	public virtual void Activate()
	{
	}

	// Token: 0x0600304D RID: 12365 RVA: 0x000281FD File Offset: 0x000263FD
	public virtual MapUIInteractionDialogue Show(PlayerInput player)
	{
		AudioManager.Play("world_map_level_bubble_appear");
		this.state = AbstractMapInteractiveEntity.State.Ready;
		return MapUIInteractionDialogue.Create(this.dialogueProperties, player, this.dialogueOffset);
	}

	// Token: 0x0600304E RID: 12366 RVA: 0x00028222 File Offset: 0x00026422
	public virtual void Hide(MapUIInteractionDialogue dialogue)
	{
		AudioManager.Play("world_map_level_bubble_disappear");
		if (dialogue == null)
		{
			return;
		}
		dialogue.Close();
		dialogue = null;
		this.state = AbstractMapInteractiveEntity.State.Inactive;
	}

	// Token: 0x0600304F RID: 12367 RVA: 0x000E5984 File Offset: 0x000E3B84
	public void SetPlayerReturnPos()
	{
		PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.playerOne;
		PlayerData.Data.CurrentMapData.playerTwoPosition = base.transform.position + this.returnPositions.playerTwo;
		if (!PlayerManager.Multiplayer)
		{
			PlayerData.Data.CurrentMapData.playerOnePosition = base.transform.position + this.returnPositions.singlePlayer;
		}
	}

	// Token: 0x06003050 RID: 12368 RVA: 0x0002824B File Offset: 0x0002644B
	public override void OnDestroy()
	{
		base.OnDestroy();
		AbstractMapInteractiveEntity.HasPopupOpened = false;
	}

	// Token: 0x06003051 RID: 12369 RVA: 0x000E5A38 File Offset: 0x000E3C38
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.dialogueOffset, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.dialogueOffset, 0.06f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, 0.06f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, this.interactionDistance);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, this.interactionDistance + 0.01f);
		Vector3 vector;
		vector..ctor(0.3f, 0.3f, 0.3f);
		Vector3 vector2 = vector * 0.9f;
		Vector3 vector3;
		vector3..ctor(0.25f, 0.25f, 0.25f);
		Vector3 vector4 = vector3 * 0.9f;
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(this.returnPositions.singlePlayer + base.transform.position, vector);
		Gizmos.color = Color.black;
		Gizmos.DrawWireCube(this.returnPositions.playerOne + base.transform.position, vector3);
		Gizmos.DrawWireCube(this.returnPositions.playerTwo + base.transform.position, vector3);
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(this.returnPositions.singlePlayer + base.transform.position, vector2);
		Gizmos.DrawWireCube(this.returnPositions.playerOne + base.transform.position, vector4);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(this.returnPositions.playerTwo + base.transform.position, vector4);
		Gizmos.color = Color.white;
	}

	// Token: 0x06003052 RID: 12370 RVA: 0x000E5CE8 File Offset: 0x000E3EE8
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (!Application.isPlaying)
		{
			return;
		}
		switch (this.interactor)
		{
		case AbstractMapInteractiveEntity.Interactor.Cuphead:
			this.DrawGizmoLineToPlayer(0, this.PlayerWithinDistance(0));
			break;
		case AbstractMapInteractiveEntity.Interactor.Mugman:
			this.DrawGizmoLineToPlayer(1, this.PlayerWithinDistance(1));
			break;
		case AbstractMapInteractiveEntity.Interactor.Either:
			this.DrawGizmoLineToPlayer(0, this.PlayerWithinDistance(0));
			this.DrawGizmoLineToPlayer(1, this.PlayerWithinDistance(1));
			break;
		case AbstractMapInteractiveEntity.Interactor.Both:
			this.DrawGizmoLineToPlayer(0, this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1));
			this.DrawGizmoLineToPlayer(1, this.PlayerWithinDistance(0) && this.PlayerWithinDistance(1));
			break;
		}
	}

	// Token: 0x06003053 RID: 12371 RVA: 0x000E5DB0 File Offset: 0x000E3FB0
	public void DrawGizmoLineToPlayer(int i, bool valid)
	{
		if (Map.Current.players[i] == null)
		{
			return;
		}
		Gizmos.color = ((!valid) ? Color.red : Color.green);
		Gizmos.DrawLine(base.transform.position + this.interactionPoint, Map.Current.players[i].transform.position);
	}

	// Token: 0x040027DE RID: 10206
	public const string MapWorld1 = "MapWorld_1";

	// Token: 0x040027DF RID: 10207
	public const string MapWorld2 = "MapWorld_2";

	// Token: 0x040027E0 RID: 10208
	public const string MapWorld3 = "MapWorld_3";

	// Token: 0x040027E1 RID: 10209
	public const string MapWorld4Exit = "KingDiceToWorld3WorldMap";

	// Token: 0x040027E2 RID: 10210
	public const string Inkwell = "Inkwell";

	// Token: 0x040027E3 RID: 10211
	public const string Mausoleum = "Mausoleum";

	// Token: 0x040027E4 RID: 10212
	public const string Mausoleum1 = "Mausoleum_1";

	// Token: 0x040027E5 RID: 10213
	public const string Mausoleum2 = "Mausoleum_2";

	// Token: 0x040027E6 RID: 10214
	public const string Mausoleum3 = "Mausoleum_3";

	// Token: 0x040027E7 RID: 10215
	public const string Devil = "Devil";

	// Token: 0x040027E8 RID: 10216
	public const string DicePalaceMain = "DicePalaceMain";

	// Token: 0x040027E9 RID: 10217
	public const string KingDice = "KingDice";

	// Token: 0x040027EA RID: 10218
	public const string Shop = "Shop";

	// Token: 0x040027EB RID: 10219
	public const string ElderKettleLevel = "ElderKettleLevel";

	// Token: 0x040027EC RID: 10220
	public const string Kitchen = "BakeryWorldMap";

	// Token: 0x040027ED RID: 10221
	public const string KitchenFight = "Saltbaker";

	// Token: 0x040027EE RID: 10222
	public const string KingOfGamesCastle = "KingOfGamesWorldMap";

	// Token: 0x040027EF RID: 10223
	public static bool HasPopupOpened;

	// Token: 0x040027F1 RID: 10225
	public AbstractMapInteractiveEntity.Interactor interactor = AbstractMapInteractiveEntity.Interactor.Either;

	// Token: 0x040027F2 RID: 10226
	public Vector2 interactionPoint;

	// Token: 0x040027F3 RID: 10227
	public float interactionDistance = 1f;

	// Token: 0x040027F4 RID: 10228
	public AbstractUIInteractionDialogue.Properties dialogueProperties;

	// Token: 0x040027F5 RID: 10229
	public Vector2 dialogueOffset;

	// Token: 0x040027F6 RID: 10230
	public AbstractMapInteractiveEntity.PositionProperties returnPositions;

	// Token: 0x040027F7 RID: 10231
	public bool playerCanWalkBehind = true;

	// Token: 0x040027FB RID: 10235
	[HideInInspector]
	public MapUIInteractionDialogue[] dialogues = new MapUIInteractionDialogue[2];

	// Token: 0x040027FC RID: 10236
	public bool lastInteractable;

	// Token: 0x040027FD RID: 10237
	public bool lockInput;

	// Token: 0x040027FE RID: 10238
	public bool[] showed = new bool[2];

	// Token: 0x040027FF RID: 10239
	public bool[] checkPrevious = new bool[2];

	// Token: 0x04002800 RID: 10240
	public bool[] recheckPrevious = new bool[2];

	// Token: 0x020010EC RID: 4332
	public enum Interactor
	{
		// Token: 0x040077EF RID: 30703
		Cuphead,
		// Token: 0x040077F0 RID: 30704
		Mugman,
		// Token: 0x040077F1 RID: 30705
		Either,
		// Token: 0x040077F2 RID: 30706
		Both
	}

	// Token: 0x020010ED RID: 4333
	public enum State
	{
		// Token: 0x040077F4 RID: 30708
		Inactive,
		// Token: 0x040077F5 RID: 30709
		Ready,
		// Token: 0x040077F6 RID: 30710
		Activated
	}

	// Token: 0x020010EE RID: 4334
	public struct MapActivateData
	{
		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x06007BBA RID: 31674 RVA: 0x0005358C File Offset: 0x0005178C
		public int Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x17001761 RID: 5985
		public MapPlayerController this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.controller1;
				}
				if (index == 1)
				{
					return this.controller2;
				}
				return null;
			}
		}

		// Token: 0x06007BBC RID: 31676 RVA: 0x000535B2 File Offset: 0x000517B2
		public static AbstractMapInteractiveEntity.MapActivateData Fill(ref AbstractMapInteractiveEntity.MapActivateData mapActivateData, int length, MapPlayerController controller1 = null, MapPlayerController controller2 = null)
		{
			mapActivateData.length = length;
			if (length >= 1)
			{
				mapActivateData.controller1 = controller1;
			}
			if (length >= 2)
			{
				mapActivateData.controller2 = controller2;
			}
			return mapActivateData;
		}

		// Token: 0x040077F7 RID: 30711
		public int length;

		// Token: 0x040077F8 RID: 30712
		public MapPlayerController controller1;

		// Token: 0x040077F9 RID: 30713
		public MapPlayerController controller2;
	}

	// Token: 0x020010EF RID: 4335
	[Serializable]
	public class PositionProperties
	{
		// Token: 0x040077FA RID: 30714
		[Header("One Player")]
		public Vector2 singlePlayer = new Vector2(0f, -1f);

		// Token: 0x040077FB RID: 30715
		[Header("Two Players")]
		public Vector2 playerOne = new Vector2(-1f, -1f);

		// Token: 0x040077FC RID: 30716
		public Vector2 playerTwo = new Vector2(1f, -1f);
	}
}
