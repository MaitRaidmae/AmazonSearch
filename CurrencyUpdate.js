$(function () {
    
    var currencies;
    var ITEMS_PER_PAGE = 13;
    //Disable "previous" button at first: 
    $('#previousBtn').prop('disabled', true);

    
    //Load Available Currencies from the webService
    $.get('https://openexchangerates.org/api/latest.json?app_id=f74a25be2a354788891b42df1ed6d271').done(function (data) {
        currencies = data.rates;
        // Generate the Currency selection dropdown values;
        $.each(currencies, function (key) {
            $('#CurrencySelector').append($('<option></option>').val(key).html(key));
        });
        // Update prices with default currency;
        updatePrices(currencies);
    });
       
    //If new currency is selected updated prices in local currency.
    $('#CurrencySelector').change(function (e) {
        // Update the column header for selected currency
        $('#priceCol').text("Price in " + $('#CurrencySelector option:selected').text());
        updatePrices(currencies);
        e.preventDefault();
    });
    
    $('#nextBtn').click(function (e) {
        e.preventDefault();
        // Get the current tablesize (found search results) for next button
        var tableSize = $('#ResultsTable tr').size();
        console.log(tableSize);
        var currentPage = parseInt($('#pageNr').text());
        currentPage++;
        $('#pageNr').text(currentPage);
        updateShownTableRows(currentPage);
        
        // Disable next button if there are no more results
        if (currentPage * ITEMS_PER_PAGE > tableSize - 1) {
            $('#nextBtn').prop('disabled',true);
        }
        //Enable "previous" button
        $('#previousBtn').prop('disabled',false);
    });

    $('#previousBtn').click(function (d) {
        d.preventDefault();
        var currentPage = parseInt($('#pageNr').text());
        currentPage--;
        $('#pageNr').text(currentPage);
        updateShownTableRows(currentPage);
        //Disable "previous" button if currentPage reaches 1;
        if (currentPage == 1) {
            $('#previousBtn').prop('disabled', true);
        }
        //Enable "next" button.
        $('#nextBtn').prop('disabled', false);
    });

    function updateShownTableRows(pageNr) {
        var lastItem = pageNr * ITEMS_PER_PAGE;
        var firstItem = lastItem - 12;
        $('tr').each(function (index) {
          if (index + 1 < firstItem || index > lastItem) {
              $('tr.RowNr' + index).hide();
          } else {
              $('tr.RowNr' + index).show();
          }
        });
    }

   // Function that gets currency rates from a webservice and updates the prices in the table accordingly.
    function updatePrices(currencies) {
        var currencyRate = 0.0;
        var currency = $('#CurrencySelector option:selected').text();
        currencyRate = currencies[currency];
        GBPRate = currencies["GBP"]
        var i = 0;
        $('tr').each(function () { 
            var GBPPrice = $('tr.RowNr' + i + '> td.priceGBPCellClass').text();
            if (GBPPrice == "-") {
                $('tr.RowNr' + i + '> td.priceCellClass').text("-");
            } else {
                var USDPrice = GBPPrice / GBPRate;
                var newPrice = (USDPrice * currencyRate).toFixed(2);
                $('tr.RowNr' + i + '> td.priceCellClass').text(newPrice);
            }
            i = i + 1;
        });
    };
});
