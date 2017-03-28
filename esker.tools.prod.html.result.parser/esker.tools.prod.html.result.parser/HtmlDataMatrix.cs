namespace esker.tools.prod.html.result.parser
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using HtmlAgilityPack;

	public class HtmlDataMatrix
	{
		private readonly HtmlDocument _htmlDocument = new HtmlDocument();

		private readonly IEnumerable<string> _referenceFormats;

		public HtmlDataMatrix (string filePath)
		{
			var content = File.ReadAllText(filePath);
			content = WebUtility.HtmlDecode(content);
			_htmlDocument.LoadHtml(content);

			DataRows = GetDataRows(_htmlDocument).Select(r => new HtmlDataRow(r)).ToList();
			_referenceFormats = DataRows.Select(r => r.DataCells.First().InnerText);
		}

		public IReadOnlyList<HtmlDataRow> DataRows { get; }

		private static IEnumerable<HtmlNode> GetDataRows(HtmlDocument htmlDocument)
		{
			return htmlDocument.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th"));
		}

		public void Save(string filePath)
		{
			_htmlDocument.Save(filePath);
		}

		public bool IsAdditionalDataRow(HtmlDataRow currentDataRow)
		{
			return !_referenceFormats.Contains(currentDataRow.DataCells.First().InnerText);
		}

		public HtmlDataRow GetDataRowByDateFormat(string dateFormat)
		{
			return DataRows.First(r => r.DateFormat == dateFormat);
		}
	}
}
