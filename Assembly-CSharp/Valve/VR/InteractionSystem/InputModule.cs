using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Valve.VR.InteractionSystem
{
	// Token: 0x02000BAB RID: 2987
	public class InputModule : BaseInputModule
	{
		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06005C9C RID: 23708 RVA: 0x00205E16 File Offset: 0x00204216
		public static InputModule instance
		{
			get
			{
				if (InputModule._instance == null)
				{
					InputModule._instance = UnityEngine.Object.FindObjectOfType<InputModule>();
				}
				return InputModule._instance;
			}
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x00205E37 File Offset: 0x00204237
		public override bool ShouldActivateModule()
		{
			return base.ShouldActivateModule() && this.submitObject != null;
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x00205E54 File Offset: 0x00204254
		public void HoverBegin(GameObject gameObject)
		{
			PointerEventData eventData = new PointerEventData(base.eventSystem);
			ExecuteEvents.Execute<IPointerEnterHandler>(gameObject, eventData, ExecuteEvents.pointerEnterHandler);
		}

		// Token: 0x06005C9F RID: 23711 RVA: 0x00205E7C File Offset: 0x0020427C
		public void HoverEnd(GameObject gameObject)
		{
			ExecuteEvents.Execute<IPointerExitHandler>(gameObject, new PointerEventData(base.eventSystem)
			{
				selectedObject = null
			}, ExecuteEvents.pointerExitHandler);
		}

		// Token: 0x06005CA0 RID: 23712 RVA: 0x00205EA9 File Offset: 0x002042A9
		public void Submit(GameObject gameObject)
		{
			this.submitObject = gameObject;
		}

		// Token: 0x06005CA1 RID: 23713 RVA: 0x00205EB4 File Offset: 0x002042B4
		public override void Process()
		{
			if (this.submitObject)
			{
				BaseEventData baseEventData = this.GetBaseEventData();
				baseEventData.selectedObject = this.submitObject;
				ExecuteEvents.Execute<ISubmitHandler>(this.submitObject, baseEventData, ExecuteEvents.submitHandler);
				this.submitObject = null;
			}
		}

		// Token: 0x04004231 RID: 16945
		private GameObject submitObject;

		// Token: 0x04004232 RID: 16946
		private static InputModule _instance;
	}
}
