
// JavaScript source code
function DrawCircle(lon, lat,context) {
    context.beginPath();
    context.lineWidth = 1;
    context.arc(lon, lat, 6, 0, 2 * Math.PI);
    context.fillStyle = 'red';
    context.fill();
    context.stroke();
}

function DrawLine(locations, context) {
    context.moveTo(locations[0].x, locations[0].y);
    for (var i = 0; i< locations.length; i++) {
        context.lineTo(locations[i].x, locations[i].y);
    }
    
    context.strokeStyle = 'red';
    context.stroke();
}

function DrawRoute(context, lon, lat, locations) {
    var lonn = (lon + 180) * (window.innerWidth / 360);
    var latn = (lat + -90) * (window.innerHeight / -180);
    var point = { x: lonn, y: latn };
    locations.push(point);
    DrawCircle(locations[0].x, locations[0].y, context);
    DrawLine(locations, context);   
}