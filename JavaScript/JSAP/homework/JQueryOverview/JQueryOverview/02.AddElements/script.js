﻿// Generated with CoffeeScript 1.4.0
(function () {
    var action, el, operations, _fn;

    operations = {
        '.js-before': 'insertBefore',
        '.js-after': 'insertAfter'
    };

    _fn = function (action) {
        return $(el).click(function () {
            return $('<div class="box" />')[action]($('#js-main'));
        });
    };
    for (el in operations) {
        action = operations[el];
        _fn(action);
    }

}).call(this);