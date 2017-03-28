namespace esker.tools.prod.html.result.parser
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Net;
	using HtmlAgilityPack;

	public class Program
	{
		private const string RootDirectory = @"C:\Users\DicuA\Documents\EOD\Stories\FT-010905\DateFormat\";
		private const string ReferenceFileTemplate = RootDirectory + @"{0}\AttachFiles\Reference\EOD\AttachFile1.htm";
		private const string CourantFileTemplate = RootDirectory + @"{0}\AttachFiles\Courant\EOD\AttachFile1.htm";
		private const string DiffsFileTemplate = RootDirectory + @"{0}\AttachFiles\Diffs\EOD\AttachFile1.htm";

		public static void Main(string[] args)
		{
			var testNames = new[] { "T-1680020001", "T-1680020002", "T-1680020003", "T-1680020004", "T-1680020005", "T-1680020006" };
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

			DoForFiles(referenceFile, courantFile, diffsFile);
		}

		private static string GetFileName(string testName, string fileTemplate)
		{
			return string.Format(CultureInfo.InvariantCulture, fileTemplate, testName);
		}

		private static void DoForFiles(string referenceFile, string courantFile, string diffsFile)
		{
			var referenceDataColumns = GetDataColumns(referenceFile).ToArray();
			var courantHtmlDoc = GetHtmlDoc(courantFile);
			var courantDataColumns = GetDataColumns(courantHtmlDoc).ToArray();

			CheckDataLines(referenceDataColumns, courantDataColumns);

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

		private static IEnumerable<HtmlNode> GetDataColumns(string file)
		{
			return GetDataColumns(GetHtmlDoc(file));
		}

		private static IEnumerable<HtmlNode> GetDataColumns(HtmlDocument htmlDocument)
		{
			return htmlDocument.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th"));
		}

		private static void CheckDataLines(
			IReadOnlyList<HtmlNode> referenceDataLines,
			IReadOnlyList<HtmlNode> courantDataLines)
		{
			for (var i = 0; i < referenceDataLines.Count; i++)
			{
				var referenceCells = GetDataCells(referenceDataLines[i]).ToArray();
				var courantCells = GetDataCells(courantDataLines[i]).ToArray();
				if (referenceCells.Length != courantCells.Length)
				{
					throw new ApplicationException("Nombre de cellules différentes sur la ligne " + i);
				}

				CheckDataCells(referenceCells, courantCells);
			}
		}

		private static IEnumerable<HtmlNode> GetDataCells(HtmlNode dataLine)
		{
			return dataLine.Descendants().Where(d => d.Name == "td");
		}

		private static void CheckDataCells(
			IReadOnlyList<HtmlNode> referenceLineCells,
			IReadOnlyList<HtmlNode> courantLineCells)
		{
			for (var j = 0; j < referenceLineCells.Count; j++)
			{
				var referenceCell = referenceLineCells[j];
				var courantCell = courantLineCells[j];
				if (referenceCell.InnerHtml != courantCell.InnerHtml)
				{
					courantCell.SetAttributeValue("style", "background-color:orange");
				}
			}
		}
	}
}
