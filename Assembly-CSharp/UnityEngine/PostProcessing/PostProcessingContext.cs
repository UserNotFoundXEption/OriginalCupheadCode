using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200062B RID: 1579
	public class PostProcessingContext
	{
		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x060040DD RID: 16605 RVA: 0x00033FE9 File Offset: 0x000321E9
		// (set) Token: 0x060040DE RID: 16606 RVA: 0x00033FF1 File Offset: 0x000321F1
		public bool interrupted { get; set; }

		// Token: 0x060040DF RID: 16607 RVA: 0x00033FFA File Offset: 0x000321FA
		public void Interrupt()
		{
			this.interrupted = true;
		}

		// Token: 0x060040E0 RID: 16608 RVA: 0x00034003 File Offset: 0x00032203
		public PostProcessingContext Reset()
		{
			this.profile = null;
			this.camera = null;
			this.materialFactory = null;
			this.renderTextureFactory = null;
			this.interrupted = false;
			return this;
		}

		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x060040E1 RID: 16609 RVA: 0x00034029 File Offset: 0x00032229
		public bool isGBufferAvailable
		{
			get
			{
				return this.camera.actualRenderingPath == 3;
			}
		}

		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x060040E2 RID: 16610 RVA: 0x00034039 File Offset: 0x00032239
		public bool isHdr
		{
			get
			{
				return this.camera.allowHDR;
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x060040E3 RID: 16611 RVA: 0x00034046 File Offset: 0x00032246
		public int width
		{
			get
			{
				return this.camera.pixelWidth;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x060040E4 RID: 16612 RVA: 0x00034053 File Offset: 0x00032253
		public int height
		{
			get
			{
				return this.camera.pixelHeight;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x060040E5 RID: 16613 RVA: 0x00034060 File Offset: 0x00032260
		public Rect viewport
		{
			get
			{
				return this.camera.rect;
			}
		}

		// Token: 0x04003385 RID: 13189
		public PostProcessingProfile profile;

		// Token: 0x04003386 RID: 13190
		public Camera camera;

		// Token: 0x04003387 RID: 13191
		public MaterialFactory materialFactory;

		// Token: 0x04003388 RID: 13192
		public RenderTextureFactory renderTextureFactory;
	}
}
