var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('porequestapp', AppDependencies);
app.controller('porequestCtrl', function ($scope, $http) {
    
   
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
                '': { templateUrl: '/Scripts/App/tpls/porequest/index.tpl.html' }
            }
        })
        .state('create', {
            url: '/create',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/porequest/create.tpl.html' }
            }
        }).state('edit', {
            url: '/edit/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/porequest/edit.tpl.html' }
            }
        })
        .state('display', {
            url: '/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/porequest/display.tpl.html' }
            }
        });

        $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    
}]);


app.controller('IndexporequestCtrl', function ($scope, $http, $state, $stateParams) {
   
    $scope.qname = "";
    $scope.qurl = "";

    $scope.search = function () {
        var config = { params: { 'qname': $scope.qname, 'qurl': $scope.qurl } };
        $http.get("/Order/PORequest/porequestInfo", config)
        .success(function (response) { $scope.porequests = response.data; });

        console.log('search');
    };

    $scope.search();

});


app.controller('EditporequestCtrl', function ($scope, $http, $state, $stateParams) {

    console.log($stateParams.ID);

    $scope.loadporequest = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Order/PORequest/porequest", config)
        .success(function (response) { $scope.porequest = response.data; });

    };

    $scope.save = function () {
        var config = {};
        var data = $scope.porequest;
        $http.put("/Order/PORequest/Putporequest", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log($scope.success);
        });
    };

    $scope.loadporequest();

});

app.controller('DisplayporequestCtrl', function ($scope, $http, $state, $stateParams) {
    $scope.porequestid = $stateParams.ID;
    $scope.loadporequest = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Order/PORequest/porequest", config)
        .success(function (response) { $scope.porequest = response.data; });
    };

    $scope.loadporequest();

});

app.controller('CreateporequestCtrl', function ($scope, $http, $state, $stateParams) {
    $scope.porequest = { 'Id': "2020", "FBillNo": "12121212", "FDate": "2016/01/01", "FNote": "", "FStatus": "", "PoAddress": { "StreetNumber": "", "StreetName": "" }, "PORequestEntrys": [{ "Id": "1", "FQty": "25.0", "FPrice": "7", "FInterID": "1", "PORequest_Id": "2020" }, { "Id": "2", "FQty": "33.0", "FPrice": "7", "FInterID": "3", "PORequest_Id": "2020" }, { "Id": "3", "FQty": "125.0", "FPrice": "7", "FInterID": "9", "PORequest_Id": "2020" }] };
    
    $scope.save = function () {

        console.log('save');
       
        var config = {};
        var data = $scope.porequest;
        $http.post("/Order/PORequest/Saveporequest", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log(response.data);
        });       
    };

});
