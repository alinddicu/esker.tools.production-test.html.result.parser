namespace esker.tools.prod.html.result.parser
{
	public class DateFormatProductionHtmlDiffEnhancer
	{
		public void Execute(string referenceFile, string courantFile, string diffsFile)
		{
			var referenceMatrix = new HtmlDataMatrix(referenceFile);
			var currentMatrix = new HtmlDataMatrix(courantFile);

			CheckMatrixes(referenceMatrix, currentMatrix);

			currentMatrix.Save(diffsFile);
		}

		private static void CheckMatrixes(
			HtmlDataMatrix referenceMatrix,
			HtmlDataMatrix currentMatrix)
		{
			foreach (var currentDataRow in currentMatrix.Rows)
			{
				if (referenceMatrix.IsAdditionalRow(currentDataRow))
				{
					CheckCellsInRows(null, currentDataRow);
					continue;
				}

				var currentDateFormat = currentDataRow.DateFormat;
				var referenceDataRow = referenceMatrix.GetRowByDateFormat(currentDateFormat);
				CheckCellsInRows(referenceDataRow, currentDataRow);
			}
		}

		private static void CheckCellsInRows(
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
