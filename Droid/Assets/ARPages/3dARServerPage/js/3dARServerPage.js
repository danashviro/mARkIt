var World = {
    // true once data was fetched
    initiallyLoadedData: false,

    // POI-Marker asset
    markerDrawable_idle: null,

    // called to inject new POI data
    loadPoisFromJsonData: function loadPoisFromJsonDataFn(poiData) {

        /*
            The example Image Recognition already explained how images are loaded and displayed in the augmented reality view. This sample loads an AR.ImageResource when the World variable was defined. It will be reused for each marker that we will create afterwards.
        */
        World.markerDrawable_idle = new AR.ImageResource("assets/woodSign.png");

        /*
            Since there are additional changes concerning the marker it makes sense to extract the code to a separate Marker class (see marker.js). Parts of the code are moved from loadPoisFromJsonData to the Marker-class: the creation of the AR.GeoLocation, the creation of the AR.ImageDrawable and the creation of the AR.GeoObject. Then instantiate the Marker in the function loadPoisFromJsonData:
        */
        var marker = new Marker(poiData);


    },
    

    // location updates, fired every time you call architectView.setLocation() in native environment
    locationChanged: function locationChangedFn(lat, lon, alt, acc) {

        /*
            The custom function World.onLocationChanged checks with the flag World.initiallyLoadedData if the function was already called. With the first call of World.onLocationChanged an object that contains geo information will be created which will be later used to create a marker using the World.loadPoisFromJsonData function.
        */
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

/* 
    Set a custom function where location changes are forwarded to. There is also a possibility to set AR.context.onLocationChanged to null. In this case the function will not be called anymore and no further location updates will be received. 
*/
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
