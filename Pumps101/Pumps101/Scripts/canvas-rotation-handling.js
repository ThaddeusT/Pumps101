$(function () { // document ready, resize container

    var canvas = document.getElementById("canvas");
    var ctx = canvas.getContext("2d");
    var orientation = "";
    var rc = 0;  // resize counter
    var oc = 0;  // orientiation counter
    var ios = navigator.userAgent.match(/(iPhone)|(iPod)/); // is iPhone 
    var last_width = 0;
    var last_height = 1;

    function resizeCanvas() {
        // inc resize counter
        rc++;

        if (ios) {
            // increase height to get rid off ios address bar
            $("#container").height($(window).height() + 60)
        }

        var width = $("#container").width();
        var height = $("#container").height();

        if (width != last_width && width != last_height) {
            last_width = width;
            last_height = height;
            cheight = height; // subtract the fix height
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
            if (orientation == "Portrait") {
                ctx.beginPath();
                ctx.lineWidth = "4";
                ctx.strokeStyle = "purple";
                ctx.fillStyle = 'purple';
                ctx.rect(0, 0, cwidth, cheight);
                ctx.fill();
                ctx.fillStyle = 'white';
                ctx.textAlign = 'center';
                ctx.fillText('Please Rotate Device To Landscape', cwidth / 2, cheight / 2);
                $("#HPGuess").hide();
                $("label").hide();
                $("#controls").hide();
            }
            else {
                drawcanvas();
                console.log("drew Canvas");
                $("#HPGuess").show();
                $("label").show();
                $("#controls").show();
            }
        }
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

    function toggleFullScreen() {
        var doc = window.document;
        var docEl = doc.documentElement;

        var requestFullScreen = docEl.requestFullscreen || docEl.mozRequestFullScreen || docEl.webkitRequestFullScreen || docEl.msRequestFullscreen;
        var cancelFullScreen = doc.exitFullscreen || doc.mozCancelFullScreen || doc.webkitExitFullscreen || doc.msExitFullscreen;

        if (!doc.fullscreenElement && !doc.mozFullScreenElement && !doc.webkitFullscreenElement && !doc.msFullscreenElement) {
            requestFullScreen.call(docEl);
        }
        else {
            cancelFullScreen.call(doc);
        }
    }

    function hideAddressBar() {
        if (document.documentElement.scrollHeight < window.outerHeight / window.devicePixelRatio)
            document.documentElement.style.height = (window.outerHeight / window.devicePixelRatio) + 'px';
        setTimeout(window.scrollTo(1, 1), 0);
    }
    window.addEventListener("load", function () {
        switch (window.orientation) {
            case 0:
                orientation = "Portrait";
                break;
            case 180:
                orientation = "Portrait";
                break;
            case -90:
                orientation = "Landscape";
                break;
            case 90:
                orientation = "Landscape";
                break;
        }
        if (orientation == "Portrait") {
            ctx.beginPath();
            ctx.lineWidth = "4";
            ctx.strokeStyle = "purple";
            ctx.fillStyle = 'purple';
            ctx.rect(0, 0, cwidth, cheight);
            ctx.fill();
            ctx.fillStyle = 'white';
            ctx.textAlign = 'center';
            ctx.fillText('Please Rotate Device To Landscape', cwidth / 2, cheight / 2);
            $("#HPGuess").hide();
            $("label").hide();
            $("#controls").hide();
        }
        else {
            drawcanvas();
            console.log("drew Canvas");
            $("#HPGuess").show();
            $("label").show();
            $("#controls").show();
        }
        hideAddressBar();
        toggleFullScreen();
    });

    window.onorientationchange = function () {
        switch (window.orientation)
        {
            case 0:
                orientation = "Portrait";
                break;
            case 180:
                orientation = "Portrait";
                break;
            case -90:
                orientation = "Landscape";
                break;
            case 90:
                orientation = "Landscape";
                break;
        }
        hideAddressBar();
        toggleFullScreen();
    }
});