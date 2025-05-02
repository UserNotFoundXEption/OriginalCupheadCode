using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200062C RID: 1580
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x060040E6 RID: 16614 RVA: 0x0003406D File Offset: 0x0003226D
		public PostProcessingModel()
		{
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x060040E7 RID: 16615 RVA: 0x00034075 File Offset: 0x00032275
		// (set) Token: 0x060040E8 RID: 16616 RVA: 0x0003407D File Offset: 0x0003227D
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x060040E9 RID: 16617
		public abstract void Reset();

		// Token: 0x060040EA RID: 16618 RVA: 0x00034092 File Offset: 0x00032292
		public virtual void OnValidate()
		{
		}

		// Token: 0x0400338A RID: 13194
		[SerializeField]
		[GetSet("enabled")]
		public bool m_Enabled;
	}
}
