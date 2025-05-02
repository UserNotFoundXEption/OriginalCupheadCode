using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000687 RID: 1671
	public class TMP_UpdateRegistry
	{
		// Token: 0x0600471B RID: 18203 RVA: 0x00159434 File Offset: 0x00157634
		public TMP_UpdateRegistry()
		{
			Canvas.willRenderCanvases += new Canvas.WillRenderCanvases(this.PerformUpdateForCanvasRendererObjects);
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600471C RID: 18204 RVA: 0x000388C9 File Offset: 0x00036AC9
		public static TMP_UpdateRegistry instance
		{
			get
			{
				if (TMP_UpdateRegistry.s_Instance == null)
				{
					TMP_UpdateRegistry.s_Instance = new TMP_UpdateRegistry();
				}
				return TMP_UpdateRegistry.s_Instance;
			}
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x000388E4 File Offset: 0x00036AE4
		public static void RegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element);
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x00159484 File Offset: 0x00157684
		public bool InternalRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			int instanceID = (element as Object).GetInstanceID();
			if (this.m_LayoutQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_LayoutQueueLookup[instanceID] = instanceID;
			this.m_LayoutRebuildQueue.Add(element);
			return true;
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x000388F2 File Offset: 0x00036AF2
		public static void RegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x001594CC File Offset: 0x001576CC
		public bool InternalRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			int instanceID = (element as Object).GetInstanceID();
			if (this.m_GraphicQueueLookup.ContainsKey(instanceID))
			{
				return false;
			}
			this.m_GraphicQueueLookup[instanceID] = instanceID;
			this.m_GraphicRebuildQueue.Add(element);
			return true;
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x00159514 File Offset: 0x00157714
		public void PerformUpdateForCanvasRendererObjects()
		{
			for (int i = 0; i < this.m_LayoutRebuildQueue.Count; i++)
			{
				ICanvasElement canvasElement = TMP_UpdateRegistry.instance.m_LayoutRebuildQueue[i];
				canvasElement.Rebuild(0);
			}
			if (this.m_LayoutRebuildQueue.Count > 0)
			{
				this.m_LayoutRebuildQueue.Clear();
				this.m_LayoutQueueLookup.Clear();
			}
			for (int j = 0; j < this.m_GraphicRebuildQueue.Count; j++)
			{
				ICanvasElement canvasElement2 = TMP_UpdateRegistry.instance.m_GraphicRebuildQueue[j];
				canvasElement2.Rebuild(3);
			}
			if (this.m_GraphicRebuildQueue.Count > 0)
			{
				this.m_GraphicRebuildQueue.Clear();
				this.m_GraphicQueueLookup.Clear();
			}
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x00038900 File Offset: 0x00036B00
		public void PerformUpdateForMeshRendererObjects()
		{
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x00038902 File Offset: 0x00036B02
		public static void UnRegisterCanvasElementForRebuild(ICanvasElement element)
		{
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForLayoutRebuild(element);
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForGraphicRebuild(element);
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x001595D8 File Offset: 0x001577D8
		public void InternalUnRegisterCanvasElementForLayoutRebuild(ICanvasElement element)
		{
			int instanceID = (element as Object).GetInstanceID();
			TMP_UpdateRegistry.instance.m_LayoutRebuildQueue.Remove(element);
			this.m_GraphicQueueLookup.Remove(instanceID);
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x00159610 File Offset: 0x00157810
		public void InternalUnRegisterCanvasElementForGraphicRebuild(ICanvasElement element)
		{
			int instanceID = (element as Object).GetInstanceID();
			TMP_UpdateRegistry.instance.m_GraphicRebuildQueue.Remove(element);
			this.m_LayoutQueueLookup.Remove(instanceID);
		}

		// Token: 0x04003703 RID: 14083
		public static TMP_UpdateRegistry s_Instance;

		// Token: 0x04003704 RID: 14084
		public readonly List<ICanvasElement> m_LayoutRebuildQueue = new List<ICanvasElement>();

		// Token: 0x04003705 RID: 14085
		public Dictionary<int, int> m_LayoutQueueLookup = new Dictionary<int, int>();

		// Token: 0x04003706 RID: 14086
		public readonly List<ICanvasElement> m_GraphicRebuildQueue = new List<ICanvasElement>();

		// Token: 0x04003707 RID: 14087
		public Dictionary<int, int> m_GraphicQueueLookup = new Dictionary<int, int>();
	}
}
