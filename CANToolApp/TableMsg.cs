using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANToolApp
{
    class TableMsg
    {
        TableMsg()
        {
            for(int i = 0; i < 64; i++)
            {
                Binarydata[i] = 0;
                Signalpos[i] = 0;
            }
        }



        private int[] binarydata = new int[64];
        private int[] signalpos = new int[64];

        public int[] Binarydata
        {
            get
            {
                return binarydata;
            }

            set
            {
                binarydata = value;
            }
        }

        public int[] Signalpos
        {
            get
            {
                return signalpos;
            }

            set
            {
                signalpos = value;
            }
        }
    }

}
