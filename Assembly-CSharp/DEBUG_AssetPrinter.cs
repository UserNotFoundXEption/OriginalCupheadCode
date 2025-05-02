using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;

// Token: 0x020005C4 RID: 1476
public class DEBUG_AssetPrinter : MonoBehaviour
{
	// Token: 0x06003DD6 RID: 15830 RVA: 0x00031C66 File Offset: 0x0002FE66
	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06003DD7 RID: 15831 RVA: 0x0011A34C File Offset: 0x0011854C
	public void OnGUI()
	{
		GUIStyle guistyle = new GUIStyle(GUI.skin.GetStyle("Box"));
		guistyle.alignment = 0;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("Load Operations: " + AssetBundleLoader.loadCounter);
		IList list = AssetBundleLoader.DEBUG_LoadedAssetBundles();
		stringBuilder.AppendFormat("=== AssetBundles ({0}) ===\n", list.Count);
		IEnumerator enumerator = list.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				string value = (string)obj;
				stringBuilder.AppendLine(value);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		IEnumerable<AssetBundle> allLoadedAssetBundles = AssetBundle.GetAllLoadedAssetBundles();
		int num = 0;
		foreach (AssetBundle assetBundle in AssetBundle.GetAllLoadedAssetBundles())
		{
			num++;
		}
		stringBuilder.AppendFormat("=== System AssetBundles ({0}) ===\n", num);
		foreach (AssetBundle assetBundle2 in AssetBundle.GetAllLoadedAssetBundles())
		{
			stringBuilder.AppendLine(assetBundle2.name);
		}
		GUI.Box(new Rect(0f, 0f, 400f, (float)Screen.height), stringBuilder.ToString());
		stringBuilder.Length = 0;
		list = AssetLoader<SpriteAtlas>.DEBUG_GetLoadedAssets();
		stringBuilder.AppendFormat("=== Cached SpriteAtlases ({0}) ===\n", list.Count);
		IEnumerator enumerator4 = list.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext())
			{
				object obj2 = enumerator4.Current;
				string value2 = (string)obj2;
				stringBuilder.AppendLine(value2);
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator4 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		list = AssetLoader<AudioClip>.DEBUG_GetLoadedAssets();
		stringBuilder.AppendFormat("=== Cached Music ({0}) ===\n", list.Count);
		IEnumerator enumerator5 = list.GetEnumerator();
		try
		{
			while (enumerator5.MoveNext())
			{
				object obj3 = enumerator5.Current;
				string value3 = (string)obj3;
				stringBuilder.AppendLine(value3);
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator5 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		list = AssetLoader<Texture2D[]>.DEBUG_GetLoadedAssets();
		stringBuilder.AppendFormat("=== Cached Textures ({0}) ===\n", list.Count);
		IEnumerator enumerator6 = list.GetEnumerator();
		try
		{
			while (enumerator6.MoveNext())
			{
				object obj4 = enumerator6.Current;
				string value4 = (string)obj4;
				stringBuilder.AppendLine(value4);
			}
		}
		finally
		{
			IDisposable disposable4;
			if ((disposable4 = (enumerator6 as IDisposable)) != null)
			{
				disposable4.Dispose();
			}
		}
		GUI.Box(new Rect(400f, 0f, 400f, (float)Screen.height), stringBuilder.ToString());
		stringBuilder.Length = 0;
		list = Resources.FindObjectsOfTypeAll<SpriteAtlas>();
		stringBuilder.AppendFormat("=== System SpriteAtlases ({0}) ===\n", list.Count);
		IEnumerator enumerator7 = list.GetEnumerator();
		try
		{
			while (enumerator7.MoveNext())
			{
				object obj5 = enumerator7.Current;
				stringBuilder.AppendLine(((SpriteAtlas)obj5).name);
			}
		}
		finally
		{
			IDisposable disposable5;
			if ((disposable5 = (enumerator7 as IDisposable)) != null)
			{
				disposable5.Dispose();
			}
		}
		GUI.Box(new Rect(800f, 0f, 400f, (float)Screen.height), stringBuilder.ToString());
	}
}
