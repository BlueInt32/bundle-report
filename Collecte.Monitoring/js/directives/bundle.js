//'use strict';

angular.module('monitoringDirectives', ['monitoringController'])
	.directive('bundle', [ function ()
	{
		return {
			restrict: 'E',
			templateUrl: 'bundle.partial.html',
			scope: {
				bundle: '=which'
			}			
		};
	}]);