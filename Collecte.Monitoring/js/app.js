//'use strict';

angular.module('myServices', []);
angular.module('bundleControllers', []);
angular.module('myDirectives', []);
angular.module('app', ['myServices', 'bundleControllers', 'myDirectives']);