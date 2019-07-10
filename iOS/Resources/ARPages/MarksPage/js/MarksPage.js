var loaded = false;

function locationChanged(lat, lon, alt, acc){
    if(!loaded){

        var marker_image = new AR.ImageResource("assets/woodSign.png");
        var marker_loc = new AR.GeoLocation(lat+0.05, lon, alt);
        var marker_drawable = new AR.ImageDrawable(marker_image, 8);
        var marker_object = new AR.GeoObject(marker_loc, {
            drawables:{
                cam: [marker_drawable]
                }
        });
        loaded = true;
     }
        
}

AR.context.onLocationChanged = locationChanged;
