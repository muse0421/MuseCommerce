using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Redis
{
    [ProtoContract]
    public class TestQuene
    {

        [ProtoMember(1)]
        public int Id
        {
            get;
            set;
        }


        [ProtoMember(2)]
        public List<String> data
        {
            get;
            set;
        }


        public override string ToString()
        {
            String str = Id + ":";
            foreach (String d in data)
            {
                str += d + ",";
            }
            return str;
        }

    }  
}
