using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200006F RID: 111
[Serializable]
public class VectorPath
{
	// Token: 0x060005A5 RID: 1445 RVA: 0x0006CF08 File Offset: 0x0006B108
	public static Vector3 Lerp(VectorPath path, float t)
	{
		if (path == null || path._points.Count < 1)
		{
			return Vector3.zero;
		}
		if (path._points.Count == 1)
		{
			return path._points[0];
		}
		if (path._points.Count == 2)
		{
			return Vector3.Lerp(path._points[0], path._points[1], t);
		}
		Vector3 vector = default(Vector3);
		int num = 0;
		if (path.Distance < 0f)
		{
			path.Calculate();
		}
		for (int i = 0; i < path.infoNodes.Count - 1; i++)
		{
			num = i;
			if (path.infoNodes[i + 1].distance > t)
			{
				break;
			}
		}
		Vector3 vector2 = path.infoNodes[num];
		Vector3 vector3 = path.infoNodes[num + 1];
		float distance = path.infoNodes[num].distance;
		float distance2 = path.infoNodes[num + 1].distance;
		float num2 = (t - distance) / (distance2 - distance);
		return Vector3.Lerp(path.infoNodes[num], path.infoNodes[num + 1], num2);
	}

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x060005A6 RID: 1446 RVA: 0x00006046 File Offset: 0x00004246
	public List<Vector3> Points
	{
		get
		{
			return this._points;
		}
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0000604E File Offset: 0x0000424E
	// (set) Token: 0x060005A8 RID: 1448 RVA: 0x00006056 File Offset: 0x00004256
	public bool Closed
	{
		get
		{
			return this._closed;
		}
		set
		{
			this._closed = value;
			this.Calculate();
		}
	}

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00006065 File Offset: 0x00004265
	public float Distance
	{
		get
		{
			if (this._distance < 0f)
			{
				this.Calculate();
			}
			return this._distance;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x060005AA RID: 1450 RVA: 0x00006083 File Offset: 0x00004283
	public List<VectorPath.Node> infoNodes
	{
		get
		{
			if (this.__infoNodes == null)
			{
				this.Calculate();
			}
			return this.__infoNodes;
		}
	}

	// Token: 0x060005AB RID: 1451 RVA: 0x0006D074 File Offset: 0x0006B274
	public void Calculate()
	{
		this.__infoNodes = VectorPath.Node.NewList(this._points);
		if (this._closed)
		{
			this.infoNodes.Add(new VectorPath.Node(this._points[0]));
		}
		this._distance = 0f;
		for (int i = 1; i < this.infoNodes.Count; i++)
		{
			this._distance += Vector3.Distance(this.infoNodes[i - 1], this.infoNodes[i]);
		}
		float num = 0f;
		for (int j = 1; j < this.infoNodes.Count; j++)
		{
			num += Vector3.Distance(this.infoNodes[j - 1], this.infoNodes[j]);
			VectorPath.Node value = this.infoNodes[j];
			value.distance = num / this._distance;
			this.infoNodes[j] = value;
		}
	}

	// Token: 0x060005AC RID: 1452 RVA: 0x0000609C File Offset: 0x0000429C
	public Vector3 Lerp(float t)
	{
		return VectorPath.Lerp(this, t);
	}

	// Token: 0x060005AD RID: 1453 RVA: 0x0006D190 File Offset: 0x0006B390
	public Vector2 GetClosestPoint(Vector2 originalPosition, Vector2 positionToCheck, bool moveX, bool moveY)
	{
		Vector2 result = originalPosition;
		float num = float.MaxValue;
		Vector2 vector = Vector2.zero;
		Vector2 vector2 = Vector2.zero;
		Vector2 vector3 = Vector2.zero;
		Vector2 vector4 = Vector2.zero;
		Vector2 vector5 = Vector2.zero;
		for (int i = 0; i < this.Points.Count - 1; i++)
		{
			vector2 = this.Points[i];
			vector3 = this.Points[i + 1];
			vector4 = positionToCheck - vector2;
			vector5 = vector3 - vector2;
			if (moveX)
			{
				float num2 = vector4.x / vector5.x;
				if (num2 < 0f)
				{
					vector = vector2;
				}
				else if (num2 > 1f)
				{
					vector = vector3;
				}
				else
				{
					vector = vector2 + vector5 * num2;
				}
				float num3 = Vector2.Distance(positionToCheck, vector);
				if (num3 <= num)
				{
					num = num3;
					result = vector;
				}
			}
			if (moveY)
			{
				float num2 = vector4.y / vector5.y;
				if (num2 < 0f)
				{
					vector = vector2;
				}
				else if (num2 > 1f)
				{
					vector = vector3;
				}
				else
				{
					vector = vector2 + vector5 * num2;
				}
				float num3 = Vector2.Distance(positionToCheck, vector);
				if (num3 <= num)
				{
					num = num3;
					result = vector;
				}
			}
		}
		return result;
	}

	// Token: 0x060005AE RID: 1454 RVA: 0x0006D300 File Offset: 0x0006B500
	public float GetClosestNormalizedPoint(Vector2 originalPosition, Vector2 positionToCheck, bool moveX, bool moveY)
	{
		Vector2 vector = originalPosition;
		float num = float.MaxValue;
		Vector2 vector2 = Vector2.zero;
		VectorPath.Node node = Vector2.zero;
		VectorPath.Node node2 = Vector2.zero;
		Vector2 vector3 = Vector2.zero;
		Vector2 vector4 = Vector2.zero;
		VectorPath.Node node3 = Vector2.zero;
		VectorPath.Node node4 = Vector2.zero;
		for (int i = 0; i < this.Points.Count - 1; i++)
		{
			node = this.infoNodes[i];
			node2 = this.infoNodes[i + 1];
			vector3 = positionToCheck - node.position;
			vector4 = node2.position - node.position;
			if (moveX)
			{
				float num2 = vector3.x / vector4.x;
				if (num2 < 0f)
				{
					vector2 = node;
				}
				else if (num2 > 1f)
				{
					vector2 = node2;
				}
				else
				{
					vector2 = node + vector4 * num2;
				}
				float num3 = Vector2.Distance(positionToCheck, vector2);
				if (num3 <= num)
				{
					num = num3;
					vector = vector2;
					node3 = node;
					node4 = node2;
				}
			}
			if (moveY)
			{
				float num2 = vector3.y / vector4.y;
				if (num2 < 0f)
				{
					vector2 = node;
				}
				else if (num2 > 1f)
				{
					vector2 = node2;
				}
				else
				{
					vector2 = node + vector4 * num2;
				}
				float num3 = Vector2.Distance(positionToCheck, vector2);
				if (num3 <= num)
				{
					num = num3;
					vector = vector2;
					node3 = node;
					node4 = node2;
				}
			}
		}
		float num4 = Vector2.Distance(node3.position, node4.position);
		float num5 = Vector2.Distance(node3.position, vector);
		return Mathf.Lerp(node3.distance, node4.distance, num5 / num4);
	}

	// Token: 0x060005AF RID: 1455 RVA: 0x000060A5 File Offset: 0x000042A5
	public void DrawGizmos(Vector3 offset)
	{
		this.DrawGizmos(1f, offset);
	}

	// Token: 0x060005B0 RID: 1456 RVA: 0x0006D520 File Offset: 0x0006B720
	public void DrawGizmos(float a, Vector3 offset)
	{
		for (int i = 0; i < this._points.Count; i++)
		{
			Gizmos.color = new Color(0f, 0f, 1f, a);
			Gizmos.DrawWireSphere(this._points[i] + offset, 10f);
			if (i < this._points.Count - 1)
			{
				Gizmos.color = new Color(0f, 0f, 1f, a);
				Vector3 vector = this._points[i] + offset;
				Vector3 vector2 = this._points[i + 1] + offset;
				Gizmos.DrawLine(vector, vector2);
				Vector3 vector3 = Vector3.Lerp(vector, vector2, 0.45f);
				Vector3 vector4 = Vector3.Lerp(vector, vector2, 0.55f);
				Vector3 vector5 = Quaternion.Euler(0f, 0f, 90f) * (vector2 - vector).normalized * 10f;
				Gizmos.color = new Color(0f, 1f, 0f, a);
				Gizmos.DrawLine(vector3 + vector5, vector4);
				Gizmos.DrawLine(vector3 - vector5, vector4);
			}
		}
		if (this.Closed)
		{
			Gizmos.color = new Color(0f, 1f, 0f, a * 0.5f);
			Gizmos.DrawLine(this._points[this._points.Count - 1] + offset, this._points[0] + offset);
		}
	}

	// Token: 0x040004AE RID: 1198
	[SerializeField]
	public List<Vector3> _points = new List<Vector3>
	{
		new Vector2(-100f, 0f),
		new Vector2(100f, 0f)
	};

	// Token: 0x040004AF RID: 1199
	[SerializeField]
	public bool _closed;

	// Token: 0x040004B0 RID: 1200
	public float _distance = -1f;

	// Token: 0x040004B1 RID: 1201
	public List<VectorPath.Node> __infoNodes;

	// Token: 0x020008B4 RID: 2228
	public struct Node
	{
		// Token: 0x06005231 RID: 21041 RVA: 0x0003EE4C File Offset: 0x0003D04C
		public Node(Vector3 v)
		{
			this.x = v.x;
			this.y = v.y;
			this.z = v.z;
			this.distance = 0f;
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06005232 RID: 21042 RVA: 0x0003EE80 File Offset: 0x0003D080
		public Vector3 position
		{
			get
			{
				return new Vector3(this.x, this.y, this.z);
			}
		}

		// Token: 0x06005233 RID: 21043 RVA: 0x001BB730 File Offset: 0x001B9930
		public static List<VectorPath.Node> NewList(List<Vector3> oldList)
		{
			List<VectorPath.Node> list = new List<VectorPath.Node>(oldList.Count);
			for (int i = 0; i < oldList.Count; i++)
			{
				list.Add(new VectorPath.Node(oldList[i]));
			}
			return list;
		}

		// Token: 0x06005234 RID: 21044 RVA: 0x0003EE99 File Offset: 0x0003D099
		public static implicit operator VectorPath.Node(Vector2 v)
		{
			return new VectorPath.Node(v);
		}

		// Token: 0x06005235 RID: 21045 RVA: 0x0003EEA6 File Offset: 0x0003D0A6
		public static implicit operator Vector2(VectorPath.Node t)
		{
			return new Vector2(t.x, t.y);
		}

		// Token: 0x06005236 RID: 21046 RVA: 0x0003EEBB File Offset: 0x0003D0BB
		public static implicit operator VectorPath.Node(Vector3 v)
		{
			return new VectorPath.Node(v);
		}

		// Token: 0x06005237 RID: 21047 RVA: 0x0003EEC3 File Offset: 0x0003D0C3
		public static implicit operator Vector3(VectorPath.Node t)
		{
			return new Vector3(t.x, t.y, t.z);
		}

		// Token: 0x0400429E RID: 17054
		public float x;

		// Token: 0x0400429F RID: 17055
		public float y;

		// Token: 0x040042A0 RID: 17056
		public float z;

		// Token: 0x040042A1 RID: 17057
		public float distance;
	}
}
