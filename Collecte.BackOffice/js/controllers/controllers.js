'use strict';

angular.module('myControllers')
	.controller('appctrl', ['$scope', '$http', 'bundleService', function ($scope, $http, bundleService)
	{
		$scope.showLoader = true;
		bundleService.getBundles().then(function (data)
		{
			$scope.weeks = data;

			$scope.showLoader = false;
		});
	}]);
