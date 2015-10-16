using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AmazonProductAPI;
using System.Xml;
using System.Xml.Linq;

public partial class SearchPage : System.Web.UI.Page
{
    // Maximum pages returned by AmazonAPI
    private static int MAX_PAGES = 5;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Search_Click(object sender, EventArgs e)
    {
        //int SheetLoaded = Convert.ToInt32(SearchSheet.SelectedValue);
        String searchString = SearchField.Text;
        IEnumerable<XElement> items = null;
        XNamespace nameSpace = AmazonAPIRequest.NAMESPACE;
        XElement responseX = null;
        try
        {
            for (int i = 1; i < MAX_PAGES+1; i++)
            {

                XmlDocument responseXml = AmazonAPIRequest.Request(i, searchString);
                // Convert XmlDocument to XDocument
                responseX = XElement.Load(new XmlNodeReader(responseXml));

                // Get Items
                if (items == null)
                {
                    items = from els in responseX.Elements(nameSpace + "Items") select els;
                }
                else
                {
                    IEnumerable<XElement> pageItems = from els in responseX.Elements(nameSpace + "Items") select els;
                    items = items.Concat(pageItems);
                }
            }

            // Get Titles
            IEnumerable<XElement> titles = from x in items.Elements(nameSpace + "Item") select x.Element(nameSpace + "ItemAttributes").Element(nameSpace + "Title");
            // Get Images
            IEnumerable<XElement> images = from x in items.Elements(nameSpace + "Item") select x.Element(nameSpace + "SmallImage");
            //Get Categories
            IEnumerable<XElement> productGroups = from x in items.Elements(nameSpace + "Item") select x.Element(nameSpace + "ItemAttributes").Element(nameSpace + "ProductGroup");
            //Get Prices
            IEnumerable<XElement> prices = from x in items.Elements(nameSpace + "Item") select x.Element(nameSpace + "ItemAttributes").Element(nameSpace + "ListPrice");
            //Get Urls
            IEnumerable<XElement> urls = from x in items.Elements(nameSpace + "Item") select x.Element(nameSpace + "DetailPageURL");

            Create_Table_Rows(titles, images, productGroups, prices, urls);
        }
        catch (Exception exception)
        {
            errorMessage.Text = "Error occured: Failed to load data from Amazon. Please check your connection and/or Amazon Associate Credentials.";
        }

    }

    private void Create_Table_Rows(IEnumerable<XElement> titles, IEnumerable<XElement> images, IEnumerable<XElement> productGroups, IEnumerable<XElement> priceGBPs, IEnumerable<XElement> urls)
    {
        IEnumerable<XElement> priceElements = null;
        IEnumerable<XElement> imageElements = null;

        for (int i = 0; i < titles.Count(); i++)
        {
            TableRow tRow = new TableRow();
            // Hide all results not on page 1:
            if (i > 13)
            {
                tRow.Style.Add("display", "none");
            }

            TableCell imageCell = new TableCell();
            Image productImage = new Image();

            productImage.ID = "image" + i;
            if (images.ElementAt(i) != null)
            {
                imageElements = images.ElementAt(i).Descendants();
                productImage.ImageUrl = imageElements.ElementAt(0).Value;
            }
            else
            {
                // If there is no image use default amazon icon
                productImage.ImageUrl = "/Images/Amazon-icon-50-50.jpg";
            }
            imageCell.Controls.Add(productImage);

            TableCell titleCell = new TableCell();
            HyperLink link = new HyperLink();
            link.NavigateUrl = urls.ElementAt(i).Value;
            link.Text = titles.ElementAt(i).Value;
            titleCell.Controls.Add(link);

            TableCell categoryCell = new TableCell();
            categoryCell.Text = productGroups.ElementAt(i).Value;
            categoryCell.CssClass = "categoryCellClass";

            //priceGBPCell creation
            TableCell priceGBPCell = new TableCell();
            priceGBPCell.CssClass = "priceGBPCellClass";
            if (priceGBPs.ElementAt(i) != null)
            {
                priceElements = priceGBPs.ElementAt(i).Descendants();
                //Add decimal place to amount value:
                double itemPrice = ((Double)priceElements.ElementAt(0) / 100);
                priceGBPCell.Text = itemPrice.ToString("0.00");
            }
            else
            {
                priceGBPCell.Text = "-";
            }

            // priceCell creation and classes:
            TableCell priceCell = new TableCell();
            priceCell.Text = "";
            priceCell.CssClass = "priceCellClass";

            tRow.CssClass = "RowNr" + i;

            tRow.Cells.Add(imageCell);
            tRow.Cells.Add(titleCell);
            tRow.Cells.Add(categoryCell);
            tRow.Cells.Add(priceGBPCell);
            tRow.Cells.Add(priceCell);

            ResultsTable.Rows.Add(tRow);
        }
    }
}