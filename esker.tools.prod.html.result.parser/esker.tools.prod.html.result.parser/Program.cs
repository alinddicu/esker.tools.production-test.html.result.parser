namespace esker.tools.prod.html.result.parser
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Net;
	using HtmlAgilityPack;

	public class Program
	{
		private const string ReferenceFile = @"C:\Users\DicuA\Documents\EOD\Stories\FT-010905\DateFormat\T-1680020001\AttachFiles\Reference\EOD\AttachFile1.htm";
		private const string CourantFile = @"C:\Users\DicuA\Documents\EOD\Stories\FT-010905\DateFormat\T-1680020001\AttachFiles\Courant\EOD\AttachFile1.htm";
		private const string DiffsFile = @"C:\Users\DicuA\Documents\EOD\Stories\FT-010905\DateFormat\T-1680020001\AttachFiles\Diffs\EOD\AttachFile1.htm";

		public static void Main(string[] args)
		{
			var referenceHtmlDoc = GetHtmlDoc(ReferenceFile);
			var referenceDescendants = referenceHtmlDoc.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th")).ToArray();
			var referenceDescendantsCount = referenceDescendants.Length;

			var courantHtmlDoc = GetHtmlDoc(CourantFile);
			var courantDescendants = courantHtmlDoc.DocumentNode.Descendants().Where(d => d.Name == "tr" && !d.InnerHtml.Contains("th")).ToArray();
			var courantDescendantsCount = courantDescendants.Length;

			if (referenceDescendantsCount != courantDescendantsCount)
			{
				throw new ApplicationException("Nombre de lignes différents");
			}

			for (var i = 0; i < referenceDescendantsCount; i++)
			{
				var referenceLine = referenceDescendants[i];
				var courantLine = courantDescendants[i];

				var referenceColumns = referenceLine.Descendants().Where(d => d.Name == "td").ToArray();
				var courantColumns = courantLine.Descendants().Where(d => d.Name == "td").ToArray();

				if (referenceColumns.Length != courantColumns.Length)
				{
					throw new ApplicationException("Nombre de cellules différentes sur la ligne " + i);
				}

				for (var j = 0; j < referenceColumns.Length; j++)
				{
					var referenceColum = referenceColumns[j];
					var courantColum = courantColumns[j];
					if (referenceColum.InnerHtml != courantColum.InnerHtml)
					{
						courantColum.SetAttributeValue("style", "color:red");
						courantColum.SetAttributeValue("style", "background-color:yellow");
					}
				}
			}

			courantHtmlDoc.Save(DiffsFile);
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
