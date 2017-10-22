using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace JsonSerializerAndDeSerializer
{
    [DataContract]
    class sigJson
    {
        [DataMember]
        public string sigName
        {
            get;
            set;
        }

        [DataMember]
        public string pyh
        {
            get;
            set;
        }
    }
}
