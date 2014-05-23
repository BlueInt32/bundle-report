//'use strict';

angular.module('myDirectives', ['bundleControllers'])
	.directive('bundle', [ function ()
	{
		return {
			restrict: 'E',
			templateUrl: 'bundle.partial.html',
			scope: {
				bundle: '=which'
			}			
		};
	}])
	.directive('previewlink', ['bundleService', function (bundleService)
	{
		return {
			restrict: 'E',
			scope: {
				file: "="
			},
			link: function (scope, element, attrs)
			{
				element.bind('click', function($event)
				{
					console.log($event);
					bundleService.getBundleFileContent(scope.file.fileApiPath).then(function (data)
					{
						bundleService.setFileContentToDom(data);
					});
				});
			},
			template: '<a class="fileLink {{file.icoType}}">{{file.FileName}}</a> | <a href="{{file.Url}}" target="_blank"><img src="img/save.png"/></a>'
		};
	}])
;