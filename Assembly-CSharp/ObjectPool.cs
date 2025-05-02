using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000E0 RID: 224
public sealed class ObjectPool : MonoBehaviour
{
	// Token: 0x06000A55 RID: 2645 RVA: 0x0000964C File Offset: 0x0000784C
	public void Awake()
	{
		ObjectPool._instance = this;
		if (this.startupPoolMode == ObjectPool.StartupPoolMode.Awake)
		{
			ObjectPool.CreateStartupPools();
		}
	}

	// Token: 0x06000A56 RID: 2646 RVA: 0x00009664 File Offset: 0x00007864
	public void Start()
	{
		if (this.startupPoolMode == ObjectPool.StartupPoolMode.Start)
		{
			ObjectPool.CreateStartupPools();
		}
	}

	// Token: 0x06000A57 RID: 2647 RVA: 0x0007B754 File Offset: 0x00079954
	public void OnDestroy()
	{
		foreach (KeyValuePair<GameObject, List<GameObject>> keyValuePair in this.pooledObjects)
		{
			ObjectPool.DestroyAll(keyValuePair.Key);
		}
		this.spawnedObjects.Clear();
		this.pooledObjects.Clear();
	}

	// Token: 0x06000A58 RID: 2648 RVA: 0x0007B7CC File Offset: 0x000799CC
	public static void CreateStartupPools()
	{
		if (!ObjectPool.instance.startupPoolsCreated)
		{
			ObjectPool.instance.startupPoolsCreated = true;
			ObjectPool.StartupPool[] array = ObjectPool.instance.startupPools;
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					ObjectPool.CreatePool(array[i].prefab, array[i].size);
				}
			}
		}
	}

	// Token: 0x06000A59 RID: 2649 RVA: 0x00009677 File Offset: 0x00007877
	public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
	{
		ObjectPool.CreatePool(prefab.gameObject, initialPoolSize);
	}

	// Token: 0x06000A5A RID: 2650 RVA: 0x0007B838 File Offset: 0x00079A38
	public static void CreatePool(GameObject prefab, int initialPoolSize)
	{
		if (prefab != null && !ObjectPool.instance.pooledObjects.ContainsKey(prefab))
		{
			List<GameObject> list = new List<GameObject>();
			ObjectPool.instance.pooledObjects.Add(prefab, list);
			if (initialPoolSize > 0)
			{
				bool activeSelf = prefab.activeSelf;
				prefab.SetActive(true);
				Transform transform = ObjectPool.instance.transform;
				while (list.Count < initialPoolSize)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(prefab);
					gameObject.SetActive(false);
					gameObject.transform.parent = transform;
					list.Add(gameObject);
				}
				prefab.SetActive(activeSelf);
			}
		}
	}

	// Token: 0x06000A5B RID: 2651 RVA: 0x0000968C File Offset: 0x0000788C
	public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
	}

	// Token: 0x06000A5C RID: 2652 RVA: 0x000096A8 File Offset: 0x000078A8
	public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
	}

	// Token: 0x06000A5D RID: 2653 RVA: 0x000096C4 File Offset: 0x000078C4
	public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
	}

	// Token: 0x06000A5E RID: 2654 RVA: 0x000096E4 File Offset: 0x000078E4
	public static T Spawn<T>(T prefab, Vector3 position) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
	}

	// Token: 0x06000A5F RID: 2655 RVA: 0x00009704 File Offset: 0x00007904
	public static T Spawn<T>(T prefab, Transform parent) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}

	// Token: 0x06000A60 RID: 2656 RVA: 0x00009728 File Offset: 0x00007928
	public static T Spawn<T>(T prefab) where T : Component
	{
		return ObjectPool.Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0007B8D8 File Offset: 0x00079AD8
	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
	{
		List<GameObject> list;
		GameObject gameObject;
		Transform transform;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			gameObject = null;
			if (list.Count > 0)
			{
				while (gameObject == null && list.Count > 0)
				{
					gameObject = list[0];
					list.RemoveAt(0);
				}
				if (gameObject != null)
				{
					transform = gameObject.transform;
					transform.parent = parent;
					transform.localPosition = position;
					transform.localRotation = rotation;
					gameObject.SetActive(true);
					ObjectPool.instance.spawnedObjects.Add(gameObject, prefab);
					return gameObject;
				}
			}
			gameObject = Object.Instantiate<GameObject>(prefab);
			transform = gameObject.transform;
			transform.parent = parent;
			transform.localPosition = position;
			transform.localRotation = rotation;
			ObjectPool.instance.spawnedObjects.Add(gameObject, prefab);
			return gameObject;
		}
		gameObject = Object.Instantiate<GameObject>(prefab);
		transform = gameObject.GetComponent<Transform>();
		transform.parent = parent;
		transform.localPosition = position;
		transform.localRotation = rotation;
		return gameObject;
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0000974C File Offset: 0x0000794C
	public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
	{
		return ObjectPool.Spawn(prefab, parent, position, Quaternion.identity);
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0000975B File Offset: 0x0000795B
	public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		return ObjectPool.Spawn(prefab, null, position, rotation);
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x00009766 File Offset: 0x00007966
	public static GameObject Spawn(GameObject prefab, Transform parent)
	{
		return ObjectPool.Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06000A65 RID: 2661 RVA: 0x00009779 File Offset: 0x00007979
	public static GameObject Spawn(GameObject prefab, Vector3 position)
	{
		return ObjectPool.Spawn(prefab, null, position, Quaternion.identity);
	}

	// Token: 0x06000A66 RID: 2662 RVA: 0x00009788 File Offset: 0x00007988
	public static GameObject Spawn(GameObject prefab)
	{
		return ObjectPool.Spawn(prefab, null, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x06000A67 RID: 2663 RVA: 0x0000979B File Offset: 0x0000799B
	public static void Recycle<T>(T obj) where T : Component
	{
		ObjectPool.Recycle(obj.gameObject);
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0007B9D4 File Offset: 0x00079BD4
	public static void Recycle(GameObject obj)
	{
		GameObject prefab;
		if (ObjectPool.instance.spawnedObjects.TryGetValue(obj, out prefab))
		{
			ObjectPool.Recycle(obj, prefab);
		}
		else
		{
			Object.Destroy(obj);
		}
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0007BA0C File Offset: 0x00079C0C
	public static void Recycle(GameObject obj, GameObject prefab)
	{
		ObjectPool.instance.pooledObjects[prefab].Add(obj);
		ObjectPool.instance.spawnedObjects.Remove(obj);
		if (obj != null)
		{
			obj.transform.parent = ObjectPool.instance.transform;
			obj.SetActive(false);
		}
	}

	// Token: 0x06000A6A RID: 2666 RVA: 0x000097AF File Offset: 0x000079AF
	public static void RecycleAll<T>(T prefab) where T : Component
	{
		ObjectPool.RecycleAll(prefab.gameObject);
	}

	// Token: 0x06000A6B RID: 2667 RVA: 0x0007BA68 File Offset: 0x00079C68
	public static void RecycleAll(GameObject prefab)
	{
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectPool.instance.spawnedObjects)
		{
			if (keyValuePair.Value == prefab)
			{
				ObjectPool.tempList.Add(keyValuePair.Key);
			}
		}
		for (int i = 0; i < ObjectPool.tempList.Count; i++)
		{
			ObjectPool.Recycle(ObjectPool.tempList[i]);
		}
		ObjectPool.tempList.Clear();
	}

	// Token: 0x06000A6C RID: 2668 RVA: 0x0007BB1C File Offset: 0x00079D1C
	public static void RecycleAll()
	{
		ObjectPool.tempList.AddRange(ObjectPool.instance.spawnedObjects.Keys);
		for (int i = 0; i < ObjectPool.tempList.Count; i++)
		{
			ObjectPool.Recycle(ObjectPool.tempList[i]);
		}
		ObjectPool.tempList.Clear();
	}

	// Token: 0x06000A6D RID: 2669 RVA: 0x000097C3 File Offset: 0x000079C3
	public static bool IsSpawned(GameObject obj)
	{
		return ObjectPool.instance.spawnedObjects.ContainsKey(obj);
	}

	// Token: 0x06000A6E RID: 2670 RVA: 0x000097D5 File Offset: 0x000079D5
	public static int CountPooled<T>(T prefab) where T : Component
	{
		return ObjectPool.CountPooled(prefab.gameObject);
	}

	// Token: 0x06000A6F RID: 2671 RVA: 0x0007BB78 File Offset: 0x00079D78
	public static int CountPooled(GameObject prefab)
	{
		List<GameObject> list;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			return list.Count;
		}
		return 0;
	}

	// Token: 0x06000A70 RID: 2672 RVA: 0x000097E9 File Offset: 0x000079E9
	public static int CountSpawned<T>(T prefab) where T : Component
	{
		return ObjectPool.CountSpawned(prefab.gameObject);
	}

	// Token: 0x06000A71 RID: 2673 RVA: 0x0007BBA4 File Offset: 0x00079DA4
	public static int CountSpawned(GameObject prefab)
	{
		int num = 0;
		foreach (GameObject gameObject in ObjectPool.instance.spawnedObjects.Values)
		{
			if (prefab == gameObject)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000A72 RID: 2674 RVA: 0x0007BC18 File Offset: 0x00079E18
	public static int CountAllPooled()
	{
		int num = 0;
		foreach (List<GameObject> list in ObjectPool.instance.pooledObjects.Values)
		{
			num += list.Count;
		}
		return num;
	}

	// Token: 0x06000A73 RID: 2675 RVA: 0x0007BC84 File Offset: 0x00079E84
	public static List<GameObject> GetPooled(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		List<GameObject> collection;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out collection))
		{
			list.AddRange(collection);
		}
		return list;
	}

	// Token: 0x06000A74 RID: 2676 RVA: 0x0007BCCC File Offset: 0x00079ECC
	public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		List<GameObject> list2;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab.gameObject, out list2))
		{
			for (int i = 0; i < list2.Count; i++)
			{
				list.Add(list2[i].GetComponent<T>());
			}
		}
		return list;
	}

	// Token: 0x06000A75 RID: 2677 RVA: 0x0007BD40 File Offset: 0x00079F40
	public static List<GameObject> GetSpawned(GameObject prefab, List<GameObject> list, bool appendList)
	{
		if (list == null)
		{
			list = new List<GameObject>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectPool.instance.spawnedObjects)
		{
			if (keyValuePair.Value == prefab)
			{
				list.Add(keyValuePair.Key);
			}
		}
		return list;
	}

	// Token: 0x06000A76 RID: 2678 RVA: 0x0007BDD4 File Offset: 0x00079FD4
	public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
	{
		if (list == null)
		{
			list = new List<T>();
		}
		if (!appendList)
		{
			list.Clear();
		}
		GameObject gameObject = prefab.gameObject;
		foreach (KeyValuePair<GameObject, GameObject> keyValuePair in ObjectPool.instance.spawnedObjects)
		{
			if (keyValuePair.Value == gameObject)
			{
				list.Add(keyValuePair.Key.GetComponent<T>());
			}
		}
		return list;
	}

	// Token: 0x06000A77 RID: 2679 RVA: 0x0007BE7C File Offset: 0x0007A07C
	public static void DestroyPooled(GameObject prefab)
	{
		List<GameObject> list;
		if (ObjectPool.instance.pooledObjects.TryGetValue(prefab, out list))
		{
			for (int i = 0; i < list.Count; i++)
			{
				Object.Destroy(list[i]);
			}
			list.Clear();
		}
	}

	// Token: 0x06000A78 RID: 2680 RVA: 0x000097FD File Offset: 0x000079FD
	public static void DestroyPooled<T>(T prefab) where T : Component
	{
		ObjectPool.DestroyPooled(prefab.gameObject);
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x00009811 File Offset: 0x00007A11
	public static void DestroyAll(GameObject prefab)
	{
		ObjectPool.RecycleAll(prefab);
		ObjectPool.DestroyPooled(prefab);
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0000981F File Offset: 0x00007A1F
	public static void DestroyAll<T>(T prefab) where T : Component
	{
		ObjectPool.DestroyAll(prefab.gameObject);
	}

	// Token: 0x170001B1 RID: 433
	// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0007BECC File Offset: 0x0007A0CC
	public static ObjectPool instance
	{
		get
		{
			if (ObjectPool._instance != null)
			{
				return ObjectPool._instance;
			}
			ObjectPool._instance = Object.FindObjectOfType<ObjectPool>();
			if (ObjectPool._instance != null)
			{
				return ObjectPool._instance;
			}
			ObjectPool._instance = new GameObject("ObjectPool")
			{
				transform = 
				{
					localPosition = Vector3.zero,
					localRotation = Quaternion.identity,
					localScale = Vector3.one
				}
			}.AddComponent<ObjectPool>();
			return ObjectPool._instance;
		}
	}

	// Token: 0x0400084A RID: 2122
	public static ObjectPool _instance;

	// Token: 0x0400084B RID: 2123
	public static List<GameObject> tempList = new List<GameObject>();

	// Token: 0x0400084C RID: 2124
	public Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();

	// Token: 0x0400084D RID: 2125
	public Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();

	// Token: 0x0400084E RID: 2126
	public ObjectPool.StartupPoolMode startupPoolMode;

	// Token: 0x0400084F RID: 2127
	public ObjectPool.StartupPool[] startupPools;

	// Token: 0x04000850 RID: 2128
	public bool startupPoolsCreated;

	// Token: 0x02000952 RID: 2386
	public enum StartupPoolMode
	{
		// Token: 0x04004620 RID: 17952
		Awake,
		// Token: 0x04004621 RID: 17953
		Start,
		// Token: 0x04004622 RID: 17954
		CallManually
	}

	// Token: 0x02000953 RID: 2387
	[Serializable]
	public class StartupPool
	{
		// Token: 0x04004623 RID: 17955
		public int size;

		// Token: 0x04004624 RID: 17956
		public GameObject prefab;
	}
}
