var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('mgpermissionApp', AppDependencies);
app.controller('mgpermissionCtrl', function ($scope, $http) {


}).config(['$stateProvider', '$urlRouterProvider', '$httpProvider', function ($stateProvider, $urlRouterProvider, $httpProvider) {

    $httpProvider.interceptors.push(['$rootScope', '$q', '$location', '$timeout',
        function ($rootScope, $q, $location, $timeout) {
            return {
                'request': function (config) {
                    //处理AJAX请求（否则后台IsAjaxRequest()始终false）
                    config.headers['X-Requested-With'] = 'XMLHttpRequest';
                    return config || $q.when(config);
                },
                'requestError': function (rejection) {
                    return rejection;
                },
                'response': function (response) {
                    return response || $q.when(response);
                },
                'responseError': function (response) {
                    console.log('responseError:' + response);
                    if (response.status === 401 || response.status === 403) {
                        abp.notify.error("会话超时，请重新登录！");
                        $timeout(function () { window.location = "/Login"; }, 3000);
                        return false;
                    }
                    else if (response.status === 500) {
                        $location.path('/error');
                        return false;
                    }
                    return $q.reject(response);
                }
            };
        }]);

    $stateProvider
        .state('index', {
            url: '/index',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgpermission/index.tpl.html' }
            }
        })
        .state('create', {
            url: '/create',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgpermission/create.tpl.html' }
            }
        }).state('edit', {
            url: '/edit/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgpermission/edit.tpl.html' }
            }
        })
        .state('display', {
            url: '/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgpermission/display.tpl.html' }
            }
        });

    $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

}]);


app.controller('IndexmgpermissionCtrl', function ($scope, $http, $state, $stateParams) {

    $scope.qname = "";
    $scope.qdescription = "";

    $scope.search = function () {
        var config = { params: { 'qname': $scope.qname, 'qdescription': $scope.qdescription } };
        $http.get("/Manage/mgpermission/mgpermissionInfo", config)
        .success(function (response) { $scope.mgpermissions = response.data; });

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
            $http.delete("/Manage/mgpermission/DeleteMGpermission", config)
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

    $scope.forbidden = function (fid) {
        var data = { 'fid': fid };

        $http.put("/Manage/mgpermission/forbidden", data)
           .success(function (response) {
               $scope.success = response.success;

               $scope.search();
           });
    };

    $scope.restore = function (fid) {
        var data = { 'fid': fid };

        $http.put("/Manage/mgpermission/restore", data)
           .success(function (response) {
               $scope.success = response.success;

               $scope.search();
           });
    };

    $scope.search();

});


app.controller('EditmgpermissionCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {

    console.log($stateParams.ID);

    $scope.loadmgpermission = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/mgpermission/mgpermission", config)
        .success(function (response) { $scope.mgpermission = response.data; });

    };

    $scope.save = function () {
        var config = {};
        var data = $scope.mgpermission;
        $http.put("/Manage/mgpermission/Putmgpermission", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log($scope.success);
            $rootScope.$state.go('index');
        });
    };

    $scope.loadmgpermission();

});

app.controller('DisplaymgpermissionCtrl', function ($scope, $http, $state, $stateParams) {
    $scope.mgpermissionid = $stateParams.ID;
    $scope.loadmgpermission = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/mgpermission/mgpermission", config)
        .success(function (response) { $scope.mgpermission = response.data; });
    };

    $scope.loadmgpermission();

});

app.controller('CreatemgpermissionCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.mgpermission = { 'Id': "", "FName": "", "FUrl": "" };


    $scope.save = function () {

        console.log('save');

        var config = {};
        var data = $scope.mgpermission;
        $http.post("/Manage/mgpermission/Savemgpermission", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log($scope.success);
            $rootScope.$state.go('index');
        });
    };

});
