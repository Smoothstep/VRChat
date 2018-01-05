using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Eyeshot360cam
{
	// Token: 0x02000470 RID: 1136
	public class ReadPanoConfig : MonoBehaviour
	{
		// Token: 0x0600276F RID: 10095 RVA: 0x000CB594 File Offset: 0x000C9994
		private void Start()
		{
			if (Application.isEditor && !this.useAtEditor)
			{
				return;
			}
			Eyeshot360cam component = base.GetComponent<Eyeshot360cam>();
			string text = this.iniPath;
			if (text.Equals(string.Empty))
			{
				string str = "VRCHIVE.ini";
				text = Application.dataPath + "/" + str;
			}
			if (File.Exists(text))
			{
				foreach (string text2 in File.ReadAllLines(text))
				{
					if (!text2.Trim().Equals(string.Empty))
					{
						if (text2.Contains("="))
						{
							string[] array2 = text2.Split(new char[]
							{
								'='
							}, 2);
							string text3 = array2[0].Trim();
							string text4 = array2[1].Trim();
							switch (text3)
							{
							case "Panorama Name":
								if ((this.parametersForInit & Eyeshot360camParameter.PanoramaName) != (Eyeshot360camParameter)0)
								{
									component.panoramaName = ((!string.IsNullOrEmpty(text4)) ? text4 : "default_panorama_name");
								}
								break;
							case "Quality Setting":
								if ((this.parametersForInit & Eyeshot360camParameter.QualitySetting) != (Eyeshot360camParameter)0)
								{
									List<string> list = new List<string>(new string[]
									{
										"Fasters",
										"Fast",
										"Simple",
										"Good",
										"Beautiful",
										"Fantastic"
									});
									component.qualitySetting = ((!list.Contains(text4)) ? "Good" : text4);
								}
								break;
							case "Default Capture Key":
								if ((this.parametersForInit & Eyeshot360camParameter.DefaultCaptureKey) != (Eyeshot360camParameter)0)
								{
									if (Enum.IsDefined(typeof(KeyCode), text4))
									{
										component.defaultCaptureKey = (KeyCode)Enum.Parse(typeof(KeyCode), text4);
									}
									else
									{
										component.defaultCaptureKey = KeyCode.P;
									}
								}
								break;
							case "Selfie Capture Key":
								if ((this.parametersForInit & Eyeshot360camParameter.SelfieCaptureKey) != (Eyeshot360camParameter)0)
								{
									if (Enum.IsDefined(typeof(KeyCode), text4))
									{
										component.selfieCaptureKey = (KeyCode)Enum.Parse(typeof(KeyCode), text4);
									}
									else
									{
										component.selfieCaptureKey = KeyCode.O;
									}
								}
								break;
							case "Image Format":
								if ((this.parametersForInit & Eyeshot360camParameter.ImageFormat) != (Eyeshot360camParameter)0)
								{
									if (Enum.IsDefined(typeof(Eyeshot360cam.ImageFormat), text4))
									{
										component.imageFormat = (Eyeshot360cam.ImageFormat)Enum.Parse(typeof(Eyeshot360cam.ImageFormat), text4);
									}
									else
									{
										component.imageFormat = Eyeshot360cam.ImageFormat.PNG;
									}
								}
								break;
							case "Panorama Format":
								if ((this.parametersForInit & Eyeshot360camParameter.PanoramaFormat) != (Eyeshot360camParameter)0)
								{
									if (Enum.IsDefined(typeof(Eyeshot360cam.PanoramaFormat), text4))
									{
										component.panoramaFormat = (Eyeshot360cam.PanoramaFormat)Enum.Parse(typeof(Eyeshot360cam.PanoramaFormat), text4);
									}
									else
									{
										component.panoramaFormat = Eyeshot360cam.PanoramaFormat.LongLatUnwrap;
									}
								}
								break;
							case "Capture Stereoscopic":
								if ((this.parametersForInit & Eyeshot360camParameter.CaptureStereoscopic) != (Eyeshot360camParameter)0)
								{
									bool captureStereoscopic = false;
									if (bool.TryParse(text4, out captureStereoscopic))
									{
										component.captureStereoscopic = captureStereoscopic;
									}
									else
									{
										component.captureStereoscopic = true;
									}
								}
								break;
							case "Interpupillary Distance":
								if ((this.parametersForInit & Eyeshot360camParameter.InterpupillaryDistance) != (Eyeshot360camParameter)0)
								{
									float interpupillaryDistance = 0f;
									if (float.TryParse(text4, out interpupillaryDistance))
									{
										component.interpupillaryDistance = interpupillaryDistance;
									}
									else
									{
										component.interpupillaryDistance = 0.0635f;
									}
								}
								break;
							case "Num Circle Points":
								if ((this.parametersForInit & Eyeshot360camParameter.NumCirclePoints) != (Eyeshot360camParameter)0)
								{
									int numCirclePoints = 0;
									if (int.TryParse(text4, out numCirclePoints))
									{
										component.numCirclePoints = numCirclePoints;
									}
									else
									{
										component.numCirclePoints = 128;
									}
								}
								break;
							case "Panorama Width":
								if ((this.parametersForInit & Eyeshot360camParameter.PanoramaWidth) != (Eyeshot360camParameter)0)
								{
									int panoramaWidth = 0;
									if (int.TryParse(text4, out panoramaWidth))
									{
										component.panoramaWidth = panoramaWidth;
									}
									else
									{
										component.panoramaWidth = 4096;
									}
								}
								break;
							case "Anti Aliasing":
								if ((this.parametersForInit & Eyeshot360camParameter.AntiAliasing) != (Eyeshot360camParameter)0)
								{
									if (Enum.IsDefined(typeof(Eyeshot360cam.AntiAliasing), text4))
									{
										component.antiAliasing = (Eyeshot360cam.AntiAliasing)Enum.Parse(typeof(Eyeshot360cam.AntiAliasing), text4);
									}
									else
									{
										component.antiAliasing = Eyeshot360cam.AntiAliasing._8;
									}
								}
								break;
							case "Ssaa Factor":
								if ((this.parametersForInit & Eyeshot360camParameter.SsaaFactor) != (Eyeshot360camParameter)0)
								{
									int ssaaFactor = 0;
									if (int.TryParse(text4, out ssaaFactor))
									{
										component.ssaaFactor = ssaaFactor;
									}
									else
									{
										component.ssaaFactor = 1;
									}
								}
								break;
							case "Depth Map":
								if ((this.parametersForInit & Eyeshot360camParameter.DepthMap) != (Eyeshot360camParameter)0)
								{
									bool depthMap = false;
									if (bool.TryParse(text4, out depthMap))
									{
										component.depthMap = depthMap;
									}
									else
									{
										component.depthMap = false;
									}
								}
								break;
							case "Depth Level":
								if ((this.parametersForInit & Eyeshot360camParameter.DepthLevel) != (Eyeshot360camParameter)0)
								{
									float value = 0f;
									if (float.TryParse(text4, out value))
									{
										component.depthLevel = Mathf.Clamp(value, 0f, 3f);
									}
									else
									{
										component.depthLevel = 1f;
									}
								}
								break;
							case "Depth Far":
								if ((this.parametersForInit & Eyeshot360camParameter.DepthFar) != (Eyeshot360camParameter)0)
								{
									float value2 = 0f;
									if (float.TryParse(text4, out value2))
									{
										component.depthFar = Math.Abs(value2);
									}
									else
									{
										component.depthFar = 100f;
									}
								}
								break;
							case "Save Image Path":
								if ((this.parametersForInit & Eyeshot360camParameter.SaveImagePath) != (Eyeshot360camParameter)0)
								{
									component.saveImagePath = text4;
								}
								break;
							case "Save Cubemap":
								if ((this.parametersForInit & Eyeshot360camParameter.SaveCubemap) != (Eyeshot360camParameter)0)
								{
									bool saveCubemap = false;
									if (bool.TryParse(text4, out saveCubemap))
									{
										component.saveCubemap = saveCubemap;
									}
									else
									{
										component.saveCubemap = false;
									}
								}
								break;
							case "Upload Images":
								if ((this.parametersForInit & Eyeshot360camParameter.UploadImages) != (Eyeshot360camParameter)0)
								{
									bool uploadImages = false;
									if (bool.TryParse(text4, out uploadImages))
									{
										component.uploadImages = uploadImages;
									}
									else
									{
										component.uploadImages = true;
									}
								}
								break;
							case "Save Short Url":
								if ((this.parametersForInit & Eyeshot360camParameter.SaveShortUrl) != (Eyeshot360camParameter)0)
								{
									bool saveShortUrl = false;
									if (bool.TryParse(text4, out saveShortUrl))
									{
										component.saveShortUrl = saveShortUrl;
									}
									else
									{
										component.saveShortUrl = false;
									}
								}
								break;
							case "Use Default Orientation":
								if ((this.parametersForInit & Eyeshot360camParameter.UseDefaultOrientation) != (Eyeshot360camParameter)0)
								{
									bool useDefaultOrientation = false;
									if (bool.TryParse(text4, out useDefaultOrientation))
									{
										component.useDefaultOrientation = useDefaultOrientation;
									}
									else
									{
										component.useDefaultOrientation = true;
									}
								}
								break;
							case "Use GPU Transform":
								if ((this.parametersForInit & Eyeshot360camParameter.UseGPUTransform) != (Eyeshot360camParameter)0)
								{
									bool useGPUTransform = false;
									if (bool.TryParse(text4, out useGPUTransform))
									{
										component.useGPUTransform = useGPUTransform;
									}
									else
									{
										component.useGPUTransform = true;
									}
								}
								break;
							case "User Token":
								if ((this.parametersForInit & Eyeshot360camParameter.UserToken) != (Eyeshot360camParameter)0)
								{
									component.userToken = text4;
								}
								break;
							case "Album Name":
								if ((this.parametersForInit & Eyeshot360camParameter.Album) != (Eyeshot360camParameter)0)
								{
									component.metadata.albumName = text4;
								}
								break;
							case "Album Privacy":
								if ((this.parametersForInit & Eyeshot360camParameter.Album) != (Eyeshot360camParameter)0)
								{
									string value3 = "Private";
									string value4 = "Public";
									if (text4.Equals(value3) || text4.Equals(value4))
									{
										component.metadata.isPrivateAlbum = text4.Equals(value3);
									}
									else
									{
										component.metadata.isPrivateAlbum = true;
									}
								}
								break;
							}
						}
					}
				}
			}
			this.WriteConfig(text, component);
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x000CBE94 File Offset: 0x000CA294
		private void WriteConfig(string path, Eyeshot360cam pano)
		{
			using (StreamWriter streamWriter = new StreamWriter(path, false))
			{
				streamWriter.WriteLine("## VRCHIVE config ##");
				streamWriter.WriteLine("## See vrchive.com/apidocumentation for more info ##");
				streamWriter.WriteLine();
				if ((this.parametersForInit & Eyeshot360camParameter.UserToken) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("## VRCHIVE user token is found under settings on vrchive.com ##");
					streamWriter.WriteLine("User Token = " + pano.userToken);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.PanoramaName) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Panorama Name = " + pano.panoramaName);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.QualitySetting) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Quality Setting = " + pano.qualitySetting);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.DefaultCaptureKey) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Default Capture Key = " + pano.defaultCaptureKey);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.SelfieCaptureKey) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Selfie Capture Key = " + pano.selfieCaptureKey);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.ImageFormat) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Image Format = " + pano.imageFormat);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.PanoramaFormat) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Panorama Format = " + pano.panoramaFormat);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.CaptureStereoscopic) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("## Save left and right eyes? [supported variants: True, False. default: True] ##");
					streamWriter.WriteLine("Capture Stereoscopic = " + pano.captureStereoscopic);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.InterpupillaryDistance) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Interpupillary Distance = " + pano.interpupillaryDistance);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.NumCirclePoints) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Num Circle Points = " + pano.numCirclePoints);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.PanoramaWidth) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("## Resolution in pixels [default: 4096] ##");
					streamWriter.WriteLine("Panorama Width = " + pano.panoramaWidth);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.AntiAliasing) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("## Antialiasing makes it look smoother [supported variants: 1, 2, 4, 8. default: 8] ##");
					streamWriter.WriteLine("Anti Aliasing = " + (int)pano.antiAliasing);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.SsaaFactor) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Ssaa Factor = " + pano.ssaaFactor);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.DepthMap) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("## Do depth map? [supported variants: True, False. default: False] ##");
					streamWriter.WriteLine("Depth Map = " + pano.depthMap);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.DepthLevel) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Depth Level = " + pano.depthLevel);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.DepthFar) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Depth Far = " + pano.depthFar);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.SaveImagePath) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Save Image Path = " + pano.saveImagePath);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.SaveCubemap) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Save Cubemap = " + pano.saveCubemap);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.UploadImages) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("## Auto upload to VRCHIVE account. Disable to save locally. [supported variants: True, False. default: True] ##");
					streamWriter.WriteLine("Upload Images = " + pano.uploadImages);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.SaveShortUrl) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Save Short Url = " + pano.saveShortUrl);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.UseGPUTransform) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("Use GPU Transform = " + pano.useGPUTransform);
					streamWriter.WriteLine();
				}
				if ((this.parametersForInit & Eyeshot360camParameter.Album) != (Eyeshot360camParameter)0)
				{
					streamWriter.WriteLine("## Set which album uploads go to, public or private. If no album exists, one will be created. [supported variants: Public, Private. default: Private] ##");
					streamWriter.WriteLine("Album Name = " + pano.metadata.albumName);
					streamWriter.WriteLine("Album Privacy = " + ((!pano.metadata.isPrivateAlbum) ? "Public" : "Private"));
				}
				streamWriter.WriteLine();
				streamWriter.WriteLine("## Eyeshot current version: 0.1 ##");
			}
		}

		// Token: 0x0400153F RID: 5439
		private const string PANORAMA_NAME = "Panorama Name";

		// Token: 0x04001540 RID: 5440
		private const string QUALITY_SETTING = "Quality Setting";

		// Token: 0x04001541 RID: 5441
		private const string DEFAULT_CAPTURE_KEY = "Default Capture Key";

		// Token: 0x04001542 RID: 5442
		private const string SELFIE_CAPTURE_KEY = "Selfie Capture Key";

		// Token: 0x04001543 RID: 5443
		private const string IMAGE_FORMAT = "Image Format";

		// Token: 0x04001544 RID: 5444
		private const string PANORAMA_FORMAT = "Panorama Format";

		// Token: 0x04001545 RID: 5445
		private const string CAPTURE_STEREOSCOPIC = "Capture Stereoscopic";

		// Token: 0x04001546 RID: 5446
		private const string INTERPUPILLARY_DISTANCE = "Interpupillary Distance";

		// Token: 0x04001547 RID: 5447
		private const string NUM_CIRCLE_POINTS = "Num Circle Points";

		// Token: 0x04001548 RID: 5448
		private const string PANORAMA_WIDTH = "Panorama Width";

		// Token: 0x04001549 RID: 5449
		private const string ANTI_ALIASING = "Anti Aliasing";

		// Token: 0x0400154A RID: 5450
		private const string SSAA_FACTOR = "Ssaa Factor";

		// Token: 0x0400154B RID: 5451
		private const string DEPTH_MAP = "Depth Map";

		// Token: 0x0400154C RID: 5452
		private const string DEPTH_LEVEL = "Depth Level";

		// Token: 0x0400154D RID: 5453
		private const string DEPTH_FAR = "Depth Far";

		// Token: 0x0400154E RID: 5454
		private const string SAVE_IMAGE_PATH = "Save Image Path";

		// Token: 0x0400154F RID: 5455
		private const string SAVE_CUBEMAP = "Save Cubemap";

		// Token: 0x04001550 RID: 5456
		private const string UPLOAD_IMAGES = "Upload Images";

		// Token: 0x04001551 RID: 5457
		private const string SAVE_SHORT_URL = "Save Short Url";

		// Token: 0x04001552 RID: 5458
		private const string USE_DEFAULT_ORIENTATION = "Use Default Orientation";

		// Token: 0x04001553 RID: 5459
		private const string USE_GPU_TRANSFORM = "Use GPU Transform";

		// Token: 0x04001554 RID: 5460
		private const string USER_TOKEN = "User Token";

		// Token: 0x04001555 RID: 5461
		private const string ALBUM_NAME = "Album Name";

		// Token: 0x04001556 RID: 5462
		private const string ALBUM_PRIVACY = "Album Privacy";

		// Token: 0x04001557 RID: 5463
		private const string DEFAULT_PANORAMA_NAME = "default_panorama_name";

		// Token: 0x04001558 RID: 5464
		[BitMask(typeof(Eyeshot360camParameter))]
		public Eyeshot360camParameter parametersForInit = Eyeshot360camParameter.CaptureStereoscopic | Eyeshot360camParameter.PanoramaWidth | Eyeshot360camParameter.AntiAliasing | Eyeshot360camParameter.UploadImages | Eyeshot360camParameter.UserToken | Eyeshot360camParameter.Album;

		// Token: 0x04001559 RID: 5465
		public string iniPath = "VRCHIVE.ini";

		// Token: 0x0400155A RID: 5466
		public bool useAtEditor = true;
	}
}
