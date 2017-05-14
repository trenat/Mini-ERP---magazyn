using System;
using MahApps.Metro.Controls;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MiniERP_desktop.Helpers;
using MiniERP_desktop.Models;
using PdfSharp.Pdf.Security;
using Utils = MiniERP_desktop.Helpers.Utils;

namespace MiniERP_desktop.Services
{
    public class PdfInvoice
    {
        public Document Pdf { get; private set; }
        public Invoice Invoice { get; private set; }

        private PageFormat InvoiceFormat
        {
            get
            {
                switch (Invoice.PageSize)
                {
                    case SizeOption.A4:
                        return PageFormat.A4;
                    case SizeOption.Legal:
                        return PageFormat.Legal;
                    case SizeOption.Letter:
                        return PageFormat.Letter;
                    default:
                        throw new ArgumentException("Unable to find matching size.");
                }
            }
        }

        private Orientation InvoiceOrientation
        {
            get
            {
                switch (Invoice.PageOrientation)
                {
                    case OrientationOption.Landscape:
                        return Orientation.Landscape;
                    case OrientationOption.Portrait:
                        return Orientation.Portrait;
                    default:
                        throw new ArgumentException("Unable to find matching orientation.");
                }
            }
        }

        private Border BorderLine
        {
            get
            {
                Border bottomLine = new Border();
                bottomLine.Width = new Unit(0.5);
             //TODO:   bottomLine.Color = MigraDocHelpers.TextColorFromHtml(Invoice.TextColor);
                return bottomLine;
            }
        }

        public PdfInvoice(Invoice invoice)
        {
            Pdf = new Document();
            Invoice = invoice;
            
        }

        public void Save(string filename, string password = null)
        {
            CreateDocument();

            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = Pdf;
            renderer.RenderDocument();
            if (!string.IsNullOrEmpty(password))
                SetPassword(renderer, password);
            renderer.PdfDocument.Save(filename);
        }

        public Document GenerateWithoutSave()
        {
            CreateDocument();
            return Pdf;
        }

        private void CreateDocument()
        {
            Pdf.DefaultPageSetup.PageFormat = InvoiceFormat;
            Pdf.DefaultPageSetup.Orientation = InvoiceOrientation;
            Pdf.DefaultPageSetup.TopMargin = 125;
            Pdf.Info.Title = Invoice.Title;

            DefineStyles();

            Pdf.AddSection();
            HeaderSection();
            AddressSection();
            BillingSection();
            PaymentSection();
            FooterSection();
        }

        private void SetPassword(PdfDocumentRenderer renderer, string password)
        {
            PdfSecuritySettings securitySettings = renderer.PdfDocument.SecuritySettings;
            securitySettings.OwnerPassword = password;
            securitySettings.UserPassword = password;
            securitySettings.PermitAccessibilityExtractContent = false;
            securitySettings.PermitAnnotations = false;
            securitySettings.PermitAssembleDocument = false;
            securitySettings.PermitExtractContent = false;
            securitySettings.PermitFormsFill = false;
            securitySettings.PermitFullQualityPrint = false;
            securitySettings.PermitModifyDocument = false;
            securitySettings.PermitPrint = false;
        }

        private void DefineStyles()
        {
            MigraDoc.DocumentObjectModel.Style style = Pdf.Styles["Normal"];
            style.Font.Name = "Calibri";

            style = Pdf.Styles.AddStyle("H1-20", "Normal");
            style.Font.Size = 20;
            style.Font.Bold = true;

            style = Pdf.Styles.AddStyle("H2-8", "Normal");
            style.Font.Size = 8;

            style = Pdf.Styles.AddStyle("H2-8-Blue", "H2-8");
            style.ParagraphFormat.Font.Color = Colors.Blue;

            style = Pdf.Styles.AddStyle("H2-8B", "H2-8");
            style.Font.Bold = true;

            style = Pdf.Styles.AddStyle("H2-9", "Normal");
            style.Font.Size = 9;

            style = Pdf.Styles.AddStyle("H2-9-Grey", "H2-9");
            style.Font.Color = Colors.Gray;

            style = Pdf.Styles.AddStyle("H2-9B", "H2-9");
            style.Font.Bold = true;

            style = Pdf.Styles.AddStyle("H2-9B-Inverse", "H2-9B");
            style.ParagraphFormat.Font.Color = Colors.White;

            style = Pdf.Styles.AddStyle("H2-9B-Color", "H2-9B");
            style.Font.Color = MigraDocHelpers.TextColorFromHtml(Invoice.TextColor);

            style = Pdf.Styles.AddStyle("H2-10", "Normal");
            style.Font.Size = 10;

            style = Pdf.Styles.AddStyle("H2-10B", "H2-10");
            style.Font.Bold = true;

            style = Pdf.Styles.AddStyle("H2-10B-Color", "H2-10B");
          //TODO  style.Font.Color = MigraDocHelpers.TextColorFromHtml(Invoice.TextColor);
        }

        private void HeaderSection()
        {
            HeaderFooter header = Pdf.LastSection.Headers.Primary;

            Table table = header.AddTable();
            double thirdWidth = Pdf.PageWidth() / 3;

            table.AddColumn(ParagraphAlignment.Left, thirdWidth * 2);
            table.AddColumn();

            Row row = table.AddRow();

            if (!string.IsNullOrEmpty(Invoice.Image))
            {
                Image image = row.Cells[0].AddImage(Invoice.Image);
                row.Cells[0].VerticalAlignment = VerticalAlignment.Center;

                image.Height = Invoice.ImageSize.Height;
                image.Width = Invoice.ImageSize.Width;
            }

            TextFrame frame = row.Cells[1].AddTextFrame();

            Table subTable = frame.AddTable();
            subTable.AddColumn(thirdWidth / 2);
            subTable.AddColumn(thirdWidth / 2);

            row = subTable.AddRow();
            row.Cells[0].MergeRight = 1;
            row.Cells[0].AddParagraph(Invoice.Title, ParagraphAlignment.Right, "H1-20");

            row = subTable.AddRow();
            row.Cells[0].AddParagraph("REFERENCE:", ParagraphAlignment.Left, "H2-9B-Color");
            row.Cells[1].AddParagraph(Invoice.Reference, ParagraphAlignment.Right, "H2-9");
            row.Cells[0].AddParagraph("BILLING DATE:", ParagraphAlignment.Left, "H2-9B-Color");
            row.Cells[1].AddParagraph(Invoice.BillingDate.ToShortDateString(), ParagraphAlignment.Right, "H2-9");
            row.Cells[0].AddParagraph("DUE DATE:", ParagraphAlignment.Left, "H2-9B-Color");
            row.Cells[1].AddParagraph(Invoice.DueDate.ToShortDateString(), ParagraphAlignment.Right, "H2-9");
        }

        public void FooterSection()
        {
            HeaderFooter footer = Pdf.LastSection.Footers.Primary;

            Table table = footer.AddTable();
            table.AddColumn(footer.Section.PageWidth() / 2);
            table.AddColumn(footer.Section.PageWidth() / 2);
            Row row = table.AddRow();
            if (!string.IsNullOrEmpty(Invoice.Footer))
            {
                Paragraph paragraph = row.Cells[0].AddParagraph(Invoice.Footer, ParagraphAlignment.Left, "H2-8-Blue");
                Hyperlink link = paragraph.AddHyperlink(Invoice.Footer, HyperlinkType.Web);
            }

            Paragraph info = row.Cells[1].AddParagraph();
            info.Format.Alignment = ParagraphAlignment.Right;
            info.Style = "H2-8";
            info.AddText("Page ");
            info.AddPageField();
            info.AddText(" of ");
            info.AddNumPagesField();
        }

        private void AddressSection()
        {
            Section section = Pdf.LastSection;

            Address leftAddress = Invoice.Company;
            Address rightAddress = Invoice.Client;

            if (Invoice.CompanyOrientation == PositionOption.Right)
                Utils.Swap<Address>(ref leftAddress, ref rightAddress);

            Table table = section.AddTable();
            table.AddColumn(ParagraphAlignment.Left, section.Document.PageWidth() / 2 - 10);
            table.AddColumn(ParagraphAlignment.Center, Unit.FromPoint(20));
            table.AddColumn(ParagraphAlignment.Left, section.Document.PageWidth() / 2 - 10);

            Row row = table.AddRow();
            row.Style = "H2-10B-Color";
            row.Shading.Color = Colors.White;

            row.Cells[0].AddParagraph(leftAddress.Title, ParagraphAlignment.Left);
            row.Cells[0].Format.Borders.Bottom = BorderLine;
            row.Cells[2].AddParagraph(rightAddress.Title, ParagraphAlignment.Left);
            row.Cells[2].Format.Borders.Bottom = BorderLine;

            row = table.AddRow();
            AddressCell(row.Cells[0], leftAddress.AddressLines);
            AddressCell(row.Cells[2], rightAddress.AddressLines);

            row = table.AddRow();
        }

        private void AddressCell(Cell cell, string[] address)
        {
            foreach (string line in address)
            {
                Paragraph name = cell.AddParagraph();
                if (line == address[0])
                    name.AddFormattedText(line, "H2-10B");
                else
                    name.AddFormattedText(line, "H2-9-Grey");
            }
        }

        private void BillingSection()
        {
            Section section = Pdf.LastSection;

            Table table = section.AddTable();

            double width = section.PageWidth();
            double productWidth = Unit.FromPoint(150);
            double numericWidth = (width - productWidth) / (Invoice.HasDiscount ? 5 : 4);
            table.AddColumn(productWidth);
            table.AddColumn(ParagraphAlignment.Center, numericWidth);
            table.AddColumn(ParagraphAlignment.Center, numericWidth);
            table.AddColumn(ParagraphAlignment.Center, numericWidth);
            if (Invoice.HasDiscount)
                table.AddColumn(ParagraphAlignment.Center, numericWidth);
            table.AddColumn(ParagraphAlignment.Center, numericWidth);

            BillingHeader(table);

            foreach (ItemRow item in Invoice.Items)
            {
                BillingRow(table, item);
            }

            if (Invoice.Totals != null)
            {
                foreach (TotalRow total in Invoice.Totals)
                {
                    BillingTotal(table, total);
                }
            }
            table.AddRow();
        }

        private void BillingHeader(Table table)
        {
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Style = "H2-10B-Color";
            row.Shading.Color = Colors.White;
            row.TopPadding = 10;
            row.Borders.Bottom = BorderLine;

            row.Cells[0].AddParagraph("PRODUCT", ParagraphAlignment.Left);
            row.Cells[1].AddParagraph("AMOUNT", ParagraphAlignment.Center);
            row.Cells[2].AddParagraph("VAT %", ParagraphAlignment.Center);
            row.Cells[3].AddParagraph("UNIT PRICE", ParagraphAlignment.Center);
            if (Invoice.HasDiscount)
            {
                row.Cells[4].AddParagraph("DISCOUNT", ParagraphAlignment.Center);
                row.Cells[5].AddParagraph("TOTAL", ParagraphAlignment.Center);
            }
            else
            {
                row.Cells[4].AddParagraph("TOTAL", ParagraphAlignment.Center);
            }
        }

        private void BillingRow(Table table, ItemRow item)
        {
            Row row = table.AddRow();
            row.Style = "TableRow";
            row.Shading.Color = MigraDocHelpers.BackColorFromHtml(Invoice.BackColor);

            Cell cell = row.Cells[0];
            cell.AddParagraph(item.Name, ParagraphAlignment.Left, "H2-9B");
            cell.AddParagraph(item.Description, ParagraphAlignment.Left, "H2-9-Grey");

            cell = row.Cells[1];
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph(item.Amount.ToCurrency(), ParagraphAlignment.Center, "H2-9");

            cell = row.Cells[2];
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph(item.VAT.ToCurrency(), ParagraphAlignment.Center, "H2-9");

            cell = row.Cells[3];
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.AddParagraph(item.Price.ToCurrency(Invoice.Currency), ParagraphAlignment.Center, "H2-9");

            if (Invoice.HasDiscount)
            {
                cell = row.Cells[4];
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.AddParagraph(item.Discount, ParagraphAlignment.Center, "H2-9");

                cell = row.Cells[5];
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.AddParagraph(item.Total.ToCurrency(Invoice.Currency), ParagraphAlignment.Center, "H2-9");
            }
            else
            {
                cell = row.Cells[4];
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.AddParagraph(item.Total.ToCurrency(Invoice.Currency), ParagraphAlignment.Center, "H2-9");
            }
        }

        private void BillingTotal(Table table, TotalRow total)
        {
            if (Invoice.HasDiscount)
            {
                table.Columns[4].Format.Alignment = ParagraphAlignment.Left;
                table.Columns[5].Format.Alignment = ParagraphAlignment.Left;
            }
            else
            {
                table.Columns[4].Format.Alignment = ParagraphAlignment.Left;
            }

            Row row = table.AddRow();
            row.Style = "TableRow";

            string font; Color shading;
            if (total.Inverse == true)
            {
                font = "H2-9B-Inverse";
                shading = MigraDocHelpers.TextColorFromHtml(Invoice.TextColor);
            }
            else
            {
                font = "H2-9B";
                shading = MigraDocHelpers.BackColorFromHtml(Invoice.BackColor);
            }

            if (Invoice.HasDiscount)
            {
                Cell cell = row.Cells[4];
                cell.Shading.Color = shading;
                cell.AddParagraph(total.Name, ParagraphAlignment.Left, font);

                cell = row.Cells[5];
                cell.Shading.Color = shading;
                cell.AddParagraph(total.Value.ToCurrency(Invoice.Currency), ParagraphAlignment.Center, font);
            }
            else
            {
                Cell cell = row.Cells[3];
                cell.Shading.Color = shading;
                cell.AddParagraph(total.Name, ParagraphAlignment.Left, font);

                cell = row.Cells[4];
                cell.Shading.Color = shading;
                cell.AddParagraph(total.Value.ToCurrency(Invoice.Currency), ParagraphAlignment.Center, font);
            }
        }

        private void PaymentSection()
        {
            Section section = Pdf.LastSection;

            Table table = section.AddTable();
            table.AddColumn(Unit.FromPoint(section.Document.PageWidth()));
            Row row = table.AddRow();

            if (Invoice.Details != null && Invoice.Details.Count > 0)
            {
                foreach (DetailRow detail in Invoice.Details)
                {
                    row.Cells[0].AddParagraph(detail.Title, ParagraphAlignment.Left, "H2-9B-Color");
                    row.Cells[0].Borders.Bottom = BorderLine;

                    row = table.AddRow();
                    TextFrame frame = null;
                    foreach (string line in detail.Paragraphs)
                    {
                        if (line == detail.Paragraphs[0])
                        {
                            frame = row.Cells[0].AddTextFrame();
                            frame.Width = section.Document.PageWidth();
                        }
                        frame.AddParagraph(line, ParagraphAlignment.Left, "H2-9");
                    }
                }
            }

            if (Invoice.Company.HasCompanyNumber || Invoice.Company.HasVatNumber)
            {
                row = table.AddRow();

                Color shading = MigraDocHelpers.TextColorFromHtml(Invoice.TextColor);

                if (Invoice.Company.HasCompanyNumber && Invoice.Company.HasVatNumber)
                {
                    row.Cells[0].AddParagraph(string.Format("Company Number: {0}, VAT Number: {1}",
                                Invoice.Company.CompanyNumber, Invoice.Company.VatNumber),
                            ParagraphAlignment.Center, "H2-9B-Inverse")
                        .Format.Shading.Color = shading;
                }
                else
                {
                    if (Invoice.Company.HasCompanyNumber)
                        row.Cells[0].AddParagraph(string.Format("Company Number: {0}", Invoice.Company.CompanyNumber),
                                ParagraphAlignment.Center, "H2-9B-Inverse")
                            .Format.Shading.Color = shading;
                    else
                        row.Cells[0].AddParagraph(string.Format("VAT Number: {0}", Invoice.Company.VatNumber),
                                ParagraphAlignment.Center, "H2-9B-Inverse")
                            .Format.Shading.Color = shading;
                }
            }
        }
    }
}

