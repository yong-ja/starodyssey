using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AvengersUtd.Odyssey.Utils.Logging;

namespace AvengersUtd.Odyssey.Network
{
    public struct GazeData
    {
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
            GazePoint0Y = GazePoint0Y;
            M0ulX = m0ulX;
            M0ulY = m0ulY;
            M0urX = m0urX;
            M0urY = m0urY;
            M0lrX = m0lrX;
            M0lrY = m0lrY;
            M0llX = m0llX;
            M0llY = m0llY;
            hasMarkerData = true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} {1} {2} - Eye ({3:f2},{4:f2}) Field({5},{6}) nM: {7}",
                Index, TimestampMs, EyeDetected ? "Yes" : "No", EyeX, EyeY, FieldX, FieldY, Markers));
            
            if (hasMarkerData)
                sb.AppendLine(string.Format("G0 ({0},{1}) - M0 ul ({2:f2},{3:f2}) ur ({4:f2},{5:f2}) lr ({6:f2},{7:f2}) ll ({8:f2},{9:f2})",
                    GazePoint0X, GazePoint0Y,  M0ulX,  M0ulY,  M0urX,  M0urY,  M0lrX,  M0lrY,  M0llX,  M0llY));

            return sb.ToString();
        }
    }


    public class EyeTrackerServer : UdpServer
    {

        protected override void ProcessData(byte[] data)
        {
            if (data == null)
                return;
            string text = Encoding.ASCII.GetString(data, 0, data.Length);
            string[] dataArray = text.Split('\t');

            if (dataArray == null)
                return;

            GazeData gazeData = dataArray.Length == 8 ? ParseNoMarkerData(dataArray) : ParseMarkerData(dataArray);

            LogEvent.Engine.Write(gazeData.ToString());
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
                    convertedData.Add(uint.Parse(item));
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
                    convertedData.Add(uint.Parse(item));
            }

            convertedDataF.Add(string.IsNullOrEmpty(dataArray[9]) ? 0 : float.Parse(dataArray[9]));
            convertedDataF.Add(string.IsNullOrEmpty(dataArray[10]) ? 0 : float.Parse(dataArray[10]));

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
