$(document).ready(function () {

});

//subscription ajax update callback methods
function UpdateSubscriptionsBegin() {
    $("#manage-subs-wrapper input[type='submit']").block({});
}
function UpdateSubscriptionsFailure() {
    alert("Sorry, the subscription update failed. Please try again later.");
}
function UpdateSubscriptionsComplete() {
    $("#manage-subs-wrapper input[type='submit']").unblock();

    window.setTimeout(function () {
        $("#manage-subs-wrapper .ajax-status .Success").fadeOut("slow");
    }, 2000)
}