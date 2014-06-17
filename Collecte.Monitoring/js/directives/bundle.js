//'use strict';

monitoringApp.directive('bundle', [function ()
	{
		return {
			restrict: 'E',
			templateUrl: 'bundle.partial.html',
			scope: {
				bundle: '=which'
			}			
		};
	}]);