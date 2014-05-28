//'use strict';

angular.module('bundleControllers')
	.controller('mainCtrl', ['$scope', '$log', 'bundleService', function ($scope, $log, bundleService)
	{
		$scope.showLoader = true;
		
		bundleService.getBundles().then(function (data)
		{
			$scope.weeks = data;
			$scope.showLoader = false;
		});
		$scope.setContent = function(content)
		{
			//console.log(content);
			$scope.fileContent = content;
		}
	}]);
