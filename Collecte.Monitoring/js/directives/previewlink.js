//'use strict';

monitoringApp.directive('previewlink', ['bundleService', function (bundleService)
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
				bundleService.getBundleFileContent(scope.file.fileApiPath).then(function (data)
				{
					bundleService.setFileContentToDom(data);
				});
			});
		},
		template: '<a class="fileLink {{file.icoType}}" href="#">{{file.FileName}}</a> | <a href="{{file.Url}}" target="_blank"><img src="img/save.png"/></a>'
	};
}]);