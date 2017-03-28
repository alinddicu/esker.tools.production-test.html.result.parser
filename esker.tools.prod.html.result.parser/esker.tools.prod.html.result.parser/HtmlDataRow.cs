namespace esker.tools.prod.html.result.parser
{
	using System.Collections.Generic;
	using System.Linq;
	using HtmlAgilityPack;

	public class HtmlDataRow
	{
		public HtmlDataRow(HtmlNode dataRow)
		{
			DataCells = FillDataCells(dataRow).ToList();
			DateFormat = DataCells.First().InnerText;
		}

		public string DateFormat { get; }

		public IReadOnlyList<HtmlDataCell> DataCells { get; }

		private static IEnumerable<HtmlDataCell> FillDataCells(HtmlNode dataRow)
		{
			return dataRow.Descendants().Where(d => d.Name == "td").Select(d => new HtmlDataCell(d));
		}
	}
}
