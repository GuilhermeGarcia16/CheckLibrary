$.ajax({
    type: 'GET',
    url: "https://restcountries.com/v3.1/all?fields=name,flag",
    dataType: 'json',
    success: function (data) {
        $.each(data, function (index, value) {
            $('#nationality')
                .append($("<option>")
                    .attr("value", value.flag)
                    .html(value.name.common + "-" + value.flag));
        })
    }
})