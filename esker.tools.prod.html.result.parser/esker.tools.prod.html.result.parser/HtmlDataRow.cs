namespace esker.tools.prod.html.result.parser
{
	using System.Collections.Generic;
	using System.Linq;
	using HtmlAgilityPack;

	public class HtmlDataRow
	{
		public HtmlDataRow(HtmlNode dataRow)
		{
			DataCells = dataRow.Descendants().Where(d => d.Name == "td").ToList();
			DateFormat = DataCells.First().InnerText.Trim();
		}
		public string DateFormat { get; }

		public IReadOnlyList<HtmlNode> DataCells { get; }
	}
}
