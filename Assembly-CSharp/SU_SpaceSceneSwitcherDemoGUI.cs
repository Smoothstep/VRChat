using System;
using UnityEngine;

// Token: 0x020008D3 RID: 2259
public class SU_SpaceSceneSwitcherDemoGUI : MonoBehaviour
{
	// Token: 0x060044CF RID: 17615 RVA: 0x001704E4 File Offset: 0x0016E8E4
	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 180f, 25f), "Switch to Scene 1 (F1)"))
		{
			SU_SpaceSceneSwitcher.Switch("SpaceScene1");
		}
		if (GUI.Button(new Rect(10f, 40f, 180f, 25f), "Switch to Scene 2 (F2)"))
		{
			SU_SpaceSceneSwitcher.Switch("SpaceScene2");
		}
		SU_CameraFollow component = Camera.main.GetComponent<SU_CameraFollow>();
		if (GUI.Button(new Rect(10f, 70f, 180f, 25f), "Camera " + component.followMode.ToString()))
		{
			if (component.followMode == SU_CameraFollow.FollowMode.CHASE)
			{
				component.followMode = SU_CameraFollow.FollowMode.SPECTATOR;
			}
			else
			{
				component.followMode = SU_CameraFollow.FollowMode.CHASE;
			}
		}
		this._spaceParticles = GUI.Toggle(new Rect(10f, 100f, 180f, 25f), this._spaceParticles, "Space Particles");
		if (this._oldSpaceParticles != this._spaceParticles)
		{
			SU_SpaceSceneSwitcher.SetActive(this.spaceParticles.gameObject, this._spaceParticles);
			this._oldSpaceParticles = this._spaceParticles;
		}
		this._spaceFog = GUI.Toggle(new Rect(10f, 130f, 180f, 25f), this._spaceFog, "Space Fog");
		if (this._oldSpaceFog != this._spaceFog)
		{
			SU_SpaceSceneSwitcher.SetActive(this.spaceFog.gameObject, this._spaceFog);
			this._oldSpaceFog = this._spaceFog;
		}
		this._spaceAsteroids = GUI.Toggle(new Rect(10f, 160f, 180f, 25f), this._spaceAsteroids, "Space Asteroids");
		if (this._oldSpaceAsteroids != this._spaceAsteroids)
		{
			SU_SpaceSceneSwitcher.SetActive(this.spaceAsteroids.gameObject, this._spaceAsteroids);
			this._oldSpaceAsteroids = this._spaceAsteroids;
		}
	}

	// Token: 0x060044D0 RID: 17616 RVA: 0x001706E2 File Offset: 0x0016EAE2
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F1))
		{
			SU_SpaceSceneSwitcher.Switch("SpaceScene1");
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			SU_SpaceSceneSwitcher.Switch("SpaceScene2");
		}
	}

	// Token: 0x04002ECB RID: 11979
	public Transform spaceParticles;

	// Token: 0x04002ECC RID: 11980
	private bool _spaceParticles = true;

	// Token: 0x04002ECD RID: 11981
	private bool _oldSpaceParticles = true;

	// Token: 0x04002ECE RID: 11982
	public Transform spaceFog;

	// Token: 0x04002ECF RID: 11983
	private bool _spaceFog = true;

	// Token: 0x04002ED0 RID: 11984
	private bool _oldSpaceFog = true;

	// Token: 0x04002ED1 RID: 11985
	public Transform spaceAsteroids;

	// Token: 0x04002ED2 RID: 11986
	private bool _spaceAsteroids = true;

	// Token: 0x04002ED3 RID: 11987
	private bool _oldSpaceAsteroids = true;
}
