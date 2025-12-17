$('#CustomerId').on('change', function () {
    const o = $(this).find(':selected');

    $('#CustomerOib').val(o.data('oib') || '');
    $('#CustomerName').val(o.data('name') || '');
    $('#CustomerStreet').val(o.data('street') || '');
    $('#CustomerPostalCode').val(o.data('postal') || '');
    $('#CustomerCity').val(o.data('city') || '');
    $('#CustomerEmail').val(o.data('email') || '');
    $('#CustomerPhone').val(o.data('phone') || '');
});