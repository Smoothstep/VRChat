// Decompiled with JetBrains decompiler
// Type: UIInput
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Input Field")]
public class UIInput : MonoBehaviour
{
    protected static string mLastIME = string.Empty;
    [NonSerialized]
    public bool selectAllTextOnFocus = true;
    public Color activeTextColor = Color.white;
    public Color caretColor = new Color(1f, 1f, 1f, 0.8f);
    public Color selectionColor = new Color(1f, 0.8745098f, 0.5529412f, 0.5f);
    public List<EventDelegate> onSubmit = new List<EventDelegate>();
    public List<EventDelegate> onChange = new List<EventDelegate>();
    [NonSerialized]
    protected string mDefaultText = string.Empty;
    [NonSerialized]
    protected Color mDefaultColor = Color.white;
    [NonSerialized]
    protected bool mDoInit = true;
    [NonSerialized]
    protected bool mLoadSavedValue = true;
    [NonSerialized]
    protected string mCached = string.Empty;
    [NonSerialized]
    protected int mSelectMe = -1;
    public static UIInput current;
    public static UIInput selection;
    public UILabel label;
    public UIInput.InputType inputType;
    public UIInput.OnReturnKey onReturnKey;
    public UIInput.KeyboardType keyboardType;
    public bool hideInput;
    public UIInput.Validation validation;
    public int characterLimit;
    public string savedAs;
    [HideInInspector]
    [SerializeField]
    private GameObject selectOnTab;
    public UIInput.OnValidate onValidate;
    [SerializeField]
    [HideInInspector]
    protected string mValue;
    [NonSerialized]
    protected float mPosition;
    [NonSerialized]
    protected UIWidget.Pivot mPivot;
    protected static int mDrawStart;
    [NonSerialized]
    protected int mSelectionStart;
    [NonSerialized]
    protected int mSelectionEnd;
    [NonSerialized]
    protected UITexture mHighlight;
    [NonSerialized]
    protected UITexture mCaret;
    [NonSerialized]
    protected Texture2D mBlankTex;
    [NonSerialized]
    protected float mNextBlink;
    [NonSerialized]
    protected float mLastAlpha;
    [NonSerialized]
    private UIInputOnGUI mOnGUI;

    public string defaultText
    {
        get
        {
            if (this.mDoInit)
                this.Init();
            return this.mDefaultText;
        }
        set
        {
            if (this.mDoInit)
                this.Init();
            this.mDefaultText = value;
            this.UpdateLabel();
        }
    }

    public bool inputShouldBeHidden
    {
        get
        {
            if (this.hideInput && (UnityEngine.Object)this.label != (UnityEngine.Object)null && !this.label.multiLine)
                return this.inputType != UIInput.InputType.Password;
            return false;
        }
    }

    [Obsolete("Use UIInput.value instead")]
    public string text
    {
        get
        {
            return this.value;
        }
        set
        {
            this.value = value;
        }
    }

    public string value
    {
        get
        {
            if (this.mDoInit)
                this.Init();
            return this.mValue;
        }
        set
        {
            if (this.mDoInit)
                this.Init();
            UIInput.mDrawStart = 0;
            value = value.Replace("\\b", "\b");
            value = this.Validate(value);
            if (!(this.mValue != value))
                return;
            this.mValue = value;
            this.mLoadSavedValue = false;
            if (this.isSelected)
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.mSelectionStart = 0;
                    this.mSelectionEnd = 0;
                }
                else
                {
                    this.mSelectionStart = value.Length;
                    this.mSelectionEnd = this.mSelectionStart;
                }
            }
            else
                this.SaveToPlayerPrefs(value);
            this.UpdateLabel();
            this.ExecuteOnChange();
        }
    }

    [Obsolete("Use UIInput.isSelected instead")]
    public bool selected
    {
        get
        {
            return this.isSelected;
        }
        set
        {
            this.isSelected = value;
        }
    }

    public bool isSelected
    {
        get
        {
            return (UnityEngine.Object)UIInput.selection == (UnityEngine.Object)this;
        }
        set
        {
            if (!value)
            {
                if (!this.isSelected)
                    return;
                UICamera.selectedObject = (GameObject)null;
            }
            else
                UICamera.selectedObject = this.gameObject;
        }
    }

    public int cursorPosition
    {
        get
        {
            if (this.isSelected)
                return this.mSelectionEnd;
            return this.value.Length;
        }
        set
        {
            if (!this.isSelected)
                return;
            this.mSelectionEnd = value;
            this.UpdateLabel();
        }
    }

    public int selectionStart
    {
        get
        {
            if (this.isSelected)
                return this.mSelectionStart;
            return this.value.Length;
        }
        set
        {
            if (!this.isSelected)
                return;
            this.mSelectionStart = value;
            this.UpdateLabel();
        }
    }

    public int selectionEnd
    {
        get
        {
            if (this.isSelected)
                return this.mSelectionEnd;
            return this.value.Length;
        }
        set
        {
            if (!this.isSelected)
                return;
            this.mSelectionEnd = value;
            this.UpdateLabel();
        }
    }

    public UITexture caret
    {
        get
        {
            return this.mCaret;
        }
    }

    public string Validate(string val)
    {
        if (string.IsNullOrEmpty(val))
            return string.Empty;
        StringBuilder stringBuilder = new StringBuilder(val.Length);
        for (int index = 0; index < val.Length; ++index)
        {
            char ch = val[index];
            if (this.onValidate != null)
                ch = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, ch);
            else if (this.validation != UIInput.Validation.None)
                ch = this.Validate(stringBuilder.ToString(), stringBuilder.Length, ch);
            if ((int)ch != 0)
                stringBuilder.Append(ch);
        }
        if (this.characterLimit > 0 && stringBuilder.Length > this.characterLimit)
            return stringBuilder.ToString(0, this.characterLimit);
        return stringBuilder.ToString();
    }

    private void Start()
    {
        if ((UnityEngine.Object)this.selectOnTab != (UnityEngine.Object)null)
        {
            if ((UnityEngine.Object)this.GetComponent<UIKeyNavigation>() == (UnityEngine.Object)null)
                this.gameObject.AddComponent<UIKeyNavigation>().onDown = this.selectOnTab;
            this.selectOnTab = (GameObject)null;
            NGUITools.SetDirty((UnityEngine.Object)this);
        }
        if (this.mLoadSavedValue && !string.IsNullOrEmpty(this.savedAs))
            this.LoadValue();
        else
            this.value = this.mValue.Replace("\\n", "\n");
    }

    protected void Init()
    {
        if (!this.mDoInit || !((UnityEngine.Object)this.label != (UnityEngine.Object)null))
            return;
        this.mDoInit = false;
        this.mDefaultText = this.label.text;
        this.mDefaultColor = this.label.color;
        this.label.supportEncoding = false;
        if (this.label.alignment == NGUIText.Alignment.Justified)
        {
            this.label.alignment = NGUIText.Alignment.Left;
            Debug.LogWarning((object)"Input fields using labels with justified alignment are not supported at this time", (UnityEngine.Object)this);
        }
        this.mPivot = this.label.pivot;
        this.mPosition = this.label.cachedTransform.localPosition.x;
        this.UpdateLabel();
    }

    protected void SaveToPlayerPrefs(string val)
    {
        if (string.IsNullOrEmpty(this.savedAs))
            return;
        if (string.IsNullOrEmpty(val))
            PlayerPrefs.DeleteKey(this.savedAs);
        else
            PlayerPrefs.SetString(this.savedAs, val);
    }

    protected virtual void OnSelect(bool isSelected)
    {
        if (isSelected)
        {
            if ((UnityEngine.Object)this.mOnGUI == (UnityEngine.Object)null)
                this.mOnGUI = this.gameObject.AddComponent<UIInputOnGUI>();
            this.OnSelectEvent();
        }
        else
        {
            if ((UnityEngine.Object)this.mOnGUI != (UnityEngine.Object)null)
            {
                UnityEngine.Object.Destroy((UnityEngine.Object)this.mOnGUI);
                this.mOnGUI = (UIInputOnGUI)null;
            }
            this.OnDeselectEvent();
        }
    }

    protected void OnSelectEvent()
    {
        UIInput.selection = this;
        if (this.mDoInit)
            this.Init();
        if (!((UnityEngine.Object)this.label != (UnityEngine.Object)null) || !NGUITools.GetActive((Behaviour)this))
            return;
        this.mSelectMe = Time.frameCount;
    }

    protected void OnDeselectEvent()
    {
        if (this.mDoInit)
            this.Init();
        if ((UnityEngine.Object)this.label != (UnityEngine.Object)null && NGUITools.GetActive((Behaviour)this))
        {
            this.mValue = this.value;
            if (string.IsNullOrEmpty(this.mValue))
            {
                this.label.text = this.mDefaultText;
                this.label.color = this.mDefaultColor;
            }
            else
                this.label.text = this.mValue;
            Input.imeCompositionMode = IMECompositionMode.Auto;
            this.RestoreLabelPivot();
        }
        UIInput.selection = (UIInput)null;
        this.UpdateLabel();
    }

    protected virtual void Update()
    {
        if (!this.isSelected)
            return;
        if (this.mDoInit)
            this.Init();
        if (this.mSelectMe != -1 && this.mSelectMe != Time.frameCount)
        {
            this.mSelectMe = -1;
            this.mSelectionEnd = !string.IsNullOrEmpty(this.mValue) ? this.mValue.Length : 0;
            UIInput.mDrawStart = 0;
            this.mSelectionStart = !this.selectAllTextOnFocus ? this.mSelectionEnd : 0;
            this.label.color = this.activeTextColor;
            Vector2 vector2 = (Vector2)(!((UnityEngine.Object)UICamera.current != (UnityEngine.Object)null) || !((UnityEngine.Object)UICamera.current.cachedCamera != (UnityEngine.Object)null) ? this.label.worldCorners[0] : UICamera.current.cachedCamera.WorldToScreenPoint(this.label.worldCorners[0]));
            vector2.y = (float)Screen.height - vector2.y;
            Input.imeCompositionMode = IMECompositionMode.On;
            Input.compositionCursorPos = vector2;
            this.UpdateLabel();
            if (string.IsNullOrEmpty(Input.inputString))
                return;
        }
        string compositionString = Input.compositionString;
        if (string.IsNullOrEmpty(compositionString) && !string.IsNullOrEmpty(Input.inputString))
        {
            foreach (char ch in Input.inputString)
            {
                if ((int)ch >= 32 && (int)ch != 63232 && ((int)ch != 63233 && (int)ch != 63234) && (int)ch != 63235)
                    this.Insert(ch.ToString());
            }
        }
        if (UIInput.mLastIME != compositionString)
        {
            this.mSelectionEnd = !string.IsNullOrEmpty(compositionString) ? this.mValue.Length + compositionString.Length : this.mSelectionStart;
            UIInput.mLastIME = compositionString;
            this.UpdateLabel();
            this.ExecuteOnChange();
        }
        if ((UnityEngine.Object)this.mCaret != (UnityEngine.Object)null && (double)this.mNextBlink < (double)RealTime.time)
        {
            this.mNextBlink = RealTime.time + 0.5f;
            this.mCaret.enabled = !this.mCaret.enabled;
        }
        if (!this.isSelected || (double)this.mLastAlpha == (double)this.label.finalAlpha)
            return;
        this.UpdateLabel();
    }

    protected void DoBackspace()
    {
        if (string.IsNullOrEmpty(this.mValue))
            return;
        if (this.mSelectionStart == this.mSelectionEnd)
        {
            if (this.mSelectionStart < 1)
                return;
            --this.mSelectionEnd;
        }
        this.Insert(string.Empty);
    }

    public virtual bool ProcessEvent(UnityEngine.Event ev)
    {
        if ((UnityEngine.Object)this.label == (UnityEngine.Object)null)
            return false;
        RuntimePlatform platform = Application.platform;
        bool flag1 = platform != RuntimePlatform.OSXEditor && platform != RuntimePlatform.OSXPlayer ? (ev.modifiers & EventModifiers.Control) != EventModifiers.None : (ev.modifiers & EventModifiers.Command) != EventModifiers.None;
        if ((ev.modifiers & EventModifiers.Alt) != EventModifiers.None)
            flag1 = false;
        bool flag2 = (ev.modifiers & EventModifiers.Shift) != EventModifiers.None;
        KeyCode keyCode = ev.keyCode;
        switch (keyCode)
        {
            case KeyCode.KeypadEnter:
                label_75:
                ev.Use();
                if (this.onReturnKey == UIInput.OnReturnKey.NewLine || this.onReturnKey == UIInput.OnReturnKey.Default && this.label.multiLine && (!flag1 && this.label.overflowMethod != UILabel.Overflow.ClampContent) && this.validation == UIInput.Validation.None)
                {
                    this.Insert("\n");
                }
                else
                {
                    UICamera.currentScheme = UICamera.ControlScheme.Controller;
                    UICamera.currentKey = ev.keyCode;
                    this.Submit();
                    UICamera.currentKey = KeyCode.None;
                }
                return true;
            case KeyCode.UpArrow:
                ev.Use();
                if (!string.IsNullOrEmpty(this.mValue))
                {
                    this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, KeyCode.UpArrow);
                    if (this.mSelectionEnd != 0)
                        this.mSelectionEnd += UIInput.mDrawStart;
                    if (!flag2)
                        this.mSelectionStart = this.mSelectionEnd;
                    this.UpdateLabel();
                }
                return true;
            case KeyCode.DownArrow:
                ev.Use();
                if (!string.IsNullOrEmpty(this.mValue))
                {
                    this.mSelectionEnd = this.label.GetCharacterIndex(this.mSelectionEnd, KeyCode.DownArrow);
                    if (this.mSelectionEnd != this.label.processedText.Length)
                        this.mSelectionEnd += UIInput.mDrawStart;
                    else
                        this.mSelectionEnd = this.mValue.Length;
                    if (!flag2)
                        this.mSelectionStart = this.mSelectionEnd;
                    this.UpdateLabel();
                }
                return true;
            case KeyCode.RightArrow:
                ev.Use();
                if (!string.IsNullOrEmpty(this.mValue))
                {
                    this.mSelectionEnd = Mathf.Min(this.mSelectionEnd + 1, this.mValue.Length);
                    if (!flag2)
                        this.mSelectionStart = this.mSelectionEnd;
                    this.UpdateLabel();
                }
                return true;
            case KeyCode.LeftArrow:
                ev.Use();
                if (!string.IsNullOrEmpty(this.mValue))
                {
                    this.mSelectionEnd = Mathf.Max(this.mSelectionEnd - 1, 0);
                    if (!flag2)
                        this.mSelectionStart = this.mSelectionEnd;
                    this.UpdateLabel();
                }
                return true;
            case KeyCode.Home:
                ev.Use();
                if (!string.IsNullOrEmpty(this.mValue))
                {
                    this.mSelectionEnd = !this.label.multiLine ? 0 : this.label.GetCharacterIndex(this.mSelectionEnd, KeyCode.Home);
                    if (!flag2)
                        this.mSelectionStart = this.mSelectionEnd;
                    this.UpdateLabel();
                }
                return true;
            case KeyCode.End:
                ev.Use();
                if (!string.IsNullOrEmpty(this.mValue))
                {
                    this.mSelectionEnd = !this.label.multiLine ? this.mValue.Length : this.label.GetCharacterIndex(this.mSelectionEnd, KeyCode.End);
                    if (!flag2)
                        this.mSelectionStart = this.mSelectionEnd;
                    this.UpdateLabel();
                }
                return true;
            case KeyCode.PageUp:
                ev.Use();
                if (!string.IsNullOrEmpty(this.mValue))
                {
                    this.mSelectionEnd = 0;
                    if (!flag2)
                        this.mSelectionStart = this.mSelectionEnd;
                    this.UpdateLabel();
                }
                return true;
            case KeyCode.PageDown:
                ev.Use();
                if (!string.IsNullOrEmpty(this.mValue))
                {
                    this.mSelectionEnd = this.mValue.Length;
                    if (!flag2)
                        this.mSelectionStart = this.mSelectionEnd;
                    this.UpdateLabel();
                }
                return true;
            default:
                switch (keyCode - 97)
                {
                    case KeyCode.None:
                        if (flag1)
                        {
                            ev.Use();
                            this.mSelectionStart = 0;
                            this.mSelectionEnd = this.mValue.Length;
                            this.UpdateLabel();
                        }
                        return true;
                    case (KeyCode)2:
                        if (flag1)
                        {
                            ev.Use();
                            NGUITools.clipboard = this.GetSelection();
                        }
                        return true;
                    default:
                        switch (keyCode - 118)
                        {
                            case KeyCode.None:
                                if (flag1)
                                {
                                    ev.Use();
                                    this.Insert(NGUITools.clipboard);
                                }
                                return true;
                            case (KeyCode)2:
                                if (flag1)
                                {
                                    ev.Use();
                                    NGUITools.clipboard = this.GetSelection();
                                    this.Insert(string.Empty);
                                }
                                return true;
                            default:
                                if (keyCode != KeyCode.Backspace)
                                {
                                    if (keyCode != KeyCode.Return)
                                    {
                                        if (keyCode != KeyCode.Delete)
                                            return false;
                                        ev.Use();
                                        if (!string.IsNullOrEmpty(this.mValue))
                                        {
                                            if (this.mSelectionStart == this.mSelectionEnd)
                                            {
                                                if (this.mSelectionStart >= this.mValue.Length)
                                                    return true;
                                                ++this.mSelectionEnd;
                                            }
                                            this.Insert(string.Empty);
                                        }
                                        return true;
                                    }
                                    goto label_75;
                                }
                                else
                                {
                                    ev.Use();
                                    this.DoBackspace();
                                    return true;
                                }
                        }
                }
        }
    }

    protected virtual void Insert(string text)
    {
        string leftText = this.GetLeftText();
        string rightText = this.GetRightText();
        int length1 = rightText.Length;
        StringBuilder stringBuilder = new StringBuilder(leftText.Length + rightText.Length + text.Length);
        stringBuilder.Append(leftText);
        int index1 = 0;
        for (int length2 = text.Length; index1 < length2; ++index1)
        {
            char ch = text[index1];
            if ((int)ch == 8)
                this.DoBackspace();
            else if (this.characterLimit <= 0 || stringBuilder.Length + length1 < this.characterLimit)
            {
                if (this.onValidate != null)
                    ch = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, ch);
                else if (this.validation != UIInput.Validation.None)
                    ch = this.Validate(stringBuilder.ToString(), stringBuilder.Length, ch);
                if ((int)ch != 0)
                    stringBuilder.Append(ch);
            }
            else
                break;
        }
        this.mSelectionStart = stringBuilder.Length;
        this.mSelectionEnd = this.mSelectionStart;
        int index2 = 0;
        for (int length2 = rightText.Length; index2 < length2; ++index2)
        {
            char ch = rightText[index2];
            if (this.onValidate != null)
                ch = this.onValidate(stringBuilder.ToString(), stringBuilder.Length, ch);
            else if (this.validation != UIInput.Validation.None)
                ch = this.Validate(stringBuilder.ToString(), stringBuilder.Length, ch);
            if ((int)ch != 0)
                stringBuilder.Append(ch);
        }
        this.mValue = stringBuilder.ToString();
        this.UpdateLabel();
        this.ExecuteOnChange();
    }

    protected string GetLeftText()
    {
        int length = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
        if (string.IsNullOrEmpty(this.mValue) || length < 0)
            return string.Empty;
        return this.mValue.Substring(0, length);
    }

    protected string GetRightText()
    {
        int startIndex = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
        if (string.IsNullOrEmpty(this.mValue) || startIndex >= this.mValue.Length)
            return string.Empty;
        return this.mValue.Substring(startIndex);
    }

    protected string GetSelection()
    {
        if (string.IsNullOrEmpty(this.mValue) || this.mSelectionStart == this.mSelectionEnd)
            return string.Empty;
        int startIndex = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
        int num = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
        return this.mValue.Substring(startIndex, num - startIndex);
    }

    protected int GetCharUnderMouse()
    {
        Vector3[] worldCorners = this.label.worldCorners;
        Ray currentRay = UICamera.currentRay;
        float enter;
        if (new Plane(worldCorners[0], worldCorners[1], worldCorners[2]).Raycast(currentRay, out enter))
            return UIInput.mDrawStart + this.label.GetCharacterIndexAtPosition(currentRay.GetPoint(enter), false);
        return 0;
    }

    protected virtual void OnPress(bool isPressed)
    {
        if (!isPressed || !this.isSelected || !((UnityEngine.Object)this.label != (UnityEngine.Object)null) || UICamera.currentScheme != UICamera.ControlScheme.Mouse && UICamera.currentScheme != UICamera.ControlScheme.Touch)
            return;
        this.selectionEnd = this.GetCharUnderMouse();
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            return;
        this.selectionStart = this.mSelectionEnd;
    }

    protected virtual void OnDrag(Vector2 delta)
    {
        if (!((UnityEngine.Object)this.label != (UnityEngine.Object)null) || UICamera.currentScheme != UICamera.ControlScheme.Mouse && UICamera.currentScheme != UICamera.ControlScheme.Touch)
            return;
        this.selectionEnd = this.GetCharUnderMouse();
    }

    private void OnDisable()
    {
        this.Cleanup();
    }

    protected virtual void Cleanup()
    {
        if ((bool)((UnityEngine.Object)this.mHighlight))
            this.mHighlight.enabled = false;
        if ((bool)((UnityEngine.Object)this.mCaret))
            this.mCaret.enabled = false;
        if (!(bool)((UnityEngine.Object)this.mBlankTex))
            return;
        NGUITools.Destroy((UnityEngine.Object)this.mBlankTex);
        this.mBlankTex = (Texture2D)null;
    }

    public void Submit()
    {
        if (!NGUITools.GetActive((Behaviour)this))
            return;
        this.mValue = this.value;
        if ((UnityEngine.Object)UIInput.current == (UnityEngine.Object)null)
        {
            UIInput.current = this;
            EventDelegate.Execute(this.onSubmit);
            UIInput.current = (UIInput)null;
        }
        this.SaveToPlayerPrefs(this.mValue);
    }

    public void UpdateLabel()
    {
        if (!((UnityEngine.Object)this.label != (UnityEngine.Object)null))
            return;
        if (this.mDoInit)
            this.Init();
        bool isSelected = this.isSelected;
        string str1 = this.value;
        bool flag = string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(Input.compositionString);
        this.label.color = !flag || isSelected ? this.activeTextColor : this.mDefaultColor;
        string text;
        if (flag)
        {
            text = !isSelected ? this.mDefaultText : string.Empty;
            this.RestoreLabelPivot();
        }
        else
        {
            string str2;
            if (this.inputType == UIInput.InputType.Password)
            {
                str2 = string.Empty;
                string str3 = "*";
                if ((UnityEngine.Object)this.label.bitmapFont != (UnityEngine.Object)null && this.label.bitmapFont.bmFont != null && this.label.bitmapFont.bmFont.GetGlyph(42) == null)
                    str3 = "x";
                int num = 0;
                for (int length = str1.Length; num < length; ++num)
                    str2 += str3;
            }
            else
                str2 = str1;
            int num1 = !isSelected ? 0 : Mathf.Min(str2.Length, this.cursorPosition);
            string str4 = str2.Substring(0, num1);
            if (isSelected)
                str4 += Input.compositionString;
            text = str4 + str2.Substring(num1, str2.Length - num1);
            if (isSelected && this.label.overflowMethod == UILabel.Overflow.ClampContent && this.label.maxLineCount == 1)
            {
                int offsetToFit1 = this.label.CalculateOffsetToFit(text);
                if (offsetToFit1 == 0)
                {
                    UIInput.mDrawStart = 0;
                    this.RestoreLabelPivot();
                }
                else if (num1 < UIInput.mDrawStart)
                {
                    UIInput.mDrawStart = num1;
                    this.SetPivotToLeft();
                }
                else if (offsetToFit1 < UIInput.mDrawStart)
                {
                    UIInput.mDrawStart = offsetToFit1;
                    this.SetPivotToLeft();
                }
                else
                {
                    int offsetToFit2 = this.label.CalculateOffsetToFit(text.Substring(0, num1));
                    if (offsetToFit2 > UIInput.mDrawStart)
                    {
                        UIInput.mDrawStart = offsetToFit2;
                        this.SetPivotToRight();
                    }
                }
                if (UIInput.mDrawStart != 0)
                    text = text.Substring(UIInput.mDrawStart, text.Length - UIInput.mDrawStart);
            }
            else
            {
                UIInput.mDrawStart = 0;
                this.RestoreLabelPivot();
            }
        }
        this.label.text = text;
        if (isSelected)
        {
            int start = this.mSelectionStart - UIInput.mDrawStart;
            int end = this.mSelectionEnd - UIInput.mDrawStart;
            if ((UnityEngine.Object)this.mBlankTex == (UnityEngine.Object)null)
            {
                this.mBlankTex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                for (int y = 0; y < 2; ++y)
                {
                    for (int x = 0; x < 2; ++x)
                        this.mBlankTex.SetPixel(x, y, Color.white);
                }
                this.mBlankTex.Apply();
            }
            if (start != end)
            {
                if ((UnityEngine.Object)this.mHighlight == (UnityEngine.Object)null)
                {
                    this.mHighlight = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject);
                    this.mHighlight.name = "Input Highlight";
                    this.mHighlight.mainTexture = (Texture)this.mBlankTex;
                    this.mHighlight.fillGeometry = false;
                    this.mHighlight.pivot = this.label.pivot;
                    this.mHighlight.SetAnchor(this.label.cachedTransform);
                }
                else
                {
                    this.mHighlight.pivot = this.label.pivot;
                    this.mHighlight.mainTexture = (Texture)this.mBlankTex;
                    this.mHighlight.MarkAsChanged();
                    this.mHighlight.enabled = true;
                }
            }
            if ((UnityEngine.Object)this.mCaret == (UnityEngine.Object)null)
            {
                this.mCaret = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject);
                this.mCaret.name = "Input Caret";
                this.mCaret.mainTexture = (Texture)this.mBlankTex;
                this.mCaret.fillGeometry = false;
                this.mCaret.pivot = this.label.pivot;
                this.mCaret.SetAnchor(this.label.cachedTransform);
            }
            else
            {
                this.mCaret.pivot = this.label.pivot;
                this.mCaret.mainTexture = (Texture)this.mBlankTex;
                this.mCaret.MarkAsChanged();
                this.mCaret.enabled = true;
            }
            if (start != end)
            {
                this.label.PrintOverlay(start, end, this.mCaret.geometry, this.mHighlight.geometry, this.caretColor, this.selectionColor);
                this.mHighlight.enabled = this.mHighlight.geometry.hasVertices;
            }
            else
            {
                this.label.PrintOverlay(start, end, this.mCaret.geometry, (UIGeometry)null, this.caretColor, this.selectionColor);
                if ((UnityEngine.Object)this.mHighlight != (UnityEngine.Object)null)
                    this.mHighlight.enabled = false;
            }
            this.mNextBlink = RealTime.time + 0.5f;
            this.mLastAlpha = this.label.finalAlpha;
        }
        else
            this.Cleanup();
    }

    protected void SetPivotToLeft()
    {
        Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.mPivot);
        pivotOffset.x = 0.0f;
        this.label.pivot = NGUIMath.GetPivot(pivotOffset);
    }

    protected void SetPivotToRight()
    {
        Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.mPivot);
        pivotOffset.x = 1f;
        this.label.pivot = NGUIMath.GetPivot(pivotOffset);
    }

    protected void RestoreLabelPivot()
    {
        if (!((UnityEngine.Object)this.label != (UnityEngine.Object)null) || this.label.pivot == this.mPivot)
            return;
        this.label.pivot = this.mPivot;
    }

    protected char Validate(string text, int pos, char ch)
    {
        if (this.validation == UIInput.Validation.None || !this.enabled)
            return ch;
        if (this.validation == UIInput.Validation.Integer)
        {
            if ((int)ch >= 48 && (int)ch <= 57 || (int)ch == 45 && pos == 0 && !text.Contains("-"))
                return ch;
        }
        else if (this.validation == UIInput.Validation.Float)
        {
            if ((int)ch >= 48 && (int)ch <= 57 || (int)ch == 45 && pos == 0 && !text.Contains("-") || (int)ch == 46 && !text.Contains("."))
                return ch;
        }
        else if (this.validation == UIInput.Validation.Alphanumeric)
        {
            if ((int)ch >= 65 && (int)ch <= 90 || (int)ch >= 97 && (int)ch <= 122 || (int)ch >= 48 && (int)ch <= 57)
                return ch;
        }
        else if (this.validation == UIInput.Validation.Username)
        {
            if ((int)ch >= 65 && (int)ch <= 90)
                return (char)((int)ch - 65 + 97);
            if ((int)ch >= 97 && (int)ch <= 122 || (int)ch >= 48 && (int)ch <= 57)
                return ch;
        }
        else if (this.validation == UIInput.Validation.Name)
        {
            char ch1 = text.Length <= 0 ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
            char ch2 = text.Length <= 0 ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
            if ((int)ch >= 97 && (int)ch <= 122)
            {
                if ((int)ch1 == 32)
                    return (char)((int)ch - 97 + 65);
                return ch;
            }
            if ((int)ch >= 65 && (int)ch <= 90)
            {
                if ((int)ch1 != 32 && (int)ch1 != 39)
                    return (char)((int)ch - 65 + 97);
                return ch;
            }
            if ((int)ch == 39)
            {
                if ((int)ch1 != 32 && (int)ch1 != 39 && ((int)ch2 != 39 && !text.Contains("'")))
                    return ch;
            }
            else if ((int)ch == 32 && (int)ch1 != 32 && ((int)ch1 != 39 && (int)ch2 != 32) && (int)ch2 != 39)
                return ch;
        }
        return char.MinValue;
    }

    protected void ExecuteOnChange()
    {
        if (!((UnityEngine.Object)UIInput.current == (UnityEngine.Object)null) || !EventDelegate.IsValid(this.onChange))
            return;
        UIInput.current = this;
        EventDelegate.Execute(this.onChange);
        UIInput.current = (UIInput)null;
    }

    public void RemoveFocus()
    {
        this.isSelected = false;
    }

    public void SaveValue()
    {
        this.SaveToPlayerPrefs(this.mValue);
    }

    public void LoadValue()
    {
        if (string.IsNullOrEmpty(this.savedAs))
            return;
        string str = this.mValue.Replace("\\n", "\n");
        this.mValue = string.Empty;
        this.value = !PlayerPrefs.HasKey(this.savedAs) ? str : PlayerPrefs.GetString(this.savedAs);
    }

    public enum InputType
    {
        Standard,
        AutoCorrect,
        Password,
    }

    public enum Validation
    {
        None,
        Integer,
        Float,
        Alphanumeric,
        Username,
        Name,
    }

    public enum KeyboardType
    {
        Default,
        ASCIICapable,
        NumbersAndPunctuation,
        URL,
        NumberPad,
        PhonePad,
        NamePhonePad,
        EmailAddress,
    }

    public enum OnReturnKey
    {
        Default,
        Submit,
        NewLine,
    }

    public delegate char OnValidate(string text, int charIndex, char addedChar);
}
