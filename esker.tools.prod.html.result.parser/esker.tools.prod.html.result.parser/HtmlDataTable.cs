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

		public HtmlDataTable (string filePath)
		{
			var content = File.ReadAllText(filePath);
			content = WebUtility.HtmlDecode(content);
			_htmlDocument.LoadHtml(content);

			Rows = InitRows(_htmlDocument).Select(r => new HtmlDataRow(r)).ToList();
		}

		public IReadOnlyList<HtmlDataRow> Rows { get; }

		private static IEnumerable<HtmlNode> InitRows(HtmlDocument htmlDocument)
		{
			return htmlDocument
				.DocumentNode
				.Descendants()
				.Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th"));
		}

		public void Save(string filePath)
		{
			_htmlDocument.Save(filePath);
		}

		public HtmlDataRow GetRowByDateFormat(string dateFormat)
		{
			return Rows.FirstOrDefault(r => r.DateFormat == dateFormat);
		}
	}
}
