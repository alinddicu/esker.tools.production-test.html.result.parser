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
			var referenceDataColumns = referenceHtmlDoc.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th")).ToArray();
			var referenceColumnsCount = referenceDataColumns.Length;

			var courantHtmlDoc = GetHtmlDoc(courantFile);
			var courantDataColumns = courantHtmlDoc.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th")).ToArray();
			var courantColumnsCount = courantDataColumns.Length;

			if (referenceColumnsCount != courantColumnsCount)
			{
				throw new ApplicationException("Nombre de lignes différents");
			}

			CheckDataColumns(referenceDataColumns, courantDataColumns);

			courantHtmlDoc.Save(diffsFile);
		}

		private static void CheckDataColumns(
			IReadOnlyList<HtmlNode> referenceDataColumns,
			IReadOnlyList<HtmlNode> courantDataColumns)
		{
			for (var i = 0; i < referenceDataColumns.Count; i++)
			{
				var referenceLine = referenceDataColumns[i];
				var courantLine = courantDataColumns[i];

				var referenceCells = referenceLine.Descendants().Where(d => d.Name == "td").ToArray();
				var courantCells = courantLine.Descendants().Where(d => d.Name == "td").ToArray();

				if (referenceCells.Length != courantCells.Length)
				{
					throw new ApplicationException("Nombre de cellules différentes sur la ligne " + i);
				}

				CheckDataCells(referenceCells, courantCells);
			}
		}

		private static void CheckDataCells(IReadOnlyList<HtmlNode> referenceCells, IReadOnlyList<HtmlNode> courantCells)
		{
			for (var j = 0; j < referenceCells.Count; j++)
			{
				var referenceCell = referenceCells[j];
				var courantCell = courantCells[j];
				if (referenceCell.InnerHtml != courantCell.InnerHtml)
				{
					courantCell.SetAttributeValue("style", "background-color:yellow");
				}
			}
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
