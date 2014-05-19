﻿'use strict';

angular.module('myDirectives', [])
	.directive('bundle', [function ()
	{
		return {
			restrict: 'E',
			templateUrl: 'bundle.partial.html',
			scope: {
				bundle: '=which'
			}
		};
	}]);