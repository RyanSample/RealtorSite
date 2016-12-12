//this code will make sure that each listing-item is the same height
function getLargestHeight(element, largestHeight){
    var elementHeight = $(element).height();
    if (elementHeight > largestHeight) {
        return elementHeight;
    }
    return largestHeight;
}

$(document).ready(function () {
    var largestHeight = 0;
    var elements = $(".listing-item")
    
    console.log("Number of elements " + elements.length);
    elements.each(function (index) {
        $(this).ready(largestHeight = getLargestHeight(this, largestHeight));
        //console.log("largest height: " + largestHeight + " this element's height: " + $(this).height() + " item # " + index);
    });

    //console.log("largest height: " + largestHeight + " px");

    elements.each(function (index) {
        //console.log("Element # " + index);
        $(this).height(largestHeight);
    });
//    console.log("StandardizeHeight.js ran");
});