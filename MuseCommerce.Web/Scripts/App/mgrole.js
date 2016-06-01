var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('mgroleApp', AppDependencies);
app.controller('mgroleCtrl', function ($scope, $http) {


}).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

    $stateProvider
        .state('index', {
            url: '/index',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgrole/index.tpl.html' }
            }
        })
        .state('create', {
            url: '/create',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgrole/create.tpl.html' }
            }
        }).state('edit', {
            url: '/edit/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgrole/edit.tpl.html' }
            }
        })
        .state('display', {
            url: '/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgrole/display.tpl.html' }
            }
        });

    $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

}]);


app.controller('IndexmgroleCtrl', function ($scope, $http, $state, $stateParams) {

    $scope.qname = "";
    $scope.qurl = "";

    $scope.search = function () {
        var config = { params: { 'qname': $scope.qname, 'qurl': $scope.qurl } };
        $http.get("/Manage/mgrole/mgroleInfo", config)
        .success(function (response) { $scope.mgroles = response.data; });

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
            $http.delete("/Manage/mgrole/DeleteMGRole", config)
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


app.controller('EditmgroleCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {

    console.log($stateParams.ID);

    $scope.loadmgrole = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/mgrole/mgrole", config)
        .success(function (response) { $scope.mgrole = response.data; });

    };

    $scope.save = function () {
        var config = {};
        var data = $scope.mgrole;
        $http.put("/Manage/mgrole/Putmgrole", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log($scope.success);
            $rootScope.$state.go('index');
        });
    };

    $scope.loadmgrole();

});

app.controller('DisplaymgroleCtrl', function ($scope, $http, $state, $stateParams) {
    $scope.mgroleid = $stateParams.ID;
    $scope.loadmgrole = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/mgrole/mgrole", config)
        .success(function (response) { $scope.mgrole = response.data; });
    };

    $scope.loadmgrole();

});

app.controller('CreatemgroleCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.mgrole = { 'Id': "", "FName": "", "FUrl": "" };


    $scope.save = function () {

        console.log('save');

        var config = {};
        var data = $scope.mgrole;
        $http.post("/Manage/mgrole/Savemgrole", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log($scope.success);
            $rootScope.$state.go('index');
        });
    };

});
