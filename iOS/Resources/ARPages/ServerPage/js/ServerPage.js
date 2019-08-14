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
        m_ShowedMarks.push(marker);
    },
    

    locationChanged: function locationChangedFn(lat, lon, alt, acc) {
        m_lat=lat;
        m_lon = lon;
        m_acc = acc;
        m_alt = alt;
        if (!m_LocationChanged) {
            m_LocationChanged = true;
            getMarks();
            setInterval(getMarks, 60000);
        }

        if (m_MarksLoaded) {

               for (var i = 0 ; i < m_Marks.length ; i++) {
                   var mark = m_Marks[i];
                   if((!markIsShowed(mark)) && (mark.Longitude <= (lon + 0.0002))&& (mark.Longitude >= (lon - 0.0002)) && (mark.Latitude <= (lat + 0.0002))&& (mark.Latitude >= (lat - 0.0002)))
                    {
                         var markData = {
                             "id": mark.id,
                             "longitude": mark.Longitude,
                             "latitude": mark.Latitude,
                             "altitude": alt,
                             "message": mark.Message,
                             "style": mark.Style

                        };
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
var m_ShowedMarks = [];

AR.context.onLocationChanged = World.locationChanged;

var m_Marks;
var m_MarksLoaded = false;

function setMarks(marksList) {
    m_Marks = marksList;
    m_MarksLoaded = true;
    World.locationChanged(m_lat,m_lon,m_alt,m_acc);
}

function getMarks() {
   AR.platform.sendJSONObject({ "option": "getMarks", "longitude": m_lon, "latitude": m_lat });   
}

function markIsShowed(mark)
{
    
    for (var i = 0 ; i < m_ShowedMarks.length ; i++) {
        if(mark.id == m_ShowedMarks[i].markData.id)
            return true;
    }
    return false;


}