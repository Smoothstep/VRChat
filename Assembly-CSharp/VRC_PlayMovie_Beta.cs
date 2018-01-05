using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using VRCSDK2;

// Token: 0x02000B72 RID: 2930
public class VRC_PlayMovie_Beta : MonoBehaviour
{
	// Token: 0x06005A92 RID: 23186 RVA: 0x001F8E08 File Offset: 0x001F7208
	private void Awake()
	{
		string path = Application.persistentDataPath + "/Movies";
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		if (this.screen == null)
		{
			this.screen = base.gameObject;
		}
		if (this.screen.GetComponent<Renderer>())
		{
			this.screenMaterial = this.screen.GetComponent<Renderer>().material;
		}
		else
		{
			Debug.LogWarning("VRC_PlayMovie_Beta: A object with a Mesh Renderer is required for the \"screen\" variable.");
		}
		this.screenMaxWidth = this.screen.transform.localScale.x;
		this.screenMaxHeight = this.screen.transform.localScale.z;
	}

	// Token: 0x06005A93 RID: 23187 RVA: 0x001F8ECC File Offset: 0x001F72CC
	private void Start()
	{
		if (this.screenMaterial)
		{
			this.originalTexture = this.screenMaterial.mainTexture;
			if (this.speakers == null)
			{
				this.speakers = base.gameObject;
			}
			if ((this.audioSource = this.speakers.GetComponent<AudioSource>()) == null)
			{
				this.audioSource = this.speakers.AddComponent<AudioSource>();
			}
			this.audioSource.playOnAwake = false;
			this.audioSource.volume = this.defaultVolume;
			if (this.video)
			{
				this.screenMaterial.mainTexture = this.video;
				this.audioSource.clip = this.video.audioClip;
				this.video.loop = this.loop;
			}
			else if (!string.IsNullOrEmpty(this.streamedVideoURL))
			{
				this.LoadSteamedMovie(this.streamedVideoURL);
			}
			if (this.playOnAwake)
			{
				this.Play();
			}
		}
		else
		{
			Debug.LogWarning("VRC_PlayMovie_Beta: A material is required for the \"screenMaterial\" variable. Do you have a object with a Mesh Renderer Selected?");
		}
	}

	// Token: 0x06005A94 RID: 23188 RVA: 0x001F8FED File Offset: 0x001F73ED
	private void Update()
	{
	}

	// Token: 0x06005A95 RID: 23189 RVA: 0x001F8FF0 File Offset: 0x001F73F0
	private void TriggerEvent(string name, VRC_EventHandler.VrcBroadcastType bc)
	{
		VRC_EventHandler componentInParent = base.GetComponentInParent<VRC_EventHandler>();
		foreach (VRC_EventHandler.VrcEvent e2 in from e in componentInParent.Events
		where e.Name == name
		select e)
		{
			componentInParent.TriggerEvent(e2, bc, null, 0f);
		}
	}

	// Token: 0x06005A96 RID: 23190 RVA: 0x001F9078 File Offset: 0x001F7478
	private void Play()
	{
		if (this.video != null && !this.video.isPlaying)
		{
			if (this.currentStatusMessage)
			{
				UnityEngine.Object.Destroy(this.currentStatusMessage);
			}
			this.screenMaterial.mainTexture = this.video;
			this.video.Play();
			this.audioSource.Play();
			this.AdjustScreenToAspectRatio(this.video.width, this.video.height);
		}
	}

	// Token: 0x06005A97 RID: 23191 RVA: 0x001F9104 File Offset: 0x001F7504
	private void Pause()
	{
		if (this.video != null)
		{
			if (this.isPaused)
			{
				this.Play();
				this.isPaused = false;
			}
			else
			{
				this.video.Pause();
				this.audioSource.Pause();
				this.isPaused = true;
			}
		}
	}

	// Token: 0x06005A98 RID: 23192 RVA: 0x001F915C File Offset: 0x001F755C
	private void Stop()
	{
		if (this.video != null)
		{
			this.video.Stop();
			this.screenMaterial.mainTexture = this.originalTexture;
		}
	}

	// Token: 0x06005A99 RID: 23193 RVA: 0x001F918B File Offset: 0x001F758B
	private void Mute()
	{
		if (this.audioSource.mute)
		{
			this.audioSource.mute = false;
		}
		else
		{
			this.audioSource.mute = true;
		}
	}

	// Token: 0x06005A9A RID: 23194 RVA: 0x001F91BA File Offset: 0x001F75BA
	private void VolumeUp()
	{
		this.audioSource.volume += this.volumeIncrimentValue;
	}

	// Token: 0x06005A9B RID: 23195 RVA: 0x001F91D4 File Offset: 0x001F75D4
	private void VolumeDown()
	{
		this.audioSource.volume -= this.volumeIncrimentValue;
	}

	// Token: 0x06005A9C RID: 23196 RVA: 0x001F91EE File Offset: 0x001F75EE
	private void LoadSteamedMovie(string url)
	{
		base.StartCoroutine(this.LoadSteamedMovie_(url, true));
	}

	// Token: 0x06005A9D RID: 23197 RVA: 0x001F9200 File Offset: 0x001F7600
	private IEnumerator LoadSteamedMovie_(string url, bool ignoreThisVariable)
	{
		if (this.video && this.video.isPlaying)
		{
			this.video.Stop();
		}
		this.screenMaterial.mainTexture = this.originalTexture;
		this.videoBufferPercent = 0;
		int index = Path.GetFileName(url).IndexOfAny(new char[]
		{
			'\\',
			'/',
			':',
			'*',
			'?',
			'<',
			'>',
			'|'
		});
		string movieFile;
		if (index == -1)
		{
			movieFile = Application.persistentDataPath + "/Movies/" + Path.GetFileName(url);
		}
		else
		{
			movieFile = Application.persistentDataPath + "/Movies/" + Path.GetFileName(url).Remove(index);
		}
		bool movieExisits = File.Exists(movieFile);
		if (movieExisits)
		{
			url = "file:///" + movieFile;
		}
		GameObject status = this.DisplayStatusMessage("%0");
		TextMesh statusText = status.GetComponent<TextMesh>();
		WWW www = new WWW(url);
		if (!string.IsNullOrEmpty(www.error) && status)
		{
			statusText.text = "ERROR";
		}
		if (this.video)
		{
			Resources.UnloadUnusedAssets();
			GC.Collect();
		}
		this.video = www.GetMovieTexture();
		this.audioSource.clip = this.video.audioClip;
		while (!www.isDone)
		{
			this.videoBufferPercent = Mathf.RoundToInt(www.progress * 100f);
			if (status)
			{
				statusText.text = "%" + this.videoBufferPercent.ToString();
			}
			yield return null;
		}
		if (!movieExisits)
		{
			FileStream fileStream = new FileStream(movieFile, FileMode.Create);
			fileStream.Write(www.bytes, 0, www.bytes.Length);
			fileStream.Flush();
			fileStream.Close();
		}
		if (status)
		{
			statusText.text = "Ready";
		}
		if (this.autoPlayStreamedVideo)
		{
			this.Play();
		}
		yield break;
	}

	// Token: 0x06005A9E RID: 23198 RVA: 0x001F9222 File Offset: 0x001F7622
	private int StreamedVideoBufferPercent()
	{
		return this.videoBufferPercent;
	}

	// Token: 0x06005A9F RID: 23199 RVA: 0x001F922C File Offset: 0x001F762C
	private GameObject DisplayStatusMessage(string statusMessage)
	{
		if (this.currentStatusMessage)
		{
			UnityEngine.Object.Destroy(this.currentStatusMessage);
		}
		GameObject gameObject = new GameObject();
		gameObject.name = "Status";
		gameObject.transform.position = base.transform.position;
		TextMesh textMesh = gameObject.AddComponent<TextMesh>();
		textMesh.text = statusMessage;
		textMesh.characterSize = 0.25f;
		textMesh.fontSize = 36;
		textMesh.color = Color.black;
		textMesh.anchor = TextAnchor.MiddleCenter;
		Font font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		textMesh.font = font;
		gameObject.GetComponent<Renderer>().material = font.material;
		this.currentStatusMessage = gameObject;
		return gameObject;
	}

	// Token: 0x06005AA0 RID: 23200 RVA: 0x001F92E8 File Offset: 0x001F76E8
	private void AdjustScreenToAspectRatio(int w, int h)
	{
		Vector3 localScale = this.screen.transform.localScale;
		int num = this.GCD(w, h);
		int num2 = w / num;
		int num3 = h / num;
		float num4 = this.screenMaxWidth / (float)num2;
		float num5 = this.screenMaxHeight / (float)num3;
		if (num4 <= num5)
		{
			localScale.x = this.screenMaxWidth;
			localScale.z = (float)num3 * (this.screenMaxWidth / (float)num2);
		}
		else
		{
			localScale.x = (float)num2 * (this.screenMaxHeight / (float)num3);
			localScale.z = this.screenMaxHeight;
		}
		this.screen.transform.localScale = localScale;
	}

	// Token: 0x06005AA1 RID: 23201 RVA: 0x001F938E File Offset: 0x001F778E
	private int GCD(int a, int b)
	{
		return (b != 0) ? this.GCD(b, a % b) : a;
	}

	// Token: 0x04004069 RID: 16489
	public GameObject screen;

	// Token: 0x0400406A RID: 16490
	public MovieTexture video;

	// Token: 0x0400406B RID: 16491
	public GameObject speakers;

	// Token: 0x0400406C RID: 16492
	public bool playOnAwake = true;

	// Token: 0x0400406D RID: 16493
	public bool loop = true;

	// Token: 0x0400406E RID: 16494
	public float defaultVolume = 0.25f;

	// Token: 0x0400406F RID: 16495
	public string streamedVideoURL;

	// Token: 0x04004070 RID: 16496
	[HideInInspector]
	public bool autoPlayStreamedVideo;

	// Token: 0x04004071 RID: 16497
	private Material screenMaterial;

	// Token: 0x04004072 RID: 16498
	private AudioSource audioSource;

	// Token: 0x04004073 RID: 16499
	private Texture originalTexture;

	// Token: 0x04004074 RID: 16500
	private bool isPaused;

	// Token: 0x04004075 RID: 16501
	private float volumeIncrimentValue = 0.025f;

	// Token: 0x04004076 RID: 16502
	private GameObject currentStatusMessage;

	// Token: 0x04004077 RID: 16503
	private int videoBufferPercent = -1;

	// Token: 0x04004078 RID: 16504
	private float screenMaxWidth;

	// Token: 0x04004079 RID: 16505
	private float screenMaxHeight;
}
