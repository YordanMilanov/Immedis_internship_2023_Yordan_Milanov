$(document).ready(function () {
    // Get references to the input and button
    const currentPageInput = $("#CurrentPage");
    const nextPageButton = $("#nextPageButton");
    const previousPageButton = $("#previousPageButton");

    // Add a click event handler to the button
    nextPageButton.click(function () {
        currentPageInput.val(parseInt(currentPageInput.val(), 10) + 1);
        $('#formId').submit();
    });

    previousPageButton.click(function () {
        currentPageInput.val(parseInt(currentPageInput.val(), 10) - 1);
        $('#formId').submit();
    });
});