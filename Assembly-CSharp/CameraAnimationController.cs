using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x0200009C RID: 156
public class CameraAnimationController : MonoBehaviour
{
	// Token: 0x06000783 RID: 1923 RVA: 0x00072D04 File Offset: 0x00070F04
	public void Start()
	{
		GameObject gameObject = GameObject.FindWithTag("MainCamera");
		this.Camera = gameObject.GetComponent<Camera>();
		this.MapCamera = gameObject.GetComponent<CupheadMapCamera>();
		this.BlurOptimized = gameObject.GetComponent<BlurOptimized>();
	}

	// Token: 0x06000784 RID: 1924 RVA: 0x000076F5 File Offset: 0x000058F5
	public void Update()
	{
		this.ApplyProperties();
	}

	// Token: 0x06000785 RID: 1925 RVA: 0x000076FD File Offset: 0x000058FD
	public void OnEnable()
	{
	}

	// Token: 0x06000786 RID: 1926 RVA: 0x000076FF File Offset: 0x000058FF
	public void OnDisable()
	{
		this.ApplyProperties();
	}

	// Token: 0x06000787 RID: 1927 RVA: 0x00072D40 File Offset: 0x00070F40
	public void ApplyProperties()
	{
		if (this.Blur > 0f && !this.BlurOptimized.enabled)
		{
			this.BlurOptimized.enabled = true;
		}
		if (this.Blur <= 0f && this.BlurOptimized.enabled)
		{
			this.BlurOptimized.enabled = false;
		}
		if (this.MapCamera != null)
		{
			this.MapCamera.centerOnPlayer = this.CenterOnPlayer;
			this.Camera.orthographicSize = this.OrthoSize;
		}
		this.BlurOptimized.blurSize = this.Blur;
	}

	// Token: 0x040005AE RID: 1454
	public bool CenterOnPlayer = true;

	// Token: 0x040005AF RID: 1455
	public float OrthoSize;

	// Token: 0x040005B0 RID: 1456
	public float Blur;

	// Token: 0x040005B1 RID: 1457
	public Camera Camera;

	// Token: 0x040005B2 RID: 1458
	public CupheadMapCamera MapCamera;

	// Token: 0x040005B3 RID: 1459
	public BlurOptimized BlurOptimized;
}
