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
        if(!req)
        {   req = true;
            AR.platform.sendJSONObject({ "option": "getMarks", "longitude": lon, "latitude": lat });
        }

        if (marksLoaded) {

               for (var i = 0 ; i < marks.length ; i++) {
                   var mark = marks[i];
                   if((mark.Longitude <= (lon + 0.0005))&& (mark.Longitude >= (lon - 0.0005)) && (mark.Latitude <= (lat + 0.0005))&& (mark.Latitude >= (lat - 0.0005)) && noMarks)
                    {
                         var markData = {
                             "id": mark.id,
                             "longitude": mark.Longitude,
                             "latitude": mark.Latitude,
                             "altitude": alt,
                             "message": mark.Message,
                             "style": mark.Style

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
var req = false;

AR.context.onLocationChanged = World.locationChanged;

var marks;
var marksLoaded = false;
var noMarks = true;


function setMarks(marksList)
{
   marks = marksList;
   marksLoaded = true;
   World.locationChanged(m_lat,m_lon,m_alt,m_acc);
}
