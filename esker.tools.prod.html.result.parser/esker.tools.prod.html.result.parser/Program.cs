namespace esker.tools.prod.html.result.parser
{
	using System;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Net;
	using HtmlAgilityPack;

	public class Program
	{
		private const string RootDirectory = @"C:\Users\DicuA\Documents\EOD\Stories\FT-010905\DateFormat\";
		private const string ReferenceFileTemplate = RootDirectory+ @"{0}\AttachFiles\Reference\EOD\AttachFile1.htm";
		private const string CourantFileTemplate = RootDirectory + @"{0}\AttachFiles\Courant\EOD\AttachFile1.htm";
		private const string DiffsFileTemplate = RootDirectory + @"{0}\AttachFiles\Diffs\EOD\AttachFile1.htm";
		
		public static void Main(string[] args)
		{
			var testNames = new [] { "T-1680020001", "T-1680020002", "T-1680020003", "T-1680020004", "T-1680020005", "T-1680020006" };
			foreach (var testName in testNames)
			{
				DoForTest(testName);
			}
		}

		private static string GetFileName(string testName, string fileTemplate)
		{
			return string.Format(CultureInfo.InvariantCulture, fileTemplate, testName);
		}

		private static void DoForTest(string testName)
		{
			var referenceFile = GetFileName(testName, ReferenceFileTemplate);
			var courantFile = GetFileName(testName, CourantFileTemplate);
			var diffsFile = GetFileName(testName, DiffsFileTemplate);

			DoForFiles(referenceFile, courantFile, diffsFile);
		}

		private static void DoForFiles(string referenceFile, string courantFile, string diffsFile)
		{
			var referenceHtmlDoc = GetHtmlDoc(referenceFile);
			var referenceDescendants = referenceHtmlDoc.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th")).ToArray();
			var referenceDescendantsCount = referenceDescendants.Length;

			var courantHtmlDoc = GetHtmlDoc(courantFile);
			var courantDescendants = courantHtmlDoc.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th")).ToArray();
			var courantDescendantsCount = courantDescendants.Length;

			if (referenceDescendantsCount != courantDescendantsCount)
			{
				throw new ApplicationException("Nombre de lignes différents");
			}

			for (var i = 0; i < referenceDescendantsCount; i++)
			{
				var referenceLine = referenceDescendants[i];
				var courantLine = courantDescendants[i];

				var referenceColumns = referenceLine.Descendants().Where(d => d.Name == "td").ToArray();
				var courantColumns = courantLine.Descendants().Where(d => d.Name == "td").ToArray();

				if (referenceColumns.Length != courantColumns.Length)
				{
					throw new ApplicationException("Nombre de cellules différentes sur la ligne " + i);
				}

				for (var j = 0; j < referenceColumns.Length; j++)
				{
					var referenceColum = referenceColumns[j];
					var courantColum = courantColumns[j];
					if (referenceColum.InnerHtml != courantColum.InnerHtml)
					{
						courantColum.SetAttributeValue("style", "background-color:yellow");
					}
				}
			}

			courantHtmlDoc.Save(diffsFile);
		}

		private static HtmlDocument GetHtmlDoc(string filePath)
		{
			var referenceContent = File.ReadAllText(filePath);
			referenceContent = WebUtility.HtmlDecode(referenceContent);
			var referenceHtmlDoc = new HtmlDocument();
			referenceHtmlDoc.LoadHtml(referenceContent);
			return referenceHtmlDoc;
		}
	}
}
