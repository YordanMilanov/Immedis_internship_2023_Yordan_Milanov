$(document).ready(function () {
    $('#companySelect').select2({
        placeholder: 'Company...',
        width: '100%',
        allowClear: false,
    });
});

$(document).ready(function () {
    $('#degreeSelect').select2({
        tags: true,
        placeholder: 'Your input here...',
        width: '100%',
        allowClear: true,
    });
});