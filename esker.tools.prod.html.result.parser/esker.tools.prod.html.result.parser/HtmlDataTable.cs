namespace esker.tools.prod.html.result.parser
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using HtmlAgilityPack;

	public class HtmlDataTable
	{
		private readonly HtmlDocument _htmlDocument = new HtmlDocument();

		private readonly IEnumerable<string> _referenceFormats;

		public HtmlDataTable (string filePath)
		{
			var content = File.ReadAllText(filePath);
			content = WebUtility.HtmlDecode(content);
			_htmlDocument.LoadHtml(content);

			Rows = InitRows(_htmlDocument).Select(r => new HtmlDataRow(r)).ToList();
			_referenceFormats = Rows.Select(r => r.Cells.First().InnerText);
		}

		public IReadOnlyList<HtmlDataRow> Rows { get; }

		private static IEnumerable<HtmlNode> InitRows(HtmlDocument htmlDocument)
		{
			return htmlDocument.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th"));
		}

		public void Save(string filePath)
		{
			_htmlDocument.Save(filePath);
		}

		public bool IsAdditionalRow(HtmlDataRow currentRow)
		{
			return !_referenceFormats.Contains(currentRow.Cells.First().InnerText);
		}

		public HtmlDataRow GetRowByDateFormat(string dateFormat)
		{
			return Rows.First(r => r.DateFormat == dateFormat);
		}
	}
}
