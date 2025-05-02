using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000652 RID: 1618
	[AddComponentMenu("")]
	[RequireComponent(typeof(CanvasGroup))]
	public class Window : MonoBehaviour
	{
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06004382 RID: 17282 RVA: 0x00035D63 File Offset: 0x00033F63
		public bool hasFocus
		{
			get
			{
				return this._isFocusedCallback != null && this._isFocusedCallback(this._id);
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06004383 RID: 17283 RVA: 0x00035D87 File Offset: 0x00033F87
		public int id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06004384 RID: 17284 RVA: 0x00035D8F File Offset: 0x00033F8F
		public RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.gameObject.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06004385 RID: 17285 RVA: 0x00035DB9 File Offset: 0x00033FB9
		public Text titleText
		{
			get
			{
				return this._titleText;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06004386 RID: 17286 RVA: 0x00035DC1 File Offset: 0x00033FC1
		public List<Text> contentText
		{
			get
			{
				return this._contentText;
			}
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06004387 RID: 17287 RVA: 0x00035DC9 File Offset: 0x00033FC9
		// (set) Token: 0x06004388 RID: 17288 RVA: 0x00035DD1 File Offset: 0x00033FD1
		public GameObject defaultUIElement
		{
			get
			{
				return this._defaultUIElement;
			}
			set
			{
				this._defaultUIElement = value;
			}
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06004389 RID: 17289 RVA: 0x00035DDA File Offset: 0x00033FDA
		// (set) Token: 0x0600438A RID: 17290 RVA: 0x00035DE2 File Offset: 0x00033FE2
		public Action<int> updateCallback
		{
			get
			{
				return this._updateCallback;
			}
			set
			{
				this._updateCallback = value;
			}
		}

		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x0600438B RID: 17291 RVA: 0x00035DEB File Offset: 0x00033FEB
		public Window.Timer timer
		{
			get
			{
				return this._timer;
			}
		}

		// Token: 0x170005F1 RID: 1521
		// (get) Token: 0x0600438C RID: 17292 RVA: 0x00139A64 File Offset: 0x00137C64
		// (set) Token: 0x0600438D RID: 17293 RVA: 0x00139A88 File Offset: 0x00137C88
		public int width
		{
			get
			{
				return (int)this.rectTransform.sizeDelta.x;
			}
			set
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.x = (float)value;
				this.rectTransform.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x170005F2 RID: 1522
		// (get) Token: 0x0600438E RID: 17294 RVA: 0x00139AB8 File Offset: 0x00137CB8
		// (set) Token: 0x0600438F RID: 17295 RVA: 0x00139ADC File Offset: 0x00137CDC
		public int height
		{
			get
			{
				return (int)this.rectTransform.sizeDelta.y;
			}
			set
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.y = (float)value;
				this.rectTransform.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x170005F3 RID: 1523
		// (get) Token: 0x06004390 RID: 17296 RVA: 0x00035DF3 File Offset: 0x00033FF3
		public bool initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x00035DFB File Offset: 0x00033FFB
		public void OnEnable()
		{
			base.StartCoroutine("OnEnableAsync");
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x00035E09 File Offset: 0x00034009
		public virtual void Update()
		{
			if (!this._initialized)
			{
				return;
			}
			if (!this.hasFocus)
			{
				return;
			}
			this.CheckUISelection();
			if (this._updateCallback != null)
			{
				this._updateCallback(this._id);
			}
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x00139B0C File Offset: 0x00137D0C
		public virtual void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this._initialized)
			{
				Debug.LogError("Window is already initialized!");
				return;
			}
			this._id = id;
			this._isFocusedCallback = isFocusedCallback;
			this._timer = new Window.Timer();
			this._contentText = new List<Text>();
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._initialized = true;
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x00035E45 File Offset: 0x00034045
		public void SetSize(int width, int height)
		{
			this.rectTransform.sizeDelta = new Vector2((float)width, (float)height);
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x00035E5B File Offset: 0x0003405B
		public void CreateTitleText(GameObject prefab, Vector2 offset)
		{
			this.CreateText(prefab, ref this._titleText, "Title Text", UIPivot.TopCenter, UIAnchor.TopHStretch, offset);
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x00035E7A File Offset: 0x0003407A
		public void CreateTitleText(GameObject prefab, Vector2 offset, string text)
		{
			this.CreateTitleText(prefab, offset);
			this.SetTitleText(text);
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x00139B68 File Offset: 0x00137D68
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			Text item = null;
			this.CreateText(prefab, ref item, "Content Text", pivot, anchor, offset);
			this._contentText.Add(item);
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x00035E8B File Offset: 0x0003408B
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text)
		{
			this.AddContentText(prefab, pivot, anchor, offset, text, 0);
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x00035E9B File Offset: 0x0003409B
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text, int fontSize)
		{
			this.AddContentText(prefab, pivot, anchor, offset);
			this.SetContentText(text, fontSize, this._contentText.Count - 1);
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x00035EBF File Offset: 0x000340BF
		public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			this.CreateImage(prefab, "Image", pivot, anchor, offset);
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x00035ED1 File Offset: 0x000340D1
		public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text)
		{
			this.AddContentImage(prefab, pivot, anchor, offset);
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x00139B98 File Offset: 0x00137D98
		public void CreateButton(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string buttonText, UnityAction confirmCallback, UnityAction cancelCallback, bool setDefault)
		{
			if (prefab == null)
			{
				return;
			}
			ButtonInfo buttonInfo;
			GameObject gameObject = this.CreateButton(prefab, "Button", anchor, pivot, offset, out buttonInfo);
			if (gameObject == null)
			{
				return;
			}
			Button component = gameObject.GetComponent<Button>();
			if (confirmCallback != null)
			{
				component.onClick.AddListener(confirmCallback);
			}
			CustomButton customButton = component as CustomButton;
			if (cancelCallback != null && customButton != null)
			{
				customButton.CancelEvent += cancelCallback;
			}
			if (buttonInfo.text != null)
			{
				buttonInfo.text.text = buttonText;
			}
			if (setDefault)
			{
				this._defaultUIElement = gameObject;
			}
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x00035EDE File Offset: 0x000340DE
		public string GetTitleText(string text)
		{
			if (this._titleText == null)
			{
				return string.Empty;
			}
			return this._titleText.text;
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x00035F02 File Offset: 0x00034102
		public void SetTitleText(string text)
		{
			if (this._titleText == null)
			{
				return;
			}
			this._titleText.text = text;
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x00139C3C File Offset: 0x00137E3C
		public string GetContentText(int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return string.Empty;
			}
			return this._contentText[index].text;
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x00139C94 File Offset: 0x00137E94
		public float GetContentTextHeight(int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return 0f;
			}
			return this._contentText[index].rectTransform.sizeDelta.y;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x00035F22 File Offset: 0x00034122
		public void SetContentText(string text, int fontsize, int index)
		{
			this.SetContentText(text, index);
			if (fontsize > 0)
			{
				this._contentText[index].fontSize = fontsize;
			}
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x00139CF8 File Offset: 0x00137EF8
		public void SetContentText(string text, int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return;
			}
			this._contentText[index].text = text;
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x00035F45 File Offset: 0x00034145
		public void SetUpdateCallback(Action<int> callback)
		{
			this.updateCallback = callback;
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x00035F4E File Offset: 0x0003414E
		public virtual void TakeInputFocus()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(this._defaultUIElement);
			this.Enable();
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x00035F77 File Offset: 0x00034177
		public virtual void Enable()
		{
			this._canvasGroup.interactable = true;
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x00035F85 File Offset: 0x00034185
		public virtual void Disable()
		{
			this._canvasGroup.interactable = false;
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x00035F93 File Offset: 0x00034193
		public virtual void Cancel()
		{
			if (!this.initialized)
			{
				return;
			}
			if (this.cancelCallback != null)
			{
				this.cancelCallback.Invoke();
			}
		}

		// Token: 0x060043A8 RID: 17320 RVA: 0x00139D4C File Offset: 0x00137F4C
		public void CreateText(GameObject prefab, ref Text textComponent, string name, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			if (prefab == null || this.content == null)
			{
				return;
			}
			if (textComponent != null)
			{
				Debug.LogError("Window already has " + name + "!");
				return;
			}
			GameObject gameObject = UITools.InstantiateGUIObject<Text>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
			if (gameObject == null)
			{
				return;
			}
			textComponent = gameObject.GetComponent<Text>();
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x00139DDC File Offset: 0x00137FDC
		public void CreateImage(GameObject prefab, string name, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			if (prefab == null || this.content == null)
			{
				return;
			}
			UITools.InstantiateGUIObject<Image>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x00139E30 File Offset: 0x00138030
		public GameObject CreateButton(GameObject prefab, string name, UIAnchor anchor, UIPivot pivot, Vector2 offset, out ButtonInfo buttonInfo)
		{
			buttonInfo = null;
			if (prefab == null)
			{
				return null;
			}
			GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
			if (gameObject == null)
			{
				return null;
			}
			buttonInfo = gameObject.GetComponent<ButtonInfo>();
			Button component = gameObject.GetComponent<Button>();
			if (component == null)
			{
				Debug.Log("Button prefab is missing Button component!");
				return null;
			}
			if (buttonInfo == null)
			{
				Debug.Log("Button prefab is missing ButtonInfo component!");
				return null;
			}
			return gameObject;
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x00139ECC File Offset: 0x001380CC
		public IEnumerator OnEnableAsync()
		{
			yield return 1;
			if (EventSystem.current == null)
			{
				yield break;
			}
			if (this.defaultUIElement != null)
			{
				EventSystem.current.SetSelectedGameObject(this.defaultUIElement);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			yield break;
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x00139EE8 File Offset: 0x001380E8
		public void CheckUISelection()
		{
			if (!this.hasFocus)
			{
				return;
			}
			if (EventSystem.current == null)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				this.RestoreDefaultOrLastUISelection();
			}
			this.lastUISelection = EventSystem.current.currentSelectedGameObject;
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x00139F40 File Offset: 0x00138140
		public void RestoreDefaultOrLastUISelection()
		{
			if (!this.hasFocus)
			{
				return;
			}
			if (this.lastUISelection == null || !this.lastUISelection.activeInHierarchy)
			{
				this.SetUISelection(this._defaultUIElement);
				return;
			}
			this.SetUISelection(this.lastUISelection);
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x00035FB7 File Offset: 0x000341B7
		public void SetUISelection(GameObject selection)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(selection);
		}

		// Token: 0x040034EC RID: 13548
		public Image backgroundImage;

		// Token: 0x040034ED RID: 13549
		public GameObject content;

		// Token: 0x040034EE RID: 13550
		public bool _initialized;

		// Token: 0x040034EF RID: 13551
		public int _id = -1;

		// Token: 0x040034F0 RID: 13552
		public RectTransform _rectTransform;

		// Token: 0x040034F1 RID: 13553
		public Text _titleText;

		// Token: 0x040034F2 RID: 13554
		public List<Text> _contentText;

		// Token: 0x040034F3 RID: 13555
		public GameObject _defaultUIElement;

		// Token: 0x040034F4 RID: 13556
		public Action<int> _updateCallback;

		// Token: 0x040034F5 RID: 13557
		public Func<int, bool> _isFocusedCallback;

		// Token: 0x040034F6 RID: 13558
		public Window.Timer _timer;

		// Token: 0x040034F7 RID: 13559
		public CanvasGroup _canvasGroup;

		// Token: 0x040034F8 RID: 13560
		public UnityAction cancelCallback;

		// Token: 0x040034F9 RID: 13561
		public GameObject lastUISelection;

		// Token: 0x020012E5 RID: 4837
		public class Timer
		{
			// Token: 0x170019CD RID: 6605
			// (get) Token: 0x060083A0 RID: 33696 RVA: 0x00057BEC File Offset: 0x00055DEC
			public bool started
			{
				get
				{
					return this._started;
				}
			}

			// Token: 0x170019CE RID: 6606
			// (get) Token: 0x060083A1 RID: 33697 RVA: 0x00057BF4 File Offset: 0x00055DF4
			public bool finished
			{
				get
				{
					if (!this.started)
					{
						return false;
					}
					if (Time.realtimeSinceStartup < this.end)
					{
						return false;
					}
					this._started = false;
					return true;
				}
			}

			// Token: 0x170019CF RID: 6607
			// (get) Token: 0x060083A2 RID: 33698 RVA: 0x00057C1D File Offset: 0x00055E1D
			public float remaining
			{
				get
				{
					if (!this._started)
					{
						return 0f;
					}
					return this.end - Time.realtimeSinceStartup;
				}
			}

			// Token: 0x060083A3 RID: 33699 RVA: 0x00057C3C File Offset: 0x00055E3C
			public void Start(float length)
			{
				this.end = Time.realtimeSinceStartup + length;
				this._started = true;
			}

			// Token: 0x060083A4 RID: 33700 RVA: 0x00057C52 File Offset: 0x00055E52
			public void Stop()
			{
				this._started = false;
			}

			// Token: 0x040081AF RID: 33199
			public bool _started;

			// Token: 0x040081B0 RID: 33200
			public float end;
		}
	}
}
