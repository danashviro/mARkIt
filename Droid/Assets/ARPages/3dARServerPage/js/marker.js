function Marker(poiData) {



    this.poiData = poiData;
    var clicked = false;

    //var markerLocation = new AR.GeoLocation(poiData.latitude, poiData.longitude, poiData.altitude);
    var markerLocation = new AR.RelativeLocation(null, 5, 0, 1);
    var modelEarth = new AR.Model("assets/earth.wt3", {
            onLoaded: this.worldLoaded,
            scale: {
                x: 1,
                y: 1,
                z: 1
            },
            onClick : function() {
                if(!clicked)
                {
                    clicked = true;
                    this.markerDrawable_idle = new AR.ImageDrawable(World.markerDrawable_idle, 2.5, {
                        zOrder: 0,
                        opacity: 1.0
                    });
                                  
                    this.descriptionLabel = new AR.Label(poiData.description.trunc(15), 0.5, {
                        zOrder: 1,
                        style: {
                        textColor: '#FFFFFF'
                        }
                    });
        
                    this.markerObject = new AR.GeoObject(markerLocation, {
                        drawables: {
                            cam: [this.markerDrawable_idle, this.descriptionLabel]
                        }
                    });
                }
         }
        });

        var indicatorImage = new AR.ImageResource("assets/indi.png");

        var indicatorDrawable = new AR.ImageDrawable(indicatorImage, 0.1, {
            verticalAnchor: AR.CONST.VERTICAL_ANCHOR.TOP
        });
        
        var obj = new AR.GeoObject(markerLocation, {
            drawables: {
               cam: [modelEarth],
               indicator: [indicatorDrawable]
            }
        });

    return this;
}

String.prototype.trunc = function(n) {
    return this.substr(0, n - 1) + (this.length > n ? '...' : '');
};