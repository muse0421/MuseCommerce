var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('mgaccountApp', AppDependencies);
app.controller('mgaccountCtrl', function ($scope, $http) {


}).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state('index', {
            url: '/index',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgaccount/index.tpl.html' }
            }
        })
        .state('create', {
            url: '/create',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgaccount/create.tpl.html' }
            }
        }).state('edit', {
            url: '/edit/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgaccount/edit.tpl.html' }
            }
        })
        .state('display', {
            url: '/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgaccount/display.tpl.html' }
            }
        });

    $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

}]);


app.controller('IndexmgaccountCtrl', function ($scope, $http, $state, $stateParams) {

    $scope.qname = "";
    $scope.qusertype = "";

    $scope.search = function () {
        var config = { params: { 'qname': $scope.qname, 'qusertype': $scope.qusertype } };
        $http.get("/Manage/mgaccount/mgaccountInfo", config)
        .success(function (response) { $scope.mgaccounts = response.data; });

        console.log('search');
    };

    $scope.delete = function (fid) {
        var config = { params: { 'fid': fid } };

        console.log("fid=");
        console.log(fid);

        swal({
            title: "您确定要删除这条信息吗",
            text: "删除后将无法恢复，请谨慎操作！",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "删除",
            closeOnConfirm: false
        }, function () {
            $http.delete("/Manage/mgaccount/DeleteMGaccount", config)
            .success(function (response) {
                $scope.success = response.success;
                console.log('save');
                console.log($scope.success);

                $scope.search();

                swal("删除成功！", "您已经永久删除了这条信息。", "success");
            });
        });
        return;



        console.log('search');
    };

    $scope.search();

});


app.controller('EditmgaccountCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {

    console.log($stateParams.ID);

    $scope.loadmgaccount = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/mgaccount/mgaccount", config)
        .success(function (response) { $scope.mgaccount = response.data; });

    };

    $scope.save = function () {
        var config = {};
        var data = $scope.mgaccount;
        $http.put("/Manage/mgaccount/Putmgaccount", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log($scope.success);
            $rootScope.$state.go('index');
        });
    };

    $scope.loadmgaccount();

});

app.controller('DisplaymgaccountCtrl', function ($scope, $http, $state, $stateParams) {
    $scope.mgaccountid = $stateParams.ID;
    $scope.loadmgaccount = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/mgaccount/mgaccount", config)
        .success(function (response) { $scope.mgaccount = response.data; });
    };

    $scope.loadmgaccount();

});

app.controller('CreatemgaccountCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.mgaccount = { 'Id': "", "FName": "", "FUrl": "" };


    $scope.save = function () {

        console.log('save');

        var config = {};
        var data = $scope.mgaccount;
        $http.post("/Manage/mgaccount/Savemgaccount", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log(response);
            $rootScope.$state.go('index');
        });
    };

});
