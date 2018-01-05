using System;
using System.Runtime.InteropServices;

// Token: 0x02000671 RID: 1649
public class OVRNativeBuffer : IDisposable
{
	// Token: 0x060037D1 RID: 14289 RVA: 0x0011CA2E File Offset: 0x0011AE2E
	public OVRNativeBuffer(int numBytes)
	{
		this.Reallocate(numBytes);
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x0011CA48 File Offset: 0x0011AE48
	~OVRNativeBuffer()
	{
		this.Dispose(false);
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x0011CA78 File Offset: 0x0011AE78
	public void Reset(int numBytes)
	{
		this.Reallocate(numBytes);
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x0011CA81 File Offset: 0x0011AE81
	public int GetCapacity()
	{
		return this.m_numBytes;
	}

	// Token: 0x060037D5 RID: 14293 RVA: 0x0011CA89 File Offset: 0x0011AE89
	public IntPtr GetPointer(int byteOffset = 0)
	{
		if (byteOffset < 0 || byteOffset >= this.m_numBytes)
		{
			return IntPtr.Zero;
		}
		return (byteOffset != 0) ? new IntPtr(this.m_ptr.ToInt64() + (long)byteOffset) : this.m_ptr;
	}

	// Token: 0x060037D6 RID: 14294 RVA: 0x0011CAC8 File Offset: 0x0011AEC8
	public void Dispose()
	{
		this.Dispose(true);
		GC.SuppressFinalize(this);
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x0011CAD7 File Offset: 0x0011AED7
	private void Dispose(bool disposing)
	{
		if (this.disposed)
		{
			return;
		}
		if (disposing)
		{
		}
		this.Release();
		this.disposed = true;
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x0011CAF8 File Offset: 0x0011AEF8
	private void Reallocate(int numBytes)
	{
		this.Release();
		if (numBytes > 0)
		{
			this.m_ptr = Marshal.AllocHGlobal(numBytes);
			this.m_numBytes = numBytes;
		}
		else
		{
			this.m_ptr = IntPtr.Zero;
			this.m_numBytes = 0;
		}
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x0011CB31 File Offset: 0x0011AF31
	private void Release()
	{
		if (this.m_ptr != IntPtr.Zero)
		{
			Marshal.FreeHGlobal(this.m_ptr);
			this.m_ptr = IntPtr.Zero;
			this.m_numBytes = 0;
		}
	}

	// Token: 0x04002056 RID: 8278
	private bool disposed;

	// Token: 0x04002057 RID: 8279
	private int m_numBytes;

	// Token: 0x04002058 RID: 8280
	private IntPtr m_ptr = IntPtr.Zero;
}
