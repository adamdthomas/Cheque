
function CountDownTimer(duration, granularity) {
    this.duration = duration;
    this.granularity = granularity || 1000;
    this.tickFtns = [];
    this.running = false;
    this.timer = false;
}

CountDownTimer.prototype.start = function () {
    if (this.running) {
        return;
    }
    this.running = true;
    var start = Date.now(),
        that = this,
        diff, obj;

    (function timer() {
        diff = that.duration - (((Date.now() - start) / 1000) | 0);

        if (diff > 0) {
            that.timer = setTimeout(timer, that.granularity);
        } else {
            diff = 0;
            that.running = false;
        }

        obj = CountDownTimer.parse(diff);
        that.tickFtns.forEach(function (ftn) {
            ftn.call(this, obj.minutes, obj.seconds, obj.hours);
        }, that);
    }());
};

CountDownTimer.prototype.onTick = function (ftn) {
    if (typeof ftn === 'function') {
        this.tickFtns.push(ftn);
    }
    return this;
};

CountDownTimer.prototype.stop = function () {
    return clearTimeout(this.timer);
};

CountDownTimer.prototype.expired = function () {
    return !this.running;
};

CountDownTimer.parse = function (seconds) {

    var min = (seconds / 60) | 0,
      minu = 0,
      sec = (seconds % 60) | 0,
      hor = (min / 60) | 0;

    if (min > 59) {
        minu = min - (hor * 60);
    } else {
        minu = min;
        hor = 0;
    }

    if (sec < 0) {
        sec = 0;
    }

    return {
        'minutes': minu,
        'seconds': sec,
        'hours': hor
    };
};

function format(display) {
    return function (minutes, seconds, hours) {
        minutes = minutes < 10 ? "0" + minutes : minutes;
        seconds = seconds < 10 ? "0" + seconds : seconds;
        hours = hours < 10 ? "0" + hours : hours;
        display.textContent = hours + ":" + minutes + ':' + seconds;
    };
}


function ToSec(milliseconds) {

    secLeft = milliseconds / 1000;
    if (secLeft < 0) {
        secLeft = 0;
    }
    return secLeft;
}

function handleTime(rNum, time) {
    $.ajax({
        url: '@Url.Action("HandleTime", "home")',
        data: { relay: rNum, time: time }

    });
}
