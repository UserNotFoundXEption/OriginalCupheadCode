using System;
using UnityEngine;

// Token: 0x020004AD RID: 1197
public class MapLadder : AbstractMonoBehaviour
{
	// Token: 0x060031B0 RID: 12720 RVA: 0x000EACD8 File Offset: 0x000E8ED8
	public override void OnDrawGizmos()
	{
		float num = 0.1f;
		base.OnDrawGizmos();
		Vector3 position = base.baseTransform.position;
		Vector3 vector = position + new Vector3(0f, this.height, 0f);
		this.DrawPointGizmos(position, this.bottom);
		this.DrawPointGizmos(vector, this.top);
		Gizmos.color = Color.black;
		Gizmos.DrawLine(position, vector);
		Gizmos.DrawLine(new Vector2(position.x - num, position.y), new Vector2(position.x + num, position.y));
	}

	// Token: 0x060031B1 RID: 12721 RVA: 0x000EAD88 File Offset: 0x000E8F88
	public void DrawPointGizmos(Vector2 point, MapLadder.PointProperties properties)
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(point + properties.dialogueOffset, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(point + properties.dialogueOffset, 0.07f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(point + properties.interactionPoint, 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(point + properties.interactionPoint, 0.07f);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(point + properties.interactionPoint, properties.interactionDistance);
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(point + properties.interactionPoint, properties.interactionDistance + 0.02f);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(point + properties.exit, Vector3.one * 0.05f);
		Gizmos.color = Color.white;
		Gizmos.DrawWireCube(point + properties.exit, Vector3.one * 0.07f);
	}

	// Token: 0x060031B2 RID: 12722 RVA: 0x0002950C File Offset: 0x0002770C
	public void SetLayer(SpriteRenderer renderer)
	{
		if (renderer == null)
		{
			return;
		}
		renderer.sortingLayerName = "Background";
		renderer.sortingOrder = 100;
	}

	// Token: 0x040028DE RID: 10462
	public static readonly Vector2 DIALOGUE_OFFSET = new Vector2(0f, 0.5f);

	// Token: 0x040028DF RID: 10463
	public static readonly Vector2 INTERACTION_POINT_TOP = new Vector2(0f, 0.1f);

	// Token: 0x040028E0 RID: 10464
	public static readonly Vector2 INTERACTION_POINT_BOTTOM = new Vector2(0f, -0.1f);

	// Token: 0x040028E1 RID: 10465
	public const float INTERACTION_DISTANCE = 0.2f;

	// Token: 0x040028E2 RID: 10466
	public static readonly AbstractUIInteractionDialogue.Properties DIALOGUE_ENTER = new AbstractUIInteractionDialogue.Properties("CLIMB");

	// Token: 0x040028E3 RID: 10467
	public static readonly AbstractUIInteractionDialogue.Properties DIALOGUE_EXIT = new AbstractUIInteractionDialogue.Properties("EXIT");

	// Token: 0x040028E4 RID: 10468
	public static readonly Vector2 EXIT_TOP = new Vector2(0f, 0.2f);

	// Token: 0x040028E5 RID: 10469
	public static readonly Vector2 EXIT_BOTTOM = new Vector2(0f, -0.2f);

	// Token: 0x040028E6 RID: 10470
	public float height = 1f;

	// Token: 0x040028E7 RID: 10471
	[SerializeField]
	public MapLadder.PointProperties top = MapLadder.PointProperties.TopDefault();

	// Token: 0x040028E8 RID: 10472
	[SerializeField]
	public MapLadder.PointProperties bottom = MapLadder.PointProperties.BottomDefault();

	// Token: 0x0200110C RID: 4364
	public enum Location
	{
		// Token: 0x04007887 RID: 30855
		Top,
		// Token: 0x04007888 RID: 30856
		Bottom
	}

	// Token: 0x0200110D RID: 4365
	[Serializable]
	public class PointProperties
	{
		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x06007C3C RID: 31804 RVA: 0x000539F2 File Offset: 0x00051BF2
		// (set) Token: 0x06007C3D RID: 31805 RVA: 0x000539FA File Offset: 0x00051BFA
		public MapLadder.Location location { get; set; }

		// Token: 0x06007C3E RID: 31806 RVA: 0x0028971C File Offset: 0x0028791C
		public static MapLadder.PointProperties TopDefault()
		{
			return new MapLadder.PointProperties
			{
				interactionPoint = MapLadder.INTERACTION_POINT_TOP,
				exit = MapLadder.EXIT_TOP,
				location = MapLadder.Location.Top
			};
		}

		// Token: 0x06007C3F RID: 31807 RVA: 0x00289750 File Offset: 0x00287950
		public static MapLadder.PointProperties BottomDefault()
		{
			return new MapLadder.PointProperties
			{
				interactionPoint = MapLadder.INTERACTION_POINT_BOTTOM,
				exit = MapLadder.EXIT_BOTTOM,
				location = MapLadder.Location.Bottom
			};
		}

		// Token: 0x04007889 RID: 30857
		public Vector2 interactionPoint = Vector2.zero;

		// Token: 0x0400788A RID: 30858
		public float interactionDistance = 0.2f;

		// Token: 0x0400788B RID: 30859
		public Vector2 dialogueOffset = MapLadder.DIALOGUE_OFFSET;

		// Token: 0x0400788C RID: 30860
		public Vector2 exit = Vector2.zero;
	}
}
