using System;




namespace CANToolApp
{
    class Encode
    {
        static int i = 0;
        public static string encode(CANMessageObject cmo,CANSignalObject cso,Boolean msgOrSgn,Boolean torT,double value)
        {
            string canstr="";
            if (msgOrSgn)
            {
                canstr = cmo.CanMessageSymbol + cmo.Id.ToString() +" "+ cmo.MessageName + cmo.Separator + cmo.DLC1.ToString() +" "+ cmo.NodeName;
            }else
            {
                canstr = ""+cso.CanSignalSymbol + cso.SignalName + cso.Separator + cso.Start_length_pattern.ToString() +"("+ cso.A1+"," + cso.B1+")" + "["+cso.C1 + cso.D1+"]" + cso.Measure + cso.NodeName;
            }
            if (torT)
            {
                canstr = "t" + i.ToString().PadLeft(3, '0')+canstr;
            }
            else
            {
                canstr = "T" + i.ToString().PadLeft(8, '0') + canstr;
            }
            i++;
            return canstr;

        }




    }


}

