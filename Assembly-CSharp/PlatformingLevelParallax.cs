using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000454 RID: 1108
[ExecuteInEditMode]
public class PlatformingLevelParallax : AbstractPausableComponent
{
	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06002F5F RID: 12127 RVA: 0x00027748 File Offset: 0x00025948
	public PlatformingLevel.Theme Theme
	{
		get
		{
			return this._theme;
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06002F60 RID: 12128 RVA: 0x00027750 File Offset: 0x00025950
	public Color Color
	{
		get
		{
			return this._color;
		}
	}

	// Token: 0x17000368 RID: 872
	// (get) Token: 0x06002F61 RID: 12129 RVA: 0x00027758 File Offset: 0x00025958
	public PlatformingLevelParallax.Sides Side
	{
		get
		{
			return this._side;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06002F62 RID: 12130 RVA: 0x00027760 File Offset: 0x00025960
	public int Layer
	{
		get
		{
			return this._layer;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x06002F63 RID: 12131 RVA: 0x00027768 File Offset: 0x00025968
	public int SortingOrderOffset
	{
		get
		{
			return this._sortingOrderOffset;
		}
	}

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06002F64 RID: 12132 RVA: 0x000E1934 File Offset: 0x000DFB34
	public SpriteRenderer[] _spriteRenderers
	{
		get
		{
			if (this._s == null)
			{
				List<SpriteRenderer> list = new List<SpriteRenderer>(base.GetComponentsInChildren<SpriteRenderer>());
				this._s = list.ToArray();
			}
			return this._s;
		}
	}

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06002F65 RID: 12133 RVA: 0x00027770 File Offset: 0x00025970
	public new Transform transform
	{
		get
		{
			if (!this.transformCached)
			{
				this._cachedTransform = base.transform;
				this.transformCached = true;
			}
			return this._cachedTransform;
		}
	}

	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06002F66 RID: 12134 RVA: 0x00027796 File Offset: 0x00025996
	public ParallaxPropertiesData.ThemeProperties.Layer LayerProperties
	{
		get
		{
			if (!this.layerPropertiesCached)
			{
				this._layerProperties = ParallaxPropertiesData.Instance.GetProperty(this._theme, this._layer, this._side);
				this.layerPropertiesCached = true;
			}
			return this._layerProperties;
		}
	}

	// Token: 0x06002F67 RID: 12135 RVA: 0x000277D2 File Offset: 0x000259D2
	public void Start()
	{
		base.FrameDelayedCallback(new Action(this.DelayedStart), 1);
		this.UpdatePosition();
	}

	// Token: 0x06002F68 RID: 12136 RVA: 0x000277EE File Offset: 0x000259EE
	public void DelayedStart()
	{
		this.SetSpriteProperties();
		this.UpdatePosition();
	}

	// Token: 0x06002F69 RID: 12137 RVA: 0x000E196C File Offset: 0x000DFB6C
	public void UpdateBasePosition()
	{
		if (this.levelCameraTransform == null)
		{
			CupheadLevelCamera cupheadLevelCamera = Object.FindObjectOfType<CupheadLevelCamera>();
			if (cupheadLevelCamera == null)
			{
				return;
			}
			this.levelCameraTransform = cupheadLevelCamera.transform;
			if (this.levelCameraTransform == null)
			{
				return;
			}
		}
		if (this.overrideLayerYSpeed)
		{
			this.basePos.x = this.transform.position.x - this.levelCameraTransform.position.x * this.LayerProperties.speed;
			this.basePos.y = this.transform.position.y - this.levelCameraTransform.position.y * this.overrideYSpeed;
		}
		else
		{
			this.basePos = this.transform.position - this.levelCameraTransform.position * this.LayerProperties.speed;
		}
	}

	// Token: 0x06002F6A RID: 12138 RVA: 0x000E1A74 File Offset: 0x000DFC74
	public void SetSpriteProperties()
	{
		foreach (SpriteRenderer spriteRenderer in this._spriteRenderers)
		{
			spriteRenderer.sortingLayerName = ((this._side != PlatformingLevelParallax.Sides.Background) ? SpriteLayer.Foreground.ToString() : SpriteLayer.Background.ToString());
			spriteRenderer.sortingOrder = this.LayerProperties.sortingOrder + this._sortingOrderOffset;
			PlatformingLevelParallaxChild component = spriteRenderer.gameObject.GetComponent<PlatformingLevelParallaxChild>();
			if (component != null)
			{
				spriteRenderer.sortingOrder += component.SortingOrderOffset;
			}
			spriteRenderer.color = this._color;
		}
	}

	// Token: 0x06002F6B RID: 12139 RVA: 0x000277FC File Offset: 0x000259FC
	public void LateUpdate()
	{
		this.UpdatePosition();
	}

	// Token: 0x06002F6C RID: 12140 RVA: 0x000E1B28 File Offset: 0x000DFD28
	public void UpdatePosition()
	{
		if (this.levelCameraTransform == null)
		{
			CupheadLevelCamera cupheadLevelCamera = Object.FindObjectOfType<CupheadLevelCamera>();
			if (cupheadLevelCamera == null)
			{
				return;
			}
			this.levelCameraTransform = cupheadLevelCamera.transform;
			if (this.levelCameraTransform == null)
			{
				return;
			}
		}
		if (this.overrideLayerYSpeed)
		{
			this.transform.SetPosition(new float?(this.basePos.x + this.levelCameraTransform.position.x * this.LayerProperties.speed), new float?(this.basePos.y + this.levelCameraTransform.position.y * this.overrideYSpeed), null);
		}
		else
		{
			this.transform.position = this.basePos + this.levelCameraTransform.position * this.LayerProperties.speed;
		}
	}

	// Token: 0x06002F6D RID: 12141 RVA: 0x000E1C28 File Offset: 0x000DFE28
	public void OnValidate()
	{
		foreach (SpriteRenderer spriteRenderer in this._spriteRenderers)
		{
			if (!(spriteRenderer == null))
			{
				spriteRenderer.hideFlags = 0;
			}
		}
	}

	// Token: 0x0400274A RID: 10058
	[SerializeField]
	public PlatformingLevel.Theme _theme;

	// Token: 0x0400274B RID: 10059
	[SerializeField]
	public Color _color = Color.white;

	// Token: 0x0400274C RID: 10060
	[SerializeField]
	public PlatformingLevelParallax.Sides _side;

	// Token: 0x0400274D RID: 10061
	[SerializeField]
	[Range(0f, 19f)]
	public int _layer;

	// Token: 0x0400274E RID: 10062
	[SerializeField]
	[Range(-2000f, 2000f)]
	public int _sortingOrderOffset;

	// Token: 0x0400274F RID: 10063
	[HideInInspector]
	public Vector3 basePos;

	// Token: 0x04002750 RID: 10064
	[HideInInspector]
	public Vector3 lastPos;

	// Token: 0x04002751 RID: 10065
	public bool overrideLayerYSpeed;

	// Token: 0x04002752 RID: 10066
	public float overrideYSpeed;

	// Token: 0x04002753 RID: 10067
	public Transform levelCameraTransform;

	// Token: 0x04002754 RID: 10068
	public ParallaxLayer _parallaxLayer;

	// Token: 0x04002755 RID: 10069
	public SpriteRenderer[] _s;

	// Token: 0x04002756 RID: 10070
	public bool transformCached;

	// Token: 0x04002757 RID: 10071
	public Transform _cachedTransform;

	// Token: 0x04002758 RID: 10072
	public bool layerPropertiesCached;

	// Token: 0x04002759 RID: 10073
	public ParallaxPropertiesData.ThemeProperties.Layer _layerProperties;

	// Token: 0x020010CF RID: 4303
	public enum Sides
	{
		// Token: 0x0400773A RID: 30522
		Background,
		// Token: 0x0400773B RID: 30523
		Foreground
	}
}
