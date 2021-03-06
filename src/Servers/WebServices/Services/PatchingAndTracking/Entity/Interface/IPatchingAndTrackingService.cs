﻿using RetroSpyServices.PatchingAndTracking.Entity.Structure.Model;
using System.ServiceModel;

namespace RetroSpyServices.PatchingAndTracking.Entity.Interface
{
    [ServiceContract]
    public interface IPatchingAndTrackingService
    {
        [OperationContract]
        string Test(string s);

        [OperationContract]
        void XmlMethod(System.Xml.Linq.XElement xml);

        [OperationContract]
        PatchingAndTrackingServiceModel TestPatchingAndTrackingServiceModel(PatchingAndTrackingServiceModel inputModel);
    }
}