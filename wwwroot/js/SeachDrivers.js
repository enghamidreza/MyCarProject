$('#searchText').on('keyup', function () {
    var keyword = $(this).val();

    if (keyword.length < 2) {
        $('#suggestions').empty();
        return;
    }

    $.ajax({
        url: '/Drivers/SearchDrivers',
        type: 'GET',
        data: { keyword: keyword },
        success: function (data) {
            $('#suggestions').empty();

            data.forEach(function (driver) {
                // فقط اسم نمایش بده
                $('#suggestions').append(
                    '<li class="list-group-item suggestion-item" data-id="' + driver.id + '">' +
                    driver.fullName +
                    '</li>'
                );
            });
        }
    });
});
$(document).on('click', '.suggestion-item', function () {
    var id = $(this).data('id');
    $('#suggestions').empty();

    // درخواست AJAX برای گرفتن اطلاعات کامل راننده
    $.ajax({
        url: '/Drivers/GetDriverById',
        type: 'GET',
        data: { id: id },
        success: function (driver) {
            // پر کردن فیلدها
            $('#FullName').val(driver.fullName);
            $('#NationalCode').val(driver.nationalCode);
            $('#Phone').val(driver.phone);
        }
    });
});
