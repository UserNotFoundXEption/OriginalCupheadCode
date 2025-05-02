using System;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class LevelUIInteractionDialogue : AbstractUIInteractionDialogue
{
	// Token: 0x06000C48 RID: 3144 RVA: 0x00082E44 File Offset: 0x00081044
	public static LevelUIInteractionDialogue Create(AbstractUIInteractionDialogue.Properties properties, PlayerInput player, Vector2 offset, float glyphOffsetAddition = 0f, LevelUIInteractionDialogue.TailPosition tailPosition = LevelUIInteractionDialogue.TailPosition.Bottom, bool playerTarget = true)
	{
		LevelUIInteractionDialogue levelUIInteractionDialogue = Object.Instantiate<LevelUIInteractionDialogue>(Level.Current.LevelResources.levelUIInteractionDialogue);
		levelUIInteractionDialogue.glyphOffsetAddition = glyphOffsetAddition;
		levelUIInteractionDialogue.tailPosition = tailPosition;
		levelUIInteractionDialogue.Init(properties, player, offset);
		if (tailPosition == LevelUIInteractionDialogue.TailPosition.Right)
		{
			levelUIInteractionDialogue.dialogueOffset = new Vector2(offset.x - levelUIInteractionDialogue.back.sizeDelta.x * 0.5f - 14f, offset.y);
		}
		else if (tailPosition == LevelUIInteractionDialogue.TailPosition.Left)
		{
			levelUIInteractionDialogue.dialogueOffset = new Vector2(offset.x + levelUIInteractionDialogue.back.sizeDelta.x * 0.5f + 14f, offset.y);
		}
		if (!playerTarget && LevelUIInteractionDialogue.defaultTarget == null)
		{
			LevelUIInteractionDialogue.defaultTarget = GameObject.CreatePrimitive(3);
			LevelUIInteractionDialogue.defaultTarget.transform.position = Vector3.zero;
			LevelUIInteractionDialogue.defaultTarget.transform.localScale = Vector3.zero;
			levelUIInteractionDialogue.target = LevelUIInteractionDialogue.defaultTarget.transform;
		}
		else if (!playerTarget)
		{
			levelUIInteractionDialogue.target = LevelUIInteractionDialogue.defaultTarget.transform;
		}
		return levelUIInteractionDialogue;
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x06000C49 RID: 3145 RVA: 0x00082F7C File Offset: 0x0008117C
	public override float PreferredWidth
	{
		get
		{
			if (this.tmpText.text.Length == 0)
			{
				return this.tmpText.preferredWidth + this.glyph.preferredWidth + 5.3f + this.glyphOffsetAddition;
			}
			return this.tmpText.preferredWidth + this.glyph.preferredWidth + 27f + this.glyphOffsetAddition;
		}
	}

	// Token: 0x06000C4A RID: 3146 RVA: 0x0000AC16 File Offset: 0x00008E16
	public override void Awake()
	{
		base.Awake();
		base.transform.SetParent(LevelHUD.Current.Canvas.transform, false);
	}

	// Token: 0x06000C4B RID: 3147 RVA: 0x0000AC39 File Offset: 0x00008E39
	public override void Init(AbstractUIInteractionDialogue.Properties properties, PlayerInput player, Vector2 offset)
	{
		base.Init(properties, player, offset);
		this.UpdatePos();
	}

	// Token: 0x06000C4C RID: 3148 RVA: 0x0000AC4A File Offset: 0x00008E4A
	public void Update()
	{
		this.UpdatePos();
		this.UpdateTailPosition();
	}

	// Token: 0x06000C4D RID: 3149 RVA: 0x0000AC58 File Offset: 0x00008E58
	public virtual void UpdatePos()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position + this.dialogueOffset;
		}
	}

	// Token: 0x06000C4E RID: 3150 RVA: 0x00082FE8 File Offset: 0x000811E8
	public void UpdateTailPosition()
	{
		LevelUIInteractionDialogue.TailPosition tailPosition = this.tailPosition;
		if (tailPosition != LevelUIInteractionDialogue.TailPosition.Bottom)
		{
			if (tailPosition != LevelUIInteractionDialogue.TailPosition.Right)
			{
				if (tailPosition == LevelUIInteractionDialogue.TailPosition.Left)
				{
					this.leftTail.SetActive(true);
				}
			}
			else
			{
				this.rightTail.SetActive(true);
			}
		}
		else
		{
			this.bottomTail.SetActive(true);
		}
	}

	// Token: 0x040009C3 RID: 2499
	public const float TAIL_WIDTH = 14f;

	// Token: 0x040009C4 RID: 2500
	public const float OFFSET_GLYPH = 27f;

	// Token: 0x040009C5 RID: 2501
	public const float OFFSET_GLYPH_ONLY = 5.3f;

	// Token: 0x040009C6 RID: 2502
	public float glyphOffsetAddition;

	// Token: 0x040009C7 RID: 2503
	public LevelUIInteractionDialogue.TailPosition tailPosition;

	// Token: 0x040009C8 RID: 2504
	[SerializeField]
	public GameObject bottomTail;

	// Token: 0x040009C9 RID: 2505
	[SerializeField]
	public GameObject leftTail;

	// Token: 0x040009CA RID: 2506
	[SerializeField]
	public GameObject rightTail;

	// Token: 0x040009CB RID: 2507
	public static GameObject defaultTarget;

	// Token: 0x02000988 RID: 2440
	public enum TailPosition
	{
		// Token: 0x04004733 RID: 18227
		Right,
		// Token: 0x04004734 RID: 18228
		Left,
		// Token: 0x04004735 RID: 18229
		Bottom
	}
}
