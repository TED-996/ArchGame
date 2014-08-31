using ArchGame.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestingExtensions
{
    
    
    /// <summary>
    ///This is a test class for MiscExtensionsTest and is intended
    ///to contain all MiscExtensionsTest Unit Tests
    ///</summary>
	[TestClass()]
	public class MiscExtensionsTest {


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext {
			get {
				return testContextInstance;
			}
			set {
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Multiply
		///</summary>
		[TestMethod()]
		public void MultiplyTest() {
			Assert.AreEqual("123".Multiply(1), "123");
			Assert.AreEqual("123".Multiply(2), "123123");
			Assert.AreEqual("123".Multiply(3), "123123123");
			Assert.AreEqual("123".Multiply(4), "123123123123");
			Assert.AreEqual("123".Multiply(5), "123123123123123");
			return;
			Assert.Inconclusive("Verify the correctness of this test method.");
		}
	}
}
