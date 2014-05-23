function OmnitureTag(eventName)
{
	if (s) { s.pageName = 'EC - ZA - Event - Gumball - ' + eventName; s_code = s.t(); /*console.log(s_code);*/ }
}

function GaPageTrack(pageName)
{
	_gaq.push(['_trackPageview', '/' + eventName]);
}

function GaEventTrack(eventName)
{
	// see https://developers.google.com/analytics/devguides/collection/gajs/eventTrackerGuide
	_trackEvent(category, action, opt_label, opt_value, opt_noninteraction)
}