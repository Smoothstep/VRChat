using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x020007A8 RID: 1960
	[RequireComponent(typeof(Text))]
	public class TextButtonTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x06003F52 RID: 16210 RVA: 0x0013EAC4 File Offset: 0x0013CEC4
		public void Awake()
		{
			this._text = base.GetComponent<Text>();
		}

		// Token: 0x06003F53 RID: 16211 RVA: 0x0013EAD2 File Offset: 0x0013CED2
		public void OnPointerEnter(PointerEventData eventData)
		{
			this._text.color = this.HoverColor;
		}

		// Token: 0x06003F54 RID: 16212 RVA: 0x0013EAE5 File Offset: 0x0013CEE5
		public void OnPointerExit(PointerEventData eventData)
		{
			this._text.color = this.NormalColor;
		}

		// Token: 0x040027A5 RID: 10149
		private Text _text;

		// Token: 0x040027A6 RID: 10150
		public Color NormalColor = Color.white;

		// Token: 0x040027A7 RID: 10151
		public Color HoverColor = Color.black;
	}
}
