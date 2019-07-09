var World = {
    loaded: false,
    rotating: false,

    init: function initFn() {
        this.createModelAtLocation();
    },

    createModelAtLocation: function createModelAtLocationFn() {
    
        var location = new AR.RelativeLocation(null, 5, 0, 2);
        
        var modelEarth = new AR.Model("assets/earth.wt3", {
            onLoaded: this.worldLoaded,
            scale: {
                x: 1,
                y: 1,
                z: 1
            },
            onClick : function() {
                var marker_image = new AR.ImageResource("assets/woodSign.png");
                var marker_drawable = new AR.ImageDrawable(marker_image, 3);
                var markerlocation = new AR.RelativeLocation(null, 5, 0, 1);

                var marker_object = new AR.GeoObject(markerlocation, {
                drawables:{
                    cam: [marker_drawable]
                }
        });
         }
        });

        var indicatorImage = new AR.ImageResource("assets/indi.png");

        var indicatorDrawable = new AR.ImageDrawable(indicatorImage, 0.1, {
            verticalAnchor: AR.CONST.VERTICAL_ANCHOR.TOP
        });


        var obj = new AR.GeoObject(location, {
            drawables: {
               cam: [modelEarth],
               indicator: [indicatorDrawable]
            }
        });
    },

    worldLoaded: function worldLoadedFn() {
        World.loaded = true;
        var e = document.getElementById('loadingMessage');
        e.parentElement.removeChild(e);
    }
};

World.init();
