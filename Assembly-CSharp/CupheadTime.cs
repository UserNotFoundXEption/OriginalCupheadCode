using System;
using System.Collections;
using System.Collections.Generic;
using GCFreeUtils;
using UnityEngine;

// Token: 0x020000B1 RID: 177
public static class CupheadTime
{
	// Token: 0x06000837 RID: 2103 RVA: 0x00075C2C File Offset: 0x00073E2C
	static CupheadTime()
	{
		CupheadTime.delta = new CupheadTime.DeltaObject();
		CupheadTime.layers = new Dictionary<int, float>();
		CupheadTime.Layer[] values = EnumUtils.GetValues<CupheadTime.Layer>();
		foreach (CupheadTime.Layer key in values)
		{
			CupheadTime.layers.Add((int)key, 1f);
		}
	}

	// Token: 0x1700016E RID: 366
	// (get) Token: 0x06000838 RID: 2104 RVA: 0x00007E58 File Offset: 0x00006058
	public static CupheadTime.DeltaObject Delta
	{
		get
		{
			return CupheadTime.delta;
		}
	}

	// Token: 0x1700016F RID: 367
	// (get) Token: 0x06000839 RID: 2105 RVA: 0x00007E5F File Offset: 0x0000605F
	public static float GlobalDelta
	{
		get
		{
			return Time.deltaTime;
		}
	}

	// Token: 0x17000170 RID: 368
	// (get) Token: 0x0600083A RID: 2106 RVA: 0x00007E66 File Offset: 0x00006066
	public static float FixedDelta
	{
		get
		{
			return Time.fixedDeltaTime * CupheadTime.GlobalSpeed;
		}
	}

	// Token: 0x17000171 RID: 369
	// (get) Token: 0x0600083B RID: 2107 RVA: 0x00007E73 File Offset: 0x00006073
	// (set) Token: 0x0600083C RID: 2108 RVA: 0x00007E7A File Offset: 0x0000607A
	public static float GlobalSpeed
	{
		get
		{
			return CupheadTime.globalSpeed;
		}
		set
		{
			CupheadTime.globalSpeed = Mathf.Clamp(value, 0f, 1f);
			CupheadTime.OnChanged();
		}
	}

	// Token: 0x0600083D RID: 2109 RVA: 0x00007E96 File Offset: 0x00006096
	public static float GetLayerSpeed(CupheadTime.Layer layer)
	{
		return CupheadTime.layers[(int)layer];
	}

	// Token: 0x0600083E RID: 2110 RVA: 0x00007EA3 File Offset: 0x000060A3
	public static void SetLayerSpeed(CupheadTime.Layer layer, float value)
	{
		CupheadTime.layers[(int)layer] = value;
		CupheadTime.OnChanged();
	}

	// Token: 0x0600083F RID: 2111 RVA: 0x00007EB6 File Offset: 0x000060B6
	public static void Reset()
	{
		CupheadTime.SetAll(1f);
	}

	// Token: 0x06000840 RID: 2112 RVA: 0x00075C98 File Offset: 0x00073E98
	public static void SetAll(float value)
	{
		CupheadTime.GlobalSpeed = value;
		foreach (CupheadTime.Layer key in EnumUtils.GetValues<CupheadTime.Layer>())
		{
			CupheadTime.layers[(int)key] = value;
		}
		CupheadTime.OnChanged();
	}

	// Token: 0x06000841 RID: 2113 RVA: 0x00007EC2 File Offset: 0x000060C2
	public static void OnChanged()
	{
		if (CupheadTime.OnChangedEvent != null)
		{
			CupheadTime.OnChangedEvent.Call();
		}
	}

	// Token: 0x06000842 RID: 2114 RVA: 0x00007ED8 File Offset: 0x000060D8
	public static bool IsPaused()
	{
		return CupheadTime.GlobalSpeed <= 1E-05f || PauseManager.state == PauseManager.State.Paused;
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x00007EF4 File Offset: 0x000060F4
	public static Coroutine WaitForSeconds(MonoBehaviour m, float time)
	{
		return m.StartCoroutine(CupheadTime.waitForSeconds_cr(time, CupheadTime.Layer.Default));
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00007F03 File Offset: 0x00006103
	public static Coroutine WaitForSeconds(MonoBehaviour m, float time, CupheadTime.Layer layer)
	{
		return m.StartCoroutine(CupheadTime.waitForSeconds_cr(time, layer));
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x00075CDC File Offset: 0x00073EDC
	public static IEnumerator waitForSeconds_cr(float time, CupheadTime.Layer layer)
	{
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta[layer];
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00007F12 File Offset: 0x00006112
	public static Coroutine WaitForUnpause(MonoBehaviour m)
	{
		return m.StartCoroutine(CupheadTime.waitForUnpause_cr());
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00075D00 File Offset: 0x00073F00
	public static IEnumerator waitForUnpause_cr()
	{
		while (CupheadTime.GlobalSpeed == 0f)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x04000650 RID: 1616
	public static readonly CupheadTime.DeltaObject delta;

	// Token: 0x04000651 RID: 1617
	public static float globalSpeed = 1f;

	// Token: 0x04000652 RID: 1618
	public static Dictionary<int, float> layers;

	// Token: 0x04000653 RID: 1619
	public static GCFreeActionList OnChangedEvent = new GCFreeActionList(200, true);

	// Token: 0x02000904 RID: 2308
	public enum Layer
	{
		// Token: 0x04004498 RID: 17560
		Default,
		// Token: 0x04004499 RID: 17561
		Player,
		// Token: 0x0400449A RID: 17562
		Enemy,
		// Token: 0x0400449B RID: 17563
		UI
	}

	// Token: 0x02000905 RID: 2309
	public class DeltaObject
	{
		// Token: 0x17000A2A RID: 2602
		public float this[CupheadTime.Layer layer]
		{
			get
			{
				return Time.deltaTime * CupheadTime.GetLayerSpeed(layer) * CupheadTime.GlobalSpeed;
			}
		}

		// Token: 0x06005358 RID: 21336 RVA: 0x0003F609 File Offset: 0x0003D809
		public static implicit operator float(CupheadTime.DeltaObject d)
		{
			return d[CupheadTime.Layer.Default] * CupheadTime.GlobalSpeed;
		}
	}
}
