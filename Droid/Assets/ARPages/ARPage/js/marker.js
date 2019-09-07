function Marker(markData,markerLocation) {

    this.markData = markData;
    this.deleted = false;
    this.markerLocation = markerLocation;
    //var markerLocation = new AR.RelativeLocation(null, 5, 0, 1);
    this.markerImage;
    var seen = false;
    
    if (markData.style == "Wood") {
        this.markerImage = new AR.ImageResource("assets/woodSign.png");
    } else if (markData.style == "Metal") {
        this.markerImage = new AR.ImageResource("assets/metalSign.png");             
    } else {
        this.markerImage = new AR.ImageResource("assets/schoolSign.png");      
    }
    var drawables = new Array();

    drawables.push(new AR.ImageDrawable(this.markerImage, 2.5, {
        zOrder: 0,
        opacity: 1.0
    }));
    for(var i=0;i<markData.message.length/13;i++){
        drawables.push(new AR.Label(markData.message.substr(i*13,13), 0.5, {
            zOrder: 1,
            style: {
                textColor: '#FFFFFF'
            },
            offsetY: 0.5 - 0.5*i
        }));
    }

     var indicatorImage = new AR.ImageResource("assets/indi.png");

        var indicatorDrawable = new AR.ImageDrawable(indicatorImage, 0.1, {
            verticalAnchor: AR.CONST.VERTICAL_ANCHOR.TOP
        });

    this.markerObject = new AR.GeoObject(markerLocation, {
        drawables: {
            cam: drawables, 
            indicator: [indicatorDrawable]
        },
        onClick: function () {
            AR.platform.sendJSONObject({ "option": "rate", "markId": markData.id });
        },
        onEnterFieldOfVision: function () {
            if (seen == false) {
                seen = true;
                AR.platform.sendJSONObject({ "option": "seen", "markId": markData.id });
            }
        } 
    });

    return this;
}
