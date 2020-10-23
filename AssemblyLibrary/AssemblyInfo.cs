using System.Collections.Generic;

namespace AssemblyLibrary
{
    public class AssemblyInfo
    {
        public string AssemblyName { get; set; }
        public readonly List<AssemblyNamespace> Namespaces;

        public AssemblyInfo(string name, List<AssemblyNamespace> n)
        {
            AssemblyName = name;
            Namespaces = n;
        }
    }
}