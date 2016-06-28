var underscore = angular.module('underscore', []);
underscore.factory('_', ['$window', function ($window) {
    return $window._; // assumes underscore has already been loaded on the page
}]);

var ngdatepicker = angular.module('ngdatepicker', []);
ngdatepicker.factory('mydatepicker', [function () {
    var jquery = $;
    return jquery; // assumes underscore has already been loaded on the page
}]);

var ngpohttprest = angular.module('ngpohttprest', []);


ngpohttprest.factory('httpItemCore',
  function ($http, $q) {
      return {
          search: function (params) {
              var defer = $q.defer();
              $http({
                  method: 'get',
                  params: params,
                  url: '/MGCode/Item/ItemCoreInfo'
              }).success(function (data) {
                  defer.resolve(data);
              }).error(function (data) {
                  defer.reject(data);
              });
              return defer.promise
          }
      };
  });

ngpohttprest.factory('httpPOTranTypeInfo',
  function ($http, $q) {
      return {
          getPOTranTypeInfo: function () {
              var defer = $q.defer();
              $http({
                  method: 'get',
                  url: '/Order/PORequest/POTranTypeInfo'
              }).success(function (data) {
                  defer.resolve(data);
              }).error(function (data) {
                  defer.reject(data);
              });
              return defer.promise
          }
      };
  });
ngpohttprest.factory('httpPorequest',
  function ($http, $q) {
      return {
          search: function (params) {
              var defer = $q.defer();
              $http({
                  method: 'get',
                  params: params,
                  url: '/Order/PORequest/porequestInfo'
              }).success(function (data) {
                  defer.resolve(data);
              }).error(function (data) {
                  defer.reject(data);
              });
              return defer.promise
          },
          getbyId: function (Id) {
              var defer = $q.defer();
              var params = { 'ID': Id };
              $http({
                  method: 'get',
                  params: params,
                  url: '/Order/PORequest/porequest'
              }).success(function (data) {
                  defer.resolve(data);
              }).error(function (data) {
                  defer.reject(data);
              });
              return defer.promise
          },
          put: function (data) {
              var defer = $q.defer();
              $http({
                  method: 'put',
                  data: data,
                  url: '/Order/PORequest/Putporequest'
              }).success(function (data) {
                  defer.resolve(data);
              }).error(function (data) {
                  defer.reject(data);
              });
              return defer.promise
          },
          save: function (data) {
              var defer = $q.defer();
              $http({
                  method: 'post',
                  data: data,
                  url: '/Order/PORequest/Saveporequest'
              }).success(function (data) {
                  defer.resolve(data);
              }).error(function (data) {
                  defer.reject(data);
              });
              return defer.promise
          }
      };
  });

var AppDependencies = ['ui.router', 'ngAnimate', 'ngDialog', 'vModal', 'underscore', 'ngdatepicker', 'ngpohttprest'];
var app = angular.module('porequestapp', AppDependencies);

app.factory('SearchModal', function (vModal) {
    return vModal({
        controller: 'IndexmgitemCtrl',
        controllerAs: 'SearchModal',
        templateUrl: '/Scripts/App/tpls/mgitem/search.tpl.html'        
    });
});
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
        })
        .state('setting', {
            url: '/setting/{ID}',
            views: {
                '': { templateUrl: '/Scripts/App/tpls/porequest/setting.tpl.html' }
            }
        });

    $urlRouterProvider.otherwise('/index');
}])
.run(['$rootScope', '$state', '$stateParams', function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
    
}]);

app.filter("jsonDate", function ($filter) {
    return function (input, format) {
        //从字符串 /Date(1448864369815)/ 得到时间戳 1448864369815
        var timestamp = Number(input.replace(/\/Date\((\d+)\)\//, "$1"));
        //转成指定格式
        return $filter("date")(timestamp, format);
    };
});

app.controller('IndexporequestCtrl', function ($scope, $http, $state, $stateParams, mydatepicker, httpPorequest) {
    mydatepicker('[data-provide="datepicker-inline"]').datepicker();
    $scope.qbillno = "";
    $scope.qsdate = "";
    $scope.qedate = "";

    $scope.search = function () {
        var config = { 'qbillno': $scope.qbillno, 'qsdate': $scope.qsdate, 'qedate': $scope.qedate };
        httpPorequest.search(config).then(function (response) {
            $scope.porequests = response.data;
        });        
    };

    $scope.search();
});

app.controller('EditporequestCtrl', function ($scope, $http, $state, $stateParams, $rootScope, SearchModal, mydatepicker, httpPorequest, httpPOTranTypeInfo) {
    mydatepicker('[data-provide="datepicker-inline"]').datepicker();

    httpPOTranTypeInfo.getPOTranTypeInfo().then(function (response) {
        $scope.POTranTypeInfo = response.data;
        httpPorequest.getbyId($stateParams.ID).then(function (response) {
            $scope.porequest = response.data;
        });
    });

    SearchModal.itemadd = function (msg) {
        angular.forEach(msg, function (item) {
            if (item.check) {

                var PORequestEntry = new Object();

                PORequestEntry.Id = "Add";
                PORequestEntry.FItemID = item.Id;
                PORequestEntry.FItem = new Object();
                PORequestEntry.FItem.FName = item.FName;
                PORequestEntry.FQty = 0;
                PORequestEntry.FPrice = 0;

                if (_.isUndefined(_.findWhere($scope.porequest.PORequestEntrys, { FItemID: item.Id }))) {
                    $scope.porequest.PORequestEntrys.push(PORequestEntry);
                }
            }
        });
        SearchModal.deactivate();
    }

    $scope.open = function () {
        SearchModal.activate();
    };
   
    $scope.save = function () {
        var config = {};
        var data = $scope.porequest;
       
        httpPorequest.put(data).then(function (response) {
            $scope.success = response.success;
            console.log(response.success);
            $rootScope.$state.go('index');
        });
    };
});

app.controller('DisplayporequestCtrl', function ($scope, $http, $state, $stateParams, httpPorequest) {
    $scope.porequestid = $stateParams.ID;
    
    $scope.loadporequest = function () {
        httpPorequest.getbyId($stateParams.ID).then(function (response) {
            $scope.porequest = response.data;
        });
    };

    $scope.loadporequest();

});

app.controller('CreateporequestCtrl', function ($scope, $http, $state, $stateParams, $rootScope, SearchModal, _, mydatepicker, httpPOTranTypeInfo, httpPorequest) {
    mydatepicker('[data-provide="datepicker-inline"]').datepicker();

    httpPOTranTypeInfo.getPOTranTypeInfo().then(function (response) { 
        $scope.POTranTypeInfo = response.data;
        $scope.porequest = {
            'Id': "", "FBillNo": "", "FDate": "", "FNote": "", "FTranType": "",
            "FStatus": "", "PoAddress": { "StreetNumber": "", "StreetName": "" },
            "PORequestEntrys": []
        };
    });
    
    SearchModal.itemadd = function (msg) {
        angular.forEach(msg, function (item) {
            if (item.check) {

                var PORequestEntry = new Object();

                PORequestEntry.FInterID = "Add";
                PORequestEntry.FItemID = item.Id;
                PORequestEntry.FItem = new Object();
                PORequestEntry.FItem.FName = item.FName;
                PORequestEntry.FQty = 0;
                PORequestEntry.FPrice = 0;

                if (_.isUndefined(_.findWhere($scope.porequest.PORequestEntrys, { FItemID: item.Id }))) {
                    $scope.porequest.PORequestEntrys.push(PORequestEntry);
                }
            }
        });
        SearchModal.deactivate();
    }
    
    $scope.save = function () {       
        var config = {};
        var data = $scope.porequest;
        
        httpPorequest.save(data).then(function (response) {
            $scope.success = response.success;
            $rootScope.$state.go('index');
        });
    };
    
    $scope.open = function () {
        SearchModal.activate();
    };


});

app.controller('IndexmgitemCtrl', function ($scope, $http, $state, $stateParams, SearchModal, httpItemCore) {
    var ctrl = this;

    ctrl.close = SearchModal.deactivate;

    $scope.start = 0;
    $scope.length = 8;
    $scope.recordsTotal = 0;

    $scope.qname = "";
    $scope.qnumber = "";


    $scope.search = function () {
        var config = {           
                'qname': $scope.qname, 'qnumber': $scope.qnumber,
                'start': $scope.start, 'length': $scope.length           
        };
        httpItemCore.search(config).then(function (response) {
            angular.forEach(response.data, function (item) {
                item.check = false;
            });

            $scope.start = response.start;
            $scope.recordsTotal = response.recordsTotal;
            $scope.mgitems = response.data;

            $scope.pagination();
        });        
    };

    $scope.query = function () {
        $scope.start = 0;
        $scope.search();
    }

    $scope.checkok = function () {

        var checkitems = new Array();
        angular.forEach($scope.mgitems, function (item) {
            if (item.check)
            {
                checkitems.push(item);
            }
        });
        SearchModal.itemadd(checkitems);       
        
        angular.forEach($scope.mgitems, function (item) {
            item.check = false;
        });
       
    };

    $scope.setpagination = function (pageflag) {

        switch (pageflag) {
            case "first":
                $scope.start = 0;
                break;
            case "prev":
                $scope.start = ($scope.pageIndex - 2) * $scope.length;
                break;
            case "next":
                $scope.start = ($scope.pageIndex) * $scope.length;
                break;
            case "last":
                $scope.start = ($scope.endIndex - 1) * $scope.length;
                break;
        }
        $scope.search();
    }

    $scope.pagination = function () {
        $scope.pageIndex = parseInt($scope.start / $scope.length) + 1;
        $scope.endIndex = parseInt($scope.recordsTotal / $scope.length) + 1;
        $scope.first = $scope.pageIndex <= 1 ? false : true;
        $scope.last = $scope.pageIndex >= $scope.endIndex ? false : true;
        $scope.prev = $scope.pageIndex > 1 ? true : false;
        $scope.next = $scope.pageIndex < $scope.endIndex ? true : false;
    }
    
    $scope.search();

});

