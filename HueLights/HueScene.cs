
namespace HueLights
{
    public class HueScene
    {
        public string ID;
        public string Name;
        public string[] Loads = new string[20];
        public string Group;

        public HueScene(string id, string name, string[] loads)
        {
            this.ID = id;
            this.Name = name;
            this.Loads = loads;
        }
    }
}