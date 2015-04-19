(function () {

    $('#live-chat header').on('click', function () {

        $('.chat').slideToggle(300, 'swing');
        $('.chat-message-counter').fadeToggle(300, 'swing');

    });

    $('.chat-close').on('click', function (e) {
        console.log("vetuvam ke se zatvoram");
        e.preventDefault();
        $('#live-chat').fadeOut(300);

    });

})();