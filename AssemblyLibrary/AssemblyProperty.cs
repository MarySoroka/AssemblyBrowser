namespace AssemblyLibrary
{
    public class AssemblyProperty
    {
        public string propertyType;
        public string propertyName { get; set; }
        public string GetPropertiesInfo => propertyType + " " + propertyName;

        public AssemblyProperty(string type,string name)
        {
            propertyType = type;
            propertyName = name;
        }

    }
}