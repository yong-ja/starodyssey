using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Utils.Logging;
using AvengersUtd.Odyssey.Geometry;

namespace AvengersUtd.Odyssey.Network
{
    public struct GazeData
    {
        const float MarkerSize = 107;
        const float ScreenWidth = 598;
        const float ScreenHeight = 336;
          
        public uint Index { get; private set; }
        public uint TimestampMs { get; private set; }
        public bool EyeDetected { get; private set; }
        public ushort EyeX { get; private set; }
        public ushort EyeY { get; private set; }
        public ushort FieldX { get; private set; }
        public ushort FieldY { get; private set; }
        public ushort Markers { get; private set; }

        public float GazePoint0X { get; private set; }
        public float GazePoint0Y { get; private set; }

        public float M0ulX { get; private set; }
        public float M0ulY { get; private set; }
        public float M0urX { get; private set; }
        public float M0urY { get; private set; }
        public float M0lrX { get; private set; }
        public float M0lrY { get; private set; }
        public float M0llX { get; private set; }
        public float M0llY { get; private set; }

        bool hasMarkerData;

        public float ScreenX { get; private set; }
        public float ScreenY { get; private set; }
        

        public GazeData(uint index, uint timestampMs, bool eyeDetected, ushort eyeX, ushort eyeY, ushort fieldX, ushort fieldY, ushort markers)
            : this()
        {
            Index = index;
            TimestampMs = timestampMs;
            EyeDetected = eyeDetected;
            EyeX = eyeX;
            EyeY = eyeY;
            FieldX = fieldX;
            FieldY = fieldY;
            Markers = markers;
            hasMarkerData = false;
        }

        public GazeData(uint index, uint timestampMs, bool eyeDetected, ushort eyeX, ushort eyeY, ushort fieldX, ushort fieldY, ushort markers,
            float gazePoint0X, float gazePoint0Y,
            float m0ulX, float m0ulY, float m0urX, float m0urY, float m0lrX, float m0lrY, float m0llX, float m0llY)
            : this(index, timestampMs, eyeDetected, eyeX, eyeY, fieldX, fieldY, markers)
        {
            GazePoint0X = gazePoint0X;
            GazePoint0Y = gazePoint0Y;
            M0ulX = m0ulX;
            M0ulY = m0ulY;
            M0urX = m0urX;
            M0urY = m0urY;
            M0lrX = m0lrX;
            M0lrY = m0lrY;
            M0llX = m0llX;
            M0llY = m0llY;
            hasMarkerData = true;
            ComputeScreenCoordinates();
        }

        void ComputeScreenCoordinates() 
        {

            const float markerScreenWidth = (ScreenWidth / MarkerSize);
            const float markerScreenHeight = (ScreenHeight / MarkerSize);

            //const float offsetBorder = 0.125f;
            //const float offsetX = 1 + offsetBorder;

            float offsetX = 1f;
            //float offsetY = 1.15f;

            //float x = GazePoint0X == 0 ? 0 : MathHelper.Clamp(-markerScreenWidth + (GazePoint0X + offsetX), 0, markerScreenWidth);
            //float y = GazePoint0Y == 0 ? 0 : MathHelper.Clamp(Math.Abs(GazePoint0Y + offsetY), 0, markerScreenHeight);

            float x = (markerScreenWidth + offsetX) - GazePoint0X;
            //float y = GazePoint0Y + offsetY;
            //float y = GazePoint0Y + offsetY;
            float y = Math.Abs(GazePoint0Y);

            ScreenX = (x/ markerScreenWidth) * 1920;
            ScreenY = (y / markerScreenHeight) * 1080;

            ScreenX = MathHelper.Clamp(ScreenX, 0, 1920);
            ScreenY = MathHelper.Clamp(ScreenY, 0, 1080);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} {1} {2} - Eye ({3:f2},{4:f2}) Field({5},{6}) nM: {7} Screen ({8},{9})",
                Index, TimestampMs, EyeDetected ? "Yes" : "No", EyeX, EyeY, FieldX, FieldY, Markers, ScreenX, ScreenY));
            
            if (hasMarkerData)
                sb.AppendLine(string.Format("G0 ({0},{1}) - M0 ul ({2:f2},{3:f2}) ur ({4:f2},{5:f2}) lr ({6:f2},{7:f2}) ll ({8:f2},{9:f2})",
                    GazePoint0X, GazePoint0Y,  M0ulX,  M0ulY,  M0urX,  M0urY,  M0lrX,  M0lrY,  M0llX,  M0llY));

            return sb.ToString();
        }
    }


    public class EyeTrackerServer : UdpServer
    {

        public GazeData LastGazeData { get; private set; }

        protected override void ProcessData(byte[] data)
        {
            if (data == null)
                return;
            string text = Encoding.ASCII.GetString(data, 0, data.Length);
            string[] dataArray = text.Split('\t');

            if (dataArray == null)
                return;

            LastGazeData = dataArray.Length == 8 ? ParseNoMarkerData(dataArray) : ParseMarkerData(dataArray);

            //LogEvent.Engine.Write(LastGazeData.ToString());
        }

        GazeData ParseNoMarkerData(string[] dataArray)
        {
            List<uint> convertedData = new List<uint>();

            for (int i=0; i < 8; i++)
            {
                string item = dataArray[i];
                if (string.IsNullOrEmpty(item))
                {
                    convertedData.Add(0);
                    continue;
                }
                else
                {
                    int value = int.Parse(item);
                    if (value > 0)
                        convertedData.Add((uint)value);
                    else
                        convertedData.Add(0);
                }
            }

            GazeData gazeData = new GazeData(convertedData[0], convertedData[1],
                convertedData[2] == 1 ? true : false,
                (ushort)convertedData[3], (ushort)convertedData[4], (ushort)convertedData[5], (ushort)convertedData[6], (ushort)convertedData[7]);

            return gazeData;
        }

        GazeData ParseMarkerData(string[] dataArray)
        {
            List<uint> convertedData = new List<uint>();
            List<float> convertedDataF = new List<float>();

            for (int i = 0; i < 8; i++)
            {
                string item = dataArray[i];
                if (string.IsNullOrEmpty(item))
                {
                    convertedData.Add(0);
                    continue;
                }
                else
                {
                    int value = int.Parse(item);
                    if (value > 0)
                        convertedData.Add((uint)value);
                    else
                        convertedData.Add(0);
                }
            }

            convertedDataF.Add(string.IsNullOrEmpty(dataArray[8]) ? 0 : float.Parse(dataArray[8]));
            convertedDataF.Add(string.IsNullOrEmpty(dataArray[9]) ? 0 : float.Parse(dataArray[9]));

            for (int i = 40; i <= 47; i++)
            {
                string item = dataArray[i];
                if (string.IsNullOrEmpty(item))
                {
                    convertedDataF.Add(0);
                    continue;
                }
                else
                    convertedDataF.Add(float.Parse(item));
            }

            GazeData gazeData = new GazeData(convertedData[0], convertedData[1],
                convertedData[2] == 1 ? true : false,
                (ushort)convertedData[3], (ushort)convertedData[4], (ushort)convertedData[5], (ushort)convertedData[6], (ushort)convertedData[7],
                convertedDataF[0], convertedDataF[1],
                convertedDataF[2], convertedDataF[3], convertedDataF[4], convertedDataF[5], convertedDataF[6], convertedDataF[7], convertedDataF[8], convertedDataF[9]);

            return gazeData;
        }
    }
}
