$(document).ready(function () {
    $("#lstItem").change(function () {
        var $quantityValue = $("#lstItem option:selected").attr('quantity');
        $("#lstProductRange").empty();
        for (i = 1; i <= $quantityValue; i++) {
            $("#lstProductRange").append('<option value=' + i + '>' + i + '</option>');
        }
    }).change();

});