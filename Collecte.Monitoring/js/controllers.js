//'use strict';

angular.module('bundleControllers')
	.controller('mainCtrl', ['$scope', '$log', 'bundleService', function ($scope, $log, bundleService)
	{
		$scope.showLoaderTree = true;
		//$scope.showLoaderFile = true;
		
		bundleService.getBundles().then(function (data)
		{
			$scope.weeks = data;
			$scope.showLoaderTree = false;
		});
	}]);
