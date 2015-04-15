$(document).ready(function () {

});

//subscription ajax update callback methods
function UpdateAccountBegin() {
    $("#manage-account-wrapper input[type='submit']").block({});
}
function UpdateAccountFailure() {
    alert("Sorry, the account update failed. Please try again later.");
}
function UpdateAccountComplete() {
    $("#manage-account-wrapper input[type='submit']").unblock();

    window.setTimeout(function () {
        $("#manage-account-wrapper .ajax-status .Success").fadeOut("slow");
    }, 2000)
    
}