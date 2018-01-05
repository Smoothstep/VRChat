using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExitGames.UtilityScripts
{
	// Token: 0x020007A9 RID: 1961
	[RequireComponent(typeof(Text))]
	public class TextToggleIsOnTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
	{
		// Token: 0x06003F56 RID: 16214 RVA: 0x0013EB2C File Offset: 0x0013CF2C
		public void OnEnable()
		{
			this._text = base.GetComponent<Text>();
			this.toggle.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x0013EB56 File Offset: 0x0013CF56
		public void OnDisable()
		{
			this.toggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
		}

		// Token: 0x06003F58 RID: 16216 RVA: 0x0013EB74 File Offset: 0x0013CF74
		public void OnValueChanged(bool isOn)
		{
			this._text.color = ((!isOn) ? ((!this.isHover) ? this.NormalOffColor : this.NormalOnColor) : ((!this.isHover) ? this.HoverOffColor : this.HoverOnColor));
		}

		// Token: 0x06003F59 RID: 16217 RVA: 0x0013EBCF File Offset: 0x0013CFCF
		public void OnPointerEnter(PointerEventData eventData)
		{
			this.isHover = true;
			this._text.color = ((!this.toggle.isOn) ? this.HoverOffColor : this.HoverOnColor);
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x0013EC04 File Offset: 0x0013D004
		public void OnPointerExit(PointerEventData eventData)
		{
			this.isHover = false;
			this._text.color = ((!this.toggle.isOn) ? this.NormalOffColor : this.NormalOnColor);
		}

		// Token: 0x040027A8 RID: 10152
		public Toggle toggle;

		// Token: 0x040027A9 RID: 10153
		private Text _text;

		// Token: 0x040027AA RID: 10154
		public Color NormalOnColor = Color.white;

		// Token: 0x040027AB RID: 10155
		public Color NormalOffColor = Color.black;

		// Token: 0x040027AC RID: 10156
		public Color HoverOnColor = Color.black;

		// Token: 0x040027AD RID: 10157
		public Color HoverOffColor = Color.black;

		// Token: 0x040027AE RID: 10158
		private bool isHover;
	}
}
