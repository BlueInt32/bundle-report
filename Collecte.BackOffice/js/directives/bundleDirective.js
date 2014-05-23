//'use strict';

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
	}])
	//.directive('see', [function()
	//{
	//	return {
	//		restrict: 'A',
	//		scope: {},
	//		link:function(scope, element, attrs) {
	//			element.bind('click', function()
	//			{
	//				console.log($root);
	//				scope.$apply(attrs.see);
	//				//console.log(scope.setContent);
	//			});
	//		}
	//	};
	//}])
;