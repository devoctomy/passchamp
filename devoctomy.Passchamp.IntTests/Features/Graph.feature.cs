﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (https://www.specflow.org/).
//      SpecFlow Version:3.4.0.0
//      SpecFlow Generator Version:3.4.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace devoctomy.Passchamp.IntTests.Features
{
    using TechTalk.SpecFlow;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.4.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class GraphFeature : object, Xunit.IClassFixture<GraphFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
        private string[] _featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "Graph.feature"
#line hidden
        
        public GraphFeature(GraphFeature.FixtureData fixtureData, devoctomy_Passchamp_IntTests_XUnitAssemblyFixture assemblyFixture, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Graph", "Method of building up many small actions into a larger, more complex task.", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void TestTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioInitialize(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public virtual void ScenarioStart()
        {
            testRunner.OnScenarioStart();
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        void System.IDisposable.Dispose()
        {
            this.TestTearDown();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="01) Encrypt string using a password and write to disk")]
        [Xunit.TraitAttribute("FeatureTitle", "Graph")]
        [Xunit.TraitAttribute("Description", "01) Encrypt string using a password and write to disk")]
        [Xunit.InlineDataAttribute("saltgenerator", "16", "ivgenerator", "16", "derive", "123", "32", "Hello World!", "encoder", "encrypt", "joiner", "writer", "Output/test.dat", new string[0])]
        public virtual void _01EncryptStringUsingAPasswordAndWriteToDisk(string saltGeneratorNodeName, string saltLength, string ivGeneratorNodeName, string ivLength, string deriveKeyNodeName, string password, string keyLength, string plainText, string utf8EncoderNodeName, string encryptNodeName, string joinerNodeName, string fileWriterNodeName, string fileName, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("SaltGeneratorNodeName", saltGeneratorNodeName);
            argumentsOfScenario.Add("SaltLength", saltLength);
            argumentsOfScenario.Add("IvGeneratorNodeName", ivGeneratorNodeName);
            argumentsOfScenario.Add("IvLength", ivLength);
            argumentsOfScenario.Add("DeriveKeyNodeName", deriveKeyNodeName);
            argumentsOfScenario.Add("Password", password);
            argumentsOfScenario.Add("KeyLength", keyLength);
            argumentsOfScenario.Add("PlainText", plainText);
            argumentsOfScenario.Add("Utf8EncoderNodeName", utf8EncoderNodeName);
            argumentsOfScenario.Add("EncryptNodeName", encryptNodeName);
            argumentsOfScenario.Add("JoinerNodeName", joinerNodeName);
            argumentsOfScenario.Add("FileWriterNodeName", fileWriterNodeName);
            argumentsOfScenario.Add("FileName", fileName);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("01) Encrypt string using a password and write to disk", null, tagsOfScenario, argumentsOfScenario);
#line 5
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 6
 testRunner.Given("A new dictionary of nodes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 7
 testRunner.And(string.Format("RandomByteGeneratorNode named {0} with a length of {1} and NextKey of {2}", saltGeneratorNodeName, saltLength, ivGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 8
 testRunner.And(string.Format("RandomByteGeneratorNode named {0} with a length of {1} and NextKey of {2}", ivGeneratorNodeName, ivLength, deriveKeyNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 9
 testRunner.And(string.Format("DeriveKeyFromPasswordNode named {0} with a password of {1} and key length of {2} " +
                            "and NextKey of {3}", deriveKeyNodeName, password, keyLength, utf8EncoderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 10
 testRunner.And(string.Format("Utf8EncoderNode named {0} with plain text of {1} and NextKey of {2}", utf8EncoderNodeName, plainText, encryptNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 11
 testRunner.And(string.Format("EncryptNode named {0} and NextKey of {1}", encryptNodeName, joinerNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 12
 testRunner.And(string.Format("ArrayJoinerNode named {0} and NextKey of {1}", joinerNodeName, fileWriterNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 13
 testRunner.And(string.Format("FileWriterNode named {0} with a filename of {1}", fileWriterNodeName, fileName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 14
 testRunner.And(string.Format("Node {0} input pin Salt connected to node {1} output pin RandomBytes", deriveKeyNodeName, saltGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 15
 testRunner.And(string.Format("Node {0} input pin PlainTextBytes connected to node {1} output pin EncodedBytes", encryptNodeName, utf8EncoderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 16
 testRunner.And(string.Format("Node {0} input pin Iv connected to node {1} output pin RandomBytes", encryptNodeName, ivGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 17
 testRunner.And(string.Format("Node {0} input pin Key connected to node {1} output pin Key", encryptNodeName, deriveKeyNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 18
 testRunner.And(string.Format("Node {0} input pin Part1 connected to node {1} output pin RandomBytes", joinerNodeName, ivGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 19
 testRunner.And(string.Format("Node {0} input pin Part2 connected to node {1} output pin EncryptedBytes", joinerNodeName, encryptNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 20
 testRunner.And(string.Format("Node {0} input pin Part3 connected to node {1} output pin RandomBytes", joinerNodeName, saltGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 21
 testRunner.And(string.Format("Node {0} input pin InputData connected to node {1} output pin JoinedOutput", fileWriterNodeName, joinerNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 22
 testRunner.And(string.Format("All nodes added to a new graph with a start node named {0}", saltGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 23
 testRunner.When("Execute graph", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 24
 testRunner.Then(string.Format("Output file {0} created of 48 bytes in length", fileName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 25
 testRunner.And("All nodes executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 26
 testRunner.And("All all nodes executed in correct order of saltgenerator,ivgenerator,derive,encod" +
                        "er,encrypt,joiner,writer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="02) Encrypt string using a password with scrypt and write to disk")]
        [Xunit.TraitAttribute("FeatureTitle", "Graph")]
        [Xunit.TraitAttribute("Description", "02) Encrypt string using a password with scrypt and write to disk")]
        [Xunit.InlineDataAttribute("saltgenerator", "16", "ivgenerator", "16", "derive", "123", "32", "Hello World!", "encoder", "encrypt", "joiner", "writer", "Output/test2.dat", new string[0])]
        public virtual void _02EncryptStringUsingAPasswordWithScryptAndWriteToDisk(string saltGeneratorNodeName, string saltLength, string ivGeneratorNodeName, string ivLength, string deriveKeyNodeName, string password, string keyLength, string plainText, string utf8EncoderNodeName, string encryptNodeName, string joinerNodeName, string fileWriterNodeName, string fileName, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("SaltGeneratorNodeName", saltGeneratorNodeName);
            argumentsOfScenario.Add("SaltLength", saltLength);
            argumentsOfScenario.Add("IvGeneratorNodeName", ivGeneratorNodeName);
            argumentsOfScenario.Add("IvLength", ivLength);
            argumentsOfScenario.Add("DeriveKeyNodeName", deriveKeyNodeName);
            argumentsOfScenario.Add("Password", password);
            argumentsOfScenario.Add("KeyLength", keyLength);
            argumentsOfScenario.Add("PlainText", plainText);
            argumentsOfScenario.Add("Utf8EncoderNodeName", utf8EncoderNodeName);
            argumentsOfScenario.Add("EncryptNodeName", encryptNodeName);
            argumentsOfScenario.Add("JoinerNodeName", joinerNodeName);
            argumentsOfScenario.Add("FileWriterNodeName", fileWriterNodeName);
            argumentsOfScenario.Add("FileName", fileName);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("02) Encrypt string using a password with scrypt and write to disk", null, tagsOfScenario, argumentsOfScenario);
#line 33
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 34
 testRunner.Given("A new dictionary of nodes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 35
 testRunner.And(string.Format("RandomByteGeneratorNode named {0} with a length of {1} and NextKey of {2}", saltGeneratorNodeName, saltLength, ivGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 36
 testRunner.And(string.Format("RandomByteGeneratorNode named {0} with a length of {1} and NextKey of {2}", ivGeneratorNodeName, ivLength, deriveKeyNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 37
 testRunner.And(string.Format("SCryptNode named {0} with a password of {1} and NextKey of {2}", deriveKeyNodeName, password, utf8EncoderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 38
 testRunner.And(string.Format("Utf8EncoderNode named {0} with plain text of {1} and NextKey of {2}", utf8EncoderNodeName, plainText, encryptNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 39
 testRunner.And(string.Format("EncryptNode named {0} and NextKey of {1}", encryptNodeName, joinerNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 40
 testRunner.And(string.Format("ArrayJoinerNode named {0} and NextKey of {1}", joinerNodeName, fileWriterNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 41
 testRunner.And(string.Format("FileWriterNode named {0} with a filename of {1}", fileWriterNodeName, fileName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 42
 testRunner.And(string.Format("Node {0} input pin Salt connected to node {1} output pin RandomBytes", deriveKeyNodeName, saltGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 43
 testRunner.And(string.Format("Node {0} input pin PlainTextBytes connected to node {1} output pin EncodedBytes", encryptNodeName, utf8EncoderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 44
 testRunner.And(string.Format("Node {0} input pin Iv connected to node {1} output pin RandomBytes", encryptNodeName, ivGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 45
 testRunner.And(string.Format("Node {0} input pin Key connected to node {1} output pin Key", encryptNodeName, deriveKeyNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 46
 testRunner.And(string.Format("Node {0} input pin Part1 connected to node {1} output pin RandomBytes", joinerNodeName, ivGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 47
 testRunner.And(string.Format("Node {0} input pin Part2 connected to node {1} output pin EncryptedBytes", joinerNodeName, encryptNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 48
 testRunner.And(string.Format("Node {0} input pin Part3 connected to node {1} output pin RandomBytes", joinerNodeName, saltGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 49
 testRunner.And(string.Format("Node {0} input pin InputData connected to node {1} output pin JoinedOutput", fileWriterNodeName, joinerNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 50
 testRunner.And(string.Format("All nodes added to a new graph with a start node named {0}", saltGeneratorNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 51
 testRunner.When("Execute graph", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 52
 testRunner.Then(string.Format("Output file {0} created of 48 bytes in length", fileName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 53
 testRunner.And("All nodes executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 54
 testRunner.And("All all nodes executed in correct order of saltgenerator,ivgenerator,derive,encod" +
                        "er,encrypt,joiner,writer", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="03) Decrypt file using a password to correct plain text")]
        [Xunit.TraitAttribute("FeatureTitle", "Graph")]
        [Xunit.TraitAttribute("Description", "03) Decrypt file using a password to correct plain text")]
        [Xunit.InlineDataAttribute("reader", "Output/test.dat", "parser", "Iv,0,16;Cipher,16,~16;Salt,~16,~0", "derivekey", "123", "32", "decrypt", "Iv", "Cipher", "Salt", "decode", "Hello World!", new string[0])]
        public virtual void _03DecryptFileUsingAPasswordToCorrectPlainText(string fileReaderNodeName, string fileName, string dataParserNodeName, string parserSections, string deriveKeyNodeName, string password, string keyLength, string decryptNodeName, string ivSectionKey, string cipherSectionKey, string saltSectionKey, string decoderNodeName, string plainText, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("FileReaderNodeName", fileReaderNodeName);
            argumentsOfScenario.Add("FileName", fileName);
            argumentsOfScenario.Add("DataParserNodeName", dataParserNodeName);
            argumentsOfScenario.Add("ParserSections", parserSections);
            argumentsOfScenario.Add("DeriveKeyNodeName", deriveKeyNodeName);
            argumentsOfScenario.Add("Password", password);
            argumentsOfScenario.Add("KeyLength", keyLength);
            argumentsOfScenario.Add("DecryptNodeName", decryptNodeName);
            argumentsOfScenario.Add("IvSectionKey", ivSectionKey);
            argumentsOfScenario.Add("CipherSectionKey", cipherSectionKey);
            argumentsOfScenario.Add("SaltSectionKey", saltSectionKey);
            argumentsOfScenario.Add("DecoderNodeName", decoderNodeName);
            argumentsOfScenario.Add("PlainText", plainText);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("03) Decrypt file using a password to correct plain text", null, tagsOfScenario, argumentsOfScenario);
#line 61
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 62
 testRunner.Given("A new dictionary of nodes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 63
 testRunner.And(string.Format("FileReaderNode named {0} with a filename of {1} and NextKey of {2}", fileReaderNodeName, fileName, dataParserNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 64
 testRunner.And(string.Format("DataParserNode named {0} with parser sections of {1} and NextKey of {2}", dataParserNodeName, parserSections, deriveKeyNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 65
 testRunner.And(string.Format("DeriveKeyFromPasswordNode named {0} with a password of {1} and key length of {2} " +
                            "and NextKey of {3}", deriveKeyNodeName, password, keyLength, decryptNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 66
 testRunner.And(string.Format("DecryptNode named {0} and NextKey of {1}", decryptNodeName, decoderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 67
 testRunner.And(string.Format("Utf8DecoderNode named {0}", decoderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 68
 testRunner.And(string.Format("Node {0} input pin Bytes connected to node {1} output pin Bytes", dataParserNodeName, fileReaderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 69
 testRunner.And(string.Format("Node {0} input pin {1} connected to DataParserNode {2} section {1} value", deriveKeyNodeName, saltSectionKey, dataParserNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 70
 testRunner.And(string.Format("Node {0} input pin {1} connected to DataParserNode {2} section {1} value", decryptNodeName, ivSectionKey, dataParserNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 71
 testRunner.And(string.Format("Node {0} input pin {1} connected to DataParserNode {2} section {1} value", decryptNodeName, cipherSectionKey, dataParserNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 72
 testRunner.And(string.Format("Node {0} input pin Key connected to node {1} output pin Key", decryptNodeName, deriveKeyNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 73
 testRunner.And(string.Format("Node {0} input pin EncodedBytes connected to node {1} output pin DecryptedBytes", decoderNodeName, decryptNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 74
 testRunner.And(string.Format("All nodes added to a new graph with a start node named {0}", fileReaderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 75
 testRunner.When("Execute graph", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 76
 testRunner.Then(string.Format("Node {0} output pin PlainText equals string {1}", decoderNodeName, plainText), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 77
 testRunner.And("All nodes executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 78
 testRunner.And("All all nodes executed in correct order of reader,parser,derivekey,decrypt,decode" +
                        "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [Xunit.SkippableTheoryAttribute(DisplayName="04) Decrypt file using a password with scrypt to correct plain text")]
        [Xunit.TraitAttribute("FeatureTitle", "Graph")]
        [Xunit.TraitAttribute("Description", "04) Decrypt file using a password with scrypt to correct plain text")]
        [Xunit.InlineDataAttribute("reader", "Output/test2.dat", "parser", "Iv,0,16;Cipher,16,~16;Salt,~16,~0", "derivekey", "123", "32", "decrypt", "Iv", "Cipher", "Salt", "decode", "Hello World!", new string[0])]
        public virtual void _04DecryptFileUsingAPasswordWithScryptToCorrectPlainText(string fileReaderNodeName, string fileName, string dataParserNodeName, string parserSections, string deriveKeyNodeName, string password, string keyLength, string decryptNodeName, string ivSectionKey, string cipherSectionKey, string saltSectionKey, string decoderNodeName, string plainText, string[] exampleTags)
        {
            string[] tagsOfScenario = exampleTags;
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            argumentsOfScenario.Add("FileReaderNodeName", fileReaderNodeName);
            argumentsOfScenario.Add("FileName", fileName);
            argumentsOfScenario.Add("DataParserNodeName", dataParserNodeName);
            argumentsOfScenario.Add("ParserSections", parserSections);
            argumentsOfScenario.Add("DeriveKeyNodeName", deriveKeyNodeName);
            argumentsOfScenario.Add("Password", password);
            argumentsOfScenario.Add("KeyLength", keyLength);
            argumentsOfScenario.Add("DecryptNodeName", decryptNodeName);
            argumentsOfScenario.Add("IvSectionKey", ivSectionKey);
            argumentsOfScenario.Add("CipherSectionKey", cipherSectionKey);
            argumentsOfScenario.Add("SaltSectionKey", saltSectionKey);
            argumentsOfScenario.Add("DecoderNodeName", decoderNodeName);
            argumentsOfScenario.Add("PlainText", plainText);
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("04) Decrypt file using a password with scrypt to correct plain text", null, tagsOfScenario, argumentsOfScenario);
#line 85
this.ScenarioInitialize(scenarioInfo);
#line hidden
            bool isScenarioIgnored = default(bool);
            bool isFeatureIgnored = default(bool);
            if ((tagsOfScenario != null))
            {
                isScenarioIgnored = tagsOfScenario.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((this._featureTags != null))
            {
                isFeatureIgnored = this._featureTags.Where(__entry => __entry != null).Where(__entry => String.Equals(__entry, "ignore", StringComparison.CurrentCultureIgnoreCase)).Any();
            }
            if ((isScenarioIgnored || isFeatureIgnored))
            {
                testRunner.SkipScenario();
            }
            else
            {
                this.ScenarioStart();
#line 86
 testRunner.Given("A new dictionary of nodes", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line hidden
#line 87
 testRunner.And(string.Format("FileReaderNode named {0} with a filename of {1} and NextKey of {2}", fileReaderNodeName, fileName, dataParserNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 88
 testRunner.And(string.Format("DataParserNode named {0} with parser sections of {1} and NextKey of {2}", dataParserNodeName, parserSections, deriveKeyNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 89
 testRunner.And(string.Format("SCryptNode named {0} with a password of {1} and NextKey of {2}", deriveKeyNodeName, password, decryptNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 90
 testRunner.And(string.Format("DecryptNode named {0} and NextKey of {1}", decryptNodeName, decoderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 91
 testRunner.And(string.Format("Utf8DecoderNode named {0}", decoderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 92
 testRunner.And(string.Format("Node {0} input pin Bytes connected to node {1} output pin Bytes", dataParserNodeName, fileReaderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 93
 testRunner.And(string.Format("Node {0} input pin Salt connected to DataParserNode {1} section {2} value", deriveKeyNodeName, dataParserNodeName, saltSectionKey), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 94
 testRunner.And(string.Format("Node {0} input pin {1} connected to DataParserNode {2} section {1} value", decryptNodeName, ivSectionKey, dataParserNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 95
 testRunner.And(string.Format("Node {0} input pin {1} connected to DataParserNode {2} section {1} value", decryptNodeName, cipherSectionKey, dataParserNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 96
 testRunner.And(string.Format("Node {0} input pin Key connected to node {1} output pin Key", decryptNodeName, deriveKeyNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 97
 testRunner.And(string.Format("Node {0} input pin EncodedBytes connected to node {1} output pin DecryptedBytes", decoderNodeName, decryptNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 98
 testRunner.And(string.Format("All nodes added to a new graph with a start node named {0}", fileReaderNodeName), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 99
 testRunner.When("Execute graph", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line hidden
#line 100
 testRunner.Then(string.Format("Node {0} output pin PlainText equals string {1}", decoderNodeName, plainText), ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
#line 101
 testRunner.And("All nodes executed", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
#line 102
 testRunner.And("All all nodes executed in correct order of reader,parser,derivekey,decrypt,decode" +
                        "", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            }
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "3.4.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                GraphFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                GraphFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion
