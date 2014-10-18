$(function () { // document ready, resize container

    var canvas = document.getElementById("canvas");
    var ctx = canvas.getContext("2d");

    var rc = 0;  // resize counter
    var oc = 0;  // orientiation counter
    var ios = navigator.userAgent.match(/(iPhone)|(iPod)/); // is iPhone

    function orientationChange() {
        // inc orientation counter
        oc++;
    }
    function resizeCanvas() {
        // inc resize counter
        rc++;

        if (ios) {
            // increase height to get rid off ios address bar
            $("#container").height($(window).height() + 60)
        }

        var width = $("#container").width();
        var height = $("#container").height();

        cheight = height - 20; // subtract the fix height
        cwidth = width;

        // set canvas width and height
        $("#canvas").attr('width', cwidth);
        $("#canvas").attr('height', cheight)

        // hides the WebKit url bar
        if (ios) {
            setTimeout(function () {
                window.scrollTo(0, 1);
            }, 100);
        }

        // write number of orientation changes and resize events
        ctx.fillStyle = 'white';
        ctx.textAlign = 'center';
        ctx.fillText('Orientiation changes: ' + oc, cwidth / 2, cheight / 2);
        ctx.fillText('Resize events: ' + rc, cwidth / 2, cheight / 2 + 10);
        drawcanvas();
    }

    // Install resize and orientation change handlers. Note Android may firef both
    // resize and orientation changes when rotating.
    var resizeTimeout;
    $(window).resize(function () {
        clearTimeout(resizeTimeout);
        resizeTimeout = setTimeout(resizeCanvas, 100);
    });
    resizeCanvas();

    var otimeout;
    window.onorientationchange = function () {
        clearTimeout(otimeout);
        otimeout = setTimeout(orientationChange, 50);
    }
});