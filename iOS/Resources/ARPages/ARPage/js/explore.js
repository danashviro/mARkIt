var World = {

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
            setInterval(getMarks, 30000);
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
var m_Marks=[];
var m_MarksLoaded = false;

AR.context.onLocationChanged = World.locationChanged;

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
        if(m_Marks[i].id == mark.id){
            return true;
        }        
    }  
    return false;
  
}

function deleteOldMarks(){
    for (var i = 0 ; i < m_ShowedMarks.length ; i++) {
        var markData = m_ShowedMarks[i].markData;
        if( (markInNewList(markData)==false) || (m_ShowedMarks[i].markerLocation.distanceToUser()>50)){     
            m_ShowedMarks[i].markerObject.enabled = false;
            m_ShowedMarks[i].deleted = true;
            m_ShowedMarks.splice(i, 1);
            i--;
        }
    }
}

function showMarks(){
    for (var i = 0 ; i < m_Marks.length ; i++) {
        var mark = m_Marks[i];
        var altitude = mark.Altitude== 1? AR.CONST.UNKNOWN_ALTITUDE: mark.Altitude;
        var markerLocation = new AR.GeoLocation(mark.Latitude, mark.Longitude, altitude);
        if((!markIsShowed(mark)) && (markerLocation.distanceToUser() <= 50))
         {
              var markData = {
                  "id": mark.id,
                  "longitude": mark.Longitude,
                  "latitude": mark.Latitude,
                  "altitude": altitude,
                  "message": mark.Message,
                  "style": mark.Style

             };
             World.loadMarksFromJsonData(markData, markerLocation);                        
         }
      }
}