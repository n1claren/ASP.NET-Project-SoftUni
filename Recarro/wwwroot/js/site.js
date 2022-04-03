$('#stats-button').on('click', ev => {
    $.get('api/stats', (data) => {
        $('#addUsers').text('Registered users: ' + data.totalUsers);
        $('#addVehicles').text('Registered vehicles: ' + data.totalVehicles);
        $('#addRents').text('Total rents: ' + data.totalRents);
    });

    $('#statSelect').removeClass('d-none');
    $('#stats-button').hide();
});