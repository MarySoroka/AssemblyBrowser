using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace AssemblyLibrary
{
    public class BrowserLibrary
    {
        private static Assembly LoadLibraryForAssembly(string libraryPath)
        {
            var file = new FileInfo(libraryPath);
            var currentAssembly = Assembly.LoadFrom(file.FullName);
            return currentAssembly;
        }

        private List<AssemblyNamespace> GetNamespaces(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var assemblyInfo = new Dictionary<string, List<TypeInfo>>();
            foreach (var t in types)
            {
                if (IsCompilerGenerated(t)) continue;
                var typeName = t.IsGenericType ? GenericType(t) : t.Name;

                var typeInfo = new TypeInfo(typeName)
                {
                    Fields = GetFields(t), Properties = GetProperties(t), Methods = GetMethods(t)
                };
                if (assemblyInfo.ContainsKey(t.Namespace))
                {
                    assemblyInfo[t.Namespace].Add(typeInfo);
                }
                else
                {
                    var infos = new List<TypeInfo> {typeInfo};
                    assemblyInfo.Add(t.Namespace, infos);
                }
            }

            return assemblyInfo.Keys.Select(k => new AssemblyNamespace(k, assemblyInfo[k])).ToList();
        }


        public AssemblyInfo GetResult(string assemblyPath)
        {
            var assembly = LoadLibraryForAssembly(assemblyPath);
            var namespaces = GetNamespaces(assembly);
            var assemblyInfo = new AssemblyInfo(assembly.FullName, namespaces);

            return assemblyInfo;
        }

        private static List<AssemblyField> GetFields(Type type)
        {
            return (from f in type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.DeclaredOnly) where f.GetCustomAttribute<CompilerGeneratedAttribute>() == null let fieldType = f.FieldType.IsGenericType ? GenericType(f.FieldType) : f.FieldType.Name select new AssemblyField(fieldType, f.Name)).ToList();
        }

        private List<AssemblyProperty> GetProperties(IReflect type)
        {
            var currentProperties = new List<AssemblyProperty>();
            foreach (var p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance |
                                                 BindingFlags.NonPublic | BindingFlags.Static |
                                                 BindingFlags.DeclaredOnly))
            {
                var propertyType = p.PropertyType.IsGenericType ? GenericType(p.PropertyType) : p.PropertyType.Name;

                var assemblyProperty = new AssemblyProperty(propertyType, p.Name);

                if (!IsCompilerGenerated(p.PropertyType))
                    currentProperties.Add(assemblyProperty);
            }

            return currentProperties;
        }

        private static List<AssemblyMethod> GetMethods(IReflect type)
        {
            return (from m in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic |
                                              BindingFlags.Static | BindingFlags.DeclaredOnly)
                let currentMethodSignature = GetMethodSignature(m)
                select new AssemblyMethod(m.Name, currentMethodSignature)).ToList();
        }

        private static string GetMethodSignature(MethodBase method)
        {
            var signature = new StringBuilder();

            signature.Append("(");
            var parameters = method.GetParameters();
            foreach (var p in parameters)
            {
                if (p.IsOut)
                    signature.Append("out ");
                else if (p.IsIn)
                    signature.Append("in ");
                else if (p.ParameterType.IsByRef)
                    signature.Append("ref ");

                if (p.ParameterType.IsGenericType)
                {
                    var s = GenericType(p.ParameterType);
                    signature.Append(s + ',');
                }
                else
                    signature.Append(p.ParameterType.Name + ',');
            }

            if (signature[signature.Length - 1] == ',')
                signature[signature.Length - 1] = ')';
            else
                signature.Append(')');

            return signature.ToString();
        }

        private static string GenericType(Type type)
        {
            var str = new StringBuilder();
            str.Append(type.Name.Split('`')[0]);
            str.Append('<');
            foreach (var t in type.GetGenericArguments())
            {
                var s = t.IsGenericType ? GenericType(t) : t.Name;
                str.Append(s + ',');
            }

            str[str.Length - 1] = '>';

            return str.ToString();
        }

        private static bool IsCompilerGenerated(MemberInfo type)
        {
            return Attribute.GetCustomAttribute(type, typeof(CompilerGeneratedAttribute)) != null;
        }
    }
}