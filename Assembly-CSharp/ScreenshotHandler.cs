using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class ScreenshotHandler : MonoBehaviour
{
	// Token: 0x060009A8 RID: 2472 RVA: 0x00008F64 File Offset: 0x00007164
	public void Awake()
	{
		ScreenshotHandler.instance = this;
		Object.DontDestroyOnLoad(base.gameObject);
		Object.Destroy(this);
	}

	// Token: 0x060009A9 RID: 2473 RVA: 0x00008F7D File Offset: 0x0000717D
	public static void TakeScreenshot_Static(ScreenshotHandler.cameraType _camera, string _folderName, string _fileName)
	{
	}

	// Token: 0x0400073B RID: 1851
	public static ScreenshotHandler instance;

	// Token: 0x0400073C RID: 1852
	public bool takeScreenshotNextFrame;

	// Token: 0x0400073D RID: 1853
	public Camera myCamera;

	// Token: 0x0400073E RID: 1854
	public string fileName;

	// Token: 0x0400073F RID: 1855
	public string folderName;

	// Token: 0x04000740 RID: 1856
	public ScreenshotHandler.cameraType currentCameraType;

	// Token: 0x02000933 RID: 2355
	public enum cameraType
	{
		// Token: 0x04004569 RID: 17769
		Map,
		// Token: 0x0400456A RID: 17770
		UI,
		// Token: 0x0400456B RID: 17771
		Level
	}
}
