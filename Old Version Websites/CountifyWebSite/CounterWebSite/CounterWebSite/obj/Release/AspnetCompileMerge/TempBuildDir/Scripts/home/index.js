$(document).ready(function () {

    //TODO: use this to track where the BlockUI elements are being modified

    //orig = $.fn.css;

    //$.fn.css = function () {
    //    $.fn.css = function () {
    //        orig.apply(this, arguments);

    //        if (this.hasClass('blockUI')) {
    //            console.log(arguments)
    //            //log stack trace
    //            var e = new Error('dummy');
    //            var stack = e.stack.replace(/^[^\(]+?[\n$]/gm, '')
    //                .replace(/^\s+at\s+/gm, '')
    //                .replace(/^Object.<anonymous>\s*\(/gm, '{anonymous}()@')
    //                .split('\n');
    //            console.log(stack);
    //        }
    //    }
    //}

    //handle image loading
    //Loader();

    //execute responsive code
    Responsive();

    //create image slider
    CreateSlider();

    //create sticky toolbar
    $('header').stickytoolbar({ onStaticClass: 'sticktoolbar-static' });

    //create animations for menu links
    $("header a.deneyimi").click(function (e) {
        SlideToElement(e, "header", 0);
    });

    $("header a.nasil").click(function (e) {
        SlideToElement(e, "#nasil");
    });

    $("header a.siparis").click(function (e) {
        SlideToElement(e, "#order-container");
    });

    //slide down when clicking on the slider
    $("#main-img").click(function (e) {
        SlideToElement(e, "header", 0);
    });
    $("#main-img .slides-pagination a").click(function (e) { e.stopPropagation(); });

    //slide up when clickin on the logo
    $("header .float-left a").click(function (e) {
        SlideToElement(e, "body", 0);
    });

    //TODO: scroll video into view on click

    //wait for 'how it works' icons to load, then center them
    var waitCount = $(".content .how-works .col img").length;
    $(".content .how-works .col img").load(function () {
        waitCount -= 1;
        if (waitCount === 0) {
            var middleHeight = $(".content .how-works .col.middle img").height();
            var sideHeight = $(".content .how-works .col.side img").first().height();
            $(".content .how-works .col.side").css("margin-top", (middleHeight / 2 - sideHeight / 2) + 'px');
        }
    });

    //click button to show the order form
    $("#order-container > button").click(function () {
        $(this).slideUp();
        $("#order-form-container").slideDown(function () {
            SetTextAreaHeight();
        });
    });

    //manage the overlay and progress bar on page load
    function Loader() {
        //block the screen
        $.blockUI({
            message: null,
            baseZ: '99',            //just behind the NProgress bar
            css: {
                border: 'none'
            },
            overlayCSS: {
                backgroundColor: 'white',
                opacity: '1'
            },
            onUnblock: function (event, options) {
                //start slider animation after 5 seconds
                window.setTimeout(function () {
                    $('#slides').superslides('start');
                }, 5000);
            }
        });

        //prevent scrolling while loading (TODO: enable once blockUI starts working)
        //$('html, body').css({
        //    overflow: 'hidden',
        //    height: '100%'
        //});
        //window.scrollTo(0,0);

        //start the loader
        NProgress.configure({
            showSpinner: false,
            doneCallback: function () {
                $.unblockUI();
                $('html, body').css({
                    overflow: 'auto',
                    height: 'auto'
                });
            }
        });
        NProgress.start();

        //get all images on the page
        var images = $(document).find("img");
        var imgCount = images.length;
        var loaded = 0;

        //update loading progress on each image load
        images.load(function (e) {
            loaded++;
            var progress = Math.max((loaded / imgCount), NProgress.status);
            NProgress.set(progress);
        });

        //also finish on window load in case something goes wrong with loading callbacks
        $(window).load(function () {
            //NProgress.done();
        });
    }

    //callback for screen resize or orientationChange
    function Responsive() {
        //bind responsive code to veiwport size changes
        if (!navigator.userAgent.match(/(iPhone|iPod|iPad|BlackBerry|IEMobile)/)) {
            $(window).bind('resize', Responsive);
        }
        else {
            $(window).bind("orientationchange", Responsive);
        }

        var screenWidth = $(window).width();
        var screenHeight = $(window).height();
        var toolBarHeight = $("header").outerHeight();
        var contentWidth = $(".content-wrapper").first().width();

        //set the slider to the width and height of the viewport
        $('#main-img').width(Math.max(screenWidth, contentWidth));
        $('#main-img').height(screenHeight);

        //set the video width so it doesn't overflow the height (minimum width 500px)
        var videoWidth = Math.min((screenHeight - toolBarHeight - 50) * (16 / 9), contentWidth);
        videoWidth = Math.max(videoWidth, 500);
        $(".video-container").width(videoWidth);
    }

    //creat the image slider
    function CreateSlider() {
        //create the slider
        $('#slides').superslides({
            play: 5000,
            animation: 'slide',
            speed: 'slow',
            inherit_width_from: '.wide-container',
            inherit_height_from: '.wide-container'
        });

        //$('#slides').superslides('stop');   //will be started when all images have loaded

        //TODO: add a wrapper around the nav that looks nice (line below breaks the nav)
        //$('#slides').children(".slides-pagination").wrapInner("<div class=slides-pagination-wrapper>");
    }

    function SlideToElement(event, element, offset) {
        var defaultOffset = 30 + $('header').height();
        offset = typeof offset !== 'undefined' ? offset : defaultOffset;

        event.preventDefault();
        $('html, body').animate({
            scrollTop: $(element).offset().top - offset
        }, 1000);
    }
});

//order form ajax methods
function PreOrderFormSubmitBegin() {
    var submitButton = $("#order-form-container input[type='submit']");

    //save data to be restored later
    submitButton.data({
        'originalVal': submitButton.val(),
        'originalColor': submitButton.css('background-color')
    });

    //update submit button
    submitButton.block({})
        .val("Gönderiliyor...")
        .css("background-color", "grey");
}
function PreOrderFormSubmitFailure() {
    alert("We're sorry, your order failed to submit. Please try again later.");
    var submitButton = $("#order-form-container input[type='submit']");

    //restore submit button
    $("#order-form-container input[type='submit']")
        .unblock()
        .val(submitButton.data('originalVal'))
        .css('background-color', submitButton.data('originalColor'));
}
function PreOrderFormSubmitComplete() {
}

//subscription form ajax methods
function NotifySubscribeBegin() {
    //$("#order-form-container").block({});
}
function NotifySubscribeFailure() {
    alert("We're sorry, but your email could not be registered for notifications. Please try again later");
}
function NotifySubscribeComplete() {
    //$("#order-form-container").unblock();
}