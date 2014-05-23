//'use strict';

angular.module('myControllers')
	.controller('appCtrl', ['$scope', '$http', '$log', function ($scope, $http,  $log)
	{
		$scope.showLoader = true;
		
		$scope.alert = function()
		{
			$log.log('salut mec !');
		}
		//bundleService.getBundles().then(function (data)
		//{
		//	$scope.weeks = data;

		//	$scope.showLoader = false;
		//});
		//$scope.setContent = function(contentPath)
		//{
		//	console.log(contentPath);
		//	bundleService.getBundleFileContent(contentPath).then(function(data)
		//	{
		//		console.log(data);
		//		$scope.fileContent = data;
		//	});

		//}
	}]);
