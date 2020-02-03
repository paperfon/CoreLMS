// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label').html(fileName);
    });
});

$(document).ready(function () {
    $("#selectedentity").on("change", function () {
        $.getJSON("/Documents/GetEntityNamelist", { entityname: $("#selectedentity").val() }, function (d) {

            var row = "";
            $("#selectedentityid").empty();
            $.each(d, function (i, v) {
                row += "<option value=" + v.value + ">" + v.text + "</option>";

            });
            $("#selectedentityid").append(row);

        });


    });
});
