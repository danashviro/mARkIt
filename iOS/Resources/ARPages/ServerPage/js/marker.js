function Marker(markData) {

    this.markData = markData;

    //var markerLocation = new AR.GeoLocation(markData.latitude, markData.longitude, markData.altitude);
    var markerLocation = new AR.RelativeLocation(null, 5, 0, 1);

    this.markerDrawable_idle = new AR.ImageDrawable(World.markerDrawable_idle, 2.5, {
        zOrder: 0,
        opacity: 1.0
    });



    this.descriptionLabel = new AR.Label(markData.message.trunc(15), 0.5, {
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




// will truncate all strings longer than given max-length "n". e.g. "foobar".trunc(3) -> "foo..."
String.prototype.trunc = function(n) {
    return this.substr(0, n - 1) + (this.length > n ? '...' : '');
};