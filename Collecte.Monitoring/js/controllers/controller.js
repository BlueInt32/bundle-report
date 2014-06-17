//'use strict';

monitoringApp.controller('mainCtrl', ['$scope', '$log', 'bundleService', function ($scope, $log, bundleService)
	{
		$scope.showLoaderTree = true;
		
		bundleService.getBundles().then(function (data)
		{
			$scope.weeks = data;
			$scope.showLoaderTree = false;
		});
	}]);
