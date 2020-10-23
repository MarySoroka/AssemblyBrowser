using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblyLibrary;
using System.Linq;
using System.IO;

namespace AssemblyLibTests
{
    [TestClass]
    public class UnitTest1
    {
        private BrowserLibrary _browserLibrary;
        private AssemblyInfo _assemblyInfo;

        [TestInitialize]
        public void Setup()
        {
            _browserLibrary = new BrowserLibrary();
            var pluginsPath = Directory.GetCurrentDirectory() + "\\FakerLibrary.dll";
            _assemblyInfo = _browserLibrary.GetResult(pluginsPath);
        }


        [TestMethod]
        public void NamespaceNameTest()
        {
            var namespaces = _assemblyInfo.Namespaces;
            Assert.IsTrue(namespaces.Any(n => n.NamespaceName == "FakerLibrary"));
        }

        [TestMethod]
        public void TypesCount()
        {
            const int typesCountExpectedFaker = 5;
            const int typesCountExpectedGens = 7;

            var namespaces = _assemblyInfo.Namespaces;
            var typesFaker = namespaces.First(n => n.NamespaceName == "FakerLibrary").info;
            var typesCountActualFaker = typesFaker.Count;

            var typesGenerators = namespaces.First(n => n.NamespaceName == "FakerLibrary.Generators.TypesGenerators").info;
            var typesCountActualGenerators = typesGenerators.Count;

            Assert.AreEqual(typesCountExpectedFaker, typesCountActualFaker);
            Assert.AreEqual(typesCountExpectedGens, typesCountActualGenerators);
        }

        [TestMethod()]
        public void MethodsCheck()
        {
            var namespaces = _assemblyInfo.Namespaces;
            var testType = namespaces.First(n => n.NamespaceName == "FakerLibrary").info.FirstOrDefault(t => t.TypeName == "IGenerator");
            const string methodname = "CanGenerate";
            const string methodSignature = "(Type)";

            Assert.IsNotNull(testType);
            Assert.IsTrue(testType.Methods.Count == 2);
            Assert.IsTrue(testType.Methods.Any(m => m.MethodName == methodname));
            Assert.IsTrue(testType.Methods.FirstOrDefault(m => m.MethodName == methodname)?.methodSignature == methodSignature);
        }

        [TestMethod()]
        public void TypeCheck()
        {
            var namespaces = _assemblyInfo.Namespaces;
            var testType = namespaces.First(n => n.NamespaceName == "FakerLibrary").info.FirstOrDefault(t => t.TypeName == "Faker");
            const int expectedFieldsAmounts = 4;
            const int expectedPropertiesAmounts = 1;
            const int expectedMethodsAmounts = 10;

            Assert.IsNotNull(testType);
            Assert.AreEqual(expectedFieldsAmounts,testType.Fields.Count);
            Assert.AreEqual(expectedPropertiesAmounts, testType.Properties.Count);
            Assert.AreEqual(expectedMethodsAmounts, testType.Methods.Count);
        }


    }
}
