using System;

namespace RektTransform
{
	// Token: 0x0200005E RID: 94
	public static class Anchors
	{
		// Token: 0x04000481 RID: 1153
		public static readonly MinMax TopLeft = new MinMax(0f, 1f, 0f, 1f);

		// Token: 0x04000482 RID: 1154
		public static readonly MinMax TopCenter = new MinMax(0.5f, 1f, 0.5f, 1f);

		// Token: 0x04000483 RID: 1155
		public static readonly MinMax TopRight = new MinMax(1f, 1f, 1f, 1f);

		// Token: 0x04000484 RID: 1156
		public static readonly MinMax TopStretch = new MinMax(0f, 1f, 1f, 1f);

		// Token: 0x04000485 RID: 1157
		public static readonly MinMax MiddleLeft = new MinMax(0f, 0.5f, 0f, 0.5f);

		// Token: 0x04000486 RID: 1158
		public static readonly MinMax TrueCenter = new MinMax(0.5f, 0.5f, 0.5f, 0.5f);

		// Token: 0x04000487 RID: 1159
		public static readonly MinMax MiddleCenter = new MinMax(0.5f, 0.5f, 0.5f, 0.5f);

		// Token: 0x04000488 RID: 1160
		public static readonly MinMax MiddleRight = new MinMax(1f, 0.5f, 1f, 0.5f);

		// Token: 0x04000489 RID: 1161
		public static readonly MinMax MiddleStretch = new MinMax(0f, 0.5f, 1f, 0.5f);

		// Token: 0x0400048A RID: 1162
		public static readonly MinMax BottomLeft = new MinMax(0f, 0f, 0f, 0f);

		// Token: 0x0400048B RID: 1163
		public static readonly MinMax BottomCenter = new MinMax(0.5f, 0f, 0.5f, 0f);

		// Token: 0x0400048C RID: 1164
		public static readonly MinMax BottomRight = new MinMax(1f, 0f, 1f, 0f);

		// Token: 0x0400048D RID: 1165
		public static readonly MinMax BottomStretch = new MinMax(0f, 0f, 1f, 0f);

		// Token: 0x0400048E RID: 1166
		public static readonly MinMax StretchLeft = new MinMax(0f, 0f, 0f, 1f);

		// Token: 0x0400048F RID: 1167
		public static readonly MinMax StretchCenter = new MinMax(0.5f, 0f, 0.5f, 1f);

		// Token: 0x04000490 RID: 1168
		public static readonly MinMax StretchRight = new MinMax(1f, 0f, 1f, 1f);

		// Token: 0x04000491 RID: 1169
		public static readonly MinMax TrueStretch = new MinMax(0f, 0f, 1f, 1f);

		// Token: 0x04000492 RID: 1170
		public static readonly MinMax StretchStretch = new MinMax(0f, 0f, 1f, 1f);
	}
}
