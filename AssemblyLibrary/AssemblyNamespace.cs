using System.Collections.Generic;

namespace AssemblyLibrary
{
    public class AssemblyNamespace
    {
        public List<TypeInfo> info { get; set; }
        public string NamespaceName { get; set; }

        public AssemblyNamespace(string name, List<TypeInfo> types )
        {
            NamespaceName = name;
            info = types;
        }
    }
}