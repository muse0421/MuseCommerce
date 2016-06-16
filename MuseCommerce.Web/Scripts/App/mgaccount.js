var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('mgaccountApp', AppDependencies);
app.controller('mgaccountCtrl', function ($scope, $http) {


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
        })
        .state('setting', {
            url: '/setting/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgaccount/setting.tpl.html' }
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



app.controller('SettingmgaccountCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.mgaccountid = $stateParams.ID;


    $scope.loadmgaccount = function () {

        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/mgaccount/MGRoleAssignment", config)
        .success(function (response) {
            $scope.mgaccount = response.data;
            console.log(response.data);


            $http.get("/Manage/mgrole/MGRoleInfo", config)
          .success(function (response) {

              angular.forEach(response.data, function (item) {
                  item.check = false;

                  if ($scope.mgaccount.FRoles != null) {
                      console.log($scope.mgaccount.FRoles.length);
                      if ($scope.mgaccount.FRoles.length > 0) {
                          angular.forEach($scope.mgaccount.FRoles, function (pitem) {
                              if (pitem.Id == item.Id) {
                                  item.check = true;
                              }
                          });
                      }
                  }
              });

              $scope.mgpermissionInfo = response.data;
              console.log(response.data);

          });

        });


    };

    $scope.save = function () {
        var config = {};


        $scope.mgaccount.FRoles.splice(0, $scope.mgaccount.FRoles.length);
        angular.forEach($scope.mgpermissionInfo, function (item) {
            console.log(item.FName);
            console.log(item.check);

            if (item.check == true) {
                $scope.mgaccount.FRoles.push(item);
            }

        });

        var data = { 'oData': $scope.mgaccount };

        //
        console.log(data);
        console.log($scope.mgaccount.FRoles);
        //angular.fromJson(mydata);
        //console.log(JSON.stringify(mydata));
        //console.log(eval(mydata));
        //return;

        $http.post("/Manage/mgaccount/SaveMGRoleAssignment", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log(response);
            console.log($scope.success);
            $rootScope.$state.go('index');
        });
    };

    $scope.loadmgaccount();

});