namespace esker.tools.prod.html.result.parser
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using HtmlAgilityPack;

	public class DateFormatProductionHtmlDiffEnhancer
	{
		public void Execute(string referenceFile, string courantFile, string diffsFile)
		{
			var referenceDataRows = GetDataRows(referenceFile).ToArray();
			var currentHtmlDoc = GetHtmlDoc(courantFile);
			var currentDataRows = GetDataRows(currentHtmlDoc).ToArray();

			CheckDataRows(referenceDataRows, currentDataRows);

			currentHtmlDoc.Save(diffsFile);
		}

		private static HtmlDocument GetHtmlDoc(string filePath)
		{
			var content = File.ReadAllText(filePath);
			content = WebUtility.HtmlDecode(content);
			var referenceHtmlDoc = new HtmlDocument();
			referenceHtmlDoc.LoadHtml(content);
			return referenceHtmlDoc;
		}

		private static IEnumerable<HtmlNode> GetDataRows(string file)
		{
			return GetDataRows(GetHtmlDoc(file));
		}

		private static IEnumerable<HtmlNode> GetDataRows(HtmlDocument htmlDocument)
		{
			return htmlDocument.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th"));
		}

		private static void CheckDataRows(
			IReadOnlyList<HtmlNode> referenceDataRows,
			IReadOnlyList<HtmlNode> currentDataRows)
		{
			for (var currentLineIndex = 0; currentLineIndex < currentDataRows.Count; currentLineIndex++)
			{
				var currentCells = GetDataCells(currentDataRows[currentLineIndex]).ToArray();
				if (IsAdditionalDataRow(referenceDataRows, currentCells))
				{
					CheckDataCells(new HtmlNode[0], currentCells, true);
					continue;
				}

				var currentRowCulture = currentCells.First().InnerText;
				var referenceCells = GetReferenceCellsByCurrentRowCulture(referenceDataRows, currentRowCulture).ToArray();
				CheckDataCells(referenceCells, currentCells, false);
			}
		}

		private static IEnumerable<HtmlNode> GetReferenceCellsByCurrentRowCulture(
			IEnumerable<HtmlNode> referenceDataRows,
			string currentRowCulture)
		{
			return GetDataCells(referenceDataRows.First(r => GetDataCells(r).First().InnerText == currentRowCulture));
		}

		private static bool IsAdditionalDataRow(
			IEnumerable<HtmlNode> referenceDataRows,
			IEnumerable<HtmlNode> currentRowCells)
		{
			var referenceFormats = referenceDataRows.Select(r => GetDataCells(r).First().InnerText);
			return !referenceFormats.Contains(currentRowCells.First().InnerText);
		}

		private static IEnumerable<HtmlNode> GetDataCells(HtmlNode dataRow)
		{
			return dataRow.Descendants().Where(d => d.Name == "td");
		}

		private static void CheckDataCells(
			IReadOnlyList<HtmlNode> referenceLineCells,
			IReadOnlyList<HtmlNode> currentLineCells,
			bool isAdditionalDataRow)
		{
			for (var currantCellIndex = 0; currantCellIndex < currentLineCells.Count; currantCellIndex++)
			{
				var currentCell = currentLineCells[currantCellIndex];
				if (isAdditionalDataRow || IsCellContentDifferent(referenceLineCells, currentCell, currantCellIndex))
				{
					MarkCellDifference(currentCell);
				}
			}
		}

		private static bool IsCellContentDifferent(
			IReadOnlyList<HtmlNode> referenceLineCells,
			HtmlNode currentCell, 
			int currentCellIndex)
		{
			return currentCell.InnerHtml != referenceLineCells[currentCellIndex].InnerHtml;
		}

		private static void MarkCellDifference(HtmlNode currentCell)
		{
			currentCell.SetAttributeValue("style", "background-color:orange");
		}
	}
}
