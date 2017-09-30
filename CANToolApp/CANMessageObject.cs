using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANToolApp
{
    class CANMessageObject
    {
        public CANMessageObject()
        {
            canMessageSymbol = "BO_".ToCharArray();
            Separator = ':';
        }


        private char[] canMessageSymbol = new char[32];
        private UInt32 id;
        private char[] messageName = new char[32];
        private char separator;
        private byte[] DLC = new byte[2];
        private char[] nodeName = new char[32];

        public char[] CanMessageSymbol
        {
            get
            {
                return canMessageSymbol;
            }

            set
            {
                canMessageSymbol = value;
            }
        }

        public uint Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public char[] MessageName
        {
            get
            {
                return messageName;
            }

            set
            {
                messageName = value;
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

        public byte[] DLC1
        {
            get
            {
                return DLC;
            }

            set
            {
                DLC = value;
            }
        }
    }
}
