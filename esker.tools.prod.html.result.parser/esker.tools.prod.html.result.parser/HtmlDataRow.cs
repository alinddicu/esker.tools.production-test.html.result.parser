namespace esker.tools.prod.html.result.parser
{
	using System.Collections.Generic;
	using System.Linq;
	using HtmlAgilityPack;

	public class HtmlDataRow
	{
		public HtmlDataRow(HtmlNode dataRow)
		{
			Cells = InitCells(dataRow).ToList();
			DateFormat = Cells.First().InnerText;
		}

		public string DateFormat { get; }

		public IReadOnlyList<HtmlDataCell> Cells { get; }

		private static IEnumerable<HtmlDataCell> InitCells(HtmlNode parentNode)
		{
			return parentNode.Descendants().Where(d => d.Name == "td").Select(d => new HtmlDataCell(d));
		}
	}
}
