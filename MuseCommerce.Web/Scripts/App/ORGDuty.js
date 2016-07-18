var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('orgcodeApp', AppDependencies);
app.controller('orgcodeCtrl', function ($scope, $http) {


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
                '': { templateUrl: '/Scripts/App/tpls/ORGCode/index.tpl.html' }
            }
        })
        .state('create', {
            url: '/create',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/ORGCode/create.tpl.html' }
            }
        }).state('edit', {
            url: '/edit/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/ORGCode/edit.tpl.html' }
            }
        })
        .state('display', {
            url: '/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/ORGCode/display.tpl.html' }
            }
        });

    $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
}]);


app.controller('IndexorgcodeCtrl', function ($scope, $http, $state, $stateParams) {
   
    $scope.start = 0;
    $scope.length = 8;
    $scope.recordsTotal = 0;

    $scope.qname = "";

    $scope.search = function () {
        var config = {
            params: {
                'qname': $scope.qname,
                'start': $scope.start, 'length': $scope.length
            }
        };
        $http.get("/MGCode/ORGPDescriptionDuty/ORGPDescriptionDutyInfo", config)
        .success(function (response) {
            $scope.start = response.start;
            $scope.recordsTotal = response.recordsTotal;
            $scope.orgcodes = response.data;

            $scope.pagination();
        });
    };

    $scope.query=function()
    {
        $scope.start = 0;
        $scope.search();
    }

    $scope.setpagination = function (pageflag) {
        
        switch (pageflag)
        {
            case "first":
                $scope.start = 0;
                break;
            case "prev":
                $scope.start = ($scope.pageIndex - 2) * $scope.length;
                break;
            case "next":
                $scope.start = ($scope.pageIndex ) * $scope.length;
                break;
            case "last":
                $scope.start = ($scope.endIndex - 1) * $scope.length;
                break;                
        }
        $scope.search();
    }

    $scope.pagination = function ()
    {
        $scope.pageIndex = parseInt($scope.start / $scope.length) + 1;
        $scope.endIndex = parseInt($scope.recordsTotal / $scope.length) + 1;
        $scope.first = $scope.pageIndex <= 1 ? false : true;
        $scope.last = $scope.pageIndex >= $scope.endIndex ? false : true;
        $scope.prev = $scope.pageIndex > 1 ? true : false;
        $scope.next = $scope.pageIndex < $scope.endIndex ? true : false;
    }

    $scope.delete = function (fid) {
        var config = { params: { 'fid': fid } };

        swal({
            title: "您确定要删除这条信息吗",
            text: "删除后将无法恢复，请谨慎操作！",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "删除",
            closeOnConfirm: false
        }, function () {
            $http.delete("/MGCode/ORGPDescriptionDuty/DeleteORGPDescriptionDuty", config)
            .success(function (response) {
                
                $scope.success = response.success;
                $scope.search();
                swal("删除成功！", "您已经永久删除了这条信息。", "success");
            });
        });        
    };

    $scope.forbidden = function (fid) { 
        var data = { 'fid': fid };
        $http.put("/MGCode/ORGPDescriptionDuty/forbidden",data)
           .success(function (response) {
               $scope.success = response.success;
               $scope.search();
        });
    };
    
    $scope.restore = function (fid) {       
        var data = { 'fid': fid };
        $http.put("/MGCode/ORGPDescriptionDuty/restore", data)
           .success(function (response) {
               $scope.success = response.success;
               $scope.search();
           });
    };

    $scope.search();   

});


app.controller('EditorgcodeCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {

    console.log($stateParams.ID);

    $scope.loadorgcode = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/MGCode/ORGPDescriptionDuty/ORGPDescriptionDuty", config)
        .success(function (response) { $scope.orgcode = response.data; });
    };

    $scope.save = function () {
        var config = {};
        var data = $scope.orgcode;
        $http.put("/MGCode/ORGPDescriptionDuty/PutORGPDescriptionDuty", data, config)
        .success(function (response) {
            $scope.success = response.success;
            $rootScope.$state.go('index');
        });
    };

    $scope.loadorgcode();

});

app.controller('DisplayorgcodeCtrl', function ($scope, $http, $state, $stateParams) {
    $scope.orgcodeid = $stateParams.ID;
    $scope.loadorgcode = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/MGCode/ORGPDescriptionDuty/ORGPDescriptionDuty", config)
        .success(function (response) { $scope.orgcode = response.data; });
    };

    $scope.loadorgcode();

});

app.controller('CreateorgcodeCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.orgcode = { 'Id': "", "FName": "", "FUrl": "" };


    $scope.save = function () {

        var config = {};
        var data = $scope.orgcode;
        $http.post("/MGCode/ORGPDescriptionDuty/SaveORGPDescriptionDuty", data, config)
        .success(function (response) {
            $scope.success = response.success;           
            $rootScope.$state.go('index');
        });
    };

});

