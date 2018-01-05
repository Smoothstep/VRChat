using System;
using System.Runtime.InteropServices;

namespace FragLabs.Audio.Codecs.Opus
{
	// Token: 0x02000709 RID: 1801
	internal class API
	{
		// Token: 0x06003AE8 RID: 15080
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr opus_encoder_create(int Fs, int channels, int application, out IntPtr error);

		// Token: 0x06003AE9 RID: 15081
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void opus_encoder_destroy(IntPtr encoder);

		// Token: 0x06003AEA RID: 15082
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_encode(IntPtr st, byte[] pcm, int frame_size, IntPtr data, int max_data_bytes);

		// Token: 0x06003AEB RID: 15083
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_encode_float(IntPtr st, float[] pcm, int frame_size, IntPtr data, int max_data_bytes);

		// Token: 0x06003AEC RID: 15084
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr opus_decoder_create(int Fs, int channels, out IntPtr error);

		// Token: 0x06003AED RID: 15085
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void opus_decoder_destroy(IntPtr decoder);

		// Token: 0x06003AEE RID: 15086
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_decode(IntPtr st, byte[] data, int len, IntPtr pcm, int frame_size, int decode_fec);

		// Token: 0x06003AEF RID: 15087
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_decode_float(IntPtr st, byte[] data, int len, IntPtr pcm, int frame_size, int decode_fec);

		// Token: 0x06003AF0 RID: 15088
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_encoder_ctl(IntPtr st, Ctl request, int value);

		// Token: 0x06003AF1 RID: 15089
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_encoder_ctl(IntPtr st, Ctl request, out int value);

		// Token: 0x06003AF2 RID: 15090
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_decoder_ctl(IntPtr st, Ctl request, int value);

		// Token: 0x06003AF3 RID: 15091
		[DllImport("opus", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int opus_decoder_ctl(IntPtr st, Ctl request, out int value);

		// Token: 0x040023A8 RID: 9128
		private const string importDLL = "opus";
	}
}
