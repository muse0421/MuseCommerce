var AppDependencies = ['ui.router', 'ngAnimate'];
var app = angular.module('mgroleApp', AppDependencies);
app.controller('mgroleCtrl', function ($scope, $http) {


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
        })
        .state('setting', {
            url: '/setting/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/mgrole/setting.tpl.html' }
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
    $scope.qdescription = "";

    $scope.search = function () {
        var config = { params: { 'qname': $scope.qname, 'qdescription': $scope.qdescription } };
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

                $scope.search();

                swal("删除成功！", "您已经永久删除了这条信息。", "success");
            });
        });
        

        console.log('search');
    };

    $scope.forbidden = function (fid) { 
        var data = { 'fid': fid };
        $http.put("/Manage/mgrole/forbidden",data)
           .success(function (response) {
               $scope.success = response.success;
               $scope.search();
        });
    };
    
    $scope.restore = function (fid) {       
        var data = { 'fid': fid };
        $http.put("/Manage/mgrole/restore", data)
           .success(function (response) {
               $scope.success = response.success;

               $scope.search();
           });
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


app.controller('SettingmgroleCtrl', function ($scope, $http, $state, $stateParams, $rootScope) {
    $scope.mgroleid = $stateParams.ID;
       

    $scope.loadmgrole = function () {

        var config = { params: { 'ID': $stateParams.ID } };
        $http.get("/Manage/mgrole/MGRoleAssignment", config)
        .success(function (response) {
            $scope.mgrole = response.data;
            console.log(response.data);


           $http.get("/Manage/mgpermission/mgpermissionInfo", config)
          .success(function (response) {

              angular.forEach(response.data, function (item) {
                  item.check = false;

                  if ($scope.mgrole.FPermissions != null) {
                      console.log($scope.mgrole.FPermissions.length);
                      if($scope.mgrole.FPermissions.length>0)
                      {
                          angular.forEach($scope.mgrole.FPermissions, function (pitem) {
                              if(pitem.Id==item.Id)
                              {
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
        
        
        $scope.mgrole.FPermissions.splice(0, $scope.mgrole.FPermissions.length);
        angular.forEach($scope.mgpermissionInfo, function (item) {
            console.log(item.FName);
            console.log(item.check);
                        
            if (item.check == true) {
                $scope.mgrole.FPermissions.push(item);
            }

        });

        var data = { 'oData': $scope.mgrole };

        //
        console.log(data);
        console.log($scope.mgrole.FPermissions);
        //angular.fromJson(mydata);
        //console.log(JSON.stringify(mydata));
        //console.log(eval(mydata));
        //return;

        $http.post("/Manage/mgrole/SaveMGRolePermission", data, config)
        .success(function (response) {
            $scope.success = response.success;
            console.log('save');
            console.log(response);
            console.log($scope.success);
            $rootScope.$state.go('index');
        });
    };

    $scope.loadmgrole();

});
