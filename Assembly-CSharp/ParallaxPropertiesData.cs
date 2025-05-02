using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000453 RID: 1107
public class ParallaxPropertiesData : ScriptableObject
{
	// Token: 0x17000364 RID: 868
	// (get) Token: 0x06002F5A RID: 12122 RVA: 0x000276CE File Offset: 0x000258CE
	public static ParallaxPropertiesData Instance
	{
		get
		{
			if (ParallaxPropertiesData._instance == null)
			{
				ParallaxPropertiesData._instance = Resources.Load<ParallaxPropertiesData>("Parallax/data");
			}
			return ParallaxPropertiesData._instance;
		}
	}

	// Token: 0x17000365 RID: 869
	// (get) Token: 0x06002F5B RID: 12123 RVA: 0x000276F4 File Offset: 0x000258F4
	public List<ParallaxPropertiesData.ThemeProperties> Properties
	{
		get
		{
			return this._properties;
		}
	}

	// Token: 0x06002F5C RID: 12124 RVA: 0x000276FC File Offset: 0x000258FC
	public ParallaxPropertiesData.ThemeProperties.Layer GetProperty(PlatformingLevel.Theme theme, int layer, PlatformingLevelParallax.Sides side)
	{
		if (side == PlatformingLevelParallax.Sides.Background || side != PlatformingLevelParallax.Sides.Foreground)
		{
			return this.GetTheme(theme).background.GetLayer(layer);
		}
		return this.GetTheme(theme).foreground.GetLayer(layer);
	}

	// Token: 0x06002F5D RID: 12125 RVA: 0x000E18C8 File Offset: 0x000DFAC8
	public ParallaxPropertiesData.ThemeProperties GetTheme(PlatformingLevel.Theme theme)
	{
		foreach (ParallaxPropertiesData.ThemeProperties themeProperties in this._properties)
		{
			if (themeProperties.theme == theme)
			{
				return themeProperties;
			}
		}
		return null;
	}

	// Token: 0x04002746 RID: 10054
	public const string PATH = "Parallax/data";

	// Token: 0x04002747 RID: 10055
	public const int LAYER_COUNT = 20;

	// Token: 0x04002748 RID: 10056
	public static ParallaxPropertiesData _instance;

	// Token: 0x04002749 RID: 10057
	[SerializeField]
	public List<ParallaxPropertiesData.ThemeProperties> _properties;

	// Token: 0x020010CE RID: 4302
	[Serializable]
	public class ThemeProperties
	{
		// Token: 0x06007B5B RID: 31579 RVA: 0x000531ED File Offset: 0x000513ED
		public ThemeProperties()
		{
			this.background = new ParallaxPropertiesData.ThemeProperties.LayerGroup();
			this.background.InvertSortingOrder();
			this.foreground = new ParallaxPropertiesData.ThemeProperties.LayerGroup();
			this.foreground.InvertSpeed();
		}

		// Token: 0x06007B5C RID: 31580 RVA: 0x00053221 File Offset: 0x00051421
		public ThemeProperties(PlatformingLevel.Theme theme)
		{
			this.theme = theme;
			this.background = new ParallaxPropertiesData.ThemeProperties.LayerGroup();
			this.background.InvertSortingOrder();
			this.foreground = new ParallaxPropertiesData.ThemeProperties.LayerGroup();
			this.foreground.InvertSpeed();
		}

		// Token: 0x04007735 RID: 30517
		public PlatformingLevel.Theme theme;

		// Token: 0x04007736 RID: 30518
		public ParallaxPropertiesData.ThemeProperties.LayerGroup background;

		// Token: 0x04007737 RID: 30519
		public ParallaxPropertiesData.ThemeProperties.LayerGroup foreground;

		// Token: 0x04007738 RID: 30520
		[NonSerialized]
		public bool zEditor_expanded;

		// Token: 0x020015DE RID: 5598
		[Serializable]
		public class LayerGroup
		{
			// Token: 0x06008765 RID: 34661 RVA: 0x002A5120 File Offset: 0x002A3320
			public LayerGroup()
			{
				this.layers = new ParallaxPropertiesData.ThemeProperties.Layer[20];
				for (int i = 0; i < this.layers.Length; i++)
				{
					this.layers[i] = new ParallaxPropertiesData.ThemeProperties.Layer();
					this.layers[i].speed = 0.05f * (float)(i + 1);
					this.layers[i].sortingOrder = 100 * (i + 1);
				}
			}

			// Token: 0x06008766 RID: 34662 RVA: 0x002A5190 File Offset: 0x002A3390
			public void InvertSpeed()
			{
				foreach (ParallaxPropertiesData.ThemeProperties.Layer layer in this.layers)
				{
					layer.speed *= -1f;
				}
			}

			// Token: 0x06008767 RID: 34663 RVA: 0x002A51D0 File Offset: 0x002A33D0
			public void InvertSortingOrder()
			{
				foreach (ParallaxPropertiesData.ThemeProperties.Layer layer in this.layers)
				{
					layer.sortingOrder *= -1;
				}
			}

			// Token: 0x06008768 RID: 34664 RVA: 0x0005BAC1 File Offset: 0x00059CC1
			public ParallaxPropertiesData.ThemeProperties.Layer GetLayer(int layer)
			{
				return this.layers[layer];
			}

			// Token: 0x040091DA RID: 37338
			public ParallaxPropertiesData.ThemeProperties.Layer[] layers;

			// Token: 0x040091DB RID: 37339
			[NonSerialized]
			public bool zEditor_expanded;
		}

		// Token: 0x020015DF RID: 5599
		[Serializable]
		public class Layer
		{
			// Token: 0x040091DC RID: 37340
			public float speed;

			// Token: 0x040091DD RID: 37341
			public int sortingOrder;
		}
	}
}
