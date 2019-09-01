var World = {
    initiallyLoadedData: false,


    // called to inject new POI data
    loadMarksFromJsonData: function loadMarksFromJsonDataFn(markData,markerLocation) {

        var marker = new Marker(markData,markerLocation);
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
            setInterval(getMarks, 10000);
        }

        if (m_MarksLoaded) {
            deleteOldMarks();
            showMarks();
        }
    },
};

var m_lat;
var m_lon;
var m_alt;
var m_acc;
var m_LocationChanged = false;
var m_ShowedMarks = [];
// scaling value of 1 between 0 meters and 1 meters from the marker
// linear scaling starts at this distance
//AR.context.scene.minScalingDistance = 0.5;

// scaling value of AR.context.scene.scalingFactor at 50km and more distance
// linear scaling stops at this distance
//AR.context.scene.maxScalingDistance = 500;

// the scaling factor at maxScalingDistance
// this is the smallest scaling applied
//AR.context.scene.scalingFactor = 0.001;

AR.context.onLocationChanged = World.locationChanged;

var m_Marks=[];
var m_MarksLoaded = false;
var x = 1;
function setMarks(marksList) {
    m_Marks = null;
    m_Marks = marksList;
    m_MarksLoaded = true;
    World.locationChanged(m_lat,m_lon,m_alt,m_acc);
}

function getMarks() {
   AR.platform.sendJSONObject({ "option": "getMarks", "longitude": m_lon, "latitude": m_lat });   
}

function markIsShowed(mark){  
    for (var i = 0 ; i < m_ShowedMarks.length ; i++) {
        if(mark.id == m_ShowedMarks[i].markData.id && m_ShowedMarks[i].deleted==false)
            return true;
    }
    return false;
}

function markInNewList(mark){  
    for (var i = 0 ; i < m_Marks.length ; i++) {
        if(m_Marks[i].id == mark.id)
            return true;
    }
    return false;
}

function deleteOldMarks(){
    var e = document.getElementById('debug');
    e.innerHTML = x;
    x++;
    for (var i = 0 ; i < m_ShowedMarks.length ; i++) {
        var markData = m_ShowedMarks[i].markData;
        if( (markInNewList(markData)==false) || (m_ShowedMarks.markerLocation.distanceToUser()>100)){
            m_ShowedMarks[i].markerObject.enabled = false;
            m_ShowedMarks[i].deleted = true;
            m_ShowedMarks.splice(i, i);
        }
    }
}

function showMarks(){
    for (var i = 0 ; i < m_Marks.length ; i++) {
        var mark = m_Marks[i];
        var markerLocation = new AR.GeoLocation(mark.Latitude, mark.Longitude, m_alt);
        if((!markIsShowed(mark)) && (markerLocation.distanceToUser() <= 100))
         {
              var markData = {
                  "id": mark.id,
                  "longitude": mark.Longitude,
                  "latitude": mark.Latitude,
                  "altitude": m_alt,
                  "message": mark.Message,
                  "style": mark.Style

             };
             World.loadMarksFromJsonData(markData,markerLocation);                        
         }
      }
}