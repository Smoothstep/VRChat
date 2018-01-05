using System;
using System.Collections;
using UnityEngine;

namespace TMPro.Examples
{
	// Token: 0x02000903 RID: 2307
	public class TextConsoleSimulator : MonoBehaviour
	{
		// Token: 0x06004596 RID: 17814 RVA: 0x00176979 File Offset: 0x00174D79
		private void Awake()
		{
			this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x0017698C File Offset: 0x00174D8C
		private void Start()
		{
			base.StartCoroutine(this.RevealCharacters(this.m_TextComponent));
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x001769A1 File Offset: 0x00174DA1
		private void OnEnable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x001769B9 File Offset: 0x00174DB9
		private void OnDisable()
		{
			TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.ON_TEXT_CHANGED));
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x001769D1 File Offset: 0x00174DD1
		private void ON_TEXT_CHANGED(UnityEngine.Object obj)
		{
			this.hasTextChanged = true;
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x001769DC File Offset: 0x00174DDC
		private IEnumerator RevealCharacters(TMP_Text textComponent)
		{
			textComponent.ForceMeshUpdate();
			TMP_TextInfo textInfo = textComponent.textInfo;
			int totalVisibleCharacters = textInfo.characterCount;
			int visibleCount = 0;
			for (;;)
			{
				if (this.hasTextChanged)
				{
					totalVisibleCharacters = textInfo.characterCount;
					this.hasTextChanged = false;
				}
				if (visibleCount > totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
					visibleCount = 0;
				}
				textComponent.maxVisibleCharacters = visibleCount;
				visibleCount++;
				yield return new WaitForSeconds(0f);
			}
			yield break;
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x00176A00 File Offset: 0x00174E00
		private IEnumerator RevealWords(TMP_Text textComponent)
		{
			textComponent.ForceMeshUpdate();
			int totalWordCount = textComponent.textInfo.wordCount;
			int totalVisibleCharacters = textComponent.textInfo.characterCount;
			int counter = 0;
			int currentWord = 0;
			int visibleCount = 0;
			for (;;)
			{
				currentWord = counter % (totalWordCount + 1);
				if (currentWord == 0)
				{
					visibleCount = 0;
				}
				else if (currentWord < totalWordCount)
				{
					visibleCount = textComponent.textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
				}
				else if (currentWord == totalWordCount)
				{
					visibleCount = totalVisibleCharacters;
				}
				textComponent.maxVisibleCharacters = visibleCount;
				if (visibleCount >= totalVisibleCharacters)
				{
					yield return new WaitForSeconds(1f);
				}
				counter++;
				yield return new WaitForSeconds(0.1f);
			}
			yield break;
		}

		// Token: 0x04002FBF RID: 12223
		private TMP_Text m_TextComponent;

		// Token: 0x04002FC0 RID: 12224
		private bool hasTextChanged;
	}
}
