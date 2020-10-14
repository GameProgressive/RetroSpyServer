﻿using RetroSpyServices.Direct2Game.Entity.Interface;
using RetroSpyServices.Direct2Game.Entity.Structure.Model;
using System;
using System.Xml.Linq;

namespace RetroSpyServices.Direct2Game.Service
{
    public class Direct2GameService : IDirect2GameService
    {
        public string Test(string s)
        {
            Console.WriteLine("Test Method Executed!");
            return s;
        }
        public void XmlMethod(XElement xml)
        {
            Console.WriteLine(xml.ToString());
        }
        public Direct2GameServiceModel TestDirect2GameServiceModel(Direct2GameServiceModel customModel)
        {
            return customModel;
        }
    }


}
