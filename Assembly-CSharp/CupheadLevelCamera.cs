using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200009E RID: 158
public class CupheadLevelCamera : AbstractCupheadGameCamera
{
	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000794 RID: 1940 RVA: 0x00007780 File Offset: 0x00005980
	// (set) Token: 0x06000795 RID: 1941 RVA: 0x00007787 File Offset: 0x00005987
	public static CupheadLevelCamera Current { get; set; }

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x0000778F File Offset: 0x0000598F
	// (set) Token: 0x06000797 RID: 1943 RVA: 0x00007797 File Offset: 0x00005997
	public bool cameraLocked { get; set; }

	// Token: 0x17000158 RID: 344
	// (get) Token: 0x06000798 RID: 1944 RVA: 0x000077A0 File Offset: 0x000059A0
	// (set) Token: 0x06000799 RID: 1945 RVA: 0x000077A8 File Offset: 0x000059A8
	public bool cameraOffset { get; set; }

	// Token: 0x17000159 RID: 345
	// (get) Token: 0x0600079A RID: 1946 RVA: 0x000077B1 File Offset: 0x000059B1
	// (set) Token: 0x0600079B RID: 1947 RVA: 0x000077B9 File Offset: 0x000059B9
	public bool autoScrolling { get; set; }

	// Token: 0x1700015A RID: 346
	// (get) Token: 0x0600079C RID: 1948 RVA: 0x000077C2 File Offset: 0x000059C2
	// (set) Token: 0x0600079D RID: 1949 RVA: 0x000077CA File Offset: 0x000059CA
	public float autoScrollSpeedMultiplier { get; set; }

	// Token: 0x1700015B RID: 347
	// (get) Token: 0x0600079E RID: 1950 RVA: 0x000077D3 File Offset: 0x000059D3
	// (set) Token: 0x0600079F RID: 1951 RVA: 0x000077DB File Offset: 0x000059DB
	public float Left { get; set; }

	// Token: 0x1700015C RID: 348
	// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000077E4 File Offset: 0x000059E4
	// (set) Token: 0x060007A1 RID: 1953 RVA: 0x000077EC File Offset: 0x000059EC
	public float Right { get; set; }

	// Token: 0x1700015D RID: 349
	// (get) Token: 0x060007A2 RID: 1954 RVA: 0x000077F5 File Offset: 0x000059F5
	// (set) Token: 0x060007A3 RID: 1955 RVA: 0x000077FD File Offset: 0x000059FD
	public float Bottom { get; set; }

	// Token: 0x1700015E RID: 350
	// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00007806 File Offset: 0x00005A06
	// (set) Token: 0x060007A5 RID: 1957 RVA: 0x0000780E File Offset: 0x00005A0E
	public new float Top { get; set; }

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00007817 File Offset: 0x00005A17
	public override float OrthographicSize
	{
		get
		{
			return 360f;
		}
	}

	// Token: 0x060007A7 RID: 1959 RVA: 0x0000781E File Offset: 0x00005A1E
	public override void Awake()
	{
		base.Awake();
		CupheadLevelCamera.Current = this;
	}

	// Token: 0x060007A8 RID: 1960 RVA: 0x0000782C File Offset: 0x00005A2C
	public void Start()
	{
		this.autoScrolling = false;
		this.cameraLocked = false;
		this.cameraOffset = false;
		this.autoScrollSpeedMultiplier = 1f;
		this._position = base.transform.position;
	}

	// Token: 0x060007A9 RID: 1961 RVA: 0x0000785F File Offset: 0x00005A5F
	public void OnDestroy()
	{
		if (CupheadLevelCamera.Current == this)
		{
			CupheadLevelCamera.Current = null;
		}
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x000730B4 File Offset: 0x000712B4
	public void Update()
	{
		if (PlayerManager.Count <= 0)
		{
			return;
		}
		this.UpdateBounds();
		Vector3 position = this._position;
		CupheadLevelCamera.Mode mode = this.mode;
		switch (mode)
		{
		case CupheadLevelCamera.Mode.Lerp:
			break;
		case CupheadLevelCamera.Mode.TrapBox:
			this.UpdateModeTrapBox();
			goto IL_A4;
		case CupheadLevelCamera.Mode.Relative:
			this.UpdateModeRelative();
			goto IL_A4;
		case CupheadLevelCamera.Mode.Platforming:
			this.UpdatePlatforming();
			goto IL_A4;
		case CupheadLevelCamera.Mode.Path:
			this.UpdatePath();
			goto IL_A4;
		case CupheadLevelCamera.Mode.RelativeRook:
			this.UpdateModeRelativeRook();
			goto IL_A4;
		case CupheadLevelCamera.Mode.RelativeRumRunners:
			this.UpdateModeRelativeRumRunners();
			goto IL_A4;
		default:
			if (mode == CupheadLevelCamera.Mode.Static)
			{
				goto IL_A4;
			}
			break;
		}
		this.UpdateModeLerp();
		IL_A4:
		Vector3 position2 = this._position;
		if (base.Width * 2f > (float)this.bounds.Width)
		{
			position2.x = Mathf.Lerp(position.x, 0f, CupheadTime.Delta * 10f);
		}
		if (base.Height * 2f > (float)this.bounds.Height)
		{
			position2.y = Mathf.Lerp(position.y, 0f, CupheadTime.Delta * 10f);
		}
		this._position = position2;
		base.Move();
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x00007877 File Offset: 0x00005A77
	public override void LateUpdate()
	{
		base.LateUpdate();
		base.Move();
	}

	// Token: 0x060007AC RID: 1964 RVA: 0x00073204 File Offset: 0x00071404
	public void UpdateBounds()
	{
		this.Left = ((!this.bounds.leftEnabled) ? float.MinValue : ((float)(-(float)this.bounds.left) + base.Width));
		this.Right = ((!this.bounds.rightEnabled) ? float.MaxValue : ((float)this.bounds.right - base.Width));
		this.Bottom = ((!this.bounds.bottomEnabled) ? float.MinValue : ((float)(-(float)this.bounds.bottom) + base.Height));
		this.Top = ((!this.bounds.topEnabled) ? float.MaxValue : ((float)this.bounds.top - base.Height));
	}

	// Token: 0x060007AD RID: 1965 RVA: 0x00007885 File Offset: 0x00005A85
	public void DisableRightCollider()
	{
		this.rightCollider.gameObject.SetActive(false);
	}

	// Token: 0x060007AE RID: 1966 RVA: 0x00007898 File Offset: 0x00005A98
	public void MoveRightCollider()
	{
		this.rightCollider.transform.AddPosition(100f, 0f, 0f);
	}

	// Token: 0x060007AF RID: 1967 RVA: 0x000732E0 File Offset: 0x000714E0
	public void Init(Level.Camera properties)
	{
		base.enabled = true;
		this.mode = properties.mode;
		base.zoom = properties.zoom;
		this.moveX = properties.moveX;
		this.moveY = properties.moveY;
		this.stabilizeY = properties.stabilizeY;
		this.stabilizePaddingTop = properties.stabilizePaddingTop;
		this.stabilizePaddingBottom = properties.stabilizePaddingBottom;
		this.bounds = properties.bounds;
		this.path = properties.path;
		this.pathMovesOnlyForward = properties.pathMovesOnlyForward;
		if (properties.mode == CupheadLevelCamera.Mode.Path)
		{
			base.transform.position = this.path.Lerp(0f);
		}
		this.UpdateBounds();
		if (properties.colliders)
		{
			this.collidersRoot = new GameObject("Colliders").transform;
			this.collidersRoot.parent = base.transform;
			this.collidersRoot.ResetLocalTransforms();
			this.SetupCollider(Level.Bounds.Side.Left);
			this.rightCollider = this.SetupCollider(Level.Bounds.Side.Right);
		}
	}

	// Token: 0x060007B0 RID: 1968 RVA: 0x000733EC File Offset: 0x000715EC
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
							zero..ctor((float)this.bounds.Width + 2000f, 1000f);
							zero2..ctor(this.bounds.Center.x, (float)(-(float)this.bounds.bottom));
							zero3..ctor(0.5f, 1f);
						}
					}
					else
					{
						zero..ctor((float)this.bounds.Width + 2000f, 1000f);
						zero2..ctor(this.bounds.Center.x, (float)this.bounds.top);
						zero3..ctor(0.5f, 0f);
					}
				}
				else
				{
					zero..ctor(1000f, (float)this.bounds.Height + 2000f);
					zero2..ctor((float)this.bounds.right, this.bounds.Center.y);
					zero3..ctor(0f, 0.5f);
				}
			}
			else
			{
				zero..ctor(1000f, (float)this.bounds.Height + 2000f);
				zero2..ctor((float)(-(float)this.bounds.left), this.bounds.Center.y);
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

	// Token: 0x060007B1 RID: 1969 RVA: 0x00073664 File Offset: 0x00071864
	public Texture2D CreateBorderTexture()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.filterMode = 0;
		texture2D.SetPixel(0, 0, Color.black);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x060007B2 RID: 1970 RVA: 0x00073694 File Offset: 0x00071894
	public Transform SetupCollider(Level.Bounds.Side side)
	{
		string text = string.Empty;
		string tag = string.Empty;
		int layer = 0;
		int num = 0;
		Vector2 zero = Vector2.zero;
		Vector2 vector;
		vector..ctor(base.Bounds.xMin, base.Bounds.yMax);
		Vector2 vector2;
		vector2..ctor(base.Bounds.xMax, base.Bounds.yMin);
		float x = vector.x;
		float x2 = vector2.x;
		float y = vector2.y;
		float y2 = vector.y;
		switch (side)
		{
		case Level.Bounds.Side.Left:
			text = "Level_Wall_Left";
			tag = "Wall";
			layer = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString());
			num = 90;
			zero..ctor(x - 200f, 0f);
			break;
		case Level.Bounds.Side.Right:
			text = "Level_Wall_Right";
			tag = "Wall";
			layer = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString());
			num = -90;
			zero..ctor(x2 + 200f, 0f);
			break;
		case Level.Bounds.Side.Top:
			text = "Level_Ceiling";
			tag = "Ceiling";
			layer = LayerMask.NameToLayer(Layers.Bounds_Ceiling.ToString());
			zero..ctor(0f, y + 200f);
			break;
		case Level.Bounds.Side.Bottom:
			text = "Level_Ground";
			tag = "Ground";
			layer = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString());
			num = 180;
			zero..ctor(0f, y2 - 200f);
			break;
		}
		GameObject gameObject = new GameObject(text);
		gameObject.tag = tag;
		gameObject.layer = layer;
		gameObject.transform.ResetLocalTransforms();
		gameObject.transform.SetPosition(new float?(zero.x), new float?(zero.y), null);
		gameObject.transform.SetEulerAngles(null, null, new float?((float)num));
		gameObject.transform.parent = this.collidersRoot;
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		boxCollider2D.size = new Vector2(2000f, 400f);
		return gameObject.transform;
	}

	// Token: 0x060007B3 RID: 1971 RVA: 0x000738F8 File Offset: 0x00071AF8
	public void UpdateModeLerp()
	{
		Vector3 position = this._position;
		Vector3 vector = PlayerManager.Center;
		if (this.moveX)
		{
			position.x = vector.x;
		}
		if (this.moveY)
		{
			position.y = vector.y;
		}
		position.x = Mathf.Clamp(position.x, this.Left, this.Right);
		position.y = Mathf.Clamp(position.y, this.Bottom, this.Top);
		this._position = Vector3.Lerp(this._position, position, CupheadTime.Delta * this.LERP_SPEED);
	}

	// Token: 0x060007B4 RID: 1972 RVA: 0x000739AC File Offset: 0x00071BAC
	public void UpdateModeTrapBox()
	{
		Vector3 position = this._position;
		Vector3 vector = PlayerManager.CameraCenter;
		if (this.moveX)
		{
			position.x = vector.x;
		}
		if (this.moveY)
		{
			position.y = vector.y;
		}
		position.x = Mathf.Clamp(position.x, this.Left, this.Right);
		position.y = Mathf.Clamp(position.y, this.Bottom, this.Top);
		this._position = Vector3.Lerp(this._position, position, Time.deltaTime * this.LERP_SPEED);
	}

	// Token: 0x060007B5 RID: 1973 RVA: 0x00073A5C File Offset: 0x00071C5C
	public void UpdateModeRelative()
	{
		Vector2 vector = this._position;
		Vector2 vector2;
		vector2..ctor(0f, 0f);
		vector2.x = MathUtils.GetPercentage((float)Level.Current.Left, (float)Level.Current.Right, PlayerManager.Center.x);
		vector2.y = MathUtils.GetPercentage((float)Level.Current.Ground, (float)Level.Current.Ceiling, PlayerManager.Center.y);
		if (this.moveX)
		{
			vector.x = Mathf.Lerp(this.Left, this.Right, vector2.x);
		}
		if (this.moveY)
		{
			vector.y = Mathf.Lerp(this.Bottom, this.Top, vector2.y);
		}
		vector.x = Mathf.Clamp(vector.x, this.Left, this.Right);
		vector.y = Mathf.Clamp(vector.y, this.Bottom, this.Top);
		this._position = Vector3.Lerp(this._position, vector, CupheadTime.Delta * 5f);
	}

	// Token: 0x060007B6 RID: 1974 RVA: 0x00073BA0 File Offset: 0x00071DA0
	public void UpdateModeRelativeRook()
	{
		Vector2 vector = this._position;
		Vector2 vector2;
		vector2..ctor(0f, 0f);
		vector2.x = MathUtils.GetPercentage((float)Level.Current.Left, (float)Level.Current.Right, PlayerManager.TopPlayerPosition.x);
		vector2.y = PlayerManager.TopPlayerPosition.y;
		if (this.moveX)
		{
			vector.x = Mathf.Lerp(this.Left, this.Right, vector2.x);
		}
		vector.y = Mathf.Lerp(this.Bottom, this.Top, Mathf.InverseLerp(200f, 400f, vector2.y));
		vector.x = Mathf.Clamp(vector.x, this.Left, this.Right);
		vector.y = Mathf.Clamp(vector.y, this.Bottom, this.Top);
		this._position = new Vector3(Mathf.Lerp(this._position.x, vector.x, CupheadTime.Delta * 5f), Mathf.Lerp(this._position.y, vector.y, CupheadTime.Delta * 2.5f));
	}

	// Token: 0x060007B7 RID: 1975 RVA: 0x00073CFC File Offset: 0x00071EFC
	public void UpdateModeRelativeRumRunners()
	{
		Vector2 vector = this._position;
		Vector2 vector2;
		vector2..ctor(0f, 0f);
		vector2.x = MathUtils.GetPercentage((float)Level.Current.Left, (float)Level.Current.Right, PlayerManager.TopPlayerPosition.x);
		vector2.y = PlayerManager.TopPlayerPosition.y;
		if (this.moveX)
		{
			vector.x = Mathf.Lerp(this.Left, this.Right, vector2.x);
		}
		vector.y = ((vector2.y >= 200f) ? this.Top : this.Bottom);
		vector.x = Mathf.Clamp(vector.x, this.Left, this.Right);
		vector.y = Mathf.Clamp(vector.y, this.Bottom, this.Top);
		this._position = new Vector3(Mathf.Lerp(this._position.x, vector.x, CupheadTime.Delta * 5f), Mathf.Lerp(this._position.y, vector.y, CupheadTime.Delta * 2.5f));
	}

	// Token: 0x060007B8 RID: 1976 RVA: 0x00073E54 File Offset: 0x00072054
	public void UpdatePlatforming()
	{
		Vector3 position = this._position;
		Vector3 vector = PlayerManager.Center;
		if (this.moveX && position.x < vector.x)
		{
			position.x = vector.x;
		}
		if (this.moveY)
		{
			position.y = vector.y;
		}
		position.x = Mathf.Clamp(position.x, this.Left, this.Right);
		position.y = Mathf.Clamp(position.y, this.Bottom, this.Top);
		this._position = Vector3.Lerp(this._position, position, CupheadTime.Delta * 5f);
	}

	// Token: 0x060007B9 RID: 1977 RVA: 0x000078B9 File Offset: 0x00005AB9
	public void LockCamera(bool lockCamera)
	{
		this.cameraLocked = lockCamera;
	}

	// Token: 0x060007BA RID: 1978 RVA: 0x000078C2 File Offset: 0x00005AC2
	public void SetAutoScroll(bool isScrolling)
	{
		this.autoScrolling = isScrolling;
	}

	// Token: 0x060007BB RID: 1979 RVA: 0x000078CB File Offset: 0x00005ACB
	public void OffsetCamera(bool cameraOffset, bool leftOffset)
	{
		this.cameraOffset = cameraOffset;
		this.leftOffset = leftOffset;
	}

	// Token: 0x060007BC RID: 1980 RVA: 0x000078DB File Offset: 0x00005ADB
	public void SetAutoscrollSpeedMultiplier(float multiplier)
	{
		this.autoScrollSpeedMultiplier = multiplier;
	}

	// Token: 0x060007BD RID: 1981 RVA: 0x00073F18 File Offset: 0x00072118
	public void UpdatePath()
	{
		Vector3 position = this._position;
		Vector2 vector = PlayerManager.Center;
		if (this.stabilizeY)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			Vector2 vector2 = (!(player == null)) ? player.center : Vector2.zero;
			Vector2 vector3 = (!(player2 == null)) ? player2.center : Vector2.zero;
			if (vector2.y > position.y + this.stabilizePaddingTop)
			{
				vector2.y -= this.stabilizePaddingTop;
			}
			else if (vector2.y < position.y - this.stabilizePaddingBottom)
			{
				vector2.y += this.stabilizePaddingBottom;
			}
			else
			{
				vector2.y = position.y;
			}
			if (vector3.y > position.y + this.stabilizePaddingTop)
			{
				vector3.y -= this.stabilizePaddingTop;
			}
			else if (vector3.y < position.y - this.stabilizePaddingBottom)
			{
				vector3.y += this.stabilizePaddingBottom;
			}
			else
			{
				vector3.y = position.y;
			}
			if (player != null && !player.IsDead && player2 != null && !player2.IsDead)
			{
				vector = (vector2 + vector3) / 2f;
			}
			else if (player != null && !player.IsDead)
			{
				vector = vector2;
			}
			else if (player2 != null && !player2.IsDead)
			{
				vector = vector3;
			}
		}
		if (this.cameraOffset)
		{
			float num = (!this.leftOffset) ? -500f : 500f;
			this.targetPos = new Vector3(vector.x + num, vector.y);
		}
		else
		{
			this.targetPos = vector;
		}
		Vector3 vector4 = this.path.GetClosestPoint(this._position, this.targetPos, this.moveX, this.moveY);
		float num2 = (vector4 - position).magnitude / CupheadTime.Delta;
		float num3 = Mathf.Max(this._speedLastFrame + 5000f * CupheadTime.Delta, 1000f);
		if (num2 > num3)
		{
			vector4 = position + (vector4 - position).normalized * num3 * CupheadTime.Delta;
		}
		this._speedLastFrame = Mathf.Min(num2, num3);
		if (this.pathMovesOnlyForward)
		{
			float closestNormalizedPoint = this.path.GetClosestNormalizedPoint(this._position, vector4, this.moveX, this.moveY);
			if (closestNormalizedPoint < this._minPathValue)
			{
				return;
			}
		}
		position.x = vector4.x;
		position.y = vector4.y;
		if (!this.cameraLocked)
		{
			if (!this.autoScrolling)
			{
				this._position = Vector3.Lerp(this._position, position, CupheadTime.Delta * 15f);
			}
			else
			{
				Vector3 vector5;
				vector5..ctor(base.transform.position.x + 500f, base.transform.position.y);
				Vector3 vector6 = this.path.GetClosestPoint(this._position, vector5, this.moveX, this.moveY);
				float num4 = 200f * this.autoScrollSpeedMultiplier;
				this._position = Vector3.MoveTowards(this._position, vector6, CupheadTime.Delta * num4);
			}
		}
		if (this.pathMovesOnlyForward)
		{
			this._minPathValue = this.path.GetClosestNormalizedPoint(this._position, this._position, this.moveX, this.moveY);
		}
	}

	// Token: 0x060007BE RID: 1982 RVA: 0x00074378 File Offset: 0x00072578
	public IEnumerator rotate_camera()
	{
		float time = 2f;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			float phase = Mathf.Sin(t / time);
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * 1f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007BF RID: 1983 RVA: 0x00074394 File Offset: 0x00072594
	public void SetRotation(float amount)
	{
		base.transform.SetEulerAngles(null, null, new float?(amount));
	}

	// Token: 0x060007C0 RID: 1984 RVA: 0x000743C4 File Offset: 0x000725C4
	public IEnumerator change_zoom_cr(float newSize, float time)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.zoom = Mathf.Lerp(base.zoom, newSize, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.zoom = newSize;
		yield return null;
		yield break;
	}

	// Token: 0x060007C1 RID: 1985 RVA: 0x000743F0 File Offset: 0x000725F0
	public IEnumerator slide_camera_cr(Vector3 slideAmount, float time)
	{
		float t = 0f;
		Vector3 start = this._position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			this._position = Vector3.Lerp(start, slideAmount, val);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060007C2 RID: 1986 RVA: 0x000078E4 File Offset: 0x00005AE4
	public void ChangeHorizontalBounds(int left, int right)
	{
		this.bounds.left = left;
		this.bounds.right = right;
	}

	// Token: 0x060007C3 RID: 1987 RVA: 0x000078FE File Offset: 0x00005AFE
	public void ChangeVerticalBounds(int top, int bottom)
	{
		this.bounds.top = top;
		this.bounds.bottom = bottom;
	}

	// Token: 0x060007C4 RID: 1988 RVA: 0x00007918 File Offset: 0x00005B18
	public void ChangeCameraMode(CupheadLevelCamera.Mode mode)
	{
		this.mode = mode;
	}

	// Token: 0x060007C5 RID: 1989 RVA: 0x00007921 File Offset: 0x00005B21
	public void SetPosition(Vector3 pos)
	{
		this._position = pos;
		base.transform.position = pos;
	}

	// Token: 0x040005C0 RID: 1472
	public const string EDITOR_PATH = "Assets/_CUPHEAD/Prefabs/Camera/LevelCamera.prefab";

	// Token: 0x040005C1 RID: 1473
	public const float AUTOSCROLL_SPEED = 200f;

	// Token: 0x040005C6 RID: 1478
	public bool leftOffset;

	// Token: 0x040005C7 RID: 1479
	public const float BOUND_COLLIDER_SIZE = 400f;

	// Token: 0x040005C8 RID: 1480
	public const float BORDER_THICKNESS = 1000f;

	// Token: 0x040005C9 RID: 1481
	public const float CENTER_SPEED = 10f;

	// Token: 0x040005CA RID: 1482
	public const float AUTOSCROLL_CHECK = 500f;

	// Token: 0x040005CB RID: 1483
	public const float OFFSET_AMOUNT = 500f;

	// Token: 0x040005CC RID: 1484
	public const float THREE_SIXTY = 360f;

	// Token: 0x040005CD RID: 1485
	public bool moveX;

	// Token: 0x040005CE RID: 1486
	public bool moveY;

	// Token: 0x040005CF RID: 1487
	public bool stabilizeY;

	// Token: 0x040005D0 RID: 1488
	public float stabilizePaddingTop;

	// Token: 0x040005D1 RID: 1489
	public float stabilizePaddingBottom;

	// Token: 0x040005D2 RID: 1490
	public Vector3 targetPos;

	// Token: 0x040005D3 RID: 1491
	public CupheadLevelCamera.Mode mode;

	// Token: 0x040005D4 RID: 1492
	public Level.Bounds bounds;

	// Token: 0x040005D5 RID: 1493
	public Transform collidersRoot;

	// Token: 0x040005D6 RID: 1494
	public VectorPath path;

	// Token: 0x040005D7 RID: 1495
	public bool pathMovesOnlyForward;

	// Token: 0x040005D8 RID: 1496
	public bool enablePathScrubbing;

	// Token: 0x040005D9 RID: 1497
	[Range(0f, 1f)]
	public float scrub;

	// Token: 0x040005DE RID: 1502
	public Transform leftCollider;

	// Token: 0x040005DF RID: 1503
	public Transform rightCollider;

	// Token: 0x040005E0 RID: 1504
	public Transform topCollider;

	// Token: 0x040005E1 RID: 1505
	public Transform bottomCollider;

	// Token: 0x040005E2 RID: 1506
	[HideInInspector]
	public float LERP_SPEED = 2f;

	// Token: 0x040005E3 RID: 1507
	public const float RELATIVE_LERP_SPEED = 5f;

	// Token: 0x040005E4 RID: 1508
	public const float ROOK_SCROLL_UP_MIN = 200f;

	// Token: 0x040005E5 RID: 1509
	public const float ROOK_SCROLL_UP_MAX = 400f;

	// Token: 0x040005E6 RID: 1510
	public const float V_LERP_SLOW_SPEED = 2.5f;

	// Token: 0x040005E7 RID: 1511
	public const float RUMRUNNERS_SCROLL_UP_THRESHOLD = 200f;

	// Token: 0x040005E8 RID: 1512
	public const float PLATFORMING_LERP_SPEED = 5f;

	// Token: 0x040005E9 RID: 1513
	public const float PATH_LERP_SPEED = 15f;

	// Token: 0x040005EA RID: 1514
	public const float PATH_MAX_SPEED_BEFORE_ACCELERATION = 1000f;

	// Token: 0x040005EB RID: 1515
	public const float PATH_ACCELERATION = 5000f;

	// Token: 0x040005EC RID: 1516
	public float _minPathValue = float.MinValue;

	// Token: 0x040005ED RID: 1517
	public float _speedLastFrame;

	// Token: 0x020008F6 RID: 2294
	public enum Mode
	{
		// Token: 0x04004428 RID: 17448
		Lerp,
		// Token: 0x04004429 RID: 17449
		TrapBox,
		// Token: 0x0400442A RID: 17450
		Relative,
		// Token: 0x0400442B RID: 17451
		Platforming,
		// Token: 0x0400442C RID: 17452
		Path,
		// Token: 0x0400442D RID: 17453
		RelativeRook,
		// Token: 0x0400442E RID: 17454
		RelativeRumRunners,
		// Token: 0x0400442F RID: 17455
		Static = 10000
	}
}
