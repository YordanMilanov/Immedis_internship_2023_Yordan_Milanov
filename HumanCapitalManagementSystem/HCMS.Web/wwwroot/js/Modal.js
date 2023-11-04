$(document).ready(function () {
    const deleteButton = $("#deleteButton");
    const confirmationModal = $("#confirmationModal");
    const confirmDelete = $("#confirmDelete");

    deleteButton.click(function (event) {
        event.preventDefault(); // Prevent the default link behavior
        confirmationModal.modal("show");
    });

    confirmDelete.click(function () {
        // Perform the delete action, e.g., by navigating to the delete URL
        const deleteUrl = '@Url.Action("Delete", "Education", new { id = @Model.Id })';
        window.location.href = deleteUrl;
    });
});