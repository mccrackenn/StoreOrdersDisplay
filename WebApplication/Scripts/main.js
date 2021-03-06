var uri = 'api/Orders';
myCityArray = [];
mySalesArray = [];
myMarkupArray = [];
tempMarkupArray = [];

/*https://nodogmablog.bryanhogan.net/2016/10/web-api-2-controller-with-multiple-get-methods/*/

$(document).ready(function () {
    fetch(uri).then(function (response) {
        if (response.ok) {
            return response.json();
        } else {
            return Promise.reject(response);
        }

    }).then(function (salesData) {
        console.log(salesData);
        mySalesArray = salesData;
        $.each(salesData, function (key, item) {$('#select-people').append($('<option></option').val(item.salesPersonID).html(item.FirstName +" "+item.LastName));
        });
        return fetch(uri + '/getcities');
    }).then(function (response) {
        if (response.ok) {
            return response.json();
        } else {
            return Proomise.reject(response);
        }
    }).then((cityData) => {
        myCityArray = cityData;
        console.log(cityData);
        $.each(cityData, function (key, item) {
            $('#select-cities').append($('<option></option').val(item.storeID).html(item.City));
        });
    })

});

GetEmp = () => {
    $('#employee-annual').empty();
    let empValue = $('#select-people').val();
    empValue = parseInt(empValue);
    console.log(empValue);
    fetch(uri + '/GetEmployeeSales?empID=' + empValue).then(function (response) {
        if (response.ok) {
            return response.json();
        } else {
            return Promise.reject(response);
        }
    }).then((sumData) => {
        console.log(sumData);
        $('#employee-annual').append("For the year, this Employee has sold: $"+sumData)
    })
}

GetCity = () => {
    $('#city-annual').empty();
    let cityValue = $('#select-cities').val();
    
    console.log(cityValue);
    fetch(uri + '/GetCitySales?cityID=' + cityValue).then(function (response) {
        if (response.ok) {
            return response.json();
        } else {
            return Promise.reject(response);
        }
    }).then((sumData) => {
        console.log(sumData);
        $('#city-annual').append("This Store has sold: $" + sumData)
    })
}

GetMarkups = () => {
    $('#markup-list').empty();
    fetch(uri + '/getmarkupresults').then(function (response) {
        if (response.ok) {
            return response.json();
        } else {
            return Promise.reject(response);
        }
    }).then((markupData) => {
        console.log(markupData);
        tempMarkupArray = markupData;
        myMarkupArray = mySort(tempMarkupArray);
        console.log(myMarkupArray);
        $.each(myMarkupArray, function (key, item) {

            var $myVar = $("<li>City:"+ item[0].City +", Count:" +item.length +"</li>");
            $('#markup-list').append($myVar);

        });
    })

}

mySort = (anArray) => {
    anArray.sort(function (a, b) {
        if (a.length < b.length) return 1;
        if (a.length > b.length) return -1;
        return 0;
    });
    return anArray;
}
