#region Disclaimer

/* 
 * DebugManager
 *
 * Created on 21 August 2007
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey User Interface Library
 *
 * This source code is Intellectual property of the Author
 * and is released under the Creative Commons Attribution 
 * NonCommercial License, available at:
 * http://creativecommons.org/licenses/by-nc/3.0/ 
 * You can alter and use this source code as you wish, 
 * provided that you do not use the results in any 
 * commercial project without the prior express and 
 * written consent of the Author.
 *
 */

#endregion

#region Using directives
		using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
#if !(SlimDX)
    using Microsoft.DirectX;
    using Microsoft.DirectX.Direct3D;
    using Font=Microsoft.DirectX.Direct3D.Font;
#else
using SlimDX.Direct3D9;
using Font = SlimDX.Direct3D9.Font;
#endif 
	#endregion

namespace AvengersUtd.Odyssey.UserInterface.Helpers
{
    public delegate double FrameTimeCounter();

    /// <summary>
    /// Descrizione di riepilogo per DebugManager.
    /// </summary>
    public sealed class DebugManager : IDisposable
    {
        const int messageLimit = 5;
        static FrameTimeCounter frameTimeCounter;
        static DebugManager singletonDM;

        bool disposed;
        string debugInfo = string.Empty;

        string deviceStats = string.Empty;
        Font font;
        string frameStats = string.Empty;
        uint lastStatsUpdateFrames; // frames count since last time the stats were updated
        double lastStatsUpdateTime; // last time the stats were updated
        Queue<string> messageQueue = new Queue<string>(messageLimit);
        StringBuilder stringBuffer = new StringBuilder();
        Timer timer;


        DebugManager()
        {
            font = new Font(
                OdysseyUI.Device,
                20,
                0,
                FontWeight.Bold,
                1,
                false,
                CharacterSet.Default,
                Precision.Default,
                FontQuality.Default,
#if (!SlimDX)
                PitchAndFamily.FamilyDoNotCare,
#else
                PitchAndFamily.DontCare,
#endif
                 "Courier New"
                );
            timer = new Timer();
        }

        public static DebugManager Instance
        {
            get
            {
                if (singletonDM == null)
                {
                    singletonDM = new DebugManager();
                    return singletonDM;
                }
                else
                    return singletonDM;
            }
        }

        public string DeviceInfo
        {
            get { return deviceStats; }
        }

        public static string FrameStats
        {
            get
            {
                Instance.UpdateFrameStats(frameTimeCounter());
                return Instance.frameStats;
            }
        }

        public void DisplayStats()
        {
            TextManager txtManager = new TextManager(font, 15);

            // Output statistics
            //txtManager.Begin();
            OdysseyUI.CurrentHud.SpriteManager.Begin(SpriteFlags.AlphaBlend);
            txtManager.SetInsertionPoint(200, 5);
            txtManager.SetForegroundColor(Color.Yellow);
            txtManager.DrawTextLine(FrameStats);
            txtManager.DrawTextLine(deviceStats);
            txtManager.DrawTextLine(debugInfo);
            OdysseyUI.CurrentHud.SpriteManager.End();
            //txtManager.SetForegroundColor(Color.White);
            //txtManager.End();
        }

        void CheckBounds()
        {
            if (messageQueue.Count > messageLimit)
            {
                messageQueue.Dequeue();
            }

            stringBuffer = new StringBuilder();
            foreach (string s in messageQueue.ToArray())
            {
                stringBuffer.AppendLine(s);
            }
            debugInfo = stringBuffer.ToString();
        }


        public static void LogError(string method, string text, string id)
        {
            DebugManager dm = Instance;
            dm.messageQueue.Enqueue(string.Format("Error in {0}: {1} ({2})", method, text, id));
            dm.CheckBounds();
        }

        public static void LogToScreen(string text)
        {
            DebugManager dm = Instance;
            dm.messageQueue.Enqueue(text);
            dm.CheckBounds();
        }

        /// <summary>
        /// Updates the static part of the frame stats so it doesn't have be generated every frame
        /// </summary>
        public static void UpdateStaticStats(string deviceInfo)
        {
            Instance.deviceStats = deviceInfo;
        }

        public static void SetFrameTimeCounterDelegate(FrameTimeCounter frameDel)
        {
            frameTimeCounter = frameDel;
        }

        /// <summary>
        /// Updates the frames/sec stat once per second
        /// </summary>
        void UpdateFrameStats(double framerate)
        {
            double time = timer.GetAbsoluteTime();
            lastStatsUpdateFrames++;

            if (time - lastStatsUpdateTime > 1.0)
            {
                float fps = (float) (lastStatsUpdateFrames/(time - lastStatsUpdateTime));
//				currentFrameRate = fps;
                lastStatsUpdateFrames = 0;
                lastStatsUpdateTime = time;

                frameStats = "FPS: " +
                             fps.ToString("f2") + " FrameTime: " + framerate.ToString("f6");
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes all managed and unmanaged resources of this object.
        /// </summary>
        /// <remarks>Be sure to call this method when you don't need this control anymore.</remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// IDisposable pattern implementation. Override this method if your control 
        /// uses resources that should be freed as soon as possible.
        /// </summary>
        /// <param name="disposing">A value, that if <b>true</b>, indicates that the method
        /// has been called by the user. <b>False</b>, if it has been automatically
        /// called by the finalizer.</param>
        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // dispose managed components
                    font.Dispose();
                }

                // dispose unmanaged components
            }
            disposed = true;
        }

        ~DebugManager()
        {
            Dispose(false);
        }

        #endregion
    }
}