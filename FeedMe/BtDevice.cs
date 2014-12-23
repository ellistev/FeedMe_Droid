namespace FeedMe
{
    public class BtDevice
    {
        public string Name;
        public string Type;
        public string MacAddress;
        public int Strength;
        public string Uuid;
        
        public BtDevice(){}

        public BtDevice(string name, string type, string macAddress, int strength, string uuid)
        {
            this.Name = name;
            this.Type = type;
            this.MacAddress = macAddress;
            this.Strength = strength;
            this.Uuid = uuid;
        }


    }
}