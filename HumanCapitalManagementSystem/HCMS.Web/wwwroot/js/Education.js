$(document).ready(function () {
    // Get references to the input and button
    const currentPageInput = $("#Page");
    const nextPageButton = $("#nextPageButton");
    const previousPageButton = $("#previousPageButton");

    // Add a click event handler to the button
    nextPageButton.click(function (e) {
        e.preventDefault();
        currentPageInput.val(parseInt(currentPageInput.val(), 10) - 1);
        $('#previousPageButton').submit();
    });

    previousPageButton.click(function (e) {
        e.preventDefault();
        currentPageInput.val(parseInt(currentPageInput.val(), 10) + 1);
        $('#nextPageButton').submit();
    });
});