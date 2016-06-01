﻿var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('mgfuncApp', AppDependencies);
app.controller('mgfuncCtrl', function ($scope, $http) {
    
   
}).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

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

    $scope.search();

});


app.controller('EditMGFuncCtrl', function ($scope, $http, $state, $stateParams) {

    console.log($stateParams.ID);

    $scope.loadmgfunc = function () {
        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/MGFunc/MGFunc", config)
        .success(function (response) { $scope.mgfunc = response.data; });

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
    $scope.mgfunc = { 'Id': "", "FName": "", "FUrl": "" };
    
    
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
