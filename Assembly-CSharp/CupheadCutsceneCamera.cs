using System;
using UnityEngine;

// Token: 0x0200009D RID: 157
public class CupheadCutsceneCamera : AbstractCupheadGameCamera
{
	// Token: 0x17000154 RID: 340
	// (get) Token: 0x06000789 RID: 1929 RVA: 0x0000770F File Offset: 0x0000590F
	// (set) Token: 0x0600078A RID: 1930 RVA: 0x00007716 File Offset: 0x00005916
	public static CupheadCutsceneCamera Current { get; set; }

	// Token: 0x17000155 RID: 341
	// (get) Token: 0x0600078B RID: 1931 RVA: 0x0000771E File Offset: 0x0000591E
	public override float OrthographicSize
	{
		get
		{
			return 360f;
		}
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00007725 File Offset: 0x00005925
	public override void Awake()
	{
		base.Awake();
		CupheadCutsceneCamera.Current = this;
	}

	// Token: 0x0600078D RID: 1933 RVA: 0x00007733 File Offset: 0x00005933
	public void OnDestroy()
	{
		if (CupheadCutsceneCamera.Current == this)
		{
			CupheadCutsceneCamera.Current = null;
		}
	}

	// Token: 0x0600078E RID: 1934 RVA: 0x0000774B File Offset: 0x0000594B
	public override void LateUpdate()
	{
		base.LateUpdate();
		base.Move();
	}

	// Token: 0x0600078F RID: 1935 RVA: 0x00007759 File Offset: 0x00005959
	public void SetPosition(Vector3 newPos)
	{
		this._position = newPos;
	}

	// Token: 0x06000790 RID: 1936 RVA: 0x00072DEC File Offset: 0x00070FEC
	public void Init()
	{
		base.enabled = true;
		Texture2D texture = this.CreateBorderTexture();
		if (!this.noBars)
		{
			Transform transform = new GameObject("Border").transform;
			this.CreateBorderRenderer(texture, transform, "Left");
			this.CreateBorderRenderer(texture, transform, "Right");
			this.CreateBorderRenderer(texture, transform, "Top");
			this.CreateBorderRenderer(texture, transform, "Bottom");
		}
		if (!this.noShake)
		{
			if (this.minimalShake)
			{
				base.StartSmoothShake(3f, 1.5f, 6);
			}
			else
			{
				base.StartSmoothShake(4f, 0.75f, 8);
			}
		}
	}

	// Token: 0x06000791 RID: 1937 RVA: 0x00072E98 File Offset: 0x00071098
	public SpriteRenderer CreateBorderRenderer(Texture2D texture, Transform parent, string name)
	{
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		Vector2 zero3 = Vector2.zero;
		string text = name.ToLower();
		if (text != null)
		{
			if (!(text == "left"))
			{
				if (!(text == "right"))
				{
					if (!(text == "top"))
					{
						if (text == "bottom")
						{
							zero..ctor(3280f, 1000f);
							zero2..ctor(0f, -360f);
							zero3..ctor(0.5f, 1f);
						}
					}
					else
					{
						zero..ctor(3280f, 1000f);
						zero2..ctor(0f, 360f);
						zero3..ctor(0.5f, 0f);
					}
				}
				else
				{
					zero..ctor(1000f, 2720f);
					zero2..ctor(640f, 0f);
					zero3..ctor(0f, 0.5f);
				}
			}
			else
			{
				zero..ctor(1000f, 2720f);
				zero2..ctor(-640f, 0f);
				zero3..ctor(1f, 0.5f);
			}
		}
		SpriteRenderer spriteRenderer = new GameObject(name).AddComponent<SpriteRenderer>();
		spriteRenderer.transform.SetParent(parent);
		Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), zero3, 1f);
		spriteRenderer.sprite = sprite;
		spriteRenderer.transform.localScale = zero;
		spriteRenderer.transform.position = zero2;
		spriteRenderer.sortingLayerName = SpriteLayer.Foreground.ToString();
		spriteRenderer.sortingOrder = 10000;
		return spriteRenderer;
	}

	// Token: 0x06000792 RID: 1938 RVA: 0x00073084 File Offset: 0x00071284
	public Texture2D CreateBorderTexture()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.filterMode = 0;
		texture2D.SetPixel(0, 0, Color.black);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x040005B5 RID: 1461
	public const string EDITOR_PATH = "Assets/_CUPHEAD/Prefabs/Camera/CutsceneCamera.prefab";

	// Token: 0x040005B6 RID: 1462
	public const float BOUND_COLLIDER_SIZE = 400f;

	// Token: 0x040005B7 RID: 1463
	public const float BORDER_THICKNESS = 1000f;

	// Token: 0x040005B8 RID: 1464
	public const float LEFT = -640f;

	// Token: 0x040005B9 RID: 1465
	public const float RIGHT = 640f;

	// Token: 0x040005BA RID: 1466
	public const float BOTTOM = -360f;

	// Token: 0x040005BB RID: 1467
	public const float TOP = 360f;

	// Token: 0x040005BC RID: 1468
	public bool noShake;

	// Token: 0x040005BD RID: 1469
	public bool minimalShake;

	// Token: 0x040005BE RID: 1470
	public bool noBars;

	// Token: 0x020008F5 RID: 2293
	public enum Mode
	{
		// Token: 0x04004422 RID: 17442
		Lerp,
		// Token: 0x04004423 RID: 17443
		TrapBox,
		// Token: 0x04004424 RID: 17444
		Relative,
		// Token: 0x04004425 RID: 17445
		Platforming,
		// Token: 0x04004426 RID: 17446
		Static = 10000
	}
}
