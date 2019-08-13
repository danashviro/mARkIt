var World = {
    initiallyLoadedData: false,

    markerDrawable_idle: null,

    // called to inject new POI data
    loadMarksFromJsonData: function loadMarksFromJsonDataFn(markData) {
        if (markData.style == "Wood") {
            World.markerDrawable_idle = new AR.ImageResource("assets/woodSign.png");
        } else if (markData.style == "Metal") {
            World.markerDrawable_idle = new AR.ImageResource("assets/metalSign.png");             
        } else {
            World.markerDrawable_idle = new AR.ImageResource("assets/schoolSign.png");      
        }

    
       
        var marker = new Marker(markData);
    },
    

    locationChanged: function locationChangedFn(lat, lon, alt, acc) {
        m_lat=lat;
        m_lon = lon;
        m_acc = acc;
        m_alt = alt;
        m_LocationChanged = true;

        if (tableLoaded) {

               for (var i = 0 ; i < table.length ; i++) {
                   var mark = table[i];
                   if((mark.longitude <= (lon + 0.0005))&& (mark.longitude >= (lon - 0.0005)) && (mark.latitude <= (lat + 0.0005))&& (mark.latitude >= (lat - 0.0005)) && noMarks)
                    {
                         var markData = {
                             "id": mark.id,
                             "longitude": mark.longitude,
                             "latitude": mark.latitude,
                             "altitude": alt,
                             "message": mark.message,
                             "style": mark.style

                        };
                        noMarks = false;
                        World.loadMarksFromJsonData(markData);
                        
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
  "url": "https://mark-api.azurewebsites.net/tables/Mark?ZUMO-API-VERSION=2.0.0",
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
    //var e = document.getElementById('debug');
    //e.innerHTML = "got response";
    tableLoaded = true;
    if(m_LocationChanged){
        World.locationChanged(m_lat, m_lon, m_alt, m_acc);
    }
    
});



function addButtonClicked() {
    AR.platform.sendJSONObject({"option":"add"});
}
