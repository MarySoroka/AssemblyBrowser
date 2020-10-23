namespace AssemblyLibrary
{
    public class AssemblyField
    {
        public string fieldType;
        public string FieldName { get; set; }
        public string GetFieldsInfo => fieldType + " " + FieldName;

        public AssemblyField(string type, string name)
        {
            fieldType = type;
            FieldName = name;
        }
    }
}