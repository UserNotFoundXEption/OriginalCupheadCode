using System;
using UnityEngine;

// Token: 0x02000065 RID: 101
public class GameObjectHelper
{
	// Token: 0x06000552 RID: 1362 RVA: 0x00005B76 File Offset: 0x00003D76
	public GameObjectHelper(string name)
	{
		this._gameObject = new GameObject("[Helper] " + name);
		this.events = this._gameObject.AddComponent<GameObjectHelperGO>();
	}

	// Token: 0x1700012C RID: 300
	// (get) Token: 0x06000553 RID: 1363 RVA: 0x00005BA5 File Offset: 0x00003DA5
	// (set) Token: 0x06000554 RID: 1364 RVA: 0x00005BAD File Offset: 0x00003DAD
	public GameObjectHelperGO events { get; set; }

	// Token: 0x06000555 RID: 1365 RVA: 0x00005BB6 File Offset: 0x00003DB6
	public void Destroy()
	{
		Object.Destroy(this._gameObject);
	}

	// Token: 0x06000556 RID: 1366 RVA: 0x00005BC3 File Offset: 0x00003DC3
	public void DontDestroyOnLoad()
	{
		Object.DontDestroyOnLoad(this._gameObject);
	}

	// Token: 0x04000496 RID: 1174
	public GameObject _gameObject;
}
