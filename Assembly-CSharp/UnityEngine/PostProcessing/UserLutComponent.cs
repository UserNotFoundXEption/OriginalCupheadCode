using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000615 RID: 1557
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06004071 RID: 16497 RVA: 0x0012E2F4 File Offset: 0x0012C4F4
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x06004072 RID: 16498 RVA: 0x0012E378 File Offset: 0x0012C578
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x0012E400 File Offset: 0x0012C600
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			Rect rect;
			rect..ctor(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height);
			GUI.DrawTexture(rect, settings.lut);
		}

		// Token: 0x02001280 RID: 4736
		public static class Uniforms
		{
			// Token: 0x04007FE0 RID: 32736
			public static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x04007FE1 RID: 32737
			public static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
