using System;
using UnityEngine;

// Token: 0x0200088A RID: 2186
public class MyGui : MonoBehaviour
{
	// Token: 0x0600434B RID: 17227 RVA: 0x00162128 File Offset: 0x00160528
	private void Start()
	{
		this.oldAmbientColor = RenderSettings.ambientLight;
		this.oldLightIntensity = this.DirLight.intensity;
		this.anim = this.Target.GetComponent<Animator>();
		this.guiStyleHeader.fontSize = 14;
		this.guiStyleHeader.normal.textColor = new Color(1f, 1f, 1f);
		EffectSettings component = this.Prefabs[this.current].GetComponent<EffectSettings>();
		if (component != null)
		{
			this.prefabSpeed = component.MoveSpeed;
		}
		this.current = this.CurrentPrefabNomber;
		this.InstanceCurrent(this.GuiStats[this.CurrentPrefabNomber]);
	}

	// Token: 0x0600434C RID: 17228 RVA: 0x001621E0 File Offset: 0x001605E0
	private void InstanceEffect(Vector3 pos)
	{
		this.currentGo = UnityEngine.Object.Instantiate<GameObject>(this.Prefabs[this.current], pos, this.Prefabs[this.current].transform.rotation);
		this.effectSettings = this.currentGo.GetComponent<EffectSettings>();
		this.effectSettings.Target = this.Target;
		if (this.isHomingMove)
		{
			this.effectSettings.IsHomingMove = this.isHomingMove;
		}
		this.prefabSpeed = this.effectSettings.MoveSpeed;
		this.effectSettings.EffectDeactivated += this.effectSettings_EffectDeactivated;
		this.currentGo.transform.parent = base.transform;
	}

	// Token: 0x0600434D RID: 17229 RVA: 0x0016229C File Offset: 0x0016069C
	private void InstanceDefaulBall()
	{
		this.defaultBall = UnityEngine.Object.Instantiate<GameObject>(this.Prefabs[1], base.transform.position, this.Prefabs[1].transform.rotation);
		this.defaultBallEffectSettings = this.defaultBall.GetComponent<EffectSettings>();
		this.defaultBallEffectSettings.Target = this.Target;
		this.defaultBallEffectSettings.EffectDeactivated += this.defaultBall_EffectDeactivated;
		this.defaultBall.transform.parent = base.transform;
	}

	// Token: 0x0600434E RID: 17230 RVA: 0x00162328 File Offset: 0x00160728
	private void defaultBall_EffectDeactivated(object sender, EventArgs e)
	{
		this.defaultBall.transform.position = base.transform.position;
		this.isReadyDefaulBall = true;
	}

	// Token: 0x0600434F RID: 17231 RVA: 0x0016234C File Offset: 0x0016074C
	private void effectSettings_EffectDeactivated(object sender, EventArgs e)
	{
		this.currentGo.transform.position = this.GetInstancePosition(this.GuiStats[this.current]);
		this.isReadyEffect = true;
	}

	// Token: 0x06004350 RID: 17232 RVA: 0x00162378 File Offset: 0x00160778
	private void OnGUI()
	{
		if (!this.UseGui)
		{
			return;
		}
		if (GUI.Button(new Rect(10f, 15f, 105f, 30f), "Previous Effect"))
		{
			this.ChangeCurrent(-1);
		}
		if (GUI.Button(new Rect(130f, 15f, 105f, 30f), "Next Effect"))
		{
			this.ChangeCurrent(1);
		}
		if (this.Prefabs[this.current] != null)
		{
			GUI.Label(new Rect(300f, 15f, 100f, 20f), "Prefab name is \"" + this.Prefabs[this.current].name + "\"  \r\nHold any mouse button that would move the camera", this.guiStyleHeader);
		}
		if (GUI.Button(new Rect(10f, 60f, 225f, 30f), "Day/Night"))
		{
			this.DirLight.intensity = (this.isDay ? this.oldLightIntensity : 0f);
			RenderSettings.ambientLight = (this.isDay ? this.oldAmbientColor : new Color(0.1f, 0.1f, 0.1f));
			this.isDay = !this.isDay;
		}
		if (GUI.Button(new Rect(10f, 105f, 225f, 30f), "Change environment"))
		{
			if (this.isDefaultPlaneTexture)
			{
				this.Plane1.GetComponent<Renderer>().material = this.PlaneMaterials[0];
				this.Plane2.GetComponent<Renderer>().material = this.PlaneMaterials[0];
			}
			else
			{
				this.Plane1.GetComponent<Renderer>().material = this.PlaneMaterials[1];
				this.Plane2.GetComponent<Renderer>().material = this.PlaneMaterials[2];
			}
			this.isDefaultPlaneTexture = !this.isDefaultPlaneTexture;
		}
		if (this.current <= 15)
		{
			GUI.Label(new Rect(10f, 152f, 225f, 30f), "Ball Speed " + (int)this.prefabSpeed + "m", this.guiStyleHeader);
			this.prefabSpeed = GUI.HorizontalSlider(new Rect(115f, 155f, 120f, 30f), this.prefabSpeed, 1f, 30f);
			this.isHomingMove = GUI.Toggle(new Rect(10f, 190f, 150f, 30f), this.isHomingMove, " Is Homing Move");
			this.effectSettings.MoveSpeed = this.prefabSpeed;
		}
	}

	// Token: 0x06004351 RID: 17233 RVA: 0x00162640 File Offset: 0x00160A40
	private void Update()
	{
		if (this.anim != null)
		{
			this.anim.enabled = this.isHomingMove;
		}
		this.effectSettings.IsHomingMove = this.isHomingMove;
		this.timeleft -= Time.deltaTime;
		this.accum += Time.timeScale / Time.deltaTime;
		this.frames++;
		if ((double)this.timeleft <= 0.0)
		{
			this.timeleft = this.UpdateInterval;
			this.frames = 0;
		}
		if (this.isReadyEffect)
		{
			this.isReadyEffect = false;
			this.currentGo.SetActive(true);
		}
		if (this.isReadyDefaulBall)
		{
			this.isReadyDefaulBall = false;
			this.defaultBall.SetActive(true);
		}
	}

	// Token: 0x06004352 RID: 17234 RVA: 0x0016271C File Offset: 0x00160B1C
	private void InstanceCurrent(MyGui.GuiStat stat)
	{
		switch (stat)
		{
		case MyGui.GuiStat.Ball:
			this.InstanceEffect(base.transform.position);
			break;
		case MyGui.GuiStat.Bottom:
			this.InstanceEffect(this.BottomPosition.transform.position);
			break;
		case MyGui.GuiStat.Middle:
			this.MiddlePosition.SetActive(true);
			this.InstanceEffect(this.MiddlePosition.transform.position);
			break;
		case MyGui.GuiStat.Top:
			this.InstanceEffect(this.TopPosition.transform.position);
			break;
		}
	}

	// Token: 0x06004353 RID: 17235 RVA: 0x001627B8 File Offset: 0x00160BB8
	private Vector3 GetInstancePosition(MyGui.GuiStat stat)
	{
		switch (stat)
		{
		case MyGui.GuiStat.Ball:
			return base.transform.position;
		case MyGui.GuiStat.Bottom:
			return this.BottomPosition.transform.position;
		case MyGui.GuiStat.Middle:
			return this.MiddlePosition.transform.position;
		case MyGui.GuiStat.Top:
			return this.TopPosition.transform.position;
		default:
			return base.transform.position;
		}
	}

	// Token: 0x06004354 RID: 17236 RVA: 0x0016282C File Offset: 0x00160C2C
	private void ChangeCurrent(int delta)
	{
		UnityEngine.Object.Destroy(this.currentGo);
		UnityEngine.Object.Destroy(this.defaultBall);
		base.CancelInvoke("InstanceDefaulBall");
		this.current += delta;
		if (this.current > this.Prefabs.Length - 1)
		{
			this.current = 0;
		}
		else if (this.current < 0)
		{
			this.current = this.Prefabs.Length - 1;
		}
		if (this.effectSettings != null)
		{
			this.effectSettings.EffectDeactivated -= this.effectSettings_EffectDeactivated;
		}
		if (this.defaultBallEffectSettings != null)
		{
			this.defaultBallEffectSettings.EffectDeactivated -= this.effectSettings_EffectDeactivated;
		}
		this.MiddlePosition.SetActive(this.GuiStats[this.current] == MyGui.GuiStat.Middle);
		if (this.GuiStats[this.current] == MyGui.GuiStat.Middle)
		{
			base.Invoke("InstanceDefaulBall", 2f);
		}
		this.InstanceEffect(this.GetInstancePosition(this.GuiStats[this.current]));
	}

	// Token: 0x04002B91 RID: 11153
	public bool UseGui = true;

	// Token: 0x04002B92 RID: 11154
	public int CurrentPrefabNomber;

	// Token: 0x04002B93 RID: 11155
	private float UpdateInterval = 1f;

	// Token: 0x04002B94 RID: 11156
	public Light DirLight;

	// Token: 0x04002B95 RID: 11157
	public GameObject Target;

	// Token: 0x04002B96 RID: 11158
	public GameObject TopPosition;

	// Token: 0x04002B97 RID: 11159
	public GameObject MiddlePosition;

	// Token: 0x04002B98 RID: 11160
	public GameObject BottomPosition;

	// Token: 0x04002B99 RID: 11161
	public GameObject Plane1;

	// Token: 0x04002B9A RID: 11162
	public GameObject Plane2;

	// Token: 0x04002B9B RID: 11163
	public Material[] PlaneMaterials;

	// Token: 0x04002B9C RID: 11164
	public MyGui.GuiStat[] GuiStats;

	// Token: 0x04002B9D RID: 11165
	public float[] Times;

	// Token: 0x04002B9E RID: 11166
	public GameObject[] Prefabs;

	// Token: 0x04002B9F RID: 11167
	private float oldLightIntensity;

	// Token: 0x04002BA0 RID: 11168
	private Color oldAmbientColor;

	// Token: 0x04002BA1 RID: 11169
	private GameObject currentGo;

	// Token: 0x04002BA2 RID: 11170
	private GameObject defaultBall;

	// Token: 0x04002BA3 RID: 11171
	private bool isDay;

	// Token: 0x04002BA4 RID: 11172
	private bool isHomingMove;

	// Token: 0x04002BA5 RID: 11173
	private bool isDefaultPlaneTexture;

	// Token: 0x04002BA6 RID: 11174
	private int current;

	// Token: 0x04002BA7 RID: 11175
	private Animator anim;

	// Token: 0x04002BA8 RID: 11176
	private float prefabSpeed = 4f;

	// Token: 0x04002BA9 RID: 11177
	private EffectSettings effectSettings;

	// Token: 0x04002BAA RID: 11178
	private EffectSettings defaultBallEffectSettings;

	// Token: 0x04002BAB RID: 11179
	private bool isReadyEffect;

	// Token: 0x04002BAC RID: 11180
	private bool isReadyDefaulBall;

	// Token: 0x04002BAD RID: 11181
	private float accum;

	// Token: 0x04002BAE RID: 11182
	private int frames;

	// Token: 0x04002BAF RID: 11183
	private float timeleft;

	// Token: 0x04002BB0 RID: 11184
	private GUIStyle guiStyleHeader = new GUIStyle();

	// Token: 0x0200088B RID: 2187
	public enum GuiStat
	{
		// Token: 0x04002BB2 RID: 11186
		Ball,
		// Token: 0x04002BB3 RID: 11187
		Bottom,
		// Token: 0x04002BB4 RID: 11188
		Middle,
		// Token: 0x04002BB5 RID: 11189
		Top
	}
}
