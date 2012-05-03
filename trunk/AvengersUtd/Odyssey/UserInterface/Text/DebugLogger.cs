﻿#region Disclaimer

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
        string debugInfo = string.Empty;
        string frameStats = string.Empty;
        uint lastStatsUpdateFrames; // frames count since last time the stats were updated
        double lastStatsUpdateTime; // last time the stats were updated
        
        Font font;
        private TextLiteral text;

        Queue<string> messageQueue = new Queue<string>(messageLimit);
        StringBuilder stringBuffer = new StringBuilder();
        Timer timer;


        public DebugLogger()
        {
            DeviceInfo = string.Empty;
            timer = new Timer();

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
                           Depth = Depth.Topmost
                       };
            OdysseyUI.CurrentHud.Add(text);
            //TextMaterial textWriter = new TextMaterial();
            //MaterialNode mNode = new MaterialNode("Logger", textWriter);
            //RenderableNode rNode = new RenderableNode(text);
            //mNode.AppendChild(rNode);
            //RenderableCollection rCollection = new RenderableCollection(textWriter.RenderableCollectionDescription)
            //                                       {rNode};
            ////RenderCommand rCommand = new RenderCommand(mNode, rCollection);
            //Game.CurrentRenderer.Scene.Tree.RootNode.AppendChild(mNode);
        }

        public void Update()
        {
            text.Content = FrameStats;
        }

        public void DisplayStats()
        {
            //TextManager txtManager = new TextManager(font, 15);

            //// Output statistics
            ////txtManager.Begin();
            //OdysseyUI.CurrentHud.SpriteManager.Begin(SpriteFlags.AlphaBlend);
            //txtManager.SetInsertionPoint(200, 5);
            //txtManager.SetForegroundColor4(Color.Yellow);
            //txtManager.DrawTextLine(FrameStats);
            //txtManager.DrawTextLine(deviceStats);
            //txtManager.DrawTextLine(debugInfo);
            //OdysseyUI.CurrentHud.SpriteManager.End();
            ////txtManager.SetForegroundColor4(Color.White);
            ////txtManager.End();
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


        //public static void LogError(string method, string text, string id)
        //{
        //    DebugManager dm = Instance;
        //    dm.messageQueue.Enqueue(string.Format("Error in {0}: {1} ({2})", method, text, id));
        //    dm.CheckBounds();
        //}

        //public static void LogToScreen(string text)
        //{
        //    DebugManager dm = Instance;
        //    dm.messageQueue.Enqueue(text);
        //    dm.CheckBounds();
        //}

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
                frameStats = "FPS: " + fps.ToString("f2");
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