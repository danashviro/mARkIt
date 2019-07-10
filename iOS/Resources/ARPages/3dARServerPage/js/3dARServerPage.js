var World = {
    initiallyLoadedData: false,

    markerDrawable_idle: null,

    loadPoisFromJsonData: function loadPoisFromJsonDataFn(poiData) {
       
        World.markerDrawable_idle = new AR.ImageResource("assets/woodSign.png");

        var marker = new Marker(poiData);


    },
    

    locationChanged: function locationChangedFn(lat, lon, alt, acc) {

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
                    }

                 }
        }
    },
};


AR.context.onLocationChanged = World.locationChanged;


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
    e.innerHTML = "got response";
    tableLoaded = true;

});
