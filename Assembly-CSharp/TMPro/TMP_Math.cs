using System;

namespace TMPro
{
	// Token: 0x0200068D RID: 1677
	public static class TMP_Math
	{
		// Token: 0x06004746 RID: 18246 RVA: 0x00038AD7 File Offset: 0x00036CD7
		public static bool Approximately(float a, float b)
		{
			return b - 0.0001f < a && a < b + 0.0001f;
		}
	}
}
