//'use strict';
angular.module('myServices')
	.service('bundleService', ['$http', '$q', function ($http, $q)
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
							bundleFile.CreationDate = moment(bundleFile.CreationDate).format("hh:mm:ss");
							switch (bundleFile.Type)
							{
								case 0:
									bundleFile.Url = '/Files/csvin/' + bundleFile.FileName; bundleFile.icoType = 'home';
									var csvUrlReduced = bundleFile.Url.substring(1, bundleFile.Url.length).replace('.csv', '');
									//console.log(csvUrlReduced);
									bundleFile.fileApiPath = '/api/bundlefiles/' + encodeURIComponent(csvUrlReduced);
									break;
								case 1:
									bundleFile.Url = '/Files/csvout/' + bundleFile.FileName; bundleFile.icoType = 'external';
									var csvUrlReduced = bundleFile.Url.substring(1, bundleFile.Url.length).replace('.csv', '');
									//console.log(csvUrlReduced);
									bundleFile.fileApiPath = '/api/bundlefiles/' + encodeURIComponent(csvUrlReduced);
									break;
								case 2:
									bundleFile.Url = '/Files/xml/' + bundleFile.FileName; bundleFile.icoType = 'xml';
									bundleFile.fileApiPath = bundleFile.Url;
									break;
							}
							//console.log(bundleFile.Url);

						}
					}
				}
				deferred.resolve(data);
			});
			return deferred.promise;
		};
		var setTreeClickable = function ()
		{
			console.log($('.tree > ul').attr('role', 'tree').find('ul'));
			$('.tree > ul').attr('role', 'tree').find('ul').attr('role', 'group');
			$('.tree').find('li:has(ul)').addClass('parent_li').attr('role', 'treeitem').find(' > span').attr('title', 'Collapse this branch').on('click', function (e)
			{
				var children = $(this).parent('li.parent_li').find(' > ul > li');
				if (children.is(':visible'))
				{
					children.hide('fast');
					$(this).attr('title', 'Expand this branch').find(' > i').addClass('icon-plus-sign').removeClass('icon-minus-sign');
				}
				else
				{
					children.show('fast');
					$(this).attr('title', 'Collapse this branch').find(' > i').addClass('icon-minus-sign').removeClass('icon-plus-sign');
				}
				e.stopPropagation();
			});
		};

		var getBundleFileContent = function (path)
		{
			var deferred = $q.defer();
			$http.get('api/bundlefiles/' + path).success(function (data)
			{
				deferred.resolve(data);
			});
			return deferred.promise;
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
			getBundleFileContent: getBundleFileContent
			//setTreeClickable: setTreeClickable
		};
	}]);