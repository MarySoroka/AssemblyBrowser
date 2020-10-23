using System.IO;
using System.Linq;
using AssemblyLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssemblyBrowserTests
{    [TestClass]
    public class BrowserTest
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

            var namespaces = _assemblyInfo.Namespaces;
            var typesFaker = namespaces.First(n => n.NamespaceName == "FakerLibrary").Info;
            var typesCountActualFaker = typesFaker.Count;

            var typesGenerators = namespaces.First(n => n.NamespaceName == "FakerLibrary.Generators.TypesGenerators").Info;
            var typesCountActualGenerators = typesGenerators.Count;

            Assert.AreEqual(4, typesCountActualFaker);
            Assert.AreEqual(7, typesCountActualGenerators);
        }

        [TestMethod()]
        public void MethodsCheck()
        {
            var namespaces = _assemblyInfo.Namespaces;
            var testType = namespaces.First(n => n.NamespaceName == "FakerLibrary").Info.FirstOrDefault(t => t.TypeName == "IGenerator");
            const string methodName = "CanGenerate";
            const string methodSignature = "(Type)";

            Assert.IsNotNull(testType);
            Assert.IsTrue(testType.Methods.Count == 2);
            Assert.IsTrue(testType.Methods.Any(m => m.MethodName == methodName));
            Assert.IsTrue(testType.Methods.FirstOrDefault(m => m.MethodName == methodName)?.methodSignature == methodSignature);
        }

        [TestMethod()]
        public void TypeCheck()
        {
            var namespaces = _assemblyInfo.Namespaces;
            var testType = namespaces.First(n => n.NamespaceName == "FakerLibrary").Info.FirstOrDefault(t => t.TypeName == "Faker");
            const int expectedFieldsAmounts = 4;
            const int expectedPropertiesAmounts = 1;
            const int expectedMethodsAmounts = 7;

            Assert.IsNotNull(testType);
            Assert.AreEqual(expectedFieldsAmounts,testType.Fields.Count);
            Assert.AreEqual(expectedPropertiesAmounts, testType.Properties.Count);
            Assert.AreEqual(expectedMethodsAmounts, testType.Methods.Count);
        }


    }
}