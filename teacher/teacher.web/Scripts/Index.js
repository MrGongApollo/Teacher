$(function () {

    $.web.getAsync(document.weburl + "Home/GetCurentUser").done(function (xhr) {
        switch (xhr.Ret.Key) {
            case "success":
                $("#txt_NickName").text(xhr.Data.UserNickName);
                break;
            case "error":
                swal("获取用户失败", xhr.Ret.Value, "error");
                break;

        }
    });

    var $bar = $("#left_bar");
    var _obj_a = {
        "href": "",
        "data-ajax-id": "Loadmainpage",
        "data-ajax-update": "#page-main",
        "data-ajax": "true",
        "data-ajax-success": "UpdatePage",
        "data-ajax-failure":"fnAjaxError",
        "data-href": ""
    };

    $.web.getAsync( document.weburl + "XT/GetIndexMenus").done(function (xhr) {
        switch (xhr.Ret.Key) {
            case "success":
                $.each(xhr.Data, function (index, item) {
                    if (item.children && item.children.length > 0) {
                        var $el = $("<li>", { "class": "has-sub" + (index == 0 ? " active" : "") }),
                            $ul = $("<ul>", { "class": "sub-menu" }).hide();
                        $.each(item.children, function (i, t) {
                            $.extend(_obj_a, { href:document.weburl+t.href, "data-href": t.href?document.weburl+t.href:"" });

                            $ul.append($("<li>").append(
                                $("<a>", _obj_a).append($("<i>", { "class": "menuIcon " + t.icon })).append($("<span>").text(t.title)).click(function () {
                                    $("#header_progress").stop(false, true).animate({ width: "80%" }, 2000, "swing");
                                })
                                ));
                        });

                        $bar.append($el.append(
                                $("<a>", { "href": "javascript:;" }).append($("<b>", { "class": "caret pull-right" })).append($("<i>", { "class": "menuIcon " + item.icon })).append($("<span>").text(item.title)).click(function (e) {
                                    var $parent = $(this).parent();
                                    if ($parent.hasClass('active')) {
                                        $parent.find(">ul").stop(true, false).slideToggle(300);

                                    }
                                    else {
                                        $parent.addClass('active').find(">ul").stop(true, false).slideDown(300).end().siblings('.active').removeClass('active').find(">ul").slideUp(300);
                                    }
                                })
                                ).append($ul)
                            );
                    }
                    else {
                        $.extend(_obj_a, { href: document.weburl + item.href, "data-href": item.href ? document.weburl + item.href : "" });
                        var $el = $("<li>", { "class": (index == 0 ? " active" : "") }).append(
                                $("<a>", _obj_a).append($("<i>", { "class": "menuIcon " + item.icon })).append($("<span>").text(item.title)).click(function () {
                                    var $parent = $(this).parent();
                                    if ($parent.hasClass('active')) {
                                        //return false;
                                    }
                                    else {
                                        //if (item.islink && item.href) {}
                                        $("#header_progress").stop(false, true).animate({ width: "80%" }, 2000, "swing");
                                        $parent.addClass('active').find(">ul").stop(true, false).slideDown(300).end().siblings('.active').removeClass('active').find(">ul").slideUp(300);
                                    }
                                })
                                );
                        $bar.append($el);
                    }
                });
                //$bar.find(">li").eq(0).find(">a").trigger('click');
                break;
            case "error":
                swal("错误", xhr.Ret.Value, "error");
                break;
        }
    });

    //注销
    $("#Signout").click(function () {
        swal({
            title: "确认注销当前用户",
            text: "注销后自动跳转登录界面",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "是",
            cancelButtonText: "否",
            closeOnConfirm: false
        }, function () {
            $.web.get(document.weburl + "Login/SignOut").done(function () {
                swal("注销成功", "1秒后自动跳转", "success");
                setTimeout(function () {
                    window.location.replace(document.weburl + "Login/Index");
                }, 500);
            });
        });

    });
});

//切换页面时候更新
function UpdatePage(data, status, xhr) {
    UpdateProgressBar();
    UpdateBreadcrumb();
   // UpdateMessage();
}



//更新进度条
function UpdateProgressBar() {
    $("#header_progress").stop(false, true).animate({ width: "100%" }, 500, function () {
        $(this).width(0);
    });
}

//更新面包屑
function UpdateBreadcrumb() {
    //更新当前地址

    if (window.curlocation) {
        var $breadcrumb = $("#page-breadcrumb");
        $breadcrumb.empty();
        $.each(window.curlocation, function (_i, _t) {
            switch (typeof _t) {
                case "string":
                    $breadcrumb.append(
              (_i == window.curlocation.length - 1 ? $("<li>", { "class": "active" }).text(_t) : $("<li>").append($("<a>", { "href": "javascript:;" }).text(_t)))
                );
                    break;
                case "object":
                    $breadcrumb.append(
              (_i == window.curlocation.length - 1 ? $("<li>", { "class": "active" }).text(_t.title) : $("<li>").append($("<a>", { "href": (_t.href || "javascript:;") }).text(_t.title)))
                );
                    break;
            }

        });
    }
}

//更新消息
function UpdateMessage() {
    toastr.options = {
        closeButton: true,
        positionClass: "toast-bottom-right",
    };
    toastr.info("您有一条新的消息，请及时查看！");
    var msgcnt = parseInt($("#user_notice .badge").text());
    $("#user_notice .badge").text(++msgcnt);
}

//页面加载失败
function fnAjaxError(xhr, status, error)
{
    if (xhr.status == 908)
    {
        swal("用户当前身份过期", "1秒后自动跳转", "error");
        window.location.replace(document.weburl + "Login/Index");
    }
    else {
        swal("操作失败", error||"未知错误", "error");
    }
}