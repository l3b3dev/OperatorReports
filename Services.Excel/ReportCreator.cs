using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntity;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Services.Excel.Interfaces;
using X14 = DocumentFormat.OpenXml.Office2010.Excel;
using X15 = DocumentFormat.OpenXml.Office2013.Excel;

namespace Services.Excel
{
    /// <summary>
    /// ReportCreator
    /// </summary>
    /// <seealso cref="Services.Excel.Interfaces.IReportCreator" />
    public class ReportCreator : IReportCreator
    {
        /// <summary>
        /// Generates the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void Generate(IEnumerable<OperatorReport> payload, string fileName)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException(nameof(fileName));

            using (var package = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                CreatePartsForExcel(package, payload);
            }
        }

        /// <summary>
        /// Generates the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="stream"></param>
        /// <exception cref="System.ArgumentNullException">payload</exception>
        public void Generate(IEnumerable<OperatorReport> payload, MemoryStream stream)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            using (var package = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                CreatePartsForExcel(package, payload);
            }
        }

        #region Private Methods

        /// <summary>
        /// Creates the parts for excel.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="data">The data.</param>
        private void CreatePartsForExcel(SpreadsheetDocument document, IEnumerable<OperatorReport> data)
        {
            var partSheetData = GenerateSheetDataForDetails(data);

            var workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);

            var workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPartContent(workbookStylesPart1);

            var worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1, partSheetData);
        }

        /// <summary>
        /// Generates the sheet data for details.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        private SheetData GenerateSheetDataForDetails(IEnumerable<OperatorReport> data)
        {
            var sheetData1 = new SheetData();
            sheetData1.Append(CreateHeaderRowForExcel());

            foreach (var report in data)
            {
                var partsRows = GenerateRowForChildPartDetail(report);
                sheetData1.Append(partsRows);
            }
            return sheetData1;
        }

        /// <summary>
        /// Generates the row for child part detail.
        /// </summary>
        /// <param name="report">The report.</param>
        /// <returns></returns>
        private Row GenerateRowForChildPartDetail(OperatorReport report)
        {
            var tRow = new Row();
            tRow.Append(CreateCell(report.Id.ToString()));
            tRow.Append(CreateCell(report.Name));
            tRow.Append(CreateCell(report.ProactiveSent.ToString()));
            tRow.Append(CreateCell(report.ProactiveAnswered.ToString()));
            tRow.Append(CreateCell(report.ProactiveResponseRate.ToString()));
            tRow.Append(CreateCell(report.ReactiveReceived.ToString()));
            tRow.Append(CreateCell(report.ReactiveAnswered.ToString()));
            tRow.Append(CreateCell(report.ReactiveResponseRate.ToString()));
            tRow.Append(CreateCell(report.TotalChatLength??"-"));
            tRow.Append(CreateCell(report.AverageChatLength??"-"));

            return tRow;
        }

        /// <summary>
        /// Generates the content of the workbook part.
        /// </summary>
        /// <param name="workbookPart1">The workbook part1.</param>
        private void GenerateWorkbookPartContent(WorkbookPart workbookPart1)
        {
            var workbook1 = new Workbook();
            var sheets1 = new Sheets();
            var sheet1 = new Sheet() { Name = "Sheet1", SheetId = (UInt32Value)1U, Id = "rId1" };
            sheets1.Append(sheet1);
            workbook1.Append(sheets1);
            workbookPart1.Workbook = workbook1;
        }

        /// <summary>
        /// Generates the content of the workbook styles part.
        /// </summary>
        /// <param name="workbookStylesPart1">The workbook styles part1.</param>
        private void GenerateWorkbookStylesPartContent(WorkbookStylesPart workbookStylesPart1)
        {
            var stylesheet1 = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            stylesheet1.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            stylesheet1.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");

            var fonts1 = new Fonts() { Count = (UInt32Value)2U, KnownFonts = true };

            var font1 = new Font();
            var fontSize1 = new FontSize() { Val = 11D };
            var color1 = new Color() { Theme = (UInt32Value)1U };
            var fontName1 = new FontName() { Val = "Calibri" };
            var fontFamilyNumbering1 = new FontFamilyNumbering() { Val = 2 };
            var fontScheme1 = new FontScheme() { Val = FontSchemeValues.Minor };

            font1.Append(fontSize1);
            font1.Append(color1);
            font1.Append(fontName1);
            font1.Append(fontFamilyNumbering1);
            font1.Append(fontScheme1);

            var font2 = new Font();
            var bold1 = new Bold();
            var fontSize2 = new FontSize() { Val = 11D };
            var color2 = new Color() { Theme = (UInt32Value)1U };
            var fontName2 = new FontName() { Val = "Calibri" };
            var fontFamilyNumbering2 = new FontFamilyNumbering() { Val = 2 };
            var fontScheme2 = new FontScheme() { Val = FontSchemeValues.Minor };

            font2.Append(bold1);
            font2.Append(fontSize2);
            font2.Append(color2);
            font2.Append(fontName2);
            font2.Append(fontFamilyNumbering2);
            font2.Append(fontScheme2);

            fonts1.Append(font1);
            fonts1.Append(font2);

            var fills1 = new Fills() { Count = (UInt32Value)2U };

            var fill1 = new Fill();
            var patternFill1 = new PatternFill() { PatternType = PatternValues.None };

            fill1.Append(patternFill1);

            var fill2 = new Fill();
            var patternFill2 = new PatternFill() { PatternType = PatternValues.Gray125 };

            fill2.Append(patternFill2);

            fills1.Append(fill1);
            fills1.Append(fill2);

            var borders1 = new Borders() { Count = (UInt32Value)2U };

            var border1 = new Border();
            var leftBorder1 = new LeftBorder();
            var rightBorder1 = new RightBorder();
            var topBorder1 = new TopBorder();
            var bottomBorder1 = new BottomBorder();
            var diagonalBorder1 = new DiagonalBorder();

            border1.Append(leftBorder1);
            border1.Append(rightBorder1);
            border1.Append(topBorder1);
            border1.Append(bottomBorder1);
            border1.Append(diagonalBorder1);

            var border2 = new Border();

            var leftBorder2 = new LeftBorder() { Style = BorderStyleValues.Thin };
            var color3 = new Color() { Indexed = (UInt32Value)64U };

            leftBorder2.Append(color3);

            var rightBorder2 = new RightBorder() { Style = BorderStyleValues.Thin };
            var color4 = new Color() { Indexed = (UInt32Value)64U };

            rightBorder2.Append(color4);

            var topBorder2 = new TopBorder() { Style = BorderStyleValues.Thin };
            var color5 = new Color() { Indexed = (UInt32Value)64U };

            topBorder2.Append(color5);

            var bottomBorder2 = new BottomBorder() { Style = BorderStyleValues.Thin };
            var color6 = new Color() { Indexed = (UInt32Value)64U };

            bottomBorder2.Append(color6);
            var diagonalBorder2 = new DiagonalBorder();

            border2.Append(leftBorder2);
            border2.Append(rightBorder2);
            border2.Append(topBorder2);
            border2.Append(bottomBorder2);
            border2.Append(diagonalBorder2);

            borders1.Append(border1);
            borders1.Append(border2);

            var cellStyleFormats1 = new CellStyleFormats() { Count = (UInt32Value)1U };
            var cellFormat1 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U };

            cellStyleFormats1.Append(cellFormat1);

            var cellFormats1 = new CellFormats() { Count = (UInt32Value)3U };
            var cellFormat2 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)0U, FormatId = (UInt32Value)0U };
            var cellFormat3 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)0U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyBorder = true };
            var cellFormat4 = new CellFormat() { NumberFormatId = (UInt32Value)0U, FontId = (UInt32Value)1U, FillId = (UInt32Value)0U, BorderId = (UInt32Value)1U, FormatId = (UInt32Value)0U, ApplyFont = true, ApplyBorder = true };

            cellFormats1.Append(cellFormat2);
            cellFormats1.Append(cellFormat3);
            cellFormats1.Append(cellFormat4);

            var cellStyles1 = new CellStyles() { Count = (UInt32Value)1U };
            var cellStyle1 = new CellStyle() { Name = "Normal", FormatId = (UInt32Value)0U, BuiltinId = (UInt32Value)0U };

            cellStyles1.Append(cellStyle1);
            var differentialFormats1 = new DifferentialFormats() { Count = (UInt32Value)0U };
            var tableStyles1 = new TableStyles() { Count = (UInt32Value)0U, DefaultTableStyle = "TableStyleMedium2", DefaultPivotStyle = "PivotStyleLight16" };

            var stylesheetExtensionList1 = new StylesheetExtensionList();

            var stylesheetExtension1 = new StylesheetExtension() { Uri = "{EB79DEF2-80B8-43e5-95BD-54CBDDF9020C}" };
            stylesheetExtension1.AddNamespaceDeclaration("x14", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/main");
            X14.SlicerStyles slicerStyles1 = new X14.SlicerStyles() { DefaultSlicerStyle = "SlicerStyleLight1" };

            stylesheetExtension1.Append(slicerStyles1);

            var stylesheetExtension2 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
            stylesheetExtension2.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
            X15.TimelineStyles timelineStyles1 = new X15.TimelineStyles() { DefaultTimelineStyle = "TimeSlicerStyleLight1" };

            stylesheetExtension2.Append(timelineStyles1);

            stylesheetExtensionList1.Append(stylesheetExtension1);
            stylesheetExtensionList1.Append(stylesheetExtension2);

            stylesheet1.Append(fonts1);
            stylesheet1.Append(fills1);
            stylesheet1.Append(borders1);
            stylesheet1.Append(cellStyleFormats1);
            stylesheet1.Append(cellFormats1);
            stylesheet1.Append(cellStyles1);
            stylesheet1.Append(differentialFormats1);
            stylesheet1.Append(tableStyles1);
            stylesheet1.Append(stylesheetExtensionList1);

            workbookStylesPart1.Stylesheet = stylesheet1;
        }

        /// <summary>
        /// Generates the content of the worksheet part.
        /// </summary>
        /// <param name="worksheetPart1">The worksheet part1.</param>
        /// <param name="sheetData1">The sheet data1.</param>
        private void GenerateWorksheetPartContent(WorksheetPart worksheetPart1, SheetData sheetData1)
        {
            var worksheet = new Worksheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac" } };
            worksheet.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            worksheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            worksheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            var sheetDimension1 = new SheetDimension() { Reference = "A1" };

            var sheetViews1 = new SheetViews();

            var sheetView1 = new SheetView() { TabSelected = true, WorkbookViewId = (UInt32Value)0U };
            var selection1 = new Selection() { ActiveCell = "A1", SequenceOfReferences = new ListValue<StringValue>() { InnerText = "A1" } };

            sheetView1.Append(selection1);

            sheetViews1.Append(sheetView1);
            var sheetFormatProperties1 = new SheetFormatProperties() { DefaultRowHeight = 15D, DyDescent = 0.25D };

            var pageMargins1 = new PageMargins() { Left = 0.7D, Right = 0.7D, Top = 0.75D, Bottom = 0.75D, Header = 0.3D, Footer = 0.3D };
            worksheet.Append(sheetDimension1);
            worksheet.Append(sheetViews1);
            worksheet.Append(sheetFormatProperties1);
            worksheet.Append(sheetData1);
            worksheet.Append(pageMargins1);
            worksheetPart1.Worksheet = worksheet;
        }

        /// <summary>
        /// Creates the header row for excel.
        /// </summary>
        /// <returns></returns>
        private Row CreateHeaderRowForExcel()
        {
            var workRow = new Row();
            workRow.Append(CreateCell("S. No", 2U));
            workRow.Append(CreateCell("Operator Name", 2U));
            workRow.Append(CreateCell("Proactive Sent", 2U));
            workRow.Append(CreateCell("Proactive Answered", 2U));
            workRow.Append(CreateCell("Proactive Response Rate", 2U));
            workRow.Append(CreateCell("Reactive Received", 2U));
            workRow.Append(CreateCell("Reactive Answered", 2U));
            workRow.Append(CreateCell("Reactive Response Rate", 2U));
            workRow.Append(CreateCell("Total Chat Length", 2U));
            workRow.Append(CreateCell("Average Chant Length", 2U));
            return workRow;
        }

        /// <summary>
        /// Creates the cell.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private Cell CreateCell(string text)
        {
            var cell = new Cell
            {
                StyleIndex = 1U, DataType = ResolveCellDataTypeOnValue(text), CellValue = new CellValue(text)
            };
            return cell;
        }
        /// <summary>
        /// Creates the cell.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="styleIndex">Index of the style.</param>
        /// <returns></returns>
        private Cell CreateCell(string text, uint styleIndex)
        {
            var cell = new Cell
            {
                StyleIndex = styleIndex,
                DataType = ResolveCellDataTypeOnValue(text),
                CellValue = new CellValue(text)
            };
            return cell;
        }
        /// <summary>
        /// Resolves the cell data type on value.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private EnumValue<CellValues> ResolveCellDataTypeOnValue(string text)
        {
            int intVal;
            double doubleVal;
            if (int.TryParse(text, out intVal) || double.TryParse(text, out doubleVal))
            {
                return CellValues.Number;
            }
            else
            {
                return CellValues.String;
            }
        }

        #endregion
    }
}
