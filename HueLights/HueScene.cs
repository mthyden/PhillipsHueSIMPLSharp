using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace HueLights
{
    public class HueScene
    {
        public string ID;
        public string Name;
        public string AssignedLoad;
        public string Group;

        public HueScene(string id, string name, string load)
        {
            this.ID = id;
            this.Name = name;
            this.AssignedLoad = load;
        }
    }
}