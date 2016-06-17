var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('mgitemApp', AppDependencies);
app.controller('mgitemCtrl', function ($scope, $http) {


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
                '': { templateUrl: '/Scripts/App/tpls/mgitem/index.tpl.html' }
            }
        })
        .state('create', {
            url: '/create',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgitem/create.tpl.html' }
            }
        }).state('edit', {
            url: '/edit/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgitem/edit.tpl.html' }
            }
        })
        .state('display', {
            url: '/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgitem/display.tpl.html' }
            }
        });

    $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
}]);


app.controller('IndexmgitemCtrl', function ($scope, $http, $state, $stateParams) {
   
    $scope.start = 0;
    $scope.length = 8;
    $scope.recordsTotal = 0;

    $scope.qname = "";
    $scope.qnumber = "";

    $scope.search = function () {
        var config = {
            params: {
                'qname': $scope.qname, 'qnumber': $scope.qnumber,
                'start': $scope.start, 'length': $scope.length
            }
        };
        $http.get("/MGCode/Item/ItemCoreInfo", config)
        .success(function (response) {
            $scope.start = response.start;
            $scope.recordsTotal = response.recordsTotal;
            $scope.mgitems = response.data;

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
            $http.delete("/MGCode/Item/Deletemgitem", config)
            .success(function (response) {
                
                $scope.success = response.success;
                $scope.search();
                swal("删除成功！", "您已经永久删除了这条信息。", "success");
            });
        });        
    };

    $scope.forbidden = function (fid) { 
        var data = { 'fid': fid };
        $http.put("/MGCode/Item/forbidden",data)
           .success(function (response) {
               $scope.success = response.success;
               $scope.search();
        });
    };
    
    $scope.restore = function (fid) {       
        var data = { 'fid': fid };
        $http.put("/MGCode/Item/restore", data)
           .success(function (response) {
               $scope.success = response.success;
               $scope.search();
           });
    };

    $scope.search();   

});


app.controller('EditmgitemCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {

    console.log($stateParams.ID);

    $scope.loadmgitem = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/MGCode/Item/ItemCore", config)
        .success(function (response) { $scope.mgitem = response.data; });
    };

    $scope.save = function () {
        var config = {};
        var data = $scope.mgitem;
        $http.put("/MGCode/Item/PutItemCore", data, config)
        .success(function (response) {
            $scope.success = response.success;
            $rootScope.$state.go('index');
        });
    };

    $scope.loadmgitem();

});

app.controller('DisplaymgitemCtrl', function ($scope, $http, $state, $stateParams) {
    $scope.mgitemid = $stateParams.ID;
    $scope.loadmgitem = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/MGCode/Item/ItemCore", config)
        .success(function (response) { $scope.mgitem = response.data; });
    };

    $scope.loadmgitem();

});

app.controller('CreatemgitemCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.mgitem = { 'Id': "", "FName": "", "FUrl": "" };


    $scope.save = function () {

        var config = {};
        var data = $scope.mgitem;
        $http.post("/MGCode/Item/SaveItemCore", data, config)
        .success(function (response) {
            $scope.success = response.success;           
            $rootScope.$state.go('index');
        });
    };

});

