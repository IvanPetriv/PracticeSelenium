using System.Xml.Linq;
using Locators.Models;
using System.Xml;
using Locators.Exceptions;
using Locators.Objects;

namespace Locators.Utils {
    /*
        XML structure ([attribute] - optional):
    <Tests>
        <TestClass name="name_of_the_test_class">
            <TestMethod name="name_of_the_method_in_the_class">
                <TestSuite name="test_suite_kind">
                    <TestCase>
                        [<Parameter type="type_of_parameter">
                            value
                        </Parameter>]
                        ...
                        [<Return type="type_of_return">
                            
                        </Return> |
                        <Throw exception="type_of_exception"/>]
                    </TestCase>
                    ...
                </TestSuite>
                ...
            </TestMethod>
            ...
        </TestClass>
        ...
    </Tests>
    */

    /// <summary>
    /// XML tests parser which can convert XML into `TestCaseData` list object
    /// </summary>
    public class XmlTestsParser {
        public Dictionary<string, Dictionary<string, Dictionary<string, List<TestCaseData>>>> TestCases { get; private set; }

        public XmlTestsParser(string filePath) {
            TestCases = [];
            ParseTestCasesFromXml(filePath);
        }

        public IEnumerable<TestCaseData> GetTestData(string testClass, string testMethod, string testSuite) {
            return TestCases[testClass][testMethod][testSuite];
        }

        private void ParseTestCasesFromXml(string filePath) {
            var xmlDoc = XDocument.Load(filePath);

            var testElement = xmlDoc.Element("Tests")
                ?? throw new ArgumentNullException($"The file '{filePath}' does not contain <Tests> tag");

            // Iterates through all TestClass tags
            foreach (var testClassElement in testElement.Elements("TestClass")) {
                var className = testClassElement.Attribute("name")?.Value
                    ?? throw new ArgumentException($"In file '{filePath}' a <TestClass> tag does not have 'name' attribute");

                var classDictionary = TestCases.GetOrAdd(className);

                // Iterates through all TestMethod tags
                foreach (var testMethodElement in testClassElement.Elements("TestMethod")) {
                    var methodName = testMethodElement.Attribute("name")?.Value
                        ?? throw new ArgumentException($"In file '{filePath}' a <TestMethod> tag does not have 'name' attribute");

                    var methodDictionary = classDictionary.GetOrAdd(methodName);

                    // Iterates through all TestSuite tags
                    foreach (var testSuiteElement in testMethodElement.Elements("TestSuite")) {
                        var suiteName = testSuiteElement.Attribute("name")?.Value
                            ?? throw new ArgumentException($"In file '{filePath}' a <TestSuite> tag does not have 'name' attribute");

                        var suiteList = methodDictionary.GetOrAdd(suiteName);

                        // Uterates through all TestCase elements
                        foreach (var testCaseElement in testSuiteElement.Elements("TestCase")) {
                            var parameters = testCaseElement.Elements("Parameter")
                                .Select(ParseParameter)
                                .ToList();

                            TestCaseData testCase = new([.. parameters]);

                            // Defines returns and thros if present
                            string? returnTypeString = testCaseElement.Descendants("Return").FirstOrDefault()?.Attribute("type")?.Value;
                            string? exceptionTypeString = testCaseElement.Descendants("Throw").FirstOrDefault()?.Attribute("exception")?.Value;

                            // Checks if both present
                            if (!string.IsNullOrEmpty(returnTypeString) && !string.IsNullOrEmpty(exceptionTypeString)) {
                                throw new XmlException($"In file '{filePath}' <TestCase> does not allow both <Return> and <Throw> in the same tag");
                            }
                            // Checks if return present
                            else if (!string.IsNullOrEmpty(returnTypeString)) {
                                Type returnType = ResolveType(returnTypeString)
                                    ?? throw new XmlException($"In file '{filePath}' <Return> does not have a type");
                                string returnValue = testCaseElement.Descendants("Return").FirstOrDefault()?.Value
                                    ?? string.Empty;

                                if (returnType == typeof(Type)) {
                                    testCase.Returns(ResolveType(returnValue));
                                } else if (returnType == typeof(string)) {
                                    testCase.Returns(returnValue);
                                } else {
                                    testCase.Returns(Activator.CreateInstance(returnType, new object[] { returnValue }));
                                }
                            }
                            // Checks if throw present
                            else if (!string.IsNullOrEmpty(exceptionTypeString)) {
                                testCase.Returns(ResolveType(exceptionTypeString));
                            }

                            suiteList.Add(testCase);
                        }
                    }
                }
            }
        }

        private object ParseParameter(XElement parameterElement) {
            if (parameterElement.Name != "Parameter") {
                throw new ArgumentException($"Tag '{parameterElement.Name}' is not 'Parameter'");
            }

            var fields = parameterElement.Elements("Field")
                .Select(fieldElement => {
                    var fieldTypeString = fieldElement.Attribute("type")?.Value;
                    var fieldType = ResolveType(fieldTypeString)
                        ?? throw new NullReferenceException($"Field '{fieldElement}' does not have a resolvable type '{fieldTypeString}'");
                    return Convert.ChangeType(fieldElement.Value, fieldType);
                })
                .ToArray();

            string parameterTypeString = parameterElement.Attribute("type")?.Value;
            Type parameterType = ResolveType(parameterTypeString)
                ?? throw new NullReferenceException($"Parameter does not have a resolvable type '{parameterTypeString}'");

            if (parameterType == typeof(string)) {
                return parameterElement.Value;
            } else {
                return Activator.CreateInstance(parameterType, fields);
            }
        }


        private Type? ResolveType(string? typeName) {
            if (string.IsNullOrEmpty(typeName)) {
                return null;
            }

            if (typeName == "Type") {
                return typeof(Type);
            }

            // Handle built-in types
            if (BuiltInTypes.TryGetValue(typeName, out var type)) {
                return type;
            }

            // Handle nullable types
            if (typeName.EndsWith('?')) {
                var underlyingTypeName = typeName.TrimEnd('?');
                var underlyingType = ResolveType(underlyingTypeName);
                if (underlyingType != null && underlyingType.IsValueType) {
                    return typeof(Nullable<>).MakeGenericType(underlyingType);
                }
            }

            // Try to get the type directly
            type = Type.GetType(typeName);

            return type;
            
        }

        private static readonly Dictionary<string, Type> BuiltInTypes = new() {
            { "string", typeof(string) },
            { "int", typeof(int) },
            { "long", typeof(long) },
            { "float", typeof(float) },
            { "double", typeof(double) },
            { "decimal", typeof(decimal) },
            { "bool", typeof(bool) },
            { "object", typeof(object) },
            { "char", typeof(char) },
            { "byte", typeof(byte) },
            { "sbyte", typeof(sbyte) },
            { "short", typeof(short) },
            { "ushort", typeof(ushort) },
            { "uint", typeof(uint) },
            { "ulong", typeof(ulong) },
            { "void", typeof(void) },
            { "User", typeof(User) },
            { "Letter", typeof(Letter) },
            { "IncorrectUrlException", typeof(IncorrectUrlException) },
            { "LoginFailedException", typeof(LoginFailedException) },
            { "NotLoggedInException", typeof(NotLoggedInException) },
            { "SendEmailFailedException", typeof(SendEmailFailedException) },
            { "InboxPageGmail", typeof(InboxPageGmail) },
            { "InboxPageOutlook", typeof(InboxPageOutlook) },
            { "LoginPageGmail", typeof(LoginPageGmail) },
            { "LoginPageOutlook", typeof(LoginPageOutlook) }
        };
    }

    public static class DictionaryExtensions {
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new() {
            if (!dictionary.TryGetValue(key, out var value)) {
                value = new TValue();
                dictionary[key] = value;
            }
            return value;
        }
    }

   
}
