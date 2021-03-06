﻿using System.Runtime.Serialization;

namespace WebServices.RetroSpyServices.Sake.Entity.Structure.Request
{
    [DataContract(Namespace = SakeXmlLable.SakeNameSpace)]
    public class SakeRateRecordRequest : SakeRequestBase
    {
        [DataMember(Name = SakeXmlLable.Rating)]
        public uint Rating;
    }
}
