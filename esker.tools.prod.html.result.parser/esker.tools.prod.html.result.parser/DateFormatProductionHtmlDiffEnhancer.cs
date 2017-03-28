namespace esker.tools.prod.html.result.parser
{
	public class DateFormatProductionHtmlDiffEnhancer
	{
		public void Execute(string referenceFile, string courantFile, string diffsFile)
		{
			var referenceMatrix = new HtmlDataMatrix(referenceFile);
			var currentMatrix = new HtmlDataMatrix(courantFile);

			CheckDataRows(referenceMatrix, currentMatrix);

			currentMatrix.Save(diffsFile);
		}

		private static void CheckDataRows(
			HtmlDataMatrix referenceMatrix,
			HtmlDataMatrix currentMatrix)
		{
			for (var currentLineIndex = 0; currentLineIndex < currentMatrix.DataRows.Count; currentLineIndex++)
			{
				var currentDataRow = currentMatrix.DataRows[currentLineIndex];
				if (referenceMatrix.IsAdditionalDataRow(currentDataRow))
				{
					CheckDataCells(null, currentDataRow);
					continue;
				}

				var currentDateFormat = currentDataRow.DateFormat;
				var referenceDataRow = referenceMatrix.GetDataRowByDateFormat(currentDateFormat);
				CheckDataCells(referenceDataRow, currentDataRow);
			}
		}

		private static void CheckDataCells(
			HtmlDataRow referenceDataRow,
			HtmlDataRow currentDataRow)
		{
			for (var currentCellIndex = 0; currentCellIndex < currentDataRow.DataCells.Count; currentCellIndex++)
			{
				var currentCell = currentDataRow.DataCells[currentCellIndex];
				if (referenceDataRow == null 
					|| currentCell.IsDifferentText(referenceDataRow.DataCells[currentCellIndex]))
				{
					currentCell.MarkDifferent();
				}
			}
		}
	}
}
