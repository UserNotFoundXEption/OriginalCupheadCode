using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E1 RID: 225
public static class ObjectPoolExtensions
{
	// Token: 0x06000A7D RID: 2685 RVA: 0x0000983F File Offset: 0x00007A3F
	public static void CreatePool<T>(this T prefab) where T : Component
	{
		ObjectPool.CreatePool<T>(prefab, 0);
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x00009848 File Offset: 0x00007A48
	public static void CreatePool<T>(this T prefab, int initialPoolSize) where T : Component
	{
		ObjectPool.CreatePool<T>(prefab, initialPoolSize);
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x00009851 File Offset: 0x00007A51
	public static void CreatePool(this GameObject prefab)
	{
		ObjectPool.CreatePool(prefab, 0);
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0000985A File Offset: 0x00007A5A
	public static void CreatePool(this GameObject prefab, int initialPoolSize)
	{
		ObjectPool.CreatePool(prefab, initialPoolSize);
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x00009863 File Offset: 0x00007A63
	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, parent, position, rotation);
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0000986E File Offset: 0x00007A6E
	public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, null, position, rotation);
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x00009879 File Offset: 0x00007A79
	public static T Spawn<T>(this T prefab, Transform parent, Vector3 position) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, parent, position, Quaternion.identity);
	}

	// Token: 0x06000A84 RID: 2692 RVA: 0x00009888 File Offset: 0x00007A88
	public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, null, position, Quaternion.identity);
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x00009897 File Offset: 0x00007A97
	public static T Spawn<T>(this T prefab, Transform parent) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x000098AA File Offset: 0x00007AAA
	public static T Spawn<T>(this T prefab) where T : Component
	{
		return ObjectPool.Spawn<T>(prefab, null, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x000098BD File Offset: 0x00007ABD
	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		return ObjectPool.Spawn(prefab, parent, position, rotation);
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x000098C8 File Offset: 0x00007AC8
	public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return ObjectPool.Spawn(prefab, null, position, rotation);
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x000098D3 File Offset: 0x00007AD3
	public static GameObject Spawn(this GameObject prefab, Transform parent, Vector3 position)
	{
		return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x000098E2 File Offset: 0x00007AE2
	public static GameObject Spawn(this GameObject prefab, Vector3 position)
	{
		return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x000098F1 File Offset: 0x00007AF1
	public static GameObject Spawn(this GameObject prefab, Transform parent)
	{
		return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x00009904 File Offset: 0x00007B04
	public static GameObject Spawn(this GameObject prefab)
	{
		return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x00009917 File Offset: 0x00007B17
	public static void Recycle<T>(this T obj) where T : Component
	{
		ObjectPool.Recycle<T>(obj);
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0000991F File Offset: 0x00007B1F
	public static void Recycle(this GameObject obj)
	{
		ObjectPool.Recycle(obj);
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x00009927 File Offset: 0x00007B27
	public static void RecycleAll<T>(this T prefab) where T : Component
	{
		ObjectPool.RecycleAll<T>(prefab);
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0000992F File Offset: 0x00007B2F
	public static void RecycleAll(this GameObject prefab)
	{
		ObjectPool.RecycleAll(prefab);
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x00009937 File Offset: 0x00007B37
	public static int CountPooled<T>(this T prefab) where T : Component
	{
		return ObjectPool.CountPooled<T>(prefab);
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0000993F File Offset: 0x00007B3F
	public static int CountPooled(this GameObject prefab)
	{
		return ObjectPool.CountPooled(prefab);
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00009947 File Offset: 0x00007B47
	public static int CountSpawned<T>(this T prefab) where T : Component
	{
		return ObjectPool.CountSpawned<T>(prefab);
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0000994F File Offset: 0x00007B4F
	public static int CountSpawned(this GameObject prefab)
	{
		return ObjectPool.CountSpawned(prefab);
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x00009957 File Offset: 0x00007B57
	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return ObjectPool.GetSpawned(prefab, list, appendList);
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x00009961 File Offset: 0x00007B61
	public static List<GameObject> GetSpawned(this GameObject prefab, List<GameObject> list)
	{
		return ObjectPool.GetSpawned(prefab, list, false);
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0000996B File Offset: 0x00007B6B
	public static List<GameObject> GetSpawned(this GameObject prefab)
	{
		return ObjectPool.GetSpawned(prefab, null, false);
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00009975 File Offset: 0x00007B75
	public static List<T> GetSpawned<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return ObjectPool.GetSpawned<T>(prefab, list, appendList);
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0000997F File Offset: 0x00007B7F
	public static List<T> GetSpawned<T>(this T prefab, List<T> list) where T : Component
	{
		return ObjectPool.GetSpawned<T>(prefab, list, false);
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00009989 File Offset: 0x00007B89
	public static List<T> GetSpawned<T>(this T prefab) where T : Component
	{
		return ObjectPool.GetSpawned<T>(prefab, null, false);
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x00009993 File Offset: 0x00007B93
	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list, bool appendList)
	{
		return ObjectPool.GetPooled(prefab, list, appendList);
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0000999D File Offset: 0x00007B9D
	public static List<GameObject> GetPooled(this GameObject prefab, List<GameObject> list)
	{
		return ObjectPool.GetPooled(prefab, list, false);
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x000099A7 File Offset: 0x00007BA7
	public static List<GameObject> GetPooled(this GameObject prefab)
	{
		return ObjectPool.GetPooled(prefab, null, false);
	}

	// Token: 0x06000A9E RID: 2718 RVA: 0x000099B1 File Offset: 0x00007BB1
	public static List<T> GetPooled<T>(this T prefab, List<T> list, bool appendList) where T : Component
	{
		return ObjectPool.GetPooled<T>(prefab, list, appendList);
	}

	// Token: 0x06000A9F RID: 2719 RVA: 0x000099BB File Offset: 0x00007BBB
	public static List<T> GetPooled<T>(this T prefab, List<T> list) where T : Component
	{
		return ObjectPool.GetPooled<T>(prefab, list, false);
	}

	// Token: 0x06000AA0 RID: 2720 RVA: 0x000099C5 File Offset: 0x00007BC5
	public static List<T> GetPooled<T>(this T prefab) where T : Component
	{
		return ObjectPool.GetPooled<T>(prefab, null, false);
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x000099CF File Offset: 0x00007BCF
	public static void DestroyPooled(this GameObject prefab)
	{
		ObjectPool.DestroyPooled(prefab);
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x000099D7 File Offset: 0x00007BD7
	public static void DestroyPooled<T>(this T prefab) where T : Component
	{
		ObjectPool.DestroyPooled(prefab.gameObject);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x000099EB File Offset: 0x00007BEB
	public static void DestroyAll(this GameObject prefab)
	{
		ObjectPool.DestroyAll(prefab);
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x000099F3 File Offset: 0x00007BF3
	public static void DestroyAll<T>(this T prefab) where T : Component
	{
		ObjectPool.DestroyAll(prefab.gameObject);
	}
}
