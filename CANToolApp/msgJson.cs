using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JsonSerializerAndDeSerializer
{
    [DataContract]
    class MsgJson
    {
        [DataMember]
        public string message
        {
            get;
            set;
        }

        [DataMember]
        public SigJson[] signal
        {
            get;
            set;
        }
    }
}
