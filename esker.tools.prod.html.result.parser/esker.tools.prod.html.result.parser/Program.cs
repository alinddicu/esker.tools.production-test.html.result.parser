namespace esker.tools.prod.html.result.parser
{
	using System.Globalization;

	public class Program
	{
		private const string RootDirectory = @"C:\Users\DicuA\Documents\EOD\Stories\FT-010905\DateFormat\";
		private const string ReferenceFileTemplate = RootDirectory+ @"{0}\AttachFiles\Reference\EOD\AttachFile1.htm";
		private const string CourantFileTemplate = RootDirectory + @"{0}\AttachFiles\Courant\EOD\AttachFile1.htm";
		private const string DiffsFileTemplate = RootDirectory + @"{0}\AttachFiles\Diffs\EOD\AttachFile1.htm";

		private static readonly DateFormatProductionHtmlDiffEnhancer Enhancer = new DateFormatProductionHtmlDiffEnhancer();

		public static void Main(string[] args)
		{
			var testNames = new [] { "T-1680020001", "T-1680020002", "T-1680020003", "T-1680020004", "T-1680020005", "T-1680020006" };
			foreach (var testName in testNames)
			{
				DoForTest(testName);
			}
		}

		private static void DoForTest(string testName)
		{
			var referenceFile = GetFileName(testName, ReferenceFileTemplate);
			var courantFile = GetFileName(testName, CourantFileTemplate);
			var diffsFile = GetFileName(testName, DiffsFileTemplate);

			Enhancer.Execute(referenceFile, courantFile, diffsFile);
		}

		private static string GetFileName(string testName, string fileTemplate)
		{
			return string.Format(CultureInfo.InvariantCulture, fileTemplate, testName);
		}
	}
}
