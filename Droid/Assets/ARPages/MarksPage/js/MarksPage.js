
function locationChanged(lat, lon, alt, acc){

        var marker_image = new AR.ImageResource("assets/d.png");
        var marker_loc = new AR.GeoLocation(lat+0.05, lon, alt);
        var marker_drawable = new AR.ImageDrawable(marker_image, 8);
        var marker_object = new AR.GeoObject(marker_loc, {
            drawables:{
                cam: [marker_drawable]
                }
        });
    
        
}

AR.context.onLocationChanged = locationChanged;
