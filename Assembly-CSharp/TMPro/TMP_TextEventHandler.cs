using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro
{
	// Token: 0x020008F6 RID: 2294
	public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06004565 RID: 17765 RVA: 0x00174A67 File Offset: 0x00172E67
		// (set) Token: 0x06004566 RID: 17766 RVA: 0x00174A6F File Offset: 0x00172E6F
		public TMP_TextEventHandler.CharacterSelectionEvent onCharacterSelection
		{
			get
			{
				return this.m_OnCharacterSelection;
			}
			set
			{
				this.m_OnCharacterSelection = value;
			}
		}

		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06004567 RID: 17767 RVA: 0x00174A78 File Offset: 0x00172E78
		// (set) Token: 0x06004568 RID: 17768 RVA: 0x00174A80 File Offset: 0x00172E80
		public TMP_TextEventHandler.WordSelectionEvent onWordSelection
		{
			get
			{
				return this.m_OnWordSelection;
			}
			set
			{
				this.m_OnWordSelection = value;
			}
		}

		// Token: 0x17000A92 RID: 2706
		// (get) Token: 0x06004569 RID: 17769 RVA: 0x00174A89 File Offset: 0x00172E89
		// (set) Token: 0x0600456A RID: 17770 RVA: 0x00174A91 File Offset: 0x00172E91
		public TMP_TextEventHandler.LineSelectionEvent onLineSelection
		{
			get
			{
				return this.m_OnLineSelection;
			}
			set
			{
				this.m_OnLineSelection = value;
			}
		}

		// Token: 0x17000A93 RID: 2707
		// (get) Token: 0x0600456B RID: 17771 RVA: 0x00174A9A File Offset: 0x00172E9A
		// (set) Token: 0x0600456C RID: 17772 RVA: 0x00174AA2 File Offset: 0x00172EA2
		public TMP_TextEventHandler.LinkSelectionEvent onLinkSelection
		{
			get
			{
				return this.m_OnLinkSelection;
			}
			set
			{
				this.m_OnLinkSelection = value;
			}
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00174AAC File Offset: 0x00172EAC
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
			if (this.m_TextComponent.GetType() == typeof(TextMeshProUGUI))
			{
				this.m_Canvas = base.gameObject.GetComponentInParent<Canvas>();
				if (this.m_Canvas != null)
				{
					if (this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
					{
						this.m_Camera = null;
					}
					else
					{
						this.m_Camera = this.m_Canvas.worldCamera;
					}
				}
			}
			else
			{
				this.m_Camera = Camera.main;
			}
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x00174B44 File Offset: 0x00172F44
		private void LateUpdate()
		{
			if (TMP_TextUtilities.IsIntersectingRectTransform(this.m_TextComponent.rectTransform, Input.mousePosition, this.m_Camera))
			{
				int num = TMP_TextUtilities.FindIntersectingCharacter(this.m_TextComponent, Input.mousePosition, this.m_Camera, true);
				if (num != -1 && num != this.m_lastCharIndex)
				{
					this.m_lastCharIndex = num;
					this.SendOnCharacterSelection(this.m_TextComponent.textInfo.characterInfo[num].character, num);
				}
				int num2 = TMP_TextUtilities.FindIntersectingWord(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num2 != -1 && num2 != this.m_lastWordIndex)
				{
					this.m_lastWordIndex = num2;
					TMP_WordInfo tmp_WordInfo = this.m_TextComponent.textInfo.wordInfo[num2];
					this.SendOnWordSelection(tmp_WordInfo.GetWord(), tmp_WordInfo.firstCharacterIndex, tmp_WordInfo.characterCount);
				}
				int num3 = TMP_TextUtilities.FindIntersectingLine(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num3 != -1 && num3 != this.m_lastLineIndex)
				{
					this.m_lastLineIndex = num3;
					TMP_LineInfo tmp_LineInfo = this.m_TextComponent.textInfo.lineInfo[num3];
					char[] array = new char[tmp_LineInfo.characterCount];
					int num4 = 0;
					while (num4 < tmp_LineInfo.characterCount && num4 < this.m_TextComponent.textInfo.characterInfo.Length)
					{
						array[num4] = this.m_TextComponent.textInfo.characterInfo[num4 + tmp_LineInfo.firstCharacterIndex].character;
						num4++;
					}
					string line = new string(array);
					this.SendOnLineSelection(line, tmp_LineInfo.firstCharacterIndex, tmp_LineInfo.characterCount);
				}
				int num5 = TMP_TextUtilities.FindIntersectingLink(this.m_TextComponent, Input.mousePosition, this.m_Camera);
				if (num5 != -1 && num5 != this.m_selectedLink)
				{
					this.m_selectedLink = num5;
					TMP_LinkInfo tmp_LinkInfo = this.m_TextComponent.textInfo.linkInfo[num5];
					this.SendOnLinkSelection(tmp_LinkInfo.GetLinkID(), tmp_LinkInfo.GetLinkText(), num5);
				}
			}
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x00174D6F File Offset: 0x0017316F
		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x00174D71 File Offset: 0x00173171
		public void OnPointerExit(PointerEventData eventData)
		{
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x00174D73 File Offset: 0x00173173
		private void SendOnCharacterSelection(char character, int characterIndex)
		{
			if (this.onCharacterSelection != null)
			{
				this.onCharacterSelection.Invoke(character, characterIndex);
			}
		}

		// Token: 0x06004572 RID: 17778 RVA: 0x00174D8D File Offset: 0x0017318D
		private void SendOnWordSelection(string word, int charIndex, int length)
		{
			if (this.onWordSelection != null)
			{
				this.onWordSelection.Invoke(word, charIndex, length);
			}
		}

		// Token: 0x06004573 RID: 17779 RVA: 0x00174DA8 File Offset: 0x001731A8
		private void SendOnLineSelection(string line, int charIndex, int length)
		{
			if (this.onLineSelection != null)
			{
				this.onLineSelection.Invoke(line, charIndex, length);
			}
		}

		// Token: 0x06004574 RID: 17780 RVA: 0x00174DC3 File Offset: 0x001731C3
		private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			if (this.onLinkSelection != null)
			{
				this.onLinkSelection.Invoke(linkID, linkText, linkIndex);
			}
		}

		// Token: 0x04002F7B RID: 12155
		[SerializeField]
		private TMP_TextEventHandler.CharacterSelectionEvent m_OnCharacterSelection = new TMP_TextEventHandler.CharacterSelectionEvent();

		// Token: 0x04002F7C RID: 12156
		[SerializeField]
		private TMP_TextEventHandler.WordSelectionEvent m_OnWordSelection = new TMP_TextEventHandler.WordSelectionEvent();

		// Token: 0x04002F7D RID: 12157
		[SerializeField]
		private TMP_TextEventHandler.LineSelectionEvent m_OnLineSelection = new TMP_TextEventHandler.LineSelectionEvent();

		// Token: 0x04002F7E RID: 12158
		[SerializeField]
		private TMP_TextEventHandler.LinkSelectionEvent m_OnLinkSelection = new TMP_TextEventHandler.LinkSelectionEvent();

		// Token: 0x04002F7F RID: 12159
		private TMP_Text m_TextComponent;

		// Token: 0x04002F80 RID: 12160
		private Camera m_Camera;

		// Token: 0x04002F81 RID: 12161
		private Canvas m_Canvas;

		// Token: 0x04002F82 RID: 12162
		private int m_selectedLink = -1;

		// Token: 0x04002F83 RID: 12163
		private int m_lastCharIndex = -1;

		// Token: 0x04002F84 RID: 12164
		private int m_lastWordIndex = -1;

		// Token: 0x04002F85 RID: 12165
		private int m_lastLineIndex = -1;

		// Token: 0x020008F7 RID: 2295
		[Serializable]
		public class CharacterSelectionEvent : UnityEvent<char, int>
		{
		}

		// Token: 0x020008F8 RID: 2296
		[Serializable]
		public class WordSelectionEvent : UnityEvent<string, int, int>
		{
		}

		// Token: 0x020008F9 RID: 2297
		[Serializable]
		public class LineSelectionEvent : UnityEvent<string, int, int>
		{
		}

		// Token: 0x020008FA RID: 2298
		[Serializable]
		public class LinkSelectionEvent : UnityEvent<string, string, int>
		{
		}
	}
}
