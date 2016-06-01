var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('porequestapp', AppDependencies);
app.controller('porequestCtrl', function ($scope, $http) {
    
   
}).config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

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
