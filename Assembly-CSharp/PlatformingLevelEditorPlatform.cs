using System;
using UnityEngine;

// Token: 0x02000459 RID: 1113
public class PlatformingLevelEditorPlatform : AbstractMonoBehaviour
{
	// Token: 0x06002F95 RID: 12181 RVA: 0x000E1F50 File Offset: 0x000E0150
	public override void Awake()
	{
		base.Awake();
		PlatformingLevelEditorPlatform.Type type = this._type;
		if (type != PlatformingLevelEditorPlatform.Type.Platform)
		{
			if (type == PlatformingLevelEditorPlatform.Type.Solid)
			{
				GameObject gameObject = new GameObject("ground");
				GameObject gameObject2 = new GameObject("walls");
				GameObject gameObject3 = new GameObject("ceiling");
				gameObject.layer = 20;
				gameObject.tag = "Ground";
				gameObject2.layer = 18;
				gameObject2.tag = "Wall";
				gameObject3.layer = 19;
				gameObject3.tag = "Ceiling";
				gameObject.transform.SetParent(base.transform);
				gameObject2.transform.SetParent(base.transform);
				gameObject3.transform.SetParent(base.transform);
				gameObject.transform.ResetLocalTransforms();
				gameObject2.transform.ResetLocalTransforms();
				gameObject3.transform.ResetLocalTransforms();
				this._topCollider = gameObject.AddComponent<BoxCollider2D>();
				this._middleCollider = gameObject2.AddComponent<BoxCollider2D>();
				this._bottomCollider = gameObject3.AddComponent<BoxCollider2D>();
				this._topCollider.isTrigger = true;
				this._middleCollider.isTrigger = true;
				this._bottomCollider.isTrigger = true;
				this._topCollider.size = new Vector2(this._size.x, 20f);
				this._middleCollider.size = this._size - new Vector2(0f, 40f);
				this._bottomCollider.size = new Vector2(this._size.x, 20f);
				this._topCollider.offset = new Vector2(0f, this._size.y / 2f - this._topCollider.size.y / 2f) + this._offset;
				this._middleCollider.offset = Vector2.zero + this._offset;
				this._bottomCollider.offset = new Vector2(0f, -(this._size.y / 2f - this._bottomCollider.size.y / 2f)) + this._offset;
			}
		}
		else
		{
			this._collider = base.gameObject.AddComponent<BoxCollider2D>();
			this._collider.size = this._size;
			this._collider.offset = this._offset;
			this._collider.isTrigger = true;
			this._platform = base.gameObject.AddComponent<LevelPlatform>();
			this._platform.canFallThrough = this._canFallThrough;
		}
	}

	// Token: 0x06002F96 RID: 12182 RVA: 0x00027A9E File Offset: 0x00025C9E
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.5f);
	}

	// Token: 0x06002F97 RID: 12183 RVA: 0x00027AB1 File Offset: 0x00025CB1
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002F98 RID: 12184 RVA: 0x000E21F8 File Offset: 0x000E03F8
	public void DrawGizmos(float a)
	{
		Matrix4x4 matrix = Gizmos.matrix;
		Gizmos.matrix = base.transform.localToWorldMatrix;
		Vector2 vector = Vector2.zero + this._offset;
		Gizmos.color = new Color(0f, 0f, 0f, 0.4f * a);
		Gizmos.DrawCube(vector, this._size);
		Gizmos.color = Color.cyan * new Color(1f, 1f, 1f, a);
		PlatformingLevelEditorPlatform.Type type = this._type;
		if (type != PlatformingLevelEditorPlatform.Type.Platform)
		{
			if (type == PlatformingLevelEditorPlatform.Type.Solid)
			{
				Gizmos.DrawWireCube(vector, this._size);
			}
		}
		else
		{
			float num = vector.y + this._size.y / 2f;
			float num2 = vector.y - this._size.y / 2f;
			float num3 = num - 10f;
			float num4 = vector.x - this._size.x / 2f;
			float num5 = vector.x + this._size.x / 2f;
			Gizmos.DrawLine(new Vector2(num4, num), new Vector2(num5, num));
			if (!this._canFallThrough)
			{
				Gizmos.DrawLine(new Vector2(num4, num3), new Vector2(num5, num3));
			}
			else
			{
				Gizmos.DrawLine(new Vector2(vector.x, num2 + 50f), new Vector2(vector.x, num2));
				Gizmos.DrawLine(new Vector2(vector.x - 20f, num2 + 20f), new Vector2(vector.x, num2));
				Gizmos.DrawLine(new Vector2(vector.x + 20f, num2 + 20f), new Vector2(vector.x, num2));
			}
		}
		Gizmos.matrix = matrix;
	}

	// Token: 0x0400276E RID: 10094
	public const int THICKNESS = 20;

	// Token: 0x0400276F RID: 10095
	[SerializeField]
	public PlatformingLevelEditorPlatform.Type _type;

	// Token: 0x04002770 RID: 10096
	[SerializeField]
	public bool _canFallThrough;

	// Token: 0x04002771 RID: 10097
	[SerializeField]
	public Vector2 _size = new Vector2(100f, 10f);

	// Token: 0x04002772 RID: 10098
	[SerializeField]
	public Vector2 _offset = new Vector2(0f, 0f);

	// Token: 0x04002773 RID: 10099
	public LevelPlatform _platform;

	// Token: 0x04002774 RID: 10100
	public BoxCollider2D _collider;

	// Token: 0x04002775 RID: 10101
	public BoxCollider2D _topCollider;

	// Token: 0x04002776 RID: 10102
	public BoxCollider2D _middleCollider;

	// Token: 0x04002777 RID: 10103
	public BoxCollider2D _bottomCollider;

	// Token: 0x020010D4 RID: 4308
	public enum Type
	{
		// Token: 0x0400774E RID: 30542
		Platform,
		// Token: 0x0400774F RID: 30543
		Solid
	}
}
