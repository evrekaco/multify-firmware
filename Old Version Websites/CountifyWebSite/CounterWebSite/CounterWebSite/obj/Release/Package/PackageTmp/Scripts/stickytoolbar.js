/**
 * This plugin is basically designed to allow some one to widgetise the handling of sticky toolbars from the example on
 * http://www.bennadel.com/blog/1810-Creating-A-Sometimes-Fixed-Position-Element-With-jQuery.htm
 *
 * You can use this plugin like so (as an example I have also placed in a setting param):
 * 
 * $('.videos_toolbar').stickytoolbar({ onStaticClass: 'sticktoolbar-static' });
 *
 * Using this html structure:
 * <div class='stickytoolbar-placeholder'>
 *    <div class='stickytoolbar-bar'>
 *    </div>
 *  </div>
 */

(function ($) {
    $.fn.stickytoolbar = function (options) {

        $(this).addClass(options.onStaticClass).data('options', $.extend({
            'onStaticClass': '',
            'onFixedClass': ''
        }, options));

        $(window).bind(
            'scroll resize orientationchange', function (e) {
                $('.stickytoolbar-placeholder').each(function () {
                    var options = $(this).parent().data("options");

                    // Get the current offset of the placeholder.
                    // Since the message might be in fixed
                    // position, it is the plcaeholder that will
                    // give us reliable offsets.
                    placeholderTop = $(this).offset().top;

                    // Gets the message element so it can be used
                    bar = $(this).find('.stickytoolbar-bar');

                    // Get the current scroll of the window.
                    viewTop = $(window).scrollTop();

                    // Check to see if the view had scroll down
                    // past the top of the placeholder AND that
                    // the message is not yet fixed.
                    if ((viewTop > placeholderTop) && !bar.hasClass('stickytoolbar-fixed')) {

                        // The message needs to be fixed. Before
                        // we change its positon, we need to re-
                        // adjust the placeholder height to keep
                        // the same space as the message.
                        //
                        // NOTE: All we're doing here is going
                        // from auto height to explicit height.
                        $(this).height($(this).height());

                        // Make the message fixed.
                        bar.addClass('stickytoolbar-fixed')
                            .css({
                                left: $(this).offset().left + 'px',
                                top: '0px',
                                position: 'fixed',
                                width: $(this).width() + 'px',
                            })
                            .removeClass(options.onStaticClass)
                            .addClass(options.onFixedClass);

                        

                        // Check to see if the view has scroll back up
                        // above the message AND that the message is
                        // currently fixed.
                    } else if ((viewTop <= placeholderTop) && bar.hasClass('stickytoolbar-fixed')) {

                        // Make the placeholder height auto again.
                        $(this).css({ 'height': 'auto', 'width': 'auto' });

                        // Remove the fixed position class on the
                        // message. This will pop it back into its
                        // static position.
                        bar.removeClass('stickytoolbar-fixed')
                            .css({
                                left: 'auto',
                                top: 'auto',
                                position: 'static',
                                width: 'auto',
                                'margin-left': 'auto',
                                'margin-right': 'auto'
                            })
                            .removeClass(options.onFixedClass)
                            .addClass(options.onStaticClass);
                    }
                    else if(bar.hasClass('stickytoolbar-fixed') && (e.type == "resize" || e.type == "orientationchange"))
                    {
                        //reset the bar width
                        bar.width($(this).width());
                    }
                });
            }
        );
    };
})(jQuery);