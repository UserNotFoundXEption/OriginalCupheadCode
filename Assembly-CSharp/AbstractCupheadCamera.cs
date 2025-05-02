using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200009A RID: 154
[RequireComponent(typeof(Camera))]
public abstract class AbstractCupheadCamera : AbstractMonoBehaviour
{
	// Token: 0x0600074F RID: 1871 RVA: 0x00007336 File Offset: 0x00005536
	public AbstractCupheadCamera()
	{
	}

	// Token: 0x1700014C RID: 332
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x0000733E File Offset: 0x0000553E
	public Camera camera
	{
		get
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			return this._camera;
		}
	}

	// Token: 0x06000751 RID: 1873 RVA: 0x00007363 File Offset: 0x00005563
	public bool ContainsPoint(Vector2 point)
	{
		return this.ContainsPoint(point, Vector2.zero);
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x00072834 File Offset: 0x00070A34
	public bool ContainsPoint(Vector2 point, Vector2 padding)
	{
		return this.CalculateContainsBounds(padding).Contains(point);
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x00072854 File Offset: 0x00070A54
	public Rect CalculateContainsBounds(Vector2 padding)
	{
		float orthographicSize = this.camera.orthographicSize;
		Vector3 position = base.transform.position;
		float width = orthographicSize * 1.77777779f * 2f + padding.x * 2f;
		float height = orthographicSize * 2f + padding.y * 2f;
		return RectUtils.NewFromCenter(position.x, position.y, width, height);
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x00007371 File Offset: 0x00005571
	public override void Awake()
	{
		base.Awake();
		this.camera.clearFlags = 4;
	}

	// Token: 0x1700014D RID: 333
	// (get) Token: 0x06000755 RID: 1877 RVA: 0x000728C8 File Offset: 0x00070AC8
	public Rect Bounds
	{
		get
		{
			float width = this.camera.orthographicSize * 1.77777779f * 2f;
			float height = this.camera.orthographicSize * 2f;
			return RectUtils.NewFromCenter(base.transform.position.x, base.transform.position.y, width, height);
		}
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x00007385 File Offset: 0x00005585
	public virtual void LateUpdate()
	{
		this.UpdateRect();
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00072934 File Offset: 0x00070B34
	public void UpdateRect()
	{
		float num = (float)Screen.width / (float)Screen.height;
		float num2 = 1f - 0.1f * SettingsData.Data.overscan;
		Rect rect;
		if (num > 1.77777779f)
		{
			rect = RectUtils.NewFromCenter(0.5f, 0.5f, num2 * 1.77777779f / num, num2 * 1f);
		}
		else
		{
			rect = RectUtils.NewFromCenter(0.5f, 0.5f, num2 * 1f, num2 * num / 1.77777779f);
		}
		if (this.camera.rect != rect)
		{
			this.camera.rect = rect;
			CanvasScaler[] array = Object.FindObjectsOfType<CanvasScaler>();
			foreach (CanvasScaler canvasScaler in array)
			{
				canvasScaler.referenceResolution = new Vector2(1280f / rect.height, 720f / rect.height);
			}
		}
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x0000738D File Offset: 0x0000558D
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.1f);
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x000073A0 File Offset: 0x000055A0
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00072A28 File Offset: 0x00070C28
	public void DrawGizmos(float a)
	{
		Gizmos.color = Color.white * new Color(1f, 1f, 1f, a);
		Gizmos.DrawWireCube(this.camera.transform.position, new Vector3(this.camera.orthographicSize * this.camera.aspect * 2f, this.camera.orthographicSize * 2f, 0f));
		Gizmos.DrawWireSphere(this.camera.transform.position, 50f);
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Gizmos.DrawWireSphere(this.camera.transform.position, 10f);
	}

	// Token: 0x0400059C RID: 1436
	public Camera _camera;
}
