using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000068 RID: 104
public class ComponentReferenceManager
{
	// Token: 0x06000571 RID: 1393 RVA: 0x0006C970 File Offset: 0x0006AB70
	public void AddRef(Component c)
	{
		string key = string.Format("Index:<color=red>{0}</color> ComponentType:<color=red>{1}</color> GameObject:<color=red>{2}</color>", this.count, c.GetType().ToString(), this.GetGameObjectPath(c));
		this.refs[key] = new WeakReference(c);
		this.count++;
	}

	// Token: 0x06000572 RID: 1394 RVA: 0x0006C9C8 File Offset: 0x0006ABC8
	public string GetGameObjectPath(Component c)
	{
		GameObject gameObject = c.gameObject;
		string text = "/" + gameObject.name;
		while (gameObject.transform.parent != null)
		{
			gameObject = gameObject.transform.parent.gameObject;
			text = "/" + gameObject.name + text;
		}
		return text;
	}

	// Token: 0x06000573 RID: 1395 RVA: 0x0006CA2C File Offset: 0x0006AC2C
	public void PrintLog()
	{
		GC.Collect();
		Debug.LogError("Objects that are destroyed but still referenced:", null);
		foreach (KeyValuePair<string, WeakReference> keyValuePair in this.refs)
		{
			if (keyValuePair.Value.IsAlive)
			{
				if (keyValuePair.Value.Target == null)
				{
					Debug.LogErrorFormat("Target is null {0}", new object[]
					{
						keyValuePair.Key
					});
				}
				else
				{
					Component component = keyValuePair.Value.Target as Component;
					if (component == null)
					{
						Debug.LogErrorFormat("Component is null {0}", new object[]
						{
							keyValuePair.Key
						});
					}
					else if (component.gameObject == null)
					{
						Debug.LogErrorFormat("Component attached game object is null {0}", new object[]
						{
							keyValuePair.Key
						});
					}
				}
			}
		}
	}

	// Token: 0x0400049C RID: 1180
	public static ComponentReferenceManager Instance = new ComponentReferenceManager();

	// Token: 0x0400049D RID: 1181
	public Dictionary<string, WeakReference> refs = new Dictionary<string, WeakReference>();

	// Token: 0x0400049E RID: 1182
	public int count;
}
