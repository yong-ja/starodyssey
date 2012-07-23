﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SlimDX.Direct3D9;
using System.Windows.Interop;
using System.Windows;
using SlimDX.Direct3D11;
using AvengersUtd.Odyssey.Settings;

namespace AvengersUtd.Odyssey
{
    public class D3DImageSlimDX : D3DImage, IDisposable
    {
        [DllImport("user32.dll", SetLastError = false)]
        static extern IntPtr GetDesktopWindow();

        static int NumActiveImages = 0;
        static Direct3DEx D3DContext;
        static DeviceEx D3DDevice;

        DeviceSettings settings;
        Texture SharedTexture;

        public D3DImageSlimDX(DeviceSettings settings)
        {
            this.settings = settings;
            InitD3D9();
            NumActiveImages++;
        }

        public void Dispose()
        {
            SetBackBufferSlimDX(null);
            if (SharedTexture != null)
            {
                SharedTexture.Dispose();
                SharedTexture = null;
            }

            NumActiveImages--;
            ShutdownD3D9();
        }

        public void InvalidateD3DImage()
        {
            if (SharedTexture != null)
            {
                Lock();
                AddDirtyRect(new Int32Rect(0, 0, PixelWidth, PixelHeight));
                Unlock();
            }
        }

        public void SetBackBufferSlimDX(Texture2D Texture)
        {
            if (SharedTexture != null)
            {
                SharedTexture.Dispose();
                SharedTexture = null;
            }

            if (Texture == null)
            {
                if (SharedTexture != null)
                {
                    SharedTexture = null;
                    Lock();
                    SetBackBuffer(D3DResourceType.IDirect3DSurface9, IntPtr.Zero);
                    Unlock();
                }
            }
            else if (IsShareable(Texture))
            {
                Format format = TranslateFormat(Texture.Description.Format);
                if (format == Format.Unknown)
                    throw new ArgumentException("Texture format is not compatible with OpenSharedResource");

                IntPtr Handle = GetSharedHandle(Texture);
                if (Handle == IntPtr.Zero)
                    throw new ArgumentNullException("Handle");

                SharedTexture = new Texture(D3DDevice, Texture.Description.Width, Texture.Description.Height, 1, Usage.RenderTarget, format, Pool.Default, ref Handle);
                using (Surface Surface = SharedTexture.GetSurfaceLevel(0))
                {
                    Lock();
                    SetBackBuffer(D3DResourceType.IDirect3DSurface9, Surface.ComPointer);
                    Unlock();
                }
            }
            else
                throw new ArgumentException("Texture must be created with ResourceOptionFlags.Shared");
        }

        void InitD3D9()
        {
            if (NumActiveImages == 0)
            {
                D3DContext = new Direct3DEx();

                PresentParameters presentparams = new PresentParameters();
                presentparams.Windowed = settings.IsWindowed;
                presentparams.SwapEffect = SwapEffect.Discard;
                presentparams.DeviceWindowHandle = GetDesktopWindow();
                presentparams.PresentationInterval = PresentInterval.Immediate;


                if (!settings.IsWindowed)
                {
                    presentparams.FullScreenRefreshRateInHertz = 120;
                    presentparams.BackBufferHeight = settings.ScreenHeight;
                    presentparams.BackBufferWidth = settings.ScreenWidth;
                    presentparams.BackBufferFormat = TranslateFormat(settings.Format);
                    DisplayModeEx fullScreen = new DisplayModeEx()
                    {
                        Format = TranslateFormat(settings.Format),
                        Width = settings.ScreenWidth,
                        Height = settings.ScreenHeight,
                        RefreshRate = 120
                    };
                    D3DDevice = new DeviceEx(D3DContext,
                        0, DeviceType.Hardware, IntPtr.Zero,
                        CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve,
                        presentparams, fullScreen);
                }
                else
                    D3DDevice = new DeviceEx(D3DContext, 0, DeviceType.Hardware, IntPtr.Zero,
                        CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve,
                        presentparams);
            }
        }

        void ShutdownD3D9()
        {
            if (NumActiveImages == 0)
            {
                if (SharedTexture != null)
                {
                    SharedTexture.Dispose();
                    SharedTexture = null;
                }

                if (D3DDevice != null)
                {
                    D3DDevice.Dispose();
                    D3DDevice = null;
                }

                if (D3DContext != null)
                {
                    D3DContext.Dispose();
                    D3DContext = null;
                }
            }
        }

        IntPtr GetSharedHandle(Texture2D Texture)
        {
            SlimDX.DXGI.Resource resource = new SlimDX.DXGI.Resource(Texture);
            IntPtr result = resource.SharedHandle;

            resource.Dispose();

            return result;
        }

        Format TranslateFormat(SlimDX.DXGI.Format format)
        {
            switch (format)
            {
                case SlimDX.DXGI.Format.R10G10B10A2_UNorm:
                    return SlimDX.Direct3D9.Format.A2B10G10R10;

                case SlimDX.DXGI.Format.R16G16B16A16_Float:
                    return SlimDX.Direct3D9.Format.A16B16G16R16F;

                case SlimDX.DXGI.Format.R8G8B8A8_UNorm:
                case SlimDX.DXGI.Format.B8G8R8A8_UNorm:
                    return SlimDX.Direct3D9.Format.A8R8G8B8;

                default:
                    return SlimDX.Direct3D9.Format.Unknown;
            }
        }

        bool IsShareable(SlimDX.Direct3D11.Texture2D Texture)
        {
            return (Texture.Description.OptionFlags & ResourceOptionFlags.Shared) != 0;
        }
    }

}