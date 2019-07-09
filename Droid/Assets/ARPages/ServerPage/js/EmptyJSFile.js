
var World = {
    initiallyLoadedData: false,

    markerDrawable_idle: null,

    // called to inject new POI data
    loadPoisFromJsonData: function loadPoisFromJsonDataFn(poiData) {
    
        World.markerDrawable_idle = new AR.ImageResource("assets/woodSign.png");
       
        var marker = new Marker(poiData);
    },
    

    navigator.geolocation.watchPosition( function(position) {
        var e = document.getElementById('debug');
        e.innerHTML = position.coords.latitude;
        var lon = position.coords.longitude;
        var lat = position.coords.latitude;
        var alt = position.coords.altitude;
        if (tableLoaded) {

               for (var i = 0 ; i < table.length ; i++) {
                   var row = table[i];
                   if((row.longitude <= (lon + 0.001))&& (row.longitude >= (lon - 0.001)) && (row.latitude <= (lat + 0.001))&& (row.latitude >= (lat - 0.001)) && noMarks)
                    {
                         var poiData = {
                             "id": i,
                             "longitude": (lon),
                             "latitude": (lat +0.5),
                             "altitude": alt,
                             "description": row.message

                        };
                        noMarks = false;
                       World.loadPoisFromJsonData(poiData);
                       var e = document.getElementById('debug');
                         e.innerHTML = "mark here";
                        
                    }

                 }
        }
    });
};


//AR.context.onLocationChanged = World.locationChanged;


var settings = {
  "async": true,
  "crossDomain": true,
  "url": "https://mark-api.azurewebsites.net/tables/Location?ZUMO-API-VERSION=2.0.0",
  "method": "GET",
  "headers": {
    "cache-control": "no-cache",
  }
}

var table;
var tableLoaded = false;
var noMarks = true;

$.ajax(settings).done(function (response) {
    table = response;
    var e = document.getElementById('debug');
    //e.innerHTML = "got response";
    tableLoaded = true;

});


var m = {
  "async": true,
  "crossDomain": true,
  "url": "https://www.googleapis.com/geolocation/v1/geolocate?key=AIzaSyAuIzE1MiZgMe5QmfL6eYaJHaqMGHvKSaw",
  "method": "POST",
  "headers": {
    "cache-control": "no-cache",
  }
}



$.ajax(m).done(function (response) {
    table = response;
    var e = document.getElementById('debug');
    e.innerHTML = response.location.lat;
    tableLoaded = true;

});
