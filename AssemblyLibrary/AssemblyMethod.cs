namespace AssemblyLibrary
{
    public class AssemblyMethod
    {
        public string MethodName { get; set; }
        public string methodSignature;
        public string GetMethodInfo => MethodName + " " + methodSignature;

        public AssemblyMethod(string name, string signature)
        {
            MethodName = name;
            methodSignature = signature;
        }

    }
}