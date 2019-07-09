var World = {
    initiallyLoadedData: false,

    markerDrawable_idle: null,

    // called to inject new POI data
    loadPoisFromJsonData: function loadPoisFromJsonDataFn(poiData) {
    
        World.markerDrawable_idle = new AR.ImageResource("assets/woodSign.png");
       
        var marker = new Marker(poiData);
    },
    

    locationChanged: function locationChangedFn(lat, lon, alt, acc) {
        m_lat=lat;
        m_lon = lon;
        m_acc = acc;
        m_alt = alt;
        m_LocationChanged = true;

        if (tableLoaded) {

               for (var i = 0 ; i < table.length ; i++) {
                   var row = table[i];
                   if((row.longitude <= (lon + 0.0005))&& (row.longitude >= (lon - 0.0005)) && (row.latitude <= (lat + 0.0005))&& (row.latitude >= (lat - 0.0005)) && noMarks)
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

var m_lat;
var m_lon;
var m_alt;
var m_acc;
var m_LocationChanged = false;


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
    if(m_LocationChanged){
        World.locationChanged(m_lat, m_lon, m_alt, m_acc);
    }
    
});
