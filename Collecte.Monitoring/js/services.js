//'use strict';
angular.module('myServices')
	.service('bundleService', ['$rootScope', '$http', '$q', '$sce', function ($rootScope, $http, $q, $sce)
	{
		// public functions
		var getBundles = function ()
		{
			var deferred = $q.defer();
			$http.get('api/bundles').success(function (data)
			{
				for (var i = 0, l = data.length; i < l; i++)
				{
					var week = data[i];
					for (var j = 0, m = week.Value.length; j < m; j++)
					{
						var bundle = setBundleStatus(week.Value[j]);

						for (var k = 0, n = bundle.BundleFiles.length; k < n; k++)
						{
							var bundleFile = bundle.BundleFiles[k];
							bundleFile.CreationDate = moment(bundleFile.CreationDate).format("HH:mm:ss");
							//var csvUrlReduced = bundleFile.Url/*.replace('.csv', '$csv')*/;
							switch (bundleFile.Type)
							{
								case 0: bundleFile.Url = 'Files/csvin/' + bundleFile.FileName; bundleFile.icoType = 'home'; break;
								case 1: bundleFile.Url = 'Files/csvout/' + bundleFile.FileName; bundleFile.icoType = 'external'; break;
								case 2: bundleFile.Url = 'Files/xml/' + bundleFile.FileName; bundleFile.icoType = 'xml'; break;
							}
							bundleFile.fileApiPath = encodeURIComponent(bundleFile.Url);
							//console.log(bundleFile.Url);

						}
					}
				}
				deferred.resolve(data);
			});
			return deferred.promise;
		};

		var getBundleFileContent = function (path)
		{
			$rootScope.showLoaderFile = true;
			var deferred = $q.defer();
			$http.get('api/bundlefiles/' + path).success(function (data)
			{
				deferred.resolve(data);
			});
			return deferred.promise;
		}

		var setFileContentToDom = function (content)
		{
			content = content.substr(1, content.length - 2);
			$rootScope.showLoaderFile = false;
			$rootScope.fileContent = $sce.trustAsHtml(content);
			
			
		}

		// private functions
		var setBundleStatus = function (bundle)
		{
			bundle.Date = moment(bundle.Date).format("DD/MM/YYYY");
			switch (bundle.Status)
			{
				case 0: // BundleStatus.NoFileCreated
					bundle.displayClass = '';
					bundle.displayStatus = 'Aucun fichier créé';
					break;
				case 1: // BundleStatus.CsvInCreated
					bundle.displayClass = 'badge badge-warning';
					bundle.displayStatus = 'Csv IN créé';
					break;
				case 2: // BundleStatus.CsvInSentToCanal
					bundle.displayClass = 'badge badge-warning';
					bundle.displayStatus = 'CSV IN Envoyé chez Canal';
					break;
				case 3: // BundleStatus.CsvOutReceived
					bundle.displayClass = 'badge badge-important';
					bundle.displayStatus = 'CSV OUT Reçu';
					break;
				case 4: // BundleStatus.CsvOutParsed
					bundle.displayClass = 'badge badge-important';
					bundle.displayStatus = 'CSV OUT Scanné';
					break;
				case 5: // BundleStatus.XmlCreated
					bundle.displayClass = 'badge badge-success';
					bundle.displayStatus = 'XML créé';
					break;
				case 6: // BundleStatus.XmlSentToTrade
					bundle.displayClass = 'badge badge-success';
					bundle.displayStatus = 'XML envoyé';
					break;
				default:
			}
			return bundle;
		};

		return {
			getBundles: getBundles,
			getBundleFileContent: getBundleFileContent,
			setFileContentToDom: setFileContentToDom
		};
	}]);