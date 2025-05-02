using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x0200068A RID: 1674
	public static class TMPro_EventManager
	{
		// Token: 0x0600472C RID: 18220 RVA: 0x00038993 File Offset: 0x00036B93
		public static void ON_PRE_RENDER_OBJECT_CHANGED()
		{
			TMPro_EventManager.OnPreRenderObject_Event.Call();
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x0003899F File Offset: 0x00036B9F
		public static void ON_MATERIAL_PROPERTY_CHANGED(bool isChanged, Material mat)
		{
			TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Call(isChanged, mat);
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x000389AD File Offset: 0x00036BAD
		public static void ON_FONT_PROPERTY_CHANGED(bool isChanged, TMP_FontAsset font)
		{
			TMPro_EventManager.FONT_PROPERTY_EVENT.Call(isChanged, font);
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x000389BB File Offset: 0x00036BBB
		public static void ON_SPRITE_ASSET_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			TMPro_EventManager.SPRITE_ASSET_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x000389C9 File Offset: 0x00036BC9
		public static void ON_TEXTMESHPRO_PROPERTY_CHANGED(bool isChanged, TextMeshPro obj)
		{
			TMPro_EventManager.TEXTMESHPRO_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x000389D7 File Offset: 0x00036BD7
		public static void ON_DRAG_AND_DROP_MATERIAL_CHANGED(GameObject sender, Material currentMaterial, Material newMaterial)
		{
			TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Call(sender, currentMaterial, newMaterial);
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x000389E6 File Offset: 0x00036BE6
		public static void ON_TEXT_STYLE_PROPERTY_CHANGED(bool isChanged)
		{
			TMPro_EventManager.TEXT_STYLE_PROPERTY_EVENT.Call(isChanged);
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x000389F3 File Offset: 0x00036BF3
		public static void ON_TEXT_CHANGED(Object obj)
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Call(obj);
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x00038A00 File Offset: 0x00036C00
		public static void ON_TMP_SETTINGS_CHANGED()
		{
			TMPro_EventManager.TMP_SETTINGS_PROPERTY_EVENT.Call();
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x00038A0C File Offset: 0x00036C0C
		public static void ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(bool isChanged, TextMeshProUGUI obj)
		{
			TMPro_EventManager.TEXTMESHPRO_UGUI_PROPERTY_EVENT.Call(isChanged, obj);
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x00038A1A File Offset: 0x00036C1A
		public static void ON_BASE_MATERIAL_CHANGED(Material mat)
		{
			TMPro_EventManager.BASE_MATERIAL_EVENT.Call(mat);
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x00038A27 File Offset: 0x00036C27
		public static void ON_COMPUTE_DT_EVENT(object Sender, Compute_DT_EventArgs e)
		{
			TMPro_EventManager.COMPUTE_DT_EVENT.Call(Sender, e);
		}

		// Token: 0x0400370D RID: 14093
		public static readonly FastAction<object, Compute_DT_EventArgs> COMPUTE_DT_EVENT = new FastAction<object, Compute_DT_EventArgs>();

		// Token: 0x0400370E RID: 14094
		public static readonly FastAction<bool, Material> MATERIAL_PROPERTY_EVENT = new FastAction<bool, Material>();

		// Token: 0x0400370F RID: 14095
		public static readonly FastAction<bool, TMP_FontAsset> FONT_PROPERTY_EVENT = new FastAction<bool, TMP_FontAsset>();

		// Token: 0x04003710 RID: 14096
		public static readonly FastAction<bool, Object> SPRITE_ASSET_PROPERTY_EVENT = new FastAction<bool, Object>();

		// Token: 0x04003711 RID: 14097
		public static readonly FastAction<bool, TextMeshPro> TEXTMESHPRO_PROPERTY_EVENT = new FastAction<bool, TextMeshPro>();

		// Token: 0x04003712 RID: 14098
		public static readonly FastAction<GameObject, Material, Material> DRAG_AND_DROP_MATERIAL_EVENT = new FastAction<GameObject, Material, Material>();

		// Token: 0x04003713 RID: 14099
		public static readonly FastAction<bool> TEXT_STYLE_PROPERTY_EVENT = new FastAction<bool>();

		// Token: 0x04003714 RID: 14100
		public static readonly FastAction TMP_SETTINGS_PROPERTY_EVENT = new FastAction();

		// Token: 0x04003715 RID: 14101
		public static readonly FastAction<bool, TextMeshProUGUI> TEXTMESHPRO_UGUI_PROPERTY_EVENT = new FastAction<bool, TextMeshProUGUI>();

		// Token: 0x04003716 RID: 14102
		public static readonly FastAction<Material> BASE_MATERIAL_EVENT = new FastAction<Material>();

		// Token: 0x04003717 RID: 14103
		public static readonly FastAction OnPreRenderObject_Event = new FastAction();

		// Token: 0x04003718 RID: 14104
		public static readonly FastAction<Object> TEXT_CHANGED_EVENT = new FastAction<Object>();

		// Token: 0x04003719 RID: 14105
		public static readonly FastAction WILL_RENDER_CANVASES = new FastAction();
	}
}
