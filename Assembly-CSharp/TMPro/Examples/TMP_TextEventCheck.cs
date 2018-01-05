using System;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro.Examples
{
	// Token: 0x020008F5 RID: 2293
	public class TMP_TextEventCheck : MonoBehaviour
	{
		// Token: 0x0600455E RID: 17758 RVA: 0x001747B8 File Offset: 0x00172BB8
		private void OnEnable()
		{
			if (this.TextEventHandler != null)
			{
				this.TextEventHandler.onCharacterSelection.AddListener(new UnityAction<char, int>(this.OnCharacterSelection));
				this.TextEventHandler.onWordSelection.AddListener(new UnityAction<string, int, int>(this.OnWordSelection));
				this.TextEventHandler.onLineSelection.AddListener(new UnityAction<string, int, int>(this.OnLineSelection));
				this.TextEventHandler.onLinkSelection.AddListener(new UnityAction<string, string, int>(this.OnLinkSelection));
			}
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x00174848 File Offset: 0x00172C48
		private void OnDisable()
		{
			if (this.TextEventHandler != null)
			{
				this.TextEventHandler.onCharacterSelection.RemoveListener(new UnityAction<char, int>(this.OnCharacterSelection));
				this.TextEventHandler.onWordSelection.RemoveListener(new UnityAction<string, int, int>(this.OnWordSelection));
				this.TextEventHandler.onLineSelection.RemoveListener(new UnityAction<string, int, int>(this.OnLineSelection));
				this.TextEventHandler.onLinkSelection.RemoveListener(new UnityAction<string, string, int>(this.OnLinkSelection));
			}
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x001748D6 File Offset: 0x00172CD6
		private void OnCharacterSelection(char c, int index)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Character [",
				c,
				"] at Index: ",
				index,
				" has been selected."
			}));
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x00174914 File Offset: 0x00172D14
		private void OnWordSelection(string word, int firstCharacterIndex, int length)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Word [",
				word,
				"] with first character index of ",
				firstCharacterIndex,
				" and length of ",
				length,
				" has been selected."
			}));
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x00174968 File Offset: 0x00172D68
		private void OnLineSelection(string lineText, int firstCharacterIndex, int length)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Line [",
				lineText,
				"] with first character index of ",
				firstCharacterIndex,
				" and length of ",
				length,
				" has been selected."
			}));
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x001749BC File Offset: 0x00172DBC
		private void OnLinkSelection(string linkID, string linkText, int linkIndex)
		{
			Debug.Log(string.Concat(new object[]
			{
				"Link Index: ",
				linkIndex,
				" with ID [",
				linkID,
				"] and Text \"",
				linkText,
				"\" has been selected."
			}));
		}

		// Token: 0x04002F7A RID: 12154
		public TMP_TextEventHandler TextEventHandler;
	}
}
