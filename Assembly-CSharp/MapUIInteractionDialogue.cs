using System;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
public class MapUIInteractionDialogue : AbstractUIInteractionDialogue
{
	// Token: 0x06003346 RID: 13126 RVA: 0x000F3CE8 File Offset: 0x000F1EE8
	public static MapUIInteractionDialogue Create(AbstractUIInteractionDialogue.Properties properties, PlayerInput player, Vector2 offset)
	{
		MapUIInteractionDialogue mapUIInteractionDialogue = Object.Instantiate<MapUIInteractionDialogue>(Map.Current.MapResources.mapUIInteractionDialogue);
		properties.text = string.Empty;
		mapUIInteractionDialogue.Init(properties, player, offset);
		return mapUIInteractionDialogue;
	}

	// Token: 0x170003BD RID: 957
	// (get) Token: 0x06003347 RID: 13127 RVA: 0x0002A779 File Offset: 0x00028979
	public override float PreferredWidth
	{
		get
		{
			return this.tmpText.preferredWidth + this.glyph.preferredWidth + 10f;
		}
	}

	// Token: 0x06003348 RID: 13128 RVA: 0x0002A798 File Offset: 0x00028998
	public override void Awake()
	{
		base.Awake();
		base.transform.SetParent(MapUI.Current.sceneCanvas.transform);
		base.transform.ResetLocalTransforms();
	}

	// Token: 0x06003349 RID: 13129 RVA: 0x0002A7C5 File Offset: 0x000289C5
	public void Update()
	{
		this.UpdatePos();
	}

	// Token: 0x0600334A RID: 13130 RVA: 0x000F3D20 File Offset: 0x000F1F20
	public void UpdatePos()
	{
		if (this.target == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Vector2 vector = this.target.position + this.dialogueOffset;
		base.transform.position = vector;
	}

	// Token: 0x04002A63 RID: 10851
	public const float OFFSET_GLYPH = 10f;
}
