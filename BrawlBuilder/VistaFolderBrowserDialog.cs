/**
 * This code was taken from Ookii.Dialogs and reduced to a single file for 
 * cleanliness and to remove unused code, since all BrawlBuilder used from
 * Ookii.Dialogs was the Vista-style folder browser.
 * 
 * Ookii.Dialogs is distributed under the following license:
 * ----------------------------------------------------------------------------
 * 
 * License agreement for Ookii.Dialogs.
 * 
 * Copyright © Sven Groot (Ookii.org) 2009
 * All rights reserved.
 * 
 * 
 * Redistribution and use in source and binary forms, with or without 
 * modification, are permitted provided that the following conditions are met:
 * 
 * 1) Redistributions of source code must retain the above copyright notice, 
 *    this list of conditions and the following disclaimer. 
 * 2) Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution. 
 * 3) Neither the name of the ORGANIZATION nor the names of its contributors
 *    may be used to endorse or promote products derived from this software
 *    without specific prior written permission. 
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
 * THE POSSIBILITY OF SUCH DAMAGE.
 * 
 */
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

// Disable warning CS0108: 'x' hides inherited member 'y'. Use the new keyword if hiding was intended.
#pragma warning disable 0108

namespace Ookii.Dialogs.Interop
{
	internal enum HRESULT : long
	{
		S_FALSE = 0x0001,
		S_OK = 0x0000,
		E_INVALIDARG = 0x80070057,
		E_OUTOFMEMORY = 0x8007000E,
		ERROR_CANCELLED = 0x800704C7
	}

	internal static class IIDGuid
	{
		internal const string IModalWindow = "b4db1657-70d7-485e-8e3e-6fcb5a5c1802";
		internal const string IFileDialog = "42f85136-db7e-439c-85f1-e4075d135fc8";
		internal const string IFileOpenDialog = "d57c7288-d4ad-4768-be02-9d969532d960";
		internal const string IFileDialogEvents = "973510DB-7D7F-452B-8975-74A85828D354";
		internal const string IFileDialogCustomize = "e6fdd21a-163f-4975-9c8c-a69f1ba37034";
		internal const string IShellItem = "43826D1E-E718-42EE-BC55-A1E261C37BFE";
		internal const string IShellItemArray = "B63EA76D-1F85-456F-A19C-48159EFA858B";
	}

	internal static class CLSIDGuid
	{
		internal const string FileOpenDialog = "DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7";
	}

	// ---------------------------------------------------------
    // Coclass interfaces - designed to "look like" the object 
    // in the API, so that the 'new' operator can be used in a 
    // straightforward way. Behind the scenes, the C# compiler
    // morphs all 'new CoClass()' calls to 'new CoClassWrapper()'
    [ComImport,
    Guid(IIDGuid.IFileOpenDialog), 
    CoClass(typeof(FileOpenDialogRCW))]
    internal interface NativeFileOpenDialog : IFileOpenDialog
    {
    }

    // ---------------------------------------------------
    // .NET classes representing runtime callable wrappers
    [ComImport,
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(CLSIDGuid.FileOpenDialog)]
    internal class FileOpenDialogRCW
    {
    }

	[ComImport(),
	Guid(IIDGuid.IModalWindow),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IModalWindow
	{

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
		PreserveSig]
		int Show([In] IntPtr parent);
	}

	[ComImport,
	Guid(IIDGuid.IShellItem),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellItem
	{
		// Not supported: IBindCtx
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetDisplayName([In] NativeMethods.SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);
	}

	[ComImport(),
	Guid(IIDGuid.IFileDialog),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IFileDialog : IModalWindow
	{
		// Defined on IModalWindow - repeated here due to requirements of COM interop layer
		// --------------------------------------------------------------------------------
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
		PreserveSig]
		int Show([In] IntPtr parent);

		// IFileDialog-Specific interface members
		// --------------------------------------------------------------------------------
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFileTypes([In] uint cFileTypes, [In, MarshalAs(UnmanagedType.LPArray)] NativeMethods.COMDLG_FILTERSPEC[] rgFilterSpec);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFileTypeIndex([In] uint iFileType);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFileTypeIndex(out uint piFileType);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Advise([In, MarshalAs(UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Unadvise([In] uint dwCookie);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetOptions([In] NativeMethods.FOS fos);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetOptions(out NativeMethods.FOS pfos);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, NativeMethods.FDAP fdap);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetClientGuid([In] ref Guid guid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ClearClientData();

		// Not supported:  IShellItemFilter is not defined, converting to IntPtr
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);
	}

	[ComImport,
	Guid(IIDGuid.IFileDialogEvents),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IFileDialogEvents
	{
		// NOTE: some of these callbacks are cancelable - returning S_FALSE means that 
		// the dialog should not proceed (e.g. with closing, changing folder); to 
		// support this, we need to use the PreserveSig attribute to enable us to return
		// the proper HRESULT
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
		PreserveSig]
		HRESULT OnFileOk([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
		PreserveSig]
		HRESULT OnFolderChanging([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psiFolder);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnFolderChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnSelectionChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnShareViolation([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, out NativeMethods.FDE_SHAREVIOLATION_RESPONSE pResponse);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnTypeChange([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnOverwrite([In, MarshalAs(UnmanagedType.Interface)] IFileDialog pfd, [In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, out NativeMethods.FDE_OVERWRITE_RESPONSE pResponse);
	}

	[ComImport,
	Guid(IIDGuid.IFileDialogCustomize),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IFileDialogCustomize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void EnableOpenDropDown([In] int dwIDCtl);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddMenu([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddPushButton([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddComboBox([In] int dwIDCtl);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddRadioButtonList([In] int dwIDCtl);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddCheckButton([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel, [In] bool bChecked);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddEditBox([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszText);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddSeparator([In] int dwIDCtl);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddText([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszText);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetControlLabel([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetControlState([In] int dwIDCtl, [Out] out NativeMethods.CDCONTROLSTATE pdwState);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetControlState([In] int dwIDCtl, [In] NativeMethods.CDCONTROLSTATE dwState);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetEditBoxText([In] int dwIDCtl, [Out] IntPtr ppszText);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetEditBoxText([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszText);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCheckButtonState([In] int dwIDCtl, [Out] out bool pbChecked);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetCheckButtonState([In] int dwIDCtl, [In] bool bChecked);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddControlItem([In] int dwIDCtl, [In] int dwIDItem, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RemoveControlItem([In] int dwIDCtl, [In] int dwIDItem);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RemoveAllControlItems([In] int dwIDCtl);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetControlItemState([In] int dwIDCtl, [In] int dwIDItem, [Out] out NativeMethods.CDCONTROLSTATE pdwState);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetControlItemState([In] int dwIDCtl, [In] int dwIDItem, [In] NativeMethods.CDCONTROLSTATE dwState);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSelectedControlItem([In] int dwIDCtl, [Out] out int pdwIDItem);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetSelectedControlItem([In] int dwIDCtl, [In] int dwIDItem); // Not valid for OpenDropDown
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void StartVisualGroup([In] int dwIDCtl, [In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void EndVisualGroup();
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void MakeProminent([In] int dwIDCtl);
	}

	[ComImport(),
	Guid(IIDGuid.IFileOpenDialog),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IFileOpenDialog : IFileDialog
	{
		// Defined on IModalWindow - repeated here due to requirements of COM interop layer
		// --------------------------------------------------------------------------------
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime),
		PreserveSig]
		int Show([In] IntPtr parent);

		// Defined on IFileDialog - repeated here due to requirements of COM interop layer
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFileTypes([In] uint cFileTypes, [In] ref NativeMethods.COMDLG_FILTERSPEC rgFilterSpec);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFileTypeIndex([In] uint iFileType);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFileTypeIndex(out uint piFileType);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Advise([In, MarshalAs(UnmanagedType.Interface)] IFileDialogEvents pfde, out uint pdwCookie);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Unadvise([In] uint dwCookie);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetOptions([In] NativeMethods.FOS fos);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetOptions(out NativeMethods.FOS pfos);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, NativeMethods.FDAP fdap);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Close([MarshalAs(UnmanagedType.Error)] int hr);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetClientGuid([In] ref Guid guid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ClearClientData();

		// Not supported:  IShellItemFilter is not defined, converting to IntPtr
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);

		// Defined by IFileOpenDialog
		// ---------------------------------------------------------------------------------
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetResults([MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppenum);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppsai);
	}

	[ComImport,
	Guid(IIDGuid.IShellItemArray),
	InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellItemArray
	{
		// Not supported: IBindCtx
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, out IntPtr ppvOut);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetPropertyStore([In] int Flags, [In] ref Guid riid, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetPropertyDescriptionList([In] ref NativeMethods.PROPERTYKEY keyType, [In] ref Guid riid, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAttributes([In] NativeMethods.SIATTRIBFLAGS dwAttribFlags, [In] uint sfgaoMask, out uint psfgaoAttribs);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCount(out uint pdwNumItems);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetItemAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		// Not supported: IEnumShellItems (will use GetCount and GetItemAt instead)
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
	}

	class WindowHandleWrapper : IWin32Window
	{
		private IntPtr _handle;

		public WindowHandleWrapper(IntPtr handle)
		{
			_handle = handle;
		}

		#region IWin32Window Members

		public IntPtr Handle
		{
			get { return _handle; }
		}

		#endregion
	}
}

namespace Ookii.Dialogs
{
	static class NativeMethods
	{
		public static bool IsWindowsVistaOrLater
		{
			get
			{
				return Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version >= new Version(6, 0, 6000);
			}
		}

		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		public static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		public static Interop.IShellItem CreateItemFromParsingName(string path)
		{
			object item;
			Guid guid = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe"); // IID_IShellItem
			int hr = NativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, ref guid, out item);
			if (hr != 0)
				throw new System.ComponentModel.Win32Exception(hr);
			return (Interop.IShellItem)item;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		internal struct COMDLG_FILTERSPEC
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszName;
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string pszSpec;
		}

		// Property System structs and consts
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		internal struct PROPERTYKEY
		{
			internal Guid fmtid;
			internal uint pid;
		}

		[Flags]
		internal enum FOS : uint
		{
			FOS_OVERWRITEPROMPT = 0x00000002,
			FOS_STRICTFILETYPES = 0x00000004,
			FOS_NOCHANGEDIR = 0x00000008,
			FOS_PICKFOLDERS = 0x00000020,
			FOS_FORCEFILESYSTEM = 0x00000040, // Ensure that items returned are filesystem items.
			FOS_ALLNONSTORAGEITEMS = 0x00000080, // Allow choosing items that have no storage.
			FOS_NOVALIDATE = 0x00000100,
			FOS_ALLOWMULTISELECT = 0x00000200,
			FOS_PATHMUSTEXIST = 0x00000800,
			FOS_FILEMUSTEXIST = 0x00001000,
			FOS_CREATEPROMPT = 0x00002000,
			FOS_SHAREAWARE = 0x00004000,
			FOS_NOREADONLYRETURN = 0x00008000,
			FOS_NOTESTFILECREATE = 0x00010000,
			FOS_HIDEMRUPLACES = 0x00020000,
			FOS_HIDEPINNEDPLACES = 0x00040000,
			FOS_NODEREFERENCELINKS = 0x00100000,
			FOS_DONTADDTORECENT = 0x02000000,
			FOS_FORCESHOWHIDDEN = 0x10000000,
			FOS_DEFAULTNOMINIMODE = 0x20000000
		}

		internal enum SIGDN : uint
		{
			SIGDN_NORMALDISPLAY = 0x00000000,           // SHGDN_NORMAL
			SIGDN_PARENTRELATIVEPARSING = 0x80018001,   // SHGDN_INFOLDER | SHGDN_FORPARSING
			SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,  // SHGDN_FORPARSING
			SIGDN_PARENTRELATIVEEDITING = 0x80031001,   // SHGDN_INFOLDER | SHGDN_FOREDITING
			SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,  // SHGDN_FORPARSING | SHGDN_FORADDRESSBAR
			SIGDN_FILESYSPATH = 0x80058000,             // SHGDN_FORPARSING
			SIGDN_URL = 0x80068000,                     // SHGDN_FORPARSING
			SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,     // SHGDN_INFOLDER | SHGDN_FORPARSING | SHGDN_FORADDRESSBAR
			SIGDN_PARENTRELATIVE = 0x80080001           // SHGDN_INFOLDER
		}

		internal enum FDAP
		{
			FDAP_BOTTOM = 0x00000000,
			FDAP_TOP = 0x00000001,
		}

		internal enum CDCONTROLSTATE
		{
			CDCS_INACTIVE = 0x00000000,
			CDCS_ENABLED = 0x00000001,
			CDCS_VISIBLE = 0x00000002
		}

		internal enum FDE_SHAREVIOLATION_RESPONSE
		{
			FDESVR_DEFAULT = 0x00000000,
			FDESVR_ACCEPT = 0x00000001,
			FDESVR_REFUSE = 0x00000002
		}

		internal enum FDE_OVERWRITE_RESPONSE
		{
			FDEOR_DEFAULT = 0x00000000,
			FDEOR_ACCEPT = 0x00000001,
			FDEOR_REFUSE = 0x00000002
		}

		internal enum SIATTRIBFLAGS
		{
			SIATTRIBFLAGS_AND = 0x00000001, // if multiple items and the attirbutes together.
			SIATTRIBFLAGS_OR = 0x00000002, // if multiple items or the attributes together.
			SIATTRIBFLAGS_APPCOMPAT = 0x00000003, // Call GetAttributes directly on the ShellFolder for multiple attributes
		}
	}

	/// <summary>
	/// Prompts the user to select a folder.
	/// </summary>
	/// <remarks>
	/// This class will use the Vista style Select Folder dialog if possible, or the regular FolderBrowserDialog
	/// if it is not. Note that the Vista style dialog is very different, so using this class without testing
	/// in both Vista and older Windows versions is not recommended.
	/// </remarks>
	/// <threadsafety instance="false" static="true" />
	[DefaultEvent("HelpRequest"), Designer("System.Windows.Forms.Design.FolderBrowserDialogDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"), DefaultProperty("SelectedPath"), Description("Prompts the user to select a folder.")]
    public sealed class VistaFolderBrowserDialog : CommonDialog
    {
        private FolderBrowserDialog _downlevelDialog;
        private string _description;
        private bool _useDescriptionForTitle;
        private string _selectedPath;
        private System.Environment.SpecialFolder _rootFolder;

        /// <summary>
        /// Occurs when the user clicks the Help button on the dialog box.
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new event EventHandler HelpRequest
        {
            add
            {
                base.HelpRequest += value;
            }
            remove
            {
                base.HelpRequest -= value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="VistaFolderBrowserDialog" /> class.
        /// </summary>
        public VistaFolderBrowserDialog()
        {
            if( !IsVistaFolderDialogSupported )
                _downlevelDialog = new FolderBrowserDialog();
            else
                Reset();
        }

        #region Public Properties

        /// <summary>
        /// Gets a value that indicates whether the current OS supports Vista-style common file dialogs.
        /// </summary>
        /// <value>
        /// <see langword="true" /> on Windows Vista or newer operating systems; otherwise, <see langword="false" />.
        /// </value>
        [Browsable(false)]
        public static bool IsVistaFolderDialogSupported
        {
            get
            {
                return NativeMethods.IsWindowsVistaOrLater;
            }
        }

        /// <summary>
        /// Gets or sets the descriptive text displayed above the tree view control in the dialog box, or below the list view control
        /// in the Vista style dialog.
        /// </summary>
        /// <value>
        /// The description to display. The default is an empty string ("").
        /// </value>
        [Category("Folder Browsing"), DefaultValue(""), Localizable(true), Browsable(true), Description("The descriptive text displayed above the tree view control in the dialog box, or below the list view control in the Vista style dialog.")]
        public string Description
        {
            get
            {
                if( _downlevelDialog != null )
                    return _downlevelDialog.Description;
                return _description;
            }
            set
            {
                if( _downlevelDialog != null )
                    _downlevelDialog.Description = value;
                else
                    _description = value ?? String.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the root folder where the browsing starts from. This property has no effect if the Vista style
        /// dialog is used.
        /// </summary>
        /// <value>
        /// One of the <see cref="System.Environment.SpecialFolder" /> values. The default is Desktop.
        /// </value>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="System.Environment.SpecialFolder" /> values.</exception>
        [Localizable(false), Description("The root folder where the browsing starts from. This property has no effect if the Vista style dialog is used."), Category("Folder Browsing"), Browsable(true), DefaultValue(typeof(System.Environment.SpecialFolder), "Desktop")]
        public System.Environment.SpecialFolder RootFolder
        {
            get
            {
                if( _downlevelDialog != null )
                    return _downlevelDialog.RootFolder;
                return _rootFolder;
            }
            set
            {
                if( _downlevelDialog != null )
                    _downlevelDialog.RootFolder = value;
                else
                    _rootFolder = value;
            }
        }
	
        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        /// <value>
        /// The path of the folder first selected in the dialog box or the last folder selected by the user. The default is an empty string ("").
        /// </value>
        [Browsable(true), Editor("System.Windows.Forms.Design.SelectedPathEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor)), Description("The path selected by the user."), DefaultValue(""), Localizable(true), Category("Folder Browsing")]
        public string SelectedPath
        {
            get
            {
                if( _downlevelDialog != null )
                    return _downlevelDialog.SelectedPath;
                return _selectedPath;
            }
            set
            {
                if( _downlevelDialog != null )
                    _downlevelDialog.SelectedPath = value;
                else
                    _selectedPath = value ?? string.Empty;
            }
        }

        private bool _showNewFolderButton;

        /// <summary>
        /// Gets or sets a value indicating whether the New Folder button appears in the folder browser dialog box. This
        /// property has no effect if the Vista style dialog is used; in that case, the New Folder button is always shown.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if the New Folder button is shown in the dialog box; otherwise, <see langword="false" />. The default is <see langword="true" />.
        /// </value>
        [Browsable(true), Localizable(false), Description("A value indicating whether the New Folder button appears in the folder browser dialog box. This property has no effect if the Vista style dialog is used; in that case, the New Folder button is always shown."), DefaultValue(true), Category("Folder Browsing")]
        public bool ShowNewFolderButton
        {
            get
            {
                if( _downlevelDialog != null )
                    return _downlevelDialog.ShowNewFolderButton;
                return _showNewFolderButton;
            }
            set
            {
                if( _downlevelDialog != null )
                    _downlevelDialog.ShowNewFolderButton = value;
                else
                    _showNewFolderButton = value;
            }
        }
	

        /// <summary>
        /// Gets or sets a value that indicates whether to use the value of the <see cref="Description" /> property
        /// as the dialog title for Vista style dialogs. This property has no effect on old style dialogs.
        /// </summary>
        /// <value><see langword="true" /> to indicate that the value of the <see cref="Description" /> property is used as dialog title; <see langword="false" />
        /// to indicate the value is added as additional text to the dialog. The default is <see langword="false" />.</value>
        [Category("Folder Browsing"), DefaultValue(false), Description("A value that indicates whether to use the value of the Description property as the dialog title for Vista style dialogs. This property has no effect on old style dialogs.")]
        public bool UseDescriptionForTitle
        {
            get { return _useDescriptionForTitle; }
            set { _useDescriptionForTitle = value; }
        }	

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets all properties to their default values.
        /// </summary>
        public override void Reset()
        {
            _description = string.Empty;
            _useDescriptionForTitle = false;
            _selectedPath = string.Empty;
            _rootFolder = Environment.SpecialFolder.Desktop;
            _showNewFolderButton = true;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Specifies a common dialog box.
        /// </summary>
        /// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
        /// <returns><see langword="true" /> if the file could be opened; otherwise, <see langword="false" />.</returns>
        protected override bool RunDialog(IntPtr hwndOwner)
        {
            if( _downlevelDialog != null )
                return _downlevelDialog.ShowDialog(hwndOwner == IntPtr.Zero ? null : new Interop.WindowHandleWrapper(hwndOwner)) == DialogResult.OK;

            Ookii.Dialogs.Interop.IFileDialog dialog = null;
            try
            {
                dialog = new Ookii.Dialogs.Interop.NativeFileOpenDialog();
                SetDialogProperties(dialog);
                int result = dialog.Show(hwndOwner);
                if( result < 0 )
                {
                    if( (uint)result == (uint)Interop.HRESULT.ERROR_CANCELLED )
                        return false;
                    else
                        throw System.Runtime.InteropServices.Marshal.GetExceptionForHR(result);
                } 
                GetResult(dialog);
                return true;
            }
            finally
            {
                if( dialog != null )
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(dialog);
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="VistaFolderBrowserDialog" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if( disposing && _downlevelDialog != null )
                    _downlevelDialog.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region Private Methods

        private void SetDialogProperties(Ookii.Dialogs.Interop.IFileDialog dialog)
        {
            // Description
            if( !string.IsNullOrEmpty(_description) )
            {
                if( _useDescriptionForTitle )
                {
                    dialog.SetTitle(_description);
                }
                else
                {
                    Ookii.Dialogs.Interop.IFileDialogCustomize customize = (Ookii.Dialogs.Interop.IFileDialogCustomize)dialog;
                    customize.AddText(0, _description);
                }
            }

            dialog.SetOptions(NativeMethods.FOS.FOS_PICKFOLDERS | NativeMethods.FOS.FOS_FORCEFILESYSTEM | NativeMethods.FOS.FOS_FILEMUSTEXIST);

            if( !string.IsNullOrEmpty(_selectedPath) )
            {
                string parent = Path.GetDirectoryName(_selectedPath);
                if( parent == null || !Directory.Exists(parent) )
                {
                    dialog.SetFileName(_selectedPath);
                }
                else
                {
                    string folder = Path.GetFileName(_selectedPath);
                    dialog.SetFolder(NativeMethods.CreateItemFromParsingName(parent));
                    dialog.SetFileName(folder);
                }
            }
        }

        private void GetResult(Ookii.Dialogs.Interop.IFileDialog dialog)
        {
            Ookii.Dialogs.Interop.IShellItem item;
            dialog.GetResult(out item);
            item.GetDisplayName(NativeMethods.SIGDN.SIGDN_FILESYSPATH, out _selectedPath);
        }

        #endregion
    }
}
