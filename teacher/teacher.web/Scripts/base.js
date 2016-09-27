(function ($) {
    $.setting = {
        //wsurl:"http://localhost:8077/WS/BGSPORT.asmx",
    },
    //禁用
    $.ban = {
        //禁用右键,需要再文档加载完毕时候使用
        banMouseRight: function (fn) {
            $("body").bind("contextmenu", function () { return false; });
            if (fn) {
                fn();
            }
        },
        //禁用全选
        banSelectAll: function (fn) {
            $("body").bind("selectstart", function () { return false; });
            if (fn) {
                fn();
            }
        },
        //禁用F12
        banF12: function (fn) {
            $("body").bind('keypress', function (e) {
                if (e.keyCode == 123) return false;
            });
        },
        //禁止所有
        banAll: function () {
            $.ban.banMouseRight();
            $.ban.banSelectAll();
            $.ban.banF12();
        }
    };
    
    var WebUntils=function(){};
    WebUntils.prototype={
        //获取今日日期
        today: function () {
            return this.dateFormat(new Date(),"yyyy-MM-dd");
        },
        //获取当前时间
        Now: function () {
            return this.dateFormat(new Date(), "yyyy-MM-dd HH:mm:ss");
        },
        MonthFirstDay: function (date) {
            var _date = !date?this.today():this.dateFormat(date, "yyyy-MM-dd");
            return this.dateAddDays(_date,-(new Date(_date).getDate()-1));
        },
        MonthLastDay: function (date) {
            var _date = this.MonthFirstDay(date);
            return this.dateAddDays(this.dateAddMonths(_date, 1), -1);
        },
        ///在现有时间上加上指定的天数，返回新的时间字符串
        dateAddDays: function (date, days) {
            if (arguments.length != 2) {
                console.warn("参数数目不合法");
                return;
            }
            if (typeof date !== "string") {
                console.warn("日期类型应为string类型");
                return;
            }
            var _date = new Date(date), dd = _date.getDate();
            _date.setDate(dd + days);
            return this.dateFormat(_date, "yyyy-MM-dd");
        },
        dateAddMonths: function (date, months) {
            if (arguments.length != 2) {
                console.warn("参数数目不合法");
                return;
            } 
            var _date = new Date(date),mm = _date.getMonth();
                _date.setMonth(mm + months);
                return this.dateFormat(_date, "yyyy-MM-dd");
        },
        changeDateFormat: function (cellval, timeformat) {
            if (!cellval) {
                return "";
            }
            if (cellval.indexOf("/Date") == 0) {
                d = new Date();
                var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
                var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                var type = "-";
                type = timeformat ? timeformat : type;
                return date.getFullYear() + type + month + type + currentDate;
            }
            else {
                return cellval;
            }
        },
        //将json中的DateTime转换成toLocaleTime
        changetoLocaleTimeString: function (cellval, timeformat) {
            if (!cellval) {
                return "";
            }
            d = new Date();
            var date = new Date(parseInt(cellval.substr(6)));
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
            var minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

            var type = "-";
            type = timeformat ? timeformat : type;
            return date.getFullYear() + type + month + type + currentDate + " " + hour + ":" + minute + ":" + seconds;
        },
        //将json中的DateTime转换成toLocaleTime
        changetoLocaleTimeString2hm: function (cellval, timeformat) {
            if (!cellval) {
                return "";
            }
            d = new Date();
            //localOffset = d.getTimezoneOffset() * 60000;
            //var date = new Date(parseInt(cellval.substr(6)) + localOffset);
            var date = new Date(parseInt(cellval.substr(6)));
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
            var minute = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            //var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();

            var type = "-";
            type = timeformat ? timeformat : type;
            return date.getFullYear() + type + month + type + currentDate + " " + hour + ":" + minute;
        },
        dateFormat: function (dataValue, fmtOpts) {
            //格式化选项匹配正则
            var formatOptions = {
                numeric: /N\d{1}/ig,
                percent: /P\d{1}/ig,
                currency: /C/ig,
                datetime: /yy(yy{0,1})|M{2}|dd|HH{1}|mm{1}|ss{1}/ig
            };
            //格式化日期
            if (fmtOpts.match(formatOptions.datetime) !== null) {
                var _dataValue = dataValue;
                var _dateParse = {
                    msJsonDate: /^\/Date\(\d*(|\+?\d*)\)\/$/ig,
                    stringDate: /^\d{4}[\.\/-]?[0-1]{1}[0-9]{1}(|[\.\/-]?[0-3]{1}[0-9]{1}[\.\/-]?|\s?[0-1]{1}[0-9]{1}|:?[0-1]{1}[0-9]{1}|:?[0-1]{1}[0-9]{1}|:?\d*)$/ig,
                    dtData: /^\d{8}\s\d{2}:\d{2}:\d{2}$/,
                    cludeTDate: /^\d{4}-\d{2}-\d{2}(T|t)\d{2}:\d{2}:\d{2}/
                }
                //判断是否为日期格式，不是需要转换成日期格式
                if (dataValue instanceof Date === false) {
                    //微软JSON日期格式，可带时区可不带
                    if (_dateParse.msJsonDate.test(dataValue)) {
                        var _regex = /^\/Date\(|\)\/$/ig;
                        var _dates = (dataValue + "").replace(_regex, "").split("+");
                        if (_dates.length > 1) {
                            _dataValue = new Date(parseInt(_dates[0]) + parseInt(_dates[1]));
                        }
                        else {
                            //添加本地时区
                            var d = new Date();
                            var localOffset = d.getTimezoneOffset() * 60000;
                            localOffset = 0;
                            _dataValue = new Date(parseInt(_dates[0]) + localOffset);
                        }
                    }
                        //判断是否为数字格式如：20101010101010
                    else if (_dateParse.stringDate.test(dataValue)) {
                        if ($.isNumeric(dataValue) === true) {
                            var _yy = (dataValue + "").substr(0, 4);
                            var _mm = (dataValue + "").substr(4, 2);
                            var _dd = (dataValue + "").substr(6, 2);
                            var _hh = (dataValue + "").substr(8, 2);
                            var _mi = (dataValue + "").substr(10, 2);
                            var _ss = (dataValue + "").substr(12, 2);
                            _dataValue = new Date(
                            _yy + "/"
                            + _mm + "/"
                            + (_dd === "" ? "01" : _dd) + " "
                            + (_hh === "" ? "00" : _hh) + ":"
                            + (_mi === "" ? "00" : _mi) + ":"
                            + (_ss === "" ? "00" : _ss));
                        }
                        else {
                            _dataValue = (dataValue + "").replace(/[-\.]/g, "/");
                            _dataValue = new Date(_dataValue);
                        }
                    } else if (_dateParse.dtData.test(dataValue)) {
                        var _yy = (dataValue + "").substr(0, 4);
                        var _mm = (dataValue + "").substr(4, 2);
                        var _dd = (dataValue + "").substr(6, 2);
                        var _hh = (dataValue + "").substr(8);
                        _dataValue = new Date(
                            _yy + "/"
                            + _mm + "/" + _dd + _hh);
                    } else if (_dateParse.cludeTDate.test(dataValue)) {
                        var _yy = (dataValue + "").substr(0, 4);
                        var _mm = (dataValue + "").substr(5, 2);
                        var _dd = (dataValue + "").substr(8, 2);
                        var _hh = (dataValue + "").substr(11);
                        _dataValue = new Date(
                            _yy + "/"
                            + _mm + "/" + _dd + " " + _hh);
                    }
                }
                //补零
                var zeroize = function (value, length) {
                    if (!length) {
                        length = 2;
                    }
                    value = new String(value);
                    for (var i = 0, zeros = ''; i < (length - value.length) ; i++) {
                        zeros += '0';
                    }
                    return zeros + value;
                };
                //是否已转成日期
                if (_dataValue instanceof Date) {
                    return fmtOpts.replace(formatOptions.datetime, function ($0) {
                        switch ($0) {
                            case 'dd': return zeroize(_dataValue.getDate());
                            case 'MM': return zeroize(_dataValue.getMonth() + 1);
                            case 'yy': return new String(_dataValue.getFullYear()).substr(2);
                            case 'yyyy': return _dataValue.getFullYear();
                            case 'HH': return zeroize(_dataValue.getHours());
                            case 'mm': return zeroize(_dataValue.getMinutes());
                            case 'ss': return zeroize(_dataValue.getSeconds());
                        }
                    });
                }
                else { return dataValue; }
            }
            else { return dataValue; }
        },
        //获取地址栏参数
        getUrlParam: function (name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) {
                return unescape(r[2]);
            }
            return null; //返回参数值
        },
        //判断是否为空或null
        IsNullOrEmpty: function (_v) {
            if (_v) {
                var ret;
                if (_v == null || _v == "") {
                    ret = true;
                }
                else {
                    ret = false;
                }
                return ret;
            }
        },
        //字符串截取
        Substr: function (_str,_len,_t) {
            if (_str&&_len) {
                return _str.length > _len ? _str.substr(0, _len) +(_t||""): _str;
            }
            return "";
        },
        //加载数据动画
        AddLoading: function (_txt) {
            if ($('#wxloading').length == 0) {
                $('<div>', { id: 'wxloading' })
                    .append(
                        $('<div>', { 'class': 'wx_loading_inner' })
                            .append($('<svg>', { 'class': 'wx_loading_icon' }))
                            .append($('<span>').text(_txt ? _txt : "正在加载..."))
                    ).appendTo($('body'));
            }
            else {
                $('#wxloading').show();
            }
        },
        //移除加载动画
        RemoveLoading: function () {
            $('#wxloading').remove();
        },
        AddLoadingSetClose: function (_txt, _fn, _time) {
            $.web.AddLoading(_txt);
            setTimeout(function () {
                if (_fn) {
                    _fn();
                }
                $.web.RemoveLoading();
            }, _time || 1000);
        },
        toast: function (_option) {
            switch (typeof _option) {
                case "string":
                    _option = { text: _option };
                    break;
                case "object":
                    break;
                default:
                    console.warn("toast参数不合法，必须是object或者string类型");
                    return;
            }
            var _config = {
                text: "",
                duration:3000
            };
            var ScreenWidth = $(window).width();
            $.extend(_config,_option);
            $('#wxtoast').length == 0 ? "" : $('#wxtoast').stop().unbind().remove();
            $('body').append($('<div>', { id: 'wxtoast' }).hide().text(_config.text));
            var $toast=$('#wxtoast');
            $toast.css("left", (ScreenWidth - $toast.width()) / 2);
            $("#wxtoast").show().fadeOut(_config.duration, function () {
                $("#wxtoast").remove();
            });
            
        },
        //同步
        get: function (_option,_data) {
            try {
                var ajaxconfig =
                                {
                                    type: "GET",
                                    contentType: "application/json;application/x-www-form-urlencoded;charset=utf-8",
                                    dataType: 'json',
                                    async: false,
                                };
                if (!_option) { console.warn("参数不可为空！"); return; }
                switch (typeof _option) {
                    case "string":
                        ajaxconfig.url = encodeURI(_option);//地址栏里转码
                        ajaxconfig.data = _data || {};
                        break;
                    case "object":
                        _option.url = encodeURI(_option.url);//地址栏里转码
                        $.extend(ajaxconfig, _option);
                        break;
                    default:
                        console.warn("参数类型不正确，必须是string,或者object");
                        return;

                }

                return $.ajax(ajaxconfig);
            }
            catch (e) {
                console.error(e.message);
            }

        },
        //异步
        getAsync: function (_option, _data) {
            try {
                var ajaxconfig =
                                {
                                    type: "GET",
                                    contentType: "application/json;application/x-www-form-urlencoded;charset=utf-8",
                                    dataType: 'json'
                                };
                if (!_option) { console.warn("参数不可为空！"); return; }
                switch (typeof _option) {
                    case "string":
                        ajaxconfig.url = encodeURI(_option);//地址栏里转码
                        ajaxconfig.data = _data || {};
                        break;
                    case "object":
                        _option.url = encodeURI(_option.url);//地址栏里转码
                        $.extend(ajaxconfig, _option);
                        break;
                    default:
                        console.warn("参数类型不正确，必须是string,或者object");
                        return;

                }

                return $.ajax(ajaxconfig);
            }
            catch (e) {
                console.error(e.message);
            }

        },
        //同步
        post: function (_option,_data) {
            try
            {
                if (!_option) { console.warn("参数不可为空！"); return; }
                var ajaxconfig =
                {
                    type: "POST",
                    async: false,
                    contentType: "application/json;application/x-www-form-urlencoded;charset=utf-8",
                    dataType: 'json'
                };

                switch (typeof _option) {
                    case "object":
                        $.extend(ajaxconfig, _option);
                        break;
                    case "string":
                        ajaxconfig.data = _data || {};
                        ajaxconfig.url = _option;
                        break;
                    default:
                        console.warn("参数类型不合法");
                        return;
                }

                ajaxconfig.url = encodeURI(ajaxconfig.url);//地址栏里转码
                ajaxconfig.data = JSON.stringify(ajaxconfig.data);//序列化
                return $.ajax(ajaxconfig);
            }
            catch (e)
            {
                console.error(e.message);
            }
        },
        //异步
        postAsync: function (_option, _data) {
            try {
                if (!_option) { console.warn("参数不可为空！"); return; }
                var ajaxconfig =
                {
                    type: "POST",
                    contentType: "application/json;application/x-www-form-urlencoded;charset=utf-8",
                    dataType: 'json'
                };
                switch (typeof _option) {
                    case "object":
                        $.extend(ajaxconfig, _option);
                        break;
                    case "string":
                        ajaxconfig.data = _data || {};
                        ajaxconfig.url = _option;
                        break;
                    default:
                        console.warn("参数类型不合法");
                        return;
                }

                ajaxconfig.url = encodeURI(ajaxconfig.url);//地址栏里转码
                ajaxconfig.data = JSON.stringify(ajaxconfig.data);//序列化
                return $.ajax(ajaxconfig);
            }
            catch (e)
            {
                console.error(e.message);
            }
        }
    };
    $.web = new WebUntils();
})(jQuery)