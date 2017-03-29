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

		private static void DiffTables(
			HtmlDataTable referenceTable,
			HtmlDataTable currentTable)
		{
			foreach (var currentDataRow in currentTable.Rows)
			{
				if (referenceTable.IsNewDateFormat(currentDataRow))
				{
					DiffCellsInRows(null, currentDataRow);
					continue;
				}

				var currentDateFormat = currentDataRow.DateFormat;
				var referenceDataRow = referenceTable.GetRowByDateFormat(currentDateFormat);
				DiffCellsInRows(referenceDataRow, currentDataRow);
			}
		}

		private static void DiffCellsInRows(
			HtmlDataRow referenceRow,
			HtmlDataRow currentRow)
		{
			for (var currentCellIndex = 0; currentCellIndex < currentRow.Cells.Count; currentCellIndex++)
			{
				var currentCell = currentRow.Cells[currentCellIndex];
				if (referenceRow == null 
					|| currentCell.IsDifferentText(referenceRow.Cells[currentCellIndex]))
				{
					currentCell.MarkDifferent();
				}
			}
		}
	}
}
