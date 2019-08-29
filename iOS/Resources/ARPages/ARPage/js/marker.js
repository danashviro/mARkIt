function Marker(markData) {

    this.markData = markData;
    this.deleted = false;
    var markerLocation = new AR.GeoLocation(markData.latitude, markData.longitude, markData.altitude);
    //var markerLocation = new AR.RelativeLocation(null, 5, 0, 1);

    this.markerDrawable_idle = new AR.ImageDrawable(World.markerDrawable_idle, 2.5, {
        zOrder: 0,
        opacity: 1.0
    });
    var labelHeight = (markData.message.length/13)  * 0.5;

    this.descriptionLabel = new AR.Label(markData.message.trunc(13), labelHeight, {
        zOrder: 1,
        style: {
            textColor: '#FFFFFF'
        }
    });

     var indicatorImage = new AR.ImageResource("assets/indi.png");

        var indicatorDrawable = new AR.ImageDrawable(indicatorImage, 0.1, {
            verticalAnchor: AR.CONST.VERTICAL_ANCHOR.TOP
        });

    this.markerObject = new AR.GeoObject(markerLocation, {
        drawables: {
            cam: [this.markerDrawable_idle, this.descriptionLabel],
            indicator: [indicatorDrawable]
        },
        onClick: function () {
            AR.platform.sendJSONObject({ "option": "rate", "markId": markData.id });
        }
    });

    return this;
}


String.prototype.trunc = function(n) {
    var size = this.length; 
    var charsLeft = this.length;
    var str = new String();
    str = this.substr(0,n-1);
    charsLeft -= n;

    while(charsLeft > 0){
      str += '\n';
      str += this.substr(size - charsLeft ,n -1);
      charsLeft -= n;
    }
    return str;
};