
/*Adding and removing class footer-positioning */

$(document).ready(function () {

    $('#btnSearch').click(function () {

        $('footer').addClass('footer-positioning');
    });

    $('#btnBack').click(function () {

        $('footer').removeClass('footer-positioning');
    });

});