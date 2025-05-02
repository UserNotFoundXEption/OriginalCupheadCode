using System;
using UnityEngine;

// Token: 0x020000FC RID: 252
public abstract class AbstractLevelInteractiveEntity : AbstractPausableComponent
{
	// Token: 0x06000BB5 RID: 2997 RVA: 0x0000A616 File Offset: 0x00008816
	public AbstractLevelInteractiveEntity()
	{
	}

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06000BB6 RID: 2998 RVA: 0x00080FF0 File Offset: 0x0007F1F0
	// (remove) Token: 0x06000BB7 RID: 2999 RVA: 0x00081028 File Offset: 0x0007F228
	public event Action OnActivateEvent;

	// Token: 0x170001DC RID: 476
	// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0000A63E File Offset: 0x0000883E
	// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x0000A646 File Offset: 0x00008846
	public AbstractLevelInteractiveEntity.State state { get; set; }

	// Token: 0x170001DD RID: 477
	// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0000A64F File Offset: 0x0000884F
	// (set) Token: 0x06000BBB RID: 3003 RVA: 0x0000A657 File Offset: 0x00008857
	public AbstractPlayerController playerActivating { get; set; }

	// Token: 0x06000BBC RID: 3004 RVA: 0x0000A660 File Offset: 0x00008860
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06000BBD RID: 3005 RVA: 0x0000A668 File Offset: 0x00008868
	public void Start()
	{
		Localization.OnLanguageChangedEvent += this.OnLanguageChanged;
	}

	// Token: 0x06000BBE RID: 3006 RVA: 0x0000A67B File Offset: 0x0000887B
	public override void OnDestroy()
	{
		base.OnDestroy();
		Localization.OnLanguageChangedEvent -= this.OnLanguageChanged;
	}

	// Token: 0x06000BBF RID: 3007 RVA: 0x0000A694 File Offset: 0x00008894
	public void OnLanguageChanged()
	{
		this.Hide(PlayerId.PlayerOne);
		this.Hide(PlayerId.PlayerTwo);
		this.lastInteractable = !this.lastInteractable;
	}

	// Token: 0x06000BC0 RID: 3008 RVA: 0x00081060 File Offset: 0x0007F260
	public void FixedUpdate()
	{
		this.Check();
		if (this.state == AbstractLevelInteractiveEntity.State.Activated)
		{
			return;
		}
		switch (this.interactor)
		{
		default:
			if (this.PlayerWithinDistance(PlayerId.PlayerOne) && PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) && !this.PlayerIsDashing(PlayerId.PlayerOne))
			{
				this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne));
			}
			break;
		case AbstractLevelInteractiveEntity.Interactor.Mugman:
			if (this.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) && !this.PlayerIsDashing(PlayerId.PlayerTwo))
			{
				this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo));
			}
			break;
		case AbstractLevelInteractiveEntity.Interactor.Either:
			if (this.PlayerWithinDistance(PlayerId.PlayerOne) || this.PlayerWithinDistance(PlayerId.PlayerTwo))
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) && this.PlayerWithinDistance(PlayerId.PlayerOne) && !this.PlayerIsDashing(PlayerId.PlayerOne))
				{
					this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne));
					return;
				}
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) == null)
				{
					return;
				}
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) && this.PlayerWithinDistance(PlayerId.PlayerTwo) && !this.PlayerIsDashing(PlayerId.PlayerTwo))
				{
					this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo));
					return;
				}
			}
			break;
		case AbstractLevelInteractiveEntity.Interactor.Both:
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne) == null || PlayerManager.GetPlayer(PlayerId.PlayerTwo) == null)
			{
				return;
			}
			if (this.PlayerWithinDistance(PlayerId.PlayerOne) && this.PlayerWithinDistance(PlayerId.PlayerTwo))
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButtonDown(13) && this.PlayerWithinDistance(PlayerId.PlayerOne) && PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButton(13) && this.PlayerWithinDistance(PlayerId.PlayerTwo) && !this.PlayerIsDashing(PlayerId.PlayerOne) && !this.PlayerIsDashing(PlayerId.PlayerTwo))
				{
					this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerOne));
					return;
				}
				if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).input.actions.GetButtonDown(13) && this.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerOne).input.actions.GetButton(13) && this.PlayerWithinDistance(PlayerId.PlayerOne) && !this.PlayerIsDashing(PlayerId.PlayerOne) && !this.PlayerIsDashing(PlayerId.PlayerTwo))
				{
					this.Activate(PlayerManager.GetPlayer(PlayerId.PlayerTwo));
					return;
				}
			}
			break;
		}
	}

	// Token: 0x06000BC1 RID: 3009 RVA: 0x00081308 File Offset: 0x0007F508
	public bool AbleToActivate()
	{
		switch (this.interactor)
		{
		default:
			return this.PlayerWithinDistance(PlayerId.PlayerOne);
		case AbstractLevelInteractiveEntity.Interactor.Mugman:
			return this.PlayerWithinDistance(PlayerId.PlayerTwo);
		case AbstractLevelInteractiveEntity.Interactor.Either:
			return this.PlayerWithinDistance(PlayerId.PlayerOne) || this.PlayerWithinDistance(PlayerId.PlayerTwo);
		case AbstractLevelInteractiveEntity.Interactor.Both:
			return this.PlayerWithinDistance(PlayerId.PlayerOne) && this.PlayerWithinDistance(PlayerId.PlayerTwo);
		}
	}

	// Token: 0x06000BC2 RID: 3010 RVA: 0x00081390 File Offset: 0x0007F590
	public bool PlayerWithinDistance(PlayerId id)
	{
		if (PlayerManager.GetPlayer(id) == null)
		{
			return false;
		}
		Vector2 vector = base.transform.position + this.interactionPoint;
		Vector2 vector2 = PlayerManager.GetPlayer(id).transform.position;
		return Vector2.Distance(vector, vector2) <= this.interactionDistance;
	}

	// Token: 0x06000BC3 RID: 3011 RVA: 0x000813F4 File Offset: 0x0007F5F4
	public bool PlayerIsDashing(PlayerId id)
	{
		if (PlayerManager.GetPlayer(id) == null)
		{
			return false;
		}
		if (PlayerManager.GetPlayer(id).GetComponent<LevelPlayerMotor>() != null)
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)PlayerManager.GetPlayer(id);
			return levelPlayerController.motor.Dashing;
		}
		return false;
	}

	// Token: 0x06000BC4 RID: 3012 RVA: 0x00081444 File Offset: 0x0007F644
	public virtual void Check()
	{
		bool flag = this.AbleToActivate();
		if (flag != this.lastInteractable)
		{
			if (flag)
			{
				if (this.PlayerWithinDistance(PlayerId.PlayerOne))
				{
					this.Show(PlayerId.PlayerOne);
				}
				else if (this.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					this.Show(PlayerId.PlayerTwo);
				}
			}
			else if (!this.PlayerWithinDistance(PlayerId.PlayerOne))
			{
				this.Hide(PlayerId.PlayerOne);
			}
			else if (!this.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				this.Hide(PlayerId.PlayerTwo);
			}
		}
		this.lastInteractable = flag;
	}

	// Token: 0x06000BC5 RID: 3013 RVA: 0x000814F0 File Offset: 0x0007F6F0
	public void Activate(AbstractPlayerController player)
	{
		if (this.dialogue == null)
		{
			return;
		}
		this.playerActivating = player;
		this.dialogue.Close();
		this.dialogue = null;
		this.state = AbstractLevelInteractiveEntity.State.Activated;
		if (this.OnActivateEvent != null)
		{
			this.OnActivateEvent();
		}
		this.Activate();
	}

	// Token: 0x06000BC6 RID: 3014 RVA: 0x0000A6B3 File Offset: 0x000088B3
	public virtual void Activate()
	{
	}

	// Token: 0x06000BC7 RID: 3015 RVA: 0x0008154C File Offset: 0x0007F74C
	public virtual void Show(PlayerId playerId)
	{
		this.state = AbstractLevelInteractiveEntity.State.Ready;
		this.dialogueProperties.text = string.Empty;
		this.dialogue = LevelUIInteractionDialogue.Create(this.dialogueProperties, PlayerManager.GetPlayer(playerId).input, this.dialogueOffset, 0f, LevelUIInteractionDialogue.TailPosition.Bottom, this.hasTarget);
	}

	// Token: 0x06000BC8 RID: 3016 RVA: 0x0000A6B5 File Offset: 0x000088B5
	public virtual void Hide(PlayerId playerId)
	{
		if (this.dialogue == null)
		{
			return;
		}
		this.dialogue.Close();
		this.dialogue = null;
		this.state = AbstractLevelInteractiveEntity.State.Inactive;
	}

	// Token: 0x06000BC9 RID: 3017 RVA: 0x000815A0 File Offset: 0x0007F7A0
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.dialogueOffset, Mathf.Min(5f, this.interactionDistance));
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.dialogueOffset, Mathf.Min(6f, this.interactionDistance + 1f));
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, this.interactionDistance);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(base.baseTransform.position + this.interactionPoint, this.interactionDistance + 1f);
	}

	// Token: 0x06000BCA RID: 3018 RVA: 0x000816A8 File Offset: 0x0007F8A8
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (!Application.isPlaying)
		{
			return;
		}
		switch (this.interactor)
		{
		case AbstractLevelInteractiveEntity.Interactor.Cuphead:
			this.DrawGizmoLineToPlayer(PlayerId.PlayerOne, this.PlayerWithinDistance(PlayerId.PlayerOne));
			break;
		case AbstractLevelInteractiveEntity.Interactor.Mugman:
			this.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, this.PlayerWithinDistance(PlayerId.PlayerTwo));
			break;
		case AbstractLevelInteractiveEntity.Interactor.Either:
			this.DrawGizmoLineToPlayer(PlayerId.PlayerOne, this.PlayerWithinDistance(PlayerId.PlayerOne));
			this.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, this.PlayerWithinDistance(PlayerId.PlayerTwo));
			break;
		case AbstractLevelInteractiveEntity.Interactor.Both:
			this.DrawGizmoLineToPlayer(PlayerId.PlayerOne, this.PlayerWithinDistance(PlayerId.PlayerOne) && this.PlayerWithinDistance(PlayerId.PlayerTwo));
			this.DrawGizmoLineToPlayer(PlayerId.PlayerTwo, this.PlayerWithinDistance(PlayerId.PlayerOne) && this.PlayerWithinDistance(PlayerId.PlayerTwo));
			break;
		}
	}

	// Token: 0x06000BCB RID: 3019 RVA: 0x00081770 File Offset: 0x0007F970
	public void DrawGizmoLineToPlayer(PlayerId id, bool valid)
	{
		if (PlayerManager.GetPlayer(id) == null)
		{
			return;
		}
		Gizmos.color = ((!valid) ? Color.red : Color.green);
		Gizmos.DrawLine(base.transform.position + this.interactionPoint, PlayerManager.GetPlayer(id).transform.position);
	}

	// Token: 0x04000968 RID: 2408
	public AbstractLevelInteractiveEntity.Interactor interactor = AbstractLevelInteractiveEntity.Interactor.Either;

	// Token: 0x04000969 RID: 2409
	public Vector2 interactionPoint;

	// Token: 0x0400096A RID: 2410
	public float interactionDistance = 100f;

	// Token: 0x0400096B RID: 2411
	public AbstractUIInteractionDialogue.Properties dialogueProperties;

	// Token: 0x0400096C RID: 2412
	public Vector2 dialogueOffset;

	// Token: 0x0400096D RID: 2413
	public bool once = true;

	// Token: 0x0400096E RID: 2414
	public bool hasTarget = true;

	// Token: 0x04000971 RID: 2417
	public LevelUIInteractionDialogue dialogue;

	// Token: 0x04000972 RID: 2418
	public bool lastInteractable;

	// Token: 0x02000975 RID: 2421
	public enum Interactor
	{
		// Token: 0x040046D2 RID: 18130
		Cuphead,
		// Token: 0x040046D3 RID: 18131
		Mugman,
		// Token: 0x040046D4 RID: 18132
		Either,
		// Token: 0x040046D5 RID: 18133
		Both
	}

	// Token: 0x02000976 RID: 2422
	public enum State
	{
		// Token: 0x040046D7 RID: 18135
		Inactive,
		// Token: 0x040046D8 RID: 18136
		Ready,
		// Token: 0x040046D9 RID: 18137
		Activated
	}
}
