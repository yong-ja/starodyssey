#region Disclaimer

/* 
 * DebugManager
 *
 * Created on 21 August 2007
 * Updated on 29 July 2010
 * Author: Adalberto L. Simeone (Taranto, Italy)
 * Website: http://www.avengersutd.com
 *
 * Part of the Odyssey Engine
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
using AvengersUtd.Odyssey.Graphics.Materials;
using AvengersUtd.Odyssey.Graphics.Rendering.Management;
using AvengersUtd.Odyssey.UserInterface.Style;
using AvengersUtd.Odyssey.Utils;
using SlimDX;
using AvengersUtd.Odyssey.UserInterface.Controls;

#endregion

namespace AvengersUtd.Odyssey.UserInterface.Text
{

    /// <summary>
    /// Descrizione di riepilogo per DebugManager.
    /// </summary>
    public sealed class DebugLogger : IDisposable
    {
        const int messageLimit = 5;
        //static FrameTimeCounter frameTimeCounter;
        
        bool disposed;
        string frameStats = string.Empty;
        uint lastStatsUpdateFrames; // frames count since last time the stats were updated
        double lastStatsUpdateTime; // last time the stats were updated
        static LoggerPanel loggerPanel;
        private TextLiteral text;

        Timer timer;

        public bool IsActive { get; private set; }

        public DebugLogger()
        {
            DeviceInfo = string.Empty;
            timer = new Timer();


        }

        public void Init()
        {
            loggerPanel = new LoggerPanel
            {
                Position = new Vector2(0f, Game.Context.Settings.ScreenHeight * 2 / 3),
                Size = new Size(512, 256),
            };
            AvengersUtd.Odyssey.Utils.Logging.LoggerTraceListener.SetLoggerPanel(loggerPanel);
        }

        public string DeviceInfo { get; private set; }

        public string FrameStats
        {
            get
            {
                UpdateFrameStats(Game.FrameTime);
                return frameStats;
            }
        }

        public void Activate()
        {
            text = new TextLiteral(true)
                       {
                           Position = new Vector2(10, 10),
                           Content = "FPSCounter",
                           Depth = Depth.Topmost,
                       };
            OdysseyUI.CurrentHud.Add(text);
            OdysseyUI.CurrentHud.Add(loggerPanel);
            IsActive = true;
        }

        public void Deactivate()
        {
            System.Diagnostics.Trace.Listeners.Remove("logger");
            OdysseyUI.CurrentHud.Remove(text);
            OdysseyUI.CurrentHud.Remove(loggerPanel);
        }

        public void Update()
        {
            text.Content = FrameStats;
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
                float fps = (float)(lastStatsUpdateFrames / (time - lastStatsUpdateTime));
                //				currentFrameRate = fps;
                lastStatsUpdateFrames = 0;
                lastStatsUpdateTime = time;

                //frameStats = "FPS: " +
                //             fps.ToString("f2") + " FrameTime: " + framerate.ToString("f6");
                frameStats = string.Format("FPS: {0:F2} t: {1:f4}", fps, framerate);
            }
        }

        public void AddLoggerPanelToHud(Hud hud)
        {
            hud.Controls.Add(loggerPanel);
        }

        public void Log(string message)
        {
            loggerPanel.EnqueueMessage(message);
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
                    text.Dispose();
                }

                // dispose unmanaged components
            }
            disposed = true;
        }

        ~DebugLogger()
        {
            Dispose(false);
        }

        #endregion
    }
}