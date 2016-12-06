//this code will make sure that each listing-item is the same height
$(document).ready(function () {
    var largestHeight = 0;
    var elements = $(".listing-item")
    elements.each(function (index) {
        var elementHeight = $(this).height();
        if (largestHeight < elementHeight) {
            largestHeight = elementHeight
        }
    });

    elements.each(function (index) {
        $(this).height(largestHeight);
    });
    //console.log("StandardizeHeight.js ran");
});