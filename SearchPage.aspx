<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchPage.aspx.cs" Inherits="SearchPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Amazon Search</title>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css"/>
</head>
<body>
    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>
    <script src="CurrencyUpdate.js"></script>
    <h1>Amazon Product Search</h1>
    <p>Enables you to search items on amazon.co.uk based on input keywords. It currently searches only products sold by Amazon.
        You can set the currency in the currency selector (default currency is EUR)
    </p>
    <form id="form1" runat="server">
        <asp:TextBox runat="server" ID="SearchField"></asp:TextBox>
        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="Search_Click" class="searchBtn"/>
   <div id="currencySelect">
       <div style="display:inline">Please select the currency to display prices in:</div>
       <select id ="CurrencySelector" >
         <option value="EUR">EUR</option>
       </select>
   </div>
    <div id="navStrip">
        <button id="previousBtn">Previous</button>
        <div class="pageNr">
            <div>Page nr:</div>
            <div id="pageNr">1</div>
        </div>
        <button id="nextBtn" >Next</button>
    </div>
    <div>
        <asp:Table runat="server" ID="ResultsTable">
            <asp:TableHeaderRow runat="server" CssClass="searchTableHeaderRow">
                <asp:TableCell runat="server" id="imageCol" CssClass="searchTableHeaderCell"></asp:TableCell>
                <asp:TableCell runat="server" id="titleCol" Text="Title" CssClass="searchTableHeaderCell"></asp:TableCell>
                <asp:TableCell runat="server" id="categoryCol" Text="Category" CssClass="searchTableHeaderCell"></asp:TableCell>
                <asp:TableCell runat="server" id="priceGBPCol" Text="Price in GBP" CssClass="searchTableHeaderCell"></asp:TableCell>
                <asp:TableCell runat="server" id="priceCol" Text="Price in EUR" CssClass="searchTableHeaderCell"></asp:TableCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <div>
       <asp:Label runat="server" ID="errorMessage"></asp:Label>
    </div>
    </form>
    </body>
</html>
