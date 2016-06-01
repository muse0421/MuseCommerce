
var hubproxy = $.connection.NoticeMessageHub;

var app = angular.module('NoticeMessageApp', []);
app.controller('NoticeMessageCtrl', function ($scope) {
    $scope.msgs = [];
    $scope.sendmsg = '';
    $scope.chartmsgs = [];

   


    $scope.send=function () {
        //hubproxy.invoke('SendChartMessage',hubconn.id, $scope.sendmsg);
        console.log("chart send");

        var senddata = new Object();
        senddata.Sender = 'ME';
        senddata.Content = $scope.sendmsg;
        senddata.SendTime = '20160531';

        console.log(senddata);

        $scope.chartmsgs.push(senddata);

        var scrollTo_h = $('.content').prop('scrollHeight') + 'px';
        $('.content').slimScroll({ scrollTo: scrollTo_h });

        hubproxy.server.sendChartMessage('dsada', $scope.sendmsg);
    };


    //注册客户端方法 “hello”
    hubproxy.client.hello=function (data) {
        console.log("客户端方法被调用");

        hubproxy.server.SendChartMessage('dsada','dsa');
    };

    hubproxy.client.ReviceNotice= function (data) {

        console.log(data);
        $scope.msgs.push(data);

        console.log($scope.msgs.length);

        console.log("客户端方法被调用2");

        $scope.$apply();
    };


    //ng-click="SendChartMessage()"

    hubproxy.client.ReviceChartNotice =  function (data) {

        $scope.chartmsgs.push(data);


        console.log($scope.chartmsgs.length);

        console.log("客户端方法被调用5");

        $scope.$apply()

        var scrollTo_h = $('.content').prop('scrollHeight') + 'px';
        $('.content').slimScroll({ scrollTo: scrollTo_h });

        
    };

});


app.directive("runoobDirective", function () {
    return {
        template: "<li class='m-t-xs' ng-model='msgs' ng-repeat='m in msgs'><div class='dropdown-messages-box'>	<div class='media-body'><strong class='label-primary'>{{m.Sender}}</strong>{{m.Content}}<br><small class='text-muted'>{{m.SendTime}}</small></div></div><div class='divider'></div></li>"
    };
});


app.directive("chartcontent", function () {
    return {
        restrict: 'E',
        template: "<div ng-model='chartmsgs' ng-repeat='m in chartmsgs'><div ng-switch on='m.Sender' ><div class='right' ng-switch-when='ME'><chartcontent2></chartcontent2></div><div class='left' ng-switch-default><chartcontent2></chartcontent2></div></div></div>"
    };
});

app.directive("chartcontent2", function () {
    return {
        restrict: 'E',
        template: "<div class='author-name'>{{m.Sender}}<small class='chat-date'>{{m.SendTime}}</small></div><div class='chat-message active'>{{m.Content}}</div>"
    };
});



//$.connection.hub.start(options, function () { log("connected"); });

$.connection.hub.start().done(function (data) {
    //调用服务器方法
    // proxy.invoke("Hello");
    //console.log(hubconn.id);
    hubproxy.server.sendChartMessage('dsada', 'dsa');

    hubproxy.server.sendMessage();

    hubproxy.server.sendMessage();

   // console.log(hubconn.id);
}).fail(function (data) { console.log("fail=" + data); });




//function sendchartmsg1() {
//    hubproxy.server.SendMessage();
//}

//function sendchartmsg2() {
//    hubproxy.server.CallHello();
//}