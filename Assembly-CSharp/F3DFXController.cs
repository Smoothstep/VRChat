using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class F3DFXController : MonoBehaviour
{
	// Token: 0x060027AE RID: 10158 RVA: 0x000CE054 File Offset: 0x000CC454
	private void Awake()
	{
		F3DFXController.instance = this;
		for (int i = 0; i < this.ShellParticles.Length; i++)
		{
			this.ShellParticles[i].enableEmission = false;
			this.ShellParticles[i].gameObject.SetActive(true);
		}
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x000CE0A4 File Offset: 0x000CC4A4
	private void OnGUI()
	{
		GUIStyle guistyle = new GUIStyle(GUI.skin.label);
		guistyle.fontSize = 25;
		guistyle.fontStyle = FontStyle.Bold;
		guistyle.wordWrap = false;
		GUIStyle guistyle2 = new GUIStyle(GUI.skin.label);
		guistyle2.fontSize = 11;
		guistyle2.wordWrap = false;
		GUILayout.BeginArea(new Rect((float)(Screen.width / 2 - 150), (float)(Screen.height - 150), 300f, 120f));
		GUILayout.BeginVertical(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label(this.fxTypeName[(int)this.DefaultFXType], guistyle, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Press Left / Right arrow keys to switch", guistyle2, new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Previous", new GUILayoutOption[]
		{
			GUILayout.Width(90f),
			GUILayout.Height(30f)
		}))
		{
			this.PrevWeapon();
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Next", new GUILayoutOption[]
		{
			GUILayout.Width(90f),
			GUILayout.Height(30f)
		}))
		{
			this.NextWeapon();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x000CE234 File Offset: 0x000CC634
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			this.NextWeapon();
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			this.PrevWeapon();
		}
	}

	// Token: 0x060027B1 RID: 10161 RVA: 0x000CE265 File Offset: 0x000CC665
	private void NextWeapon()
	{
		if (this.DefaultFXType < (F3DFXType)Enum.GetNames(typeof(F3DFXType)).Length - 1)
		{
			this.Stop();
			this.DefaultFXType++;
		}
	}

	// Token: 0x060027B2 RID: 10162 RVA: 0x000CE299 File Offset: 0x000CC699
	private void PrevWeapon()
	{
		if (this.DefaultFXType > F3DFXType.Vulcan)
		{
			this.Stop();
			this.DefaultFXType--;
		}
	}

	// Token: 0x060027B3 RID: 10163 RVA: 0x000CE2BB File Offset: 0x000CC6BB
	private void AdvanceSocket()
	{
		this.curSocket++;
		if (this.curSocket > 3)
		{
			this.curSocket = 0;
		}
	}

	// Token: 0x060027B4 RID: 10164 RVA: 0x000CE2E0 File Offset: 0x000CC6E0
	public void Fire()
	{
		switch (this.DefaultFXType)
		{
		case F3DFXType.Vulcan:
			this.timerID = F3DTime.time.AddTimer(0.05f, new Action(this.Vulcan));
			this.Vulcan();
			break;
		case F3DFXType.SoloGun:
			this.timerID = F3DTime.time.AddTimer(0.2f, new Action(this.SoloGun));
			this.SoloGun();
			break;
		case F3DFXType.Sniper:
			this.timerID = F3DTime.time.AddTimer(0.3f, new Action(this.Sniper));
			this.Sniper();
			break;
		case F3DFXType.ShotGun:
			this.timerID = F3DTime.time.AddTimer(0.3f, new Action(this.ShotGun));
			this.ShotGun();
			break;
		case F3DFXType.Seeker:
			this.timerID = F3DTime.time.AddTimer(0.2f, new Action(this.Seeker));
			this.Seeker();
			break;
		case F3DFXType.RailGun:
			this.timerID = F3DTime.time.AddTimer(0.2f, new Action(this.RailGun));
			this.RailGun();
			break;
		case F3DFXType.PlasmaGun:
			this.timerID = F3DTime.time.AddTimer(0.2f, new Action(this.PlasmaGun));
			this.PlasmaGun();
			break;
		case F3DFXType.PlasmaBeam:
			this.PlasmaBeam();
			break;
		case F3DFXType.PlasmaBeamHeavy:
			this.PlasmaBeamHeavy();
			break;
		case F3DFXType.LightningGun:
			this.LightningGun();
			break;
		case F3DFXType.FlameRed:
			this.FlameRed();
			break;
		case F3DFXType.LaserImpulse:
			this.timerID = F3DTime.time.AddTimer(0.15f, new Action(this.LaserImpulse));
			this.LaserImpulse();
			break;
		}
	}

	// Token: 0x060027B5 RID: 10165 RVA: 0x000CE4C0 File Offset: 0x000CC8C0
	public void Stop()
	{
		if (this.timerID != -1)
		{
			F3DTime.time.RemoveTimer(this.timerID);
			this.timerID = -1;
		}
	}

	// Token: 0x060027B6 RID: 10166 RVA: 0x000CE4E8 File Offset: 0x000CC8E8
	private void Vulcan()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(this.vulcanMuzzle, this.TurretSocket[this.curSocket].position, this.TurretSocket[this.curSocket].rotation, this.TurretSocket[this.curSocket]);
		F3DPool.instance.Spawn(this.vulcanProjectile, this.TurretSocket[this.curSocket].position + this.TurretSocket[this.curSocket].forward, lhs * this.TurretSocket[this.curSocket].rotation, null);
		this.ShellParticles[this.curSocket].Emit(1);
		F3DAudioController.instance.VulcanShot(this.TurretSocket[this.curSocket].position);
		this.AdvanceSocket();
	}

	// Token: 0x060027B7 RID: 10167 RVA: 0x000CE5CA File Offset: 0x000CC9CA
	public void VulcanImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(this.vulcanImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.VulcanHit(pos);
	}

	// Token: 0x060027B8 RID: 10168 RVA: 0x000CE5F0 File Offset: 0x000CC9F0
	private void SoloGun()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(this.soloGunMuzzle, this.TurretSocket[this.curSocket].position, this.TurretSocket[this.curSocket].rotation, this.TurretSocket[this.curSocket]);
		F3DPool.instance.Spawn(this.soloGunProjectile, this.TurretSocket[this.curSocket].position + this.TurretSocket[this.curSocket].forward, lhs * this.TurretSocket[this.curSocket].rotation, null);
		F3DAudioController.instance.SoloGunShot(this.TurretSocket[this.curSocket].position);
		this.AdvanceSocket();
	}

	// Token: 0x060027B9 RID: 10169 RVA: 0x000CE6BF File Offset: 0x000CCABF
	public void SoloGunImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(this.soloGunImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.SoloGunHit(pos);
	}

	// Token: 0x060027BA RID: 10170 RVA: 0x000CE6E4 File Offset: 0x000CCAE4
	private void Sniper()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(this.sniperMuzzle, this.TurretSocket[this.curSocket].position, this.TurretSocket[this.curSocket].rotation, this.TurretSocket[this.curSocket]);
		F3DPool.instance.Spawn(this.sniperBeam, this.TurretSocket[this.curSocket].position, lhs * this.TurretSocket[this.curSocket].rotation, null);
		F3DAudioController.instance.SniperShot(this.TurretSocket[this.curSocket].position);
		this.ShellParticles[this.curSocket].Emit(1);
		this.AdvanceSocket();
	}

	// Token: 0x060027BB RID: 10171 RVA: 0x000CE7AF File Offset: 0x000CCBAF
	public void SniperImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(this.sniperImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.SniperHit(pos);
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x000CE7D4 File Offset: 0x000CCBD4
	private void ShotGun()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(this.shotGunMuzzle, this.TurretSocket[this.curSocket].position, this.TurretSocket[this.curSocket].rotation, this.TurretSocket[this.curSocket]);
		F3DPool.instance.Spawn(this.shotGunProjectile, this.TurretSocket[this.curSocket].position, lhs * this.TurretSocket[this.curSocket].rotation, null);
		F3DAudioController.instance.ShotGunShot(this.TurretSocket[this.curSocket].position);
		this.ShellParticles[this.curSocket].Emit(1);
		this.AdvanceSocket();
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x000CE8A0 File Offset: 0x000CCCA0
	private void Seeker()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(this.seekerMuzzle, this.TurretSocket[this.curSocket].position, this.TurretSocket[this.curSocket].rotation, this.TurretSocket[this.curSocket]);
		F3DPool.instance.Spawn(this.seekerProjectile, this.TurretSocket[this.curSocket].position, lhs * this.TurretSocket[this.curSocket].rotation, null);
		F3DAudioController.instance.SeekerShot(this.TurretSocket[this.curSocket].position);
		this.AdvanceSocket();
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x000CE958 File Offset: 0x000CCD58
	public void SeekerImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(this.seekerImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.SeekerHit(pos);
	}

	// Token: 0x060027BF RID: 10175 RVA: 0x000CE980 File Offset: 0x000CCD80
	private void RailGun()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(this.railgunMuzzle, this.TurretSocket[this.curSocket].position, this.TurretSocket[this.curSocket].rotation, this.TurretSocket[this.curSocket]);
		F3DPool.instance.Spawn(this.railgunBeam, this.TurretSocket[this.curSocket].position, lhs * this.TurretSocket[this.curSocket].rotation, null);
		F3DAudioController.instance.RailGunShot(this.TurretSocket[this.curSocket].position);
		this.ShellParticles[this.curSocket].Emit(1);
		this.AdvanceSocket();
	}

	// Token: 0x060027C0 RID: 10176 RVA: 0x000CEA4B File Offset: 0x000CCE4B
	public void RailgunImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(this.railgunImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.RailGunHit(pos);
	}

	// Token: 0x060027C1 RID: 10177 RVA: 0x000CEA70 File Offset: 0x000CCE70
	private void PlasmaGun()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(this.plasmagunMuzzle, this.TurretSocket[this.curSocket].position, this.TurretSocket[this.curSocket].rotation, this.TurretSocket[this.curSocket]);
		F3DPool.instance.Spawn(this.plasmagunProjectile, this.TurretSocket[this.curSocket].position, lhs * this.TurretSocket[this.curSocket].rotation, null);
		F3DAudioController.instance.PlasmaGunShot(this.TurretSocket[this.curSocket].position);
		this.AdvanceSocket();
	}

	// Token: 0x060027C2 RID: 10178 RVA: 0x000CEB28 File Offset: 0x000CCF28
	public void PlasmaGunImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(this.plasmagunImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.PlasmaGunHit(pos);
	}

	// Token: 0x060027C3 RID: 10179 RVA: 0x000CEB50 File Offset: 0x000CCF50
	private void PlasmaBeam()
	{
		F3DPool.instance.Spawn(this.plasmaBeam, this.TurretSocket[0].position, this.TurretSocket[0].rotation, this.TurretSocket[0]);
		F3DPool.instance.Spawn(this.plasmaBeam, this.TurretSocket[2].position, this.TurretSocket[2].rotation, this.TurretSocket[2]);
	}

	// Token: 0x060027C4 RID: 10180 RVA: 0x000CEBC4 File Offset: 0x000CCFC4
	private void PlasmaBeamHeavy()
	{
		F3DPool.instance.Spawn(this.plasmaBeamHeavy, this.TurretSocket[0].position, this.TurretSocket[0].rotation, this.TurretSocket[0]);
		F3DPool.instance.Spawn(this.plasmaBeamHeavy, this.TurretSocket[2].position, this.TurretSocket[2].rotation, this.TurretSocket[2]);
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x000CEC38 File Offset: 0x000CD038
	private void LightningGun()
	{
		F3DPool.instance.Spawn(this.lightningGunBeam, this.TurretSocket[0].position, this.TurretSocket[0].rotation, this.TurretSocket[0]);
		F3DPool.instance.Spawn(this.lightningGunBeam, this.TurretSocket[2].position, this.TurretSocket[2].rotation, this.TurretSocket[2]);
	}

	// Token: 0x060027C6 RID: 10182 RVA: 0x000CECAC File Offset: 0x000CD0AC
	private void FlameRed()
	{
		F3DPool.instance.Spawn(this.flameRed, this.TurretSocket[0].position, this.TurretSocket[0].rotation, this.TurretSocket[0]);
		F3DPool.instance.Spawn(this.flameRed, this.TurretSocket[2].position, this.TurretSocket[2].rotation, this.TurretSocket[2]);
	}

	// Token: 0x060027C7 RID: 10183 RVA: 0x000CED20 File Offset: 0x000CD120
	private void LaserImpulse()
	{
		Quaternion lhs = Quaternion.Euler(UnityEngine.Random.onUnitSphere);
		F3DPool.instance.Spawn(this.laserImpulseMuzzle, this.TurretSocket[this.curSocket].position, this.TurretSocket[this.curSocket].rotation, this.TurretSocket[this.curSocket]);
		F3DPool.instance.Spawn(this.laserImpulseProjectile, this.TurretSocket[this.curSocket].position, lhs * this.TurretSocket[this.curSocket].rotation, null);
		F3DAudioController.instance.LaserImpulseShot(this.TurretSocket[this.curSocket].position);
		this.AdvanceSocket();
	}

	// Token: 0x060027C8 RID: 10184 RVA: 0x000CEDD8 File Offset: 0x000CD1D8
	public void LaserImpulseImpact(Vector3 pos)
	{
		F3DPool.instance.Spawn(this.laserImpulseImpact, pos, Quaternion.identity, null);
		F3DAudioController.instance.LaserImpulseHit(pos);
	}

	// Token: 0x040015C2 RID: 5570
	public static F3DFXController instance;

	// Token: 0x040015C3 RID: 5571
	private string[] fxTypeName = new string[]
	{
		"Vulcan",
		"Sologun",
		"Sniper",
		"Shotgun",
		"Seeker",
		"Railgun",
		"Plasmagun",
		"Plasma beam",
		"Heavy plasma beam",
		"Lightning gun",
		"Flamethrower",
		"Pulse laser"
	};

	// Token: 0x040015C4 RID: 5572
	private int curSocket;

	// Token: 0x040015C5 RID: 5573
	private int timerID = -1;

	// Token: 0x040015C6 RID: 5574
	[Header("Turret setup")]
	public Transform[] TurretSocket;

	// Token: 0x040015C7 RID: 5575
	public ParticleSystem[] ShellParticles;

	// Token: 0x040015C8 RID: 5576
	public F3DFXType DefaultFXType;

	// Token: 0x040015C9 RID: 5577
	[Header("Vulcan")]
	public Transform vulcanProjectile;

	// Token: 0x040015CA RID: 5578
	public Transform vulcanMuzzle;

	// Token: 0x040015CB RID: 5579
	public Transform vulcanImpact;

	// Token: 0x040015CC RID: 5580
	[Header("Solo gun")]
	public Transform soloGunProjectile;

	// Token: 0x040015CD RID: 5581
	public Transform soloGunMuzzle;

	// Token: 0x040015CE RID: 5582
	public Transform soloGunImpact;

	// Token: 0x040015CF RID: 5583
	[Header("Sniper")]
	public Transform sniperBeam;

	// Token: 0x040015D0 RID: 5584
	public Transform sniperMuzzle;

	// Token: 0x040015D1 RID: 5585
	public Transform sniperImpact;

	// Token: 0x040015D2 RID: 5586
	[Header("Shotgun")]
	public Transform shotGunProjectile;

	// Token: 0x040015D3 RID: 5587
	public Transform shotGunMuzzle;

	// Token: 0x040015D4 RID: 5588
	public Transform shotGunImpact;

	// Token: 0x040015D5 RID: 5589
	[Header("Seeker")]
	public Transform seekerProjectile;

	// Token: 0x040015D6 RID: 5590
	public Transform seekerMuzzle;

	// Token: 0x040015D7 RID: 5591
	public Transform seekerImpact;

	// Token: 0x040015D8 RID: 5592
	[Header("Rail gun")]
	public Transform railgunBeam;

	// Token: 0x040015D9 RID: 5593
	public Transform railgunMuzzle;

	// Token: 0x040015DA RID: 5594
	public Transform railgunImpact;

	// Token: 0x040015DB RID: 5595
	[Header("Plasma gun")]
	public Transform plasmagunProjectile;

	// Token: 0x040015DC RID: 5596
	public Transform plasmagunMuzzle;

	// Token: 0x040015DD RID: 5597
	public Transform plasmagunImpact;

	// Token: 0x040015DE RID: 5598
	[Header("Plasma beam")]
	public Transform plasmaBeam;

	// Token: 0x040015DF RID: 5599
	[Header("Plasma beam heavy")]
	public Transform plasmaBeamHeavy;

	// Token: 0x040015E0 RID: 5600
	[Header("Lightning gun")]
	public Transform lightningGunBeam;

	// Token: 0x040015E1 RID: 5601
	[Header("Flame")]
	public Transform flameRed;

	// Token: 0x040015E2 RID: 5602
	[Header("Laser impulse")]
	public Transform laserImpulseProjectile;

	// Token: 0x040015E3 RID: 5603
	public Transform laserImpulseMuzzle;

	// Token: 0x040015E4 RID: 5604
	public Transform laserImpulseImpact;
}
