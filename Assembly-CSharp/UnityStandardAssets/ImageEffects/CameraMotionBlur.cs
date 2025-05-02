using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006B2 RID: 1714
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Camera Motion Blur")]
	public class CameraMotionBlur : PostEffectsBase
	{
		// Token: 0x06004799 RID: 18329 RVA: 0x0015D8C0 File Offset: 0x0015BAC0
		public void CalculateViewProjection()
		{
			Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
			Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
			this.currentViewProjMat = gpuprojectionMatrix * worldToCameraMatrix;
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x0015D8F8 File Offset: 0x0015BAF8
		public new void Start()
		{
			this.CheckResources();
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this.wasActive = base.gameObject.activeInHierarchy;
			this.CalculateViewProjection();
			this.Remember();
			this.wasActive = false;
		}

		// Token: 0x0600479B RID: 18331 RVA: 0x00038E5B File Offset: 0x0003705B
		public new void OnEnable()
		{
			if (this._camera == null)
			{
				this._camera = base.GetComponent<Camera>();
			}
			this._camera.depthTextureMode |= 1;
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x0015D950 File Offset: 0x0015BB50
		public void OnDisable()
		{
			if (null != this.motionBlurMaterial)
			{
				Object.DestroyImmediate(this.motionBlurMaterial);
				this.motionBlurMaterial = null;
			}
			if (null != this.dx11MotionBlurMaterial)
			{
				Object.DestroyImmediate(this.dx11MotionBlurMaterial);
				this.dx11MotionBlurMaterial = null;
			}
			if (null != this.tmpCam)
			{
				Object.DestroyImmediate(this.tmpCam);
				this.tmpCam = null;
			}
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x0015D9C8 File Offset: 0x0015BBC8
		public override bool CheckResources()
		{
			base.CheckSupport(true, true);
			this.motionBlurMaterial = base.CheckShaderAndCreateMaterial(this.shader, this.motionBlurMaterial);
			if (this.supportDX11 && this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11)
			{
				this.dx11MotionBlurMaterial = base.CheckShaderAndCreateMaterial(this.dx11MotionBlurShader, this.dx11MotionBlurMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x0015DA3C File Offset: 0x0015BC3C
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				this.StartFrame();
			}
			RenderTextureFormat renderTextureFormat = (!SystemInfo.SupportsRenderTextureFormat(13)) ? 2 : 13;
			RenderTexture temporary = RenderTexture.GetTemporary(CameraMotionBlur.divRoundUp(source.width, this.velocityDownsample), CameraMotionBlur.divRoundUp(source.height, this.velocityDownsample), 0, renderTextureFormat);
			this.maxVelocity = Mathf.Max(2f, this.maxVelocity);
			float num = this.maxVelocity;
			bool flag = this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && this.dx11MotionBlurMaterial == null;
			int num2;
			int num3;
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || flag || this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
			{
				this.maxVelocity = Mathf.Min(this.maxVelocity, CameraMotionBlur.MAX_RADIUS);
				num2 = CameraMotionBlur.divRoundUp(temporary.width, (int)this.maxVelocity);
				num3 = CameraMotionBlur.divRoundUp(temporary.height, (int)this.maxVelocity);
				num = (float)(temporary.width / num2);
			}
			else
			{
				num2 = CameraMotionBlur.divRoundUp(temporary.width, (int)this.maxVelocity);
				num3 = CameraMotionBlur.divRoundUp(temporary.height, (int)this.maxVelocity);
				num = (float)(temporary.width / num2);
			}
			RenderTexture temporary2 = RenderTexture.GetTemporary(num2, num3, 0, renderTextureFormat);
			RenderTexture temporary3 = RenderTexture.GetTemporary(num2, num3, 0, renderTextureFormat);
			temporary.filterMode = 0;
			temporary2.filterMode = 0;
			temporary3.filterMode = 0;
			if (this.noiseTexture)
			{
				this.noiseTexture.filterMode = 0;
			}
			source.wrapMode = 1;
			temporary.wrapMode = 1;
			temporary3.wrapMode = 1;
			temporary2.wrapMode = 1;
			this.CalculateViewProjection();
			if (base.gameObject.activeInHierarchy && !this.wasActive)
			{
				this.Remember();
			}
			this.wasActive = base.gameObject.activeInHierarchy;
			Matrix4x4 matrix4x = Matrix4x4.Inverse(this.currentViewProjMat);
			this.motionBlurMaterial.SetMatrix("_InvViewProj", matrix4x);
			this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
			this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x);
			this.motionBlurMaterial.SetFloat("_MaxVelocity", num);
			this.motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num);
			this.motionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
			this.motionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
			this.motionBlurMaterial.SetFloat("_Jitter", this.jitter);
			this.motionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
			this.motionBlurMaterial.SetTexture("_VelTex", temporary);
			this.motionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
			this.motionBlurMaterial.SetTexture("_TileTexDebug", temporary2);
			if (this.preview)
			{
				Matrix4x4 worldToCameraMatrix = this._camera.worldToCameraMatrix;
				Matrix4x4 identity = Matrix4x4.identity;
				identity.SetTRS(this.previewScale * 0.3333f, Quaternion.identity, Vector3.one);
				Matrix4x4 gpuprojectionMatrix = GL.GetGPUProjectionMatrix(this._camera.projectionMatrix, true);
				this.prevViewProjMat = gpuprojectionMatrix * identity * worldToCameraMatrix;
				this.motionBlurMaterial.SetMatrix("_PrevViewProj", this.prevViewProjMat);
				this.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", this.prevViewProjMat * matrix4x);
			}
			if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				Vector4 zero = Vector4.zero;
				float num4 = Vector3.Dot(base.transform.up, Vector3.up);
				Vector3 vector = this.prevFramePos - base.transform.position;
				float magnitude = vector.magnitude;
				float num5 = Vector3.Angle(base.transform.up, this.prevFrameUp) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.x = this.rotationScale * num5;
				num5 = Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.y = this.rotationScale * num4 * num5;
				num5 = Vector3.Angle(base.transform.forward, this.prevFrameForward) / this._camera.fieldOfView * ((float)source.width * 0.75f);
				zero.z = this.rotationScale * (1f - num4) * num5;
				if (magnitude > Mathf.Epsilon && this.movementScale > Mathf.Epsilon)
				{
					zero.w = this.movementScale * Vector3.Dot(base.transform.forward, vector) * ((float)source.width * 0.5f);
					zero.x += this.movementScale * Vector3.Dot(base.transform.up, vector) * ((float)source.width * 0.5f);
					zero.y += this.movementScale * Vector3.Dot(base.transform.right, vector) * ((float)source.width * 0.5f);
				}
				if (this.preview)
				{
					this.motionBlurMaterial.SetVector("_BlurDirectionPacked", new Vector4(this.previewScale.y, this.previewScale.x, 0f, this.previewScale.z) * 0.5f * this._camera.fieldOfView);
				}
				else
				{
					this.motionBlurMaterial.SetVector("_BlurDirectionPacked", zero);
				}
			}
			else
			{
				Graphics.Blit(source, temporary, this.motionBlurMaterial, 0);
				Camera camera = null;
				if (this.excludeLayers.value != 0)
				{
					camera = this.GetTmpCam();
				}
				if (camera && this.excludeLayers.value != 0 && this.replacementClear && this.replacementClear.isSupported)
				{
					camera.targetTexture = temporary;
					camera.cullingMask = this.excludeLayers;
					camera.RenderWithShader(this.replacementClear, string.Empty);
				}
			}
			if (!this.preview && Time.frameCount != this.prevFrameCount)
			{
				this.prevFrameCount = Time.frameCount;
				this.Remember();
			}
			source.filterMode = 1;
			if (this.showVelocity)
			{
				this.motionBlurMaterial.SetFloat("_DisplayVelocityScale", this.showVelocityScale);
				Graphics.Blit(temporary, destination, this.motionBlurMaterial, 1);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 && !flag)
			{
				this.dx11MotionBlurMaterial.SetFloat("_MinVelocity", this.minVelocity);
				this.dx11MotionBlurMaterial.SetFloat("_VelocityScale", this.velocityScale);
				this.dx11MotionBlurMaterial.SetFloat("_Jitter", this.jitter);
				this.dx11MotionBlurMaterial.SetTexture("_NoiseTex", this.noiseTexture);
				this.dx11MotionBlurMaterial.SetTexture("_VelTex", temporary);
				this.dx11MotionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3);
				this.dx11MotionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				this.dx11MotionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num);
				Graphics.Blit(temporary, temporary2, this.dx11MotionBlurMaterial, 0);
				Graphics.Blit(temporary2, temporary3, this.dx11MotionBlurMaterial, 1);
				Graphics.Blit(source, destination, this.dx11MotionBlurMaterial, 2);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.Reconstruction || flag)
			{
				this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				Graphics.Blit(temporary, temporary2, this.motionBlurMaterial, 2);
				Graphics.Blit(temporary2, temporary3, this.motionBlurMaterial, 3);
				Graphics.Blit(source, destination, this.motionBlurMaterial, 4);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.CameraMotion)
			{
				Graphics.Blit(source, destination, this.motionBlurMaterial, 6);
			}
			else if (this.filterType == CameraMotionBlur.MotionBlurFilter.ReconstructionDisc)
			{
				this.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025f, this.softZDistance));
				Graphics.Blit(temporary, temporary2, this.motionBlurMaterial, 2);
				Graphics.Blit(temporary2, temporary3, this.motionBlurMaterial, 3);
				Graphics.Blit(source, destination, this.motionBlurMaterial, 7);
			}
			else
			{
				Graphics.Blit(source, destination, this.motionBlurMaterial, 5);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x0015E308 File Offset: 0x0015C508
		public void Remember()
		{
			this.prevViewProjMat = this.currentViewProjMat;
			this.prevFrameForward = base.transform.forward;
			this.prevFrameUp = base.transform.up;
			this.prevFramePos = base.transform.position;
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x0015E354 File Offset: 0x0015C554
		public Camera GetTmpCam()
		{
			if (this.tmpCam == null)
			{
				string text = "_" + this._camera.name + "_MotionBlurTmpCam";
				GameObject gameObject = GameObject.Find(text);
				if (null == gameObject)
				{
					this.tmpCam = new GameObject(text, new Type[]
					{
						typeof(Camera)
					});
				}
				else
				{
					this.tmpCam = gameObject;
				}
			}
			this.tmpCam.hideFlags = 52;
			this.tmpCam.transform.position = this._camera.transform.position;
			this.tmpCam.transform.rotation = this._camera.transform.rotation;
			this.tmpCam.transform.localScale = this._camera.transform.localScale;
			this.tmpCam.GetComponent<Camera>().CopyFrom(this._camera);
			this.tmpCam.GetComponent<Camera>().enabled = false;
			this.tmpCam.GetComponent<Camera>().depthTextureMode = 0;
			this.tmpCam.GetComponent<Camera>().clearFlags = 4;
			return this.tmpCam.GetComponent<Camera>();
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x00038E8D File Offset: 0x0003708D
		public void StartFrame()
		{
			this.prevFramePos = Vector3.Slerp(this.prevFramePos, base.transform.position, 0.75f);
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x00038EB0 File Offset: 0x000370B0
		public static int divRoundUp(int x, int d)
		{
			return (x + d - 1) / d;
		}

		// Token: 0x040038A1 RID: 14497
		public static float MAX_RADIUS = 10f;

		// Token: 0x040038A2 RID: 14498
		public CameraMotionBlur.MotionBlurFilter filterType = CameraMotionBlur.MotionBlurFilter.Reconstruction;

		// Token: 0x040038A3 RID: 14499
		public bool preview;

		// Token: 0x040038A4 RID: 14500
		public Vector3 previewScale = Vector3.one;

		// Token: 0x040038A5 RID: 14501
		public float movementScale;

		// Token: 0x040038A6 RID: 14502
		public float rotationScale = 1f;

		// Token: 0x040038A7 RID: 14503
		public float maxVelocity = 8f;

		// Token: 0x040038A8 RID: 14504
		public float minVelocity = 0.1f;

		// Token: 0x040038A9 RID: 14505
		public float velocityScale = 0.375f;

		// Token: 0x040038AA RID: 14506
		public float softZDistance = 0.005f;

		// Token: 0x040038AB RID: 14507
		public int velocityDownsample = 1;

		// Token: 0x040038AC RID: 14508
		public LayerMask excludeLayers = 0;

		// Token: 0x040038AD RID: 14509
		public GameObject tmpCam;

		// Token: 0x040038AE RID: 14510
		public Shader shader;

		// Token: 0x040038AF RID: 14511
		public Shader dx11MotionBlurShader;

		// Token: 0x040038B0 RID: 14512
		public Shader replacementClear;

		// Token: 0x040038B1 RID: 14513
		public Material motionBlurMaterial;

		// Token: 0x040038B2 RID: 14514
		public Material dx11MotionBlurMaterial;

		// Token: 0x040038B3 RID: 14515
		public Texture2D noiseTexture;

		// Token: 0x040038B4 RID: 14516
		public float jitter = 0.05f;

		// Token: 0x040038B5 RID: 14517
		public bool showVelocity;

		// Token: 0x040038B6 RID: 14518
		public float showVelocityScale = 1f;

		// Token: 0x040038B7 RID: 14519
		public Matrix4x4 currentViewProjMat;

		// Token: 0x040038B8 RID: 14520
		public Matrix4x4 prevViewProjMat;

		// Token: 0x040038B9 RID: 14521
		public int prevFrameCount;

		// Token: 0x040038BA RID: 14522
		public bool wasActive;

		// Token: 0x040038BB RID: 14523
		public Vector3 prevFrameForward = Vector3.forward;

		// Token: 0x040038BC RID: 14524
		public Vector3 prevFrameUp = Vector3.up;

		// Token: 0x040038BD RID: 14525
		public Vector3 prevFramePos = Vector3.zero;

		// Token: 0x040038BE RID: 14526
		public Camera _camera;

		// Token: 0x02001307 RID: 4871
		public enum MotionBlurFilter
		{
			// Token: 0x0400821B RID: 33307
			CameraMotion,
			// Token: 0x0400821C RID: 33308
			LocalBlur,
			// Token: 0x0400821D RID: 33309
			Reconstruction,
			// Token: 0x0400821E RID: 33310
			ReconstructionDX11,
			// Token: 0x0400821F RID: 33311
			ReconstructionDisc
		}
	}
}
