namespace esker.tools.prod.html.result.parser.test
{
	using System.IO;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using NFluent;

	[TestClass]
	public class DateFormatProductionHtmlDiffEnhancerNonRegressionTest
	{
		[TestMethod]
		[DeploymentItem("NominalCase", "NominalCase")]
		public void NominalCase()
		{
			TestCase("NominalCase");
		}

		[TestMethod]
		[DeploymentItem("AdditionalFormatsAddedAtBottom", "AdditionalFormatsAddedAtBottom")]
		public void AdditionalFormatsAddedAtBottom()
		{
			TestCase("AdditionalFormatsAddedAtBottom");
		}

		[TestMethod]
		[DeploymentItem("AdditionalFormatsAddedInTheMiddle", "AdditionalFormatsAddedInTheMiddle")]
		public void AdditionalFormatsAddedInTheMiddle()
		{
			TestCase("AdditionalFormatsAddedInTheMiddle");
		}

		private static void TestCase(string testName)
		{
			var courantPath = testName + "/courant.htm";
			var diffsPath = testName + "/diffs.htm";
			var expectedDiffsPath = testName + "/expected-diffs.htm";
			var referencePath = testName + "/reference.htm";

			new DateFormatProductionHtmlDiffEnhancer2().Execute(referencePath, courantPath, diffsPath);

			var content = File.ReadAllText(diffsPath);
			var expectedContent = File.ReadAllText(expectedDiffsPath);

			Check.That(content).IsEqualTo(expectedContent);
		}
	}
}
