$(document).ready(function () {
    HandleNav();

    $(document).scroll(function() {
        HandleNav();
    });

    $('.select').select2({
        width: '100%',
        minimumResultsForSearch: 10
    });


    var swiper = new Swiper('.swiper-container', {
        slidesPerView: 2,
        spaceBetween: 30,
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
        loop: true,
        navigation: {
            nextEl: 'next',
            prevEl: null
        }
    });

    // RevSlider
    var slider = jQuery('#rev_slider_1').show().revolution({

        /* options are 'auto', 'fullwidth' or 'fullscreen' */
        sliderLayout: 'fullwidth',
        responsiveLevels: [1240, 1024, 778, 480],
        gridheight: 600,

        /* basic navigation arrows and bullets */
        navigation: {
            bullets: {
                enable: true,
                style: 'hesperiden',
                hide_onleave: true,
                h_align: 'center',
                v_align: 'bottom',
                h_offset: 0,
                v_offset: 20,
                space: 5
            }
        }
    });

    var $grid = $('.grid').masonry({
        // options
        itemSelector: '.grid-item',
        columnWidth: '.grid-item',
        percentagePosition: true
    });

    //$grid.imagesLoaded().progress(function () {
    //    $grid.masonry('layout');
    //});
});

function HandleNav() {
    var top = $(document).scrollTop();
    if (top >= 200) {
        if (!$('#homenav').hasClass('sticky')) {
            $('#homenav').addClass('sticky');
            $('#header-placeholder').show();
        }
    }
    else {
        $('#homenav').removeClass('sticky');
        $('#header-placeholder').hide();
    }
}