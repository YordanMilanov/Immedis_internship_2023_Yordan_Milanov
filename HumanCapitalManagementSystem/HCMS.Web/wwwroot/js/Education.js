$(document).ready(function () {
    // Get references to the input and button
    const currentPageInput = $("#Page");
    const nextPageButton = $("#nextPageButton");
    const previousPageButton = $("#previousPageButton");
    const maxPage = parseInt($("#maxPage").val(), 10);

    // Add a click event handler to the button
    nextPageButton.click(function () {
        currentPageInput.val(parseInt(currentPageInput.val(), 10) + 1);
        $('#formId').submit();
    });

    previousPageButton.click(function () {
        currentPageInput.val(parseInt(currentPageInput.val(), 10) - 1);
        $('#formId').submit();
    });

    if (parseInt(currentPageInput.val(), 10) == 1) {
        previousPageButton.hide();
    }

    if (parseInt(currentPageInput.val(), 10) >= maxPage) {
        nextPageButton.hide();
    }
});