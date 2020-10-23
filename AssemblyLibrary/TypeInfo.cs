using System.Collections.Generic;

namespace AssemblyLibrary
{
    public class TypeInfo
    {
        public string TypeName { get; set; }
        public List<AssemblyField> Fields { get; set; }
        public List<AssemblyProperty> Properties { get; set; }
        public List<AssemblyMethod> Methods { get; set; }
        public TypeInfo(string name)
        {
            TypeName = name;
            Fields = new List<AssemblyField>();
            Properties = new List<AssemblyProperty>();
            Methods = new List<AssemblyMethod>();
        }
    }
}