namespace esker.tools.prod.html.result.parser
{
	using HtmlAgilityPack;

	public class HtmlDataCell
	{
		private readonly int _indexInRow;

		private readonly HtmlNode _htmlNode;

		public HtmlDataCell(int indexInRow, HtmlNode htmlNode)
		{
			_indexInRow = indexInRow;
			_htmlNode = htmlNode;
		}

		public string InnerText => _htmlNode.InnerText;

		public bool HasDifferentText(HtmlDataRow referenceRow)
		{
			var otherCell = referenceRow?.Cells[_indexInRow];
			return InnerText != otherCell?.InnerText;
		}

		public void MarkDifferent()
		{
			_htmlNode.SetAttributeValue("style", "background-color:orange");
		}
	}
}
