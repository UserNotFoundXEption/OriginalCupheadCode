using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005FE RID: 1534
public class LegacyClassicRpgGui : MonoBehaviour
{
	// Token: 0x06003FC6 RID: 16326 RVA: 0x000333D1 File Offset: 0x000315D1
	public void Awake()
	{
		Dialoguer.Initialize();
	}

	// Token: 0x06003FC7 RID: 16327 RVA: 0x000333D8 File Offset: 0x000315D8
	public void Start()
	{
		this.addDialoguerEvents();
		this._showDialogueBox = false;
	}

	// Token: 0x06003FC8 RID: 16328 RVA: 0x00128EF4 File Offset: 0x001270F4
	public void Update()
	{
		if (!this._dialogue)
		{
			return;
		}
		if (this._windowReady)
		{
			this.calculateText();
		}
		if (!this._dialogue || this._ending)
		{
			return;
		}
		if (!this._isBranchedText)
		{
			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(120) || Input.GetKeyDown(32) || Input.GetKeyDown(13))
			{
				if (this._windowCurrentText == this._windowTargetText)
				{
					Dialoguer.ContinueDialogue(0);
				}
				else
				{
					this._windowCurrentText = this._windowTargetText;
					this.audioTextEnd.Play();
				}
			}
		}
		else
		{
			if (Input.GetKeyDown(274))
			{
				this._currentChoice = (int)Mathf.Repeat((float)(this._currentChoice + 1), (float)this._branchedTextChoices.Length);
				this.audioText.Play();
			}
			if (Input.GetKeyDown(273))
			{
				this._currentChoice = (int)Mathf.Repeat((float)(this._currentChoice - 1), (float)this._branchedTextChoices.Length);
				this.audioText.Play();
			}
			if (Input.GetMouseButtonDown(0) && this._windowCurrentText != this._windowTargetText)
			{
				this._windowCurrentText = this._windowTargetText;
			}
			if (Input.GetKeyDown(120) || Input.GetKeyDown(32) || Input.GetKeyDown(13))
			{
				if (this._windowCurrentText == this._windowTargetText)
				{
					Dialoguer.ContinueDialogue(this._currentChoice);
				}
				else
				{
					this._windowCurrentText = this._windowTargetText;
					this.audioTextEnd.Play();
				}
			}
		}
	}

	// Token: 0x06003FC9 RID: 16329 RVA: 0x001290AC File Offset: 0x001272AC
	public void addDialoguerEvents()
	{
		Dialoguer.events.onStarted += this.onDialogueStartedHandler;
		Dialoguer.events.onEnded += this.onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.onDialogueInstantlyEndedHandler;
		Dialoguer.events.onTextPhase += this.onDialogueTextPhaseHandler;
		Dialoguer.events.onWindowClose += this.onDialogueWindowCloseHandler;
		Dialoguer.events.onMessageEvent += this.onDialoguerMessageEvent;
	}

	// Token: 0x06003FCA RID: 16330 RVA: 0x000333E7 File Offset: 0x000315E7
	public void onDialogueStartedHandler()
	{
		this._dialogue = true;
	}

	// Token: 0x06003FCB RID: 16331 RVA: 0x000333F0 File Offset: 0x000315F0
	public void onDialogueEndedHandler()
	{
		this._ending = true;
		this.audioTextEnd.Play();
	}

	// Token: 0x06003FCC RID: 16332 RVA: 0x00033404 File Offset: 0x00031604
	public void onDialogueInstantlyEndedHandler()
	{
		this._dialogue = false;
		this._showDialogueBox = false;
		this.resetWindowSize();
	}

	// Token: 0x06003FCD RID: 16333 RVA: 0x00129140 File Offset: 0x00127340
	public void onDialogueTextPhaseHandler(DialoguerTextData data)
	{
		this._usingPositionRect = data.usingPositionRect;
		this._positionRect = data.rect;
		this._windowCurrentText = string.Empty;
		this._windowTargetText = data.text;
		this._nameText = data.name;
		this._showDialogueBox = true;
		this._isBranchedText = (data.windowType == DialoguerTextPhaseType.BranchedText);
		this._branchedTextChoices = data.choices;
		this._currentChoice = 0;
		if (data.theme != this._theme)
		{
			this.resetWindowSize();
		}
		this._theme = data.theme;
		this.startWindowTweenIn();
	}

	// Token: 0x06003FCE RID: 16334 RVA: 0x0003341A File Offset: 0x0003161A
	public void onDialogueWindowCloseHandler()
	{
		this.startWindowTweenOut();
	}

	// Token: 0x06003FCF RID: 16335 RVA: 0x00033422 File Offset: 0x00031622
	public void onDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "playOldRpgSound")
		{
			this.playOldRpgSound(metadata);
		}
	}

	// Token: 0x06003FD0 RID: 16336 RVA: 0x001291E8 File Offset: 0x001273E8
	public void OnGUI()
	{
		if (!this._showDialogueBox)
		{
			return;
		}
		GUI.skin = this.skin;
		GUI.depth = 10;
		float num = this._usingPositionRect ? this._positionRect.x : ((float)Screen.width * 0.5f);
		float num2 = this._usingPositionRect ? this._positionRect.y : ((float)(Screen.height - 100));
		float num3 = this._usingPositionRect ? this._positionRect.width : 512f;
		float num4 = this._usingPositionRect ? this._positionRect.height : 190f;
		Rect rect = this.centerRect(new Rect(num, num2, num3 * this._windowTweenValue, num4 * this._windowTweenValue));
		rect.width = Mathf.Clamp(rect.width, 32f, 2000f);
		rect.height = Mathf.Clamp(rect.height, 32f, 2000f);
		if (this._theme == "good")
		{
			this.drawDialogueBox(rect, new Color(0.2f, 0.8f, 0.4f));
		}
		else if (this._theme == "bad")
		{
			this.drawDialogueBox(rect, new Color(0.8f, 0.2f, 0.2f));
		}
		else
		{
			this.drawDialogueBox(rect);
		}
		if (this._nameText != string.Empty)
		{
			Rect rect2;
			rect2..ctor(rect.x, rect.y - 60f, 150f * this._windowTweenValue, 50f * this._windowTweenValue);
			rect2.width = Mathf.Clamp(rect2.width, 32f, 2000f);
			rect2.height = Mathf.Clamp(rect2.height, 32f, 2000f);
			this.drawDialogueBox(rect2);
			this.drawShadowedText(new Rect(rect2.x + 15f * this._windowTweenValue - 5f * (1f - this._windowTweenValue), rect2.y + 5f * this._windowTweenValue - 10f * (1f - this._windowTweenValue), rect2.width - 30f * this._windowTweenValue, rect2.height - 5f * this._windowTweenValue), this._nameText);
		}
		Rect rect3;
		rect3..ctor(rect.x + 20f * this._windowTweenValue, rect.y + 10f * this._windowTweenValue, rect.width - 40f * this._windowTweenValue, rect.height - 20f * this._windowTweenValue);
		this.drawShadowedText(rect3, this._windowCurrentText);
		if (this._isBranchedText && this._windowCurrentText == this._windowTargetText && this._branchedTextChoices != null)
		{
			for (int i = 0; i < this._branchedTextChoices.Length; i++)
			{
				float num5 = rect.yMax - (float)(38 * this._branchedTextChoices.Length - 38 * i) - 20f;
				Rect rect4;
				rect4..ctor(rect.x + 60f, num5, rect.width - 80f, 38f);
				this.drawShadowedText(rect4, this._branchedTextChoices[i]);
				if (rect4.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)))
				{
					if (this._currentChoice != i)
					{
						this.audioText.Play();
						this._currentChoice = i;
					}
					if (Input.GetMouseButtonDown(0))
					{
						Dialoguer.ContinueDialogue(this._currentChoice);
						break;
					}
				}
				if (this._currentChoice == i)
				{
					GUI.Box(new Rect(rect4.x - 64f, rect4.y, 64f, 64f), string.Empty, GUI.skin.GetStyle("box_cursor"));
				}
			}
		}
	}

	// Token: 0x06003FD1 RID: 16337 RVA: 0x0003343B File Offset: 0x0003163B
	public void drawDialogueBox(Rect rect)
	{
		this.drawDialogueBox(rect, new Color(0.1764706f, 0.435294122f, 1f));
	}

	// Token: 0x06003FD2 RID: 16338 RVA: 0x00129644 File Offset: 0x00127844
	public void drawDialogueBox(Rect rect, Color color)
	{
		GUI.color = color;
		GUI.Box(rect, string.Empty, GUI.skin.GetStyle("box_background"));
		GUI.color = GUI.contentColor;
		GUI.color = new Color(0f, 0f, 0f, 0.25f);
		Rect rect2;
		rect2..ctor(rect.x + 7f, rect.y + 7f, rect.width - 14f, rect.height - 14f);
		GUI.DrawTextureWithTexCoords(rect2, this.diagonalLines, new Rect(0f, 0f, rect2.width / (float)this.diagonalLines.width, rect2.height / (float)this.diagonalLines.height));
		GUI.color = GUI.contentColor;
		GUI.depth = 20;
		GUI.Box(rect, string.Empty, GUI.skin.GetStyle("box_border"));
		GUI.depth = 10;
	}

	// Token: 0x06003FD3 RID: 16339 RVA: 0x0012974C File Offset: 0x0012794C
	public void drawShadowedText(Rect rect, string text)
	{
		GUI.color = new Color(0f, 0f, 0f, 0.5f);
		GUI.Label(new Rect(rect.x + 1f, rect.y + 2f, rect.width, rect.height), text);
		GUI.color = GUI.contentColor;
		GUI.Label(rect, text);
	}

	// Token: 0x06003FD4 RID: 16340 RVA: 0x00033458 File Offset: 0x00031658
	public void playOldRpgSound(string metadata)
	{
		if (metadata == "good")
		{
			this.audioGood.Play();
		}
		else if (metadata == "bad")
		{
			this.audioBad.Play();
		}
	}

	// Token: 0x06003FD5 RID: 16341 RVA: 0x00033495 File Offset: 0x00031695
	public void resetWindowSize()
	{
		this._windowTweenValue = 0f;
		this._windowReady = false;
	}

	// Token: 0x06003FD6 RID: 16342 RVA: 0x001297BC File Offset: 0x001279BC
	public void startWindowTweenIn()
	{
		this._showDialogueBox = true;
		DialogueriTween.ValueTo(base.gameObject, new Hashtable
		{
			{
				"from",
				this._windowTweenValue
			},
			{
				"to",
				1
			},
			{
				"onupdatetarget",
				base.gameObject
			},
			{
				"onupdate",
				"updateWindowTweenValue"
			},
			{
				"oncompletetarget",
				base.gameObject
			},
			{
				"oncomplete",
				"windowInComplete"
			},
			{
				"time",
				0.5f
			},
			{
				"easetype",
				DialogueriTween.EaseType.easeOutBack
			}
		});
	}

	// Token: 0x06003FD7 RID: 16343 RVA: 0x00129874 File Offset: 0x00127A74
	public void startWindowTweenOut()
	{
		this._windowReady = false;
		DialogueriTween.ValueTo(base.gameObject, new Hashtable
		{
			{
				"from",
				this._windowTweenValue
			},
			{
				"to",
				0
			},
			{
				"onupdatetarget",
				base.gameObject
			},
			{
				"onupdate",
				"updateWindowTweenValue"
			},
			{
				"oncompletetarget",
				base.gameObject
			},
			{
				"oncomplete",
				"windowOutComplete"
			},
			{
				"time",
				0.5f
			},
			{
				"easetype",
				DialogueriTween.EaseType.easeInBack
			}
		});
	}

	// Token: 0x06003FD8 RID: 16344 RVA: 0x000334A9 File Offset: 0x000316A9
	public void updateWindowTweenValue(float newValue)
	{
		this._windowTweenValue = newValue;
	}

	// Token: 0x06003FD9 RID: 16345 RVA: 0x000334B2 File Offset: 0x000316B2
	public void windowInComplete()
	{
		this._windowReady = true;
	}

	// Token: 0x06003FDA RID: 16346 RVA: 0x000334BB File Offset: 0x000316BB
	public void windowOutComplete()
	{
		this._showDialogueBox = false;
		this.resetWindowSize();
		if (this._ending)
		{
			this._dialogue = false;
			this._ending = false;
		}
	}

	// Token: 0x06003FDB RID: 16347 RVA: 0x000334E3 File Offset: 0x000316E3
	public Rect centerRect(Rect rect)
	{
		return new Rect(rect.x - rect.width * 0.5f, rect.y - rect.height * 0.5f, rect.width, rect.height);
	}

	// Token: 0x06003FDC RID: 16348 RVA: 0x0012992C File Offset: 0x00127B2C
	public void calculateText()
	{
		if (this._windowTargetText == string.Empty || this._windowCurrentText == this._windowTargetText)
		{
			return;
		}
		int num = 2;
		if (this._textFrames < num)
		{
			this._textFrames++;
			return;
		}
		this._textFrames = 0;
		int num2 = 1;
		if (this._windowCurrentText != this._windowTargetText)
		{
			for (int i = 0; i < num2; i++)
			{
				if (this._windowTargetText.Length <= this._windowCurrentText.Length)
				{
					break;
				}
				this._windowCurrentText += this._windowTargetText[this._windowCurrentText.Length];
			}
		}
		this.audioText.Play();
	}

	// Token: 0x040032C2 RID: 12994
	public GUISkin skin;

	// Token: 0x040032C3 RID: 12995
	public Texture2D diagonalLines;

	// Token: 0x040032C4 RID: 12996
	public AudioSource audioText;

	// Token: 0x040032C5 RID: 12997
	public AudioSource audioTextEnd;

	// Token: 0x040032C6 RID: 12998
	public AudioSource audioGood;

	// Token: 0x040032C7 RID: 12999
	public AudioSource audioBad;

	// Token: 0x040032C8 RID: 13000
	public bool _dialogue;

	// Token: 0x040032C9 RID: 13001
	public bool _ending;

	// Token: 0x040032CA RID: 13002
	public bool _showDialogueBox;

	// Token: 0x040032CB RID: 13003
	public bool _usingPositionRect;

	// Token: 0x040032CC RID: 13004
	public Rect _positionRect = new Rect(0f, 0f, 0f, 0f);

	// Token: 0x040032CD RID: 13005
	public string _windowTargetText = string.Empty;

	// Token: 0x040032CE RID: 13006
	public string _windowCurrentText = string.Empty;

	// Token: 0x040032CF RID: 13007
	public string _nameText = string.Empty;

	// Token: 0x040032D0 RID: 13008
	public bool _isBranchedText;

	// Token: 0x040032D1 RID: 13009
	public string[] _branchedTextChoices;

	// Token: 0x040032D2 RID: 13010
	public int _currentChoice;

	// Token: 0x040032D3 RID: 13011
	public string _theme;

	// Token: 0x040032D4 RID: 13012
	public float _windowTweenValue;

	// Token: 0x040032D5 RID: 13013
	public bool _windowReady;

	// Token: 0x040032D6 RID: 13014
	public float _nameTweenValue;

	// Token: 0x040032D7 RID: 13015
	public int _textFrames = int.MaxValue;
}
