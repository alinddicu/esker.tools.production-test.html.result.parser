namespace esker.tools.prod.html.result.parser
{
	using HtmlAgilityPack;

	public class HtmlDataCell
	{
		private readonly HtmlNode _htmlNode;

		public HtmlDataCell(HtmlNode htmlNode)
		{
			_htmlNode = htmlNode;
		}

		public string InnerText => _htmlNode.InnerText;

		public bool IsDifferentText(HtmlDataCell otherCell)
		{
			return InnerText != otherCell.InnerText;
		}

		public void MarkDifferent()
		{
			_htmlNode.SetAttributeValue("style", "background-color:orange");
		}
	}
}
