namespace esker.tools.prod.html.result.parser
{
	public class DateFormatProductionHtmlDiffEnhancer
	{
		public void Execute(string referenceFile, string courantFile, string diffsFile)
		{
			var referenceTable = new HtmlDataTable(referenceFile);
			var currentTable = new HtmlDataTable(courantFile);

			DiffTables(referenceTable, currentTable);

			currentTable.Save(diffsFile);
		}

		private static void DiffTables(HtmlDataTable referenceTable, HtmlDataTable currentTable)
		{
			foreach (var currentDataRow in currentTable.Rows)
			{
				var referenceRow = referenceTable.GetRowByDateFormat(currentDataRow.DateFormat);
				DiffCellsInRows(referenceRow, currentDataRow);
			}
		}

		private static void DiffCellsInRows(HtmlDataRow referenceRow, HtmlDataRow currentRow)
		{
			foreach (var currentCell in currentRow.Cells)
			{
				if (currentCell.HasDifferentText(referenceRow))
				{
					currentCell.MarkDifferent();
				}
			}
		}
	}
}
