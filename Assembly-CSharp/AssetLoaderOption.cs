using System;

// Token: 0x0200008A RID: 138
public class AssetLoaderOption
{
	// Token: 0x06000691 RID: 1681 RVA: 0x00006B29 File Offset: 0x00004D29
	public AssetLoaderOption(AssetLoaderOption.Type option, object context)
	{
		this.type = option;
		this.context = context;
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000692 RID: 1682 RVA: 0x00006B3F File Offset: 0x00004D3F
	// (set) Token: 0x06000693 RID: 1683 RVA: 0x00006B47 File Offset: 0x00004D47
	public AssetLoaderOption.Type type { get; set; }

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000694 RID: 1684 RVA: 0x00006B50 File Offset: 0x00004D50
	// (set) Token: 0x06000695 RID: 1685 RVA: 0x00006B58 File Offset: 0x00004D58
	public object context { get; set; }

	// Token: 0x06000696 RID: 1686 RVA: 0x00006B61 File Offset: 0x00004D61
	public static AssetLoaderOption None()
	{
		return new AssetLoaderOption(AssetLoaderOption.Type.None, null);
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x00006B6A File Offset: 0x00004D6A
	public static AssetLoaderOption PersistInCache()
	{
		return new AssetLoaderOption(AssetLoaderOption.Type.PersistInCache, null);
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x00006B73 File Offset: 0x00004D73
	public static AssetLoaderOption DontDestroyOnUnload()
	{
		return new AssetLoaderOption(AssetLoaderOption.Type.DontDestroyOnUnload, null);
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00006B7C File Offset: 0x00004D7C
	public static AssetLoaderOption PersistInCacheTagged(string tag)
	{
		return new AssetLoaderOption(AssetLoaderOption.Type.PersistInCacheTagged, tag);
	}

	// Token: 0x020008CC RID: 2252
	[Flags]
	public enum Type
	{
		// Token: 0x04004351 RID: 17233
		None = 0,
		// Token: 0x04004352 RID: 17234
		PersistInCache = 1,
		// Token: 0x04004353 RID: 17235
		DontDestroyOnUnload = 2,
		// Token: 0x04004354 RID: 17236
		PersistInCacheTagged = 4
	}
}
