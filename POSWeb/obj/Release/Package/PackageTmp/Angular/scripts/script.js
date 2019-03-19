/// <reference path="angular.min.js" />

var myApp = angular.module("myModule", [])

//var myController = function ($scope) {
//    $scope.message = "Testing angular"
//}


myApp.controller("myController", function ($scope, $http, $rootScope) {
    $scope.message = "Testing angular"
    

    $scope.InsertAllProducts = function () {
        $scope.loading = true;
        $http.get("/api/dataservices/InsertAllProducts").then(function (response) {
            $scope.response = response.data;
        }).finally(function () {
            // called no matter success or failure
            $scope.loading = false;
        });
    }

    $scope.InsertKoronaStores = function () {
        $scope.loading = true;
        $http.get("/api/dataservices/InsertKoronaStores").then(function (response) {
            $scope.response = response.data;
        }).finally(function () {
            // called no matter success or failure
            $scope.loading = false;
        });
    }

    $scope.InsertKoronaStoresProducts = function () {
        $scope.loading = true;
        $http.get("/api/dataservices/InsertKoronaStoresProducts").then(function (response) {
            $scope.response = response.data;
        }).finally(function () {
            // called no matter success or failure
            $scope.loading = false;
        });
    }

    $scope.cleardiv = function (id) {
        var myEl = angular.element(document.querySelector('#' + id));
        myEl.empty();
    }


});







myApp.controller("myLoginController", function ($scope, $http, $window, $rootScope) {
    $scope.message = "Testing angular";
    $rootScope.islogin = false;
    $scope.loginButton = function () {
      var data = {
            username : $scope.username,
            password : $scope.password
         };
         
      $http.post('/api/dataservices/login', data)
           .then(function (response) {
               $scope.PostDataResponse = response.data;

               var data = response.data;
               var status = response.status;
               var statusText = response.statusText;
               var headers = response.headers;
               var config = response.config;

               if (response.data == 'true') {
                   //$window.location.href = '../insertServices.html';
                   $scope.loginMsg = "";
                   $rootScope.islogin = true
               }
               else {
                   $scope.loginMsg = "Invalid Login";
                   $rootScope.islogin = false;
               }
               //$scope.ResponseDetails = "Data: " + data +
               //  "<hr />status: " + status +
               //  "<hr />headers: " + header +
               //  "<hr />config: " + config;

           }).finally(function () {
               //$scope.islogin = false;
           });
              

    }
});