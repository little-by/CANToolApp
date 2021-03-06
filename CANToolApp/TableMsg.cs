﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANToolApp
{
    public partial class TableMsg
    {
        public TableMsg()
        {
            for(int i = 0; i < 64; i++)
            {
                Binarydata[i] = 0;
                Signalpos[i] = 0;
            }
        }
        public TableMsg(DataTable dt)
        {
            for (int i = 0; i < 64; i++)
            {
                Binarydata[i] = 0;
                Signalpos[i] = 0;
            }
            this.DataTable = dt;
        }


        private DataTable dataTable=new DataTable();
        private int[] binarydata = new int[64];
        private int[] signalpos = new int[64];
        private Dictionary<string, string> returnedData = new Dictionary<string, string>();
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

        public Dictionary<string, string> ReturnedData
        {
            get
            {
                return returnedData;
            }

            set
            {
                returnedData = value;
            }
        }

        public DataTable DataTable
        {
            get
            {
                return DataTable1;
            }

            set
            {
                DataTable1 = value;
            }
        }

        public DataTable DataTable1
        {
            get
            {
                return dataTable;
            }

            set
            {
                dataTable = value;
            }
        }
    }

}
