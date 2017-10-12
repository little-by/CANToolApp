using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANToolApp
{
    class CANSignalObject
    {
        private char[] canSignalSymbol = new char[32];
        private char[] signalName = new char[32];
        private char separator;
        private byte[] start_length_pattern = new byte[20];  //byte[2]对应C#中的unsigned char
        private double A, B, C, D;
        private char[] measure = new char[32];
        private char[] nodeName = new char[255];

        public CANSignalObject()
        {
            canSignalSymbol = "SG_".ToCharArray();
            separator = ':';
        }

        public char[] CanSignalSymbol
        {
            get
            {
                return canSignalSymbol;
            }

            set
            {
                canSignalSymbol = value;
            }
        }

        public char[] SignalName
        {
            get
            {
                return signalName;
            }

            set
            {
                signalName = value;
            }
        }

        public byte[] Start_length_pattern
        {
            get
            {
                return start_length_pattern;
            }

            set
            {
                start_length_pattern = value;
            }
        }

        public double A1
        {
            get
            {
                return A;
            }

            set
            {
                A = value;
            }
        }

        public double B1
        {
            get
            {
                return B;
            }

            set
            {
                B = value;
            }
        }

        public double C1
        {
            get
            {
                return C;
            }

            set
            {
                C = value;
            }
        }

        public double D1
        {
            get
            {
                return D;
            }

            set
            {
                D = value;
            }
        }

        public char[] Measure
        {
            get
            {
                return measure;
            }

            set
            {
                measure = value;
            }
        }

        public char[] NodeName
        {
            get
            {
                return nodeName;
            }

            set
            {
                nodeName = value;
            }
        }

        public char Separator
        {
            get
            {
                return separator;
            }

            set
            {
                separator = value;
            }
        }
    }
}
