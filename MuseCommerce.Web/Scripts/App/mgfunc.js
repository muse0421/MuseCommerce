var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('mgfuncApp', AppDependencies);
app.controller('mgfuncCtrl', function ($scope, $http) {


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
                '': { templateUrl: '/Scripts/App/tpls/mgfunc/index.tpl.html' }
            }
        })
        .state('create', {
            url: '/create',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgfunc/create.tpl.html' }
            }
        }).state('edit', {
            url: '/edit/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgfunc/edit.tpl.html' }
            }
        })
        .state('display', {
            url: '/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgfunc/display.tpl.html' }
            }
        });

    $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

}]);


app.controller('IndexMGFuncCtrl', function ($scope, $http, $state, $stateParams) {

    $scope.qname = "";
    $scope.qurl = "";

    $scope.search = function () {
        var config = { params: { 'qname': $scope.qname, 'qurl': $scope.qurl } };
        $http.get("/Manage/MGFunc/MGFuncInfo", config)
        .success(function (response) { $scope.mgfuncs = response.data; });

        console.log('search');
    };
    $scope.forbidden = function (fid) {
        var data = { 'fid': fid };
        $http.put("/Manage/MGFunc/forbidden", data)
           .success(function (response) {
               $scope.success = response.success;
               $scope.search();
           });
    };

    $scope.restore = function (fid) {
        var data = { 'fid': fid };
        $http.put("/Manage/MGFunc/restore", data)
           .success(function (response) {
               $scope.success = response.success;

               $scope.search();
           });
    };

    $scope.search();

});


app.controller('EditMGFuncCtrl', function ($scope, $http, $state, $stateParams) {

    console.log($stateParams.ID);

    $scope.loadmgfunc = function () {
        var config = { params: { 'ID': $stateParams.ID } };


        $http.get("/Manage/mgpermission/mgpermissionInfo", config)
       .success(function (response2) {

           $scope.mgpermissionInfo = response2.data;
           console.log(response2.data);

            $http.get("/Manage/MGFunc/MGFunc", config)
            .success(function (response) {
                $scope.mgfunc = response.data;
            });

       });

    };

    $scope.save = function () {
        var config = {};
        var data = $scope.mgfunc;
        $http.put("/Manage/MGFunc/PutMGFunc", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log($scope.success);
        });
    };

    $scope.loadmgfunc();

});

app.controller('DisplayMGFuncCtrl', function ($scope, $http, $state, $stateParams) {
    $scope.mgfuncid = $stateParams.ID;
    $scope.loadmgfunc = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/MGFunc/MGFunc", config)
        .success(function (response) { $scope.mgfunc = response.data; });
    };

    $scope.loadmgfunc();

});

app.controller('CreateMGFuncCtrl', function ($scope, $http, $state, $stateParams) {
    
    var config = { params: { 'qname': '' } };
    
    $http.get("/Manage/mgpermission/mgpermissionInfo", config)
      .success(function (response2) {
          $scope.mgpermissionInfo = response2.data;
          $scope.mgfunc = { 'Id': "", "FName": "", "FUrl": "", "FPermissionID": "", "FPriority": 0 };
          
      });

    $scope.save = function () {

        console.log('save');

        var config = {};
        var data = $scope.mgfunc;
        $http.post("/Manage/MGFunc/SaveMGFunc", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log($scope.success);
        });
    };

});
