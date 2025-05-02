using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class CupheadEventSystem : AbstractMonoBehaviour
{
	// Token: 0x06000A4F RID: 2639 RVA: 0x000095ED File Offset: 0x000077ED
	public static void Init()
	{
		if (CupheadEventSystem._instance != null)
		{
			return;
		}
		CupheadEventSystem._instance = (Object.Instantiate(Resources.Load("EventSystems/CupheadEventSystem")) as GameObject).GetComponent<CupheadEventSystem>();
	}

	// Token: 0x06000A50 RID: 2640 RVA: 0x0007B6E0 File Offset: 0x000798E0
	public override void Awake()
	{
		base.Awake();
		if (CupheadEventSystem._instance == null)
		{
			CupheadEventSystem._instance = this;
			base.gameObject.name = base.gameObject.name.Replace("(Clone)", string.Empty);
			return;
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04000848 RID: 2120
	public const string PATH = "EventSystems/CupheadEventSystem";

	// Token: 0x04000849 RID: 2121
	public static CupheadEventSystem _instance;
}
