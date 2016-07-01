


var AppDependencies = ['ui.router', 'ngAnimate','vModal'];
var app = angular.module('MGJobApp', AppDependencies);

app.controller('MGJobCtrl', function ($scope, $http) {


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
                '': { templateUrl: '/Scripts/App/tpls/MGJob/index.tpl.html' }
            }
        })
        .state('index.create', {
            url: '/create',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/MGJob/create.tpl.html' }
            }
        }).state('index.sqlcreate', {
            url: '/sqlcreate',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/MGJob/sqlcreate.tpl.html' }
            }
        })
        .state('index.mailcreate', {
            url: '/mailcreate',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/MGJob/mailcreate.tpl.html' }
            }
        });

    $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;

}]);


app.controller('IndexMGJobCtrl', function ($scope, $http, $state, $stateParams) {

    $scope.qname = "";
    $scope.qgroup = "";

    $scope.search = function () {
        console.log('search');
    };
       

    $scope.search();

});


app.controller('CreateSqlMGJobCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.mgjob = { 'name': '', 'group': 'default', 'cronExpression': '3/2 * * * * ? ', 'CommandText': "update ItemCores set [ModifiedDate]=getdate() where [Id]='001'" };
    $scope.save = function () {
        $http.put("/Manage/MGJob/AddSqlStatementJob", $scope.mgjob)
           .success(function (response) {
               $scope.success = response.success;
               $rootScope.$state.go('index');
           });
    };
});

app.controller('CreateMailMGJobCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.mgjob = { 'name': '', 'group': 'default', 'cronExpression': '3/10 * * * * ? ', 'recipients': 'xpy.liu@kingmaker-footwear.com', 'subject': '', 'body': '' };
    $scope.save = function () {
        $http.put("/Manage/MGJob/AddMailJob", $scope.mgjob)
           .success(function (response) {
               $scope.success = response.success;
               $rootScope.$state.go('index');
           });
    };
});

app.controller('CreateMGJobCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    
    $scope.mgjob = { 'name': '', 'group': 'default', 'message': '' };
    
    $scope.save = function () {
                
        $http.put("/Manage/MGJob/AddMyJob", $scope.mgjob)
           .success(function (response) {
               $scope.success = response.success;
               $rootScope.$state.go('index');
           });
    };

});

