$(document).ready(function () {
    //set the counter once all images have loaded
    $(window).load(function () {
        //TODO: make the animation increment faster for larger numbers
        //      idea: accelerate animation by increasingly setting delays of zero
        //SetCounter(CounterValue, 6);
    });

    //create the chart
    CreatePlot();

    //initialize the test console
    InitTestConsole();

    //recursive funtion to animate the counter,
    //starting from 0 and counting up to the final value
    function SetCounter(value, digit) {
        var classString = Array(digit + 1).join("x");
        var digitValue = Math.floor(value / Math.pow(10, digit - 1));

        //iterate recursively up to value - 1
        for (var i = 1 ; i < digitValue ; i++) {
            //iterate the digits to the right of the current digit up to 999.... and then back to 0
            if (digit > 1) {
                SetCounter(Array(digit).join("9"), digit - 1);                        //set counter to all 9's after the current digit
                flipDigits($(".counter-wrapper .number").slice(6 - digit), 0);    //set counter to all 0's after the current digit
            }

            //increment the current digit
            flipDigits("." + classString, i);
        }

        //set all digits to the right of the current digit to their final values
        if (digit > 1) {
            SetCounter(value % Math.pow(10, digit - 1), digit - 1);
        }

        //set the current digit to its final value
        flipDigits("." + classString, digitValue);

        function flipDigits(elements, value) {
            //add the animation and the a short delay to the animation queue
            $(".counter-wrapper").queue(function (next) {
                $(elements).text(value);
                next();
            }).delay(5);
        }
    }

    function CreatePlot() {
        $.jqplot('chartdiv', chartData, {
            title: 'Foursquare Checkins',
            axes: {
                xaxis: {
                    label: "Month",
                    renderer: $.jqplot.DateAxisRenderer,
                    tickOptions: { formatString: '%b %y' },
                    tickInterval: '1 month'
                },
                yaxis: {
                    label: "# Of Checkins"
                }
            },
            series: [{ color: '#5FAB78' }]
        });
    }

    function InitTestConsole() {
        //update the testing value on the input event
        $("#testContainer input[name='value']").on("input", (function () {
            //break if testing has not been started
            var formElem = $("#testContainer form");
            if (formElem.find("input[name='start']").val() === "True") {
                return;
            }

            //validate the form
            formElem.validate({
                rules: {
                    value: {
                        required: true,
                        number: true,
                        range: [0, 999999]
                    }
                }
            });

            //break if the form is not valid
            //TODO: make this work
            if (!formElem.valid()) {
                return;
            }

            //TODO: pad the number with zeros

            //get the data to send from the existing form
            var data = formElem.serializeArray();
            var startedIndex;
            $.each(data, function (index, value) {
                if (value.name === "start") {
                    startedIndex = index;
                    return false;
                }
            });

            //send the update
            data[startedIndex].value = "True";
            var url = formElem.attr('action');
            $.post(url, data, null);
        }));
    }
});

function TestFormBegin() {
    $("#testContainer input[type='submit']").block();
}

function TestFormSuccess() {
    var startInput = $("#testContainer input[name='start']");
    var submitInput = $("#testContainer input[type='submit']");
    var started = startInput.val() === "True";

    startInput.val(started ? "False" : "True");
    submitInput
        .val(started ? "■" : "►")
        .css('background-color', started ? "#DD5444" : "green");
}

function TestFormFailure() {
    alert("Sorry, an error or occured. Ensure you have entered a valid value and try again.");
}

function TestFormComplete() {
    $("#testContainer input[type='submit']").unblock();
}